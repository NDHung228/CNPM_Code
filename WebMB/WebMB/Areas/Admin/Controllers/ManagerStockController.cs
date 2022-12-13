using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using WebMB.Areas.Admin.Models;
using WebMB.Context;
using WebMB.Models;

namespace WebMB.Areas.Admin.Controllers
{
    public class ManagerStockController : Controller
    {
        // GET: Admin/ManagerStock

        WebMobileEntities objWebMobileEntities = new WebMobileEntities();

        public ActionResult Index()
        {
            var model = from a in objWebMobileEntities.Orders
                      
                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                     
                        where a.Status == 1
                        orderby a.CreateDate
                        select new CartViewModel()
                        {
                            order = a,
                           
                            account = b,
                            statusOrder = d,
                            
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

            return View(model.ToList());
        }

        public ActionResult Stock()
        {
            var listProduct = objWebMobileEntities.Products.ToList();
            return View(listProduct);
           
        }

        public ActionResult AcceptOrder(int orderID)
        {

            // set status of order
            var order = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == orderID);
            order.Status = 2;
            objWebMobileEntities.SaveChanges();

            // set count product in database
            var orderDetails = objWebMobileEntities.OrderDetails.ToList();
            var product = objWebMobileEntities.Products.ToList();
            
            foreach (var item in orderDetails)
                if(item.OrderID == orderID)
                    foreach(var itemProduct in product)
                        if(item.ProductID == itemProduct.id)
                        {
                            itemProduct.count -= item.Quantity;
                            objWebMobileEntities.SaveChanges();
                        }

            return RedirectToAction("Index");
        }

        public ActionResult DeclineOrder(int orderID)
        {
            var order = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == orderID);
            order.Status = -1;
            objWebMobileEntities.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult ImportProduct(ImportProduct i)
        {
            int productID = i.productID;
            int quantity = i.importQuantity;

            ImportProduct report = new ImportProduct();
            report.productID = productID;
            report.importQuantity = quantity;
            report.createDate = DateTime.Now;
            report.status = 0;
            objWebMobileEntities.ImportProducts.Add(report);
            objWebMobileEntities.SaveChanges();
          
            return View();
        }
        public ActionResult ImportProduct()
        {
            var list = objWebMobileEntities.Products.ToList();
            ViewBag.listProduct = list;
            return View();
        }

        public ActionResult StatusImportProduct()
        {
            var model = from a in objWebMobileEntities.ImportProducts
                        join b in objWebMobileEntities.Products on  a.productID equals b.id
                        select new CartViewModel()
                        {
                           importProduct = a,
                           product = b,

                        };

            return View(model.ToList());
        }

        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return Redirect("~/Account/Login");
        }
    }
}