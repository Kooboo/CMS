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

namespace Kooboo.CMS.Content.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class CategoryFolder
    {
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        public string FolderName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [single choice].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [single choice]; otherwise, <c>false</c>.
        /// </value>
        public bool SingleChoice { get; set; }
    }
}
