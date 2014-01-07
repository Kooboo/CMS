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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Translator.String;
using System.Runtime.Caching;
using Kooboo.Extensions;

namespace Kooboo.CMS.Sites.DataRule
{
    public static class DataRuleExecutor
    {
        public static string ModelName = "model";
        public static void Execute(ViewDataDictionary viewData, DataRuleContext dataRuleContext, IEnumerable<DataRuleSetting> dataRules)
        {
            foreach (var item in dataRules)
            {
                var data = item.DataRule.Execute(dataRuleContext, item.TakeOperation, item.CachingDuration);
                if (item.DataName.EqualsOrNullEmpty(ModelName, StringComparison.CurrentCultureIgnoreCase))
                {
                    viewData.Model = data;
                }
                viewData[item.DataName] = data;
            }
        }


    }
}
