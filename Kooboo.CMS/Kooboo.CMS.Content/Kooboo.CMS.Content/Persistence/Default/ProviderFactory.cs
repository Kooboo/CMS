using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using System.Collections;

namespace Kooboo.CMS.Content.Persistence.Default
{
    public class ProviderFactory : IProviderFactory
    {
        Hashtable providers = new Hashtable();
        public ProviderFactory()
        {
            providers.Add(typeof(IRepositoryProvider), new RepositoryProvider());
            providers.Add(typeof(ISchemaProvider), new SchemaProvider());
            providers.Add(typeof(ITextFolderProvider), new TextFolderProvider());
            providers.Add(typeof(IMediaFolderProvider), new MediaFolderProvider());
            //providers.Add(typeof(IReceivedMessageProvider), new ReceivedMessageProvider());
            providers.Add(typeof(IReceivingSettingProvider), new ReceivingSettingProvider());
            providers.Add(typeof(ISendingSettingProvider), new SendingSettingProvider());
            providers.Add(typeof(ITextContentProvider), new TextContentProvider());
            providers.Add(typeof(IMediaContentProvider), new MediaContentProvider());
            providers.Add(typeof(IWorkflowProvider), new WorkflowProvider());

            providers.Add(typeof(IWorkflowHistoryProvider), new WorkflowHistoryProvider());

            providers.Add(typeof(IPendingWorkflowItemProvider), new PendingWorkflowItemProvider());

            providers[typeof(ITextContentFileProvider)] = new TextContentFileProvider();
        }

        #region IProviderFactory Members

        public virtual string Name
        {
            get { return "Xml file"; }
        }

        public virtual T GetProvider<T>()
        {
            foreach (var item in providers.Values)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }
            return default(T);
        }

        #endregion


        public virtual void RegisterProvider<T>(T provider)
        {
            providers[typeof(T)] = provider;
        }
    }
}
