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

using Kooboo.Globalization;
using Kooboo.ComponentModel;

namespace Kooboo.Web.Mvc
{
    public class KoobooDataAnnotationsModelValidatorProvider : System.Web.Mvc.DataAnnotationsModelValidatorProvider
    {
        private class ModelValidatorWrapper : ModelValidator
        {
            public ModelValidatorWrapper(ModelValidator modelValidator, ModelMetadata metadata, ControllerContext controllerContext)
                : base(metadata, controllerContext)
            {
                InnerValidator = modelValidator;
            }
            public ModelValidator InnerValidator { get; private set; }
            public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
            {
                foreach (var item in InnerValidator.GetClientValidationRules())
                {
                    item.ErrorMessage = item.ErrorMessage.Localize();
                    yield return item;
                }

            }
            public override IEnumerable<ModelValidationResult> Validate(object container)
            {
                foreach (var item in InnerValidator.Validate(container))
                {
                    item.Message = item.Message.Localize();
                    yield return item;
                }
            }
        }
        //private class LocalizableAttributeCacheKey
        //{
        //    public LocalizableAttributeCacheKey(Attribute attribute)
        //    {
        //        string cacheFormat = "Attribute HashCode:{0}; CultureName LCID:{1}";
        //        this.CacheKey = string.Format(cacheFormat, attribute.GetHashCode(), System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
        //    }
        //    public string CacheKey { get; private set; }

        //    public override int GetHashCode()
        //    {
        //        return CacheKey.GetHashCode();
        //    }
        //    public override bool Equals(object obj)
        //    {
        //        return this.GetHashCode() == ((LocalizableAttributeCacheKey)obj).GetHashCode();
        //    }
        //}

        //private static readonly List<LocalizableAttributeCacheKey> localized = new List<LocalizableAttributeCacheKey>();

        //protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        //{
        //    foreach (ValidationAttribute attribute in attributes.OfType<ValidationAttribute>())
        //    {
        //        var cacheKey = new LocalizableAttributeCacheKey(attribute);
        //        if (!localized.Contains(cacheKey))
        //        {
        //            lock (localized)
        //            {
        //                if (!localized.Contains(cacheKey))
        //                {
        //                    if (!string.IsNullOrWhiteSpace(attribute.ErrorMessage))
        //                    {
        //                        attribute.ErrorMessage = attribute.ErrorMessage.Localize();
        //                    }
        //                    else
        //                    {
        //                        if (attribute is RequiredAttribute)
        //                        {
        //                            attribute.ErrorMessage = "Required".Localize();
        //                        }
        //                    }
        //                    localized.Add(cacheKey);
        //                }
        //            }
        //        }
        //    }

        //    var validators = base.GetValidators(metadata, context, attributes);

        //    return validators;
        //}
        protected override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context, IEnumerable<Attribute> attributes)
        {
            foreach (var validator in base.GetValidators(metadata, context, attributes))
            {
                yield return new ModelValidatorWrapper(validator, metadata, context);
            }
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
