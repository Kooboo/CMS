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
using Kooboo.CMS.Content.Models;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Content.Versioning
{
    [DataContract]
    public class CategoryContent
    {
        [DataMember(Order = 1)]
        public string UUID { get; set; }
        [DataMember(Order = 3)]
        public string DisplayName { get; set; }
    }

    [DataContract]
    public class Category
    {
        [DataMember(Order = 1)]
        public string FolderName { get; set; }
        [DataMember(Order = 3)]
        public CategoryContent[] Contents { get; set; }
    }

    [DataContract]
    public class VersionInfo
    {
        public int Version { get; set; }

        [DataMember(Order = 1)]
        public string CommitUser { get; set; }

        [DataMember(Order = 3)]
        public DateTime UtcCommitDate { get; set; }

        [DataMember(Order = 5)]
        public Dictionary<string, object> TextContent { get; set; }

        [DataMember(Order = 7)]
        public Category[] Categories { get; set; }

    }
}
