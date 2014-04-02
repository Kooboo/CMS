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

namespace Kooboo.CMS.Common.Persistence
{
    public interface IChangeTimeline
    {
        DateTime? UtcCreationDate { get; set; }


        DateTime? UtcLastestModificationDate { get; set; }


        string LastestEditor { get; set; }
    }
}
