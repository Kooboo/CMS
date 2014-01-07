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
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Models;
using System.Runtime.Serialization;
using Kooboo.CMS.Sites.Models;
namespace Kooboo.CMS.Sites.DataRule
{
    public interface IDataRule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataRuleContext"></param>
        /// <param name="operation">The type of the query, List all data, get the first item or query the count.</param>
        /// <param name="cachingDuration">The cache time, in second.</param>
        /// <returns></returns>
        object Execute(DataRuleContext dataRuleContext, TakeOperation operation, int cacheDuration);
        DataRuleType DataRuleType { get; }

        bool HasAnyParameters();
    }
}
