﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Tables;

namespace ASPEx_2.Models
{
    public class ProductModels
    {
        #region Class properties
        public List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        public int MyProperty { get; set; }
        #endregion

        #region Class constructor
        public ProductModels()
        {
            this.ProductsList       = ECommerce.Tables.Content.Product.List();
        }
        #endregion
    }
}