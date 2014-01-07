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

namespace Kooboo.CMS.Sites.Extension.Management
{
    public class ConflictedAssemblyReference
    {
        public ConflictedAssemblyReference()
        { }
        public ConflictedAssemblyReference(AssemblyReferenceData referenceData, string conflictedVersion)
        {
            this.ReferenceData = referenceData;
            this.ConflictedVersion = conflictedVersion;
        }
        public AssemblyReferenceData ReferenceData { get; set; }
        public string ConflictedVersion { get; set; }
    }
}
