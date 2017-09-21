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
            ViewBag.LoginMessage        = Constants.ENTER_LOGIN_MESSAGE;
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
                if (ModelState.IsValid)
                {                    
                    if (Account.GetAccountByEmail(model.Email) != null)
                    {
                        Account record = Account.GetAccountByEmail(model.Email);
                        var salt = record.Salt;
                        var encodingPasswordString = Helper.EncodePassword(model.Password, salt);
                        ShoppingCartModels cart = ShoppingCartModels.GetInstanceOfObject();
                        List<Order> orderList = Order.ListByAccountID(record.ID);
                        List<OrderItem> orderItemList = new List<OrderItem>();

                        if (encodingPasswordString == record.Password)
                        {
                            ViewBag.LoginFailed = false;
                            Session["CurrentUser"] = record;
                            Session["CurrentID"] = record.ID;
                            UserModel.ID = record.ID;
                            Session["CurrentUserName"] = record.FirstName;

                            if (record.Role == 1)
                            {
                                Session["CurrentUserRole"] = record.Role;
                            }


                            foreach (Order o in orderList)
                            {
                                orderItemList = OrderItem.ListByOrderID(o.ID);
                                foreach (OrderItem item in orderItemList)
                                {
                                    cart.AddProductToCart(item.ProductID);
                                }
                            }
                            if (record.Role == 1)
                            {
                                return RedirectToAction(Constants.CONTROLLER_ADMIN, Constants.CONTROLLER_HOME);
                            }
                            else
                            {
                                return RedirectToAction(Constants.CONTROLLER_PRODUCT_LIST, Constants.CONTROLLER_HOME);
                            }
                        }
                    else
                    {
                        ViewBag.LoginMessage = Constants.WRONG_USERNAME;
                        ViewBag.LoginFailed = true;
                    }
                    }
                }
                else
                {
                    ViewBag.LoginMessage = Constants.WRONG_USERNAME;
                    ViewBag.LoginFailed = true;
                    return View();
                }
                return View(model);
            }
            catch (Exception ignored)
            {
                ViewBag.LoginMessage = Constants.WRONG_USERNAME;
                ViewBag.LoginFailed = true;
                return View();
            }
        }
        #endregion

        #region Account register
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.RegistrationMessage         = Constants.REGISTRATION_MESSAGE;
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
                    ViewBag.RegistrationMessage                 = Constants.EMAIL_IN_USE;
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
                    return RedirectToAction(Constants.CONTROLLER_INDEX, Constants.CONTROLLER_HOME);
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
            return RedirectToAction(Constants.CONTROLLER_INDEX, Constants.CONTROLLER_HOME);
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