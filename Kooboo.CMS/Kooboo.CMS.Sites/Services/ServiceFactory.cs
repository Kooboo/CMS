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
using System.Collections;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Content.Persistence;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Sites.Services
{
    public class ContentWorkflowManager : Kooboo.CMS.Content.Services.WorkflowManager
    {
        public ContentWorkflowManager(IWorkflowProvider workflowProvider,
            IPendingWorkflowItemProvider pendingWorkflowItemProvider,
            IWorkflowHistoryProvider workflowHistoryProvider)
            : base(workflowProvider, pendingWorkflowItemProvider, workflowHistoryProvider)
        {
        }
        protected override bool IsAdministrator(string userName)
        {
            return ServiceFactory.UserManager.IsAdministrator(userName);
        }
        protected override string[] GetRoles(string userName)
        {
            if (Site.Current == null)
            {
                return new string[0];
            }
            var user = ServiceFactory.UserManager.Get(Models.Site.Current, userName);
            if (user == null || user.Roles == null)
            {
                return new string[0];
            }
            else
            {
                return user.Roles.ToArray();
            }
        }
    }
    public class SendingSettingManager : Kooboo.CMS.Content.Services.SendingSettingManager
    {
        public SendingSettingManager(ISendingSettingProvider provider)
            : base(provider)
        {
        }
        public override void Add(Content.Models.Repository repository, Content.Models.SendingSetting item)
        {
            base.Add(repository, item);
            AddReceivingSettingToSubSites(repository, item);
        }

        private void AddReceivingSettingToSubSites(Content.Models.Repository repository, Content.Models.SendingSetting item)
        {
            if (Site.Current != null && item.SendToChildSites.HasValue && item.SendToChildSites.Value == true)
            {
                var repositoryList = GetAllRepositoriesForChildSites(Site.Current, item.ChildLevel);

                foreach (var repo in repositoryList)
                {
                    try
                    {
                        if (repo != repository)
                        {
                            ReceivingSetting rece = new ReceivingSetting()
                            {
                                KeepStatus = item.KeepStatus,
                                ReceivingFolder = item.FolderName,
                                Repository = repo,
                                SendingFolder = item.FolderName,
                                SendingRepository = repository.Name
                            };

                            Kooboo.CMS.Content.Services.ServiceFactory.ReceiveSettingManager.Add(repo, rece);
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
        public override void Update(Repository repository, SendingSetting @new, SendingSetting old)
        {
            base.Update(repository, @new, old);
            AddReceivingSettingToSubSites(repository, @new);
        }
        protected virtual IEnumerable<Repository> GetAllRepositoriesForChildSites(Site site, ChildLevel level)
        {
            List<Repository> repositoryList = new List<Repository>();

            GetAllRepositories(Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(site), repositoryList, level);

            return repositoryList;
        }
        private void GetAllRepositories(IEnumerable<Site> sites, List<Repository> repositoryList, ChildLevel level)
        {
            foreach (var site in sites)
            {
                var repository = site.GetRepository();
                if (!repositoryList.Contains(repository))
                {
                    repositoryList.Add(repository);
                }
                if (level == ChildLevel.All)
                {
                    GetAllRepositories(Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(site), repositoryList, level);
                }
            }
        }
    }

    public class ServiceFactory : Kooboo.Common.ObjectContainer.Dependency.IDependencyRegistrar
    {

        public static LayoutManager LayoutManager
        {
            get
            {
                return EngineContext.Current.Resolve<LayoutManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<LayoutManager>(value);
            }
        }
        public static ViewManager ViewManager
        {
            get
            {
                return EngineContext.Current.Resolve<ViewManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ViewManager>(value);
            }
        }
        public static PageManager PageManager
        {
            get
            {
                return EngineContext.Current.Resolve<PageManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<PageManager>(value);
            }
        }
        public static SiteManager SiteManager
        {
            get
            {
                return EngineContext.Current.Resolve<SiteManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<SiteManager>(value);
            }
        }
        public static ScriptManager ScriptManager
        {
            get
            {
                return EngineContext.Current.Resolve<ScriptManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ScriptManager>(value);
            }
        }
        public static ThemeManager ThemeManager
        {
            get
            {
                return EngineContext.Current.Resolve<ThemeManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ThemeManager>(value);
            }
        }
        public static LabelManager LabelManager
        {
            get
            {
                return EngineContext.Current.Resolve<LabelManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<LabelManager>(value);
            }
        }
        public static CustomErrorManager CustomErrorManager
        {
            get
            {
                return EngineContext.Current.Resolve<CustomErrorManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<CustomErrorManager>(value);
            }
        }
        public static UrlRedirectManager UrlRedirectManager
        {
            get
            {
                return EngineContext.Current.Resolve<UrlRedirectManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<UrlRedirectManager>(value);
            }
        }
        public static UrlKeyMapManager UrlKeyMapManager
        {
            get
            {
                return EngineContext.Current.Resolve<UrlKeyMapManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<UrlKeyMapManager>(value);
            }
        }
        public static AssemblyManager AssemblyManager
        {
            get
            {
                return EngineContext.Current.Resolve<AssemblyManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<AssemblyManager>(value);
            }
        }
        public static ModuleManager ModuleManager
        {
            get
            {
                return EngineContext.Current.Resolve<ModuleManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ModuleManager>(value);
            }
        }
        public static UserManager UserManager
        {
            get
            {
                return EngineContext.Current.Resolve<UserManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<UserManager>(value);
            }
        }

        public static FileManager FileManager
        {
            get
            {
                return EngineContext.Current.Resolve<FileManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<FileManager>(value);
            }
        }

        public static SiteTemplateManager SiteTemplateManager
        {
            get
            {
                return EngineContext.Current.Resolve<SiteTemplateManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<SiteTemplateManager>(value);
            }
        }
        public static LayoutItemTemplateManager LayoutItemTemplateManager
        {
            get
            {
                return EngineContext.Current.Resolve<LayoutItemTemplateManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<LayoutItemTemplateManager>(value);
            }
        }

        public static CodeSnippetManager CodeSnippetManager
        {
            get
            {
                return EngineContext.Current.Resolve<CodeSnippetManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<CodeSnippetManager>(value);
            }
        }

        public static ImportedSiteManager ImportedSiteManager
        {
            get
            {
                return EngineContext.Current.Resolve<ImportedSiteManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ImportedSiteManager>(value);
            }
        }

        public static HtmlBlockManager HtmlBlockManager
        {
            get
            {
                return EngineContext.Current.Resolve<HtmlBlockManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<HtmlBlockManager>(value);
            }
        }

        public static SystemManager SystemManager
        {
            get
            {
                return EngineContext.Current.Resolve<SystemManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<SystemManager>(value);
            }
        }

        public static HeaderBackgroundManager HeaderBackgroundManager
        {
            get
            {
                return EngineContext.Current.Resolve<HeaderBackgroundManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<HeaderBackgroundManager>(value);
            }
        }

        public static SubmissionSettingManager SubmissionSettingManager
        {
            get
            {
                return EngineContext.Current.Resolve<SubmissionSettingManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<SubmissionSettingManager>(value);
            }
        }

        public static ABPageSettingManager ABPageSettingManager
        {
            get
            {
                return EngineContext.Current.Resolve<ABPageSettingManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ABPageSettingManager>(value);
            }
        }

        public static ABRuleSettingManager ABRuleSettingManager
        {
            get
            {
                return EngineContext.Current.Resolve<ABRuleSettingManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ABRuleSettingManager>(value);
            }
        }

        public static ABSiteSettingManager ABSiteSettingManager
        {
            get
            {
                return EngineContext.Current.Resolve<ABSiteSettingManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ABSiteSettingManager>(value);
            }
        }

        public static ABPageTestResultManager ABPageTestResultManager
        {
            get
            {
                return EngineContext.Current.Resolve<ABPageTestResultManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ABPageTestResultManager>(value);
            }
        }
        public static T GetService<T>()
        where T : class
        {
            return EngineContext.Current.Resolve<T>();
        }

        public void Register(IContainerManager containerManager, ITypeFinder typeFinder)
        {
            containerManager.AddComponent<Kooboo.CMS.Content.Services.WorkflowManager, ContentWorkflowManager>();

            containerManager.AddComponent<Kooboo.CMS.Content.Services.SendingSettingManager, SendingSettingManager>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
