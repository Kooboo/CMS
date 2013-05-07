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
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites.Models;
using Kooboo.ComponentModel;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(FileResource))]
    public class FileResource_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsNameAvailable", "", AdditionalFields = "SiteName,type,old_Key,folderPath,FileExtension")]
        public string Name { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(FileExtensionsDataSource))]
        public string FileExtension { get; set; }

        [AllowHtml]
        [UIHintAttribute("FileEditor")]
        public string Body { get; set; }
    }
}