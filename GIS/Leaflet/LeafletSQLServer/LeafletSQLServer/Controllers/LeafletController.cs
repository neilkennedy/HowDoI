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
  }
}
