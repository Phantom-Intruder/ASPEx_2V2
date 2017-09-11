using ASPEx_2.Models;
using ECommerce.Tables.Active.HR;
using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {

        #region Account login 
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl           = returnUrl;
            ViewBag.LoginMessage        = "Enter your login details below";
            ViewBag.LoginFailed         = false;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                Account                 record                      = Account.GetAccountByEmail(model.Email);
                var                     salt                        = record.Salt;
                var                     encodingPasswordString      = Helper.EncodePassword(model.Password, salt);
                ShoppingCartModels      cart                        = ShoppingCartModels.GetInstanceOfObject();
                List<Order>             orderList                   = Order.ListByAccountID(record.ID);
                List<OrderItem>         orderItemList               = new List<OrderItem>();

                if (encodingPasswordString == record.Password)
                {
                    ViewBag.LoginFailed                             = false;
                    Session["CurrentUser"]                          = record;
                    Session["CurrentID"]                            = record.ID;
                    UserModel.ID                                    = record.ID;
                    Session["CurrentUserName"]                      = record.FirstName;

                    if (record.Role == 1)
                    {
                        Session["CurrentUserRole"]                  = record.Role;
                    }

                    
                    foreach (Order o in orderList)
                    {
                        orderItemList                               = OrderItem.ListByOrderID(o.ID);
                        foreach (OrderItem item in orderItemList)
                        {
                            cart.AddProductToCart(item.ProductID);
                        }
                    }
                    if (record.Role == 1)
                    {
                        return RedirectToLocal("/Home/AdminView");
                    }
                    else
                    {
                        return RedirectToLocal("/Home/ProductList");
                    }
                }
                else
                {
                    ViewBag.LoginMessage                            = "Wrong username/password";
                    ViewBag.LoginFailed                             = true;
                }
            }
            catch (Exception ignored)
            {
                ViewBag.LoginMessage                                = "Wrong username/password";
                ViewBag.LoginFailed                                 = true;
                return View();
            }
            return View();
        }
        #endregion

        #region Account register
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.RegistrationMessage         = "Enter registration details below";
            ViewBag.RegistrationFailed          = false;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account         recordData                  = Account.GetAccountByEmail(model.Email);

                    model.Role                                  = 0;
                    ViewBag.RegistrationMessage                 = "The e-mail is already in use";
                    ViewBag.RegistrationFailed                  = true;
                    return View();
                }
                catch (Exception ignored)
                {
                    var             salt                        = GetHashCode().ToString();
                    var             encodingPasswordString      = Helper.EncodePassword(model.Password, salt);
                    Account         record                      = Account.ExecuteCreate(model.FirstName, 
                                                                                        model.LastName, 
                                                                                        model.Email, 
                                                                                        encodingPasswordString, 
                                                                                        salt, 
                                                                                        model.ContactNumber, 
                                                                                        model.ShippingAddress, 
                                                                                        model.Country, 
                                                                                        1, 
                                                                                        model.Role, 
                                                                                        model.CreatedAccountID, 
                                                                                        model.ModifiedAccountID);
                    record.Insert();
                    ViewBag.RegistrationFailed                  = false;
                    Session["CurrentUser"]                      = record;
                    Session["CurrentUserName"]                  = record.FirstName;
                    return RedirectToAction("Index", "Home");
                }

            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region Helpers


        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

        }
        #endregion

    }
}