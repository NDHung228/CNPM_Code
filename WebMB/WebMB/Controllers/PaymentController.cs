using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMB.Areas.Admin.Models;
using WebMB.Context;
using WebMB.Models;

namespace WebMB.Controllers
{
    public class PaymentController : Controller
    {
        WebMobileEntities objWebMobileEntities = new WebMobileEntities();
        // GET: Payment
        public ActionResult Index()
        {
            var listOr = objWebMobileEntities.Orders.ToList();

            var model = from a in objWebMobileEntities.Orders
                        join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
                        join b in objWebMobileEntities.Products on c.ProductID equals b.id
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        select new OrderView()
                        {
                            Order = a,
                            orderDetail=c,
                            Product=b,
                            statusOrder = d
                        };
            ViewBag.Orderview=model.ToList();
            
            return View(listOr);
        }
    }
}