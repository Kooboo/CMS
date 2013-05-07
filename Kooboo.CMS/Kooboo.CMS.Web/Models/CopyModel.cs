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

namespace Kooboo.CMS.Web.Models
{
    public class CopyModel
    {
        public string UUID { get; set; }

        [Required(ErrorMessage = "Required")]
        [Display(Name = "Destination name")]
        [RemoteEx("CopyNameAvailabled", "*", RouteFields = "SiteName,RepositoryName,UUID", AdditionalFields = "ParentPage")]
        public virtual string DestinationName { get; set; }
    }
}