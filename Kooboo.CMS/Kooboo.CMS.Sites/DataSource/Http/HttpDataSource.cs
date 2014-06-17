#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Collections;
using Kooboo.CMS.Common.Formula;
using Kooboo.Collections.Generic;

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
        private List<CKeyValuePair<string, string>> _formData = new List<CKeyValuePair<string, string>>();
        [DataMember]
        public List<CKeyValuePair<string, string>> FormData
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
        private List<CKeyValuePair<string, string>> _headers = new List<CKeyValuePair<string, string>>();
        [DataMember]
        public List<CKeyValuePair<string, string>> Headers
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

            var httpDataRequest = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IHttpDataRequest>();
            return httpDataRequest.GetData(url, this.HttpMethod, ContentType, form, headers);
        }
        #endregion

        #region KeyValuesToNameValueCollection
        private static NameValueCollection KeyValuesToNameValueCollection(DataSourceContext dataSourceContext, IEnumerable<CKeyValuePair<string, string>> keyValuePairs)
        {
            NameValueCollection values = new NameValueCollection();
            if (keyValuePairs != null && keyValuePairs.Count() > 0)
            {
                foreach (var item in keyValuePairs)
                {
                    if (!string.IsNullOrEmpty(item.Key))
                    {
                        var value = EvaluateStringFormulas(item.Value, dataSourceContext);
                        values[item.Key] = value;
                    }
                }
            }

            return values;
        }
        #endregion

        #region EvaluateStringFormulas
        private class ValueProviderBridge : Kooboo.CMS.Common.Formula.IValueProvider
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
        private static string EvaluateStringFormulas(string formula, DataSourceContext dataSourceContext)
        {
            var formulaParser = new FormulaParser();
            return formulaParser.Populate(formula, new ValueProviderBridge(dataSourceContext.ValueProvider));
        }
        #endregion

        #region GetParameters
        public IEnumerable<string> GetParameters()
        {
            FormulaParser parser = new FormulaParser();

            List<string> parameters = new List<string>();

            if (!string.IsNullOrEmpty(URL))
            {
                var urlParameters = parser.GetParameters(URL);
                parameters.AddRange(urlParameters, StringComparer.OrdinalIgnoreCase);
            }
            ParseKeyValueList(FormData, ref parameters);
            ParseKeyValueList(Headers, ref parameters);

            return parameters;
        }
        private void ParseKeyValueList(IEnumerable<CKeyValuePair<string, string>> keyValues, ref List<string> parameters)
        {
            FormulaParser parser = new FormulaParser();
            if (keyValues != null)
            {
                foreach (var item in keyValues)
                {
                    if (!string.IsNullOrEmpty(item.Key))
                    {
                        var valueParameters = parser.GetParameters(item.Value);
                        parameters.AddRange(valueParameters, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
        }
        #endregion

        #region GetDefinitions
        public IDictionary<string, object> GetDefinitions(DataSourceContext dataSourceContext)
        {
            //可以试着去请求一下，拿到示例结果？
            return new Dictionary<string, object>();
        }
        #endregion


        public bool IsEnumerable()
        {
            return false;
        }
    }
}
