using ASPEx_2.Models;
using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ASPEx_2.Controllers
{
    public class AdminViewController : Controller
    {
        #region Class fields
        public static string typeOfModel        = "";

        public List<KeyValuePair<string, Product>> listOfCategoryItemsUsed =
                new List<KeyValuePair<string, Product>>();
        #endregion

        #region Display views
        // GET: AdminView
        public ActionResult AdminView()
        {
            AdminViewModels adminViewModels         = AdminViewModels.GetInstanceOfObject();

            return View(adminViewModels);
        }
        #endregion

        #region Post methods

        [HttpPost]
        public ActionResult AdminView(string categoryField, string productField, string saveTableField)
        {

            AdminViewModels adminViewModels                         = AdminViewModels.GetInstanceOfObject();

            if (categoryField != null)
            {
                adminViewModels.DestroyInstance();
                adminViewModels                                     = AdminViewModels.GetInstanceOfObject();
                adminViewModels.GetCategoriesInProduct(Int32.Parse(categoryField));
                ViewBag.typeOfModel                                 = Constants.MODEL_CATEGORY;
                typeOfModel                                         = Constants.MODEL_CATEGORY;
                listOfCategoryItemsUsed                             = adminViewModels.GetListOfCategoryItemsUsed();
                return View(adminViewModels);
            }
            else if (productField != null)
            {
                adminViewModels.DestroyInstance();
                adminViewModels                                     = AdminViewModels.GetInstanceOfObject();
                adminViewModels.GetProduct(Int32.Parse(productField));
                ViewBag.typeOfModel                                 = Constants.MODEL_PRODUCT;
                typeOfModel                                         = Constants.MODEL_PRODUCT;
                return View(adminViewModels);
            }
            else if (saveTableField != null)
            {
                //TODO: save table stuff
                DataTable dataTable                                 = new DataTable();
                if (typeOfModel == Constants.MODEL_CATEGORY)
                {
                    typeOfModel                                     = "";
                    dataTable.Columns.Add(Constants.CATEGORY_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));
                    dataTable.Columns.Add(Constants.UNITS_SOLD_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));

                    foreach (var item in adminViewModels.GetListOfCategoryItemsUsed())
                    {
                        DataRow dataRow                             = dataTable.NewRow();
                        dataRow[Constants.CATEGORY_COLUMN]          = item.Value.Name;
                        dataRow[Constants.UNITS_SOLD_COLUMN]        = item.Value.Status;
                        dataTable.Rows.Add(dataRow);
                    }

                    DataSet dataSet                                 = new DataSet();

                    dataSet.Tables.Add(dataTable);
                    ConvertToExcel(dataTable);
                }
                else if (typeOfModel == Constants.MODEL_PRODUCT)
                {
                    typeOfModel                                     = "";
                    dataTable.Columns.Add(Constants.CATEGORY_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));
                    dataTable.Columns.Add(Constants.UNITS_SOLD_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));

                    foreach (var item in adminViewModels.GetListOfCategoryItemsUsed())
                    {
                        DataRow dataRow                             = dataTable.NewRow();

                        dataRow[Constants.CATEGORY_COLUMN]          = item.Value.Name;
                        dataRow[Constants.UNITS_SOLD_COLUMN]        = item.Value.Status;
                        dataTable.Rows.Add(dataRow);
                    }

                    DataSet dataSet                                 = new DataSet();

                    dataSet.Tables.Add(dataTable);
                    ConvertToExcel(dataTable);
                }
                else
                {

                    dataTable.Columns.Add(Constants.CATEGORY_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));
                    dataTable.Columns.Add(Constants.UNITS_SOLD_COLUMN, Type.GetType(Constants.DATA_TYPE_STRING));

                    foreach (var item in adminViewModels.GetListOfCategoryItems())
                    {
                        DataRow dataRow                             = dataTable.NewRow();

                        dataRow[Constants.CATEGORY_COLUMN]          = item.Value.Name;
                        dataRow[Constants.UNITS_SOLD_COLUMN]        = item.Value.Status;
                        dataTable.Rows.Add(dataRow);
                    }

                    DataSet dataSet                                 = new DataSet();

                    dataSet.Tables.Add(dataTable);
                    ConvertToExcel(dataTable);
                }
                return View(adminViewModels);
            }
            else
            {
                ViewBag.typeOfModel                                 = Constants.CATEGORY_TYPE_NONE;
                return View(adminViewModels);
            }
        }
        #endregion

        #region Helpers
        private void ConvertToExcel(DataTable dt)
        {
            string attachment           = Constants.FILE_NAME;
            Response.ClearContent();
            Response.AddHeader(Constants.CONTENT_DISPOSITION, attachment);
            Response.ContentType        = Constants.APPLICATION_VND;
            string tab                  = "";
            foreach (DataColumn dc in dt.Columns)
            {
                Response.Write(tab + dc.ColumnName);
                tab                     = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab                     = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    Response.Write(tab + dr[i].ToString());
                    tab                 = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
        }

        #endregion
    }
}