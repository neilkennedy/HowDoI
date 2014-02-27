using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LeafletSQLServer.Controllers
{
    public class PopupController : ApiController
    {
        // GET api/popup
        public string Get(int id)
        {
            return string.Format("The server asked for ID <strong>{0}</strong> on <strong>{1}</strong>", id, DateTime.Now.ToString());
        }
    }
}
