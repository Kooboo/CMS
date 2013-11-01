#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    public class RemotePagePublishingModel : LocalPagePublishingModel
    {
        [DataSource(typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources.RemoteEndpointSettingDataSource))]
        [UIHint("Multiple_DropDownList")]
        [DisplayName("Remote sites")]
        [Required(ErrorMessage = "Required")]
        public string[] RemoteEndPoints { get; set; }
    }
}
