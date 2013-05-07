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
using NCMIS.Produce;
using Kooboo.CMS.Content.Services;
using NCMIS.ObjectModel;
using Kooboo.Extensions;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class FolderObjectService : IObjectService
    {
        string prefix = "_folder";




        public CmisObject CreateFolder(string repositoryId, NCMIS.ObjectModel.CmisProperties properties, string folderId)
        {
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            Folder parent = null;
            var objectId = folderId;

            var values = properties.ToNameValueCollection();
            if (string.IsNullOrEmpty(values["name"]))
            {
                throw new Exception("The property \"Name\" is required.");
            }
            if (TryPraseObjectId(objectId, out folderId))
            {
                var folderType = CmisFolderHelper.IdentifyFolderType(repository, folderId);
                switch (folderType)
                {
                    case FolderType.Root:
                        throw new Exception("Could not create folder under root folder.");
                    case FolderType.Content_Folder_Root:
                        parent = null;
                        return AddContentFolder(repository, parent, values);

                    case FolderType.Media_Folder_Root:
                        parent = null;
                        return AddMediaFolder(repository, parent, values);

                    case FolderType.Content_Folder:
                        parent = CmisFolderHelper.Parse(repository, folderId);
                        return AddContentFolder(repository, parent, values);

                    case FolderType.Media_Folder:
                        parent = CmisFolderHelper.Parse(repository, folderId);
                        return AddMediaFolder(repository, parent, values);
                    default:
                        break;
                }
            }
            throw new InvalidOperationException("Create folder failed.");
        }

        private static CmisObject AddContentFolder(Kooboo.CMS.Content.Models.Repository repository, Folder parent, System.Collections.Specialized.NameValueCollection values)
        {
            var textFolder = new TextFolder(repository, values["name"], parent);
            textFolder.DisplayName = values["DisplayName"];
            textFolder.SchemaName = values["schemaName"];
            var categories = values["CategoryFolders"];
            if (!string.IsNullOrEmpty(categories))
            {
                textFolder.CategoryFolders = categories.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            }
            var creationDate = values["UtcCreationDate"];
            if (!string.IsNullOrEmpty(creationDate))
            {
                textFolder.UtcCreationDate = DateTime.Parse(creationDate).ToUniversalTime();
            }

            ServiceFactory.TextFolderManager.Add(repository, textFolder);

            return ObjectConvertor.ToCmis(textFolder, false);
        }

        private static CmisObject AddMediaFolder(Kooboo.CMS.Content.Models.Repository repository, Folder parent, System.Collections.Specialized.NameValueCollection values)
        {
            var mediaFolder = new MediaFolder(repository, values["name"], parent);
            mediaFolder.DisplayName = values["DisplayName"];
            var allowedExtensions = values["AllowedExtensions"];
            if (!string.IsNullOrEmpty(allowedExtensions))
            {
                mediaFolder.AllowedExtensions = allowedExtensions.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }

            var creationDate = values["UtcCreationDate"];
            if (!string.IsNullOrEmpty(creationDate))
            {
                mediaFolder.UtcCreationDate = DateTime.Parse(creationDate).ToUniversalTime();
            }

            ServiceFactory.MediaFolderManager.Add(repository, mediaFolder);

            return ObjectConvertor.ToCmis(mediaFolder, false);
        }



        #region IObjectService<Folder> Members

        public bool TryPraseObjectId(string objectId, out string id)
        {
            id = string.Empty;
            if (IsSystemFolder(objectId))
            {
                id = objectId;
                return false;
            }
            if (objectId.StartsWith(prefix))
            {
                id = objectId.Substring(prefix.Length);
                return true;
            }
            return false;
        }
        public bool IsSystemFolder(string objectId)
        {
            if (objectId.EqualsOrNullEmpty(CmisFolderHelper.Content_Folder_Root, StringComparison.CurrentCultureIgnoreCase)
                || objectId.EqualsOrNullEmpty(CmisFolderHelper.Media_Folder_Root, StringComparison.CurrentCultureIgnoreCase)
                || objectId.EqualsOrNullEmpty(CmisFolderHelper.RootFolderName, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }
        public string GetObjectId(object o)
        {
            return prefix + CmisFolderHelper.CompositeFolderId((Folder)o);
        }


        public NCMIS.ObjectModel.CmisObject GetObject(string objectId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<NCMIS.ObjectModel.CmisObject> All(string repositoryId)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<NCMIS.ObjectModel.CmisObject> GetChildren(string repositoryId, string objectId, string filter, IncludeRelationships includeRelationships)
        {
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            string folderId;
            TryPraseObjectId(objectId, out folderId);
            switch (CmisFolderHelper.IdentifyFolderType(repository, folderId))
            {
                case FolderType.Root:
                    return new[] {
                    CmisFolderHelper.CreateSystemFolderObject(CmisFolderHelper.Content_Folder_Root,CmisFolderHelper.RootFolderName),
                   CmisFolderHelper.CreateSystemFolderObject(CmisFolderHelper.Media_Folder_Root,CmisFolderHelper.RootFolderName)
                    };
                case FolderType.Content_Folder_Root:
                    return ServiceFactory.TextFolderManager.All(repository, filter).Select(it => ObjectConvertor.ToCmis((TextFolder)(it.AsActual()), includeRelationships != IncludeRelationships.None));
                case FolderType.Media_Folder_Root:
                    return ServiceFactory.MediaFolderManager.All(repository, filter).Select(it => ObjectConvertor.ToCmis((MediaFolder)(it.AsActual()), includeRelationships != IncludeRelationships.None));
                case FolderType.Content_Folder:
                    var textFolder = (TextFolder)CmisFolderHelper.Parse(repository, folderId);
                    return ServiceFactory.TextFolderManager.ChildFolders(textFolder, filter).Select(it => ObjectConvertor.ToCmis((TextFolder)(it.AsActual()), includeRelationships != IncludeRelationships.None));
                case FolderType.Media_Folder:
                    var mediaFolder = (MediaFolder)CmisFolderHelper.Parse(repository, folderId);
                    return ServiceFactory.MediaFolderManager.ChildFolders(mediaFolder, filter).Select(it => ObjectConvertor.ToCmis((MediaFolder)(it.AsActual()), includeRelationships != IncludeRelationships.None));
                default:
                    break;
            }
            return new[] { ObjectConvertor.EmptyCmisObject() };
        }

        public NCMIS.ObjectModel.CmisObject GetParent(string repositoryId, string objectId)
        {
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            string folderId;

            if (!IsSystemFolder(objectId) && TryPraseObjectId(objectId, out folderId))
            {
                var folder = CmisFolderHelper.Parse(repository, folderId);

                if (folder.Parent != null)
                {
                    return ObjectConvertor.ToCmis(folder.Parent.AsActual(), false);
                }
            }

            return ObjectConvertor.EmptyCmisObject();
        }

        public void DeleteObject(string repositoryId, string objectId)
        {
            string folderId;
            TryPraseObjectId(objectId, out folderId);
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            var folder = CmisFolderHelper.Parse(repository, folderId);

            if (folder is TextFolder)
            {
                ServiceFactory.TextFolderManager.Remove(repository, (TextFolder)folder);
            }
            else
            {
                ServiceFactory.MediaFolderManager.Remove(repository, (MediaFolder)folder);
            }

        }

        public NCMIS.ObjectModel.MetaData.AllowableActions GetAllowableActions(string repositoryId, string objectId)
        {
            if (IsSystemFolder(objectId))
            {
                return new NCMIS.ObjectModel.MetaData.AllowableActions()
                {
                    CanAddObjectToFolder = false,
                    CanApplyACL = false,
                    CanApplyPolicy = false,
                    CanCancelCheckOut = false,
                    CanCheckIn = false,
                    CanCheckOut = false,
                    CanCreateDocument = false,
                    CanCreateFolder = CmisFolderHelper.IdentifyFolderType(new Kooboo.CMS.Content.Models.Repository(repositoryId), objectId) != FolderType.Root,
                    CanCreateRelationship = false,
                    CanDeleteContentStream = false,
                    CanDeleteObject = true,
                    CanDeleteTree = false,
                    CanGetACL = false,
                    CanGetAllVersions = false,
                    CanGetAppliedPolicies = false,
                    CanGetChildren = true,
                    CanGetContentStream = false,
                    CanGetDescendants = false,
                    CanGetFolderParent = false,
                    CanGetFolderTree = false,
                    CanGetObjectParents = false,
                    CanGetObjectRelationships = true,
                    CanGetProperties = false,
                    CanGetRenditions = false,
                    CanMoveObject = false,
                    CanRemoveObjectFromFolder = false,
                    CanRemovePolicy = false,
                    CanSetContentStream = false,
                    CanUpdateProperties = false

                };
            }
            else
            {
                return new NCMIS.ObjectModel.MetaData.AllowableActions()
                {
                    CanAddObjectToFolder = false,
                    CanApplyACL = false,
                    CanApplyPolicy = false,
                    CanCancelCheckOut = false,
                    CanCheckIn = false,
                    CanCheckOut = false,
                    CanCreateDocument = true,
                    CanCreateFolder = true,
                    CanCreateRelationship = true,
                    CanDeleteContentStream = false,
                    CanDeleteObject = true,
                    CanDeleteTree = false,
                    CanGetACL = false,
                    CanGetAllVersions = false,
                    CanGetAppliedPolicies = false,
                    CanGetChildren = true,
                    CanGetContentStream = true,
                    CanGetDescendants = false,
                    CanGetFolderParent = true,
                    CanGetFolderTree = false,
                    CanGetObjectParents = false,
                    CanGetObjectRelationships = true,
                    CanGetProperties = true,
                    CanGetRenditions = false,
                    CanMoveObject = false,
                    CanRemoveObjectFromFolder = false,
                    CanRemovePolicy = false,
                    CanSetContentStream = false,
                    CanUpdateProperties = true

                };
            }
        }


        public ContentStream GetContentStream(string repositoryId, string objectId, string streamId)
        {
            throw new NotSupportedException();
        }

        public CmisObject GetObject(string repositoryId, string objectId)
        {
            string folderId;
            TryPraseObjectId(objectId, out folderId);
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            var folder = CmisFolderHelper.Parse(repository, folderId);

            return ObjectConvertor.ToCmis(folder.AsActual(), false);
        }

        public CmisProperties GetProperties(string repositoryId, string objectId)
        {
            return GetObject(repositoryId, objectId).Properties;
        }

        public void SetContentStream(string repositoryId, string documentId, ContentStream contentStream, bool? overwriteFlag)
        {
            throw new NotSupportedException();
        }

        public void UpdateProperties(string repositoryId, string objectId, CmisProperties properties)
        {
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            string folderId;
            TryPraseObjectId(objectId, out folderId);
            var folder = CmisFolderHelper.Parse(repository, folderId);

            var values = properties.ToNameValueCollection();
            if (folder is TextFolder)
            {
                var textFolder = (TextFolder)folder;

                if (values["DisplayName"] != null)
                {
                    textFolder.DisplayName = values["DisplayName"];
                }
                if (values["SchemaName"] != null)
                {
                    textFolder.SchemaName = values["SchemaName"];
                }
                if (values["CategoryFolders"] != null)
                {
                    textFolder.CategoryFolders = values["CategoryFolders"].Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                }
                ServiceFactory.TextFolderManager.Update(repository, textFolder, textFolder);
            }
            else
            {
                var mediaFolder = (MediaFolder)folder;
                if (values["DisplayName"] != null)
                {
                    mediaFolder.DisplayName = values["DisplayName"];
                }
                if (values["AllowedExtensions"] != null)
                {
                    mediaFolder.AllowedExtensions = values["AllowedExtensions"].Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                }
                ServiceFactory.MediaFolderManager.Update(repository, mediaFolder, mediaFolder);
            }
        }

        #endregion
    }
}
