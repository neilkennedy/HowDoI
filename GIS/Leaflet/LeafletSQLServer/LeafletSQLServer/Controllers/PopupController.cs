using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LeafletSQLServer.Models;

namespace LeafletSQLServer.Controllers
{
  public class PopupController : ApiController
  {
    // GET api/popup
    public Popup Get(int id)
    {
      var popup = new Popup
      {
        Title = string.Format("Generated at {0}", DateTime.Now.ToString()),
        Rows = new List<Dictionary<string,string>>()
      };

      popup.Rows.Add(new Dictionary<string, string>()
      {
        {"item1", "Something"},
        {"item2", "Something"},
        {"item3", "Something"},
        {"item4", "Something"}
      });

      popup.Rows.Add(new Dictionary<string, string>()
      {
        {"item1", "Something else"},
        {"item2", "Something else"},
        {"item3", "Something else"},
        {"item4", "Something else"}
      });

      popup.Rows.Add(new Dictionary<string, string>()
      {
        {"item1", "Something or other"},
        {"item2", "Something or other"},
        {"item3", "Something or other"},
        {"item4", "Something or other"}
      });

      return popup;
      
    }
  }
}
