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
            var file                = model.FileUpload;

            var directories         = Directory.GetDirectories(@"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Category");
            int folderNumber        = directories.Length;
            folderNumber            = folderNumber + 1;
            string targetPath       = @"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Category\" + folderNumber;
            string destFile         = System.IO.Path.Combine(targetPath, "" + folderNumber + ".png");
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
                file.SaveAs(destFile);
            }
            else
            {
                Console.WriteLine("Source path does not exist!");
            }
            

            string filePathField        = @"/Category/" + folderNumber + "/" + folderNumber + ".png";

            if (model.EditField == null)
            {
                Category record         = Category.ExecuteCreate(model.Name,
                                                                 model.Description,
                                                                 filePathField,
                                                                 1,
                                                                 50,
                                                                 51);

                record.Insert();
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

            try
            {
                Category category               = Category.ExecuteCreate(Int32.Parse(id));

                IDNew                           = Int32.Parse(id);
                categoryModels.Name             = category.Name;
                categoryModels.Description      = category.Description;
                categoryModels.FilePath         = category.ImageName;
                categoryModels.EditField        = "true";
            }
            catch (ArgumentNullException n)
            {
                //AdminViewModels models2 = AdminViewModels.GetInstanceOfObject();
                categoryModels.FilePath         = "";
                return View(categoryModels);
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
            try
            {
                IDNew       = Int32.Parse(id);
                model       = new CategoryProductModels(IDNew);
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
    }
}