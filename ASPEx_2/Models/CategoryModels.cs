using System.Collections.Generic;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
        #region Properties
        public      List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }
        #endregion

        #region Class members
        public CategoryModels()
        {
            this.CategoriesList         = ECommerce.Tables.Content.Category.List();
        }
        #endregion
    }
}