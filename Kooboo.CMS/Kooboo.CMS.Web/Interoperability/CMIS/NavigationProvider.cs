using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMIS.Provider;
using NCMIS.Produce;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using NCMIS.ObjectModel.MetaData;
using NCMIS.ObjectModel;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class NavigationProvider : NavigationProviderBase
    {
        public override NCMIS.ObjectModel.CmisObjectList GetCheckedoutDocs(string repositoryId, string folderId, string filter, string orderBy, bool includeAllowableActions, IncludeRelationships includeRelationships, string renditionFilter, int? maxItems, int skipCount)
        {
            throw new NotSupportedException();
        }

        public override NCMIS.ObjectModel.PathedCmisObjectList GetChildren(string repositoryId, string folderId, int? maxItems, int skipCount, string orderBy, string filter, IncludeRelationships includeRelationships, string renditionFilter, bool includeAllowableActions, bool includePathSegment)
        {
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            IObjectService folderService = ObjectService.GetService(typeof(Folder));
            string objectId = folderId;
            folderService.TryPraseObjectId(objectId, out folderId);

            FolderType folderType = CmisFolderHelper.IdentifyFolderType(repository, folderId);

            PathedCmisObjectList pathedList = new PathedCmisObjectList();
            IEnumerable<PathedCmisObject> children = folderService.GetChildren(repositoryId, objectId, filter, includeRelationships)
                .Select(it => new PathedCmisObject() { Object = it });

            var count = children.Count();
            pathedList.NumItems = count.ToString();
            pathedList.HasMoreItems = false;

            //IEnumerable<ContentBase> contents = new ContentBase[0];
            if (folderType == FolderType.Content_Folder || folderType == FolderType.Media_Folder)
            {
                var folder = CmisFolderHelper.Parse(repository, folderId).AsActual();
                IContentQuery<ContentBase> contentQuery = null;
                if (folder is TextFolder)
                {
                    var textFolder = (TextFolder)folder;
                    var schema = new Schema(repository, textFolder.SchemaName).AsActual();
                    contentQuery = textFolder.CreateQuery();
                    if (!string.IsNullOrEmpty(filter))
                    {
                        foreach (var item in schema.Columns)
                        {
                            contentQuery = contentQuery.Or(new WhereContainsExpression(null, item.Name, filter));
                        }
                    }
                }
                else
                {
                    var mediaFolder = (TextFolder)folder;
                    contentQuery = mediaFolder.CreateQuery();
                    if (!string.IsNullOrEmpty(filter))
                    {
                        contentQuery = contentQuery.WhereContains("FileName", filter);
                    }
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    contentQuery = contentQuery.OrderBy(orderBy);
                }

                count = contentQuery.Count();
                var take = maxItems.HasValue ? maxItems.Value : count;
                pathedList.NumItems = count.ToString();
                pathedList.HasMoreItems = count > count + take;

                children = children.Concat(contentQuery.Select(it => new PathedCmisObject()
                {
                    Object = ObjectConvertor.ToCmis((TextContent)(it), includeRelationships != IncludeRelationships.None)
                }).Take(take));
            }

            pathedList.Objects = children.ToArray();

            return pathedList;
        }


        public override NCMIS.ObjectModel.PathedCmisObjectContainer[] GetDescendants(string repositoryId, string folderId, int? depth, string filter, IncludeRelationships includeRelationships, string renditionFilter, bool includeAllowableActions, bool includePathSegment)
        {
            throw new NotSupportedException();
        }

        // [WebGet(UriTemplate = "/{repositoryId}/object/{folderId}/parent")]
        public override NCMIS.ObjectModel.CmisObject GetFolderParent(string repositoryId, string folderId, string filter)
        {
            FolderObjectService folderService = (FolderObjectService)ObjectService.GetService(typeof(Folder));
            string objectId = folderId;
            if (folderService.TryPraseObjectId(objectId, out folderId))
            {
                return folderService.GetParent(repositoryId, objectId);
            }
            return ObjectConvertor.EmptyCmisObject();
        }

        //[WebGet(UriTemplate = "/{repositoryId}/foldertree/{folderId}")]
        public override NCMIS.ObjectModel.PathedCmisObjectContainer[] GetFolderTree(string repositoryId, string folderId, int? depth, string filter, IncludeRelationships includeRelationships, string renditionFilter, bool includeAllowableActions, bool includePathSegment)
        {
            throw new NotSupportedException();
        }

        public override NCMIS.ObjectModel.PathedCmisObject[] GetObjectParents(string repositoryId, string objectId, string filter, IncludeRelationships includeRelationships, string renditionFilter, bool includeAllowableActions, bool includeRelativePathSegment)
        {
            throw new NotSupportedException();
        }
    }
}
