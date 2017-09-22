using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
        #region Properties
        private      List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }

        [Required]
        [Display(Name = "Path")]
        public string FilePath { get; set; }

        [Required]
        public string EditField { get; set; }
        #endregion

        #region Class constructor
        public CategoryModels()
        {
            this.CategoriesList         = ECommerce.Tables.Content.Category.List();
        }
        #endregion

        #region Getters
        public List<ECommerce.Tables.Content.Category> GetCategoriesList()
        {
            return this.CategoriesList;
        }
        #endregion
    }
}