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
            ProductModels       product         = new ProductModels();
            ViewBag.Details                     = "List of Products";
            return View(product);
        }
        public ActionResult EditProductView()
        {
            AdminViewModels models              = AdminViewModels.GetInstanceOfObject();
            return View(models);
        }
        #endregion

        #region Get methods
        [HttpGet]
        public ActionResult EditProductView(string id)
        {
            ProductModels productModels             = new ProductModels();
            ViewBag.Details = "Enter details below";
            try
            {
                    Product product                 = Product.ExecuteCreate(Int32.Parse(id));

                    IDNew                           = Int32.Parse(id);
                    ViewBag.Message                 = "Added " + product.Name;
                    productModels.Name              = product.Name;
                    productModels.Description       = product.Description;
                    productModels.Price             = product.Price;
                    productModels.FilePath          = product.ImageName;
                    productModels.EditField         = "true";   
            }
            catch (ArgumentNullException n)
            {
                //AdminViewModels models2 = AdminViewModels.GetInstanceOfObject();
                productModels.FilePath               = "";
                return View(productModels);
            }
            //AdminViewModels models = AdminViewModels.GetInstanceOfObject();
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
            string filePathField                = "";
            int idOfCategoryField               = 0;
            if (model.FileUpload != null)
            {
                var file                        = model.FileUpload;

                var directories                 = Directory.GetDirectories(@"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Product");
                int folderNumber                = directories.Length;
                folderNumber                    = folderNumber + 1;
                string targetPath               = @"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Product\" + folderNumber;
                string destFile                 = System.IO.Path.Combine(targetPath, "" + folderNumber + ".png");
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);
                    file.SaveAs(destFile);
                }
                else
                {
                    Console.WriteLine("Source path does not exist!");
                }
                
                int index                       = 1;
                foreach (string name in model.GetCategoryNamesList())
                {
                    if (name == model.Name)
                    {
                        idOfCategoryField       = index;
                    }
                    index                       = index + 1;
                }

                filePathField                   = @"/Product/" + folderNumber + "/" + folderNumber + ".png";
            }
            else
            {
                filePathField                   = model.FilePath;
            }
            if (model.EditField == null)
            {
                if (model.Price == 0)
                {
                    ViewBag.Details         = "Error, please ensure that the price field is a number. The product wasn't added.";
                    RedirectToAction("ProductList", "ProductList");

                }
                else
                {
                    ViewBag.Message         = "Added " + model.Name + model.Description + " " + filePathField + " " + model.Price + " " + model.Category;

                    Product record          = Product.ExecuteCreate(Int32.Parse(idOfCategoryField + ""),
                                                                                model.Name,
                                                                                model.Description,
                                                                                model.Price,
                                                                                filePathField, 1, 50, 51);
                    record.Insert();
                }
            }
            else
            {
                if (model.Price == 0)
                {
                    ViewBag.Details = "Error, please ensure that the price field is a number";
                    RedirectToAction("ProductList", "ProductList");
                }
                else
                {
                    Product record = Product.ExecuteCreate(Int32.Parse(idOfCategoryField + ""),
                                                                        model.Name,
                                                                        model.Description,
                                                                        model.Price,
                                                                        filePathField, 1, 50, 51);
                    record.Update(IDNew, record);
                }
            }

            return RedirectToAction("ProductList", "ProductList");

        }
        
        [HttpPost]
        public ActionResult ProductList(string idField)
        {
            int id                          = Int32.Parse(idField);
            ShoppingCartModels cart         = ShoppingCartModels.GetInstanceOfObject();
            Product productToBeUpdated      = Product.ExecuteCreate(id);
            int newQuantity                 = productToBeUpdated.Status + 1;
            int idfield                     = Int32.Parse(idField);
            Product product                 = Product.ExecuteCreate(productToBeUpdated.CategoryID,
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