using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.VSExtensionTemplate.Extensions.MembershipConnectClient
{
    public static class DictionaryExtensions
    {
        public static void AddItemIfNotEmpty(this IDictionary<string, string> dictionary, string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            if (!string.IsNullOrEmpty(value))
            {
                dictionary[key] = value;
            }
        }

    }
}
