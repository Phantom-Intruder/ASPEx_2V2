using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using ECommerce.Tables.Utility.System;
using ASPEx_2.Helpers;
using System.Web.Mvc;
using ASPEx_2.Controllers;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
		#region Members
		private int						id				= Constants.DEFAULT_VALUE_INT;
		private string					name			= String.Empty;
		private string					description		= String.Empty;
		private string					fileName		= String.Empty;
		private Category				entity			= null;
		private HttpPostedFileBase		fileBase		= null;
		#endregion

        #region Properties
		[Key]
		public int ID
		{
			get { return this.id; }
		}

        [Required]
        [Display(Name = "Name")]
		[StringLength(30)]
        public string Name {
			get { return this.name; }
			set { this.name = value; }
		}

        [Required]
		[Display(Name = "Description")]
		[DataType(DataType.MultilineText)]
		public string Description
		{
			get { return this.description; }
			set { this.description = value; }
		}

        public HttpPostedFileBase FileUpload {
			get { return this.fileBase; }
			set { this.fileBase = value; }
		}

		[Required]
        public string FilePath {
			get { return this.fileName; }
			set { this.fileName = value; }
		}

        public string EditField { get; set; }
		#endregion

		#region Model methods

		/// <summary>
		/// Create a new record and insert into database
		/// </summary>
		/// <param name="model"></param>
		/// <param name="filePathField"></param>
		private void CreateNewRecord(string filePathField)
		{
			Category		record		= Category.ExecuteCreate(this.Name,
																 this.Description,
																 filePathField,
																 1,
																 50,
																 51);

			record.Insert();
		}

		/// <summary>
		/// Takes selected picture and copies it to the file store 
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		private string CopyFileIntoFilestore()
		{
			string						filePathField;
			HttpPostedFileBase			file						= this.fileBase;
			
			string						directoryWithFolder			= Volume.Toolkit.Paths.PathUtility.CombinePaths(Config.StorageUrl, Config.FOLDER_CATEGORY);
			string[]					directories					= Directory.GetDirectories(directoryWithFolder);
			int							folderNumber				= directories.Length;
			folderNumber											= folderNumber + 1;
			string						targetPath					= directoryWithFolder + "\\"+ folderNumber;
			string						destFile					= Volume.Toolkit.Paths.PathUtility.CombinePaths(targetPath, "" + folderNumber + ".png");
			if (!System.IO.Directory.Exists(targetPath))
			{
				System.IO.Directory.CreateDirectory(targetPath);
				file.SaveAs(destFile);
			}
			else
			{
				Console.WriteLine("Source path does not exist!");
			}


			filePathField											= "/" + Config.FOLDER_CATEGORY + "/" + folderNumber + "/" + folderNumber + ".png";
			return filePathField;
		}

		/// <summary>
		/// Insert data about new category
		/// </summary>
		/// <param name="id"></param>
		public void EditCategoryOfID(string id)
		{
			Category		category		= Category.ExecuteCreate(Int32.Parse(id));

			this.Name						= category.Name;
			this.Description				= category.Description;
			this.FilePath					= category.ImageName;
			this.EditField					= "true";
		}

		#endregion

		#region Class constructor
		public CategoryModels()
        {
            
        }

		private CategoryModels(Category entity)
		{
			this.id				= entity.ID;
			this.name			= entity.Name;
			this.description	= entity.Description;
			this.fileName		= entity.ImageName;
			this.entity			= entity;

		}

        #endregion

		#region Execute Create

		public static CategoryModels ExecuteCreate(int? id)
		{
			CategoryModels				result				= null;

			if(id.HasValue)
			{
				Category					entity				= Category.ExecuteCreate(id.Value);

				if(entity != null)
				{
					result										= new CategoryModels(entity);
				}
			}
			else
			{
				result											= new CategoryModels();
			}

			return result;
		}

		#endregion

		#region List 

		public static List<CategoryModels> List()
		{
			List<CategoryModels>					result				= new List<CategoryModels>();
			List<Category>							list				= Category.List();

			foreach (Category item in list)
			{
				result.Add(new CategoryModels(item));
			}

			return result;
		}

		#endregion

		#region Methods

		#region Validate
		
		public bool Validate(ModelStateDictionary state)
		{
			bool			result					= true;

			if(String.IsNullOrEmpty(this.fileName))
			{
				result								&= false;
				state.AddModelError("Filename", "Please upload an image");
			}

			return  true;
		}

		#endregion

		#region Sync

		public void Sync(CategoryModels model)
		{
			this.name				= model.Name;
			this.description		= model.Description;
			this.fileBase			= model.FileUpload;
			this.fileName			= model.FilePath;
		}

		#endregion

		#region Save

		public void Save()
		{
			string		filePathField		= String.Empty;
			if (this.fileBase != null)
			{
				filePathField				= this.CopyFileIntoFilestore();
			}
			else
			{
				filePathField				= this.FilePath;
			}
			if (this.EditField == null)
			{
				this.CreateNewRecord(filePathField);
			}
			else
			{
				Category		record		= Category.ExecuteCreate(this.Name,
																	 this.Description,
																	 filePathField,
																	 1,
																	 50,
																	 51);

				record.Update(this.ID, record);
			}
		}

		#endregion 


		#endregion

        #region Getters
        public List<ECommerce.Tables.Content.Category> GetCategoriesList()
        {
            return ECommerce.Tables.Content.Category.List();
        }
        #endregion
    }
}