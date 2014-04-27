using Couchbase;
using Kooboo.CMS.Sites.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.Collections;
using Enyim.Caching.Memcached;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public static class ModelExtensions
    {
        public static string DesignDocumentName = "SiteProvider";
        public static string DataTypeFieldName = "_datatype_";
        public static string PageDataType = "Page";
        public static string PageDraftDataType = "PageDraft";
        public static string HtmlBlockDataType = "HtmlBlock";
        public static string CustomErrorDataType = "CustomError";
        public static string UrlRedirectDataType = "UrlRedirect";
        public static string LabelDataType = "Label";
        public static string LabelCategoryDataType = "LabelCategory";
        public static string UserDataType = "User";
        public static string SiteDataType = "SiteSetting";
        public static string ABPageSettingDataType = "ABPageSetting";
        public static string ABPageTestResultDataType = "ABPageTestResult";
        public static string ABRuleSettingDataType = "ABRuleSetting";
        public static string ABSiteSettingDataType = "ABSiteSetting";

        public static string GetBucketDocumentKey(string dataType, string uuid)
        {
            return dataType + "|" + uuid;
        }
        public static string GetRawDocumentKey(string bucketDocKey)
        {
            return bucketDocKey.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Last();
        }
        public static string GetQueryViewName(string dataType)
        {
            return "Query_" + dataType;
        }

        public static string GetDataType(string view)
        {
            if (view.StartsWith("Query_"))
            {
                return view.Substring(6);
            }
            return view;
        }

        public static string GetBucketName(this Site site)
        {
            if (site == null || string.IsNullOrEmpty(site.FullName))
            {
                return DatabaseSettings.Instance.DefaultBucketName;
            }
            return string.Join("-", site.RelativePaths.ToArray()).ToLower();
        }

        public static string GetBucketPassword(this Site site)
        {
            if (!string.IsNullOrWhiteSpace(DatabaseSettings.Instance.BucketPassword))
            {
                return DatabaseSettings.Instance.BucketPassword;
            }
            return string.Empty;
        }
        public static string GetDefaultBucketName()
        {
            if (!string.IsNullOrWhiteSpace(DatabaseSettings.Instance.DefaultBucketName))
            {
                return DatabaseSettings.Instance.DefaultBucketName;
            }
            return string.Empty;
        }

        public static string GetViewBody(string dataType)
        {
            return string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(dataType), dataType);
        }
        public static string GetDesignDocumentBody()
        {
            List<string> views = new List<string>();
            views.Add(GetViewBody(ModelExtensions.PageDataType));
            views.Add(GetViewBody(ModelExtensions.PageDraftDataType));
            views.Add(GetViewBody(ModelExtensions.HtmlBlockDataType));
            views.Add(GetViewBody(ModelExtensions.LabelDataType));
            views.Add(GetViewBody(ModelExtensions.LabelCategoryDataType));
            views.Add(GetViewBody(ModelExtensions.UserDataType));
            views.Add(GetViewBody(ModelExtensions.CustomErrorDataType));
            views.Add(GetViewBody(ModelExtensions.UrlRedirectDataType));
            views.Add(GetViewBody(ModelExtensions.ABPageSettingDataType));
            views.Add(GetViewBody(ModelExtensions.ABPageTestResultDataType));
            views.Add(GetViewBody(ModelExtensions.ABRuleSettingDataType));


            return string.Format(@"{{""views"": {{{0}}}}}", string.Join(",", views.ToArray()));
        }
        #region Model To Json
        public static string ToJson(this object o, string dataType)
        {
            var json = JsonConvert.SerializeObject(o, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects });
            IDictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            dict = dict.Merge(DataTypeFieldName, dataType);
            return JsonConvert.SerializeObject(dict);
        }

        public static T JsonToObject<T>(string json)
        {
            var o = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects, ObjectCreationHandling = ObjectCreationHandling.Replace });
            return o;
        }

        #endregion

        #region ToModel
        public static T ToModel<T>(this IViewRow row, Site site, Func<Site, string, T> createModel)
            where T : IPersistable
        {
            var rawValue = row.Info["value"] as Dictionary<string, object>;

            var json = JsonConvert.SerializeObject(rawValue);

            var rawKey = ModelExtensions.GetRawDocumentKey(row.ItemId);

            var dummy = createModel(site, rawKey);

            var model = JsonToObject<T>(json);

            ((IPersistable)model).Init(dummy);

            return model;
        }
        public static T ToModel<T>(Site site, string key, string json, Func<Site, string, T> createModel)
           where T : IPersistable
        {

            var rawKey = ModelExtensions.GetRawDocumentKey(key);

            var dummy = createModel(site, rawKey);

            var model = JsonToObject<T>(json);

            ((IPersistable)model).Init(dummy);

            if (model is ISiteObject)
            {
                ((ISiteObject)model).Site = site;
            }
            return model;
        }
        public static T ToObject<T>(this IViewRow row)
        {
            var rawValue = row.Info["value"] as Dictionary<string, object>;

            var json = JsonConvert.SerializeObject(rawValue);

            var model = JsonToObject<T>(json);

            return model;
        }
        #endregion

    }
}
