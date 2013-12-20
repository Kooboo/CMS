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

namespace Kooboo.Web.Mvc.WebResourceLoader.DynamicClientResource
{
    public interface IDynamicClientResource
    {
        ResourceType ResourceType { get; }
        IEnumerable<string> SupportedFileExtensions { get; }
        bool Accept(string fileName);
        string RegisterResource(string filePath);
        /// <summary>
        /// Registers the client parser to parse the resource at client side.
        /// </summary>
        /// <returns></returns>
        string RegisterClientParser();
        /// <summary>
        /// Parses the client resource at the server side.
        /// </summary>
        /// <param name="virtualPath">Name of the file.</param>
        /// <returns></returns>
        string Parse(string virtualPath);
    }
}
