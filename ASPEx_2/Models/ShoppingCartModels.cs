﻿using ECommerce.Tables.Content;
using System.Collections.Generic;

namespace ASPEx_2.Models
{
    public class ShoppingCartModels
    {
        #region Class members
        private  static     ShoppingCartModels              instanceOfObject    = null;
        public        Dictionary<string, Product>     ProductsList        = new Dictionary<string, Product>();
        #endregion

        #region Properties
        public      decimal         TotalPrice { get; set; }
        #endregion

        #region Class constructor
        private ShoppingCartModels() { }
        #endregion

        #region Singleton class methods
        /// <summary>
        /// Get the running instance of the class
        /// </summary>
        /// <returns></returns>
        public static ShoppingCartModels GetInstanceOfObject()
        {
            if (instanceOfObject == null)
            {
                instanceOfObject        = new ShoppingCartModels();
            }

            return instanceOfObject;
        }

        /// <summary>
        /// Destroy instance of class
        /// </summary>
        public void DestroyInstance()
        {
            instanceOfObject = null;
        }
        #endregion

        #region Class methods
        /// <summary>
        /// Get the product by ID and add it to the cart 
        /// </summary>
        /// <param name="id"></param>
        public void AddProductToCart(int id)
        {
            //Find relevant product
            Product         product                  = Product.ExecuteCreate(id);
            //Increment this.TotalPrice by product.price
            this.TotalPrice                          = this.TotalPrice + product.Price;
            //Add product to this.ProductsList
            ProductsList[product.Name]          = product;
        }
        #endregion
    }
}