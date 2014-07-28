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
    public partial class Label : ISiteObject
    {
        public Site Site
        {
            get;
            set;
        }
    }
    public partial class Label : IIdentifiable
    {
        #region .ctor
        public Label()
        { }
        public Label(Site site, string category, string name)
        {
            this.Site = site;
            this.Category = category;
            this.Name = name;
        }
        #endregion
        public string Name { get; set; }
        public string Value { get; set; }
        public string Category { get; set; }
        public DateTime? UtcCreationDate { get; set; }
        public DateTime? UtcLastestModificationDate { get; set; }
        public string LastestEditor { get; set; }

        public string UUID
        {
            get;
            set;
        }
    }
}
