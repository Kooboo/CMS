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

namespace Kooboo.CMS.Sites.Models
{
    public static class ModelActivatorFactory<T>
    {
        public static IModelActivator<T> GetActivator()
        {
            return new DefaultModelActivator<T>();
        }
        public static ICascadableModelActivator<T> GetCascadableActivator()
        {
            return new DefaultCascadableModelActivator<T>();
        }
    }
}
