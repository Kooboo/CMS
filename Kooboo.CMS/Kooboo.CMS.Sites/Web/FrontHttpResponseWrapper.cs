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
using System.Web;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Sites.Web
{
    public class FrontHttpResponseWrapper : System.Web.HttpResponseWrapper
    {
        FrontHttpContextWrapper _context;
        public FrontHttpResponseWrapper(HttpResponse httpResponse, FrontHttpContextWrapper context)
            : base(httpResponse)
        {
            _context = context;
        }
        public override void End()
        {
            if (this.Output is OutputTextWriterWrapper)
            {
                ((OutputTextWriterWrapper)this.Output).Render(this);
            }
            base.End();
        }
    }
}
