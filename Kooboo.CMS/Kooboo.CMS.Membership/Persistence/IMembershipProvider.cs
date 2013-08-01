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
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence
{
    public interface IMembershipProvider : IProvider<Kooboo.CMS.Membership.Models.Membership>
    {
        Kooboo.CMS.Membership.Models.Membership Import(string membershipName, Stream stream);
        void Export(Kooboo.CMS.Membership.Models.Membership membership, Stream outputStream);
    }
}
