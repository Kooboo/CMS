#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Collections;
using Kooboo.Common.TokenTemplate;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.Http
{
    [DataContract(Name = "HttpDataSource")]
    [KnownTypeAttribute(typeof(HttpDataSource))]
    public class HttpDataSource : IDataSource
    {
        /// <summary>
        /// josn?key={key}
        /// </summary>
        [DataMember]
        public string URL { get; set; }

        private string _httpMethod = "GET";
        [DataMember]
        public string HttpMethod { get { return _httpMethod; } set { _httpMethod = value; } }
        private Dictionary<string, string> _formData = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> FormData
        {
            get
            {
                return _formData;
            }
            set
            {
                _formData = value;
            }
        }
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        [DataMember]
        public Dictionary<string, string> Headers
        {
            get
            {
                return _headers;
            }
            set
            {
                _headers = value;
            }
        }
        [DataMember]
        public ResponseDataType ResponseDataType { get; set; }
        [DataMember]
        public string ContentType { get; set; }

        #region Execute
        public object Execute(DataSourceContext dataSourceContext)
        {
            var url = EvaluateStringFormulas(this.URL, dataSourceContext);
            NameValueCollection form = KeyValuesToNameValueCollection(dataSourceContext, this.FormData);
            NameValueCollection headers = KeyValuesToNameValueCollection(dataSourceContext, this.Headers);

            var httpDataRequest = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IHttpDataRequest>();
            return httpDataRequest.GetData(url, this.HttpMethod, ContentType, form, headers);
        }
        #endregion

        #region KeyValuesToNameValueCollection
        private static NameValueCollection KeyValuesToNameValueCollection(DataSourceContext dataSourceContext, Dictionary<string, string> keyValuePairs)
        {
            NameValueCollection values = new NameValueCollection();
            if (keyValuePairs != null)
            {
                return keyValuePairs.ToNameValueCollection();
            }

            return values;
        }
        #endregion

        #region EvaluateStringFormulas
        private class ValueProviderBridge : Kooboo.Common.TokenTemplate.IValueProvider
        {
            System.Web.Mvc.IValueProvider _valueProvider;
            public ValueProviderBridge(System.Web.Mvc.IValueProvider valueProvider)
            {
                _valueProvider = valueProvider;
            }
            public object GetValue(string name)
            {
                var result = _valueProvider.GetValue(name);

                if (result != null)
                {
                    return result.AttemptedValue;
                }
                return null;
            }
        }
        private static string EvaluateStringFormulas(string templateString, DataSourceContext dataSourceContext)
        {
            var templateParser = new TemplateParser();
            return templateParser.Merge(templateString, new ValueProviderBridge(dataSourceContext.ValueProvider));
        }
        #endregion


        #region GetParameters
        public IEnumerable<string> GetParameters()
        {
            ITemplateParser templateParser = new TemplateParser();

            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(URL))
            {
                var urlParameters = templateParser.GetTokens(URL);
                parameters.AddRange(urlParameters, StringComparer.OrdinalIgnoreCase);
            }
            ParseKeyValueList(FormData, ref parameters);
            ParseKeyValueList(Headers, ref parameters);

            return parameters;
        }
        private void ParseKeyValueList(Dictionary<string, string> keyValues, ref List<string> parameters)
        {
            ITemplateParser templateParser = new TemplateParser();
            if (keyValues != null)
            {
                foreach (var item in keyValues)
                {
                    if (!string.IsNullOrEmpty(item.Key))
                    {
                        var valueParameters = templateParser.GetTokens(item.Value);
                        parameters.AddRange(valueParameters, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
        }
        #endregion
    }
}
