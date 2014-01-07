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

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management.Events
{
    public interface IModuleSiteRelationEvents
    {
        /// <summary>
        /// The event when site includeded.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        void OnIncluded(ModuleContext moduleContext);
        /// <summary>
        /// The event when site excluded.
        /// </summary>
        /// <param name="moduleContext">The module context.</param>
        void OnExcluded(ModuleContext moduleContext);
    }
}
