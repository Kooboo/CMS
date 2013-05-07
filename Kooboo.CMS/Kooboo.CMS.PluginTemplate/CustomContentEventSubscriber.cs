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
using Kooboo.CMS.Content.EventBus;
using Kooboo.CMS.Content.EventBus.Content;
using Kooboo.CMS.Content.Models;
namespace Kooboo.CMS.PluginTemplate
{
    public class CustomContentEventSubscriber : ISubscriber
    {
        EventResult ISubscriber.Receive(IEventContext context)
        {
            return null;
        }
    }
}
