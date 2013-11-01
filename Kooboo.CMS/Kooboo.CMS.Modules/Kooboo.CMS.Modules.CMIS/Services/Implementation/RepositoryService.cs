#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.CMIS.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services.Implementation
{
    //[Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IRepositoryService))]
    public partial class Service// : IRepositoryService
    {
        cmisTypeDefinitionType[] SupportedBaseObjectTypes = new cmisTypeDefinitionType[]
        {
            new cmisTypeDefinitionType(){
                baseId = enumBaseObjectTypeIds.cmisdocument,
                controllableACL = false,
                controllablePolicy = false,
                creatable = true,
                description = null,
                displayName ="cmis:document",
                fileable = true,
                fulltextIndexed = false,
                id = "cmis:document",
                includedInSupertypeQuery = false,
                localName = "cmis:document",
                localNamespace = null,
                parentId = null,
                queryable = true,
                queryName = null,
                //ContentStreamAllowed = ContentStreamAllowed.NotAllowed
            }
        };
        #region .ctor
        //RepositoryManager _repositoryManager;
        //SchemaManager _schemaManager;
        //public RepositoryService(RepositoryManager repositoryManager, SchemaManager schemaManager)
        //{
        //    this._repositoryManager = repositoryManager;
        //    this._schemaManager = schemaManager;
        //}
        #endregion

        #region GetRepositories
        public getRepositoriesResponse GetRepositories(getRepositoriesRequest request)
        {

            var sites = _siteManager.All();

            return new getRepositoriesResponse(sites.Select(it => new cmisRepositoryEntryType()
            {
                repositoryId = it.FullName,
                repositoryName = string.IsNullOrEmpty(it.DisplayName) ? it.FriendlyName : it.DisplayName
            }).ToArray());
        }
        #endregion

        #region GetRepositoryInfo
        private cmisRepositoryInfoType ToRepositoryInfo(Kooboo.CMS.Sites.Models.Site site)
        {
            var repositoryInfo = new cmisRepositoryInfoType()
            {
                repositoryId = site.FullName,
                repositoryName = string.IsNullOrEmpty(site.DisplayName) ? site.Name : site.DisplayName,
                vendorName = "Kooboo",
                repositoryDescription = "",
                productName = "Kooboo CMS",
                productVersion = this.GetType().Assembly.GetName().Version.ToString(),
                rootFolderId = "/",
                latestChangeLogToken = "",
                capabilities = GetRepositoryCapabilities(site),
                aclCapability = null, //todo: Not very clear for acl now.
                cmisVersionSupported = "1.1",
                thinClientURI = null,
                changesIncomplete = false,
                changesOnType = new[] { enumBaseObjectTypeIds.cmisdocument },
                principalAnonymous = null,
                principalAnyone = null
            };
            return repositoryInfo;
        }
        private cmisRepositoryCapabilitiesType GetRepositoryCapabilities(Kooboo.CMS.Sites.Models.Site site)
        {
            return new cmisRepositoryCapabilitiesType()
            {
                capabilityGetDescendants = true,
                capabilityGetFolderTree = true,
                capabilityContentStreamUpdatability = enumCapabilityContentStreamUpdates.anytime,
                capabilityChanges = enumCapabilityChanges.properties,
                capabilityRenditions = enumCapabilityRendition.none,
                capabilityMultifiling = false,
                capabilityUnfiling = false,
                capabilityVersionSpecificFiling = false,
                capabilityPWCUpdatable = false,
                capabilityPWCSearchable = false,
                capabilityAllVersionsSearchable = false,
                capabilityQuery = enumCapabilityQuery.metadataonly,
                capabilityJoin = enumCapabilityJoin.none,
                capabilityACL = enumCapabilityACL.none
            };
        }
        public getRepositoryInfoResponse GetRepositoryInfo(getRepositoryInfoRequest request)
        {
            var site = ModelHelper.GetSite(request.repositoryId);
            if (site != null)
            {
                var repositoryInfo = ToRepositoryInfo(site);
                return new getRepositoryInfoResponse(repositoryInfo);
            }
            else
            {
                throw new FaultException<cmisFaultType>(ModelHelper.CreateFault(enumServiceException.objectNotFound));
            }
        }

        #endregion

        #region GetTypeChildren
        public getTypeChildrenResponse GetTypeChildren(getTypeChildrenRequest request)
        {
            if (string.IsNullOrEmpty(request.typeId))
            {
                var types = SupportedBaseObjectTypes;
                cmisTypeDefinitionListType list = new cmisTypeDefinitionListType();
                list.numItems = types.Count().ToString();
                if (!string.IsNullOrEmpty(request.maxItems))
                {
                    var maxItems = request.maxItems.As<int>();
                    if (SupportedBaseObjectTypes.Length > maxItems)
                    {
                        list.hasMoreItems = true;
                    }
                    types = types.Take(maxItems).ToArray();
                }
                list.types = SupportedBaseObjectTypes;
                return new getTypeChildrenResponse(list);
            }
            else if (request.typeId.ToLower() == "cmis:document")
            {
                var repository = ModelHelper.GetRepository(request.repositoryId);
                cmisTypeDefinitionListType list = new cmisTypeDefinitionListType();
                var schemas = _schemaManager.All(repository, "");
                list.numItems = schemas.Count().ToString();
                var skipCount = request.skipCount.As<int>();
                schemas = schemas.Skip(skipCount);
                if (!string.IsNullOrEmpty(request.maxItems))
                {
                    var maxItems = request.maxItems.As<int>();
                    if (schemas.Count() > maxItems)
                    {
                        list.hasMoreItems = true;
                    }
                    schemas = schemas.Take(maxItems);
                }

                list.types = schemas.Select(it => ToTypeDefinition(it, request.includePropertyDefinitions != null ? request.includePropertyDefinitions.Value : false)).ToArray();

                return new getTypeChildrenResponse(list);
            }
            else
            {
                throw new FaultException<cmisFaultType>(ModelHelper.CreateFault(enumServiceException.notSupported, "Kooboo CMS does not support the object type hierarchy.".Localize()));
            }
        }
        private cmisTypeDefinitionType ToTypeDefinition(Kooboo.CMS.Content.Models.Schema schema, bool includePropertyDefinitions)
        {
            cmisTypeDefinitionType typeDefinition = new cmisTypeDocumentDefinitionType();

            //typeDefinition.baseId = BaseObjectTypeId.CmisDocument;
            typeDefinition.controllableACL = false;
            typeDefinition.controllablePolicy = false;
            typeDefinition.creatable = true;
            typeDefinition.description = null;
            typeDefinition.displayName = schema.Name;
            typeDefinition.fileable = true;
            typeDefinition.fulltextIndexed = false;
            typeDefinition.id = schema.Name;
            typeDefinition.includedInSupertypeQuery = false;
            typeDefinition.localName = schema.Repository.Name;
            typeDefinition.localNamespace = null;
            typeDefinition.parentId = null;
            typeDefinition.queryable = true;
            typeDefinition.queryName = schema.Name;
            //typeDefinition.ContentStreamAllowed = ContentStreamAllowed.Allowed;
            if (includePropertyDefinitions)
            {
                typeDefinition.Items = DefaultDefinitionTypes(schema).Concat(schema.AllColumns.Select(it => ToPropertyDefinition(schema, it)).Where(it => it != null)).ToArray();
            }

            return typeDefinition;
        }
        private IEnumerable<cmisPropertyDefinitionType> DefaultDefinitionTypes(Kooboo.CMS.Content.Models.Schema schema)
        {
            yield return new cmisPropertyStringDefinitionType()
            {
                id = CmisPropertyDefinitionId.BaseTypeId,
                localName = CmisPropertyDefinitionId.BaseTypeId,
                propertyType = enumPropertyType.@string,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };
            yield return new cmisPropertyStringDefinitionType()
            {
                id = CmisPropertyDefinitionId.ObjectTypeId,
                localName = "SchemaName",
                propertyType = enumPropertyType.@string,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };

            yield return new cmisPropertyStringDefinitionType()
            {
                id = CmisPropertyDefinitionId.CreatedBy,
                localName = "UserId",
                propertyType = enumPropertyType.@string,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };
            yield return new cmisPropertyDateTimeDefinitionType()
            {
                id = CmisPropertyDefinitionId.CreationDate,
                localName = "UtcCreationDate",
                propertyType = enumPropertyType.datetime,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };
            yield return new cmisPropertyDateTimeDefinitionType()
            {
                id = CmisPropertyDefinitionId.LastModificationDate,
                localName = "UtcLastModificationDate",
                propertyType = enumPropertyType.datetime,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };
            yield return new cmisPropertyStringDefinitionType()
            {
                id = CmisPropertyDefinitionId.Name,
                localName = "UserKey",
                propertyType = enumPropertyType.@string,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };
            yield return new cmisPropertyStringDefinitionType()
            {
                id = CmisPropertyDefinitionId.ObjectId,
                localName = "UUID",
                propertyType = enumPropertyType.@string,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };


            yield return new cmisPropertyStringDefinitionType()
            {
                id = CmisPropertyDefinitionId.ParentId,
                localName = "parentUUID",
                propertyType = enumPropertyType.@string,
                cardinality = enumCardinality.single,
                inherited = false,
                orderable = true,
                queryable = true,
                required = true,
                updatability = enumUpdatability.readwrite
            };

        }
        private cmisPropertyDefinitionType ToPropertyDefinition(Kooboo.CMS.Content.Models.Schema schema, Kooboo.CMS.Content.Models.Column column)
        {
            cmisPropertyDefinitionType def = null;

            #region Create PropertyDefinition object

            switch (column.DataType)
            {
                case Kooboo.CMS.Common.DataType.Int:
                    var intDef = new cmisPropertyIntegerDefinitionType();

                    if (!string.IsNullOrEmpty(column.DefaultValue))
                    {
                        int defaultValue;
                        if (int.TryParse(column.DefaultValue, out defaultValue))
                        {
                            intDef.defaultValue = new cmisPropertyInteger()
                            {
                                value = new[] { defaultValue.ToString() }
                            };
                        }
                    }
                    if (column.SelectionItems != null)
                    {
                        int value;
                        intDef.choice = column.SelectionItems.Where(it => int.TryParse(it.Value, out value))
                            .Select(it => new { it.Text, Value = int.Parse(it.Value) }).Select(it => new cmisChoiceInteger()
                            {
                                displayName = it.Text,
                                value = new[] { it.Value.ToString() }
                            }).ToArray();

                        intDef.openChoice = intDef.choice.Length > 0;
                    }
                    def = intDef;
                    def.propertyType = enumPropertyType.integer;
                    break;
                case Kooboo.CMS.Common.DataType.Decimal:
                    var decDef = new cmisPropertyDecimalDefinitionType();

                    if (!string.IsNullOrEmpty(column.DefaultValue))
                    {
                        decimal defaultValue;
                        if (decimal.TryParse(column.DefaultValue, out defaultValue))
                        {
                            decDef.defaultValue = new cmisPropertyDecimal()
                            {
                                value = new[] { defaultValue }
                            };
                        }
                    }
                    if (column.SelectionItems != null)
                    {
                        decimal value;
                        decDef.choice = column.SelectionItems.Where(it => decimal.TryParse(it.Value, out value))
                            .Select(it => new { it.Text, Value = decimal.Parse(it.Value) }).Select(it => new cmisChoiceDecimal()
                            {
                                displayName = it.Text,
                                value = new decimal[] { it.Value }
                            }).ToArray();
                        decDef.openChoice = decDef.choice.Length > 0;
                    }
                    def = decDef;
                    def.propertyType = enumPropertyType.@decimal;
                    break;
                case Kooboo.CMS.Common.DataType.DateTime:

                    var dateTimeDef = new cmisPropertyDateTimeDefinitionType();

                    if (!string.IsNullOrEmpty(column.DefaultValue))
                    {
                        DateTime defaultValue;
                        if (DateTime.TryParse(column.DefaultValue, out defaultValue))
                        {
                            dateTimeDef.defaultValue = new cmisPropertyDateTime()
                            {
                                value = new DateTime[] { defaultValue }
                            };
                        }
                    }
                    if (column.SelectionItems != null)
                    {
                        DateTime value;
                        dateTimeDef.choice = column.SelectionItems.Where(it => DateTime.TryParse(it.Value, out value))
                            .Select(it => new { it.Text, Value = DateTime.Parse(it.Value) }).Select(it => new cmisChoiceDateTime()
                            {
                                displayName = it.Text,
                                value = new DateTime[] { it.Value }
                            }).ToArray();
                        dateTimeDef.openChoice = dateTimeDef.choice.Length > 0;
                    }
                    def = dateTimeDef;
                    def.propertyType = enumPropertyType.datetime;
                    break;
                case Kooboo.CMS.Common.DataType.Bool:
                    var boolDef = new cmisPropertyBooleanDefinitionType();

                    if (!string.IsNullOrEmpty(column.DefaultValue))
                    {
                        bool defaultValue;
                        if (bool.TryParse(column.DefaultValue, out defaultValue))
                        {
                            boolDef.defaultValue = new cmisPropertyBoolean()
                            {
                                value = new bool[] { defaultValue }
                            };
                        }
                    }
                    if (column.SelectionItems != null)
                    {
                        bool value;
                        boolDef.choice = column.SelectionItems.Where(it => bool.TryParse(it.Value, out value))
                            .Select(it => new { it.Text, Value = bool.Parse(it.Value) }).Select(it => new cmisChoiceBoolean()
                            {
                                displayName = it.Text,
                                value = new bool[] { it.Value }
                            }).ToArray();

                        boolDef.openChoice = boolDef.choice.Length > 0;
                    }
                    def = boolDef;
                    def.propertyType = enumPropertyType.boolean;
                    break;
                case Kooboo.CMS.Common.DataType.String:
                default:
                    var strDef = new cmisPropertyStringDefinitionType();
                    strDef.maxLength = column.Length.ToString();
                    if (!string.IsNullOrEmpty(column.DefaultValue))
                    {
                        strDef.defaultValue = new cmisPropertyString()
                        {
                            value = new string[] { column.DefaultValue }
                        };
                    }
                    if (column.SelectionItems != null)
                    {
                        strDef.choice = column.SelectionItems.Select(it => new cmisChoiceString()
                        {
                            displayName = it.Text,
                            value = new string[] { it.Value }
                        }).ToArray();
                        strDef.openChoice = strDef.choice.Length > 0;
                    }
                    def = strDef;
                    def.propertyType = enumPropertyType.@string;
                    break;
            }
            #endregion

            if (def != null)
            {
                def.cardinality = enumCardinality.single;
                def.description = column.Tooltip;
                def.displayName = column.Label;
                def.id = column.Name;
                def.inherited = false;
                def.localName = column.Name;
                def.localNamespace = null;
                def.orderable = true;
                def.queryable = true;
                def.queryName = column.Name;
                def.required = !column.AllowNull;
                def.updatability = enumUpdatability.readwrite;
            }
            return def;
        }
        #endregion

        #region GetTypeDescendants
        public getTypeDescendantsResponse GetTypeDescendants(getTypeDescendantsRequest request)
        {
            throw new FaultException<cmisFaultType>(ModelHelper.CreateFault(enumServiceException.notSupported, "Kooboo CMS does not support the object type hierarchy.".Localize()));
        }
        #endregion

        #region GetTypeDefinition
        public getTypeDefinitionResponse GetTypeDefinition(getTypeDefinitionRequest request)
        {
            var repository = ModelHelper.GetRepository(request.repositoryId);
            var schema = ModelHelper.GetSchema(request.repositoryId, request.typeId);
            return new getTypeDefinitionResponse(ToTypeDefinition(schema, true));
        }
        #endregion
    }
}
