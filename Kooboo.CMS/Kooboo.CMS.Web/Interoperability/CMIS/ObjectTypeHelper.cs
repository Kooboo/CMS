using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMIS.ObjectModel.MetaData;
using NCMIS.Produce;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public static class ObjectTypeHelper
    {
        #region Object-type Attribute Definitions

        /// <summary>
        /// Define attributes for the Document Object-type (see section
        /// 2.1.4.3.2 Attribute values).
        /// </summary>
        /// <returns></returns>
        public static TypeDefinition CreateDocumentType()
        {
            return new TypeDocumentDefinition()
            {
                BaseId = BaseObjectTypeId.CmisDocument,
                ControllableAcl = false,
                ControllablePolicy = false,
                Creatable = true,
                Description = "Document Type",
                DisplayName = "Document",
                Fileable = true,
                FulltextIndexed = true,
                Id = "cmis:document",
                IncludedInSupertypeQuery = true,
                LocalName = "document",
                LocalNamespace = "NCMIS.ObjectModel.MetaData.TypeDocumentDefinition",
                ParentId = null,
                Queryable = true,
                QueryName = "cmis:folder"
            };
        }

        /// <summary>
        /// Define attributes for the Folder Object-type (see section
        /// 2.1.5.4.1 Attribute values).
        /// </summary>
        /// <returns></returns>
        public static TypeDefinition CreateFolderType()
        {
            return new TypeFolderDefinition()
            {
                BaseId = BaseObjectTypeId.CmisFolder,
                ControllableAcl = false,
                ControllablePolicy = false,
                Creatable = true,
                Description = "Folder Type",
                DisplayName = "Folder",
                Fileable = false,
                FulltextIndexed = true,
                Id = "cmis:folder",
                IncludedInSupertypeQuery = true,
                LocalName = "folder",
                LocalNamespace = "NCMIS.ObjectModel.MetaData.TypeFolderDefinition",
                ParentId = null,
                Queryable = true,
                QueryName = "cmis:folder"
            };
        }

        /// <summary>
        /// Define attributes for the Policy Object-type (see section
        /// 2.1.7.1.1 Attribute values).
        /// </summary>
        /// <returns></returns>
        public static TypeDefinition CreatePolicyType()
        {
            return new TypePolicyDefinition()
            {
                BaseId = BaseObjectTypeId.CmisPolicy,
                ControllableAcl = false,
                ControllablePolicy = false,
                Creatable = true,
                Description = "Policy Type",
                DisplayName = "Policy",
                Fileable = false,
                FulltextIndexed = false,
                Id = "cmis:policy",
                IncludedInSupertypeQuery = true,
                LocalName = "policy",
                LocalNamespace = "NCMIS.ObjectModel.MetaData.TypePolicyDefinition",
                ParentId = null,
                Queryable = true,
                QueryName = "cmis:policy"
            };
        }

        /// <summary>
        /// Define attributes for the Relationship Object-type (see section
        /// 2.1.6.1.2 Attribute values).
        /// </summary>
        /// <returns></returns>
        public static TypeDefinition CreateRelationshipType()
        {
            return new TypeRelationshipDefinition()
            {
                AllowedSourceTypes = new string[] { string.Empty },
                AllowedTargetTypes = new string[] { string.Empty },
                BaseId = BaseObjectTypeId.CmisRelationship,
                ControllableAcl = false,
                ControllablePolicy = false,
                Creatable = true,
                Description = "Relationship Type",
                DisplayName = "Relationship",
                Fileable = false,
                FulltextIndexed = false,
                Id = "cmis:relationship",
                IncludedInSupertypeQuery = true,
                LocalName = "relationship",
                LocalNamespace = "NCMIS.ObjectModel.MetaData.TypeRelationshipDefinition",
                ParentId = null,
                Queryable = true,
                QueryName = "cmis:relationship"
            };
        }

        /// <summary>
        /// Define attributes for the System Folder Object-type (not included
        /// in the CMIS spec).
        /// </summary>
        /// <returns></returns>
        public static TypeDefinition CreateSystemFolderType()
        {
            return new TypeFolderDefinition()
            {
                BaseId = BaseObjectTypeId.CmisFolder,
                ControllableAcl = false,
                ControllablePolicy = false,
                Creatable = true,
                Description = "System Folder Type",
                DisplayName = "System Folder",
                Fileable = false,
                FulltextIndexed = true,
                Id = "ncmis:systemfolder",
                IncludedInSupertypeQuery = true,
                LocalName = "folder",
                LocalNamespace = "NCMIS.ObjectModel.MetaData.TypeFolderDefinition",
                ParentId = "cmis:folder",
                Queryable = true,
                QueryName = "ncmis:systemfolder"
            };
        }

        #endregion
    }
}
