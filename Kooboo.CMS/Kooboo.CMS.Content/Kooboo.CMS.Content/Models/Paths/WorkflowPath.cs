using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Kooboo.Web.Url;

using Kooboo.CMS.Content.Query;

namespace Kooboo.CMS.Content.Models.Paths
{
    public class WorkflowPath : IPath
    {
        const string PATH_NAME = "Workflows";

        private string GetFileName(Workflow workflow)
        {
            return workflow.Name + ".config";
        }

        public WorkflowPath(Repository repository)
        {
            var repositoryPath = new RepositoryPath(repository);

            PhysicalPath = Path.Combine(repositoryPath.PhysicalPath, PATH_NAME);

            VirtualPath = UrlUtility.Combine(repositoryPath.VirtualPath, PATH_NAME);

            IO.IOUtility.EnsureDirectoryExists(PhysicalPath);
        }

        public WorkflowPath(Workflow workflow)
            : this(workflow.Repository)
        {
            this.PhysicalPath = SettingFile = Path.Combine(PhysicalPath, GetFileName(workflow));
            this.VirtualPath = UrlUtility.Combine(this.VirtualPath, GetFileName(workflow));
        }

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public string SettingFile
        {
            get;
            private set;
        }

        public bool Exists()
        {
            return File.Exists(SettingFile);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }
    }

    public class PendingWorkflowItemPath : IPath
    {
        const string PATH_NAME = "PendingWorkflowItems";

        private string GetFileName(PendingWorkflowItem pendingWorkflowItem)
        {
            return pendingWorkflowItem.Name + ".config";
        }

        public PendingWorkflowItemPath(Repository repository, string roleName)
        {
            var repositoryPath = new RepositoryPath(repository);

            PhysicalPath = Path.Combine(repositoryPath.PhysicalPath, PATH_NAME, roleName);

            VirtualPath = UrlUtility.Combine(repositoryPath.VirtualPath, PATH_NAME, roleName);

            IO.IOUtility.EnsureDirectoryExists(PhysicalPath);
        }

        public PendingWorkflowItemPath(PendingWorkflowItem pendingWorkflowItem)
        {
            var repositoryPath = new RepositoryPath(pendingWorkflowItem.Repository);

            this.PhysicalPath = SettingFile = Path.Combine(repositoryPath.PhysicalPath, PATH_NAME, pendingWorkflowItem.RoleName, GetFileName(pendingWorkflowItem));

            this.VirtualPath = UrlUtility.Combine(repositoryPath.VirtualPath, PATH_NAME, pendingWorkflowItem.RoleName, GetFileName(pendingWorkflowItem));
        }

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public string SettingFile
        {
            get;
            private set;
        }

        public bool Exists()
        {
            return File.Exists(SettingFile);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }
    }

    public class WorkflowHistoryPath : IPath
    {
        const string PATH_NAME = "~WorkflowHistory";

        TextContentPath ContentPath;



        public WorkflowHistoryPath(TextContent content)
        {
            ContentPath = new TextContentPath(content);
            PhysicalPath = Path.Combine(ContentPath.PhysicalPath, PATH_NAME);
            VirtualPath = UrlUtility.Combine(ContentPath.VirtualPath, PATH_NAME);
        }

        public WorkflowHistoryPath(WorkflowHistory history)
        {
            var folder = new TextFolder(history.Repository, history.ContentFolder);

            var content = folder.CreateQuery().WhereEquals("UUID", history.ContentUUID).FirstOrDefault();

            ContentPath = new TextContentPath(content);
            PhysicalPath = SettingFile = Path.Combine(ContentPath.PhysicalPath, PATH_NAME, GetFile(history));
            VirtualPath = UrlUtility.Combine(ContentPath.VirtualPath, PATH_NAME, GetFile(history));

        }

        private string GetFile(WorkflowHistory history)
        {
            return history.Id + ".config";
        }

        public string PhysicalPath
        {
            get;
            private set;
        }

        public string VirtualPath
        {
            get;
            private set;
        }

        public string SettingFile
        {
            get;
            private set;
        }

        public bool Exists()
        {

            return !string.IsNullOrEmpty(SettingFile) && File.Exists(this.SettingFile);
        }

        public void Rename(string newName)
        {
            throw new NotImplementedException();
        }
    }
}
