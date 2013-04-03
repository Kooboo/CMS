
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
namespace Kooboo.CMS.Sites.ContentEvent
{
    public class ContentBroadcastingSubscriber : ISubscriber
    {
        static ContentBroadcastingSubscriber()
        {
        }
        #region ISubscriber Members

        public virtual void Receive(IEventContext context)
        {
            if (context is ContentEventContext)
            {
                var contentEventContext = (ContentEventContext)context;
                var site = Site.Current;
                Thread processThread = new Thread(delegate()
                {
                    var contentContext = (ContentEventContext)context;

                    try
                    {
                        var sendingRepository = contentContext.Content.GetRepository().AsActual();
                        var sendingSetting = AllowSending(contentContext.Content, sendingRepository, contentContext);
                        if (sendingSetting != null)
                        {
                            var allRepositories = ServiceFactory.RepositoryManager.All().Where(it => string.Compare(it.Name, sendingRepository.Name, true) != 0);

                            var summarize = contentContext.Content.GetSummary();
                            foreach (var receiver in allRepositories)
                            {
                                var repository = receiver.AsActual();
                                if (repository.EnableBroadcasting)
                                {
                                    ServiceFactory.ReceiveSettingManager.ReceiveContent(repository, contentContext.Content, contentContext.ContentAction);
                                }
                            }

                            if (site != null && (sendingSetting.SendToAllChildSites.HasValue && sendingSetting.SendToAllChildSites.Value == true))
                            {
                                SendToChildSites(site, contentEventContext, sendingSetting.KeepStatus);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Kooboo.HealthMonitoring.Log.LogException(e);
                    }
                });

                processThread.Start();
            }
        }

        protected virtual void SendToChildSites(Site site, ContentEventContext eventContext, bool keepStatus)
        {
            List<Repository> repositoryList = new List<Repository>();

            GetAllRepositories(Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(site), repositoryList);

            foreach (var repository in repositoryList)
            {
                ServiceFactory.ReceiveSettingManager.ProcessMessage(repository, eventContext.Content, eventContext.Content.FolderName, keepStatus, eventContext.ContentAction);
            }
        }
        private void GetAllRepositories(IEnumerable<Site> sites, List<Repository> repositoryList)
        {
            foreach (var site in sites)
            {
                var repository = site.GetRepository();
                if (!repositoryList.Contains(repository))
                {
                    repositoryList.Add(repository);
                }
                GetAllRepositories(Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.ChildSites(site), repositoryList);
            }
        }

        protected virtual SendingSetting AllowSending(TextContent content, Repository repository, ContentEventContext eventContext)
        {
            if (!repository.EnableBroadcasting)
            {
                return null;
            }
            var list = Providers.DefaultProviderFactory.GetProvider<ISendingSettingProvider>().All(repository).Select(o => Providers.DefaultProviderFactory.GetProvider<ISendingSettingProvider>().Get(o));
            foreach (var item in list)
            {
                if (AllowSendingSetting(content, item, eventContext))
                {
                    return item;
                }
            }
            return null;
        }
        protected virtual bool AllowSendingSetting(TextContent content, SendingSetting sendingSetting, ContentEventContext eventContext)
        {
            if (!string.IsNullOrEmpty(sendingSetting.FolderName) && !sendingSetting.FolderName.Equals(content.FolderName, StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }
            if (!string.IsNullOrEmpty(content.OriginalUUID))
            {
                if (content.IsLocalized == null || content.IsLocalized.Value == false)
                {
                    if ((sendingSetting.SendReceived == null || sendingSetting.SendReceived.Value == false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion
    }
}
