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
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.ObjectContainer;
using System.Web.Helpers;
using Kooboo.Common.Data;
namespace Kooboo.CMS.Web.Areas.Sites.ModelBinders
{
    public class ParameterBinder : DefaultModelBinder
    {
        protected override object GetPropertyValue(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor, IModelBinder propertyBinder)
        {
            if (propertyDescriptor.Name.EqualsOrNullEmpty("Value", StringComparison.CurrentCultureIgnoreCase))
            {
                var value = controllerContext.RequestContext.HttpContext.Request.Unvalidated().Form[bindingContext.ModelName];
                var dataTypeModelName = bindingContext.ModelName.Replace("Value", "DataType");
                var dataType = (DataType)Enum.Parse(typeof(DataType), bindingContext.ValueProvider.GetValue(dataTypeModelName).AttemptedValue);
                var parameterValue = DataTypeHelper.ParseValue(dataType, value, false);
                return parameterValue;
            }
            else
                return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
        }
    }
}