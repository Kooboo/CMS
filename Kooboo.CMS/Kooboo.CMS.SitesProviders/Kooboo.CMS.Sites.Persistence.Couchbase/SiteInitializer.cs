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
                System.Threading.Thread.Sleep(3000);
                //此处需暂停几秒钟，否则，通过选择模板创建站点的方式，在导入数据时，会出现数据未导入的情况
                //大致原因在于，Couchbae在数据库创建之后，需要几秒钟的初始化过程，在这个过程中插入任何数据都将失败                
            }
            site.CreateDefaultViews();
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.PageDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.PageDataType), ModelExtensions.PageDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.PageDraftDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.PageDraftDataType), ModelExtensions.PageDraftDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.HtmlBlockDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.HtmlBlockDataType), ModelExtensions.HtmlBlockDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.LabelDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.LabelDataType), ModelExtensions.LabelDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.LabelCategoryDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.LabelCategoryDataType), ModelExtensions.LabelCategoryDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.UserDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.UserDataType), ModelExtensions.UserDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.CustomErrorDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.CustomErrorDataType), ModelExtensions.CustomErrorDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.UrlRedirectDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.UrlRedirectDataType), ModelExtensions.UrlRedirectDataType));

            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.ABPageSettingDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.ABPageSettingDataType), ModelExtensions.ABPageSettingDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.ABPageTestResultDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.ABPageTestResultDataType), ModelExtensions.ABPageTestResultDataType));
            //DatabaseHelper.CreateDesignDocument(bucketName, ModelExtensions.GetQueryViewName(ModelExtensions.ABRuleSettingDataType), string.Format(DataHelper.ViewTemplate, ModelExtensions.GetQueryViewName(ModelExtensions.ABRuleSettingDataType), ModelExtensions.ABRuleSettingDataType));

            System.Threading.Thread.Sleep(3000);
        }
    }
}
