using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafletSQLServer.Models
{
  public class Popup
  {
    public string Title { get; set; }
    public List<Dictionary<string, string>> Rows { get; set; }
  }
}