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

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    public class WhereClause
    {
        public Logical Logical { get; set; }
        public string FieldName { get; set; }
        public Operator Operator { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
}
