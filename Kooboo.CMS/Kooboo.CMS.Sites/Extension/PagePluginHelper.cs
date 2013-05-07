#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.Sites.Extension
{
    public static class PagePluginHelper
    {
        public static bool BindModel<T>(T model, ControllerContext controllerContext)
        {
            return BindModel(model, "", controllerContext);
        }
        public static bool BindModel<T>(T model, string prefix, ControllerContext controllerContext)
        {
            IModelBinder binder = ModelBinders.Binders.GetBinder(typeof(T));
            ModelBindingContext bindingContext = new ModelBindingContext
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(T)),
                ModelName = prefix,
                ModelState = controllerContext.Controller.ViewData.ModelState,
                PropertyFilter = null,
                ValueProvider = controllerContext.Controller.ValueProvider
            };
            binder.BindModel(controllerContext, bindingContext);
            return controllerContext.Controller.ViewData.ModelState.IsValid;

        }
    }
}
