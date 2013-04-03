using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMIS.ObjectModel;
using Kooboo.CMS.Content.Models;
using Kooboo.Extensions;
using NCMIS.Produce;

namespace Kooboo.CMS.Content.Interoperability.CMIS
{
    public class ObjectConvertor
    {
        static Dictionary<Type, IObjectConvertor> convertors = new Dictionary<Type, IObjectConvertor>()
        {
            {typeof(TextFolder),new TextFolderConvertor()},
            {typeof(TextContent),new TextContentConvertor()}
        };
        public static CmisObject ToCmis<T>(T o, bool includeRelationships)
        {
            return convertors[o.GetType()].ToCmis(o, includeRelationships);
        }
        //public static T ToCms<T>(CmisObject cmisObject)
        //{
        //    return (T)(convertors[typeof(T)].ToCms(cmisObject));
        //}

        public static CmisObject EmptyCmisObject()
        {
            return new CmisObject()
            {
                Properties = new CmisProperties()
                {
                    Items = new CmisProperty[] {
                CmisPropertyHelper.CreateCmisPropertyAllowedChildObjectTypeIds(null),
                       CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:empty" }),
                      CmisPropertyHelper.CreateCmisPropertyCreatedBy(new string[] { "" }),
                     CmisPropertyHelper.CreateCmisPropertyCreationDate(new DateTime[] {DateTime.UtcNow}),
                     CmisPropertyHelper.CreateCmisPropertyLastModificationDate(new DateTime[] { DateTime.UtcNow}),
                    CmisPropertyHelper.CreateCmisPropertyLastModifiedBy(new string[] { ""}),
                    CmisPropertyHelper.CreateCmisPropertyName(new string[] { "" }),
                    CmisPropertyHelper.CreateCmisPropertyObjectId(new string[] { "" }),
                   CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:empty" }),
                   CmisPropertyHelper.CreateCmisPropertyObjectTypeId(new string[]{""})
            }
                }
            };
        }
    }

    public interface IObjectConvertor
    {
        CmisObject ToCmis(object o, bool includeRelationships);
        object ToCms(CmisObject cmisObject);
    }

    public class TextFolderConvertor : IObjectConvertor
    {
        #region IObjectConvertor Members

        public CmisObject ToCmis(object o, bool includeRelationships)
        {
            var textFolder = (TextFolder)o;
            CmisObject cmisObject = new CmisObject();
            cmisObject.Id = ObjectService.GetObjectId(textFolder);
            cmisObject.Properties = new CmisProperties()
            {
                Items = new CmisProperty[] {
                                      CmisPropertyHelper.CreateCmisPropertyAllowedChildObjectTypeIds(null),
                       CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:folder" }),
                      CmisPropertyHelper.CreateCmisPropertyCreatedBy(new string[] { textFolder.UserId }),
                     CmisPropertyHelper.CreateCmisPropertyCreationDate(new DateTime[] { textFolder.UtcCreationDate == DateTime.MinValue? DateTime.UtcNow:textFolder.UtcCreationDate }),
                     CmisPropertyHelper.CreateCmisPropertyLastModificationDate(new DateTime[] { DateTime.UtcNow}),
                    CmisPropertyHelper.CreateCmisPropertyLastModifiedBy(new string[] { textFolder.UserId }),
                    CmisPropertyHelper.CreateCmisPropertyName(new string[] { textFolder.Name }),
                    CmisPropertyHelper.CreateCmisPropertyObjectId(new string[] { cmisObject.Id }),
                   CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:folder" }),
                   CmisPropertyHelper.CreateCmisPropertyObjectTypeId(new string[]{textFolder.SchemaName}),
                   CmisPropertyHelper.CreateCmisPropertyParentId(new string[] { textFolder.Parent ==null?CmisFolderHelper.RootFolderName:textFolder.Parent.FullName}),
                   CmisPropertyHelper.CreateCmisPropertyPath(new string[] { string.Join("/",textFolder.NamePath.ToArray()) }),
                   new CmisPropertyString(){
                        DisplayName="Display Name",
                        LocalName = "DispalyName",
                        PropertyDefinitionId="DisplayName",
                        Value=new string[]{textFolder.DisplayName}
                   },
                  new CmisPropertyString(){
                        DisplayName="Schema Name",
                        LocalName = "SchemaName",
                        PropertyDefinitionId="SchemaName",
                        Value=new string[]{textFolder.SchemaName}
                   },
                   new CmisPropertyString(){
                        DisplayName="Category Folders",
                        LocalName = "CategoryFolders",
                        PropertyDefinitionId="CategoryFolders",
                        Value=new string[]{textFolder.CategoryFolders==null?"":string.Join(",",textFolder.CategoryFolders.ToArray())}
                   }
                }
            };
            return cmisObject;
        }

        public object ToCms(CmisObject cmisObject)
        {
            TextFolder textFolder = (TextFolder)CmisFolderHelper.Parse(null, cmisObject.Id);

            textFolder.DisplayName = cmisObject.GetProperty("DisplayName").Value;
            if (!string.IsNullOrEmpty(cmisObject.GetProperty("CategoryFolders").Value))
            {
                textFolder.CategoryFolders = cmisObject.GetProperty("CategoryFolders").Value.Split(',');
            }

            textFolder.UserId = cmisObject.Properties.CreatedBy.SingleValue;

            textFolder.SchemaName = cmisObject.GetProperty("SchemaName").Value;

            return textFolder;
        }

        #endregion
    }

    public class MediaFolderConvertor : IObjectConvertor
    {
        #region IObjectConvertor Members

        public CmisObject ToCmis(object o, bool includeRelationships)
        {
            var mediaFolder = (MediaFolder)o;
            CmisObject cmisObject = new CmisObject();
            cmisObject.Id = ObjectService.GetObjectId(mediaFolder);
            cmisObject.Properties = new CmisProperties()
            {
                Items = new CmisProperty[] {
                    CmisPropertyHelper.CreateCmisPropertyAllowedChildObjectTypeIds(null),
                    CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:folder" }),
                    CmisPropertyHelper.CreateCmisPropertyCreatedBy(new string[] { mediaFolder.UserId }),
                    CmisPropertyHelper.CreateCmisPropertyCreationDate(new DateTime[] { mediaFolder.UtcCreationDate == DateTime.MinValue? DateTime.UtcNow:mediaFolder.UtcCreationDate }),
                    CmisPropertyHelper.CreateCmisPropertyLastModificationDate(new DateTime[] { DateTime.UtcNow}),
                    CmisPropertyHelper.CreateCmisPropertyLastModifiedBy(new string[] { mediaFolder.UserId }),
                    CmisPropertyHelper.CreateCmisPropertyName(new string[] { mediaFolder.Name }),
                    CmisPropertyHelper.CreateCmisPropertyObjectId(new string[] { cmisObject.Id }),
                    CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new string[] { "cmis:folder" }),
                    CmisPropertyHelper.CreateCmisPropertyObjectTypeId(new string[]{ "" }),
                    CmisPropertyHelper.CreateCmisPropertyParentId(new string[] { mediaFolder.Parent ==null?CmisFolderHelper.RootFolderName:mediaFolder.Parent.FullName}),
                    CmisPropertyHelper.CreateCmisPropertyPath(new string[] { string.Join("/",mediaFolder.NamePath.ToArray()) }),
                    new CmisPropertyString(){
                        DisplayName="Display Name",
                        LocalName = "DispalyName",
                        PropertyDefinitionId="DisplayName",
                        Value=new string[]{mediaFolder.DisplayName}
                    },
                    new CmisPropertyString(){
                        DisplayName="Allowed Extensions",
                        LocalName = "AllowedExtensions",
                        PropertyDefinitionId="AllowedExtensions",
                        Value=mediaFolder.AllowedExtensions==null ? new string[0] : mediaFolder.AllowedExtensions.ToArray()
                    }                   
                }
            };
            return cmisObject;
        }

        public object ToCms(CmisObject cmisObject)
        {
            MediaFolder textFolder = (MediaFolder)CmisFolderHelper.Parse(null, cmisObject.Id);

            textFolder.DisplayName = cmisObject.GetProperty("DisplayName").Value;


            textFolder.UserId = cmisObject.Properties.CreatedBy.SingleValue;

            textFolder.AllowedExtensions = ((CmisPropertyString)cmisObject.GetProperty("AllowedExtensions")).Value;

            return textFolder;
        }

        #endregion
    }

    public class TextContentConvertor : IObjectConvertor
    {
        #region IObjectConvertor Members

        public CmisObject ToCmis(object o, bool includeRelationships)
        {
            CmisObject cmisObject = new CmisObject();

            TextContent content = (TextContent)o;

            cmisObject.Id = ObjectService.GetObjectId(content);

            cmisObject.Properties = new CmisProperties()
            {
                Items = content.Keys.Select(key => CmisPropertyHelper.CreateProperty(key, content[key])).ToArray()
            };

            cmisObject.Properties.Items = cmisObject.Properties.Items.Concat(new CmisProperty[] { 
                CmisPropertyHelper.CreateCmisPropertyCreatedBy(new []{content.UserId}),
                CmisPropertyHelper.CreateCmisPropertyCreationDate(new []{content.UtcCreationDate}),
                CmisPropertyHelper.CreateCmisPropertyLastModificationDate(new []{content.UtcLastModificationDate}),
                CmisPropertyHelper.CreateCmisPropertyName(new []{content.GetSummarize() }),
                CmisPropertyHelper.CreateCmisPropertyObjectId(new []{cmisObject.Id}),
                CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new []{"cmis:document"}),
                CmisPropertyHelper.CreateCmisPropertyObjectTypeId(new []{content.SchemaName}),
            }).ToArray();

            if (includeRelationships)
            {
                var categories = Services.ServiceFactory.TextContentManager.QueryCategories(content.GetRepository(), content.FolderName, content.UUID);
                cmisObject.Relationship = categories.Select(it => it.Contents).SelectMany(it => it.Select(c => ObjectConvertor.ToCmis(c, includeRelationships))).ToArray();
            }

            return cmisObject;
        }

        public object ToCms(CmisObject cmisObject)
        {
            TextContent content = new TextContent();

            content.UUID = cmisObject.Id;

            if (cmisObject.Properties != null)
            {
                foreach (var property in cmisObject.Properties.Items)
                {
                    content[property.LocalName] = CmisPropertyHelper.GetPropertyValue(property);
                }
            }

            return content;
        }

        #endregion
    }

    public class MediaContentConvertor : IObjectConvertor
    {
        #region IObjectConvertor Members

        public CmisObject ToCmis(object o, bool includeRelationships)
        {
            CmisObject cmisObject = new CmisObject();

            MediaContent content = (MediaContent)o;

            cmisObject.Id = ObjectService.GetObjectId(content);

            cmisObject.Properties = new CmisProperties()
            {
                Items = content.Keys.Select(key => CmisPropertyHelper.CreateProperty(key, content[key])).ToArray()
            };

            cmisObject.Properties.Items = cmisObject.Properties.Items.Concat(new CmisProperty[] { 
                CmisPropertyHelper.CreateCmisPropertyCreatedBy(new []{content.UserId}),
                CmisPropertyHelper.CreateCmisPropertyCreationDate(new []{content.UtcCreationDate}),
                CmisPropertyHelper.CreateCmisPropertyLastModificationDate(new []{content.UtcLastModificationDate}),
                CmisPropertyHelper.CreateCmisPropertyName(new []{content.UserKey }),
                CmisPropertyHelper.CreateCmisPropertyObjectId(new []{cmisObject.Id}),
                CmisPropertyHelper.CreateCmisPropertyBaseTypeId(new []{"cmis:document"}),
                CmisPropertyHelper.CreateCmisPropertyObjectTypeId(new []{content.FolderName}),
            }).ToArray();

            if (includeRelationships)
            {
                var categories = Services.ServiceFactory.TextContentManager.QueryCategories(content.GetRepository(), content.FolderName, content.UUID);
                cmisObject.Relationship = categories.Select(it => it.Contents).SelectMany(it => it.Select(c => ObjectConvertor.ToCmis(c, includeRelationships))).ToArray();
            }

            return cmisObject;
        }

        public object ToCms(CmisObject cmisObject)
        {
            TextContent content = new TextContent();

            content.UUID = cmisObject.Id;

            if (cmisObject.Properties != null)
            {
                foreach (var property in cmisObject.Properties.Items)
                {
                    content[property.LocalName] = CmisPropertyHelper.GetPropertyValue(property);
                }
            }

            return content;
        }

        #endregion
    }
}
