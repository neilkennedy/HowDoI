using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LeafletSQLServer.Models;
using LeafletSQLServer.Workers;
using Newtonsoft.Json;

namespace LeafletSQLServer.Controllers
{
  public class LeafletController : Controller
  {
    public ActionResult Index()
    {
      return View();
    }

    public JsonResult PointsFromWKT(string wkt)
    {
      var multiPoints = new SpatialDBWorker().GetPointsFromWKT(wkt);
      return new JsonResult { Data = multiPoints };
    }

    public JsonResult PointsFromCircle(double lat, double lng, double radiusInMeters)
    {
      var multiPoints = new SpatialDBWorker().GetPointsFromCircle(lat, lng, radiusInMeters);
      return new JsonResult { Data = multiPoints };
    }
  }
}
