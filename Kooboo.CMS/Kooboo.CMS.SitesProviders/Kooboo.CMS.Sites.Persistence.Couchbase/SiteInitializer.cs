using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
{
    public class SiteInitializer
    {
        public virtual void Initialize(Site site)
        {
            var bucketName = site.GetBucketName();
            if (!DatabaseHelper.ExistBucket(bucketName))
            {
                DatabaseHelper.CreateBucket(bucketName);
            }
            string viewsTemplate = @"{{
                    ""views"": {{
                        {0}
                    }}
            }}";
            string viewTemplate = @"""{0}"":{{""map"":""function(doc,meta){{if(doc._datatype_==='{1}'){{emit(meta.id,doc);}}}}""}}";
            List<string> views = new List<string>();
            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), ModelExtensions.PageDataType));
            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.PageDraftDataType), ModelExtensions.PageDraftDataType));
            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.HtmlBlockDataType), ModelExtensions.HtmlBlockDataType));
            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.LabelDataType), ModelExtensions.LabelDataType));
            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.LabelCategoryDataType), ModelExtensions.LabelCategoryDataType));
            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.UserDataType), ModelExtensions.UserDataType));

            string document = string.Format(viewsTemplate, string.Join(",", views.ToArray()));
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.DesignName, document);
        }
    }
}
