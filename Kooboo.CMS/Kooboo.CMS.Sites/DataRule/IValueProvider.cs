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
using System.Collections.Specialized;
using System.Web.Mvc;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Models;
using System.Collections.ObjectModel;
using System.Web;

namespace Kooboo.CMS.Sites.DataRule
{
    public interface IValueProvider
    {
        object GetValue(string name);
    }

    public class NameValueCollectionValueProvider : IValueProvider
    {
        public NameValueCollectionValueProvider(NameValueCollection queryString)
        {
            this.QueryString = queryString;
        }
        public NameValueCollection QueryString { get; private set; }

        public object GetValue(string name)
        {
            return QueryString[name];
        }
    }

    public class DictionaryValueProvider<T> : IValueProvider
    {
        public DictionaryValueProvider(IDictionary<string, T> dic)
        {
            this.Dictionary = dic;
        }

        public IDictionary<string, T> Dictionary { get; private set; }

        public object GetValue(string name)
        {
            if (Dictionary != null)
            {
                if (Dictionary.ContainsKey(name))
                {
                    var value = Dictionary[name];
                    return value == null ? null : value.ToString();
                }
            }
            return null;
        }
    }

    public class QueryStringValueProvider : NameValueCollectionValueProvider
    {
        public QueryStringValueProvider(ControllerContext controllerContext)
            : base(controllerContext.HttpContext.Request.QueryString)
        {

        }
    }

    public class RouteValueProvider : DictionaryValueProvider<object>
    {
        public RouteValueProvider(PageRequestContext pageRequestContext)
            : base(pageRequestContext.RouteValues)
        {
        }
    }

    public class FormValueProvider : NameValueCollectionValueProvider
    {
        public FormValueProvider(ControllerContext controllerContext)
            : base(controllerContext.HttpContext.Request.Form)
        { }
    }

    public class ViewParameterValueProvider : IValueProvider
    {
        public ViewParameterValueProvider(IDictionary<string, object> paramters)
        {
            this.ViewParameters = paramters ?? new Dictionary<string, object>();
        }
        public IDictionary<string, object> ViewParameters { get; private set; }


        public object GetValue(string name)
        {
            if (ViewParameters.ContainsKey(name))
            {
                return ViewParameters[name];
            }
            return null;
        }
    }

    public class ValueProviderCollection : Collection<IValueProvider>, IValueProvider
    {
        public ValueProviderCollection()
        {

        }
        public ValueProviderCollection(IList<IValueProvider> list)
            : base(list)
        {

        }
        public object GetValue(string name)
        {
            object value = null;
            foreach (var item in this)
            {
                value = item.GetValue(name);
                if (value != null)
                {
                    break;
                }
            }
            return value;
        }
    }

    public class SessionValueProvider : IValueProvider
    {
        public SessionValueProvider(ControllerContext controllerContext)
        {
            this.Session = controllerContext.HttpContext.Session;
        }

        public HttpSessionStateBase Session { get; private set; }

        public object GetValue(string name)
        {
            if (Session != null)
            {
                return Session[name];
            }
            return null;
        }
    }

    public class MVCValueProviderWrapper : IValueProvider
    {
        System.Web.Mvc.IValueProvider valueProvider;
        public MVCValueProviderWrapper(System.Web.Mvc.IValueProvider valueProvider)
        {
            this.valueProvider = valueProvider;
        }

        public object GetValue(string name)
        {
            var result = valueProvider.GetValue(name);
            if (result != null)
            {
                return result.RawValue;
            }
            return null;
        }
    }
}
