using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPEx_2.Models
{
    public class CategoryProductModels
    {
        public List<Product> listOfProducts                = ECommerce.Tables.Content.Product.List();
        public List<Product> listOfProductsInACategory     = new List<Product>();

        public CategoryProductModels(int id) {
            foreach (var item in listOfProducts)
            {
                if (item.CategoryID == id)
                {
                    listOfProductsInACategory.Add(item);
                }
            }
}
    }
}