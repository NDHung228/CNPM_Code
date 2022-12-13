using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMB.Context;


    namespace WebMB.Models
    {
        public class OrderView
        {
            public Order Order { get; set; }

            public Product Product { get; set; }
            public OrderDetail orderDetail { get; set; }
            
            public StatusOrder statusOrder { get; set; }

        }
    }


