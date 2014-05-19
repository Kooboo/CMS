#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource
{
    public class DataSourceRelation
    {
        public string TargetDataSourceName { get; set; }

        public bool LazyLoad { get; set; }

        public Dictionary<string, string> ParameterValues { get; set; }

        //public Dictionary<string, string> MergedParameterValues(Site site)
        //{
        //    if (this.ParameterValues == null)
        //    {
        //        ParameterValues = new Dictionary<string, string>();
        //    }
        //    var 
        //}
    }
}
