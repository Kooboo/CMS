#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace Kooboo.ComponentModel
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeDescriptorHelper
    {
        #region Fields
        static Hashtable hashtable = new Hashtable();
        static ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        #endregion

        #region .ctor
        static TypeDescriptorHelper()
        {
            //The Kooboo.dll can not be loaded in Medium trust level
            //http://kooboo.codeplex.com/workitem/12954
            Discover(AppDomain.CurrentDomain.GetAssemblies()
                .Where(it => !it.IsDynamic)
                .Where(it => !it.FullName.StartsWith("System.") && !it.FullName.Contains("Kooboo,"))
                .SelectMany(it =>
                {
                    try { return it.GetTypes(); }
                    catch { return Enumerable.Empty<Type>(); }
                }), true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Discovers the specified types.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
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
        /// <summary>
        /// Registers the type of the metadata.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="metadataType">Type of the metadata.</param>
        public static void RegisterMetadataType(Type type, Type metadataType)
        {
            locker.EnterWriteLock();

            hashtable[type] = metadataType;

            locker.ExitWriteLock();
        }
        /// <summary>
        /// Gets the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ICustomTypeDescriptor Get(Type type)
        {
            try
            {
                locker.EnterReadLock();
                var metadataType = hashtable[type] as Type;
                ICustomTypeDescriptor descriptor = null;
                if (metadataType != null)
                {
                    var instance = TypeActivator.CreateInstance(metadataType);
                    if (instance != null)
                    {
                        // Order to get the metadata type instance from DI.
                        metadataType = instance.GetType();
                        descriptor = new AssociatedMetadataTypeTypeDescriptionProvider(type, metadataType).GetTypeDescriptor(type);
                    }
                }

                return descriptor;
            }
            finally
            {
                locker.ExitReadLock();
            }


        }
        #endregion
    }
}
