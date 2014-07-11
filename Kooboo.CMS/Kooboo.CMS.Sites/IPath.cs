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

namespace Kooboo.CMS.Sites
{
    public interface IPath
    {
        string PhysicalPath { get; }
        string VirtualPath { get; }
    }

    public class CommonPath : IPath
    {
        public string PhysicalPath
        {
            get;
            set;
        }

        public string VirtualPath
        {
            get;
            set;
        }
    }

    public static class PathEx
    {
        static PathEx()
        {
            var baseDir = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<Kooboo.CMS.Common.IBaseDir>();
            BasePath = baseDir.Cms_DataPathName;
        }
        public static string BasePath;
    }
}
