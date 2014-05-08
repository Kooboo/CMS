#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataSource.ValueProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.ContentDataSource
{
    public static class ParameterizedFieldValue
    {
        public static string GetFieldValue(string field, IValueProvider valueProvider)
        {
            string fieldName;
            return GetFieldValue(field, valueProvider, out fieldName);
        }
        public static string GetFieldValue(string field, IValueProvider valueProvider, out string fieldName)
        {
            fieldName = "";
            if (string.IsNullOrEmpty(field))
            {
                return field;
            }
            if (field.StartsWith("{{") && field.EndsWith("}}"))
            {
                return "{" + field.Substring(2, field.Length - 4) + "}";
            }
            if (field.StartsWith("{") && field.EndsWith("}"))
            {
                fieldName = field.Substring(1, field.Length - 2);
                var value = valueProvider.GetValue(fieldName);
                return value == null ? null : value.ToString();
            }
            return field;
        }

        public static bool IsParameterizedField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return false;
            }
            return fieldName.StartsWith("{") && fieldName.EndsWith("}");
        }
    }
}
