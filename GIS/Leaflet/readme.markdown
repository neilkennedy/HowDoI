#Leaflet.js

Website: http://leafletjs.com/

###BasicPlay
This is a very simple HTML/CSS/JS project that I used to evaluate the Leaflet API.

* Plotting Markers
* Using the clustering plugin
* Using the wicket plugin to convert to / from Well Known Text (WKT)
* Using the Leaflet.Draw plugin to draw shapes

###LeafletSQLServer
This is my main leaflet project. It is an ASP.NET MVC project with leaflet.js and SQL Server to prototype clustering large amounts of server side data.

* Fetches points from SQL Server that are contained within the current viewport of the map (map.getBounds())
* Uses the leaflet clustering plugin to cluster the returned points client side
* Fetches new points when the map is zoomed or panned.
* Intelligent fetching
    * Only when we know there are more points on the server
    * Only after 1 second of event inactivity
* Currently there is no caching of points (but might be something added in later)
* SQL query is pretty rough (separate query to get the total count) and needs to be optimised. 
    * But I have included a spatial index on the DB geography column.