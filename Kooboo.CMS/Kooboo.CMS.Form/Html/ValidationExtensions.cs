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
using System.Web;
using System.Globalization;
using Kooboo.CMS.Common;


namespace Kooboo.CMS.Form.Html
{
    public static class ValidationExtensions
    {
        private static IEnumerable<ModelClientValidationRule> GetClientValidationRule(this ColumnValidation validation, string columnName)
        {
            switch (validation.ValidationType)
            {
                case ValidationType.Required:
                    return new[] { new ModelClientValidationRequiredRule(string.IsNullOrEmpty(validation.ErrorMessage) ? "" : string.Format(validation.ErrorMessage, columnName)) };
                case ValidationType.Unique:
                    return new[] { new ModelClientValidationRequiredRule(string.IsNullOrEmpty(validation.ErrorMessage) ? "" : string.Format(validation.ErrorMessage, columnName)) };
                case ValidationType.StringLength:
                    var stringLength = (StringLengthValidation)validation;
                    return new[] { new ModelClientValidationStringLengthRule(string.IsNullOrEmpty(validation.ErrorMessage) ? "" : string.Format(validation.ErrorMessage, columnName), stringLength.Min, stringLength.Max) };
                case ValidationType.Range:
                    var rangeValidation = (RangeValidation)validation;
                    return new[] { new ModelClientValidationRangeRule(string.IsNullOrEmpty(validation.ErrorMessage) ? "" : string.Format(validation.ErrorMessage, columnName), rangeValidation.Start, rangeValidation.End) };
                case ValidationType.Regex:
                    var regexValidation = (RegexValidation)validation;
                    return new[] { new ModelClientValidationRegexRule(string.IsNullOrEmpty(validation.ErrorMessage) ? "" : string.Format(validation.ErrorMessage, columnName), regexValidation.Pattern) };
                default:
                    return new ModelClientValidationRegexRule[0];
            }
        }

        private static IEnumerable<ModelClientValidationRule> GetClientValidationRule(IColumn column)
        {
            if (column.DataType == DataType.Int || column.DataType == DataType.Decimal)
            {
                yield return new ModelClientValidationRule()
                     {
                         ValidationType = "number",
                         ErrorMessage = string.Format(SR.GetString("ClientDataTypeModelValidatorProvider_FieldMustBeNumeric"), column.Name)
                     };
            }
            if (column.DataType == DataType.String && column.Length > 0)
            {
                yield return new ModelClientValidationStringLengthRule(string.Format(SR.GetString("StringLengthAttribute_ValidationError"), column.Name, column.Length)
                    , 0, column.Length);
            }
            if (!column.AllowNull)
            {
                yield return new ModelClientValidationRequiredRule(string.Format(SR.GetString("RequiredAttribute_ValidationError"), column.Name));
            }
            if (column.Validations != null)
            {
                foreach (var validation in column.Validations)
                {
                    foreach (var clientValidation in validation.GetClientValidationRule(column.Name))
                    {
                        yield return clientValidation;
                    }
                }
            }

        }

        /// <summary>
        /// before mvc3
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="column"></param>
        /// <param name="modelName"></param>
        /// <returns></returns>
        private static FieldValidationMetadata ApplyFieldValidationMetadata(HtmlHelper htmlHelper, IColumn column, string modelName)
        {
            FieldValidationMetadata validationMetadataForField = htmlHelper.ViewContext.FormContext.GetValidationMetadataForField(modelName, true);
            if (column.DataType == DataType.Int || column.DataType == DataType.Decimal)
            {
                validationMetadataForField.ValidationRules.Add(new ModelClientValidationRule
                {
                    ValidationType = "number",
                    ErrorMessage = string.Format(SR.GetString("ClientDataTypeModelValidatorProvider_FieldMustBeNumeric"), column.Name)
                });
            }
            if (column.Validations != null)
            {
                foreach (var validation in column.Validations)
                {
                    foreach (var clientValidation in validation.GetClientValidationRule(column.Name))
                    {
                        validationMetadataForField.ValidationRules.Add(clientValidation);
                    }
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

        public static IHtmlString ValidationMessageForColumn(this HtmlHelper htmlHelper, IColumn column, IDictionary<string, object> htmlAttributes)
        {
            var validationMessage = "";
            string fullHtmlFieldName = column.Name;
            FormContext formContextForClientValidation = null;
            if (htmlHelper.ViewContext.ClientValidationEnabled)
            {
                formContextForClientValidation = htmlHelper.ViewContext.FormContext;
            }
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
                if (htmlHelper.ViewContext.UnobtrusiveJavaScriptEnabled)
                {
                    builder.MergeAttribute("data-valmsg-for", fullHtmlFieldName);
                    builder.MergeAttribute("data-valmsg-replace", replaceValidationMessageContents.ToString().ToLowerInvariant());
                }
                else
                {
                    builder.GenerateId(fullHtmlFieldName + "_validationMessage");
                    FieldValidationMetadata metadata = ApplyFieldValidationMetadata(htmlHelper, column, fullHtmlFieldName);
                    metadata.ReplaceValidationMessageContents = string.IsNullOrEmpty(validationMessage);
                    metadata.ValidationMessageId = builder.Attributes["id"];
                }
            }
            return new HtmlString(builder.ToString(TagRenderMode.Normal));
        }

        public static IDictionary<string, object> GetUnobtrusiveValidationAttributes(IColumn column)
        {
            Dictionary<string, object> resultsDictionary = new Dictionary<string, object>();

            string fullHtmlFieldName = column.Name;

            IEnumerable<ModelClientValidationRule> enumerable = GetClientValidationRule(column);
            bool flag = false;
            foreach (ModelClientValidationRule rule in enumerable)
            {
                flag = true;
                string dictionaryKey = "data-val-" + rule.ValidationType;
                //ValidateUnobtrusiveValidationRule(rule, resultsDictionary, dictionaryKey);
                resultsDictionary[dictionaryKey] = HttpUtility.HtmlEncode(rule.ErrorMessage ?? string.Empty);
                dictionaryKey = dictionaryKey + "-";
                foreach (KeyValuePair<string, object> pair in rule.ValidationParameters)
                {
                    resultsDictionary[dictionaryKey + pair.Key] = pair.Value.ToString() ?? string.Empty;
                }
            }
            if (flag)
            {
                resultsDictionary.Add("data-val", "true");
            }

            return resultsDictionary;
        }

        public static string GetUnobtrusiveValidationAttributeString(IColumn column)
        {
            var attributes = ValidationExtensions.GetUnobtrusiveValidationAttributes(column);
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, object> pair in attributes)
            {
                string key = pair.Key;
                if (!string.Equals(key, "id", StringComparison.Ordinal) || pair.Value != null)
                {
                    string str2 = pair.Value.ToString().RazorHtmlAttributeEncode();
                    sb.Append(' ').Append(key).Append("=\"").Append(str2).Append('"');
                }
            }
            return sb.ToString();
        }
    }
}
