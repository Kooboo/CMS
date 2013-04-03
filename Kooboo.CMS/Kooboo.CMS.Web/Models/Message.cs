using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Models
{
    public class Message
    {
        public MessageType Type { get; set; }
        public string Content { get; set; }
    }

    public enum MessageType
    {
        info,
        warning,
        error
    }
}