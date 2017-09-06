using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Tables.Content;
using ASPEx_2.Models;

namespace ASPEx_2.Controllers
{
    public class HomeController : Controller
    {
        public static int IDNew                           = 0;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShoppingCartView()
        {
            ViewBag.Message                     = "Your application description page.";
            ShoppingCartModels      cart        = ShoppingCartModels.getInstanceOfObject();
            return View(cart);
        }
        [HttpPost]
        public ActionResult ShoppingCartView(string name)
        {
            if (name != null)
            {
                ViewBag.Message = "Your application description page.";
                ShoppingCartModels cart = ShoppingCartModels.getInstanceOfObject();
                cart.TotalPrice = cart.TotalPrice - cart.ProductsList[name].Price;
                cart.ProductsList.Remove(name);
                return View(cart);
            }
            else
            {
                //TODO: Save shopping cart code
                ShoppingCartModels      cart                = ShoppingCartModels.getInstanceOfObject();
                int                     accountID           = Int32.Parse(Session["CurrentID"].ToString());
                Order                   order               = Order.ExecuteCreate(accountID, 1, 1, cart.TotalPrice);
                order.Insert();
                decimal                 subtotal            = 0;
                bool                    isPresent           = false;
                List<Order>             orderList           = Order.ListByAccountID(accountID);
                foreach (Order o in orderList)
                {
                    List<OrderItem>     orderItemList       = OrderItem.ListByOrderID(o.ID);
                    foreach (OrderItem item in orderItemList)
                    {
                        item.Delete();
                    }
                }
                foreach (string key in cart.ProductsList.Keys)
                {
                    Product             product     = cart.ProductsList[key];
                    subtotal                        = subtotal + product.Price;
                    OrderItem           orderItem   = OrderItem.ExecuteCreate(order.ID, product.ID, 1, product.Price, subtotal);
                    
                    if (!isPresent)
                    {
                        orderItem.Insert();
                    }
                }
                return View(cart);
            }
        }

        public ActionResult ProductList()
        {
            ViewBag.Message                     = "Your application description page.";
            ProductModels           product     = new ProductModels();

            return View(product);
        }

        [HttpPost]
        public ActionResult ProductList(string idField)
        {
            int                     id          = Int32.Parse(idField);
            ShoppingCartModels      cart        = ShoppingCartModels.getInstanceOfObject();
            cart.addProductToCart(id);
            return PartialView("_AddedCorrectlyView");
        }

        public ActionResult CategoryList()
        {
            ViewBag.Message                     = "Your contact page.";
            CategoryModels category             = new CategoryModels();
            return View(category);
        }

        public ActionResult EditCategoryView()
        {
            ViewBag.Message                     = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult EditCategoryView(string nameField, string descriptionField, string filePathField, string isEditField)
        {
            if (isEditField == "false")
            {
                Category record                 = Category.ExecuteCreate(nameField, descriptionField, filePathField, 1, 50, 51);
                record.Insert();
            }
            else
            {
                Category record                 = Category.ExecuteCreate(nameField, descriptionField, filePathField, 1, 50, 51);
                record.Update(IDNew, record);
            }
            return PartialView("_AddedView");
        }

        [HttpPost]
        public ActionResult EditProductView(string nameField, string descriptionField, string filePathField, string priceField, string categoryField, string isEditField)
        {
           if (isEditField == "false")
            {
                ViewBag.Message                 = "Added " + nameField + descriptionField + " " + filePathField + " " + priceField + " " + categoryField;

                Product record                  = Product.ExecuteCreate(Int32.Parse(categoryField),
                                                                        nameField,
                                                                        descriptionField,
                                                                        Decimal.Parse(priceField),
                                                                        filePathField, 1, 50, 51);
                record.Insert();
            }
            else
            {
                Product record                 = Product.ExecuteCreate(Int32.Parse(categoryField),
                                                                        nameField,
                                                                        descriptionField,
                                                                        Decimal.Parse(priceField),
                                                                        filePathField, 1, 50, 51);
                record.Update(IDNew, record);
            }
            return PartialView("_AddedView");
        }

        [HttpGet]
        public ActionResult EditProductView(string id)
        {
            try
            {
                IDNew                           = Int32.Parse(id);
                Product product                 = Product.ExecuteCreate(Int32.Parse(id));
                ViewBag.Message                 = "Added " + product.Name+ " " + product.Description;
                ViewBag.Name                    = product.Name;
                ViewBag.Description             = product.Description;
                ViewBag.Price                   = product.Price;
                ViewBag.FileName                = product.ImageName;
            }catch (ArgumentNullException n)
            {
                return View();
            }
                return View();
        }

        [HttpGet]
        public ActionResult EditCategoryView(string id)
        {
            try
            {
                IDNew                           = Int32.Parse(id);
                Category category               = Category.ExecuteCreate(Int32.Parse(id));
                ViewBag.Name                    = category.Name;
                ViewBag.Description             = category.Description;
                ViewBag.FileName                = category.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult DeleteCategoryView(string id)
        {
            Category.Delete(Int32.Parse(id));

            return View();
        }

        [HttpGet]
        public ActionResult DeleteProductView(string id)
        {
            Product.Delete(Int32.Parse(id));

            return View();
        }

        public ActionResult EditProductView()
        {
            ViewBag.Message                     = "Your application description page.";

            return View();
        }

        [HttpGet]
        public ActionResult ShowProductView(string id)
        {
            try
            {
                IDNew                           = Int32.Parse(id);
                Product product                 = Product.ExecuteCreate(Int32.Parse(id));
                
                ViewBag.Name                    = product.Name;
                ViewBag.Description             = product.Description;
                ViewBag.Price                   = product.Price;
                ViewBag.FileName                = product.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult ShowCategoryView(string id)
        {
            CategoryProductModels       model;
            try
            {
                IDNew                                   = Int32.Parse(id);
                model                                   = new CategoryProductModels(IDNew); 
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View(model);
        }

        
    }
}