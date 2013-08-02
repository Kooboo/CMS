#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class SiteModuleRelationModel
    {
        public SiteModuleRelationModel(Site site)
        {
            this.Name = site.FriendlyName;
            this.UUID = site.UUID;
        }
        public SiteModuleRelationModel()
        {
        }
        [GridColumnAttribute(HeaderText = "Site", Order = 1)]
        public string Name { get; set; }


        public string UUID { get; set; }
    }
}
