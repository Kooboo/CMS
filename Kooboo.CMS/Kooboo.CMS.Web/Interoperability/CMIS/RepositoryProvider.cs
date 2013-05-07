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
using NCMIS.Provider;
using NCMIS.Produce;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using NCMIS.ObjectModel.MetaData;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class RepositoryProvider : RepositoryProviderBase
    {
        public override NCMIS.Produce.RepositoryEntry[] GetRepositories()
        {
            return ServiceFactory.RepositoryManager.All().Select(it => new RepositoryEntry
                 {
                     Id = it.Name,
                     Name = it.Name
                 }).ToArray();
        }

        public override NCMIS.Produce.RepositoryEntry GetRepository(string repositoryId)
        {
            return GetRepositories().Where(it => string.Compare(repositoryId, it.Id, true) == 0).FirstOrDefault();
        }

        public override NCMIS.Produce.RepositoryInfo GetRepositoryInfo(string repositoryId)
        {
            var repositoryEntry = GetRepository(repositoryId);
            if (repositoryEntry != null)
            {
                Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId).AsActual();
                return new RepositoryInfo()
                {
                    RepositoryId = repository.Name,
                    RepositoryName = repository.Name,
                    RepositoryDescription = repository.DisplayName,
                    VendorName = "Kooboo CMS",
                    ProductName = "Kooboo CMS",
                    ProductVersion = "1.0",
                    RootFolderId = CmisFolderHelper.RootFolderName,
                    LatestChangeLogToken = DateTime.UtcNow.ToString(),
                    // Define repository capabilities (see section 2.1.1.1 in CMIS spec)
                    Capabilities = new RepositoryCapabilities()
                    {
                        Acl = CapabilityAcl.Manage,
                        AllVersionsSearchable = false,
                        Changes = CapabilityChanges.All,
                        ContentStreamUpdatability = CapabilityContentStreamUpdates.Anytime,
                        GetDescendants = false,
                        GetFolderTree = false,
                        Multifiling = false,
                        PwcSearchable = true,
                        PwcUpdatable = true,
                        Query = CapabilityQuery.BothCombined,
                        Renditions = CapabilityRendition.Read,
                        Unfiling = false,
                        VersionSpecificFiling = false,
                        Join = CapabilityJoin.InnerAndOuter
                    },
                    CmisVersionSupported = 1.0m
                };
            }
            return null;
        }

        public override NCMIS.ObjectModel.MetaData.TypeDefinitionList GetTypeChildren(string repositoryId, string typeId, bool includePropertyDefinitions, int? maxItems, int skipCount)
        {
            var list = new NCMIS.ObjectModel.MetaData.TypeDefinitionList() { Types = new TypeDefinition[0] };

            if (string.IsNullOrEmpty(typeId))
            {
                Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
                var schemas = ServiceFactory.SchemaManager.All(repository, "").Select(it => new TypeDocumentDefinition()
                {
                    Versionable = false,
                    ContentStreamAllowed = NCMIS.ObjectModel.MetaData.ContentStreamAllowed.Allowed,

                    BaseId = BaseObjectTypeId.CmisDocument,
                    ControllableAcl = false,
                    ControllablePolicy = false,
                    Creatable = true,
                    Description = it.Name,
                    DisplayName = it.Name,
                    Fileable = false,
                    FulltextIndexed = false,
                    Id = it.Name,
                    IncludedInSupertypeQuery = false,
                    LocalName = it.Name,
                    LocalNamespace = typeof(Schema).ToString(),
                    ParentId = "",
                    Queryable = true,
                    QueryName = "Schema",
                    PropertyDefinitions = includePropertyDefinitions ? null : it.AsActual().Columns.Select(c => ColumnDefinition(c)).ToArray()
                }).Skip(skipCount);
                var count = schemas.Count();
                var take = maxItems.HasValue ? maxItems.Value : count;
                list.NumItems = count.ToString();
                list.HasMoreItems = count > count + take;
                list.Types = schemas.Take(take).ToArray();
            }
            return list;
        }
        private PropertyDefinition ColumnDefinition(Column column)
        {
            switch (column.DataType)
            {

                case Kooboo.Form.DataType.Int:
                    return new PropertyIntegerDefinition()
                    {
                        Cardinality = Cardinality.Multi,
                        Choice = column.SelectionItems == null ? new CmisChoiceInteger[0] : column.SelectionItems.Select(it => new CmisChoiceInteger() { DisplayName = it.Text, Value = new int[] { string.IsNullOrEmpty(it.Value) ? 0 : int.Parse(it.Value) } }).ToArray(),
                        DefaultValue = new NCMIS.ObjectModel.CmisPropertyInteger() { Value = new int[] { string.IsNullOrEmpty(column.DefaultValue) ? 0 : int.Parse(column.DefaultValue) }, DisplayName = column.Name, LocalName = column.Name },
                        Description = "",
                        DisplayName = column.Name,
                        Id = column.Name,
                        Inherited = false,
                        LocalName = column.Name,
                        LocalNamespace = typeof(Column).ToString(),
                        OpenChoice = column.SelectionItems.Count() == 0,
                        Orderable = true,
                        PropertyType = CmisPropertyType.String,
                        Queryable = false,
                        Required = !column.AllowNull,
                        Updatability = Updatability.ReadWrite
                    };
                case Kooboo.Form.DataType.Decimal:
                    return new PropertyDecimalDefinition()
                    {
                        Cardinality = Cardinality.Multi,
                        Choice = column.SelectionItems == null ? null : column.SelectionItems.Select(it => new CmisChoiceDecimal() { DisplayName = it.Text, Value = new decimal[] { string.IsNullOrEmpty(it.Value) ? 0 : decimal.Parse(it.Value) } }).ToArray(),
                        DefaultValue = new NCMIS.ObjectModel.CmisPropertyDecimal() { Value = new decimal[] { string.IsNullOrEmpty(column.DefaultValue) ? 0 : decimal.Parse(column.DefaultValue) }, DisplayName = column.Name, LocalName = column.Name },
                        Description = "",
                        DisplayName = column.Name,
                        Id = column.Name,
                        Inherited = false,
                        LocalName = column.Name,
                        LocalNamespace = typeof(Column).ToString(),
                        OpenChoice = column.SelectionItems.Count() == 0,
                        Orderable = true,
                        PropertyType = CmisPropertyType.Decimal,
                        Queryable = false,
                        Required = !column.AllowNull,
                        Updatability = Updatability.ReadWrite
                    };
                case Kooboo.Form.DataType.DateTime:
                    return new PropertyDateTimeDefinition()
                    {
                        Cardinality = Cardinality.Multi,
                        Choice = column.SelectionItems == null ? null : column.SelectionItems.Select(it => new CmisChoiceDateTime() { DisplayName = it.Text, Value = new DateTime[] { string.IsNullOrEmpty(it.Value) ? DateTime.MinValue : DateTime.Parse(it.Value) } }).ToArray(),
                        DefaultValue = new NCMIS.ObjectModel.CmisPropertyDateTime() { Value = new DateTime[] { string.IsNullOrEmpty(column.DefaultValue) ? DateTime.MinValue : DateTime.Parse(column.DefaultValue) }, DisplayName = column.Name, LocalName = column.Name },
                        Description = "",
                        DisplayName = column.Name,
                        Id = column.Name,
                        Inherited = false,
                        LocalName = column.Name,
                        LocalNamespace = typeof(Column).ToString(),
                        OpenChoice = column.SelectionItems.Count() == 0,
                        Orderable = true,
                        PropertyType = CmisPropertyType.Decimal,
                        Queryable = false,
                        Required = !column.AllowNull,
                        Updatability = Updatability.ReadWrite
                    };
                case Kooboo.Form.DataType.Bool:
                    return new PropertyBooleanDefinition()
                    {
                        Cardinality = Cardinality.Multi,
                        choice = column.SelectionItems == null ? null : column.SelectionItems.Select(it => new CmisChoiceBoolean() { DisplayName = it.Text, Value = new bool[] { string.IsNullOrEmpty(it.Value) ? false : bool.Parse(it.Value) } }).ToArray(),
                        DefaultValue = new NCMIS.ObjectModel.CmisPropertyBoolean() { Value = new Boolean[] { string.IsNullOrEmpty(column.DefaultValue) ? false : bool.Parse(column.DefaultValue) }, DisplayName = column.Name, LocalName = column.Name },
                        Description = "",
                        DisplayName = column.Name,
                        Id = column.Name,
                        Inherited = false,
                        LocalName = column.Name,
                        LocalNamespace = typeof(Column).ToString(),
                        OpenChoice = column.SelectionItems.Count() == 0,
                        Orderable = true,
                        PropertyType = CmisPropertyType.Decimal,
                        Queryable = false,
                        Required = !column.AllowNull,
                        Updatability = Updatability.ReadWrite
                    };

                case Kooboo.Form.DataType.String:
                default:
                    return new PropertyStringDefinition()
                    {
                        Cardinality = Cardinality.Multi,
                        Choice = column.SelectionItems == null ? null : column.SelectionItems.Select(it => new CmisChoiceString() { DisplayName = it.Text, Value = new string[] { it.Value } }).ToArray(),
                        DefaultValue = new NCMIS.ObjectModel.CmisPropertyString() { Value = new string[] { column.DefaultValue }, DisplayName = column.Name, LocalName = column.Name },
                        Description = "",
                        DisplayName = column.Name,
                        Id = column.Name,
                        Inherited = false,
                        LocalName = column.Name,
                        LocalNamespace = typeof(Column).ToString(),
                        MaxLength = column.Length.ToString(),
                        OpenChoice = column.SelectionItems == null || column.SelectionItems.Count() == 0,
                        Orderable = true,
                        PropertyType = CmisPropertyType.String,
                        Queryable = false,
                        Required = !column.AllowNull,
                        Updatability = Updatability.ReadWrite
                    };
            }
        }

        public override NCMIS.ObjectModel.MetaData.TypeDefinition GetTypeDefinition(string repositoryId, string typeId)
        {
            Kooboo.CMS.Content.Models.Repository repository = new Models.Repository(repositoryId);
            var schema = ServiceFactory.SchemaManager.All(repository, "").Where(it => string.Compare(it.Name, typeId, true) == 0).FirstOrDefault();
            if (schema != null)
            {
                return new TypeDocumentDefinition()
                {
                    BaseId = BaseObjectTypeId.CmisDocument,
                    ControllableAcl = false,
                    ControllablePolicy = false,
                    Creatable = true,
                    Description = schema.Name,
                    DisplayName = schema.Name,
                    Fileable = false,
                    FulltextIndexed = false,
                    Id = schema.Name,
                    IncludedInSupertypeQuery = false,
                    LocalName = schema.Name,
                    LocalNamespace = typeof(Schema).ToString(),
                    ParentId = "",
                    Queryable = true,
                    QueryName = "Schema",
                    PropertyDefinitions = schema.AsActual().Columns.Select(c => ColumnDefinition(c)).ToArray()
                };
            }
            return null;
        }

        public override NCMIS.ObjectModel.MetaData.TypeContainer[] GetTypeDescendants(string repositoryId, string typeId, int? depth, bool includePropertyDefinitions)
        {
            return new TypeContainer[] { new TypeContainer() { Type = GetTypeDefinition(repositoryId, typeId) } };
        }
    }
}
