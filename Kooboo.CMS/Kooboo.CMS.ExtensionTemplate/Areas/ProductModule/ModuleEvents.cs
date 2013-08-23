#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common;
namespace Kooboo.CMS.ExtensionTemplate.Areas.ProductModule
{
    public static class ColumnExtensions
    {
        public static Column AddColumn(this Schema schema, string columnName, string controlType, DataType dataType,
          string label = null, bool showInGrid = false, bool summarize = false, string tooltip = null)
        {
            Column column = new Column()
            {
                Name = columnName,
                Label = label,
                ControlType = controlType,
                DataType = dataType,
                ShowInGrid = showInGrid,
                Summarize = summarize,
                Tooltip = tooltip
            };

            schema.AddColumn(column);
            return column;
        }
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IModuleEvents), Key = ModuleAreaRegistration.ModuleName)]
    public class ModuleEvents : IModuleEvents
    {
        private SchemaManager _schemaManager;
        private TextFolderManager _textFolderManager;
        public ModuleEvents(SchemaManager schemaManager, TextFolderManager textFolderManager)
        {
            this._schemaManager = schemaManager;
            this._textFolderManager = textFolderManager;
        }
        public void OnExcluded(Site site)
        {
            var repository = site.AsActual().GetRepository();
            if (repository != null)
            {
                TextFolder productFolder = new TextFolder(repository, "Product");
                if (productFolder.AsActual() != null)
                {
                    _textFolderManager.Remove(repository, productFolder);
                }
                Schema productSchema = new Schema(repository, "Product");
                if (productSchema.AsActual() != null)
                {
                    _schemaManager.Remove(repository, productSchema);
                }

            }
        }

        public void OnIncluded(Site site)
        {
            var repository = site.AsActual().GetRepository();
            if (repository != null)
            {
                //import the content types. the zip file contains "Category" content type.
                //var contentTypePath = new ModulePathHelper(ModuleAreaRegistration.ModuleName).GetModuleInstallationFilePath("ContentType.zip");
                //if (File.Exists(contentTypePath.PhysicalPath))
                //{
                //    using (FileStream fs = new FileStream(contentTypePath.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                //    {
                //        _schemaManager.Import(repository, fs, true);
                //    }
                //}
                Schema productSchema = new Schema(repository, "Product");
                productSchema.AddColumn("ProductName", "TextBox", DataType.String, "", true, true);
                productSchema.AddColumn("ProductDetail", "Tinymce", DataType.String, "", false, true);
                if (productSchema.AsActual() == null)
                {
                    _schemaManager.Add(repository, productSchema);
                }

                TextFolder productFolder = new TextFolder(repository, "Product")
                {
                    SchemaName = "Product"
                };
                if (productFolder.AsActual() == null)
                {
                    _textFolderManager.Add(repository, productFolder);
                }
            }
        }


        public void OnInstalling(ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module installing.
            // Installing UI template is defined in the module.config
        }

        public void OnUninstalling(ControllerContext controllerContext)
        {
            // Add code here that will be executed when the module uninstalling.
            // To use custom UI during uninstalling, define the view location in the module.config
        }
    }
}
