#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Content.APIs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IContentAPI
    {
        /// <summary>
        /// Adds the specified text content.
        /// </summary>
        /// <param name="textContent">Content of the text.</param>
        /// <param name="contentFiles">The content files.</param>
        void Add(TextContent textContent, IEnumerable<ContentFile> contentFiles);

        /// <summary>
        /// Updates the specified text content.
        /// </summary>
        /// <param name="textContent">Content of the text.</param>
        /// <param name="oldTextContnet">The old text contnet.</param>
        void Update(TextContent textContent, TextContent oldTextContnet);

        /// <summary>
        /// Deletes the specified content UUID.
        /// </summary>
        /// <param name="contentUUID">The content UUID.</param>
        void Delete(TextContent textContent);

        /// <summary>
        /// Adds the categories.
        /// </summary>
        /// <param name="textContent">Content of the text.</param>
        /// <param name="categories">The categories.</param>
        void AddCategories(TextContent textContent, params TextContent[] categories);

        /// <summary>
        /// Removes the categories.
        /// </summary>
        /// <param name="textContent">Content of the text.</param>
        /// <param name="categories">The categories.</param>
        void RemoveCategories(TextContent textContent, params TextContent[] categories);

        /// <summary>
        /// Removes all categories.
        /// </summary>
        /// <param name="textContent">Content of the text.</param>
        void RemoveAllCategories(TextContent textContent);

        /// <summary>
        /// Copies the specified text content.
        /// </summary>
        /// <param name="textContent">Content of the text.</param>
        /// <param name="resetValues">The reset values.</param>
        /// <returns></returns>
        TextContent Copy(TextContent textContent, IDictionary<string, object> resetValues);
        
    }
}
