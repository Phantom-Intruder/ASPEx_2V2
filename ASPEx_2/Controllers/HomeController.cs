using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class HomeController : Controller
    {
        #region Class fields
        public      static      int         IDNew               = 0;
        public      static      string      typeOfModel         = "";

        public      List<KeyValuePair<string, Product>>         listOfCategoryItemsUsed = 
                new List<KeyValuePair<string, Product>>();
        #endregion

        #region Display views
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShoppingCartView()
        {
            ShoppingCartModels      cart        = ShoppingCartModels.GetInstanceOfObject();

            return View(cart);
        }

        public ActionResult ProductList()
        {
            ProductModels       product         = new ProductModels();
          
            return View(product);
        }

        public ActionResult AdminView()
        {
            AdminViewModels         adminViewModels         = AdminViewModels.GetInstanceOfObject();

            return View(adminViewModels);
        }

        public ActionResult CheckoutView()
        {
            return View();
        }

        public ActionResult CategoryList()
        {
            CategoryModels      category        = new CategoryModels();
            
            return View(category);
        }

        public ActionResult EditCategoryView()
        {
           
            return View();
        }

        public ActionResult EditProductView()
        {
           
            return View();
        }
        #endregion

        #region Post methods
        [HttpPost]
        public ActionResult ShoppingCartView(string name)
        {
            if (name != null)
            {
                ShoppingCartModels      cart                = ShoppingCartModels.GetInstanceOfObject();

                cart.TotalPrice                             = cart.TotalPrice - cart.ProductsList[name].Price;
                cart.ProductsList.Remove(name);

                return View(cart);
            }
            else
            {
                //Save shopping cart code
                ShoppingCartModels      cart                = ShoppingCartModels.GetInstanceOfObject();
                int                     accountID           = Int32.Parse(Session["CurrentID"].ToString());
                Order                   order               = Order.ExecuteCreate(accountID, 1, 1, cart.TotalPrice);
                decimal                 subtotal            = 0;
                bool                    isPresent           = false;
                List<Order>             orderList           = Order.ListByAccountID(accountID);

                order.Insert();

                foreach (Order o in orderList)
                {
                    List<OrderItem>     orderItemList       = OrderItem.ListByOrderID(o.ID);

                    foreach (OrderItem item in orderItemList)
                    {
                        item.Delete();
                    }
                }

                foreach (string key in cart.ProductsList.Keys)
                {
                    Product             product             = cart.ProductsList[key];
                    subtotal                                = subtotal + product.Price;
                    OrderItem           orderItem           = OrderItem.ExecuteCreate(order.ID, product.ID, 1, product.Price, subtotal);
                    
                    if (!isPresent)
                    {
                        orderItem.Insert();
                    }
                }
                return View(cart);
            }
        }

        [HttpPost]
        public ActionResult AdminView(string categoryField, string productField, string saveTableField)
        {
            
            AdminViewModels         adminViewModels             = AdminViewModels.GetInstanceOfObject();

            if (categoryField != null)
            {
                adminViewModels.DestroyInstance();
                adminViewModels                                 = AdminViewModels.GetInstanceOfObject();
                adminViewModels.GetCategoriesInProduct(Int32.Parse(categoryField));
                ViewBag.typeOfModel                             = Constants.MODEL_CATEGORY;
                typeOfModel                                     = Constants.MODEL_CATEGORY;
                listOfCategoryItemsUsed                         = adminViewModels.listOfCategoryItemsUsed;
                return View(adminViewModels);
            }
            else if (productField != null)
            {
                adminViewModels.DestroyInstance();
                adminViewModels                                 = AdminViewModels.GetInstanceOfObject();
                adminViewModels.GetProduct(Int32.Parse(productField));
                ViewBag.typeOfModel                             = Constants.MODEL_PRODUCT;
                typeOfModel                                     = Constants.MODEL_PRODUCT;
                return View(adminViewModels);
            }
            else if (saveTableField != null)
            {
                //TODO: save table stuff
                DataTable               dataTable               = new DataTable();
                if (typeOfModel == Constants.MODEL_CATEGORY)
                {
                    typeOfModel                                 = "";
                    dataTable.Columns.Add(Constants.CATEGORY_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));
                    dataTable.Columns.Add(Constants.UNITS_SOLD_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));

                    foreach (var item in adminViewModels.listOfCategoryItemsUsed)
                    {
                        DataRow         dataRow                 = dataTable.NewRow();
                        dataRow[Constants.CATEGORY_COLUMN]                     = item.Value.Name;
                        dataRow[Constants.UNITS_SOLD_COLUMN]                   = item.Value.Status;
                        dataTable.Rows.Add(dataRow);
                    }

                    DataSet             dataSet                 = new DataSet();

                    dataSet.Tables.Add(dataTable);
                    ConvertToExcel(dataTable);
                }
                else if (typeOfModel == Constants.MODEL_PRODUCT)
                {
                    typeOfModel                                 = "";
                    dataTable.Columns.Add(Constants.CATEGORY_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));
                    dataTable.Columns.Add(Constants.UNITS_SOLD_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));

                    foreach (var item in adminViewModels.listOfCategoryItemsUsed)
                    {
                        DataRow         dataRow                 = dataTable.NewRow();

                        dataRow[Constants.CATEGORY_COLUMN]                     = item.Value.Name;
                        dataRow[Constants.UNITS_SOLD_COLUMN]                   = item.Value.Status;
                        dataTable.Rows.Add(dataRow);
                    }

                    DataSet             dataSet                 = new DataSet();

                    dataSet.Tables.Add(dataTable);
                    ConvertToExcel(dataTable);
                }
                else
                {
                    
                    dataTable.Columns.Add(Constants.CATEGORY_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));
                    dataTable.Columns.Add(Constants.UNITS_SOLD_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));

                    foreach (var item in adminViewModels.listOfCategoryItems)
                    {
                        DataRow         dataRow                 = dataTable.NewRow();

                        dataRow[Constants.CATEGORY_COLUMN]                     = item.Value.Name;
                        dataRow[Constants.UNITS_SOLD_COLUMN]                   = item.Value.Status;
                        dataTable.Rows.Add(dataRow);
                    }

                    DataSet             dataSet                 = new DataSet();

                    dataSet.Tables.Add(dataTable);
                    ConvertToExcel(dataTable);
                }
                return View(adminViewModels);
            }
            else
            {
                ViewBag.typeOfModel                             = Constants.CATEGORY_TYPE_NONE;
                return View(adminViewModels);
            }            
        }

        [HttpPost]
        public ActionResult ProductList(string idField)
        {
            int                     id                      = Int32.Parse(idField);
            ShoppingCartModels      cart                    = ShoppingCartModels.GetInstanceOfObject();
            Product                 productToBeUpdated      = Product.ExecuteCreate(id);
            int                     newQuantity             = productToBeUpdated.Status + 1;
            int                     idfield                 = Int32.Parse(idField);
            Product                 product                 = Product.ExecuteCreate(productToBeUpdated.CategoryID,
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

        [HttpPost]
        public ActionResult CheckoutView(string isUser)
        {
            if (isUser != null) {
                UserModel               userModel       = new UserModel();

                return PartialView("_UserView",userModel);
            }
            else
            {
                ShoppingCartModels      cart            = ShoppingCartModels.GetInstanceOfObject();

                return PartialView("_ProductView", cart);
            }
        }

        [HttpPost]
        public ActionResult EditCategoryView(string nameField, 
                                            string descriptionField, 
                                            string filePathField, 
                                            string isEditField)
        {
            if (isEditField == "false")
            {
                Category        record      = Category.ExecuteCreate(nameField, 
                                                                    descriptionField, 
                                                                    filePathField, 
                                                                    1, 
                                                                    50, 
                                                                    51);

                record.Insert();
            }
            else
            {
                Category        record      = Category.ExecuteCreate(nameField, 
                                                                    descriptionField, 
                                                                    filePathField, 
                                                                    1, 
                                                                    50, 
                                                                    51);

                record.Update(IDNew, record);
            }

            return PartialView("_AddedView");
        }

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
                ViewBag.Message                 = "Added " + nameField + descriptionField + " " + filePathField + " " + priceField + " " + categoryField;

                Product record                  = Product.ExecuteCreate(Int32.Parse(categoryField),
                                                                        nameField,
                                                                        descriptionField,
                                                                        Decimal.Parse(priceField),
                                                                        filePathField, 1, 50, 51);
                record.Insert();
            }
            else
            {
                Product record                 = Product.ExecuteCreate(Int32.Parse(categoryField),
                                                                        nameField,
                                                                        descriptionField,
                                                                        Decimal.Parse(priceField),
                                                                        filePathField, 1, 50, 51);
                record.Update(IDNew, record);
            }
            return PartialView("_AddedView");
        }
        #endregion

        #region Get methods
        [HttpGet]
        public ActionResult EditProductView(string id)
        {
            try
            {
                Product         product         = Product.ExecuteCreate(Int32.Parse(id));

                IDNew                           = Int32.Parse(id);
                ViewBag.Message                 = "Added " + product.Name;
                ViewBag.Name                    = product.Name;
                ViewBag.Description             = product.Description;
                ViewBag.Price                   = product.Price;
                ViewBag.FileName                = product.ImageName;
            }catch (ArgumentNullException n)
            {
                return View();
            }
                return View();
        }

        [HttpGet]
        public ActionResult EditCategoryView(string id)
        {
            try
            {
                Category        category        = Category.ExecuteCreate(Int32.Parse(id));

                IDNew = Int32.Parse(id);
                ViewBag.Name                    = category.Name;
                ViewBag.Description             = category.Description;
                ViewBag.FileName                = category.ImageName;
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
                Product         product         = Product.ExecuteCreate(Int32.Parse(id));

                IDNew = Int32.Parse(id);                
                ViewBag.Name                    = product.Name;
                ViewBag.Description             = product.Description;
                ViewBag.Price                   = product.Price;
                ViewBag.FileName                = product.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
        }

        [HttpGet]
        public ActionResult ShowCategoryView(string id)
        {
            CategoryProductModels       model;
            try
            {
                IDNew                                   = Int32.Parse(id);
                model                                   = new CategoryProductModels(IDNew); 
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View(model);
        }
        #endregion

        #region Helpers
        private void ConvertToExcel(DataTable dt)
        {
            string attachment = Constants.FILE_NAME;
            Response.ClearContent();
            Response.AddHeader(Constants.CONTENT_DISPOSITION, attachment);
            Response.ContentType = Constants.APPLICATION_VND ;
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        #endregion

    }
}