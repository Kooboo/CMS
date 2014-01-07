#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Reflection;
using NVelocity;
using NVelocity.Runtime;
using NVelocity.Util.Introspection;

namespace Kooboo.CMS.Sites.TemplateEngines.NVelocity.MvcViewEngine
{
	public class ExtensionDuck : IDuck
	{
		private readonly object _instance;
		private readonly Type _instanceType;
		private readonly Type[] _extensionTypes;
		private Introspector _introspector;

		public ExtensionDuck(object instance)
			: this(instance, Type.EmptyTypes)
		{
		}

		public ExtensionDuck(object instance, params Type[] extentionTypes)
		{
			if(instance == null) throw new ArgumentNullException("instance");

			_instance = instance;
			_instanceType = _instance.GetType();
			_extensionTypes = extentionTypes;
		}

		public Introspector Introspector
		{
			get
			{
				if(_introspector == null)
				{
					_introspector = RuntimeSingleton.Introspector;
				}
				return _introspector;
			}
			set { _introspector = value; }
		}

		public object GetInvoke(string propName)
		{
			throw new NotSupportedException();
		}

		public void SetInvoke(string propName, object value)
		{
			throw new NotSupportedException();
		}

		public object Invoke(string method, params object[] args)
		{
			if(string.IsNullOrEmpty(method)) return null;

			MethodInfo methodInfo = Introspector.GetMethod(_instanceType, method, args);
			if(methodInfo != null)
			{
				return methodInfo.Invoke(_instance, args);
			}

			object[] extensionArgs = new object[args.Length + 1];
			extensionArgs[0] = _instance;
			Array.Copy(args, 0, extensionArgs, 1, args.Length);

			foreach(Type extensionType in _extensionTypes)
			{
				methodInfo = Introspector.GetMethod(extensionType, method, extensionArgs);
				if(methodInfo != null)
				{
					return methodInfo.Invoke(null, extensionArgs);
				}
			}

			return null;
		}
	}
}