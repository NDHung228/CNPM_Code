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
    public class ManagerOrderController : Controller
    {
        // GET: Admin/ManagerOrder

        WebMobileEntities objWebMobileEntities = new WebMobileEntities();
        public ActionResult Index()
        {

            var model = from a in objWebMobileEntities.Orders
                        
                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        where a.Status == 0
                        orderby a.CreateDate
                        select  new CartViewModel()
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
            order.Status = 1;
           
            objWebMobileEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult DeclineOrder(int orderID)
        {
            var order = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == orderID);
            order.Status = -1;
            objWebMobileEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return Redirect("~/Account/Login");
        }

        public ActionResult Edit(int id)
        {
            var order = from a in objWebMobileEntities.Orders
                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        join orderDetail in objWebMobileEntities.OrderDetails on a.ID equals orderDetail.OrderID
                        join product in objWebMobileEntities.Products on orderDetail.ProductID equals product.id
                        where id == a.ID
                        orderby a.CreateDate
                        select new CartViewModel()
                        {
                            order = a,
                            account = b,
                            statusOrder = d
                        };

            var getOrder = order.FirstOrDefault(s => s.order.ID == id);

            // detail each product
            var orders = from a in objWebMobileEntities.Orders
                         join c in objWebMobileEntities.OrderDetails on a.ID equals c.OrderID
                         join b in objWebMobileEntities.Products on c.ProductID equals b.id
                         where c.OrderID == id
                         select new OrderView()
                         {
                             Order = a,
                             orderDetail = c,
                             Product = b,
                         };

            ViewBag.Orderview = orders.ToList();
            return View(getOrder);
        }

        [HttpPost]
        public ActionResult Edit(Order o,String phone,String address,int quantity)
        {
           
            var order = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == o.ID);
            var orderDetail = objWebMobileEntities.OrderDetails.FirstOrDefault(s => s.OrderID == o.ID);
            if (order != null)
            {
                order.Address = address;
                order.Phone = phone;

                orderDetail.Quantity = quantity;

                objWebMobileEntities.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult EditDetailOrder(Order o)
        {
            return RedirectToAction("Edit/" + o.ID);

        }

        public ActionResult DeleteProduct(int ProductID,int OrderID)
        {
            var orderDetail = objWebMobileEntities.OrderDetails.FirstOrDefault(s => s.OrderID == OrderID);
            
            if(orderDetail != null)
            {
                if(ProductID == orderDetail.ProductID)
                {
                    objWebMobileEntities.OrderDetails.Remove(orderDetail);
                    objWebMobileEntities.SaveChanges();
                }
            }
            return RedirectToAction("Edit/" + OrderID);

        }

    }
}