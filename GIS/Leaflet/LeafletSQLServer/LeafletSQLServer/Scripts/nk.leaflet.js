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

  function addMessage(text) {
    $('<div id="messages">').html(text).appendTo($('body'));

    setTimeout(function (e) { $('#messages').remove(); }, 4000);
  }

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
        getPoints(layer);
      }

      drawnItems.addLayer(layer);
    });
  }

  //The event listener that gets called to load up some points from the map viewport
  function getPointsFromEvent(e) {
    var map = e.target,
      bounds = map.getBounds();

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

        bounds = bounds.pad(1);//increase the size of the bounds by a percentage

        previouslyLoaded.bounds = bounds;
        previouslyLoaded.zoomLevel = map.getZoom();

        getPoints(L.rectangle(bounds));
      }, 1000);
    }
  }

  //go to the server to return points contained within the shape
  function getPoints(shape) {

    var wellKnownText = new Wkt.Wkt().fromObject(shape).write();

    $('#loading').show();

    if (getPointsRequest) { getPointsRequest.abort(); }

    getPointsRequest = $.post("/Leaflet/Points", "wkt=" + wellKnownText, function (data) {

      previouslyLoaded.moreOnServer = data.MoreOnServer;

      if (data.Returned !== 0) {
        var wkt = new Wkt.Wkt();
        wkt.read(data.WKT);

        clusterLayer.addLayer(wkt.toObject());

        addMessage('Showing ' + data.Returned + ' of ' + data.Total);
      } else {
        addMessage('No points for this area');
      }
    }).fail(function (err) {

      addMessage('Request failed. Try again.');

    }).always(function () {

      getPointsRequest = null;
      $('#loading').hide();
      clusterLayer.clearLayers();

    });
  }

  $(document).ready(function () {
    initializeMap();
    addDrawControl();

    getPointsFromEvent({ target: map });
  });

}());