#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class DataRuleBase : IDataRule
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

        #region IDataRule Members

        public abstract Content.Query.IContentQuery<Content.Models.TextContent> Execute(DataRuleContext dataRuleContext);

        public abstract DataRuleType DataRuleType { get; }

        public abstract Schema GetSchema(Repository repository);
        public virtual bool IsValid(Repository repository)
        {
            var folder = FolderHelper.Parse<TextFolder>(repository, FolderName);
            return folder.AsActual() != null;
        }
        #endregion

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



    }
}
