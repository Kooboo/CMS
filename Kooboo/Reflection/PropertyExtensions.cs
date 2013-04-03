using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Diagnostics.Contracts;
using Kooboo.Contracts;
using System.Collections;
namespace Kooboo.Reflection
{
    public class Properties
    {
        public Properties(object o)
        {
            this.Object = o;
        }
        public object Object { get; set; }
        public object this[string name]
        {
            get
            {
                return PropertyExtensions.GetPropery(this.Object, name);
            }
            set
            {
                PropertyExtensions.SetPropery(this.Object, name, value);
            }
        }
    }
    public static class PropertyExtensions
    {
        private static ConcurrentDictionary<string, object> cache = new ConcurrentDictionary<string, object>();
        static Func<object, object> nullGetter = delegate(object o) { return null; };
        static Action<object, object> nullSetter = delegate(object o, object value) { };

        #region Cache
        private static object GetCache(string key, Func<object> createValue)
        {
            object value = null;
            if (cache.TryGetValue(key, out value))
            {
                return value;
            }

            value = cache.GetOrAdd(key, createValue());

            return value;
        }

        #endregion

        #region Non-generic
        /// <summary>
        /// Gets the propery value.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property. example: Property1.Property2</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        public static object GetPropery(object source, string propertyName, bool throwException = true)
        {
#if CONTRACT
            Contract.Requires<ContractException>(source != null, "The source object can not be null.");
            Contract.Requires<ContractException>(!string.IsNullOrEmpty(propertyName), "The propertyName can not be null.");
#endif

            Type type = source.GetType();
            var cacheKey = "GetPropery:" + type.ToString() + "+" + propertyName;

            var getter = (Func<object, object>)GetCache(cacheKey, delegate() { return BuildGet(type, propertyName, throwException); });

            try
            {
                return getter(source);
            }
            catch (Exception e)
            {
                throw new MemberException(e.Message + "PropertyName:" + propertyName, e);
            }

        }

        /// <summary>
        /// Sets the propery.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property. example: Property1.Property2</param>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        public static void SetPropery(object source, string propertyName, object value, bool throwException = true)
        {
#if CONTRACT
            Contract.Requires<ContractException>(source != null, "The source object can not be null.");
            Contract.Requires<ContractException>(!string.IsNullOrEmpty(propertyName), "The propertyName can not be null.");
#endif

            Type type = source.GetType();
            var cacheKey = "SetProperty:" + type.ToString() + "+" + propertyName;

            var setter = (Action<object, object>)GetCache(cacheKey, delegate() { return BuildSet(type, propertyName, throwException); });

            try
            {
                setter(source, value);
            }
            catch (Exception e)
            {
                throw new MemberException(e.Message + "PropertyName:" + propertyName, e);
            }
        }

        #region GetDelegate
        private static PropertyInfo GetPropertyInfo(Type type, string propertyName, bool throwException)
        {
            PropertyInfo pi = type.GetProperty(propertyName);
            if (pi == null && throwException)
            {
                throw new MemberNotFoundException(string.Format("The property name \"{0}\" can not be found in type of \"{1}\"", propertyName, type.ToString()))
                {
                    PropertyName = propertyName,
                    Type = type
                };
            }

            return pi;
        }
        static Action<object, object> BuildSet(Type type, string property, bool throwException)
        {
            string[] props = property.Split('.');
            ParameterExpression arg = Expression.Parameter(typeof(object), "x");
            ParameterExpression valArg = Expression.Parameter(typeof(object), "val");
            Expression expr = Expression.Convert(arg, type);
            foreach (string prop in props.Take(props.Length - 1))
            {
                // use reflection (not ComponentModel) to mirror LINQ 
                var pi = GetPropertyInfo(type, prop, throwException);
                if (pi == null)
                {
                    return nullSetter;
                }
                expr = Expression.Convert(expr, type);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            // final property set...
            PropertyInfo finalProp = GetPropertyInfo(type, props.Last(), throwException);
            if (finalProp == null)
            {
                return nullSetter;
            }
            MethodInfo setter = finalProp.GetSetMethod();
            var argExression = (Expression)Expression.Convert(valArg, finalProp.PropertyType);
            expr = Expression.Call(expr, setter, argExression);
            return Expression.Lambda<Action<object, object>>(expr, arg, valArg).Compile();

        }
        static Func<object, object> BuildGet(Type type, string property, bool throwException)
        {
            string[] props = property.Split('.');
            //Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(typeof(object), "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ 
                var pi = GetPropertyInfo(type, prop, throwException);
                if (pi == null)
                {
                    return nullGetter;
                }
                expr = Expression.Convert(expr, type);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            expr = Expression.Convert(expr, typeof(object));
            return Expression.Lambda<Func<object, object>>(expr, arg).Compile();
        }
        #endregion
        #endregion


        #region Generic

        /// <summary>
        /// Gets the propery value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property. example: Property1.Property2</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        private static TProperty GetStrongTypePropery<T, TProperty>(T source, string propertyName, bool throwException = true)
        {
#if CONTRACT
            Contract.Requires<ContractException>(source != null, "The source object can not be null.");
            Contract.Requires<ContractException>(!string.IsNullOrEmpty(propertyName), "The propertyName can not be null.");
#endif
            Type type = typeof(T);
            Type propertyType = typeof(TProperty);
            var cacheKey = "GetStrongTypePropery:" + type.ToString() + propertyType.ToString() + "+" + propertyName;

            var getter = (Func<T, TProperty>)GetCache(cacheKey, delegate() { return BuildGet<T, TProperty>(propertyName, throwException); });

            try
            {
                return getter(source);
            }
            catch (Exception e)
            {
                throw new MemberException(e.Message + "PropertyName:" + propertyName, e);
            }
        }
        /// <summary>
        /// Sets the propery.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property. example: Property1.Property2</param>
        /// <param name="value">The value.</param>
        /// <param name="throwException">if set to <c>true</c> [throw exception].</param>
        /// <returns></returns>
        private static void SetStrongTypePropery<T, TProperty>(T source, string propertyName, TProperty value, bool throwException = true)
        {
#if CONTRACT
            Contract.Requires<ContractException>(source != null, "The source object can not be null.");
            Contract.Requires<ContractException>(!string.IsNullOrEmpty(propertyName), "The propertyName can not be null.");
#endif

            Type type = typeof(T);
            Type propertyType = typeof(TProperty);
            var cacheKey = "SetStrongTypePropery:" + type.ToString() + propertyType.ToString() + "+" + propertyName;

            var setter = (Action<T, TProperty>)GetCache(cacheKey, delegate() { return BuildSet<T, TProperty>(propertyName, throwException); });

            try
            {
                setter(source, value);
            }
            catch (Exception e)
            {
                throw new MemberException(e.Message + "PropertyName:" + propertyName, e);
            }
        }

        #region Compile
        static Action<T, TValue> BuildSet<T, TValue>(string property, bool throwException)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            ParameterExpression valArg = Expression.Parameter(typeof(TValue), "val");
            Expression expr = arg;
            foreach (string prop in props.Take(props.Length - 1))
            {
                // use reflection (not ComponentModel) to mirror LINQ  
                var pi = GetPropertyInfo(type, prop, throwException);
                if (pi == null)
                {
                    return delegate(T o, TValue p) { };
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            // final property set... 
            PropertyInfo finalProp = GetPropertyInfo(type, props.Last(), throwException);
            if (finalProp == null)
            {
                return delegate(T o, TValue p) { };
            }
            MethodInfo setter = finalProp.GetSetMethod();
            expr = Expression.Call(expr, setter, valArg);
            return Expression.Lambda<Action<T, TValue>>(expr, arg, valArg).Compile();

        }
        static Func<T, TValue> BuildGet<T, TValue>(string property, bool throwException)
        {
            string[] props = property.Split('.');
            Type type = typeof(T);
            ParameterExpression arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (string prop in props)
            {
                // use reflection (not ComponentModel) to mirror LINQ                 
                var pi = GetPropertyInfo(type, prop, throwException);
                if (pi == null)
                {
                    return delegate(T o) { return default(TValue); };
                }
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            return Expression.Lambda<Func<T, TValue>>(expr, arg).Compile();
        }
        #endregion

        #endregion
    }
}
