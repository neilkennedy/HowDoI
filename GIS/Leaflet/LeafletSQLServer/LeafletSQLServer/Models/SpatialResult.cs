using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeafletSQLServer.Models
{
  public class SpatialResult
  {
    public int Total { get; set; }
    public List<SpatialItem> Items { get; set; }
    public int Returned
    {
      get
      {
        return Items != null ? Items.Count : 0;
      }
    }
    public Boolean MoreOnServer
    {
      get
      {
        return Total > Returned;
      }
    }
  }
}