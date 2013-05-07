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

namespace Kooboo.CMS.Content.EventBus.Content
{
    public static class ContentEvent
    {
        static ContentEvent()
        {
            EventBus.Subscribers.Add(new ContentSequenceSubscriber());
            EventBus.Subscribers.Add(new ContentBroadcastingSubscriber());
            EventBus.Subscribers.Add(new ContentVersioningSubscriber());
            EventBus.Subscribers.Add(new ContentWorkflowSubscriber());
            EventBus.Subscribers.Add(new ContentImageCropSubscriber());
            EventBus.Subscribers.Add(new CascadingContentDeletingSubscriber());
        }

        public static void Fire(ContentAction contentAction, TextContent textContent)
        {
            EventBus.Send(new ContentEventContext(contentAction, textContent));
        }
    }
}
