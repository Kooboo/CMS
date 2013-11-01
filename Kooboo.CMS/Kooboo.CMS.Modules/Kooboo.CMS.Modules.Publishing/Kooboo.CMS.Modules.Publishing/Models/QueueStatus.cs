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

namespace Kooboo.CMS.Modules.Publishing.Models
{
    public enum QueueStatus
    {
        Pending = 1,
        Processed = 2,
        Warning = 3,
        Susppended = 4,
        OK =5 
    }
}
