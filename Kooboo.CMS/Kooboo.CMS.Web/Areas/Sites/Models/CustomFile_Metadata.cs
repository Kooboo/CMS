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
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class ImageRender : IItemColumnRender
    {
        #region IColumnRender Members

        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            string path = value.ToString();
            if (path.EndsWith(".jpg") || path.EndsWith(".png") || path.EndsWith(".gif"))
            {
                return new HtmlString(string.Format(@"<a href='{0}' alt='preview' rel='{0}' >preview</a>", Kooboo.Web.Url.UrlUtility.ResolveUrl(value.ToString())));
            }
            return new HtmlString(string.Format(@"<a href='{0}' alt='open' rel='{0}' >open</a><input type=""hidden"" rel='paths' value=""{1}""/>", Kooboo.Web.Url.UrlUtility.ResolveUrl(value.ToString()), CustomDirectoryHelper.VirtualPathToFullName(value.ToString())));


        }

        #endregion
    }
    public class FileIconRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, System.Web.Mvc.ViewContext viewContext)
        {
            //string iconPath = string.IsNullOrEmpty(value.ToString()) ? "folder" : (value.ToString().IndexOf(".") == 0 ? value.ToString().Substring(1) : "unknown"); 
            return new HtmlString(string.Format(@"<img src='/Areas/admin/Styles/images/FileExtensionIcon/{0}.gif' alt='{0}' onerror=""adminJs.fileJs.setUnknowFile(this)"" style='max-width:20px;'/>", value.ToString()));
        }
    }
    [MetadataFor(typeof(CustomFile))]
    [GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this item?", CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), RouteValueProperties = "FileName,fullName=Directory,fileType=FileType")]
    public class CustomFile_Metadata
    {
        [GridColumn]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        public string FileName { get; set; }
        [GridColumn(Order = 1, ItemRenderType = typeof(ImageRender), HeaderText = "Preview")]
        public string VirtualPath { get; set; }
        [GridColumn(Order = 0, HeaderText = "File Type", ItemRenderType = typeof(FileIconRender))]
        public string FileType { get; set; }
    }

   
}