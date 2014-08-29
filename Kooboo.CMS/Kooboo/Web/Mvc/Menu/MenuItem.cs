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
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;

namespace Kooboo.Web.Mvc.Menu
{
    public class MenuItem
    {
        public MenuItem()
        {
            Items = new List<MenuItem>();
            Visible = true;
        }

        public string Name { get; set; }
        public string Text { get; set; }
        public string Action { get; set; }
        public string Controller { get; set; }
        public bool Visible { get; set; }
        public string Area { get; set; }
        /// <summary>
        /// 文本上面的一些小标记
        /// </summary>
        public Badge Badge { get; set; }

        private bool localizable = true;
        public virtual bool Localizable { get { return localizable; } set { localizable = value; } }

        public string Tips { get; set; }

        public RouteValueDictionary RouteValues { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }

        public bool IsActive { get; set; }
        public bool IsCurrent { get; set; }
        public virtual IList<MenuItem> Items { get; set; }

        public NameValueCollection ReadOnlyProperties { get; set; }

        #region Initialize
        public bool Initialized { get; set; }
        private IMenuItemInitializer menuItemInitiaizer = new DefaultMenuItemInitializer();
        public IMenuItemInitializer Initializer
        {
            get
            {
                return menuItemInitiaizer;
            }
            set
            {
                menuItemInitiaizer = value;
            }
        }

        public virtual void Initialize(ControllerContext controllerContext)
        {
            if (!this.Initialized)
            {
                Initializer.Initialize(this, controllerContext);

                if (this.Items != null)
                {
                    foreach (var item in this.Items)
                    {
                        item.Initialize(controllerContext);
                    }
                }
            }
        }
        #endregion
        //#region ICloneable Members

        //public object Clone()
        //{
        //    var menuItem = (MenuItem)this.MemberwiseClone();

        //    menuItem.RouteValues = new RouteValueDictionary(this.RouteValues);
        //    menuItem.HtmlAttributes = new RouteValueDictionary(this.HtmlAttributes);

        //    return menuItem;
        //}

        //#endregion
    }
}
