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
        public static ObjectCache ObjectCache = MemoryCache.Default;
        public static string CacheRegionName = null;

        public static string ModelName = "model";
        public static void Execute(ViewDataDictionary viewData, DataRuleContext dataRuleContext, IEnumerable<DataRuleSetting> dataRules)
        {
            foreach (var item in dataRules)
            {
                var contentQuery = item.DataRule.Execute(dataRuleContext);
                object data = contentQuery;
                if (item.DataRule is DataRuleBase)
                {
                    var dataRuleBase = (DataRuleBase)item.DataRule;
                    if (!string.IsNullOrEmpty(dataRuleBase.SortField))
                    {
                        if (dataRuleBase.SortDirection == DataRule.SortDirection.Ascending)
                        {
                            contentQuery = contentQuery.OrderBy(dataRuleBase.SortField);
                        }
                        else
                        {
                            contentQuery = contentQuery.OrderByDescending(dataRuleBase.SortField);
                        }
                    }
                    if (item.DataRule.EnablePaging.Value)
                    {
                        string pageIndexParameterName;
                        var pageIndexValue = ParameterizedFieldValue.GetFieldValue(dataRuleBase.PageIndex, dataRuleContext.ValueProvider, out pageIndexParameterName);
                        var intPageIndexValue = 1;
                        int.TryParse(pageIndexValue, out intPageIndexValue);
                        if (intPageIndexValue < 1)
                        {
                            intPageIndexValue = 1;
                        }

                        string pageSizeParameterName;
                        var pageSizeValue = ParameterizedFieldValue.GetFieldValue(dataRuleBase.PageSize, dataRuleContext.ValueProvider, out pageSizeParameterName);
                        var intPageSize = 10;
                        int.TryParse(pageSizeValue, out intPageSize);
                        if (intPageSize < 1)
                        {
                            intPageSize = 10;
                        }

                        var totalCount = contentQuery.Count();

                        data = new DataRulePagedList(contentQuery.Skip((intPageIndexValue - 1) * intPageSize).Take(intPageSize)
                            , intPageIndexValue
                            , intPageSize
                            , totalCount)
                            {
                                PageIndexParameterName = pageIndexParameterName
                            };
                    }
                    else if (!string.IsNullOrEmpty(dataRuleBase.Top))
                    {
                        string fieldName;
                        var topValue = ParameterizedFieldValue.GetFieldValue(dataRuleBase.Top, dataRuleContext.ValueProvider, out fieldName);
                        var intTopValue = 1;
                        int.TryParse(topValue, out intTopValue);

                        data = contentQuery.Take(intTopValue);
                    }
                }

                if (data is IContentQuery<ContentBase>)
                {
                    data = GetData(item, (IContentQuery<TextContent>)data);
                }


                if (item.DataName.EqualsOrNullEmpty(ModelName, StringComparison.CurrentCultureIgnoreCase))
                {
                    viewData.Model = data;
                }
                viewData[item.DataName] = data;
            }
        }

        private static object GetData(DataRuleSetting dataRuleSetting, IContentQuery<TextContent> contentQuery)
        {
            if (dataRuleSetting.CachingDuration > 0)
            {
                var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromSeconds(dataRuleSetting.CachingDuration) };
                switch (dataRuleSetting.TakeOperation)
                {
                    case TakeOperation.First:
                        var lazyFirst = contentQuery.LazyFirstOrDefault();
                        return GetCacheData(dataRuleSetting.TakeOperation, contentQuery, policy, () => lazyFirst.Value);
                    case TakeOperation.Count:
                        var lazyCount = contentQuery.LazyCount();
                        return GetCacheData(dataRuleSetting.TakeOperation, contentQuery, policy, () => lazyCount.Value);
                    case TakeOperation.List:
                    default:
                        return GetCacheData(dataRuleSetting.TakeOperation, contentQuery, policy, () => contentQuery.ToArray());
                }
            }
            else
            {
                switch (dataRuleSetting.TakeOperation)
                {
                    case TakeOperation.First:
                        return contentQuery.FirstOrDefault();
                    case TakeOperation.Count:
                        return contentQuery.Count();
                    case TakeOperation.List:
                    default:
                        return contentQuery.ToArray();
                }
            }
        }
        private static object GetCacheData(TakeOperation operation, IContentQuery<TextContent> contentQuery, CacheItemPolicy policy, Func<object> getData)
        {
            string cacheKey = "TakeOperation:" + operation.ToString() + " " + TextTranslator.Translate(contentQuery);
            var data = ObjectCache.Get(cacheKey, CacheRegionName);
            if (data == null)
            {
                data = getData();
                ObjectCache.AddOrGetExisting(cacheKey, data, policy, CacheRegionName);
            }
            return data;
        }
    }
}
