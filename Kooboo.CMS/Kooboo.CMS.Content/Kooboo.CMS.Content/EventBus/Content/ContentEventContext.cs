using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.EventBus.Content
{
    public class ContentEventContext : IEventContext
    {
        public ContentEventContext(ContentAction contentAction, TextContent content)
        {
            this.ContentAction = contentAction;
            this.Content = content;
        }
        public ContentAction ContentAction { get; set; }

        #region IEventContext Members

        public object State
        {
            get
            {
                return this.Content;
            }
        }

        #endregion

        public TextContent Content { get; private set; }

    }
}
