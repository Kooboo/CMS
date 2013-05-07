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
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Contents.Models
{

    public class MoveMediaContent
    {
        [RemoteEx("TargetFolderAvailable", "*", RouteFields = "FolderName,SiteName")]
        [UIHint("DropDownList")]
        [DataSource(typeof(Kooboo.CMS.Web.Areas.Contents.Models.DataSources.MediaFoldersDataSource))]
        public string TargetFolder { get; set; }
    }
}