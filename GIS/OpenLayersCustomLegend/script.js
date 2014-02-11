(function (OpenLayers, document) {
    'use strict';

    //http://youmightnotneedjquery.com/
    function parseHTML(str) {
        var tmp = document.implementation.createHTMLDocument();
        tmp.body.innerHTML = str;
        return tmp.body.children;
    }

    function getLegendMarkup(control) {
        return parseHTML(document.getElementById('legendTemplate').innerHTML)[0];
    }

    document.addEventListener('DOMContentLoaded', function () {

        var map, baseLayer, customLegend, tempControl;

        map = new OpenLayers.Map('map');
        baseLayer = new OpenLayers.Layer.OSM("osm", ["http://otile1.mqcdn.com/tiles/1.0.0/map/${z}/${x}/${y}.jpg",
                "http://otile2.mqcdn.com/tiles/1.0.0/map/${z}/${x}/${y}.jpg",
                "http://otile3.mqcdn.com/tiles/1.0.0/map/${z}/${x}/${y}.jpg",
                "http://otile4.mqcdn.com/tiles/1.0.0/map/${z}/${x}/${y}.jpg"
            ], {
            attribution: "© <a href=\"http://www.openstreetmap.org/copyright\">OpenStreetMap</a> contributors. " +
                "Tiles Courtesy of <a href=\"http://www.mapquest.com/\" target=\"_blank\">MapQuest</a> <img src=\"http://developer.mapquest.com/content/osm/mq_logo.png\">"
        });
        map.addLayer(baseLayer);

        customLegend = new OpenLayers.Control.Panel();
        customLegend.createControlMarkup = function (control) {
            return getLegendMarkup(control);
        };

        tempControl = new OpenLayers.Control.Button({ //no idea why this has to be added to the panel but it does
            title: "Title",
            id: "divToggle"
        });
        customLegend.addControls(tempControl);

        map.addControl(customLegend, new OpenLayers.Pixel(8, 70));

        map.zoomToMaxExtent();

    });

}(OpenLayers, document));