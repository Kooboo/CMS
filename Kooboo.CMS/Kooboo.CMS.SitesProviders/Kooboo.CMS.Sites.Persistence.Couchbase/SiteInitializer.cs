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
                //此处需暂停几秒钟，否则，通过选择模板创建站点的方式，在导入数据时，会出现数据未导入的情况
                //大致原因在于，Couchbae在数据库创建之后，需要几秒钟的初始化过程，在这个过程中插入任何数据都将失败                
            }
            //            string viewsTemplate = @"{{
            //                    ""views"": {{
            //                        {0}
            //                    }}
            //            }}";
            //            string viewTemplate = @"""{0}"":{{""map"":""function(doc,meta){{if(doc._datatype_==='{1}'){{emit(meta.id,doc);}}}}""}}";
            //            List<string> views = new List<string>();
            //            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), ModelExtensions.PageDataType));
            //            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.PageDraftDataType), ModelExtensions.PageDraftDataType));
            //            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.HtmlBlockDataType), ModelExtensions.HtmlBlockDataType));
            //            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.LabelDataType), ModelExtensions.LabelDataType));
            //            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.LabelCategoryDataType), ModelExtensions.LabelCategoryDataType));
            //            views.Add(string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.UserDataType), ModelExtensions.UserDataType));

            //string document = string.Format(viewsTemplate, string.Join(",", views.ToArray()));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.DesignName, document);


            string viewTemplate = @"{{""views"":{{""{0}"":{{""map"":""function(doc,meta){{if(doc._datatype_==='{1}'){{emit(meta.id,null);}}}}""}}}}}}";
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.PageDataType), ModelExtensions.PageDataType));
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryView(ModelExtensions.PageDraftDataType), string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.PageDraftDataType), ModelExtensions.PageDraftDataType));
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryView(ModelExtensions.HtmlBlockDataType), string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.HtmlBlockDataType), ModelExtensions.HtmlBlockDataType));
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryView(ModelExtensions.LabelDataType), string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.LabelDataType), ModelExtensions.LabelDataType));
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryView(ModelExtensions.LabelCategoryDataType), string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.LabelCategoryDataType), ModelExtensions.LabelCategoryDataType));
            DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryView(ModelExtensions.UserDataType), string.Format(viewTemplate, ModelExtensions.GetQueryView(ModelExtensions.UserDataType), ModelExtensions.UserDataType));
            System.Threading.Thread.Sleep(3000);
        }
    }
}
