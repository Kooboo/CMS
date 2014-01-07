#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence
{
    public interface IMemberElementProvider<T> : IProvider<T>
    {
        IEnumerable<T> All(Kooboo.CMS.Membership.Models.Membership membership);
    }
}
