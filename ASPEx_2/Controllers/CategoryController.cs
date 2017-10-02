﻿using ASPEx_2.Helpers;
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

		#region Properties

		public CategoryModels TempSession
		{
			get
			{
				CategoryModels					result				=  null;

				if(Session[Constants.SESSION_NAME_CATEGORY] != null)
				{
					result												= Session[Constants.SESSION_NAME_CATEGORY] as CategoryModels;
				}

				return  result;
			}
			set
			{
				Session[Constants.SESSION_NAME_CATEGORY]				= value;
			}
		}

		#endregion
        
        #region Display views
        // GET: CategoryList
        public ActionResult List()
        {
            return View(CategoryModels.List());
        }

       public ActionResult Edit(int? id)
        {
			CategoryModels				result					= null;

			if(this.TempSession != null &&
				id.HasValue &&
				this.TempSession.ID == id.Value)
			{
				result = this.TempSession;
				CheckIfIdHasValue(id, result);
			}
			else
			{
				Session.Remove(Constants.SESSION_NAME_CATEGORY);				
				result											= CategoryModels.ExecuteCreate(id);
				CheckIfIdHasValue(id, result);
				this.TempSession								= result;
			}

			if(result == null)
			{
				return new HttpNotFoundResult();
			}

            return View(result);
        }

		private static void CheckIfIdHasValue(int? id, CategoryModels result)
		{
			if (id.HasValue)
			{
				result.EditField = "true";
			}
		}

		#endregion

		#region Post methods
		[ValidateAntiForgeryToken]
		[HttpPost]
		public ActionResult Edit(CategoryModels model)
        {
			if(ModelState.IsValid)
			{
				this.TempSession.Sync(model);

				if(this.TempSession.Validate(ModelState))
				{
					this.TempSession.Save();
					Session.Remove(Constants.SESSION_NAME_CATEGORY);
					return RedirectToAction("List");
				}
			}
			
            return View(this.TempSession);
        }
		#endregion

		#region Get methods
		[HttpGet]
        public ActionResult Delete(string id)
        {
            Category.Delete(Int32.Parse(id));

            return View();
        }

        [HttpGet]
        public ActionResult Details(string id)
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