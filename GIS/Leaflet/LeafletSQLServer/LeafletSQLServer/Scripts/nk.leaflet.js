(function () {

  var map,
    drawnItems = null,
    eventRegulator = null,
    getPointsRequest = null,
    clusterLayer = null,
    previouslyLoaded = {
      bounds: null,
      moreOnServer: true,
      zoomLevel: 0
    };

  /**
      Show's the message popup and removes it after a set amount of time
  */
  function addMessage(text) {
    $('<div id="messages">').html(text).appendTo($('body'));
    setTimeout(function (e) { $('#messages').remove(); }, 4000);
  }

  /**
      Create the map and attach any events we're interested in listening to
  */
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

  /**
      Adds the {leaflet.draw} control to the map.
      Creating a rectangle or polygon will load all the points from the database into that region
  */
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
        getPointsFromRectangle(layer);
      }

      drawnItems.addLayer(layer);
    });
  }

  /**
      The event listener that gets called to load up some points from the map viewport. Uses {getPointsFromCircle}
      This uses setInterval to cut down on the event noise as the user pans and zooms around the map.
      There must be a 1 second interval between event calls before the AJAX call is fired.
  */
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
        previouslyLoaded.zoomLevel = map.getZoom();

        getPointsFromCircle(bounds);
      }, 1000);
    }
  }

  /**
      Use an {L.latLngBounds} to create a circle and pass it to {getPointsFromServer}
  */
  function getPointsFromCircle(bounds) {
    var line = L.polyline([bounds.getSouthWest(), bounds.getNorthEast()]),
      radius = L.GeometryUtil.length(line) / 2;

    getPointsFromServer("/Leaflet/PointsFromCircle", { lat: bounds.getCenter().lat, lng: bounds.getCenter().lng, radiusInMeters: radius });
  }

  /**
      Convert an {L.rectangle} into WKT and pass it to {getPointsFromServer}
  */
  function getPointsFromRectangle(rectangle) {
    var wkt = new Wkt.Wkt().fromObject(rectangle).write();

    getPointsFromServer("/Leaflet/PointsFromWKT", { wkt: wkt });
  }

  /** 
      The AJAX method to go to the server, grab the points that are contained with the shape and display them on the map
  */
  function getPointsFromServer(endPoint, requestData) {

    //abort any currently running requests before we send off this one
    if (getPointsRequest) { getPointsRequest.abort(); }

    $('#loading').show();

    getPointsRequest = $.post(endPoint, requestData, function (data) {

      previouslyLoaded.moreOnServer = data.MoreOnServer;

      if (data.Returned !== 0) {
        var wkt = new Wkt.Wkt();
        wkt.read(data.WKT);

        clusterLayer.clearLayers();
        clusterLayer.addLayer(wkt.toObject());

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

  $(document).ready(function () {
    initializeMap();
    addDrawControl();

    getPointsFromEvent({ target: map });
  });

}());