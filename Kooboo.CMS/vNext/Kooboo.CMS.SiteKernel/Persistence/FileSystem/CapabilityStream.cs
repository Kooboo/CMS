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

namespace Kooboo.CMS.SiteKernel.Persistence.FileSystem
{
    public static class CapabilityStream
    {
        public static Stream NewNamespace(this Stream stream)
        {
            var xmlString = stream.ReadString();
            xmlString = xmlString.Replace("Kooboo.CMS.Sites.Models", "Kooboo.CMS.SiteKernel.Models");

            MemoryStream memoryStream = new MemoryStream();

            memoryStream.WriteString(xmlString);

            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
