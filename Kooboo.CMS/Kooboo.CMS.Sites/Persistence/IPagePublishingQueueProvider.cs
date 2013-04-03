using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IPagePublishingQueueProvider : IProvider<PagePublishingQueueItem>
    {
    }
}
