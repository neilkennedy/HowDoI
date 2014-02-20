using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafletSQLServer.Models
{
  public class WKTStatus
  {
    public int Total { get; set; }
    public int Returned { get; set; }
    public string WKT { get; set; }
    public Boolean MoreOnServer
    {
      get
      {
        return Total > Returned;
      }
    }
  }
}