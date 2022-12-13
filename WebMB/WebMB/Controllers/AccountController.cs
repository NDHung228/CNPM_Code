using System;
using System.Linq;
using System.Web.Mvc;
using WebMB.Context;

namespace WebMB.Controllers
{
    public class AccountController : Controller
    {
        WebMobileEntities objWebMobileEntities = new WebMobileEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account _user)
        {
            if (ModelState.IsValid)
            {
                var check = objWebMobileEntities.Accounts.FirstOrDefault(s => s.username == _user.username);

                if (check == null)
                {
                    objWebMobileEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebMobileEntities.Accounts.Add(_user);
                    objWebMobileEntities.SaveChanges();
                    ViewBag.SuccessMessage = "Register success";
                    return RedirectToAction("Register");
                }

                else
                {
                    ViewBag.Error = "username already exists";
                    return View();
                }

            }
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(String username, string password)
        {
            if (ModelState.IsValid)
            {
                var check = objWebMobileEntities.Accounts.FirstOrDefault(s => s.username == username && s.password == password);
                if (check != null)
                {
                    Session["UserName"] = check.username;

                    // login with admin
                    if (username == "AdminStock")
                        return Redirect("/Admin/ManagerStock/Index");

                    if (username == "AdminOrder")
                        return Redirect("/Admin/ManagerOrder/Index");

                    if (username == "Admin")
                        return Redirect("/Admin/HomeAdmin/SuccessOrder");

                    if (username == "Shipper")
                        return Redirect("/Admin/Delivery/Index");

                    //add session of user

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }


            }
            return View("Login");
        }
        public ActionResult LogOut()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index");
        }
    }
}