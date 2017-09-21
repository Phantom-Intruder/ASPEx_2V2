using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class CategoryListController : Controller
    {
        #region Class fields
        public static int IDNew = 0;
        #endregion
        
        #region Display views
        // GET: CategoryList
        public ActionResult CategoryList()
        {
            CategoryModels category = new CategoryModels();

            return View(category);
        }
        public ActionResult EditCategoryView()
        {

            return View();
        }
        #endregion

        #region Post methods
        [HttpPost]
        public ActionResult EditCategoryView(string nameField,
                                            string descriptionField,
                                            string filePathField,
                                            string isEditField)
        {
            if (isEditField == "false")
            {
                Category record = Category.ExecuteCreate(nameField,
                                                                    descriptionField,
                                                                    filePathField,
                                                                    1,
                                                                    50,
                                                                    51);

                record.Insert();
            }
            else
            {
                Category record = Category.ExecuteCreate(nameField,
                                                                    descriptionField,
                                                                    filePathField,
                                                                    1,
                                                                    50,
                                                                    51);

                record.Update(IDNew, record);
            }

            return PartialView("_AddedView");
        }
        #endregion
      
        #region Get methods
        [HttpGet]
        public ActionResult EditCategoryView(string id)
        {
            try
            {
                Category category = Category.ExecuteCreate(Int32.Parse(id));

                IDNew = Int32.Parse(id);
                ViewBag.Name = category.Name;
                ViewBag.Description = category.Description;
                ViewBag.FileName = category.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
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
            try
            {
                IDNew = Int32.Parse(id);
                model = new CategoryProductModels(IDNew);
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View(model);
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
    }
}