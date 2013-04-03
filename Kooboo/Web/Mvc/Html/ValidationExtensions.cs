using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Mvc.Html;
using System.Globalization;
using System.Web.Routing;

namespace Kooboo.Web.Mvc.Html
{
    public static class ValidationExtensions
    {
        private static FieldValidationMetadata ApplyFieldValidationMetadata(HtmlHelper htmlHelper, ModelMetadata modelMetadata, string modelName)
        {
            FieldValidationMetadata validationMetadataForField = htmlHelper.ViewContext.FormContext.GetValidationMetadataForField(modelName, true);
            foreach (IEnumerable<ModelClientValidationRule> rules in
                (from v in modelMetadata.GetValidators(htmlHelper.ViewContext.Controller.ControllerContext) select v.GetClientValidationRules()))
            {
                foreach (var rule in rules)
                {
                    validationMetadataForField.ValidationRules.Add(rule);
                }
            }
            return validationMetadataForField;
        }


        private static string GetInvalidPropertyValueResource(HttpContextBase httpContext)
        {
            string str = null;
            if (!string.IsNullOrEmpty(System.Web.Mvc.Html.ValidationExtensions.ResourceClassKey) && (httpContext != null))
            {
                str = httpContext.GetGlobalResourceObject(System.Web.Mvc.Html.ValidationExtensions.ResourceClassKey, "InvalidPropertyValue", CultureInfo.CurrentUICulture) as string;
            }
            return (str ?? SR.GetString("Common_ValueNotValidForProperty"));
        }



        private static string GetUserErrorMessageOrDefault(HttpContextBase httpContext, ModelError error, ModelState modelState)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
            {
                return error.ErrorMessage;
            }
            if (modelState == null)
            {
                return null;
            }
            string str = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
            return string.Format(CultureInfo.CurrentCulture, GetInvalidPropertyValueResource(httpContext), new object[] { str });
        }


        public static IHtmlString ValidationMessage(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, object htmlAttributes)
        {
            return ValidationMessage(htmlHelper, modelMetadata, new RouteValueDictionary(htmlAttributes));
        }
        public static IHtmlString ValidationMessage(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, IDictionary<string, object> htmlAttributes)
        {
            htmlAttributes = htmlAttributes ?? new RouteValueDictionary();
            var validationMessage = "";
            string fullHtmlFieldName = htmlAttributes["name"] == null ? modelMetadata.PropertyName : htmlAttributes["name"].ToString();
            if (!string.IsNullOrEmpty(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix))
            {
                fullHtmlFieldName = htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix + "." + fullHtmlFieldName;
            }
            FormContext formContextForClientValidation = htmlHelper.ViewContext.FormContext;
            //if (htmlHelper.ViewContext.ClientValidationEnabled)
            //{
            //    formContextForClientValidation = htmlHelper.ViewContext.FormContext;
            //}
            //if (!htmlHelper.ViewData.ModelState.ContainsKey(fullHtmlFieldName) && (formContextForClientValidation == null))
            //{
            //    return null;
            //}
            ModelState modelState = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            ModelErrorCollection errors = (modelState == null) ? null : modelState.Errors;
            ModelError error = ((errors == null) || (errors.Count == 0)) ? null : errors[0];
            if ((error == null) && (formContextForClientValidation == null))
            {
                return null;
            }
            TagBuilder builder = new TagBuilder("span");
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.AddCssClass((error != null) ? HtmlHelper.ValidationMessageCssClassName : HtmlHelper.ValidationMessageValidCssClassName);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                builder.SetInnerText(validationMessage);
            }
            else if (error != null)
            {
                builder.SetInnerText(GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, error, modelState));
            }
            if (formContextForClientValidation != null)
            {
                bool replaceValidationMessageContents = String.IsNullOrEmpty(validationMessage);

                FieldValidationMetadata fieldMetadata = ApplyFieldValidationMetadata(htmlHelper, modelMetadata, fullHtmlFieldName);
                // rules will already have been written to the metadata object
                fieldMetadata.ReplaceValidationMessageContents = replaceValidationMessageContents; // only replace contents if no explicit message was specified

                if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
                {
                    builder.MergeAttribute("data-valmsg-for", fullHtmlFieldName);
                    builder.MergeAttribute("data-valmsg-replace", replaceValidationMessageContents.ToString().ToLowerInvariant());
                }
                else
                {
                    // client validation always requires an ID
                    builder.GenerateId(fullHtmlFieldName + "_validationMessage");
                    fieldMetadata.ValidationMessageId = builder.Attributes["id"];
                }
            }

            //if (formContext != null)
            //{
            //    bool replaceValidationMessageContents = String.IsNullOrEmpty(validationMessage);

            //    FieldValidationMetadata fieldMetadata = ApplyFieldValidationMetadata(htmlHelper, modelMetadata, modelName);
            //    // rules will already have been written to the metadata object
            //    fieldMetadata.ReplaceValidationMessageContents = replaceValidationMessageContents; // only replace contents if no explicit message was specified

            //    if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
            //    {
            //        builder.MergeAttribute("data-valmsg-for", modelName);
            //        builder.MergeAttribute("data-valmsg-replace", replaceValidationMessageContents.ToString().ToLowerInvariant());
            //    }
            //    else
            //    {
            //        // client validation always requires an ID
            //        builder.GenerateId(modelName + "_validationMessage");
            //        fieldMetadata.ValidationMessageId = builder.Attributes["id"];
            //    }
            //}
            return new HtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static IHtmlString ValidationMessageForInput(this HtmlHelper htmlHelper, string inputName)
        {
            return ValidationMessageForInput(htmlHelper, inputName, null);
        }
        public static IHtmlString ValidationMessageForInput(this HtmlHelper htmlHelper, string inputName, IDictionary<string, object> htmlAttributes)
        {
            var validationMessage = "";
            string fullHtmlFieldName = inputName;
            if (!htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
            {
                throw new KoobooException("Kooboo.Web.Mvc.Html.ValidationExtensions.ValidationMessageForInput only support UnobtrusiveJavaScriptEnabled.");
            }
            ModelState modelState = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            ModelErrorCollection errors = (modelState == null) ? null : modelState.Errors;
            ModelError error = ((errors == null) || (errors.Count == 0)) ? null : errors[0];

            TagBuilder builder = new TagBuilder("span");
            builder.MergeAttributes<string, object>(htmlAttributes);
            builder.AddCssClass((error != null) ? HtmlHelper.ValidationMessageCssClassName : HtmlHelper.ValidationMessageValidCssClassName);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                builder.SetInnerText(validationMessage);
            }
            else if (error != null)
            {
                builder.SetInnerText(GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, error, modelState));
            }

            bool replaceValidationMessageContents = String.IsNullOrEmpty(validationMessage);

            builder.MergeAttribute("data-valmsg-for", fullHtmlFieldName);
            builder.MergeAttribute("data-valmsg-replace", replaceValidationMessageContents.ToString().ToLowerInvariant());

            return new HtmlString(builder.ToString(TagRenderMode.Normal));
        }
    }
}
