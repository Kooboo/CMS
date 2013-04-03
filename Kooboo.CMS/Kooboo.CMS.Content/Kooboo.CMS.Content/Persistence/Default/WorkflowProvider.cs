using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models.Paths;
using Kooboo.CMS.Content.Models;
using System.IO;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public class WorkflowProvider : FileSystemProviderBase<Workflow>, IWorkflowProvider
    {

        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        #region IProvider Members CRUD

        public IQueryable<Models.Workflow> All(Models.Repository repository)
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
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }

    public class PendingWorkflowItemProvider : FileSystemProviderBase<PendingWorkflowItem>, IPendingWorkflowItemProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        public IQueryable<PendingWorkflowItem> All(Repository repository, string roleName)
        {
            var path = new PendingWorkflowItemPath(repository, roleName);
            return IO.IOUtility
                     .EnumerateFilesExludeHidden(path.PhysicalPath)
                     .Select(o => Get(new PendingWorkflowItem
                     {
                         Name = Path.GetFileNameWithoutExtension(o.Name),
                         Repository = repository,
                         RoleName = roleName,
                     })).ToArray().AsQueryable();
        }

        public IQueryable<PendingWorkflowItem> All(Repository repository)
        {
            throw new NotImplementedException("Please use All(Repository repository, string roleName).");
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }

    public class WorkflowHistoryProvider : FileSystemProviderBase<WorkflowHistory>, IWorkflowHistoryProvider
    {
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();

        private string GetPath(TextContent content)
        {
            return new WorkflowHistoryPath(content).PhysicalPath;
        }

        public IQueryable<WorkflowHistory> All(TextContent content)
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
                }).ToArray().AsQueryable();
        }

        public IQueryable<WorkflowHistory> All(Repository repository)
        {
            throw new NotImplementedException();
        }

        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
    }


}
