using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMB.Areas.Admin.Models;
using WebMB.Context;
using WebMB.Models;

namespace WebMB.Areas.Admin.Controllers
{
    public class DeliveryController : Controller
    {
        // GET: Admin/Delivery
        WebMobileEntities objWebMobileEntities = new WebMobileEntities();

        public ActionResult Index()
        {
            var model = from a in objWebMobileEntities.Orders

                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        where a.Status == 2
                        orderby a.CreateDate
                        select new CartViewModel()
                        {
                            order = a,

                            account = b,
                            statusOrder = d
                        };

            var orders = from a in objWebMobileEntities.Orders
                         join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
                         join b in objWebMobileEntities.Products on c.ProductID equals b.id
                         select new OrderView()
                         {
                             Order = a,
                             orderDetail = c,
                             Product = b,
                         };
            ViewBag.Orderview = orders.ToList();

            var model1 =
                        from a in objWebMobileEntities.Orders
                        join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
                        join b in objWebMobileEntities.Products on c.ProductID equals b.id
                        join d in objWebMobileEntities.Accounts on a.Username equals d.username


                        select new CartViewModel()
                        {
                            order = a,
                            orderDetail = c,
                            product = b,
                            account = d,
                        };
            ViewBag.OrderviewAdmin = model1.ToList();
            return View(model.ToList());
        }

        public ActionResult AcceptOrder(int orderID)
        {
            var order = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == orderID);
            order.Status = 3;

            objWebMobileEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeclineOrder(int orderID)
        {
            var order = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == orderID);
            order.Status = -1;

            // set count product return stock
            var orderDetails = objWebMobileEntities.OrderDetails.ToList();
            var product = objWebMobileEntities.Products.ToList();

            foreach (var item in orderDetails)
                if (item.OrderID == orderID)
                    foreach (var itemProduct in product)
                        if (item.ProductID == itemProduct.id)
                        {
                            itemProduct.count += item.Quantity;
                            objWebMobileEntities.SaveChanges();
                        }

            objWebMobileEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return Redirect("~/Account/Login");
        }
    }

}