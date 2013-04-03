using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;

namespace Kooboo.Extensions
{
    public static class DictionaryExtensions
    {
        public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dic)
        {
            NameValueCollection nameValues = new NameValueCollection();
            foreach (var key in dic.Keys)
            {
                nameValues[key.ToString()] = dic[key] == null ? "" : dic[key].ToString();
            }
            return nameValues;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue value)
        {
            dic[key] = value;
            return dic;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> dic1)
        {
            if (dic1 != null)
            {
                foreach (var item in dic1)
                {
                    if (!source.ContainsKey(item.Key))
                    {
                        source.Add(item);
                    }
                }               
            }
            return source;
        }
    }
}
