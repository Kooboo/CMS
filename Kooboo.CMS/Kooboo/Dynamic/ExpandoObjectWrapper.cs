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
using System.Text;

namespace Kooboo.Dynamic
{
    public class ExpandoObjectWrapper : DynamicObject
    {
        #region private fields
        DynamicObject _wrapped;
        Dictionary<string, object> properties = new Dictionary<string, object>();
        Dictionary<string, Delegate> methods = new Dictionary<string, Delegate>();
        #endregion

        #region ExpandoObjectWrapper
        public ExpandoObjectWrapper(object o)
        {
            if (o is DynamicObject)
            {
                _wrapped = (DynamicObject)o;
            }
            else
            {
                _wrapped = new ReflectionDynamicObject(o);
            }
        }
        #endregion

        #region TryGetMember
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;
            if (properties.ContainsKey(binder.Name))
            {
                result = properties[binder.Name];
                return true;
            }
            var getMethod = "get_" + binder.Name;

            if (methods.ContainsKey(getMethod) && methods[getMethod].Method.ReturnType != typeof(void))
            {
                result = methods[getMethod].DynamicInvoke();
                return true;
            }

            return _wrapped.TryGetMember(binder, out result);

        } 
        #endregion

        #region TryInvokeMember
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (methods.ContainsKey(binder.Name))
            {
                result = methods[binder.Name].DynamicInvoke(args);
                return true;
            }
            return _wrapped.TryInvokeMember(binder, args, out result);
        } 
        #endregion

        #region TrySetMember
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var set = _wrapped.TrySetMember(binder, value);
            if (!set)
            {
                var setMethod = "set_" + binder.Name;
                if (methods.ContainsKey(setMethod))
                {
                    methods[setMethod].DynamicInvoke(value);
                }
                if (value is Delegate)
                {
                    methods[binder.Name] = (Delegate)value;
                }
                else
                {
                    properties[binder.Name] = value;
                }
                set = true;
            }
            return true;
        } 
        #endregion
    }
}
