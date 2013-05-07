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
using System.Net;

namespace Kooboo.Web.Css
{
    public interface IStyleContext
    {
        string BaseUri { get; }

        bool ConvertImageUriToAbsolute { get; }

        string GetExternalStyleSheet(string relativeUri);
    }

    public class WebStyleContext : IStyleContext
    {
        public string BaseUri { get; set; }

        public bool ConvertImageUriToAbsolute
        {
            get
            {
                return true;
            }
        }

        public string GetExternalStyleSheet(string relativeUri)
        {
            WebClient client = new WebClient();
            return client.DownloadString(new Uri(new Uri(BaseUri), relativeUri));
        }
    }
}
