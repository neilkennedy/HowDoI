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
    public WKTStatus GetPoints(string wkt)
    {
      int amountToReturn = 5000;

      var conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\dev\HowDoI\GIS\Leaflet\LeafletSQLServer\LeafletSQLServer\App_Data\SpatialDB.mdf;Integrated Security=True");
      var dataSet = new DataSet();
      SqlDataAdapter dataAdapter;

      //need to call .ReorientObject() because the Wicket plugin for Leaflet gives us an inverted polygon
      var sql = @"DECLARE @shape geography = geography::STGeomFromText('" + wkt + @"', 4326).ReorientObject();

                  SELECT COUNT(p1.ID), geography:: UnionAggregate(p1.spatialGeog).STAsText()
                  FROM Points p1
                  WHERE p1.ID in (SELECT TOP " + amountToReturn + @" p2.ID 
                                  FROM Points p2 
                                  WHERE p2.spatialGeog.STWithin(@shape) = 1);

                  SELECT COUNT(ID) FROM Points WHERE spatialGeog.STWithin(@shape) = 1";

      dataAdapter = new SqlDataAdapter(sql, conn);
      dataAdapter.Fill(dataSet);

      int total = (int)dataSet.Tables[1].Rows[0][0];

      var wktStatus = new WKTStatus
      {
        Total = total,
        Returned = (int)dataSet.Tables[0].Rows[0][0],
        WKT = dataSet.Tables[0].Rows[0][1].ToString()
      };

      return wktStatus;
    }
  }
}