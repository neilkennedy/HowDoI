using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoCodeTests.Models.TomTom
{
  public class Locations
  {
    public IEnumerable<Location> location { get; set; }

    public Locations()
    {
      location = new List<Location>();
    }
  }
}