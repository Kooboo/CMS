using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.CMS.Content.Services
{
    public static class ServiceFactory
    {
        static Hashtable services = new Hashtable();
        static ServiceFactory()
        {
            services.Add(typeof(RepositoryManager), new RepositoryManager());
            services.Add(typeof(SchemaManager), new SchemaManager());
            services.Add(typeof(TextFolderManager), new TextFolderManager());
            services.Add(typeof(TextContentManager), new TextContentManager());
            services.Add(typeof(MediaContentManager), new MediaContentManager());
            services.Add(typeof(MediaFolderManager), new MediaFolderManager());
            //services.Add(typeof(ReceivedMessageManager), new ReceivedMessageManager());
            services.Add(typeof(ReceivingSettingManager), new ReceivingSettingManager());

            services.Add(typeof(SendingSettingManager), new SendingSettingManager());

            services.Add(typeof(RepositoryTemplateManager), new RepositoryTemplateManager());
            services.Add(typeof(SchemaTemplateManager), new SchemaTemplateManager());

            services.Add(typeof(WorkflowManager), new WorkflowManager());
        }
        public static RepositoryManager RepositoryManager
        {
            get
            {
                return (RepositoryManager)services[typeof(RepositoryManager)];
            }
            set
            {
                services[typeof(RepositoryManager)] = value;
            }
        }
        public static SchemaManager SchemaManager
        {
            get
            {
                return (SchemaManager)services[typeof(SchemaManager)];
            }
            set
            {
                services[typeof(SchemaManager)] = value;
            }
        }

        public static TextFolderManager TextFolderManager
        {
            get
            {
                return (TextFolderManager)services[typeof(TextFolderManager)];
            }
            set
            {
                services[typeof(TextFolderManager)] = value;
            }
        }
        public static TextContentManager TextContentManager
        {
            get
            {
                return (TextContentManager)services[typeof(TextContentManager)];
            }
            set
            {
                services[typeof(TextContentManager)] = value;
            }
        }
        public static MediaContentManager MediaContentManager
        {
            get
            {
                return (MediaContentManager)services[typeof(MediaContentManager)];
            }
            set
            {
                services[typeof(MediaContentManager)] = value;
            }
        }
        public static MediaFolderManager MediaFolderManager
        {
            get
            {
                return (MediaFolderManager)services[typeof(MediaFolderManager)];
            }
            set
            {
                services[typeof(MediaFolderManager)] = value;
            }
        }       
        public static ReceivingSettingManager ReceiveSettingManager
        {
            get
            {
                return (ReceivingSettingManager)services[typeof(ReceivingSettingManager)];
            }
            set
            {
                services[typeof(ReceivingSettingManager)] = value;
            }
        }
        
        public static SendingSettingManager SendingSettingManager
        {
            get
            {
                return (SendingSettingManager)services[typeof(SendingSettingManager)];
            }
            set
            {
                services[typeof(SendingSettingManager)] = value;
            }
        }
        public static RepositoryTemplateManager RepositoryTemplateManager
        {
            get
            {
                return (RepositoryTemplateManager)services[typeof(RepositoryTemplateManager)];
            }
            set
            {
                services[typeof(RepositoryTemplateManager)] = value;
            }
        }
        public static SchemaTemplateManager SchemaTemplateManager
        {
            get
            {
                return (SchemaTemplateManager)services[typeof(SchemaTemplateManager)];
            }
            set
            {
                services[typeof(RepositoryTemplateManager)] = value;
            }
        }
        public static WorkflowManager WorkflowManager
        {
            get
            {
                return (WorkflowManager)services[typeof(WorkflowManager)];
            }
            set
            {
                services[typeof(WorkflowManager)] = value;
            }
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
