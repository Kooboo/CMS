#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Search.Persistence;
using Kooboo.CMS.Sites.View;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Routing;
namespace Kooboo.CMS.Sites.Search
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IObjectConverter), Key = "Kooboo.CMS.Content.Models.TextContent")]
    public class TextContentConverter : IObjectConverter
    {
        ITextContentBinder Binder { get; set; }
        ISearchSettingProvider SearchSettingProvider { get; set; }
        public TextContentConverter(ISearchSettingProvider searchSettingProvider, ITextContentBinder binder)
        {
            this.SearchSettingProvider = searchSettingProvider;
            this.Binder = binder;
        }
        public virtual IndexObject GetIndexObject(object o)
        {
            IndexObject indexObject = null;

            TextContent textContent = (TextContent)o;
            NameValueCollection storeFields = new NameValueCollection();
            NameValueCollection sysFields = new NameValueCollection();

            var repository = textContent.GetRepository();
            var schema = textContent.GetSchema().AsActual();
            var folder = textContent.FolderName;

            var folderSearchSetting = SearchSettingProvider.Get(new SearchSetting(repository, folder));
            if (folderSearchSetting != null)
            {
                string title = textContent.GetSummary();
                StringBuilder bodyBuilder = new StringBuilder();
                foreach (var key in textContent.Keys)
                {
                    var column = schema[key];
                    // If column == null, then it could be a system field.       
                    var isSystemField = column == null || column.IsSystemField;
                    var index = !isSystemField;

                    var value = textContent[key];

                    string strValue = "";
                    string indexValue = "";
                    if (value != null)
                    {
                        if (value is DateTime)
                        {
                            var rawValue = (DateTime)value;
                            strValue = rawValue.Ticks.ToString();
                            indexValue = rawValue.ToString();
                        }
                        else
                        {
                            indexValue = strValue = value.ToString();
                        }
                    }
                    if (index && !column.Summarize)
                    {
                        bodyBuilder.AppendFormat(" {0} ", indexValue.ToString().StripAllTags());
                    }

                    if (isSystemField)
                    {
                        sysFields[key] = strValue;
                    }
                    else
                    {
                        storeFields[key] = strValue;
                    }

                }

                indexObject = new IndexObject()
                {
                    Title = title,
                    Body = bodyBuilder.ToString(),
                    StoreFields = storeFields,
                    SystemFields = sysFields,
                    NativeType = typeof(TextContent).AssemblyQualifiedNameWithoutVersion()
                };
            }

            return indexObject;
        }
        public virtual object GetNativeObject(System.Collections.Specialized.NameValueCollection fields)
        {
            TextContent textContent = null;
            var repositoryName = Repository.Current.Name;
            var schemaName = fields["SchemaName"];
            if (!string.IsNullOrEmpty(repositoryName) && !string.IsNullOrEmpty(schemaName))
            {
                var repository = new Repository(repositoryName);
                var schema = new Schema(repository, schemaName).AsActual();
                if (schema != null)
                {
                    textContent = new TextContent(repositoryName, schemaName, fields["FolderName"]);

                    textContent.UUID = fields["UUID"];
                    textContent.ParentFolder = fields["ParentFolder"];
                    textContent.ParentUUID = fields["ParentUUID"];
                    textContent.UserId = fields["UserId"];
                    textContent.UserKey = fields["UserKey"];
                    textContent.OriginalUUID = fields["OriginalUUID"];
                    textContent.OriginalRepository = fields["OriginalRepository"];
                    textContent.OriginalFolder = fields["OriginalFolder"];
                    if (!string.IsNullOrEmpty(fields["IsLocalized"]))
                    {
                        var isLocalized = false;

                        if (bool.TryParse(fields["IsLocalized"], out isLocalized))
                        {
                            textContent.IsLocalized = isLocalized;
                        }
                    }
                    if (!string.IsNullOrEmpty(fields["UtcLastModificationDate"]))
                    {

                        textContent.UtcLastModificationDate = DateTimeHelper.Parse(fields["UtcLastModificationDate"]);

                    }

                    textContent.UtcCreationDate = DateTimeHelper.Parse(fields["UtcCreationDate"]);

                    Binder.Bind(schema, textContent, fields, false);

                }
            }

            return textContent;
        }

        public virtual KeyValuePair<string, string> GetKeyField(object o)
        {
            return new KeyValuePair<string, string>("UUID", ((TextContent)o).UUID);
        }

        public virtual string GetUrl(object nativeObject)
        {
            string url = string.Empty;
            var textContent = (TextContent)nativeObject;

            var repository = textContent.GetRepository();
            var schema = textContent.GetSchema().AsActual();
            var folder = textContent.FolderName;

            var searchSetting = SearchSettingProvider.Get(new SearchSetting(repository, folder));
            if (searchSetting != null)
            {
                if (!string.IsNullOrEmpty(searchSetting.LinkPage))
                {
                    url = GenerateContentUrl(textContent, searchSetting);
                }
            }
            return url;
        }

        protected virtual string GenerateContentUrl(TextContent content, SearchSetting searchSetting)
        {
            if (Page_Context.Current.Initialized)
            {
                var routeValues = new RouteValueDictionary();
                if (searchSetting.RouteValueFields != null && searchSetting.RouteValueFields.Count > 0)
                {
                    foreach (var routeValue in searchSetting.RouteValueFields)
                    {
                        routeValues[routeValue.Key] = EvaluateStringFormulas(content, routeValue.Value);
                    }
                }
                return Page_Context.Current.FrontUrl.PageUrl(searchSetting.LinkPage, routeValues).ToString();
            }
            return "";
        }
        protected virtual string EvaluateStringFormulas(TextContent content, string formulas)
        {
            if (string.IsNullOrEmpty(formulas))
            {
                return null;
            }
            var matches = Regex.Matches(formulas, "{(?<Name>[^{^}]+)}");
            var s = formulas;
            foreach (Match match in matches)
            {
                var value = GetFieldValue(content, match.Groups["Name"].Value);
                s = s.Replace(match.Value, value == null ? "" : value.ToString());
            }
            return s;
        }
        private string GetFieldValue(TextContent content, string fieldName)
        {
            string value = string.Empty;
            if (content.ContainsKey(fieldName))
            {
                value = content[fieldName] == null ? "" : content[fieldName].ToString();
            }
            else
            {
                value = Page_Context.Current.PageRequestContext.AllQueryString[fieldName];
            }
            return value;
        }
    }
}
