using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoCodeTests.Models
{
  public class StandardLocation
  {
    public string ID { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string County { get; set; }
    public string State { get; set; }
    public string PostCode { get; set; }
    public string CountryISO { get; set; }
    public string Country { get; set; }

    public string Accuracy { get; set; }
    public string Confidence { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
  }
}