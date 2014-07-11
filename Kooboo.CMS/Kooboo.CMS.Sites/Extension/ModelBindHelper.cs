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
using Kooboo.CMS.Sites.Models;
using System.Collections.Specialized;
using Kooboo.Common.TokenTemplate;


namespace Kooboo.CMS.Sites.Extension
{
    public static class ModelBindHelper
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
        public static bool BindModel<T>(T model, string prefix, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            if (submissionSetting == null)
            {
                return BindModel(model, prefix, controllerContext);
            }
            else
            {
                var formValues = new NameValueCollection(controllerContext.HttpContext.Request.Form);
                var formulaValueProvider = new MvcValueProvider(controllerContext.Controller.ValueProvider);
                formValues = PluginHelper.ApplySubmissionSettings(submissionSetting, formValues, formulaValueProvider);
                ValueProviderCollection valueProvider = new ValueProviderCollection();
                valueProvider.Add(new NameValueCollectionValueProvider(formValues, System.Globalization.CultureInfo.CurrentCulture));
                valueProvider.Add(controllerContext.Controller.ValueProvider);
                IModelBinder binder = ModelBinders.Binders.GetBinder(typeof(T));
                ModelBindingContext bindingContext = new ModelBindingContext
                {
                    ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, typeof(T)),
                    ModelName = prefix,
                    ModelState = controllerContext.Controller.ViewData.ModelState,
                    PropertyFilter = null,
                    ValueProvider = valueProvider
                };
                binder.BindModel(controllerContext, bindingContext);
                return controllerContext.Controller.ViewData.ModelState.IsValid;
            }
        }
    }
}
