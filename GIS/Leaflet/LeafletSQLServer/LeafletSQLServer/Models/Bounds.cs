using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafletSQLServer.Models
{
  public class Bounds
  {
    public Coordinates NorthWest { get; set; }
    public Coordinates SouthEast { get; set; }
  }
}