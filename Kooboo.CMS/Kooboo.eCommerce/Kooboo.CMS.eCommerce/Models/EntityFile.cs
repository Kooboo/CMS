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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models
{
    public class EntityFile
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public Stream Data { get; set; }
    }
}
