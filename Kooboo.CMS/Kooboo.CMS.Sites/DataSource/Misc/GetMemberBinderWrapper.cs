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
using System.Dynamic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.Misc
{
    public class GetMemberBinderWrapper : GetMemberBinder
    {
        public GetMemberBinderWrapper(string name) :
            base(name, true)
        {

        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
