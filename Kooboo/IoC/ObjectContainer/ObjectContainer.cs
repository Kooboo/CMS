using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.IoC
{
    public static class ObjectContainer
    {
        static ObjectContainer()
        {
            ObjectFactory = new ObjectFactory();
        
        }

        public static IObjectFacotry ObjectFactory
        {
            get;
            set;
        }

        public static TContract CreateInstance<TContract>(string contractName=null)
        {
            return (TContract)CreateInstance(typeof(TContract),contractName);

        }

        public static TContract TryCreateInstance<TContract>(string contractName =null)
        {
            var export = typeof(TContract);

            try
            {
                var instance = TryCreateInstance(export,contractName);
                if (instance == null)
                {
                    return default(TContract);
                }
                else
                {
                    return (TContract)instance;
                }
            }
            catch
            {
                return default(TContract);
            }

        }

        public static object CreateInstance(Type contractType,string contractName=null)
        {
            var exportType = GetFirstExportType(contractType, contractName);
            if (exportType.IsInterface || exportType.IsAbstract)
            {
                throw new MissingContractException(contractType);
            }
            else
            {
                return Create(exportType);
            }

        }

        public static object TryCreateInstance(Type contractType,string contractName=null)
        {
            try
            {
                var exportType = GetFirstExportType(contractType,contractName);
                if (exportType.IsInterface)
                {
                    return null;
                }
                else
                {
                    return Create(exportType);
                }
            }
            catch
            {
                return null;
            }

        }

        public static IEnumerable<TContract> CreateInstances<TContract>(string contractName=null)
        {
            foreach (var i in CreateInstances(typeof(TContract), contractName))
            {
                yield return (TContract)i;
            }
        }

        public static IEnumerable<object> CreateInstances(Type contractType,string contractName=null)
        {
            foreach (var provider in ExportProviders.Providers)
            {
                var exports = provider.FindExports(contractType).OrderBy(i=>i.Index).AsEnumerable();
                if (string.IsNullOrWhiteSpace(contractName) == false)
                {
                    exports = exports.Where(i => i.Name == contractName);
                }
                foreach (var export in exports)
                {
                    yield return Create(export.ExportType);
                }
            }

        }

        #region Export
        public static ExportContext Export<T>()
        {
            var context = new ExportContext();
            return context.Export<T>();
        }

        public static ExportContext Export(Type type)
        {
            var context = new ExportContext();
            return context.Export(type);
        } 
        #endregion


        volatile static object lockObject = new object();
        static Dictionary<Type, Func<object>> _Handlers;
        static Dictionary<Type, Func<object>> Handlers
        {
            get
            {
                if (_Handlers == null)
                {
                    lock (lockObject)
                    {
                        if (_Handlers == null)
                        {
                            _Handlers = GetHandlers();
                        }
                    }
                }
                return _Handlers;
            }
        }

        public static void InitHandlers()
        {
            _Handlers = null;
        }

        #region Helpers
        static Type GetFirstExportType(Type contractType,string contractName=null)
        {
            var export = FindExports(contractType, contractName).FirstOrDefault();
            if (export == null)
            {
                return contractType;//return itself
            }
            else
            {
                return export.ExportType;
            }
        }

        static IEnumerable<Contract> FindExports(Type contractType, string contractName = null)
        {
            foreach (var provider in ExportProviders.Providers)
            {
                var exports = provider.FindExports(contractType);
                if (string.IsNullOrWhiteSpace(contractName) == false)
                {
                    exports = exports.Where(i => i.Name == contractName);
                }

                
                foreach (var export in exports.OrderBy(i => i.Index))
                {
                    yield return export;
                }              
            }
        }





        static object Create(Type type)
        {
            if (Handlers.ContainsKey(type))//if the type has a initialize delegate,call it to create an instance
            {
                return Handlers[type]();
            } 
            else
            {
                return ObjectFactory.CreateInstance(type);
            }
        }

        static Dictionary<Type, Func<object>> GetHandlers()
        {
            var handlers = new Dictionary<Type, Func<object>>();
            foreach (var export in FindExports(typeof(IActivatorContext)))
            {
                var activator = ObjectFactory.CreateInstance(export.ExportType) as IActivatorContext;
                if (activator != null)
                {
                    foreach (var handler in activator.GetHandlers())
                    {
                        if (handlers.ContainsKey(handler.Type) == false)
                        {
                            handlers.Add(handler.Type, handler.Invoker);
                        }
                    }
                }
            }

            return handlers;
        }
        #endregion
    }
}
