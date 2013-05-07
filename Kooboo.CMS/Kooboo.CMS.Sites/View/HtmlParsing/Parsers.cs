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

namespace Kooboo.CMS.Sites.View.HtmlParsing
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IParsers))]
    public class Parsers : IParsers
    {
        public IParser GetParser(string tag)
        {
            return Kooboo.CMS.Common.Runtime.EngineContext.Current.TryResolve<IParser>(tag.ToLower());
        }
    }
}
