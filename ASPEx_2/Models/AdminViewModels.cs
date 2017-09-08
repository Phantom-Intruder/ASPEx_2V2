using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPEx_2.Models
{
    public class AdminViewModels
    {
        public Dictionary<string, int> productsDictionary = new Dictionary<string, int>();
        public List<ECommerce.Tables.Content.Category> CategoriesList { get; set; }
        public List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        public Dictionary<string, Product> categoryProductsDictionary = new Dictionary<string, Product>();
        public IOrderedEnumerable<KeyValuePair<string, int>> sortedDict;
        public List<KeyValuePair<string, int>> listOfItems;
        public List<KeyValuePair<string, Product>> listOfCategoryItems;
        public List<KeyValuePair<string, Product>> listOfCategoryItemsUsed = new List<KeyValuePair<string, Product>>();

        public void getCategoriesInProduct(int id)
        {
            foreach (var item in listOfCategoryItems)
            {
                if (item.Value.CategoryID == id)
                {
                    listOfCategoryItemsUsed.Add(item);
                }
            }
        }

        public void getProduct(int id)
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

       
       

        public AdminViewModels()
        {
            //TODO: take all products from db and add to productsDictionary.
            this.ProductsList = ECommerce.Tables.Content.Product.List();
            this.CategoriesList = ECommerce.Tables.Content.Category.List();

            foreach (Product product in ProductsList)
            {
                productsDictionary[product.Name]      = product.Status;
                categoryProductsDictionary[product.Name] = product;
            }
            //TODO: Order according to the keys. 
            sortedDict = from entry in productsDictionary orderby entry.Value descending select entry;
            var sortedCategoryDict = from entry in categoryProductsDictionary orderby entry.Value descending select entry;
            listOfItems = sortedDict.ToList();
            listOfCategoryItems = sortedCategoryDict.ToList();
        }
    }
}