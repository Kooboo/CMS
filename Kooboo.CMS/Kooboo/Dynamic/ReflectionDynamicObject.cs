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
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kooboo.Dynamic
{
    public class ReflectionDynamicObject : DynamicObject
    {
        #region private fields
        /// <summary>
        /// The object we are going to wrap
        /// </summary>
        object _wrapped;

        /// <summary>
        /// Specify the flags for accessing members
        /// </summary>
        static BindingFlags flags = BindingFlags.Instance
            | BindingFlags.Static | BindingFlags.Public;

        #endregion

        #region .ctor
        /// <summary>
        /// Create a simple private wrapper
        /// </summary>
        public ReflectionDynamicObject(object o)
        {
            _wrapped = o;
        }


        public ReflectionDynamicObject(Type t)
        {
            _wrapped = t;
        }
        #endregion

        #region WrappedType
        Type WrappedType
        {
            get
            {
                return (_wrapped is Type)
                    ? _wrapped as Type
                    : _wrapped.GetType();
            }
        } 
        #endregion

        #region GetActualType
        public Type GetActualType(object t)
        {
            return (t is ParameterInfo)
                        ? (t as ParameterInfo).ParameterType : t.GetType();
        } 
        #endregion

        #region TryInvokeMember
        /// <summary>
        /// Try invoking a method
        /// </summary>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var types = from a in args
                        select GetActualType(a);

            var method = WrappedType.GetMethod
                (binder.Name, flags, null, types.ToArray(), null);

            if (method == null)
            {
                result = null;
                return false;
            }
            else
            {
                result = method.Invoke(_wrapped, args);
                return true;
            }
        } 
        #endregion

        #region TryGetMember
        /// <summary>
        /// Tries to get a property or field with the given name
        /// </summary>
        public override bool TryGetMember(System.Dynamic.GetMemberBinder binder, out object result)
        {
            //Try getting a property of that name
            var prop = WrappedType.GetProperty(binder.Name, flags);

            if (prop == null)
            {
                //Try getting a field of that name
                var fld = WrappedType.GetField(binder.Name, flags);
                if (fld != null)
                {
                    result = fld.GetValue(_wrapped);
                    return true;
                }
                else
                {
                    result = null;
                    return false;
                }
            }
            else
            {
                result = prop.GetValue(_wrapped, null);
                return true;
            }
        } 
        #endregion

        #region TrySetMember
        /// <summary>
        /// Tries to set a property or field with the given name
        /// </summary>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var prop = WrappedType.GetProperty(binder.Name, flags);
            if (prop == null)
            {
                var fld = WrappedType.GetField(binder.Name, flags);
                if (fld != null)
                {
                    fld.SetValue(_wrapped, value);
                    return true;
                }
                else
                    return false;
            }
            else
            {
                prop.SetValue(_wrapped, value, null);
                return true;
            }
        } 
        #endregion
    }
}
