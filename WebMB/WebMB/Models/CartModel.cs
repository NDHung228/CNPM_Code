using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMB.Context;

namespace WebMB.Models
{
    [Serializable]
    public class CartModel
    {
        
      public Product Product { get; set; }
      public int Quantity { get; set; }

    }
}