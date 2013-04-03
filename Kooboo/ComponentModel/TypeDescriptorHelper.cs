using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using System.Threading;

namespace Kooboo.ComponentModel
{
    public static class TypeDescriptorHelper
    {
        static Hashtable hashtable = new Hashtable();
        static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        static TypeDescriptorHelper()
        {
            Discover(AppDomain.CurrentDomain.GetAssemblies()
               .Where(it => !it.IsDynamic)
               .Where(it => !it.FullName.StartsWith("System.") && !it.FullName.Contains("Kooboo,"))
               .SelectMany(it =>
               {
                   try { return it.GetTypes(); }
                   catch { return Enumerable.Empty<Type>(); }
               }), true);
        }

        private static void Discover(IEnumerable<Type> types, bool overwrite)
        {
            foreach (var each in types)
            {
                var attributes = each.GetCustomAttributes(typeof(MetadataForAttribute), false);
                if (attributes.Length > 0)
                {
                    var attr = attributes[0] as MetadataForAttribute;
                    if (!hashtable.ContainsKey(attr.Type))
                    {
                        hashtable.Add(attr.Type, each);
                    }
                    else
                    {
                        if (overwrite)
                        {
                            hashtable[attr.Type] = each;
                        }
                    }
                }
            }
        }
        public static void RegisterMetadataType(Type type, Type metadataType)
        {
            locker.EnterWriteLock();

            hashtable[type] = metadataType;

            locker.ExitWriteLock();
        }
        public static ICustomTypeDescriptor Get(Type type)
        {
            try
            {
                locker.EnterReadLock();
                var metadataType = hashtable[type] as Type;
                ICustomTypeDescriptor descriptor = null;
                if (metadataType != null)
                {
                    descriptor = new AssociatedMetadataTypeTypeDescriptionProvider(type, metadataType).GetTypeDescriptor(type);
                }

                return descriptor;
            }
            finally
            {
                locker.ExitReadLock();
            }


        }
    }
}
