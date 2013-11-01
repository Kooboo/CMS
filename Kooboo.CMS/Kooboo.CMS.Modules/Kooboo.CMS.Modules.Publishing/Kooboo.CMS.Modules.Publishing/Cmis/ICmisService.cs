#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.Publishing.Cmis
{
    public interface ICmisService
    {
        IEnumerable<KeyValuePair<string, string>> GetRepositories();
        IEnumerable<TreeNode<KeyValuePair<string, string>>> GetFolderTrees(string reposiotryId);

        string AddTextContent(string repositoryId, string folderId, TextContent textContent);
        string UpdateTextContent(string repositoryId, string folderId, TextContent textContent);
        void DeleteTextContent(string repositoryId, string folderId, string contentUUID);

        string AddPage(string repositoryId, Page page);
        string UpdatePage(string repositoryId, Page page);
        void DeletePage(string repositoryId, string pageId);
    }
}
