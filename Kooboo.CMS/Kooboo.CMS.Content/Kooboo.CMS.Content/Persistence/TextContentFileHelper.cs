#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using System.Collections.Generic;
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
                Dictionary<string, string> fileFields = new Dictionary<string, string>();
                foreach (var file in content.ContentFiles)
                {
                    var column = schema[file.Name];
                    if (column != null)
                    {
                        if (file.Stream.Length > 0 && !string.IsNullOrEmpty(file.FileName))
                        {
                            var fileVirtualPath = Kooboo.Web.Url.UrlUtility.ResolveUrl(textContentFileProvider.Save(content, file));
                            var value = content[file.Name] == null ? "" : content[file.Name].ToString();
                            if (fileFields.ContainsKey(file.Name))
                            {
                                value = fileFields[file.Name];
                            }

                            if (value == null || string.IsNullOrEmpty(value.ToString()))
                            {
                                value = fileVirtualPath;
                            }
                            else
                            {
                                value = value.ToString().Trim('|') + "|" + fileVirtualPath;
                            }
                            fileFields[file.Name] = value;
                        };

                    }
                }
                foreach (var item in fileFields)
                {
                    content[item.Key] = item.Value;
                }
            }
        }

        public static void DeleteFiles(this TextContent content)
        {
            Providers.DefaultProviderFactory.GetProvider<ITextContentFileProvider>().DeleteFiles(content);
        }
    }
}
