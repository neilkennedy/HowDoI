(function () {

  var map,
    drawnItems = null;

  function initializeMap() {
    map = L.map('map').setView([51.4438971705205, -0.16874313354492188], 5);
    //L.tileLayer('http://otile1.mqcdn.com/tiles/1.0.0/sat/{z}/{x}/{y}.png', {
    L.tileLayer('http://otile1.mqcdn.com/tiles/1.0.0/map/{z}/{x}/{y}.png', {
      attribution: ''
    }).addTo(map);
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

  function getPoints(shape) {

    var wellKnownText = new Wkt.Wkt().fromObject(shape).write();

    $.post("/Leaflet/Points", "wkt=" + wellKnownText, function (data) {
      if (data !== "") {
        var wkt = new Wkt.Wkt(),
          cluster = new L.MarkerClusterGroup();

        wkt.read(data);
        cluster.addLayer(wkt.toObject());
        cluster.addTo(map);
      } else {
        alert('No points for this area');
      }
    });
  }


  $(document).ready(function () {
    initializeMap();
    addDrawControl();
  });

}());