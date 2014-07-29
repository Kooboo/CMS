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
using NCMIS.ObjectModel;
using NCMIS.ObjectModel.MetaData;
using System.Collections.Specialized;
using Kooboo.Extensions;
namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public static class CmisPropertyHelper
    {
        #region Create CMIS Properties

        /// <summary>
        /// Id's of the set of Object-types that can be created, moved or filed into this
        /// folder. Line 1271 in CMIS specification
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyId CreateCmisPropertyAllowedChildObjectTypeIds(string[] value)
        {
            return new CmisPropertyId()
            {
                DisplayName = "Allowed Child Object Type Ids",
                LocalName = "AllowedChildObjectTypeIds",
                PropertyDefinitionId = CmisPropertyDefinitionId.AllowedChildObjectTypeIds,
                QueryName = CmisPropertyDefinitionId.AllowedChildObjectTypeIds,
                Value = value
            };
        }

        /// <summary>
        /// Id of the object's type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyId CreateCmisPropertyObjectTypeId(string[] value)
        {
            return new CmisPropertyId()
            {
                DisplayName = "Object Type Id",
                LocalName = "ObjectTypeId",
                PropertyDefinitionId = CmisPropertyDefinitionId.ObjectTypeId,
                QueryName = CmisPropertyDefinitionId.ObjectTypeId,
                Value = value
            };
        }

        /// <summary>
        /// Id of the object's type. Defaults to cmis:document.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyDocumentObjectTypeId()
        {
            return new CmisPropertyId()
            {
                DisplayName = "CMIS Document",
                LocalName = "CmisDocument",
                PropertyDefinitionId = CmisPropertyDefinitionId.ObjectTypeId,
                QueryName = "cmis:objectTypeId",
                Value = new string[] { "cmis:document" }
            };
        }

        /// <summary>
        /// Id of the object's type. Defaults to cmis:folder.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyFolderObjectTypeId()
        {
            return new CmisPropertyId()
            {
                DisplayName = "CMIS Folder",
                LocalName = "CmisFolder",
                PropertyDefinitionId = CmisPropertyDefinitionId.ObjectTypeId,
                QueryName = "cmis:objectTypeId",
                Value = new string[] { "cmis:folder" }
            };
        }

        /// <summary>
        /// Id of the object's type. Defaults to cmis:policy.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyPolicyObjectTypeId()
        {
            return new CmisPropertyId()
            {
                DisplayName = "CMIS Policy",
                LocalName = "CmisPolicy",
                PropertyDefinitionId = CmisPropertyDefinitionId.ObjectTypeId,
                QueryName = "cmis:objectTypeId",
                Value = new string[] { "cmis:policy" }
            };
        }

        /// <summary>
        /// User who last modified the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyString CreateCmisPropertyLastModifiedBy(string[] value)
        {
            return new CmisPropertyString()
            {
                DisplayName = "Last Modified By",
                LocalName = "LastModifiedBy",
                PropertyDefinitionId = CmisPropertyDefinitionId.LastModifiedBy,
                QueryName = CmisPropertyDefinitionId.LastModifiedBy,
                Value = value
            };
        }

        /// <summary>
        /// The fully qualified path to this folder.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyString CreateCmisPropertyPath(string[] value)
        {
            return new CmisPropertyString()
            {
                DisplayName = "Path",
                LocalName = "Path",
                PropertyDefinitionId = CmisPropertyDefinitionId.Path,
                QueryName = CmisPropertyDefinitionId.Path,
                Value = value
            };
        }

        /// <summary>
        /// Describes the position of an individual object with respect to the 
        /// version series, e.g. "Version 1.0".
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyVersionLabel()
        {
            return new CmisPropertyString()
            {
                DisplayName = "Version Label",
                LocalName = "VersionLabel",
                PropertyDefinitionId = CmisPropertyDefinitionId.VersionLabel,
                QueryName = "cmis:versionLabel",
                Value = null
            };
        }

        /// <summary>
        /// Indicates which user created the PWC (Private Working Copy).
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyVersionSeriesCheckedOutBy()
        {
            return new CmisPropertyString()
            {
                DisplayName = "Version Series Checked Out By",
                LocalName = "VersionSeriesCheckedOutBy",
                PropertyDefinitionId = CmisPropertyDefinitionId.VersionSeriesCheckedOutBy,
                QueryName = "cmis:versionSeriesCheckedOutBy",
                Value = null
            };
        }

        /// <summary>
        /// The object id of the PWC.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyVersionSeriesCheckedOutId()
        {
            return new CmisPropertyString()
            {
                DisplayName = "Version Series Checked Out ID",
                LocalName = "VersionSeriesCheckedOutId",
                PropertyDefinitionId = CmisPropertyDefinitionId.VerisonSeriesCheckedOutId,
                QueryName = "cmis:versionSeriesCheckedOutId",
                Value = null
            };
        }

        /// <summary>
        /// The id of the version series (a transitively closed collection of all
        /// document objects that have been created from an original document in 
        /// the repository.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyVersionSeriesId()
        {
            return new CmisPropertyString()
            {
                DisplayName = "Version Series ID",
                LocalName = "VersionSeriesId",
                PropertyDefinitionId = CmisPropertyDefinitionId.VersionSeriesId,
                QueryName = "cmis:versionSeriesId",
                Value = new string[] { "workspace://SpacesStore/7df6b8f6-12d8-4e4a-903d-63a47b88b56e" }
            };
        }

        /// <summary>
        /// The name of the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyString CreateCmisPropertyName(string[] value)
        {
            return new CmisPropertyString()
            {
                DisplayName = "Name",
                LocalName = "Name",
                PropertyDefinitionId = CmisPropertyDefinitionId.Name,
                QueryName = CmisPropertyDefinitionId.Name,
                Value = value
            };
        }

        /// <summary>
        /// Textual comment associated with the given version.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyCheckinComment()
        {
            return new CmisPropertyString()
            {
                DisplayName = "Checking Comment",
                LocalName = "CheckinComment",
                PropertyDefinitionId = CmisPropertyDefinitionId.CheckinComment,
                QueryName = "cmis:checkinComment",
                Value = null
            };
        }

        /// <summary>
        /// Id of the stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyId CreateCmisPropertyContentStreamId(string[] value)
        {
            return new CmisPropertyId()
            {
                DisplayName = "Content Stream ID",
                LocalName = "ContentStreamId",
                PropertyDefinitionId = CmisPropertyDefinitionId.ContentStreamId,
                QueryName = CmisPropertyDefinitionId.ContentStreamId,
                Value = value
            };
        }

        /// <summary>
        /// File name of the content stream. 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyString CreateCmisPropertyContentStreamFileName(string[] value)
        {
            return new CmisPropertyString()
            {
                DisplayName = "Content Stream File Name",
                LocalName = "ContentStreamFileName",
                PropertyDefinitionId = CmisPropertyDefinitionId.ContentStreamFileName,
                QueryName = CmisPropertyDefinitionId.ContentStreamFileName,
                Value = value
            };
        }

        /// <summary>
        /// Length of the content stream (in bytes).
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyInteger CreateCmisPropertyContentStreamLength(int[] value)
        {
            return new CmisPropertyInteger()
            {
                DisplayName = "Content Stream Length",
                LocalName = "ContentStreamLength",
                PropertyDefinitionId = CmisPropertyDefinitionId.ContentStreamLength,
                QueryName = CmisPropertyDefinitionId.ContentStreamLength,
                Value = value
            };
        }

        /// <summary>
        /// MIME type of the content stream.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyString CreateCmisPropertyContentStreamMimeType(string[] value)
        {
            return new CmisPropertyString()
            {
                DisplayName = "Content Stream MIME Type",
                LocalName = "ContentStreamMimeType",
                PropertyDefinitionId = CmisPropertyDefinitionId.ContentStreamMimeType,
                QueryName = CmisPropertyDefinitionId.ContentStreamMimeType,
                Value = value
            };
        }

        /// <summary>
        /// User who created the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyString CreateCmisPropertyCreatedBy(string[] value)
        {
            return new CmisPropertyString()
            {
                DisplayName = "Created By",
                LocalName = "CreatedBy",
                PropertyDefinitionId = CmisPropertyDefinitionId.CreatedBy,
                QueryName = CmisPropertyDefinitionId.CreatedBy,
                Value = value
            };
        }

        /// <summary>
        /// Id of the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyId CreateCmisPropertyObjectId(string[] value)
        {
            return new CmisPropertyId()
            {
                DisplayName = "Object Id",
                LocalName = "ObjectId",
                PropertyDefinitionId = CmisPropertyDefinitionId.ObjectId,
                QueryName = CmisPropertyDefinitionId.ObjectId,
                Value = value
            };
        }

        /// <summary>
        /// The date and time (type DateTime) of when the object was 
        /// created.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyDateTime CreateCmisPropertyCreationDate(DateTime[] value)
        {
            return new CmisPropertyDateTime()
            {
                DisplayName = "Creation Date",
                LocalName = "CreationDate",
                PropertyDefinitionId = CmisPropertyDefinitionId.CreationDate,
                QueryName = CmisPropertyDefinitionId.CreationDate,
                Value = value
            };
        }

        /// <summary>
        /// Opaque token used for optimistic locking and concurrency
        /// checking. See section 2.2.1.3 Change Tokens.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyChangeToken()
        {
            return new CmisPropertyString()
            {
                DisplayName = "Change Token",
                LocalName = "ChangeToken",
                PropertyDefinitionId = CmisPropertyDefinitionId.ChangeToken,
                QueryName = "cmis:changeToken",
                Value = null
            };
        }

        /// <summary>
        /// Id of the base object-type for the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyId CreateCmisPropertyBaseTypeId(string[] value)
        {
            return new CmisPropertyId()
            {
                DisplayName = "Base Type Id",
                LocalName = "BaseTypeId",
                PropertyDefinitionId = CmisPropertyDefinitionId.BaseTypeId,
                QueryName = CmisPropertyDefinitionId.BaseTypeId,
                Value = value
            };
        }

        /// <summary>
        /// TRUE if the repository MUST throw an error at any attempt to update
        /// or delete the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyBoolean CreateCmisPropertyIsImmutable(bool[] value)
        {
            return new CmisPropertyBoolean()
            {
                DisplayName = "Is Immutable",
                LocalName = "IsImmutable",
                PropertyDefinitionId = CmisPropertyDefinitionId.IsImmutable,
                QueryName = CmisPropertyDefinitionId.IsImmutable,
                Value = value
            };
        }

        /// <summary>
        /// TRUE if the Document object is the latest major version in its 
        /// version series.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyBoolean CreateCmisPropertyIsLatestMajorVersion(bool[] value)
        {
            return new CmisPropertyBoolean()
            {
                DisplayName = "Is Latest Major Version",
                LocalName = "IsLatestMajorVersion",
                PropertyDefinitionId = CmisPropertyDefinitionId.IsLatestMajorVersion,
                QueryName = CmisPropertyDefinitionId.IsLatestMajorVersion,
                Value = value
            };
        }


        /// <summary>
        /// TRUE if the Document object is the latest major version in its 
        /// version series. Defaults to FALSE.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyIsMajorVersion()
        {
            return new CmisPropertyBoolean()
            {
                DisplayName = "Is Major Version",
                LocalName = "IsMajorVersion",
                PropertyDefinitionId = CmisPropertyDefinitionId.IsMajorVersion,
                QueryName = "cmis:isMajorVersion",
                Value = new bool[] { false }
            };
        }

        /// <summary>
        /// TRUE if the Document object is the latest version in its version series.
        /// Defaults to TRUE.
        /// </summary>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyIsLatestVersion()
        {
            return new CmisPropertyBoolean()
            {
                DisplayName = "Is Latest Version",
                LocalName = "IsLatestVersion",
                PropertyDefinitionId = CmisPropertyDefinitionId.IsLatestVersion,
                QueryName = "cmis:isLatestVersion",
                Value = new bool[] { true }
            };
        }

        /// <summary>
        /// TRUE if there currently exists a PWC for this version series.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisProperty CreateCmisPropertyIsVersionSeriesCheckedOut(bool[] value)
        {
            return new CmisPropertyBoolean()
            {
                DisplayName = "Is Version Series Checked Out",
                LocalName = "IsVersionSeriesCheckedOut",
                PropertyDefinitionId = CmisPropertyDefinitionId.IsVersionSeriesCheckedOut,
                QueryName = CmisPropertyDefinitionId.IsVersionSeriesCheckedOut,
                Value = value
            };
        }

        /// <summary>
        /// Date and time when the object was last modified.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyDateTime CreateCmisPropertyLastModificationDate(DateTime[] value)
        {
            return new CmisPropertyDateTime()
            {
                DisplayName = "Last Modification Date",
                LocalName = "LastModificationDate",
                PropertyDefinitionId = CmisPropertyDefinitionId.LastModificationDate,
                QueryName = CmisPropertyDefinitionId.LastModificationDate,
                Value = value
            };
        }


        /// <summary>
        /// Id of the parent folder of the object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CmisPropertyId CreateCmisPropertyParentId(string[] value)
        {
            return new CmisPropertyId()
            {
                DisplayName = "Parent Id",
                LocalName = "ParentId",
                PropertyDefinitionId = CmisPropertyDefinitionId.ParentId,
                QueryName = CmisPropertyDefinitionId.ParentId,
                Value = value
            };
        }

        #endregion

        public static CmisProperty CreateProperty(string propertyName, object o)
        {
            CmisProperty cmisProperty = GetCmisProperty(o);
            cmisProperty.DisplayName = propertyName;
            cmisProperty.LocalName = propertyName;
            cmisProperty.QueryName = propertyName;
            cmisProperty.PropertyDefinitionId = propertyName;
            return cmisProperty;
        }
        private static CmisProperty GetCmisProperty(object o)
        {
            if (o == null)
            {
                return new CmisPropertyString() { Value = new string[0] };
            }
            if (o is int)
            {
                return new CmisPropertyInteger()
                {
                    Value = new int[] { (int)o }
                };
            }
            if (o is decimal)
            {
                return new CmisPropertyDecimal()
                {
                    Value = new decimal[] { (decimal)o }
                };
            }
            if (o is bool)
            {
                return new CmisPropertyBoolean()
                {
                    Value = new bool[] { (bool)o }
                };
            }
            if (o is DateTime)
            {
                return new CmisPropertyDateTime()
                {
                    Value = new DateTime[] { (DateTime)o }
                };
            }
            return new CmisPropertyString()
            {
                Value = new string[] { o.ToString() }
            };
        }

        public static object GetPropertyValue(CmisProperty property)
        {
            if (property is CmisPropertyBoolean)
            {
                var boolProperty = ((CmisPropertyBoolean)property);
                return boolProperty.SingleValue.HasValue ? default(bool) : boolProperty.SingleValue.Value;
            }
            if (property is CmisPropertyDateTime)
            {
                var dateTimeProperty = ((CmisPropertyDateTime)property);
                return dateTimeProperty.SingleValue;
            }
            if (property is CmisPropertyDecimal)
            {
                var decimalProperty = ((CmisPropertyDecimal)property);
                return decimalProperty.SingleValue.HasValue ? default(decimal) : decimalProperty.SingleValue.Value;
            }
            if (property is CmisPropertyHtml)
            {
                var htmlProperty = ((CmisPropertyHtml)property);
                return htmlProperty.SingleValue;
            }
            if (property is CmisPropertyInteger)
            {
                var intProperty = ((CmisPropertyInteger)property);
                return intProperty.SingleValue.HasValue ? default(int) : intProperty.SingleValue.Value;
            }
            return property.Value;
        }

        public static NameValueCollection ToNameValueCollection(this CmisProperties properties)
        {
            return properties.Items.ToDictionary(it => it.PropertyDefinitionId, it => it.Value).ToNameValueCollection();
        }
    }
}
