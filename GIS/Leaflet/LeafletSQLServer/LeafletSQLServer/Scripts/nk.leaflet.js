(function () {

  var map,
    drawnItems = null,
    eventRegulator = null,
    getPointsRequest = null,
    clusterLayer = null;

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
    var bounds = e.target.getBounds(),
      rectangle = L.rectangle(bounds);

    //we don't want to go to the server every time an event is raised
    //because the user might just be panning / zooming multiple times around the map
    //so we wait until there is a 1 second gap between event calls and then we go get some points!
    if (eventRegulator) { clearTimeout(eventRegulator); }

    eventRegulator = setTimeout(function () {
      eventRegulator = null;
      getPoints(rectangle);//get the bounds of the map
    }, 1000);
  }

  //go to the server to return points contained within the shape
  function getPoints(shape) {

    console.log('getting points');

    var wellKnownText = new Wkt.Wkt().fromObject(shape).write();

    $('#loading').show();

    if (getPointsRequest) { getPointsRequest.abort(); }

    getPointsRequest = $.post("/Leaflet/Points", "wkt=" + wellKnownText, function (data) {

      getPointsRequest = null;
      $('#loading').hide();

      if (data.Returned !== 0) {
        var wkt = new Wkt.Wkt();
        wkt.read(data.WKT);

        if (!clusterLayer) {
          clusterLayer = new L.MarkerClusterGroup();
          clusterLayer.addTo(map);
        }

        clusterLayer.clearLayers();
        clusterLayer.addLayer(wkt.toObject());

        addMessage('Showing ' + data.Returned + ' of ' + data.Total);
      } else {
        addMessage('No points for this area');
      }
    });
  }


  $(document).ready(function () {
    initializeMap();
    addDrawControl();
  });

}());