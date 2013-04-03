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
    public class TextContentFileProvider : ITextContentFileProvider
    {
        public string Save(TextContent content, ContentFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var fileName = Kooboo.Extensions.StringExtensions.NormalizeUrl(Path.GetFileNameWithoutExtension(file.FileName)) + extension;
            TextContentPath contentPath = new TextContentPath(content);
            string filePath = Path.Combine(contentPath.PhysicalPath, fileName);
            file.Stream.SaveAs(filePath, true);

            return UrlUtility.Combine(contentPath.VirtualPath, fileName);
        }

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
    }
}
