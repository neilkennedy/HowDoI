/*jslint browser: true, devel: true, debug: true, plusplus: true */
(function () {
	"use strict";

	var map,
		drawnItems = null,
		eventRegulator = null,
		getPointsRequest = null,
		clusterLayer = null,
		previouslyLoaded = {
			bounds: null,
			moreOnServer: true
		};

	/// <summary>
	/// Shows the message popup and removes it after a set amount of time
	/// </summary>
	/// <param name="text">The text to display in the popup</param>
	function addMessage(text) {
		$('<div id="messages">').html(text).appendTo($('body'));
		setTimeout(function (e) { $('#messages').remove(); }, 4000);
	}

	/// <summary>
	/// Create the map and attach any events we're interested in listening to
	/// </summary>
	function initializeMap() {
		
		map = L.map('map').setView([51.4438971705205, -0.16874313354492188], 5);
		//L.tileLayer('http://otile1.mqcdn.com/tiles/1.0.0/sat/{z}/{x}/{y}.png', {
		L.tileLayer('http://otile1.mqcdn.com/tiles/1.0.0/map/{z}/{x}/{y}.png', {
			attribution: ''
		}).addTo(map);

		clusterLayer = new L.MarkerClusterGroup();
		clusterLayer.addTo(map);

		map.on('zoomend', getPointsFromEvent);
		map.on('moveend', getPointsFromEvent);
	}

	/// <summary>
	/// Adds the {leaflet.draw} control to the map.
	/// Creating a rectangle or polygon will load all the points from the database into that region
	/// </summary>
	function addDrawControl() {
		// Initialize the FeatureGroup to store editable layers
		drawnItems = new L.FeatureGroup();
		map.addLayer(drawnItems);

		// Initialize the draw control and pass it the FeatureGroup of editable layers
		var drawControl = new L.Control.Draw({
			edit: {
				featureGroup: drawnItems
			}
		});
		map.addControl(drawControl);

		map.on('draw:created', function (e) {
			var type = e.layerType,
					layer = e.layer;

			if (type !== 'marker') {
				layer.setStyle({
					color: 'orange',
					fillColor: 'transparent',
					fillOpacity: '0',
					className: 'myVectorClass'
				});
			}

			if (type === 'polygon' || type === 'rectangle') {
				getPointsFromPolygon(layer, true);
			}

			drawnItems.addLayer(layer);
		});
	}

	/// <summary>
	/// Returns an 8 point L.polygon from the map's bounds (viewport).
	/// By default Leaflet wants you to create an L.rectangle from map.getBounds() but this only creates a polygon with 4 points.
	/// This becomes an issue when you try and do spatial queries in SQL Server or another database because when the 4 point polygon is applied
	/// to the curvature of the earth it loses it's "rectangular-ness".
	/// The 8 point polygon returned from this method will keep it's shape a lot more.
	/// </summary>
	/// <param name="map">L.map object</param>
	/// <returns type="">L.Polygon with 8 points starting in the bottom left and finishing in the center left</returns>
	function createViewportPolygon() {
		var center = map.getCenter(),
			bounds = map.getBounds(),
			latlngs = [];

		latlngs.push(bounds.getSouthWest());//bottom left
		latlngs.push({ lat: map.getBounds().getSouth(), lng: center.lng });//bottom center
		latlngs.push(bounds.getSouthEast());//bottom right
		latlngs.push({ lat: center.lat, lng: map.getBounds().getEast() });// center right
		latlngs.push(bounds.getNorthEast());//top right
		latlngs.push({ lat: map.getBounds().getNorth(), lng: map.getCenter().lng });//top center
		latlngs.push(bounds.getNorthWest());//top left
		latlngs.push({ lat: map.getCenter().lat, lng: map.getBounds().getWest() });//center left

		return new L.polygon(latlngs);
	}

	/// <summary>
	/// The event listener that gets called to load up some points from the map viewport. Uses {getPointsFromCircle}
	/// This uses setInterval to cut down on the event noise as the user pans and zooms around the map.
	/// There must be a 1 second interval between event calls before the AJAX call is fired.
	/// </summary>
	/// <param name="e">Event object</param>
	function getPointsFromEvent(e) {
		var bounds = map.getBounds(),
			line,
			circle;

		//we don't want to go to the server every time an event is raised
		//because the user might just be panning / zooming multiple times around the map
		//so we wait until there is a 1 second gap between event calls and then we go get some points!
		if (eventRegulator) { clearTimeout(eventRegulator); }

		/*
			Only go to the server to retrive new points if
			- this is first map load (previouslyLoaded.bounds will be null)
			- we panned away from the current area
			- we've just zoomed in but there are more points available on the server
		*/
		if (!previouslyLoaded.bounds || !previouslyLoaded.bounds.contains(bounds) || previouslyLoaded.moreOnServer) {

			eventRegulator = setTimeout(function () {
				eventRegulator = null;

				line = L.polyline([bounds.getSouthWest(), bounds.getNorthEast()]);
				circle = L.circle(map.getCenter(), (L.GeometryUtil.length(line) / 2));

				previouslyLoaded.bounds = bounds;

				getPointsFromPolygon(createViewportPolygon(), false);
			}, 1000);
		}
	}

	/// <summary>
	/// Use an {L.latLngBounds} to create a circle and pass it to {getPointsFromServer}
	/// </summary>
	/// <param name="bounds"></param>
	function getPointsFromCircle(bounds) {
		var line = L.polyline([bounds.getSouthWest(), bounds.getNorthEast()]),
			radius = L.GeometryUtil.length(line) / 2;

		getPointsFromServer({ lat: bounds.getCenter().lat, lng: bounds.getCenter().lng, radiusInMeters: radius });
	}

	/// <summary>
	/// Convert an {L.polygon} into WKT and pass it to {getPointsFromServer}
	/// </summary>
	/// <param name="polygon">L.polygon</param>
	/// <param name="reorientObject">Some generated polygons appear the wrong way around in SQL Server so we must invert them first (left foot inside rule)</param>
	function getPointsFromPolygon(polygon, reorientObject) {
		var wkt = new Wkt.Wkt().fromObject(polygon).write();
		getPointsFromServer({ wkt: wkt, reorientObject: reorientObject });
	}

	/// <summary>
	/// The AJAX method to go to the server, grab the points that are contained with the shape and display them on the map
	/// </summary>
	/// <param name="requestData">The data object to be sent to the server</param>
	function getPointsFromServer(requestData) {
		
		//abort any currently running requests before we send off this one
		if (getPointsRequest) { getPointsRequest.abort(); }

		$('#loading').show();

		getPointsRequest = $.get("/api/points", requestData, function (data) {

			previouslyLoaded.moreOnServer = data.MoreOnServer;

			if (data.Returned !== 0) {
				var wkt = new Wkt.Wkt(),
					point;

				clusterLayer.clearLayers();

				$.each(data.Items, function (index, item) {
					wkt.read(item.WKT);
					point = wkt.toObject();
					point.bindPopup('##' + item.ID);
					point.on('popupopen', popupOpen);

					clusterLayer.addLayer(point);
				});

				addMessage('Showing ' + data.Returned + ' of ' + data.Total);
			} else {
				addMessage('No points for this area');
			}
		}).fail(function (err) {
			if (err.statusText !== "abort") {
				addMessage('Request failed. Try again.');
			}
		}).always(function () {
			getPointsRequest = null;
			$('#loading').hide();
		});
	}

	/// <summary>
	/// When a popup is clicked, fetch the information to display
	/// </summary>
	/// <param name="event"></param>
	function popupOpen(event) {
		var popup = event.popup,
			content = popup.getContent();

		if (content.indexOf('##') === 0) {
		  popup.setContent('Loading...');
			$.get('/api/popup', { id: content.replace('##','') }, function (data) {
				popup.setContent(data);
			});
		}
	}

	$(document).ready(function () {
		initializeMap();
		addDrawControl();

		getPointsFromEvent({ target: map });
	});

}());