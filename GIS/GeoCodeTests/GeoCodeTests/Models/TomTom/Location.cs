using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoCodeTests.Models.TomTom
{
  public class Location
  {
    /// <summary>
    /// Custom ID to identify the result
    /// </summary>
    public string userTag { get; set; }
    /// <summary>
    /// House number
    /// </summary>
    public string ST { get; set; }
    /// <summary>
    /// Street name
    /// </summary>
    public string T { get; set; }
    /// <summary>
    /// District
    /// </summary>
    public string SL { get; set; }
    /// <summary>
    /// Town or City
    /// </summary>
    public string L { get; set; }
    /// <summary>
    /// State / Province / Region
    /// </summary>
    public string AA { get; set; }
    /// <summary>
    /// Postal Code
    /// </summary>
    public string PC { get; set; }
    /// <summary>
    /// Country Name
    /// </summary>
    public string CN { get; set; }

    public string Score { get; set; }
    public string Confidence { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string json { get; set; }
  }
}