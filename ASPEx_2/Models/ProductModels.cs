using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Tables;

namespace ASPEx_2.Models
{
    public class ProductModels
    {
        public List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        public int MyProperty { get; set; }
        public ProductModels()
        {
            this.ProductsList = ECommerce.Tables.Content.Product.List();
        }
    }
}