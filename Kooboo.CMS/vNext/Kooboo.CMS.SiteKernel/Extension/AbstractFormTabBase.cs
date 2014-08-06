#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.Web.FormTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Extension
{
    public abstract class AbstractFormTabBase : IFormTabPlugin
    {
        public abstract string Name { get; }
        public abstract string DisplayText { get; }
        public abstract Type ModelType { get; }
        public abstract bool Order { get; }
        public abstract string ViewVirtualPath { get; }
        public abstract IEnumerable<Kooboo.Common.Web.MvcRoute> ApplyTo { get; }
        public abstract void LoadData(FormTabContext context);
        public abstract void Submit(FormTabContext context);

        string Kooboo.Common.Web.IApplyTo.Position
        {
            get { throw new NotImplementedException(); }
        }
    }
}
