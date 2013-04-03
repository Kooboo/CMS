using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Search
{
    public interface IObjectConverter
    {
        KeyValuePair<string, string> GetKeyField(object o);

        IndexObject GetIndexObject(object o);

        object GetNativeObject(NameValueCollection fields);

        string GetUrl(object nativeObject);
    }

    public class Converters
    {
        static IDictionary<Type, IObjectConverter> converters = new Dictionary<Type, IObjectConverter>();
        static Converters()
        {
            Register(typeof(TextContent), new TextContentConverter());
        }
        public static void Register(Type type, IObjectConverter converter)
        {
            lock (converters)
            {
                converters[type] = converter;
            }
        }
        public static IObjectConverter GetConverter(Type type)
        {
            return converters[type];
        }
    }
}
