using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoCodeTests.Models.TomTom
{
  public class TomTomRequest
  {
    public Locations locations { get; set; }

    public TomTomRequest()
    {
      locations = new Locations();
    }
  }
}