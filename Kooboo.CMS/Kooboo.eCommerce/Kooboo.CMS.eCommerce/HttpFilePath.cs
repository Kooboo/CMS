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

namespace Kooboo.CMS.eCommerce
{
    public class HttpFilePath
    {
        public string PhysicalPath { get; set; }
        public string VirtualPath { get; set; }
        public bool Exists()
        {
            return File.Exists(PhysicalPath);
        }
    }
}
