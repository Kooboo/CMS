#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Account
{
    public interface IAccountBaseDir
    {
        string PathName { get; }
        string PhysicalPath { get; }
    }
    [Dependency(typeof(IAccountBaseDir))]
    public class AccountBaseDir : IAccountBaseDir
    {
        public AccountBaseDir(IBaseDir baseDir)
        {
            this.PathName = "Account";
            this.PhysicalPath = Path.Combine(baseDir.Cms_DataPhysicalPath, this.PathName);
        }
        public string PathName
        {
            get;
            private set;
        }

        public string PhysicalPath
        {
            get;
            private set;
        }
    }
}
