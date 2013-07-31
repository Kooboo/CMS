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
using System.ComponentModel.DataAnnotations;
using Kooboo.ComponentModel;

namespace Kooboo.Web.Mvc
{
    public class KoobooDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var baseModelMetadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            var result = new KoobooModelMetadata(this, containerType, modelAccessor, modelType, propertyName,
                attributes.OfType<DisplayColumnAttribute>().FirstOrDefault(), attributes)
            {
                TemplateHint = baseModelMetadata.TemplateHint,
                HideSurroundingHtml = baseModelMetadata.HideSurroundingHtml,
                DataTypeName = baseModelMetadata.DataTypeName,
                IsReadOnly = baseModelMetadata.IsReadOnly,
                NullDisplayText = baseModelMetadata.NullDisplayText,
                DisplayFormatString = baseModelMetadata.DisplayFormatString,
                ConvertEmptyStringToNull = false,
                EditFormatString = baseModelMetadata.EditFormatString,
                ShowForDisplay = baseModelMetadata.ShowForDisplay,
                ShowForEdit = baseModelMetadata.ShowForEdit,
                DisplayName = baseModelMetadata.DisplayName,
                IsRequired = baseModelMetadata.IsRequired
            };
            return result;
        }
        protected override System.ComponentModel.ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {
            var descriptor = TypeDescriptorHelper.Get(type);
            if (descriptor == null)
            {
                descriptor = base.GetTypeDescriptor(type);
            }
            return descriptor;
        }
    }

}
