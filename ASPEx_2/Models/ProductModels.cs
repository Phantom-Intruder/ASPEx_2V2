using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Tables;

namespace ASPEx_2.Models
{
    public class ProductModels
    {
        #region Properties
        private List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        #endregion

        #region Class constructor 
        public ProductModels()
        {
            this.ProductsList       = ECommerce.Tables.Content.Product.List();
        }
        #endregion

        #region Getters
        public List<ECommerce.Tables.Content.Product> GetProductsList()
        {
            return this.ProductsList;
        }
        #endregion
    }
}