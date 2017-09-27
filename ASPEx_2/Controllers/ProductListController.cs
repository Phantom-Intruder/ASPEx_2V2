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
			if (id != null)
			{
				Product			product				= Product.ExecuteCreate(Int32.Parse(id));
            
				IDNew								= Int32.Parse(id);
				ViewBag.Message						= "Added " + product.Name;
				productModels.CreateProduct(productModels, product);
			}
			else
			{
                productModels.FilePath               = " ";
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
			IDNew = Int32.Parse(id);

			ProductModels productModels = new ProductModels();

			productModels.ShowProductFromId(id);
			return View(productModels);
		}
		#endregion

		#region Post methods
		[HttpPost]
        public ActionResult EditProductView(ProductModels model)
		{
			if (model.Validation())
			{
				model.Save(IDNew);

				return RedirectToAction("ProductList", "ProductList");
			}
			ViewBag.NoImage = "You haven't selected an image";
			return View(model);
		}

		[HttpPost]
        public ActionResult ProductList(string idField)
		{
			ProductModels		product			= new ProductModels();
			product.AddProductToShoppingCart(idField);

			return PartialView("_AddedCorrectlyView");
		}
		#endregion
	}

}