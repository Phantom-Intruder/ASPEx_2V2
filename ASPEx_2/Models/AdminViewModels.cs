using ECommerce.Tables.Content;
using System.Collections.Generic;
using System.Linq;

namespace ASPEx_2.Models
{
    public class AdminViewModels
    {
        #region Class properties
        public      List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }
        public      List<ECommerce.Tables.Content.Product>          ProductsList { get; set; }
        #endregion

        #region Class fields
        private         static      AdminViewModels                 instanceOfObject;

        public      IOrderedEnumerable<KeyValuePair<string, int>>       sortedDictionary;
        public      Dictionary<string, int>                             productsDictionary              = new Dictionary<string, int>();
        public      Dictionary<string, Product>                         categoryProductsDictionary      = new Dictionary<string, Product>();
        public      List<KeyValuePair<string, int>>                     listOfItems                     = new List<KeyValuePair<string, int>>();
        public      List<KeyValuePair<string, Product>>                 listOfCategoryItems             = new List<KeyValuePair<string, Product>>();
        public      List<KeyValuePair<string, Product>>                 listOfCategoryItemsUsed         = new List<KeyValuePair<string, Product>>();
        #endregion

        #region Get data for admin view models 
        /// <summary>
        /// Get the products of a particular category 
        /// </summary>
        /// <param name="id"></param>
        public void GetCategoriesInProduct(int id)
        {
            foreach (var item in listOfCategoryItems)
            {
                if (item.Value.CategoryID == id)
                {
                    listOfCategoryItemsUsed.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets the product by ID
        /// </summary>
        /// <param name="id"></param>
        public void GetProduct(int id)
        {
            listOfCategoryItemsUsed     = new List<KeyValuePair<string, Product>>();
            foreach (var item in listOfCategoryItems)
            {
                if (item.Value.ID == id)
                {
                    listOfCategoryItemsUsed.Add(item);
                }
            }
        }
        #endregion

        #region Singleton class methods 
        /// <summary>
        /// Get the current running instance of the object 
        /// </summary>
        /// <returns></returns>
        public static AdminViewModels GetInstanceOfObject()
        {
            if (instanceOfObject == null)
            {
                instanceOfObject        = new AdminViewModels();
            }

            return instanceOfObject;
        }

        /// <summary>
        /// Destroys the current running instance
        /// </summary>
        public void DestroyInstance()
        {
            instanceOfObject            = null;
        }

        /// <summary>
        /// Constructor for class
        /// </summary>
        private AdminViewModels()
        {
            this.ProductsList                                   = ECommerce.Tables.Content.Product.List();
            this.CategoriesList                                 = ECommerce.Tables.Content.Category.List();

            foreach (Product product in ProductsList)
            {
                productsDictionary[product.Name]                = product.Status;
                categoryProductsDictionary[product.Name]        = product;
            }
            sortedDictionary                                    = from entry in productsDictionary orderby entry.Value descending select entry;
            var         sortedCategoryDict                      = from entry in categoryProductsDictionary orderby entry.Value descending select entry;
            listOfItems                                         = sortedDictionary.ToList();
            listOfCategoryItems                                 = sortedCategoryDict.ToList();
        }
        #endregion
    }
}