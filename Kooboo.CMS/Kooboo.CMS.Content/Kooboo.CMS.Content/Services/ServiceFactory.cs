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
using Kooboo.CMS.Common.Runtime;

namespace Kooboo.CMS.Content.Services
{
    public static class ServiceFactory
    {
        public static RepositoryManager RepositoryManager
        {
            get
            {
                return EngineContext.Current.Resolve<RepositoryManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<RepositoryManager>(value);
            }
        }
        public static SchemaManager SchemaManager
        {
            get
            {
                return EngineContext.Current.Resolve<SchemaManager>();
            }
            set
            {
                EngineContext.Current.ContainerManager.AddComponentInstance<SchemaManager>(value);
            }
        }

        public static TextFolderManager TextFolderManager
        {
            get
            {
                return EngineContext.Current.Resolve<TextFolderManager>();
            }
            set
            {
                EngineContext.Current.ContainerManager.AddComponentInstance<TextFolderManager>(value);
            }
        }
        public static TextContentManager TextContentManager
        {
            get
            {
                return EngineContext.Current.Resolve<TextContentManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<TextContentManager>(value);
            }
        }
        public static MediaContentManager MediaContentManager
        {
            get
            {
                return EngineContext.Current.Resolve<MediaContentManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<MediaContentManager>(value);
            }
        }
        public static MediaFolderManager MediaFolderManager
        {
            get
            {
                return EngineContext.Current.Resolve<MediaFolderManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<MediaFolderManager>(value);
            }
        }
        public static ReceivingSettingManager ReceiveSettingManager
        {
            get
            {
                return EngineContext.Current.Resolve<ReceivingSettingManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<ReceivingSettingManager>(value);
            }
        }

        public static SendingSettingManager SendingSettingManager
        {
            get
            {
                return EngineContext.Current.Resolve<SendingSettingManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<SendingSettingManager>(value);
            }
        }
        public static RepositoryTemplateManager RepositoryTemplateManager
        {
            get
            {
                return EngineContext.Current.Resolve<RepositoryTemplateManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<RepositoryTemplateManager>(value);
            }
        }
        public static SchemaTemplateManager SchemaTemplateManager
        {
            get
            {
                return EngineContext.Current.Resolve<SchemaTemplateManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<SchemaTemplateManager>(value);
            }
        }
        public static WorkflowManager WorkflowManager
        {
            get
            {
                return EngineContext.Current.Resolve<WorkflowManager>();
            }
            set
            {
                (EngineContext.Current).ContainerManager.AddComponentInstance<WorkflowManager>(value);
            }
        }
        public static T GetService<T>()
            where T : class
        {
            return EngineContext.Current.Resolve<T>();

        }
    }
}
