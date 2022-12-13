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
    public class HomeAdminController : Controller
    {
        WebMobileEntities objWebMobileEntities = new WebMobileEntities();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var model = from a in objWebMobileEntities.Orders

                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        where a.Status == 0
                        orderby a.CreateDate
                        select new CartViewModel()
                        {
                            order = a,

                            account = b,
                            statusOrder = d
                        };
            return View(model.ToList());
        }

        public ActionResult SuccessOrder()
        {
            var model = from a in objWebMobileEntities.Orders
                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        where a.Status == 3
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

        public ActionResult CancelOrder()
        {
            var model = from a in objWebMobileEntities.Orders
                        join b in objWebMobileEntities.Accounts on a.Username equals b.username
                        join d in objWebMobileEntities.StatusOrders on a.Status equals d.statusID
                        where a.Status == -1
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

        public ActionResult ConfirmImportProduct()
        {
            var model = from a in objWebMobileEntities.ImportProducts
                        join b in objWebMobileEntities.Products on a.productID equals b.id
                        select new CartViewModel()
                        {
                            importProduct = a,
                            product = b

                        };

            return View(model.ToList());
        }

        public ActionResult AcceptImport(int importID)
        {
            var importProduct = objWebMobileEntities.ImportProducts.FirstOrDefault(s => s.importID == importID);
            importProduct.status = 1;
            importProduct.confirmDate = DateTime.Now;

            var product = objWebMobileEntities.Products.FirstOrDefault(s => s.id == importProduct.productID);
            product.count = product.count + importProduct.importQuantity;
            objWebMobileEntities.SaveChanges();

            return RedirectToAction("ConfirmImportProduct");
        }

        public ActionResult DeclineImport(int importID)
        {
            var importProduct = objWebMobileEntities.ImportProducts.FirstOrDefault(s => s.importID == importID);
            importProduct.status = -1;
            importProduct.confirmDate = DateTime.Now;
            objWebMobileEntities.SaveChanges();

            return RedirectToAction("ConfirmImportProduct");
        }

        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return Redirect("~/Account/Login");
        }

        public ActionResult AddProduct()
        {
            return View();
        }

     
        [HttpPost]
        public ActionResult AddProduct(Product product)
        {
            String name = product.name;
            String img = product.img;
            String descript = product.description;
            double price = (double) product.price;
            int count = (int)product.count;
            
    
            objWebMobileEntities.Products.Add(product);
            objWebMobileEntities.SaveChanges();
            return Redirect("/Admin/ManagerStock/Stock");
        }
    }
    }