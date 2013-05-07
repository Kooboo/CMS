#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    [MetadataFor(typeof(UrlKeyMap))]
    [Grid(Checkable = true, IdProperty = "UUID")]    
    public class UrlKeyMap_Metadata
    {
        [GridColumn(Order = 1,GridColumnType=typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        [Remote("IsKeyAvailable", "UrlKeyMap", AdditionalFields = "SiteName,old_Key")]        
        public string Key { get; set; }

        [GridColumn(Order = 2,HeaderText="Page full name", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Page full name")]
        [Required(ErrorMessage = "Required")]        
        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(PagesDataSource))]
        public string PageFullName { get; set; }
    }
}