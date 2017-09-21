using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class ProductListController : Controller
    {
        public static int IDNew = 0;

        #region Display views
        // GET: ProductList
        public ActionResult ProductList()
        {
            ProductModels product = new ProductModels();

            return View(product);
        }
        public ActionResult EditProductView()
        {

            return View();
        }
        #endregion

        #region Get methods
        [HttpGet]
        public ActionResult EditProductView(string id)
        {
            try
            {
                Product product = Product.ExecuteCreate(Int32.Parse(id));

                IDNew = Int32.Parse(id);
                ViewBag.Message = "Added " + product.Name;
                ViewBag.Name = product.Name;
                ViewBag.Description = product.Description;
                ViewBag.Price = product.Price;
                ViewBag.FileName = product.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeleteProductView(string id)
        {
            Product.Delete(Int32.Parse(id));

            return View();
        }

        [HttpGet]
        public ActionResult ShowProductView(string id)
        {
            try
            {
                Product product = Product.ExecuteCreate(Int32.Parse(id));

                IDNew = Int32.Parse(id);
                ViewBag.Name = product.Name;
                ViewBag.Description = product.Description;
                ViewBag.Price = product.Price;
                ViewBag.FileName = product.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
        }
        #endregion

        #region Post methods
        [HttpPost]
        public ActionResult EditProductView(string nameField,
                                            string descriptionField,
                                            string filePathField,
                                            string priceField,
                                            string categoryField,
                                            string isEditField)
        {
            if (isEditField == "false")
            {
                ViewBag.Message = "Added " + nameField + descriptionField + " " + filePathField + " " + priceField + " " + categoryField;

                Product record = Product.ExecuteCreate(Int32.Parse(categoryField),
                                                                        nameField,
                                                                        descriptionField,
                                                                        Decimal.Parse(priceField),
                                                                        filePathField, 1, 50, 51);
                record.Insert();
            }
            else
            {
                Product record = Product.ExecuteCreate(Int32.Parse(categoryField),
                                                                        nameField,
                                                                        descriptionField,
                                                                        Decimal.Parse(priceField),
                                                                        filePathField, 1, 50, 51);
                record.Update(IDNew, record);
            }
            return PartialView("_AddedView");
        }
        
        [HttpPost]
        public ActionResult ProductList(string idField)
        {
            int id = Int32.Parse(idField);
            ShoppingCartModels cart = ShoppingCartModels.GetInstanceOfObject();
            Product productToBeUpdated = Product.ExecuteCreate(id);
            int newQuantity = productToBeUpdated.Status + 1;
            int idfield = Int32.Parse(idField);
            Product product = Product.ExecuteCreate(productToBeUpdated.CategoryID,
                                                                                    productToBeUpdated.Name,
                                                                                    productToBeUpdated.Description,
                                                                                    productToBeUpdated.Price,
                                                                                    productToBeUpdated.ImageName,
                                                                                    newQuantity,
                                                                                    productToBeUpdated.CreatedAccountID,
                                                                                    productToBeUpdated.ModifiedAccountID);

            cart.AddProductToCart(id);
            product.Update(idfield, product);
            return PartialView("_AddedCorrectlyView");
        }
        #endregion
    }

}