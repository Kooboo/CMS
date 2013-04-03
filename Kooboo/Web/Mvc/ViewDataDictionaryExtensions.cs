using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc
{
    public static class ViewDataDictionaryExtensions
    {
        public static ViewDataDictionary Merge(this ViewDataDictionary source, ViewDataDictionary dic1)
        {
            if (dic1 != null)
            {
                foreach (KeyValuePair<string, object> pair in dic1)
                {
                    if (!source.ContainsKey(pair.Key))
                    {
                        source.Add(pair.Key, pair.Value);
                    }
                }
                foreach (KeyValuePair<string, ModelState> pair2 in dic1.ModelState)
                {
                    if (!source.ModelState.ContainsKey(pair2.Key))
                    {
                        source.ModelState.Add(pair2.Key, pair2.Value);
                    }
                }
                if (source.Model == null)
                {
                    source.Model = dic1.Model;
                }
                if (source.TemplateInfo == null)
                {
                    source.TemplateInfo = dic1.TemplateInfo;
                }
                if (source.ModelMetadata == null)
                {
                    source.ModelMetadata = dic1.ModelMetadata;
                }
            }
            return source;
        }
    }
}
