using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class ProductListController : Controller
    {
        public static int       IDNew       = 0;
        
        #region Display views
        // GET: ProductList
        public ActionResult ProductList()
        {
            ProductModels       product				= new ProductModels();
            return View(product);
        }
        public ActionResult EditProductView()
        {
            AdminViewModels		models              = AdminViewModels.GetInstanceOfObject();
            return View(models);
        }
        #endregion

        #region Get methods
        [HttpGet]
        public ActionResult EditProductView(string id)
        {
            ProductModels		productModels		= new ProductModels();
            ViewBag.Details							= "Enter details below";
            try
			{
				Product product						= Product.ExecuteCreate(Int32.Parse(id));

				IDNew								= Int32.Parse(id);
				ViewBag.Message						= "Added " + product.Name;
				productModels.CreateProduct(productModels, product);
			}
			catch (ArgumentNullException n)
            {
                productModels.FilePath               = "";
                return View(productModels);
            }
            return View(productModels);
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
                Product product             = Product.ExecuteCreate(Int32.Parse(id));

                IDNew                       = Int32.Parse(id);
                ViewBag.Name                = product.Name;
                ViewBag.Description         = product.Description;
                ViewBag.Price               = product.Price;
                ViewBag.FileName            = product.ImageName;
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
        public ActionResult EditProductView(ProductModels model)
		{
			string		filePathField			= "";
			int			idOfCategoryField		= 0;
			int			index					= 1;
			model.SetCategoryID(model, ref idOfCategoryField, ref index);
			if (model.FileUpload != null)
			{
				filePathField					= model.CopyFileIntoFilestore(model);
			}
			else
			{
				filePathField					= model.FilePath;
			}
			if (model.EditField == null)
			{
				ViewBag.Message					= "Added " + model.Name + model.Description + " " + filePathField + " " + model.Price + " " + model.Category;

				model.CreateAndInsertNewProduct(model, filePathField, idOfCategoryField);

			}
			else
			{

				Product			record			= Product.ExecuteCreate(Int32.Parse(idOfCategoryField + ""),
																		model.Name,
																		model.Description,
																		model.Price,
																		filePathField, 1, 50, 51);
				record.Update(IDNew, record);

			}

			return RedirectToAction("ProductList", "ProductList");

		}

		[HttpPost]
        public ActionResult ProductList(string idField)
        {
            int						id                      = Int32.Parse(idField);
            ShoppingCartModels		cart					= ShoppingCartModels.GetInstanceOfObject();
            Product					productToBeUpdated		= Product.ExecuteCreate(id);
            int						newQuantity             = productToBeUpdated.Status + 1;
            int						idfield                 = Int32.Parse(idField);
            Product					product                 = Product.ExecuteCreate(productToBeUpdated.CategoryID,
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