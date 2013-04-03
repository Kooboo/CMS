using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Url;
namespace Kooboo.CMS.Sites.Services
{
    public abstract class ItemTemplateManager
    {
        protected abstract string BasePath { get; }
        protected virtual string FileExtension
        {
            get
            {
                return ".zip";
            }
        }

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

        public virtual IEnumerable<ItemTemplate> All()
        {
            return All("").Concat(AllCategories().SelectMany(it => All(it)));
        }
        public virtual IEnumerable<ItemTemplate> All(string category)
        {
            string physicalPath = GetPhysicalPath(category);
            if (Directory.Exists(physicalPath))
            {
                return Directory.EnumerateFiles(physicalPath, "*" + FileExtension).Select(it => GetItemTemplate(category, Path.GetFileNameWithoutExtension(it)));
            }
            return new ItemTemplate[0];
        }

        private string GetPhysicalPath(string category)
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
            var physicalPath = GetPhysicalPath(category);
            if (Directory.Exists(physicalPath))
            {
                ItemTemplate itemTemplate = new ItemTemplate() { TemplateName = templateName, Category = category };
                itemTemplate.TemplateFile = Path.Combine(physicalPath, templateName + FileExtension);
                var thumbnail = Path.Combine(physicalPath, templateName + ".png");
                if (File.Exists(thumbnail))
                {
                    itemTemplate.Thumbnail = UrlUtility.GetVirtualPath(thumbnail);
                }                
                return itemTemplate;
            }
            return null;
        }
    }
}
