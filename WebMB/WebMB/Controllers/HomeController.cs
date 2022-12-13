using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMB.Areas.Admin.Models;
using WebMB.Context;

namespace WebMB.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        WebMobileEntities objWebMobileEntities = new WebMobileEntities();
        public ActionResult Index()
        {
            
            var listProduct = objWebMobileEntities.Products.ToList();
            return View(listProduct);
        }

        
        public ActionResult SearchProduct(string searchString)
        {
            //string searchName = "%" + searchString + "%";
            //string query = "select * from product as p where p.name like " + searchName;
            var listProduct = objWebMobileEntities.Products.ToList();

            //IEnumerable<Product> data = objWebMobileEntities.Database.SqlQuery<Product>(query);
            List<Product> list = new List<Product>();
            foreach(var item in listProduct)
            {
                if(item.name.Contains(searchString))
                {
                    list.Add(item);
                }
            }

            ViewBag.Drum = null;
            if (list.Count == 0)
            {
                ViewBag.Drum = "Không tìm thấy sản phẩm";
            }
            return View(list);
        }


        

        public ActionResult Detail(int productID)
        {
            var product = objWebMobileEntities.Products.FirstOrDefault(s => s.id == productID);
            return View(product);
        }

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
    }
}