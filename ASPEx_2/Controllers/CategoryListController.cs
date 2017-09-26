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
    public class CategoryListController : Controller
    {
        #region Class fields
        public static int IDNew             = 0;
        #endregion
        
        #region Display views
        // GET: CategoryList
        public ActionResult CategoryList()
        {
            CategoryModels category         = new CategoryModels();

            return View(category);
        }
        public ActionResult EditCategoryView()
        {
            CategoryModels category         = new CategoryModels();

            return View(category);
        }
        #endregion

        #region Post methods
        [HttpPost]
        public ActionResult EditCategoryView(CategoryModels model)
        {
            string filePathField        = "";
            if (model.FileUpload != null)
			{
				filePathField			= model.CopyFileIntoFilestore(model);
			}
			else
            {
                filePathField           = model.FilePath;
            }
            if (model.EditField == null)
			{
				model.CreateNewRecord(model, filePathField);
			}
			else
            {
                Category record         = Category.ExecuteCreate(model.Name,
                                                                 model.Description,
                                                                 filePathField,
                                                                 1,
                                                                 50,
                                                                 51);

                record.Update(IDNew, record);
            }

            return RedirectToAction("CategoryList", "CategoryList");
        }
		#endregion

		#region Get methods
		[HttpGet]
        public ActionResult EditCategoryView(string id)
        {
            CategoryModels categoryModels       = new CategoryModels();

            if(id != null) { 

				IDNew							= Int32.Parse(id);

				categoryModels.EditCategoryOfID(id, categoryModels);
			}
			else
            {
                categoryModels.FilePath         = "";
            }
            return View(categoryModels);
        }

		

		[HttpGet]
        public ActionResult DeleteCategoryView(string id)
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
            Product				product             = Product.ExecuteCreate(Int32.Parse(id));
			ProductModels		productModels		= new ProductModels();

			if (product != null)
			{
				IDNew								= Int32.Parse(id);
				productModels.Name					= product.Name;
				productModels.Description			= product.Description;
				productModels.Price					= product.Price;
				productModels.FilePath				= product.ImageName;
            }
            return View(productModels);
        }
        #endregion
    }
}