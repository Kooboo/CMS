using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using System.Security;
using System.ComponentModel.Composition.Primitives;
using System.IO;

namespace Kooboo
{
    [Obsolete]
    [SecurityCritical]
    public class ComponentContainer
    {
        //private volatile static CompositionContainer container;
        //private static object lockHelper = new object();
        ///// <summary>
        ///// Gets the container.
        ///// </summary>
        ///// <value>The container.</value>
        //public static CompositionContainer Container
        //{
        //    get
        //    {
        //        if (container == null)
        //        {
        //            lock (lockHelper)
        //            {
        //                if (container == null)
        //                {
        //                    AggregateCatalog aggregateCatelog = new AggregateCatalog();
        //                    //从BIN目录找所有kooboo程序集,以避免去加载一些第三方程序集，因为Kooboo程序里面可能会有一些默认实现。
        //                    //如果不是默认实现，请将组件放置在Components目录
        //                    aggregateCatelog.Catalogs.Add(new DirectoryCatalog(Settings.BinDirectory, "*kooboo*.dll"));

        //                    if (Directory.Exists(Settings.ComponentsDirectory))
        //                    {
        //                        aggregateCatelog.Catalogs.Add(new DirectoryCatalog(Settings.ComponentsDirectory));
        //                    }

        //                    container = new CompositionContainer(aggregateCatelog);
        //                }
        //            }
        //        }
        //        return container;
        //    }
        //}
        ///// <summary>
        ///// Resolves this instance.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static T Resolve<T>()
        //{
        //    return ComponentContainer.Container.GetExportedValue<T>();
        //}

        ///// <summary>
        ///// Resolves the specified contract name.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="contractName">Name of the contract.</param>
        ///// <returns></returns>
        //public static T Resolve<T>(string contractName)
        //{
        //    return ComponentContainer.Container.GetExportedValue<T>(contractName);
        //}

        ///// <summary>
        ///// Tries the resolve.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static T TryResolve<T>()
        //{
        //    return TryResolve<T>(string.Empty);
        //}
        ///// <summary>
        ///// Try to resolve the export.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="contractName">Name of the contract.</param>
        ///// <returns></returns>
        //public static T TryResolve<T>(string contractName)
        //{
        //    IEnumerable<Export> exports;
        //    bool r = TryGetAllExports<T>(contractName, out exports);
        //    if (r)
        //    {
        //        return (T)exports.First().Value;
        //    }
        //    return default(T);
        //}

        //private static bool TryGetAllExports<T>(string contractName, out IEnumerable<Export> exports)
        //{
        //    var importDefinition = new ImportDefinition(ed => ed.ContractName == contractName && ed.Metadata["ExportTypeIdentity"].ToString() == typeof(T).FullName, contractName, ImportCardinality.ExactlyOne, true, true);

        //    return ComponentContainer.Container.TryGetExports(importDefinition, new AtomicComposition() { }, out exports);
        //}


        ///// <summary>
        ///// Resolves all.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <returns></returns>
        //public static IEnumerable<T> ResolveAll<T>()
        //{
        //    return Container.GetExportedValues<T>();
        //}

    }
}
