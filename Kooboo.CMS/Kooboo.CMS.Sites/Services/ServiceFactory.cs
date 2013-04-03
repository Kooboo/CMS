using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Services
{
    public class ContentWorkflowManager : Kooboo.CMS.Content.Services.WorkflowManager
    {
        protected override bool IsAdministrator(string userName)
        {
            return ServiceFactory.UserManager.IsAdministrator(userName);
        }
        protected override string[] GetRoles(string userName)
        {
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
        public override void Add(Content.Models.Repository repository, Content.Models.SendingSetting item)
        {
            base.Add(repository, item);
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


    public class ServiceFactory
    {
        static Hashtable services = new Hashtable();
        static ServiceFactory()
        {
            services.Add(typeof(LayoutManager), new LayoutManager());
            services.Add(typeof(ViewManager), new ViewManager());
            services.Add(typeof(PageManager), new PageManager());
            services.Add(typeof(SiteManager), new SiteManager());
            services.Add(typeof(ThemeManager), new ThemeManager());
            services.Add(typeof(CustomFileManager), new CustomFileManager());
            services.Add(typeof(ScriptManager), new ScriptManager());
            services.Add(typeof(LabelManager), new LabelManager());
            services.Add(typeof(CustomErrorManager), new CustomErrorManager());
            services.Add(typeof(UrlRedirectManager), new UrlRedirectManager());
            services.Add(typeof(UrlKeyMapManager), new UrlKeyMapManager());
            services.Add(typeof(AssemblyManager), new AssemblyManager());
            services.Add(typeof(ModuleManager), new ModuleManager());
            services.Add(typeof(UserManager), new UserManager());
            services.Add(typeof(SiteTemplateManager), new SiteTemplateManager());
            services.Add(typeof(LayoutItemTemplateManager), new LayoutItemTemplateManager());

            services.Add(typeof(CodeSnippetManager), new CodeSnippetManager());
            services.Add(typeof(ImportedSiteManager), new ImportedSiteManager());

            services.Add(typeof(HtmlBlockManager), new HtmlBlockManager());

            services.Add(typeof(SystemManager), new SystemManager());
            services.Add(typeof(ThemeFileManager), new ThemeFileManager());

            Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager = new ContentWorkflowManager();
            HeaderBackgroundManager = new HeaderBackgroundManager();

            Kooboo.CMS.Content.Services.ServiceFactory.SendingSettingManager = new SendingSettingManager();
        }
        public static LayoutManager LayoutManager
        {
            get
            {
                return (LayoutManager)services[typeof(LayoutManager)];
            }
            set
            {
                services[typeof(LayoutManager)] = value;
            }
        }
        public static ViewManager ViewManager
        {
            get
            {
                return (ViewManager)services[typeof(ViewManager)];
            }
            set
            {
                services[typeof(ViewManager)] = value;
            }
        }
        public static PageManager PageManager
        {
            get
            {
                return (PageManager)services[typeof(PageManager)];
            }
            set
            {
                services[typeof(PageManager)] = value;
            }
        }
        public static SiteManager SiteManager
        {
            get
            {
                return (SiteManager)services[typeof(SiteManager)];
            }
            set
            {
                services[typeof(SiteManager)] = value;
            }
        }
        public static ThemeManager ThemeManager
        {
            get
            {
                return (ThemeManager)services[typeof(ThemeManager)];
            }
            set
            {
                services[typeof(ThemeManager)] = value;
            }
        }
        public static LabelManager LabelManager
        {
            get
            {
                return (LabelManager)services[typeof(LabelManager)];
            }
            set
            {
                services[typeof(LabelManager)] = value;
            }
        }
        public static CustomErrorManager CustomErrorManager
        {
            get
            {
                return (CustomErrorManager)services[typeof(CustomErrorManager)];
            }
            set
            {
                services[typeof(CustomErrorManager)] = value;
            }
        }
        public static UrlRedirectManager UrlRedirectManager
        {
            get
            {
                return (UrlRedirectManager)services[typeof(UrlRedirectManager)];
            }
            set
            {
                services[typeof(UrlRedirectManager)] = value;
            }
        }
        public static UrlKeyMapManager UrlKeyMapManager
        {
            get
            {
                return (UrlKeyMapManager)services[typeof(UrlKeyMapManager)];
            }
            set
            {
                services[typeof(UrlKeyMapManager)] = value;
            }
        }
        public static AssemblyManager AssemblyManager
        {
            get
            {
                return (AssemblyManager)services[typeof(AssemblyManager)];
            }
            set
            {
                services[typeof(AssemblyManager)] = value;
            }
        }
        public static ModuleManager ModuleManager
        {
            get
            {
                return (ModuleManager)services[typeof(ModuleManager)];
            }
            set
            {
                services[typeof(ModuleManager)] = value;
            }
        }
        public static UserManager UserManager
        {
            get
            {
                return (UserManager)services[typeof(UserManager)];
            }
            set
            {
                services[typeof(UserManager)] = value;
            }
        }

        public static FileManager FileManager
        {
            get
            {
                return (FileManager)services[typeof(FileManager)];
            }
            set
            {
                services[typeof(FileManager)] = value;
            }
        }

        public static SiteTemplateManager SiteTemplateManager
        {
            get
            {
                return (SiteTemplateManager)services[typeof(SiteTemplateManager)];
            }
            set
            {
                services[typeof(SiteTemplateManager)] = value;
            }
        }
        public static LayoutItemTemplateManager LayoutItemTemplateManager
        {
            get
            {
                return (LayoutItemTemplateManager)services[typeof(LayoutItemTemplateManager)];
            }
            set
            {
                services[typeof(LayoutItemTemplateManager)] = value;
            }
        }

        public static CodeSnippetManager CodeSnippetManager
        {
            get
            {
                return (CodeSnippetManager)services[typeof(CodeSnippetManager)];
            }
            set
            {
                services[typeof(CodeSnippetManager)] = value;
            }
        }

        public static ImportedSiteManager ImportedSiteManager
        {
            get
            {
                return (ImportedSiteManager)services[typeof(ImportedSiteManager)];
            }
            set
            {
                services[typeof(ImportedSiteManager)] = value;
            }
        }

        public static HtmlBlockManager HtmlBlockManager
        {
            get
            {
                return (HtmlBlockManager)services[typeof(HtmlBlockManager)];
            }
            set
            {
                services[typeof(HtmlBlockManager)] = value;
            }
        }

        public static SystemManager SystemManager
        {
            get
            {
                return (SystemManager)services[typeof(SystemManager)];
            }
            set
            {
                services[typeof(SystemManager)] = value;
            }
        }

        public static HeaderBackgroundManager HeaderBackgroundManager
        {
            get
            {
                return (HeaderBackgroundManager)services[typeof(HeaderBackgroundManager)];
            }
            set
            {
                services[typeof(HeaderBackgroundManager)] = value;
            }
        }

        public static ScriptManager ScriptManager {
            get { return (ScriptManager)services[typeof(ScriptManager)]; }
            set { services[typeof(ScriptManager)] = value; }
        }

        public static ThemeFileManager ThemeFileManager {
            get { return (ThemeFileManager)services[typeof(ThemeFileManager)]; }
            set { services[typeof(ThemeFileManager)] = value; }
        }

        public static T GetService<T>()
        {
            foreach (var service in services.Values)
            {
                if (service is T)
                {
                    return (T)service;
                }
            }
            return default(T);
        }
    }
}
