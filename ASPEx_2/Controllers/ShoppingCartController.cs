using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class ShoppingCartController : Controller
    {
        #region Display views
        // GET: ShoppingCart
        public ActionResult ShoppingCartView()
        {
            ShoppingCartModels cart = ShoppingCartModels.GetInstanceOfObject();

            return View(cart);
        }
        #endregion

        #region Post methods
        [HttpPost]
        public ActionResult ShoppingCartView(string name)
        {
            if (name != null)
            {
                ShoppingCartModels cart = ShoppingCartModels.GetInstanceOfObject();

                cart.TotalPrice = cart.TotalPrice - cart.ProductsList[name].Price;
                cart.ProductsList.Remove(name);

                return View(cart);
            }
            else
            {
                //Save shopping cart code
                ShoppingCartModels cart = ShoppingCartModels.GetInstanceOfObject();
                int accountID = Int32.Parse(Session["CurrentID"].ToString());
                Order order = Order.ExecuteCreate(accountID, 1, 1, cart.TotalPrice);
                decimal subtotal = 0;
                bool isPresent = false;
                List<Order> orderList = Order.ListByAccountID(accountID);

                order.Insert();

                foreach (Order o in orderList)
                {
                    List<OrderItem> orderItemList = OrderItem.ListByOrderID(o.ID);

                    foreach (OrderItem item in orderItemList)
                    {
                        item.Delete();
                    }
                }

                foreach (string key in cart.ProductsList.Keys)
                {
                    Product product = cart.ProductsList[key];
                    subtotal = subtotal + product.Price;
                    OrderItem orderItem = OrderItem.ExecuteCreate(order.ID, product.ID, 1, product.Price, subtotal);

                    if (!isPresent)
                    {
                        orderItem.Insert();
                    }
                }
                return View(cart);
            }
        }
#endregion
    }
}