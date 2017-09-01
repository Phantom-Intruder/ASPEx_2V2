using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPEx_2.Models
{
    public class ShoppingCartModels
    {
        private static ShoppingCartModels instanceOfObject;
        private ShoppingCartModels() { }
        public static ShoppingCartModels getInstanceOfObject()
        {
            if (instanceOfObject == null)
            {
                instanceOfObject        = new ShoppingCartModels();
            }

            return instanceOfObject;
        }

        void destroyInstance()
        {
            instanceOfObject = null;
        }

        public Dictionary<string, Product> ProductsList  = new Dictionary<string, Product>();
        public decimal TotalPrice { get; set; }
        public void addProductToCart(int id)
        {
            
            //TODO: Find relevant product
            Product         product                  = Product.ExecuteCreate(id);
            //TODO: Increment this.TotalPrice by product.price
            this.TotalPrice                          = this.TotalPrice + product.Price;
            //TODO: Add product to this.ProductsList
            ProductsList[product.Name]          = product;
            //TODO: this.ProductsList[product.Name] = product
        }
    }
}