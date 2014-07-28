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
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel.Models
{
    public static class IInheritableExtensions
    {
        public static bool HasParentVersion<T>(this IInheritable<T> obj)
        {
            throw new NotImplementedException();
        }

        public static bool IsLocalized<T>(this IInheritable<T> obj, Site site)
        {
            throw new NotImplementedException();
        }

        public static T LastVersion<T>(this IInheritable<T> obj, Site site)
        {
            throw new NotImplementedException();
        }
    }
}
