using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECommerce.Tables;

namespace ASPEx_2.Models
{
    public class ProductModels
    {
        #region Properties
        private List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        private List<ECommerce.Tables.Content.Category> CategoryList { get; set; }


        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }

        [Required]
        [Display(Name = "Path")]
        public string FilePath { get; set; }

        [Required]
        public string EditField { get; set; }
        #endregion

        #region Class members
        private List<string> CategoryNamesList      = new List<string>();
        #endregion

        #region Class constructor 
        public ProductModels()
        {
            this.ProductsList       = ECommerce.Tables.Content.Product.List();
            this.CategoryList       = ECommerce.Tables.Content.Category.List();
        }
        #endregion

        #region Getters
        public List<ECommerce.Tables.Content.Product> GetProductsList()
        {
            return this.ProductsList;
        }

        public List<ECommerce.Tables.Content.Category> GetCategoryList()
        {
            return this.CategoryList;
        }

        public List<string> GetCategoryNamesList()
        {
            foreach (var item in CategoryList)
            {
                CategoryNamesList.Add(item.Name);
            }
            return this.CategoryNamesList;
        }
        #endregion
    }
}