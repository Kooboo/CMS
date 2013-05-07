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

namespace Kooboo.CMS.Search.Models
{
    [DataContract]
    public class LastAction
    {
        public Repository Repository { get; set; }
        [DataMember(Order = 1)]
        public string FolderName { get; set; }
        [DataMember(Order = 2)]
        public string ContentSummary { get; set; }
        [DataMember(Order = 3)]
        public ContentAction Action { get; set; }
        [DataMember(Order = 4)]
        public DateTime UtcActionDate { get; set; }
    }
}
