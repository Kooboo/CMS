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
using System.Text.RegularExpressions;
using System.Web;

namespace Kooboo.CMS.Common
{
    public static class UriHelper
    {
        // Methods
        public static Uri AttachQueryStringParameter(this Uri url, string parameterName, string parameterValue)
        {
            UriBuilder builder = new UriBuilder(url);
            string query = builder.Query;
            if (query.Length > 1)
            {
                query = query.Substring(1);
            }
            string str2 = parameterName + "=";
            string str3 = Uri.EscapeDataString(parameterValue);
            string str4 = Regex.Replace(query, str2 + @"[^\&]*", str2 + str3);
            if (str4 == query)
            {
                if (str4.Length > 0)
                {
                    str4 = str4 + "&";
                }
                str4 = str4 + str2 + str3;
            }
            builder.Query = str4;
            return builder.Uri;
        }

        //public static Uri ConvertToAbsoluteUri(string returnUrl, HttpContextBase context)
        //{
        //    if (Uri.IsWellFormedUriString(returnUrl, UriKind.Absolute))
        //    {
        //        return new Uri(returnUrl, UriKind.Absolute);
        //    }
        //    if (!VirtualPathUtility.IsAbsolute(returnUrl))
        //    {
        //        returnUrl = VirtualPathUtility.ToAbsolute(returnUrl);
        //    }
        //    return new Uri(context.Request.GetPublicFacingUrl(), returnUrl);
        //}
    }


}
