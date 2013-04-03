using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMIS.ObjectModel;
using NCMIS.Produce;
using Kooboo.CMS.Content.Models;
using NCMIS.ObjectModel.MetaData;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public interface IObjectService
    {
        string GetObjectId(object o);
        bool TryPraseObjectId(string objectId, out string id);
        void DeleteObject(string repositoryId, string objectId);
        CmisObject GetObject(string objectId);
        CmisObject GetParent(string repositoryId, string objectId);
        IEnumerable<CmisObject> All(string repositoryId);
        IEnumerable<CmisObject> GetChildren(string repositoryId, string objectId, string filter, IncludeRelationships includeRelationships);

        AllowableActions GetAllowableActions(string repositoryId, string objectId);
        NCMIS.ObjectModel.ContentStream GetContentStream(string repositoryId, string objectId, string streamId);
        NCMIS.ObjectModel.CmisObject GetObject(string repositoryId, string objectId);
        CmisProperties GetProperties(string repositoryId, string objectId);
        void SetContentStream(string repositoryId, string documentId, NCMIS.ObjectModel.ContentStream contentStream, bool? overwriteFlag);
        void UpdateProperties(string repositoryId, string objectId, NCMIS.ObjectModel.CmisProperties properties);
    }


    public static class ObjectService
    {
        static Dictionary<Type, IObjectService> services = new Dictionary<Type, IObjectService>()
        {
           {typeof(Folder), new FolderObjectService()},
           {typeof(ContentBase),new DocumentObjectService()}
        };
        public static IObjectService GetService(Type type)
        {
            return services.Where(it => it.Key.IsAssignableFrom(type)).Select(it => it.Value).First();
        }

        public static string GetObjectId(object o)
        {
            return GetService(o.GetType()).GetObjectId(o);
        }
        public static IObjectService GetService(string objectId)
        {
            string id;
            foreach (var service in services.Values)
            {
                if (service.TryPraseObjectId(objectId, out id))
                {
                    return service;
                }
            }
            return null;
        }
    }
}
