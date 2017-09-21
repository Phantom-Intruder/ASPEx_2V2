using ECommerce.Tables.Content;
using System.Collections.Generic;
using System.Linq;

namespace ASPEx_2.Models
{
    public class AdminViewModels
    {   
        #region Properties
        private List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }
        private List<ECommerce.Tables.Content.Product>          ProductsList { get; set; }
        #endregion

        #region members
        private static      AdminViewModels                         instanceOfObject;

        private IOrderedEnumerable<KeyValuePair<string, int>>       sortedDictionary;
        private Dictionary<string, int>                             productsDictionary              = new Dictionary<string, int>();
        private Dictionary<string, Product>                         categoryProductsDictionary      = new Dictionary<string, Product>();
        private List<KeyValuePair<string, int>>                     listOfItems                     = new List<KeyValuePair<string, int>>();
        private List<KeyValuePair<string, Product>>                 listOfCategoryItems             = new List<KeyValuePair<string, Product>>();
        private List<KeyValuePair<string, Product>>                 listOfCategoryItemsUsed         = new List<KeyValuePair<string, Product>>();
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

        /// <summary>
        /// Get category list
        /// </summary>
        /// <returns>category list</returns>
        public List<Category> GetCategoriesList()
        {
            return this.CategoriesList;
        }

        /// <summary>
        /// Get list of category items used 
        /// </summary>
        /// <returns>category items used </returns>
        public List<KeyValuePair<string, Product>> GetListOfCategoryItemsUsed()
        {
            return this.listOfCategoryItemsUsed;
        }

        /// <summary>
        /// Get list of category items
        /// </summary>
        /// <returns>list of category items</returns>
        public List<KeyValuePair<string, Product>> GetListOfCategoryItems()
        {
            return this.listOfCategoryItems;
        }

        /// <summary>
        /// Get list of items 
        /// </summary>
        /// <returnsGet list of items ></returns>
        public List<KeyValuePair<string, int>> GetListOfItems()
        {
            return this.listOfItems;
        }

        /// <summary>
        /// Gets a dictionary of products
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetProductsDictionary()
        {
            return this.productsDictionary;
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