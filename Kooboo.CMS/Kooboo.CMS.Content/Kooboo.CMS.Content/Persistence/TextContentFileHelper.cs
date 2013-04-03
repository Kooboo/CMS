using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using Kooboo.IO;
using Kooboo.Web.Url;
namespace Kooboo.CMS.Content.Persistence.Default
{
    public static class TextContentFileHelper
    {
        public static void StoreFiles(this TextContent content)
        {
            var schema = content.GetSchema();
            if (content.ContentFiles != null)
            {                
                var textContentFileProvider = Providers.DefaultProviderFactory.GetProvider<ITextContentFileProvider>();
                schema = schema.AsActual();
                foreach (var file in content.ContentFiles)
                {
                    var column = schema[file.Name];
                    if (column != null)
                    {
                        var controlType = column.GetControlType();
                        if (controlType != null && controlType.IsFile && file.Stream.Length > 0 && !string.IsNullOrEmpty(file.FileName))
                        {
                            var fileVirtualPath = textContentFileProvider.Save(content, file);
                            content[column.Name] = controlType.GetValue(content[column.Name] == null ? "" : content[column.Name].ToString(), fileVirtualPath);
                        };

                    }
                }
            }
        }

        public static void DeleteFiles(this TextContent content)
        {
            Providers.DefaultProviderFactory.GetProvider<ITextContentFileProvider>().DeleteFiles(content);
        }
    }
}
