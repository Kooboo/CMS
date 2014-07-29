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

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    public class ImageScaleModel
    {
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool PreserveAspectRatio { get; set; }
        public int? Quality { get; set; }
    }
}