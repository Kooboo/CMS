using Couchbase;
using Enyim.Caching.Memcached.Results;
using Kooboo.CMS.Content.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.Persistence.Couchbase
{
    public static class ModelExtendions
    {
        public static string GetTypePropertyName()
        {
            return "__TypeProperty".ToLower();
        }

        public static string GetCategoryTypePropertyValue()
        {
            return "__ContentCategory".ToLower();
        }

        public static string GetTypePropertyValue(this Schema schema)
        {
            return schema.Name.ToLower();
        }

        public static string GetDocumentKey(this Category category)
        {
            return string.Format("{0}_{1}_{2}", category.CategoryUUID, category.ContentUUID, category.CategoryFolder);
        }

        public static string GetDefaultViewDesign(this Repository repository)
        {
            return "__ContentDefaultViews".ToLower();
        }

        //public static string GetDesginName(this Schema schema)
        //{
        //    return ("__SchemaViews_" + schema.Name).ToLower();
        //}

        public static string GetBucketName(this Repository repository)
        {
            return repository.Name.ToLower();
        }

        public static string GetBucketPassword(this Repository repository)
        {
            if (!string.IsNullOrWhiteSpace(DatabaseSettings.Instance.BucketPassword))
            {
                return DatabaseSettings.Instance.BucketPassword;
            }
            return string.Empty;
        }

        #region Views
        public static string DefaultViews(this Repository repository)
        {
            return
            string.Format(@"{{
                    ""views"": {{
                        ""All_CategoriesData"":{{
                            ""map"":""function(doc,meta){{
                                if(doc.{0}&&doc.{0}===\""{1}\""){{
                                    emit(meta.id,doc);
                                }}
                            }}""
                        }},
                        ""CategorisData_By_ContentUUID"": {{
                            ""map"": ""function (doc,meta) {{
                                if (doc.{0}&&doc.{0}===\""{1}\""){{
                                    emit(meta.id, doc.ContentUUID); 
                                }}
                            }}""
                        }},
                        ""Sort_By_UserKey"": {{
                            ""map"": ""function (doc,meta) {{
                                if (doc.SchemaName!=undefined){{
                                    emit(doc.UserKey, doc); 
                                }}
                            }}""
                        }}
                    }}
            }}", GetTypePropertyName(), GetCategoryTypePropertyValue()).Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }

        public static string SchemaViews(this Schema schema)
        {
            return
            string.Format(@"{{
                    ""views"": {{
                        ""All"":{{
                            ""map"":""function(doc,meta){{
                                if(doc.{0}&&doc.{0}===\""{1}\""){{
                                    emit(meta.id,doc);
                                }}
                            }}""
                        }}
                    }}
            }}", GetTypePropertyName(), schema.GetTypePropertyValue()).Replace(" ", "").Replace("\r", "").Replace("\n", "");
        }
        #endregion


        #region Model To Json
        public static string ToJson(this TextContent content)
        {
            var json = JsonConvert.SerializeObject(content);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dict[GetTypePropertyName()] = content.GetSchema().GetTypePropertyValue();
            return JsonConvert.SerializeObject(dict);
        }

        public static string ToJson(this Category category)
        {
            var json = JsonConvert.SerializeObject(category);
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dict[GetTypePropertyName()] = GetCategoryTypePropertyValue();
            return JsonConvert.SerializeObject(dict);
        }

        #endregion

        public static Category ToCategory(this IViewRow row)
        {
            Category category = new Category();
            var dict = row.Info["value"] as Dictionary<string, object>;

            category.CategoryFolder = dict["CategoryFolder"] as string;
            category.CategoryUUID = dict["CategoryUUID"] as string;
            category.ContentUUID = dict["ContentUUID"] as string;

            return category;
        }

        public static TextContent ToContent(this IViewRow row)
        {
            if (row == null)
            {
                return null;
            }
            TextContent content = new TextContent();
            Dictionary<string, object> dict = row.Info["value"] as Dictionary<string, object>;
            foreach (var key in dict.Keys)
            {
                content[key] = dict[key];
            }
            return content;
        }

        public static TextContent ToContent(this IGetOperationResult row)
        {
            if (row == null)
            {
                return null;
            }
            var dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(row.Value.ToString());
            return new TextContent(dic);
        }
    }
}
