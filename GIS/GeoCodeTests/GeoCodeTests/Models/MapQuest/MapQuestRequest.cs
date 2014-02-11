using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoCodeTests.Models.MapQuest
{
  public class MapQuestRequest
  {
    public List<MapQuestLocation> locations { get; set; }

    public MapQuestRequest()
    {
      locations = new List<MapQuestLocation>();
    }
  }
}