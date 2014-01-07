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
using System.Web.Mvc;

namespace Kooboo.CMS.Common.DataViolation
{
    public static class ModelStateExtension
    {
        public static void FillDataViolation(this ModelStateDictionary modelState, IEnumerable<DataViolationItem> violations) {       
            foreach (var issue in violations)
            {
                modelState.AddModelError(issue.PropertyName, issue.ErrorMessage);
            }
        }

    }
}
