using ASPEx_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class LogOffTestController : Controller
    {
        // GET: LogOff
        public ActionResult Index()
        {
            return View();
        }
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            ShoppingCartModels cart = ShoppingCartModels.GetInstanceOfObject();
            cart.DestroyInstance();
            return RedirectToAction("Index", "Home");
        }
    }
}