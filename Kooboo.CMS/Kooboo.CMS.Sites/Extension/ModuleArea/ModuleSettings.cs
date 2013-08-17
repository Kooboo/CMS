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
using System.Web.Routing;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    [DataContract]
    [KnownType(typeof(string[]))]
    public class Entry
    {
        public Entry()
        {
            this.Name = string.Empty;
            this.Controller = string.Empty;
            this.Action = string.Empty;
            this.LinkToEntryName = string.Empty;
        }
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Controller { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string Action { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public RouteValueDictionary Values { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public string LinkToEntryName { get; set; }
    }
    [DataContract]
    public class ModuleSettings
    {
        public ModuleSettings()
        {
            CustomSettings = new Dictionary<string, string>();
        }
        [DataMember(EmitDefaultValue = false)]
        public string ThemeName { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Entry Entry { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, string> CustomSettings { get; set; }
    }
}
