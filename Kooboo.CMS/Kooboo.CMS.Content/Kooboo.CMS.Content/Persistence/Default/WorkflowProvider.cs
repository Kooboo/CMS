#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Models.Paths;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content.Persistence.Default
{
    #region WorkflowProvider
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IWorkflowProvider))]
    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Workflow>))]
    public class WorkflowProvider : FileSystemProviderBase<Workflow>, IWorkflowProvider
    {
        #region IProvider Members CRUD

        public IEnumerable<Models.Workflow> All(Models.Repository repository)
        {
            var path = new WorkflowPath(repository);
            return IO.IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(it => new Workflow
                     {
                         Repository = repository,
                         Name = Path.GetFileNameWithoutExtension(it.Name)
                     }).ToArray().AsQueryable();
        }
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion
    }

    #endregion

    #region PendingWorkflowItemProvider
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IPendingWorkflowItemProvider))]
    public class PendingWorkflowItemProvider : FileSystemProviderBase<PendingWorkflowItem>, IPendingWorkflowItemProvider
    {
        #region All
        public IEnumerable<PendingWorkflowItem> All(Repository repository, string roleName)
        {
            var path = new PendingWorkflowItemPath(repository, roleName);
            return IO.IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(o => Get(new PendingWorkflowItem
                     {
                         Name = Path.GetFileNameWithoutExtension(o.Name),
                         Repository = repository,
                         RoleName = roleName,
                     })).ToArray();
        }
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion
    }
    #endregion

    #region WorkflowHistoryProvider
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IWorkflowHistoryProvider))]
    public class WorkflowHistoryProvider : FileSystemProviderBase<WorkflowHistory>, IWorkflowHistoryProvider
    {
        #region GetPath
        private string GetPath(TextContent content)
        {
            return new WorkflowHistoryPath(content).PhysicalPath;
        }
        #endregion

        #region All
        public IEnumerable<WorkflowHistory> All(TextContent content)
        {
            var path = GetPath(content);
            Kooboo.IO.IOUtility.EnsureDirectoryExists(path);
            return Kooboo.IO.IOUtility
                .EnumerateFilesExludeHidden(path)
                .Select(o =>
                {
                    var id = Int32.Parse(Path.GetFileNameWithoutExtension(o.Name));
                    var r = Get(new WorkflowHistory
                    {
                        Id = id,
                        ContentUUID = content.UUID,
                        Name = Path.GetFileNameWithoutExtension(o.Name),
                        Repository = new Repository(content.Repository),
                        ContentFolder = content.FolderName
                    });
                    return r;
                }).ToArray();
        }
        #endregion

        #region GetLocker
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        #endregion
    } 
    #endregion


}
