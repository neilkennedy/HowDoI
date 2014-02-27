using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using LeafletSQLServer.Models;
using LeafletSQLServer.Workers;

namespace LeafletSQLServer.Controllers
{
    public class PointsController : ApiController
    {

      public SpatialResult Get(string wkt, bool reorientObject)
      {
        if (ModelState.IsValid)
        {
          var multiPoints = new SpatialDBWorker().GetPointsFromWKT(wkt, reorientObject);
          return multiPoints;
        }
        else
        {
          throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
      }

      public SpatialResult Get(double lat, double lng, double radiusInMeters)
      {
        if (ModelState.IsValid)
        {
          var multiPoints = new SpatialDBWorker().GetPointsFromCircle(lat, lng, radiusInMeters);
          return multiPoints;
        }
        else
        {
          throw new HttpResponseException(HttpStatusCode.BadRequest);
        }
      }
    }
}
