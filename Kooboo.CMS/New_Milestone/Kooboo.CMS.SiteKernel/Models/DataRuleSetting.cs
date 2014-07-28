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
    public class DataRuleSetting
    {
        public string DataName { get; set; }
        //public IDataRule DataRule { get; set; }
        //public TakeOperation TakeOperation { get; set; }

        /// <summary>
        /// The time, in seconds, that the data rule is cached. 
        /// </summary>
        public int CachingDuration { get; set; }
    }
}
