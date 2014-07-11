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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;

namespace Kooboo.CMS.Content.Services
{
    public class RepositorySendingFolders
    {
        public Repository Repository { get; set; }
        public IEnumerable<FolderTreeNode<TextFolder>> TextFolders { get; set; }
    }
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(SendingSettingManager))]
    public class SendingSettingManager : ManagerBase<SendingSetting, ISendingSettingProvider>
    {
        public SendingSettingManager(ISendingSettingProvider provider)
            : base(provider)
        {
        }
        public override SendingSetting Get(Repository repository, string name)
        {
            return Provider.Get(new SendingSetting { Repository = repository, Name = name });
        }
        public virtual IEnumerable<RepositorySendingFolders> GetAllSendingFolders()
        {
            List<RepositorySendingFolders> repositorySendingFolders = new List<RepositorySendingFolders>();

            IEnumerable<Repository> repositories = ServiceFactory.RepositoryManager.All();
            foreach (var repository in repositories)
            {
                var treeNodes = GetTreeNodes(repository, Kooboo.CMS.Content.Services.ServiceFactory.SendingSettingManager.All(repository, ""));
                if (treeNodes.Count() > 0)
                {
                    repositorySendingFolders.Add(new RepositorySendingFolders()
                    {
                        Repository = repository,
                        TextFolders = treeNodes
                    });
                }
            }

            return repositorySendingFolders;
        }
        protected IEnumerable<FolderTreeNode<TextFolder>> GetTreeNodes(Repository repository, IEnumerable<SendingSetting> sendingSettings)
        {
            List<FolderTreeNode<TextFolder>> list = new List<FolderTreeNode<TextFolder>>();

            var allFolders = sendingSettings.Select(it => it.FolderName).OrderBy(it => FolderHelper.SplitFullName(it).Count());

            foreach (var folderName in allFolders)
            {
                list.Add(new FolderTreeNode<TextFolder>()
                {
                    Folder = new TextFolder(repository, folderName)
                });
            }
            return list;
        }
    }
}
