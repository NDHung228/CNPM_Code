using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using WebMB.Context;
using WebMB.Models;

namespace WebMB.Controllers
{
    public class CartController : Controller
    {
        WebMobileEntities objWebMobileEntities = new WebMobileEntities();
        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }

            else
            {
                var cart = Session[CartSession];
                var list = new List<CartModel>();
                ViewBag.Cart = Total();
                if (cart != null)
                {
                    list = (List<CartModel>)cart;
                }
                return View(list) ;
            }


        }

        public ActionResult AddItem(int productID, int quantity)
        {
            if (Session["UserName"] == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }

            var product = objWebMobileEntities.Products.FirstOrDefault(s => s.id == productID);

            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartModel>)cart;
                if (list.Exists(x => x.Product.id == productID))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.id == productID)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    // create new object
                    var item = new CartModel();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }

                Session[CartSession] = list;

            }
         else
            {
                // create new object
              var item = new CartModel();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartModel>();
                list.Add(item);

                // assign to session
                Session[CartSession] = list;
            }


            return RedirectToAction("Index");



        }
        public ActionResult UpdateCart(FormCollection form)
        {
            var cart = Session[CartSession];
            var list = (List<CartModel>)cart;

            int id = int.Parse(form["id"]);
            int quan = int.Parse(form["quantity"]);
                foreach (var item in list)
                {
                    if (item.Product.id == id)
                    {
                        item.Quantity = quan;
                    }
                }
                Session[CartSession] = list;
            return RedirectToAction("Index");

        }
        public ActionResult DeleteCart(int ProductID)
        {
            var cart = Session[CartSession];
            var list = (List<CartModel>)cart;
            foreach(var item in list)
            {
                if(item.Product.id == ProductID)
                {
                    list.Remove(item);
                    break;
                }
            }
            Session[CartSession] = list;
            return RedirectToAction("Index");

        }
        public double Total()
        {
            var cart = Session[CartSession];
            var list = (List<CartModel>)cart;
            double total = 0;  
            if(list==null)
            {
                total = 0;
                return total;

            }
         
            foreach (var item in list)
            {
                double quan=item.Quantity;
                var price = item.Product.price.GetValueOrDefault();
                total = total + quan * price;
                
            }

            return total;
        }

        public ActionResult AddOrderToDatabase()
        {
            if (Session["UserName"] == null)
            {
                return View("~/Views/Account/Login.cshtml");
            }

            var cart = Session[CartSession];
            var list = new List<CartModel>();

            var listAccount = objWebMobileEntities.Accounts.ToList();
            
            list = (List<CartModel>)cart;
            Order objOrder = new Order();
            objOrder.Name = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            objOrder.CreateDate = DateTime.Now;
            objOrder.Status = 0;
            objOrder.Username = Session["UserName"].ToString();

            foreach (var item in listAccount)
            {
                if (item.username.Equals(objOrder.Username))
                {
                    objOrder.Address = item.address2.ToString();
                    objOrder.Phone = item.phone2.ToString();
                    break;
                }
            }

            objWebMobileEntities.Orders.Add(objOrder);
            objWebMobileEntities.SaveChanges();



            int orderID = objOrder.ID;
            List<OrderDetail> listOrderDetail = new List<OrderDetail>();

            // calculate total order
            float totalOrder = 0.0f;

            foreach (var item in list)
            {
                OrderDetail orDetail = new OrderDetail();
                orDetail.Quantity = item.Quantity;
                orDetail.OrderID = orderID;
                orDetail.ProductID = item.Product.id;
                orDetail.Price = (decimal?)item.Product.price * item.Quantity;
                totalOrder += (float)orDetail.Price;

                listOrderDetail.Add(orDetail);
            }
            Session[CartSession] = null;

            objWebMobileEntities.OrderDetails.AddRange(listOrderDetail);

            var setTotalOrder = objWebMobileEntities.Orders.FirstOrDefault(s => s.ID == orderID);
            setTotalOrder.Total = totalOrder;   // update total price of order in database 
            // save data in order and detail order of database

            objWebMobileEntities.SaveChanges();
            
            return RedirectToAction("Index", "Payment"); ;
        }
        public ActionResult Check()
        {
            var list = objWebMobileEntities.Accounts.ToList();
            string a = Session["UserName"].ToString();
            foreach (var item in list)
            {
                if(item.username.Equals(a))
                {
                    ViewBag.check = item.phone.ToString();
                    ViewBag.check2 = item.address.ToString();
                    break;
                }
            }
            return View();
        }

        public ActionResult ConfirmCheck(FormCollection form)
        {
            string phone = form["inputphone"];
            string address = form["inputadd"];
            string name = Session["UserName"].ToString();
            var list = objWebMobileEntities.Accounts.ToList();
            foreach (var item in list)
            {
                if (item.username.Equals(name))
                {

                    item.phone2 = phone;
                    item.address2 = address;

                    break;
                }
            }
            objWebMobileEntities.SaveChanges();


            return RedirectToAction("AddOrderToDatabase");
        }





    }

}