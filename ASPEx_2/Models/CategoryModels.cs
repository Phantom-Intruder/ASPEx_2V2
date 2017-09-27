﻿using ECommerce.Tables.Content;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;

namespace ASPEx_2.Models
{
    public class CategoryModels
    {
        #region Properties
        private      List<ECommerce.Tables.Content.Category>         CategoriesList { get; set; }

        [Required]
        [Display(Name = "Name")]
		[StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        public HttpPostedFileBase FileUpload { get; set; }

        public string FilePath { get; set; }

        [Required]
        public string EditField { get; set; }
		#endregion

		#region Model methods

		/// <summary>
		/// Create a new record and insert into database
		/// </summary>
		/// <param name="model"></param>
		/// <param name="filePathField"></param>
		private void CreateNewRecord(CategoryModels model, string filePathField)
		{
			Category		record		= Category.ExecuteCreate(model.Name,
																 model.Description,
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
		private string CopyFileIntoFilestore(CategoryModels model)
		{
			string		filePathField;
			var			file				= model.FileUpload;

			var			directories			= Directory.GetDirectories(@"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Category");
			int			folderNumber		= directories.Length;
			folderNumber					= folderNumber + 1;
			string		targetPath			= @"C:\inetpub\wwwroot\ASP\ASPEx_2\Filestore\Category\" + folderNumber;
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


			filePathField					= @"/Category/" + folderNumber + "/" + folderNumber + ".png";
			return filePathField;
		}

		/// <summary>
		/// Insert data about new category
		/// </summary>
		/// <param name="id"></param>
		/// <param name="categoryModels"></param>
		public void EditCategoryOfID(string id, CategoryModels categoryModels)
		{
			Category		category		= Category.ExecuteCreate(Int32.Parse(id));

			categoryModels.Name				= category.Name;
			categoryModels.Description		= category.Description;
			categoryModels.FilePath			= category.ImageName;
			categoryModels.EditField		= "true";
		}
		public bool Validation()
		{
			if((this.EditField == null) && (this.FileUpload == null)){
				return false;
			}
			return true; 
		}

		public void Save(int IDNew)
		{
			string		filePathField		= "";
			if (this.FileUpload != null)
			{
				filePathField				= this.CopyFileIntoFilestore(this);
			}
			else
			{
				filePathField				= this.FilePath;
			}
			if (this.EditField == null)
			{
				this.CreateNewRecord(this, filePathField);
			}
			else
			{
				Category		record		= Category.ExecuteCreate(this.Name,
																	 this.Description,
																	 filePathField,
																	 1,
																	 50,
																	 51);

				record.Update(IDNew, record);
			}
		}

		#endregion

		#region Class constructor
		public CategoryModels()
        {
            this.CategoriesList         = ECommerce.Tables.Content.Category.List();
        }
        #endregion

        #region Getters
        public List<ECommerce.Tables.Content.Category> GetCategoriesList()
        {
            return this.CategoriesList;
        }
        #endregion
    }
}