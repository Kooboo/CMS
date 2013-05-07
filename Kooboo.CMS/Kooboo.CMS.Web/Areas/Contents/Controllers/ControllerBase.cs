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
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.ComponentModel;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Web.Models;
///using Kooboo.CMS.Web.ErrorHandling;
using Kooboo.CMS.Content.Versioning;
using Kooboo.Web.Mvc;
using Kooboo.Extensions;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class ControllerBase : AreaControllerBase
    {
        static ControllerBase()
        {
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Repository), typeof(Repository_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Schema), typeof(Schema_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Column), typeof(Column_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Folder), typeof(Folder_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(TextFolder), typeof(TextFolder_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(MediaFolder), typeof(MediaFolder_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(MediaContent), typeof(MediaContent_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(BroadcastSetting), typeof(BroadcastSetting_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(ReceivedMessage), typeof(ReceivedMessage_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(ReceivingSetting), typeof(ReceivingSetting_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(SendingSetting), typeof(SendingSetting_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(VersionInfo), typeof(VersionInfo_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Workflow), typeof(Workflow_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(PendingWorkflowItem), typeof(PendingWorkflowItem_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(WorkflowHistory), typeof(WorkflowHistory_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Kooboo.CMS.Search.Models.SearchSetting), typeof(Kooboo.CMS.Web.Areas.Contents.Models.SearchSetting_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Kooboo.CMS.Content.Models.OrderSetting), typeof(OrderSetting_Metadata));

            //TypeDescriptorHelper.RegisterMetadataType(typeof(Kooboo.CMS.Search.Models.FolderIndexInfo), typeof(FolderIndexInfo_Metadata));
            //TypeDescriptorHelper.RegisterMetadataType(typeof(Kooboo.CMS.Search.Models.LastAction), typeof(LastAction_Metadata));

            //TypeDescriptorHelper.RegisterMetadataType(typeof(MediaContentMetadata), typeof(MediaContentMetadata_Metadata));
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }
    }
}
