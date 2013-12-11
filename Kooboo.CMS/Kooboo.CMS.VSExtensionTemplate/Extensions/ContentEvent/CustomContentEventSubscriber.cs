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
using Kooboo.CMS.Content.Persistence;
namespace Kooboo.CMS.VSExtensionTemplate.Extension.ContentEvent
{
    //uncomment below code to use this as an example

    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ISubscriber), Key = "MyEvent")]
    //public class CustomContentEventSubscriber : ISubscriber
    //{

    //    ITextContentProvider _textContentProvider;
    //    public CustomContentEventSubscriber(ITextContentProvider textContentProvider)
    //    {
    //        this._textContentProvider = textContentProvider;
    //    }

    //    EventResult ISubscriber.Receive(IEventContext context)
    //    {
    //        if (context is ContentEventContext)
    //        {
    //            var contentEventContext = (ContentEventContext)context;
    //            switch (contentEventContext.ContentAction)
    //            {
    //                case ContentAction.Add:
    //                    break;
    //                case ContentAction.Delete:
    //                    break;
    //                case ContentAction.PreAdd:
    //                    break;
    //                case ContentAction.PreDelete:
    //                    break;
    //                case ContentAction.PreUpdate:
    //                    break;
    //                case ContentAction.Update:

    //                    // sample code to append an extra string to the content titile after one content is updated.
    //                    var reponame = contentEventContext.Content.GetRepository().Name;
    //                    contentEventContext.Content["Title"] = contentEventContext.Content["Title"].ToString() + " new string appended at the end " + reponame;
    //                    _textContentProvider.Update(contentEventContext.Content, contentEventContext.Content);
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }

    //        return new EventResult() { IsCancelled = false };
    //    }
    //}
}
