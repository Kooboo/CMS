using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Content
{
    public class ItemAlreadyExistsException : FriendlyException
    {
        public ItemAlreadyExistsException()
            : base("The item already exists.")
        {

        }
    }
}
