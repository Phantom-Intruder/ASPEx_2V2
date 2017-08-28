using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ECommerce.Tables;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
        public List<ECommerce.Tables.Content.Category> CategoriesList { get; set; }
        public CategoryModels()
        {
            this.CategoriesList = ECommerce.Tables.Content.Category.List();
        }
    }
}