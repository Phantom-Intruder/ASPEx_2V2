﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommerce.Tables.Content;
using ASPEx_2.Models;

namespace ASPEx_2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProductList()
        {
            ViewBag.Message     = "Your application description page.";
            ProductModels product = new ProductModels();

            return View(product);
        }

        public ActionResult CategoryList()
        {
            ViewBag.Message     = "Your contact page.";
            CategoryModels category = new CategoryModels();
            return View(category);
        }

        public ActionResult EditCategoryView()
        {
            ViewBag.Message     = "Your application description page.";
            return View();
        }

        [HttpPost]
        public ActionResult EditCategoryView(string nameField, string descriptionField, string filePathField)
        {
            ViewBag.Message     = "Added " + nameField + descriptionField + " " + filePathField;

            Category record     = Category.ExecuteCreate(nameField, descriptionField, filePathField, 1, 50, 51);
            record.Insert();
            return PartialView("_AddedView");
        }

        [HttpPost]
        public ActionResult EditProductView(string nameField, string descriptionField, string filePathField, string priceField, string categoryField)
        {
            ViewBag.Message     = "Added " + nameField + descriptionField + " " + filePathField + " " + priceField + " " + categoryField ;

            Product record      = Product.ExecuteCreate(Int32.Parse(categoryField), nameField, descriptionField, Decimal.Parse(priceField), filePathField, 1, 50, 51);
            record.Insert();
            return PartialView("_AddedView");
        }

        [HttpGet]
        public ActionResult EditProductView(string id)
        {
            try
            {
                Product product     = Product.ExecuteCreate(Int32.Parse(id));
                ViewBag.Message     = "Added " + product.Name+ " " + product.Description;
                ViewBag.Name        = product.Name;
                ViewBag.Description = product.Description;
                ViewBag.Price       = product.Price;
                ViewBag.FileName    = product.ImageName;
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
                Category category     = Category.ExecuteCreate(Int32.Parse(id));
                ViewBag.Name        = category.Name;
                ViewBag.Description = category.Description;
                ViewBag.FileName    = category.ImageName;
            }
            catch (ArgumentNullException n)
            {
                return View();
            }
            return View();
        }

        public ActionResult EditProductView()
        {
            ViewBag.Message     = "Your application description page.";

            return View();
        }
    }
}