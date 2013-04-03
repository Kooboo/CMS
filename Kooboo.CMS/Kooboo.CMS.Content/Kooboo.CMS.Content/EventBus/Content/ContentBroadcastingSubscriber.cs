using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.Extensions;
using System.Threading;
namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentBroadcastingSubscriber : ISubscriber
    {
        static ContentBroadcastingSubscriber()
        {
        }
        #region ISubscriber Members

        public virtual void Receive(IEventContext context)
        {
            try
            {
                if (context is ContentEventContext)
                {
                    var contentEventContext = (ContentEventContext)context;
                    Thread processThread = new Thread(delegate()
                    {
                        var contentContext = (ContentEventContext)context;

                        try
                        {
                            var sendingRepository = contentContext.Content.GetRepository().AsActual();
                            var sendingSetting = AllowSending(contentContext.Content, sendingRepository, contentContext);
                            if (sendingSetting != null)
                            {
                                var allRepositories = Services.ServiceFactory.RepositoryManager.All().Where(it => string.Compare(it.Name, sendingRepository.Name, true) != 0);

                                var summarize = contentContext.Content.GetSummary();
                                foreach (var receiver in allRepositories)
                                {
                                    var repository = receiver.AsActual();
                                    if (repository.EnableBroadcasting)
                                    {
                                        Services.ServiceFactory.ReceiveSettingManager.ReceiveContent(repository, contentContext.Content, contentContext.ContentAction);
                                    }
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
            catch (Exception e)
            {
                Kooboo.HealthMonitoring.Log.LogException(e);
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
