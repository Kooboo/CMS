using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Search.Persistence;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Kooboo.Extensions;
using Kooboo.CMS.Content;

namespace Kooboo.CMS.Search
{
    public class TextContentConverter : IObjectConverter
    {
        public virtual Models.IndexObject GetIndexObject(object o)
        {
            IndexObject indexObject = null;

            TextContent textContent = (TextContent)o;
            NameValueCollection storeFields = new NameValueCollection();
            NameValueCollection sysFields = new NameValueCollection();

            var repository = textContent.GetRepository();
            var schema = textContent.GetSchema().AsActual();
            var folder = textContent.FolderName;

            var folderSearchSetting = Providers.SearchSettingProvider.Get(new SearchSetting(repository, folder));
            if (folderSearchSetting != null)
            {
                string title = textContent.GetSummary();
                StringBuilder bodyBuilder = new StringBuilder();
                foreach (var key in textContent.Keys)
                {
                    var column = schema[key];

                    var includeField = IsIncluded(folderSearchSetting, key);
                    // If column == null, then it could be a system field.       
                    var isSystemField = column == null || column.IsSystemField;
                    var index = !isSystemField && includeField;

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
                        bodyBuilder.AppendFormat(" {0} ", Kooboo.Extensions.StringExtensions.StripAllTags(indexValue.ToString()));
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

        private static bool IsIncluded(SearchSetting folderSearchSetting, string key)
        {
            return folderSearchSetting.IncludeAllFields || folderSearchSetting.Fields.Contains(key, StringComparer.OrdinalIgnoreCase);
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

                    Kooboo.CMS.Content.Models.Binder.TextContentBinder.DefaultBinder.Bind(schema, textContent, fields, false);

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

            var folderSearchSetting = Providers.SearchSettingProvider.Get(new SearchSetting(repository, folder));
            if (folderSearchSetting != null)
            {
                if (!string.IsNullOrEmpty(folderSearchSetting.UrlFormat))
                {
                    url = EvaluateStringFormulas(textContent, folderSearchSetting.UrlFormat);
                }
            }
            return url;
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
            return value;
        }
    }
}
