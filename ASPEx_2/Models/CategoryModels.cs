using System.Collections.Generic;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
        #region Class properties
        public      List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }
        #endregion

        #region Class constructor
        public CategoryModels()
        {
            this.CategoriesList         = ECommerce.Tables.Content.Category.List();
        }
        #endregion
    }
}