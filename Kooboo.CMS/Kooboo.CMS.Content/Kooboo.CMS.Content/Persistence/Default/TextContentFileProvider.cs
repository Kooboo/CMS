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
using Kooboo.CMS.Content.Models;
using System.IO;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.Web.Url;
using Kooboo.IO;
namespace Kooboo.CMS.Content.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ITextContentFileProvider))]
    public class TextContentFileProvider : ITextContentFileProvider
    {
        #region Save
        public string Save(TextContent content, ContentFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = (Path.GetFileNameWithoutExtension(file.FileName)).ToUrlString() + extension;
            TextContentPath contentPath = new TextContentPath(content);
            string filePath = Path.Combine(contentPath.PhysicalPath, fileName);
            file.Stream.SaveAs(filePath, true);

            return UrlUtility.Combine(contentPath.VirtualPath, fileName);
        } 
        #endregion

        #region DeleteFiles
        public void DeleteFiles(TextContent content)
        {
            var contentPath = new TextContentPath(content);
            try
            {
                if (Directory.Exists(contentPath.PhysicalPath))
                {
                    IOUtility.DeleteDirectory(contentPath.PhysicalPath, true);
                }
            }
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
            }
        } 
        #endregion
    }
}
