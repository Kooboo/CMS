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
using Kooboo.CMS.Content.Models;
using NCMIS.ObjectModel;
using Kooboo.CMS.Content.Services;
using NCMIS.Produce;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public enum FolderType
    {
        Root,
        Content_Folder_Root,
        Media_Folder_Root,
        Content_Folder,
        Media_Folder
    }

    public static class CmisFolderHelper
    {
        public static readonly string RootFolderName = "_rootfolder";

        public static readonly string Content_Folder_Root = "_contentfolder";

        public static readonly string Media_Folder_Root = "_mediafolder";

        public static CmisObject CreateSystemFolderObject(string systemFolder, string rootFolder)
        {
            return new CmisObject()
            {
                Id = systemFolder,
                Properties = new CmisProperties()
                {
                    Items = new CmisProperty[]{
                        CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:folder" }),
                        CmisPropertyHelper.CreateCmisPropertyCreatedBy(new string[] { "system" }),
                        CmisPropertyHelper.CreateCmisPropertyCreationDate(new DateTime[] {DateTime.UtcNow}),
                        CmisPropertyHelper.CreateCmisPropertyLastModificationDate(new DateTime[] { DateTime.UtcNow}),
                        CmisPropertyHelper.CreateCmisPropertyLastModifiedBy(new string[] { "system" }),
                        CmisPropertyHelper.CreateCmisPropertyName(new string[] { systemFolder }),
                        CmisPropertyHelper.CreateCmisPropertyObjectId(new string[] { systemFolder}),
                        CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:folder" }),
                        CmisPropertyHelper.CreateCmisPropertyObjectTypeId(new string[]{""}),
                        CmisPropertyHelper.CreateCmisPropertyParentId(new string[] { rootFolder }),
                        CmisPropertyHelper.CreateCmisPropertyPath(new string[] { string.Join("/",new []{rootFolder,systemFolder}) })
                    }
                }
            };
        }

        public static string CompositeFolderId(Folder folder)
        {
            if (folder is TextFolder)
            {
                return Content_Folder_Root + "$" + folder.FullName;
            }
            else
            {
                return Media_Folder_Root + "$" + folder.FullName;
            }
        }

        public static Folder Parse(Kooboo.CMS.Content.Models.Repository repository, string folderId)
        {
            string[] id = folderId.Split('$');
            if (string.Compare(id[0], Content_Folder_Root, true) == 0)
            {
                return FolderHelper.Parse<TextFolder>(repository, id[1]);
            }
            else if (string.Compare(id[0], Media_Folder_Root, true) == 0)
            {
                return FolderHelper.Parse<MediaFolder>(repository, id[1]);
            }
            throw new Exception("Unknow folder type");
        }

        public static FolderType IdentifyFolderType(Kooboo.CMS.Content.Models.Repository repository, string folderId)
        {
            if (string.Compare(folderId, RootFolderName, true) == 0)
            {
                return FolderType.Root;
            }
            if (string.Compare(folderId, Content_Folder_Root, true) == 0)
            {
                return FolderType.Content_Folder_Root;
            }
            if (string.Compare(folderId, Media_Folder_Root, true) == 0)
            {
                return FolderType.Media_Folder_Root;
            }
            var folder = Parse(repository, folderId);
            if (folder is TextFolder)
            {
                return FolderType.Content_Folder;
            }
            else
            {
                return FolderType.Media_Folder;
            }
        }
    }
}
