#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.Query.Expressions
{
    public static class ExpressionValueHelper
    {
        public static object Escape(object value)
        {
            //if (value is DateTime)
            //{
            //    return TimeZoneHelper.ConvertToUtcTime((DateTime)value);
            //}
            //else
            //{
            return value;
            //}
        }
    }
}
