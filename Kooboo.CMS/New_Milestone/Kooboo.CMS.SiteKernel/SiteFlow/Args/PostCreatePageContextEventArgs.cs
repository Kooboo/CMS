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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.SiteFlow.Args
{
    public class PostCreatePageContextEventArgs
    {
        public PostCreatePageContextEventArgs(Page_Context page_context)
        {
            this.Page_Context = page_context;
        }

        public Page_Context Page_Context { get; set; }
    }
}
