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

namespace Kooboo.CMS.Common.Persistence.Non_Relational
{
    public class RelationModel
    {
        public string DisplayName { get; set; }
        public string ObjectUUID { get; set; }
        public string RelationType { get; set; }
        public object RelationObject { get; set; }
    }
}
