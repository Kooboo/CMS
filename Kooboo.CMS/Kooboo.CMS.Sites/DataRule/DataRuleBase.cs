﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Translator.String;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Caching;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Sites.DataRule
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    [DataContract(Name = "DataRuleBase")]
    [KnownTypeAttribute(typeof(DataRuleBase))]
    [KnownTypeAttribute(typeof(FolderDataRule))]
    [KnownTypeAttribute(typeof(CategoryDataRule))]
    [KnownTypeAttribute(typeof(SchemaDataRule))]
    public abstract class DataRuleBase : IContentDataRule
    {
        [DataMember(Order = 13)]
        public string FolderName { get; set; }

        [DataMember(Order = 1)]
        public WhereClause[] WhereClauses
        {
            get;
            set;
        }

        private string sortField = "UtcCreationDate";
        [DataMember(Order = 3)]
        public string SortField
        {
            get
            {
                return sortField;
            }
            set
            {
                sortField = value;
            }
        }
        private SortDirection sortDir = SortDirection.Descending;
        [DataMember(Order = 5)]
        public SortDirection SortDirection
        {
            get
            {
                return sortDir;
            }
            set
            {
                sortDir = value;
            }
        }
        private bool? _enablePageing;
        [DataMember(Order = 6)]
        public bool? EnablePaging
        {
            get
            {
                if (!_enablePageing.HasValue)
                {
                    _enablePageing = !string.IsNullOrEmpty(PageIndex); // EnablePaging是一个新加字段，兼容V3版本的判断
                }
                return _enablePageing;
            }
            set
            {
                _enablePageing = value;
            }
        }
        [DataMember(Order = 7)]
        public string PageIndex { get; set; }
        [DataMember(Order = 9)]
        public string PageSize { get; set; }
        [DataMember(Order = 11)]
        public string Top { get; set; }

        public abstract IContentQuery<Content.Models.TextContent> GetContentQuery(DataRuleContext dataRuleContext);

        #region IDataRule Members
        public object Execute(DataRuleContext dataRuleContext, TakeOperation operation, int cacheDuration)
        {
            var contentQuery = this.GetContentQuery(dataRuleContext);
            object data = contentQuery;

            if (!string.IsNullOrEmpty(this.SortField))
            {
                if (this.SortDirection == DataRule.SortDirection.Ascending)
                {
                    contentQuery = contentQuery.OrderBy(this.SortField);
                }
                else
                {
                    contentQuery = contentQuery.OrderByDescending(this.SortField);
                }
            }
            if (this.EnablePaging.Value)
            {
                string pageIndexParameterName;
                var pageIndexValue = ParameterizedFieldValue.GetFieldValue(this.PageIndex, dataRuleContext.ValueProvider, out pageIndexParameterName);
                var intPageIndexValue = 1;
                int.TryParse(pageIndexValue, out intPageIndexValue);
                if (intPageIndexValue < 1)
                {
                    intPageIndexValue = 1;
                }

                string pageSizeParameterName;
                var pageSizeValue = ParameterizedFieldValue.GetFieldValue(this.PageSize, dataRuleContext.ValueProvider, out pageSizeParameterName);
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
            else if (!string.IsNullOrEmpty(this.Top))
            {
                string fieldName;
                var topValue = ParameterizedFieldValue.GetFieldValue(this.Top, dataRuleContext.ValueProvider, out fieldName);
                var intTopValue = 1;
                int.TryParse(topValue, out intTopValue);

                data = contentQuery.Take(intTopValue);
            }
            else
            {
                data = contentQuery;
            }


            if (data is IContentQuery<ContentBase>)
            {
                data = GetData((IContentQuery<TextContent>)data, operation, cacheDuration);
            }

            return data;
        }
        private static object GetData(IContentQuery<TextContent> contentQuery, TakeOperation operation, int cacheDuration)
        {
            if (cacheDuration > 0)
            {
                var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromSeconds(cacheDuration) };
                switch (operation)
                {
                    case TakeOperation.First:
                        var lazyFirst = contentQuery.LazyFirstOrDefault();
                        return GetCacheData(contentQuery, operation, policy, () => lazyFirst.Value);
                    case TakeOperation.Count:
                        var lazyCount = contentQuery.LazyCount();
                        return GetCacheData(contentQuery, operation, policy, () => lazyCount.Value);
                    case TakeOperation.List:
                    default:
                        return GetCacheData(contentQuery, operation, policy, () => contentQuery.ToArray());
                }
            }
            else
            {
                switch (operation)
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
        private static object GetCacheData(IContentQuery<TextContent> contentQuery, TakeOperation operation, CacheItemPolicy policy, Func<object> getData)
        {
            string cacheKey = "TakeOperation:" + operation.ToString() + " " + TextTranslator.Translate(contentQuery);
            var data = contentQuery.Repository.ObjectCache().Get(cacheKey);
            if (data == null)
            {
                data = getData();
                contentQuery.Repository.ObjectCache().AddOrGetExisting(cacheKey, data, policy);
            }
            return data;
        }
       
        public abstract DataRuleType DataRuleType { get; }

        public abstract Schema GetSchema(Repository repository);
        public virtual bool IsValid(Repository repository)
        {
            var folder = FolderHelper.Parse<TextFolder>(repository, FolderName);
            return folder.AsActual() != null;
        }

        public virtual bool HasAnyParameters()
        {
            //if (ParameterizedFieldValue.IsParameterizedField(PageIndex))
            //{
            //    return true;
            //}
            //if (ParameterizedFieldValue.IsParameterizedField(PageSize))
            //{
            //    return true;
            //}
            if (ParameterizedFieldValue.IsParameterizedField(Top))
            {
                return true;
            }
            if (WhereClauses != null)
            {
                foreach (var item in WhereClauses)
                {
                    if (ParameterizedFieldValue.IsParameterizedField(item.Value1) || ParameterizedFieldValue.IsParameterizedField(item.Value2))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion
    }
}
