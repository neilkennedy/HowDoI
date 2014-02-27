using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using LeafletSQLServer.Models;

namespace LeafletSQLServer.Workers
{
  public class SpatialDBWorker
  {
    /// <summary>
    /// Uses WKT to define the boundary area and returns all points contained within it
    /// </summary>
    /// <param name="wkt">A Well Known Text string</param>
    /// <param name="reorientObject">Might need to call .ReorientObject() because the Wicket plugin for Leaflet gives us an inverted polygon</param>
    /// <returns></returns>
    public SpatialResult GetPointsFromWKT(string wkt, bool reorientObject)
    {
      var shapeSQL = string.Format("geography::STGeomFromText('{0}', 4326){1};", wkt, reorientObject ? ".ReorientObject()" : "");
      return GetPoints(shapeSQL);
    }

    /// <summary>
    /// NOTE: This seems to be a lot slower than {GetPointsFromWKT}. Must be something to do with the buffer
    /// 
    /// Defines an circle boundary from central Lat/Long and radius (buffer) and returns all points contained within it.
    /// Because of the shape of the earth, the plotted results might not look like a circle when projected on a web map
    /// </summary>
    /// <param name="lat"></param>
    /// <param name="lng"></param>
    /// <param name="radiusInMeters"></param>
    /// <returns></returns>
    public SpatialResult GetPointsFromCircle(double lat, double lng, double radiusInMeters)
    {
      var shapeSQL = string.Format("geography::STGeomFromText('POINT({0} {1})', 4326).STBuffer({2});", lng, lat, radiusInMeters);
      return GetPoints(shapeSQL);
    }

    /// <summary>
    /// Connects to the database and returns all points that are contained within the given boundary
    /// </summary>
    /// <param name="shapeSQL"></param>
    /// <returns></returns>
    public SpatialResult GetPoints(string shapeSQL)
    {
      int amountToReturn = 2000;

      var conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\dev\HowDoI\GIS\Leaflet\LeafletSQLServer\LeafletSQLServer\App_Data\SpatialDB.mdf;Integrated Security=True");
      var dataSet = new DataSet();
      SqlDataAdapter dataAdapter;

      //Get a limited number of shapes (don't care what order) plus the total number available
      var sql = string.Format(@"DECLARE @shape geography = {0}

                  SELECT TOP {1} ID, spatialGeog.STAsText()
                  FROM Points
                  WHERE spatialGeog.STWithin(@shape) = 1;

                  SELECT COUNT(ID) FROM Points WHERE spatialGeog.STWithin(@shape) = 1", shapeSQL, amountToReturn);

      dataAdapter = new SqlDataAdapter(sql, conn);
      dataAdapter.Fill(dataSet);

      int total = (int)dataSet.Tables[1].Rows[0][0];

      var spatialResult = new SpatialResult
      {
        Total = total,
        Items = new List<SpatialItem>()
      };

      foreach (DataRow row in dataSet.Tables[0].Rows)
      {
        spatialResult.Items.Add(new SpatialItem
        {
          ID = (int)row[0],
          WKT = row[1].ToString()
        });
      }

      return spatialResult;
    }
  }
}