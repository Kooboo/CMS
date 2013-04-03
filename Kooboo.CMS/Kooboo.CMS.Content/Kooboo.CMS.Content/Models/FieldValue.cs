using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Content.Models
{
    [DataContract]
    public class FieldValue
    {
        [DataMember(Order = 1)]
        public string FieldName { get; set; }
        [DataMember(Order = 3)]
        public string Value { get; set; }
    }

    public static class FieldValueFilterHelper
    {
        public static bool EvaluateByRegex(this IEnumerable<FieldValue> filters, ContentBase content)
        {
            foreach (var field in filters)
            {
                if (content.ContainsKey(field.FieldName))
                {
                    var value = content[field.FieldName];
                    if (string.IsNullOrEmpty(field.Value))
                    {
                        if (value == null || string.IsNullOrEmpty(value.ToString()))
                        {
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (Regex.IsMatch(value.ToString(), field.Value, RegexOptions.IgnoreCase))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
