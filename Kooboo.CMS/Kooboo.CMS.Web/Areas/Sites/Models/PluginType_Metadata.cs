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
using System.Web;
using System.Reflection;
using Kooboo.CMS.Sites.Extension;
using Kooboo.Web.Mvc.Grid2.Design;
using System.ComponentModel;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid]
    public class PluginType_Metadata
    {
        public PluginType_Metadata(Type type)
        {
            this.FullName = type.FullName;
            this.BaseType = type.BaseType;
            this.Assembly = type.Assembly;

            this.Description = GetDescription(type);
        }
        private string GetDescription(Type type)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])type.GetCustomAttributes(typeof(DescriptionAttribute),true);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return string.Empty;
            }
        }
        [GridColumn(Order = 1)]
        public string FullName { get; set; }

        [GridColumn(Order = 3)]
        public Type BaseType { get; set; }

        //[GridColumn(Order = 5)]
        public Assembly Assembly { get; set; }

        public string Description { get; set; }
    }
}