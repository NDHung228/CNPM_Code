using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMB.Context;

namespace WebMB.Areas.Admin.Models
{
    public class CartViewModel
    {
        public Order order { get; set; }
        public OrderDetail orderDetail { get; set; }
        public Account account { get; set; }
        
        public StatusOrder statusOrder { get; set; }
        public Product product { get; set; }

        public ImportProduct importProduct { get; set; }

        

    }
}