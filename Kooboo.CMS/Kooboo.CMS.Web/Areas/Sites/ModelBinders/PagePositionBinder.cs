using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.ModelBinders
{
    public class PagePositionBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var positionType = controllerContext.HttpContext.Request["PositionType"];
            object model = null;
            switch (positionType)
            {
                case "View":
                    model = new ViewPosition();
                    break;
                case "Module":
                    model = new ModulePosition();
                    break;
                case "Content":
                    model = new HtmlPosition();
                    break;
                default:
                    break;
            }
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
            return model;
        }
    }        
}