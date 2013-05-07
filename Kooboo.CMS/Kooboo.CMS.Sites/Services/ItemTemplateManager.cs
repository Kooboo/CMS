#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
using Kooboo.IO;
using Kooboo.Globalization;
namespace Kooboo.CMS.Sites.Services
{
    public abstract class ItemTemplateManager
    {
        #region Properties
        protected abstract string BasePath { get; }
        protected virtual string FileExtension
        {
            get
            {
                return ".zip";
            }
        }
        #endregion

        #region AllCategories
        public virtual IEnumerable<string> AllCategories()
        {
            if (Directory.Exists(BasePath))
            {
                var dirInfo = new DirectoryInfo(BasePath);
                return dirInfo.EnumerateDirectories()
                    .Where(it => (it.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    .Select(it => it.Name);
            }
            return new string[0];
        }

        #endregion

        #region All
        public virtual IEnumerable<ItemTemplate> All()
        {
            return All("").Concat(AllCategories().SelectMany(it => All(it)));
        }
        public virtual IEnumerable<ItemTemplate> All(string category)
        {
            string physicalPath = GetCategoryPhysicalPath(category);
            if (Directory.Exists(physicalPath))
            {
                return Directory.EnumerateFiles(physicalPath, "*" + FileExtension).Select(it => GetItemTemplate(category, Path.GetFileNameWithoutExtension(it)));
            }
            return new ItemTemplate[0];
        }
        #endregion

        #region GetItemTemplate
        private string GetCategoryPhysicalPath(string category)
        {
            string physicalPath = BasePath;
            if (!string.IsNullOrEmpty(category))
            {
                physicalPath = Path.Combine(BasePath, category);
            }
            return physicalPath;
        }
        public virtual ItemTemplate GetItemTemplate(string category, string templateName)
        {
            var physicalPath = GetCategoryPhysicalPath(category);
            var itemFile = Path.Combine(physicalPath, templateName + FileExtension);
            if (File.Exists(itemFile))
            {
                ItemTemplate itemTemplate = new ItemTemplate() { TemplateName = templateName, Category = category };
                itemTemplate.TemplateFile = itemFile;
                var thumbnail = Path.Combine(physicalPath, templateName + ".png");
                if (File.Exists(thumbnail))
                {
                    itemTemplate.Thumbnail = UrlUtility.GetVirtualPath(thumbnail);
                }
                return itemTemplate;
            }
            return null;
        }
        #endregion

        #region AddItemTemplate
        public virtual void AddItemTemplate(string category, string templateName, Stream templateStream, Stream thumbnailStream)
        {
            if (GetItemTemplate(category, templateName) != null)
            {
                throw new Exception("The item already exists.".Localize());
            }
            var physicalPath = GetCategoryPhysicalPath(category);
            Kooboo.IO.IOUtility.EnsureDirectoryExists(physicalPath);
            var itemFile = Path.Combine(physicalPath, templateName + FileExtension);
            var thumbnail = Path.Combine(physicalPath, templateName + ".png");
            templateStream.SaveAs(itemFile);
            if (thumbnailStream != null && itemFile.Length > 0)
            {
                MemoryStream ms = new MemoryStream();
                Kooboo.Drawing.ImageTools.ResizeImage(thumbnailStream, ms, System.Drawing.Imaging.ImageFormat.Png, 100, 100, true, 100);
                ms.Position = 0;
                ms.SaveAs(thumbnail);
            }
        }
        #endregion

        #region DeleteItemTemplate
        public virtual void DeleteItemTemplate(ItemTemplate itemTemplate)
        {
            if (GetItemTemplate(itemTemplate.Category, itemTemplate.TemplateName) != null)
            {
                var physicalPath = GetCategoryPhysicalPath(itemTemplate.Category);
                var itemFile = Path.Combine(physicalPath, itemTemplate.TemplateName + FileExtension);
                if (File.Exists(itemFile))
                {
                    File.Delete(itemFile);
                }
                var thumbnail = Path.Combine(physicalPath, itemTemplate.TemplateName + ".png");
                if (File.Exists(thumbnail))
                {
                    File.Delete(thumbnail);
                }
            }
        }
        #endregion
    }
}
