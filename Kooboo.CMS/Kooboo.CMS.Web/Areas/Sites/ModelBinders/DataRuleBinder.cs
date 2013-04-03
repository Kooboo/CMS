using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.CMS.Web.Areas.Sites.ModelBinders
{
    public class DataRuleBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            string dataRuleTypeStr;

            string dataruleTypeName = !string.IsNullOrWhiteSpace(bindingContext.ModelName) ? (bindingContext.ModelName + ".DataRuleType") : "DataRuleType";

            dataRuleTypeStr = controllerContext.HttpContext.Request[dataruleTypeName];
            //if (string.IsNullOrEmpty(controllerContext.HttpContext.Request["DataRule.DataRuleType"]))
            //{
            //    dataRuleTypeStr = controllerContext.HttpContext.Request[bindingContext.ModelName + ".DataRule.DataRuleType"];
            //}
            //else
            //{
            //    dataRuleTypeStr = controllerContext.HttpContext.Request["DataRule.DataRuleType"];
            //}
            if (string.IsNullOrEmpty(dataRuleTypeStr))
            {
                return null;
            }
            var dataRuleInt = Int32.Parse(dataRuleTypeStr);
            DataRuleType dataRuleTypeEnum = (DataRuleType)dataRuleInt;
            object model = null;
            switch (dataRuleTypeEnum)
            {
                case DataRuleType.Folder:
                    model = new FolderDataRule();
                    break;
                case DataRuleType.Schema:
                    model = new SchemaDataRule();
                    break;
                case DataRuleType.Category:
                    model = new CategoryDataRule();
                    break;
                //case DataRuleType.Categorizable:
                //    model = new CategorizableDataRule();
                //    break;
            }
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            return model;
        }
    }
}