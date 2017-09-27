using ASPEx_2.Helpers;
using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class CategoryController : Controller
    {
        #region Class fields
        public static int IDNew             = 0;
        #endregion
        
        #region Display views
        // GET: CategoryList
        public ActionResult List()
        {
            CategoryModels		category					= new CategoryModels();
			SessionSingleton.Current.CurrentCategory		= category;

			return View(SessionSingleton.Current.CurrentCategory);
        }
        public ActionResult Edit()
        {
            CategoryModels		category					= new CategoryModels();
			SessionSingleton.Current.CurrentCategory		= category;

			return View(SessionSingleton.Current.CurrentCategory);
        }
        #endregion

        #region Post methods
        [HttpPost]
        public ActionResult Edit(CategoryModels model)
        {
			SessionSingleton.Current.CurrentCategory		= model;

			if (SessionSingleton.Current.CurrentCategory.Validation())
			{
				SessionSingleton.Current.CurrentCategory.Save(IDNew);

				return RedirectToAction("List", "Category");
			}
			ViewBag.NoImage									= "You haven't selected an image";
			return View(SessionSingleton.Current.CurrentCategory);
        }
		#endregion

		#region Get methods
		[HttpGet]
        public ActionResult Edit(string id)
        {
            CategoryModels		categoryModels							= new CategoryModels();
			SessionSingleton.Current.CurrentCategory					= categoryModels;

			if (id != null) { 

				IDNew													= Int32.Parse(id);

				SessionSingleton.Current.CurrentCategory.EditCategoryOfID(id);
			}
			else
            {
				SessionSingleton.Current.CurrentCategory.FilePath		= " ";
            }

            return View(SessionSingleton.Current.CurrentCategory);
        }

		[HttpGet]
        public ActionResult Delete(string id)
        {
            Category.Delete(Int32.Parse(id));

            return View();
        }

        [HttpGet]
        public ActionResult ShowCategoryView(string id)
        {
            CategoryProductModels model;
            if (id != null)
			{ 
                IDNew       = Int32.Parse(id);
                model       = new CategoryProductModels(IDNew);
            }
            else
            {
                return View();
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ShowProductView(string id)
		{
			IDNew											= Int32.Parse(id);

			ProductModels		productModels				= new ProductModels();
			SessionSingleton.Current.CurrentProduct			= productModels;
			SessionSingleton.Current.CurrentProduct.ShowProductFromId(id);
			return View(SessionSingleton.Current.CurrentProduct);
		}

		#endregion
	}
}