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


using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Web2.Areas.Contents.Controllers;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using System.ComponentModel;
using System.Web.Routing;
using Kooboo.CMS.Web2.Models;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.CMS.Web2.Grid2;
using Kooboo.Common.Misc;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    [MetadataFor(typeof(Schema))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class Schema_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RemoteEx("IsNameAvailable", "ContentType", RouteFields = "RepositoryName,SiteName", AdditionalFields = "old_Key")]
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        public string Name { get; set; }

        [DisplayName("Tree style data")]
        [Description("Create the tree style data and management interface.")]
        public bool IsTreeStyle
        {
            get;
            set;
        }
    }
}