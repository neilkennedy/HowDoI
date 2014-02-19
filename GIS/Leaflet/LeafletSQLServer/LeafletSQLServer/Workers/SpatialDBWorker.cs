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
    public string GetPoints(string wkt)
    {
      var sql = @"DECLARE @shape geography = geography::STGeomFromText('" + wkt + @"', 4326);
                  SELECT geography:: UnionAggregate(spatialGeog).STAsText()
                  FROM Points 
                  WHERE @shape.STContains(spatialGeog) = 0;";

      var conn = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\dev\HowDoI\GIS\Leaflet\LeafletSQLServer\LeafletSQLServer\App_Data\SpatialDB.mdf;Integrated Security=True");
      var dataSet = new DataSet();

      var dataAdapter = new SqlDataAdapter(sql, conn);

      dataAdapter.Fill(dataSet);

      return dataSet.Tables[0].Rows[0][0].ToString();//WKT string
    }
  }
}