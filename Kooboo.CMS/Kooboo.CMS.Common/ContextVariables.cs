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
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Common
{
    public class ContextVariables
    {
        #region fields
        [ThreadStatic]
        private static ContextVariables _current;
        internal readonly static string CONTEXT_VARIABLES = "__CONTEXT_VARIABLES__";

        IDictionary<string, object> RegisteredObjects
        {
            get;
            set;
        }
        #endregion

        #region .ctor
        /// <summary>
        /// Prevents a default instance of the <see cref="CallContext" /> class from being created.
        /// </summary>
        private ContextVariables()
        {
            this.RegisteredObjects = new Dictionary<string, object>();
        }

        #endregion

        #region Cuurent

        public static ContextVariables Current
        {
            get
            {

                if (HttpContext.Current != null)
                {
                    var context = HttpContext.Current.Items[CONTEXT_VARIABLES] as ContextVariables;

                    if (context == null)
                    {
                        context = new ContextVariables();
                        HttpContext.Current.Items.Add(CONTEXT_VARIABLES, context);
                    }
                    return context;
                }
                else
                {
                    if (_current == null)
                    {
                        _current = new ContextVariables();
                    }
                    return _current;
                }
            }
        }
        #endregion

        #region GetObject
        public T GetObject<T>(string key)
        {
            object value = null;
            if (RegisteredObjects.TryGetValue(key, out value))
            {
                return (T)value;
            }
            return default(T);
        }
        #endregion

        #region SetObject
        public void SetObject<T>(string key, T obj)
        {
            RegisteredObjects[key] = obj;
        }
        #endregion
    }
}
