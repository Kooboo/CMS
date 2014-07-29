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
using Kooboo.CMS.Content.Models;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    public class FormModel
    {
        public FormType FormType { get; set; }
        [AllowHtml]
        public string Body { get; set; }
    }
}