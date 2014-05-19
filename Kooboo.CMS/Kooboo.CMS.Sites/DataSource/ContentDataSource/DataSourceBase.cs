#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Collections;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Formula;

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    [DataContract(Name = "DataSourceBase")]
    [KnownTypeAttribute(typeof(DataSourceBase))]
    [KnownTypeAttribute(typeof(FolderDataSource))]
    public abstract class DataSourceBase : IContentDataSource
    {
        [DataMember]
        public string FolderName { get; set; }

        [DataMember]
        public WhereClause[] WhereClauses
        {
            get;
            set;
        }

        private string sortField = "UtcCreationDate";
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
        public string PageIndex { get; set; }
        [DataMember]
        public string PageSize { get; set; }
        [DataMember]
        public string Top { get; set; }

        [DataMember]
        public TakeOperation TakeOperation { get; set; }

        public abstract IContentQuery<TextContent> GetContentQuery(DataSourceContext dataSourceContext);

        #region IDataRule Members
        public object Execute(DataSourceContext dataSourceContext)
        {
            var contentQuery = this.GetContentQuery(dataSourceContext);
            object data = contentQuery;

            if (!string.IsNullOrEmpty(this.SortField))
            {
                if (this.SortDirection == SortDirection.Ascending)
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
                var pageIndexValue = ParameterizedFieldValue.GetFieldValue(this.PageIndex, dataSourceContext.ValueProvider, out pageIndexParameterName);
                var intPageIndexValue = 1;
                int.TryParse(pageIndexValue, out intPageIndexValue);
                if (intPageIndexValue < 1)
                {
                    intPageIndexValue = 1;
                }

                string pageSizeParameterName;
                var pageSizeValue = ParameterizedFieldValue.GetFieldValue(this.PageSize, dataSourceContext.ValueProvider, out pageSizeParameterName);
                var intPageSize = 10;
                int.TryParse(pageSizeValue, out intPageSize);
                if (intPageSize < 1)
                {
                    intPageSize = 10;
                }

                var totalCount = contentQuery.Count();

                data = new ContentPagedList(contentQuery.Skip((intPageIndexValue - 1) * intPageSize).Take(intPageSize)
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
                var topValue = ParameterizedFieldValue.GetFieldValue(this.Top, dataSourceContext.ValueProvider, out fieldName);
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
                data = GetData((IContentQuery<TextContent>)data, this.TakeOperation);//, cacheDuration);
            }

            return data;
        }
        private static object GetData(IContentQuery<TextContent> contentQuery, TakeOperation operation)//, int cacheDuration)
        {
            //if (cacheDuration > 0)
            //{
            //    var policy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromSeconds(cacheDuration) };
            //    switch (operation)
            //    {
            //        case TakeOperation.First:
            //            var lazyFirst = contentQuery.LazyFirstOrDefault();
            //            return GetCacheData(contentQuery, operation, policy, () => lazyFirst.Value);
            //        case TakeOperation.Count:
            //            var lazyCount = contentQuery.LazyCount();
            //            return GetCacheData(contentQuery, operation, policy, () => lazyCount.Value);
            //        case TakeOperation.List:
            //        default:
            //            return GetCacheData(contentQuery, operation, policy, () => contentQuery.ToArray());
            //    }
            //}
            //else
            //{
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
            //}
        }
        //private static object GetCacheData(IContentQuery<TextContent> contentQuery, TakeOperation operation, CacheItemPolicy policy, Func<object> getData)
        //{
        //    string cacheKey = "TakeOperation:" + operation.ToString() + " " + TextTranslator.Translate(contentQuery);
        //    var data = contentQuery.Repository.ObjectCache().Get(cacheKey);
        //    if (data == null)
        //    {
        //        data = getData();
        //        contentQuery.Repository.ObjectCache().AddOrGetExisting(cacheKey, data, policy);
        //    }
        //    return data;
        //}

        //public abstract DataRuleType DataRuleType { get; }

        public abstract Schema GetSchema(Repository repository);
        public virtual bool IsValid(Repository repository)
        {
            var folder = FolderHelper.Parse<TextFolder>(repository, FolderName);
            return folder.AsActual() != null;
        }

        public virtual IEnumerable<string> GetParameters()
        {
            FormulaParser parser = new FormulaParser();

            List<string> parameters = new List<string>();

            //if (ParameterizedFieldValue.IsParameterizedField(PageIndex))
            //{
            //    return true;
            //}
            //if (ParameterizedFieldValue.IsParameterizedField(PageSize))
            //{
            //    return true;
            //}
            if (!string.IsNullOrEmpty(this.Top))
            {
                var topParameters = parser.GetParameters(this.Top);
                parameters.AddRange(topParameters, StringComparer.OrdinalIgnoreCase);
            }


            if (WhereClauses != null)
            {
                foreach (var item in WhereClauses)
                {
                    if (!string.IsNullOrEmpty(item.Value1))
                    {
                        var value1Parameters = parser.GetParameters(item.Value1);
                        parameters.AddRange(value1Parameters, StringComparer.OrdinalIgnoreCase);
                    }
                    if (!string.IsNullOrEmpty(item.Value2))
                    {
                        var value2Paramters = parser.GetParameters(item.Value2);
                        parameters.AddRange(value2Paramters, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
            return parameters;
        }
        #endregion
    }
}
