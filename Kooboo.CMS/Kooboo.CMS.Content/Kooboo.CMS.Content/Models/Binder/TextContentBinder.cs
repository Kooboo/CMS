using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Kooboo.CMS.Form;
using Kooboo.Data;
using Kooboo.CMS.Form.Html.Controls;
using Kooboo.Globalization;

namespace Kooboo.CMS.Content.Models.Binder
{
    public class TextContentBinder : ITextContentBinder
    {
        protected TextContentBinder()
        { }
        static TextContentBinder()
        {
            DefaultBinder = new TextContentBinder();
        }
        public static TextContentBinder DefaultBinder { get; set; }
        #region ITextContentBinder Members
        public TextContent Default(Schema schema)
        {
            List<RuleViolation> violations = new List<RuleViolation>();
            TextContent textContent = new TextContent();

            foreach (Column column in ((ISchema)(schema.AsActual())).Columns)
            {
                if (!string.IsNullOrEmpty(column.DefaultValue))
                {
                    ParseColumnValue(textContent, ref violations, column, column.DefaultValue);
                }
                else
                {
                    textContent[column.Name] = DataTypeHelper.DefaultValue(column.DataType);
                }
            }

            return textContent;
        }

        public virtual TextContent Bind(Schema schema, TextContent textContent, System.Collections.Specialized.NameValueCollection values)
        {
            return Bind(schema, textContent, values, false, true);
        }
        public virtual TextContent Bind(Schema schema, TextContent textContent, System.Collections.Specialized.NameValueCollection values, bool thorwViolationException)
        {
            return Bind(schema, textContent, values, false, thorwViolationException);
        }
        public virtual TextContent Bind(Schema schema, TextContent textContent, System.Collections.Specialized.NameValueCollection values, bool update, bool thorwViolationException)
        {
            List<RuleViolation> violations = new List<RuleViolation>();
            schema = schema.AsActual();
            //do not to create a new content instance
            //it will interrupt the object state for ravendb.
            //textContent = new TextContent(textContent);

            foreach (Column column in ((ISchema)(schema.AsActual())).Columns.Where(it => !string.IsNullOrEmpty(it.ControlType)))
            {
                var value = values[column.Name];
                //IControl control = column.GetControlType();
                //// 
                //if (control != null)
                //{
                //    value = control.GetValue(textContent[column.Name], value);
                //}
                // Update content will keep the old values;
                if (value == null && update == true)
                {
                    continue;
                }
                //postedData[column.Name] = value;
                if (string.IsNullOrEmpty(value))
                {
                    if (column.AllowNull)
                    {
                        textContent[column.Name] = null;
                    }
                    else
                    {
                        violations.Add(new RuleViolation(column.Name, null, string.Format("The field {0} cannot be null or empty.", column.Name)));
                    }
                }
                else
                {
                    ParseColumnValue(textContent, ref violations, column, value);
                }

                ValidateColumn(textContent, ref violations, column);
            }
            if (thorwViolationException && violations.Count > 0)
            {
                throw new RuleViolationException(textContent, violations);
            }
            return textContent;
        }
        public virtual TextContent Update(Schema schema, TextContent textContent, System.Collections.Specialized.NameValueCollection values)
        {
            return Bind(schema, textContent, values, true);
        }
        public object ConvertToColumnType(Schema schema, string field, string value)
        {
            schema = schema.AsActual();
            var column = schema.AllColumns.Where(it => it.Name.EqualsOrNullEmpty(field, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (column == null)
            {
                return value;
            }
            return ConvertToColumnType(column, value);
        }
        private static object ConvertToColumnType(Column column, string value)
        {
            switch (column.DataType)
            {
                case DataType.String:
                    return value;
                case DataType.Int:
                    int intValue;
                    if (int.TryParse(value, out intValue))
                    {
                        return intValue;
                    }
                    else
                    {
                        return null;
                    }
                case DataType.Decimal:
                    decimal decValue;
                    if (decimal.TryParse(value, out decValue))
                    {
                        return decValue;
                    }
                    else
                    {
                        return null;
                    }
                case DataType.DateTime:
                    DateTime dateTime;
                    if (DateTimeHelper.TryParse(value, out dateTime))
                    {
                        dateTime = dateTime.ToUniversalTime();                                                
                        return dateTime;                       
                    }
                    else
                    {
                        return null;
                    }
                case DataType.Bool:

                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Split(',').First();
                        bool boolValue;
                        if (bool.TryParse(value, out boolValue))
                        {
                            return boolValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        if (!column.AllowNull)
                        {
                            return false;
                        }
                    }
                    return null;
                default:
                    return null;
            }
        }

        private static void ParseColumnValue(TextContent textContent, ref List<RuleViolation> violations, Column column, string value)
        {
            switch (column.DataType)
            {
                case DataType.String:
                    if (!string.IsNullOrEmpty(value) && column.Length > 0 && value.Length > column.Length)
                    {
                        value = value.Substring(0, column.Length);
                    }
                    textContent[column.Name] = value;
                    break;
                case DataType.Int:
                    int intValue;
                    if (int.TryParse(value, out intValue))
                    {
                        textContent[column.Name] = intValue;
                    }
                    else
                    {
                        violations.Add(new RuleViolation(column.Name, value, string.Format("The field {0} is a invalid int value.".Localize(), column.Name)));
                    }
                    break;
                case DataType.Decimal:
                    decimal decValue;
                    if (decimal.TryParse(value, out decValue))
                    {
                        textContent[column.Name] = decValue;
                    }
                    else
                    {
                        violations.Add(new RuleViolation(column.Name, value, string.Format("The field {0} is a invalid decimal value.".Localize(), column.Name)));
                    }
                    break;
                case DataType.DateTime:
                    DateTime dateTime;
                    if (DateTimeHelper.TryParse(value, out dateTime))
                    {
                        dateTime = dateTime.ToUniversalTime(); 
                        textContent[column.Name] = dateTime;
                    }
                    else
                    {
                        violations.Add(new RuleViolation(column.Name, value, string.Format("The field {0} is a invalid date value.".Localize(), column.Name)));
                    }
                    break;
                case DataType.Bool:

                    if (!string.IsNullOrEmpty(value))
                    {
                        value = value.Split(',').First();
                        bool boolValue;
                        if (bool.TryParse(value, out boolValue))
                        {
                            textContent[column.Name] = boolValue;
                        }
                        else
                        {
                            violations.Add(new RuleViolation(column.Name, value, string.Format("The field {0} is a invalid bool value.".Localize(), column.Name)));
                        }
                    }
                    else
                    {
                        if (!column.AllowNull)
                        {
                            textContent[column.Name] = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private static void ValidateColumn(TextContent textContent, ref List<RuleViolation> violations, Column column)
        {
            if (column.Validations != null)
            {
                foreach (var validation in column.Validations)
                {
                    switch (validation.ValidationType)
                    {
                        case ValidationType.Required:
                            if (textContent[column.Name] == null || string.IsNullOrEmpty(textContent[column.Name].ToString()))
                            {
                                violations.Add(new RuleViolation(column.Name, null, string.Format(validation.ErrorMessage, column.Name)));
                            }
                            break;
                        case ValidationType.StringLength:
                            var stringLength = (StringLengthValidation)validation;
                            if (column.DataType == DataType.String && textContent[column.Name] != null)
                            {
                                var length = textContent[column.Name].ToString().Length;
                                if (length < stringLength.Min || length > stringLength.Max)
                                {
                                    violations.Add(new RuleViolation(column.Name, null, string.Format(validation.ErrorMessage, column.Name)));
                                }

                            }
                            break;
                        case ValidationType.Range:
                            var rangeValidation = (RangeValidation)validation;
                            if (textContent.ContainsKey(column.Name) && textContent[column.Name] != null)
                            {
                                if ((column.DataType == DataType.Int || column.DataType == DataType.Decimal))
                                {
                                    decimal decimalValue = Convert.ToDecimal(textContent[column.Name]);

                                    decimal start = Convert.ToDecimal(rangeValidation.Start);
                                    decimal end = Convert.ToDecimal(rangeValidation.End);

                                    if (start > decimalValue || end < decimalValue)
                                    {
                                        violations.Add(new RuleViolation(column.Name, null, string.Format(validation.ErrorMessage, column.Name)));
                                    }

                                }
                            }
                            break;
                        case ValidationType.Regex:
                            var regexValidation = (RegexValidation)validation;
                            if (textContent.ContainsKey(column.Name) && textContent[column.Name] != null)
                            {
                                string pattern = regexValidation.Pattern;
                                if (!string.IsNullOrEmpty(pattern))
                                {
                                    if (!Regex.IsMatch(textContent[column.Name].ToString(), pattern))
                                    {
                                        violations.Add(new RuleViolation(column.Name, null, string.Format(validation.ErrorMessage, column.Name)));
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        #endregion


    }
}
