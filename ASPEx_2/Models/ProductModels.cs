using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using ECommerce.Tables.Utility.System;

namespace ASPEx_2.Models
{
	public class ProductModels
    {
        #region Properties
        private List<ECommerce.Tables.Content.Product> ProductsList { get; set; }
        private List<ECommerce.Tables.Content.Category> CategoryList { get; set; }


        [Required]
        [Display(Name = "Name")]
		[StringLength(30)]
		public string Name { get; set; }

        [Required]
		[StringLength(35)]
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
		/// <param name="product"></param>
		public void CreateProduct(Product product)
		{
			this.Name				= product.Name;
			this.Description		= product.Description;
			this.Price				= product.Price;
			this.FilePath			= product.ImageName;
			this.EditField			= "true";
		}

		/// <summary>
		/// Create and insert product
		/// </summary>
		/// <param name="model"></param>
		/// <param name="filePathField"></param>
		/// <param name="idOfCategoryField"></param>
		private void CreateAndInsertNewProduct(string filePathField, int idOfCategoryField)
		{

			Product			record			= Product.ExecuteCreate(Int32.Parse(idOfCategoryField + ""),
																	this.Name,
																	this.Description,
																	this.Price,
																	filePathField, 1, 50, 51);
			record.Insert();
		}

		/// <summary>
		/// Copy selected image into filestore
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private string CopyFileIntoFilestore()
		{
			string					filePathField;
			HttpPostedFileBase		file						= this.FileUpload;

			string					directoryWithFolder			= Volume.Toolkit.Paths.PathUtility.CombinePaths(Config.StorageUrl, Config.FOLDER_PRODUCT);
			string[]				directories					= Directory.GetDirectories(directoryWithFolder);
			int						folderNumber				= directories.Length;
			folderNumber										= folderNumber + 1;
			string					targetPath					= directoryWithFolder + "\\" + folderNumber;
			string					destFile					= Volume.Toolkit.Paths.PathUtility.CombinePaths(targetPath, "" + folderNumber + ".png");
			if (!System.IO.Directory.Exists(targetPath))
			{
				System.IO.Directory.CreateDirectory(targetPath);
				file.SaveAs(destFile);
			}
			else
			{
				Console.WriteLine("Source path does not exist!");
			}
			filePathField								= "/" + Config.FOLDER_PRODUCT + "/" + folderNumber + "/" + folderNumber + ".png";
			return filePathField;
		}

		/// <summary>
		/// Get the category ID of a particular product
		/// </summary>
		/// <param name="model"></param>
		/// <param name="idOfCategoryField"></param>
		/// <param name="index"></param>
		private void SetCategoryID(ref int idOfCategoryField, ref int index)
		{
			foreach (Category category in this.GetCategoryList())
			{
				if (category.Name.ToString().Equals(this.Category))
				{
					idOfCategoryField		= category.ID;
				}
				index						= index + 1;
			}
		}

		/// <summary>
		/// Show the product using ID
		/// </summary>
		/// <param name="id"></param>
		public void ShowProductFromId(string id)
		{
			Product product = Product.ExecuteCreate(Int32.Parse(id));
			if (product != null)
			{
				this.Name = product.Name;
				this.Description = product.Description;
				this.Price = product.Price;
				this.FilePath = product.ImageName;
			}
		}

		/// <summary>
		/// Custom validation for model
		/// </summary>
		/// <returns></returns>
		public bool Validation()
		{
			if ((this.EditField == null) && (this.FileUpload == null))
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Save the record
		/// </summary>
		/// <param name="IDNew"></param>
		public void Save(int IDNew)
		{
			string		filePathField			= "";
			int			idOfCategoryField		= 0;
			int			index					= 1;

			this.SetCategoryID(ref idOfCategoryField, ref index);
			if (this.FileUpload != null)
			{
				filePathField					= this.CopyFileIntoFilestore();
			}
			else
			{
				filePathField					= this.FilePath;
			}
			if (this.EditField == null)
			{
				this.CreateAndInsertNewProduct(filePathField, idOfCategoryField);

			}
			else
			{

				Product			record			= Product.ExecuteCreate(Int32.Parse(idOfCategoryField + ""),
																					this.Name,
																					this.Description,
																					this.Price,
																					filePathField, 1, 50, 51);
				record.Update(IDNew, record);

			}
		}

		/// <summary>
		/// Adds a selected product to the cart
		/// </summary>
		/// <param name="idField"></param>
		public void AddProductToShoppingCart(string idField)
		{
			int						id						= Int32.Parse(idField);
			ShoppingCartModels		cart					= ShoppingCartModels.GetInstanceOfObject();
			Product					productToBeUpdated		= Product.ExecuteCreate(id);
			int						newQuantity				= productToBeUpdated.Status + 1;
			int						idfield					= Int32.Parse(idField);
			Product					product					= Product.ExecuteCreate(productToBeUpdated.CategoryID,
																					productToBeUpdated.Name,
																					productToBeUpdated.Description,
																					productToBeUpdated.Price,
																					productToBeUpdated.ImageName,
																					newQuantity,
																					productToBeUpdated.CreatedAccountID,
																					productToBeUpdated.ModifiedAccountID);

			cart.AddProductToCart(id);
			product.Update(idfield, product);
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
            foreach (Category item in CategoryList)
            {
                CategoryNamesList.Add(item.Name);
            }
            return this.CategoryNamesList;
        }
		#endregion
	}
}