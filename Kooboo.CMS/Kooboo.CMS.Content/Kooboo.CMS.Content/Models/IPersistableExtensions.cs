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
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 可持久化对象的扩展方法
    /// </summary>
    public static class IPersistableExtensions
    {
        /// <summary>
        /// 用这个方法取得完整的对象
        /// 相当于Provider.Get(id)
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <returns></returns>
        public static Repository AsActual(Repository repository)
        {
            if (repository.IsDummy)
            {
                repository = Providers.RepositoryProvider.Get(repository);
            }
            return repository;
        }
        /// <summary>
        /// 用这个方法取得完整的对象
        /// 相当于Provider.Get(id)
        /// </summary>
        /// <param name="schema">The schema.</param>
        /// <returns></returns>
        public static Schema AsActual(Schema schema)
        {
            if (schema.IsDummy)
            {
                schema = Providers.DefaultProviderFactory.GetProvider<ISchemaProvider>().Get(schema);
            }
            return schema;
        }
        /// <summary>
        /// 用这个方法取得完整的对象
        /// 相当于Provider.Get(id)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        public static T AsActual<T>(T folder)
            where T : Folder
        {
            if (folder.IsDummy)
            {
                if (folder is MediaFolder)
                {
                    folder.IsDummy = false;
                    return folder;
                }
                else
                {
                    folder = Providers.DefaultProviderFactory.GetProvider<ITextFolderProvider>().Get((TextFolder)(object)folder) as T;
                }

            }
            return folder;
        }

        /// <summary>
        /// 用这个方法取得完整的对象
        /// 相当于Provider.Get(id)
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <returns></returns>
        public static Workflow AsActual(Workflow workflow)
        {
            return Providers.DefaultProviderFactory.GetProvider<IWorkflowProvider>().Get(workflow);
        }
    }
}
