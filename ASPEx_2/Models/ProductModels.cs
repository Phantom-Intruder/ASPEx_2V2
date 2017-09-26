using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ECommerce.Tables;
using ECommerce.Tables.Content;
using System.IO;

namespace ASPEx_2.Models
{
    public class ProductModels
    {
        #region Properties
        private List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        private List<ECommerce.Tables.Content.Category> CategoryList { get; set; }


        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }

        [Required]
        [Display(Name = "Path")]
        public string FilePath { get; set; }

        [Required]
        public string EditField { get; set; }
		#endregion

		#region Model methods

		/// <summary>
		/// Create product using model
		/// </summary>
		/// <param name="productModels"></param>
		/// <param name="product"></param>
		public void CreateProduct(ProductModels productModels, Product product)
		{
			productModels.Name				= product.Name;
			productModels.Description		= product.Description;
			productModels.Price				= product.Price;
			productModels.FilePath			= product.ImageName;
			productModels.EditField			= "true";
		}

		/// <summary>
		/// Create and insert product
		/// </summary>
		/// <param name="model"></param>
		/// <param name="filePathField"></param>
		/// <param name="idOfCategoryField"></param>
		public void CreateAndInsertNewProduct(ProductModels model, string filePathField, int idOfCategoryField)
		{

			Product			record			= Product.ExecuteCreate(Int32.Parse(idOfCategoryField + ""),
																	model.Name,
																	model.Description,
																	model.Price,
																	filePathField, 1, 50, 51);
			record.Insert();
		}

		/// <summary>
		/// Copy selected image into filestore
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public string CopyFileIntoFilestore(ProductModels model)
		{
			string filePathField;
			var			file				= model.FileUpload;

			var			directories			= Directory.GetDirectories(@"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Product");
			int			folderNumber		= directories.Length;
			folderNumber					= folderNumber + 1;
			string		targetPath			= @"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Product\" + folderNumber;
			string		destFile			= System.IO.Path.Combine(targetPath, "" + folderNumber + ".png");
			if (!System.IO.Directory.Exists(targetPath))
			{
				System.IO.Directory.CreateDirectory(targetPath);
				file.SaveAs(destFile);
			}
			else
			{
				Console.WriteLine("Source path does not exist!");
			}
			filePathField					= @"/Product/" + folderNumber + "/" + folderNumber + ".png";
			return filePathField;
		}

		/// <summary>
		/// Get the category ID of a particular product
		/// </summary>
		/// <param name="model"></param>
		/// <param name="idOfCategoryField"></param>
		/// <param name="index"></param>
		public void SetCategoryID(ProductModels model, ref int idOfCategoryField, ref int index)
		{
			foreach (string name in model.GetCategoryNamesList())
			{
				if (name.ToString().Equals(model.Category))
				{
					idOfCategoryField		= index;
				}
				index						= index + 1;
			}
		}
		#endregion

		#region Class members
		[Required]
        private List<string> CategoryNamesList      = new List<string>();
        #endregion

        #region Class constructor 
        public ProductModels()
        {
            this.ProductsList       = ECommerce.Tables.Content.Product.List();
            this.CategoryList       = ECommerce.Tables.Content.Category.List();
        }
        #endregion

        #region Getters
        public List<ECommerce.Tables.Content.Product> GetProductsList()
        {
            return this.ProductsList;
        }

        public List<ECommerce.Tables.Content.Category> GetCategoryList()
        {
            return this.CategoryList;
        }

        public List<string> GetCategoryNamesList()
        {
            foreach (var item in CategoryList)
            {
                CategoryNamesList.Add(item.Name);
            }
            return this.CategoryNamesList;
        }
        #endregion
    }
}