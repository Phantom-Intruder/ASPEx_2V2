using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPEx_2.Models
{
    public class CategoryProductModels
    {
        private      List<Product>       listOfProducts                = ECommerce.Tables.Content.Product.List();
        private      List<Product>       listOfProductsInACategory     = new List<Product>();

    #region Constructor
        public CategoryProductModels(int id) {
            foreach (var item in listOfProducts)
            {
                if (item.CategoryID == id)
                {
                    listOfProductsInACategory.Add(item);
                }
            }
}
        #endregion

        #region Getters
        public List<Product> GetListOfProducts()
        {
            return this.listOfProducts;
        }

        public List<Product> GetListOfProductsInACategory()
        {
            return this.listOfProductsInACategory;
        }
        #endregion
    }
}