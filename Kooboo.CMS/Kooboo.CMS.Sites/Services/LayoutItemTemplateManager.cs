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
using Kooboo.CMS.Sites.Models;
using Ionic.Zip;
using System.IO;
using Kooboo;
using Kooboo.Common.Web;
using Kooboo.Common.Misc;
namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(LayoutItemTemplateManager))]
    public class LayoutItemTemplateManager : ItemTemplateManager
    {
        protected override string BasePath
        {
            get { return  AreaHelpers.CombineAreaFilePhysicalPath("Sites", "Templates", "Layout"); }
        }

        public virtual IEnumerable<LayoutSample> GetLayoutSamples(string engineName)
        {
            var itemTemplates = this.All(engineName);

            foreach (var item in itemTemplates)
            {
                using (ZipFile zipFile = new ZipFile(item.TemplateFile))
                {
                    var settingEntryName = PathResource.SettingFileName;
                    if (!zipFile.ContainsEntry(settingEntryName))
                    {
                        throw new Exception("The layout item template is invalid, setting.config is required.");
                    }
                    Layout layout = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zipFile[settingEntryName].Extract(ms);
                        ms.Position = 0;
                        layout = (Layout)DataContractSerializationHelper.Deserialize(typeof(Layout), null, ms);
                    }
                    var templateEntryName = layout.TemplateFileName;
                    if (!zipFile.ContainsEntry(templateEntryName))
                    {
                        throw new Exception(string.Format("The layout item template is invalid.{0} is requried.", templateEntryName));
                    }
                    LayoutSample sample = new LayoutSample() { Name = item.TemplateName, ThumbnailVirtualPath = item.Thumbnail };
                    using (MemoryStream ms = new MemoryStream())
                    {
                        zipFile[templateEntryName].Extract(ms);
                        ms.Position = 0;
                        using (StreamReader sr = new StreamReader(ms))
                        {
                            sample.Template = sr.ReadToEnd();
                        }
                    }
                    yield return sample;
                }
            }
        }

        public virtual LayoutSample GetDefaultLayoutSample(string engineName)
        {
            var defaultLayout = GetLayoutSample(engineName, "default");
            if (defaultLayout == null)
            {
                defaultLayout = GetLayoutSamples(engineName).FirstOrDefault();
            }
            return defaultLayout;
        }

        public virtual LayoutSample GetLayoutSample(string engineName, string templateName)
        {
            return GetLayoutSamples(engineName).Where(it => it.Name.EqualsOrNullEmpty(templateName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }
    }
}
