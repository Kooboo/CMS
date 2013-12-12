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
using Kooboo.CMS.Sites.Models;
using System.Web.Routing;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Extension.ModuleArea.Runtime;
using System.Web;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleContext
    {
        #region Create
        public static ModuleContext Create(string moduleName, Site site, ModulePosition position)
        {
            var context = new ModuleContext(moduleName, site, position);

            if (!System.IO.Directory.Exists(context.ModulePath.PhysicalPath))
            {
                throw new Exception(string.Format("The module does not exist.Module name:{0}".Localize(), moduleName));
            }
            return context;
        }
        public static ModuleContext Create(string moduleName, Site site)
        {
            return new ModuleContext(moduleName, site);
        }
        #endregion

        #region Current

        public static ModuleContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    throw new ApplicationException("The module context must be run in http context.");
                }
                return (ModuleContext)(HttpContext.Current.Items["___ModuleContext"]);
            }
            set
            {
                if (HttpContext.Current == null)
                {
                    throw new ApplicationException("The module context must be run in http context.");
                }
                HttpContext.Current.Items["___ModuleContext"] = value;
            }
        }
        #endregion

        #region .ctor
        public ModuleContext(string moduleName, Site site, ModulePosition position)
            : this(moduleName, site)
        {
            this.FrontEndContext = new FrontEndContext(moduleName,this.GetModuleSettings(), position);
        }

        public ModuleContext(string moduleName, Site site)
        {
            this.Site = site;
            ModuleName = moduleName;

        }
        public ModuleContext(string moduleName)
            : this(moduleName, null)
        {

        }
        #endregion

        #region Properties
        public string ModuleName { get; private set; }

        ModuleInfo _moduleInfo;
        public ModuleInfo ModuleInfo
        {
            get
            {
                if (_moduleInfo == null)
                {
                    _moduleInfo = ModuleInfo.Get(this.ModuleName);
                }
                return _moduleInfo;
            }
        }

        public ModulePath ModulePath
        {
            get
            {
                return new ModulePath(this.ModuleName, this.Site);
            }
        }

        public Site Site { get; private set; }
        #endregion

        #region FrontEndContext
        public FrontEndContext FrontEndContext { get; private set; }
        #endregion

    }
}
