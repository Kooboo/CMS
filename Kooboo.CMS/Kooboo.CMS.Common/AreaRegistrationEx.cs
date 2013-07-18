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

namespace Kooboo.CMS.Common
{
    public abstract class AreaRegistrationEx : AreaRegistration
    {
        private static List<string> _areas = new List<string>();
        public static IEnumerable<string> AllAreas
        {
            get
            {
                return _areas;
            }
        }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            _areas.Add(context.AreaName);
        }
    }
}
