using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.VSExtensionTemplate.Extensions.MembershipConnectClient
{
    static class StringExtensions
    {
        public static string UrlEncode(this string source)
        {
            return HttpUtility.UrlEncode(source);
        }

        public static string UrlDecode(this string source)
        {
            return HttpUtility.UrlDecode(source);
        }
    }
}
