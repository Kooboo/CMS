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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ModuleArea.Areas.SampleModule.Models
{
    public class InstallModel
    {
        public string DatabaseServer { get; set; }
        public string UserName { get; set; }
        [UIHint("Password")]
        public string Password { get; set; }
    }
}
