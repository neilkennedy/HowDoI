using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoCodeTests.Models.MapQuest
{
  public class MapQuestLocation
  {
    public string street { get; set; }
    public string city { get; set; }
    public string county { get; set; }
    public string state { get; set; }
    public string postalCode { get; set; }
    public string country { get; set; }
  }
}