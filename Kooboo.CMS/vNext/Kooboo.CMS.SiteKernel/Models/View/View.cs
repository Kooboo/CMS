#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public partial class View : ISiteObject, IInheritable<View>, IIdentifiable
    {
        public Site Site
        {
            get;
            set;
        }

        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
    }
    public partial class View
    {
        #region .ctor
        public View()
        {

        }
        public View(Site site, string name)
        {
            this.Site = site;
            this.Name = name;
        }
        #endregion
        public string Name { get; set; }
        public string TemplateType { get; set; }
        public string TemplateExtension { get; set; }
        public string Body { get; set; }
        public string[] Plugins { get; set; }
        public DataRuleSetting[] DataRules { get; set; }
        public Parameter[] Parameters { get; set; }
    }
}
