using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using System.Reflection;
using Kooboo.CMS.Sites.Extension;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    public class PluginType_Metadata
    {
        public PluginType_Metadata(Type type)
        {
            this.FullName = type.FullName;
            this.BaseType = type.BaseType;
            this.Assembly = type.Assembly;

            if (typeof(IPagePlugin).IsAssignableFrom(type))
            {
                IPagePlugin pagePlugin = (IPagePlugin)Activator.CreateInstance(type);
                this.Description = pagePlugin.Description;
            }
        }
        [GridColumn(Order = 1)]
        public string FullName { get; set; }

        [GridColumn(Order = 3)]
        public Type BaseType { get; set; }

        //[GridColumn(Order = 5)]
        public Assembly Assembly { get; set; }

        [GridColumn(Order = 5)]
        public string Description { get; set; }
    }
}