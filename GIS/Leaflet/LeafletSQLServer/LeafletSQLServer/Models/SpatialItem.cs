using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafletSQLServer.Models
{
  public class SpatialItem
  {
    public int ID { get; set; }
    public string WKT { get; set; }
  }
}