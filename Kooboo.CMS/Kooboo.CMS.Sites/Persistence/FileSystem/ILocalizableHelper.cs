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
using System.Text;
using System.IO;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public static class ILocalizableHelper
    {
        public static void Localize<T>(T source, Site targetSite) where T : PathResource, IInheritable<T>, new()
        {
            if (source.Site != targetSite)
            {
                var target = new T();
                ((PathResource)target).Site = targetSite;
                target.Name = source.Name;

                CopyFiles(source.PhysicalPath, target.PhysicalPath);
            }

        }

        public static void CopyFiles(string varFromDirectory, string varToDirectory)
        {
            if (Directory.Exists(varFromDirectory))
            {
                IOUtility.CopyDirectory(varFromDirectory, varToDirectory, false);
            }
        }
    }
}
