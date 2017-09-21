using System.Collections.Generic;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
        #region Properties
        private      List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }
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