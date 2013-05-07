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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.Models
{
    [DataContract(Name = "MediaFolder")]
    [KnownTypeAttribute(typeof(MediaFolder))]
    public class MediaFolder : Folder
    {
        public MediaFolder() { }
        public MediaFolder(Repository repository, string fullName) : base(repository, fullName) { }
        public MediaFolder(Repository repository, string name, Folder parent)
            : base(repository, name, parent)
        { }
        public MediaFolder(Repository repository, IEnumerable<string> namePath) : base(repository, namePath) { }

        [DataMember(Order = 5)]
        public string[] AllowedExtensions { get; set; }
    }
}
