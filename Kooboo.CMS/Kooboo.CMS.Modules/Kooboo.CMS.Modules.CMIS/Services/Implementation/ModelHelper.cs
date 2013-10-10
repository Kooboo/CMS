#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Modules.CMIS.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Globalization;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    public static class ModelHelper
    {
        #region CreateFault
        public static cmisFaultType CreateFault(enumServiceException type, string message = null)
        {
            var code = 500;
            switch (type)
            {
                case enumServiceException.constraint:
                case enumServiceException.nameConstraintViolation:
                case enumServiceException.contentAlreadyExists:
                case enumServiceException.updateConflict:
                case enumServiceException.versioning:
                    code = 409;
                    break;
                case enumServiceException.filterNotValid:
                case enumServiceException.invalidArgument:
                    code = 400;
                    break;
                case enumServiceException.notSupported:
                    code = 405;
                    break;
                case enumServiceException.objectNotFound:
                    code = 404;
                    break;
                case enumServiceException.permissionDenied:
                case enumServiceException.streamNotSupported:
                    code = 403;
                    break;
                case enumServiceException.runtime:
                case enumServiceException.storage:
                default:
                    code = 500;
                    break;
            }

            return new cmisFaultType() { type = type, code = code.ToString(), message = message };
        }
        #endregion
        #region GetSite
        public static Kooboo.CMS.Sites.Models.Site GetSite(string repositoryId)
        {
            var site = new Kooboo.CMS.Sites.Models.Site(repositoryId).AsActual();
            if (site == null)
            {
                throw new FaultException<cmisFaultType>(CreateFault(enumServiceException.objectNotFound, string.Format("No such site:{0}.".Localize(), repositoryId)));
            }
            return site;
        }
        #endregion

        #region GetRepository
        public static Kooboo.CMS.Content.Models.Repository GetRepository(string repositoryId)
        {
            var site = GetSite(repositoryId);
            var repository = site.GetRepository();
            if (repository == null)
            {
                throw new FaultException<cmisFaultType>(CreateFault(enumServiceException.objectNotFound, string.Format("No such repository:{0}.".Localize(), repositoryId)));
            }
            return repository;
        }
        #endregion

        #region GetSchema
        public static Kooboo.CMS.Content.Models.Schema GetSchema(string repositoryId, string typeId)
        {
            var repository = GetRepository(repositoryId);
            var schema = new Kooboo.CMS.Content.Models.Schema(repository, typeId).AsActual();
            if (schema == null)
            {
                throw new FaultException<cmisFaultType>(CreateFault(enumServiceException.objectNotFound, string.Format("No such object type:{0}.".Localize(), typeId)));
            }
            return schema;
        }
        #endregion

        #region GetTextFolder
        public static Kooboo.CMS.Content.Models.TextFolder GetTextFolder(string repositoryId, string folderId)
        {
            var repository = GetRepository(repositoryId);
            var textFolder = new Kooboo.CMS.Content.Models.TextFolder(repository, folderId).AsActual();
            if (textFolder == null)
            {
                throw new FaultException<cmisFaultType>(CreateFault(enumServiceException.objectNotFound, string.Format("No such folder:{0}.".Localize(), folderId)));
            }
            return textFolder;
        }
        #endregion

        #region ToNameValueCollection
        public static NameValueCollection ToNameValueCollection(this cmisPropertiesType cmisProperties)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();

            if (cmisProperties.Items != null)
            {
                foreach (var item in cmisProperties.Items)
                {
                    nameValueCollection[string.IsNullOrEmpty(item.localName) ? item.propertyDefinitionId : item.localName] = item.stringValue;
                }
            }

            return nameValueCollection;
        }
        #endregion

        #region TocmisObjectType
        public static cmisObjectType TocmisObjectType(TextContent textContent)
        {
            cmisObjectType cmisObject = new cmisObjectType();
            cmisObject.properties = ToCmisPropertiesType(textContent);
            return cmisObject;
        }
        public static cmisPropertiesType ToCmisPropertiesType(TextContent textContent)
        {
            var properties = new cmisPropertiesType();
            var items = new List<cmisProperty>();
            items.Add(new cmisPropertyId() { localName = CmisPropertyDefinitionId.BaseTypeId, propertyDefinitionId = CmisPropertyDefinitionId.BaseTypeId, value = new string[] { "cmis:document" } });
            items.Add(new cmisPropertyId() { localName = "UserId", propertyDefinitionId = CmisPropertyDefinitionId.CreatedBy, value = new string[] { textContent.UserId } });
            items.Add(new cmisPropertyDateTime() { localName = "UtcCreationDate", propertyDefinitionId = CmisPropertyDefinitionId.CreationDate, value = new[] { textContent.UtcCreationDate } });
            items.Add(new cmisPropertyDateTime() { localName = "UtcLastModificationDate", propertyDefinitionId = CmisPropertyDefinitionId.LastModificationDate, value = new[] { textContent.UtcLastModificationDate } });
            items.Add(new cmisPropertyString() { localName = "UserKey", propertyDefinitionId = CmisPropertyDefinitionId.Name, value = new[] { textContent.UserKey } });
            items.Add(new cmisPropertyId() { localName = "IntegrateId", propertyDefinitionId = CmisPropertyDefinitionId.ObjectId, value = new[] { textContent.IntegrateId } });
            items.Add(new cmisPropertyId() { localName = "SchemaName", propertyDefinitionId = CmisPropertyDefinitionId.ObjectTypeId, value = new[] { textContent.SchemaName } });
            items.Add(new cmisPropertyString() { localName = "ParentUUID", propertyDefinitionId = CmisPropertyDefinitionId.ParentId, value = new[] { textContent.ParentUUID } });


            foreach (var item in textContent)
            {
                if (!item.Key.StartsWith("__"))
                {
                    items.Add(ToCmisProperty(item));
                }

            }
            properties.Items = items.ToArray();
            return properties;
        }
        private static cmisProperty ToCmisProperty(KeyValuePair<string, object> item)
        {
            if (item.Value == null)
            {
                return new cmisPropertyString() { propertyDefinitionId = item.Key, localName = item.Key, value = null };
            }
            else if (item.Value is DateTime)
            {
                return new cmisPropertyDateTime() { propertyDefinitionId = item.Key, localName = item.Key, value = new[] { (DateTime)item.Value } };
            }
            else if (item.Value is int)
            {
                return new cmisPropertyInteger() { propertyDefinitionId = item.Key, localName = item.Key, value = new[] { item.Value.ToString() } };
            }
            else if (item.Value is decimal)
            {
                return new cmisPropertyDecimal() { propertyDefinitionId = item.Key, localName = item.Key, value = new[] { (decimal)item.Value } };
            }
            else if (item.Value is bool)
            {
                return new cmisPropertyBoolean() { propertyDefinitionId = item.Key, localName = item.Key, value = new[] { (bool)item.Value } };
            }
            else
            {
                return new cmisPropertyString() { propertyDefinitionId = item.Key, localName = item.Key, value = new string[] { item.Value.ToString() } };
            }
        }
        #endregion
    }
}
