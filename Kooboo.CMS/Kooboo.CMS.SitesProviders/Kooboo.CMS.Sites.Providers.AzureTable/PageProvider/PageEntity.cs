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
using Microsoft.WindowsAzure.StorageClient;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.DataRule;
using System.Reflection;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.Misc;
namespace Kooboo.CMS.Sites.Providers.AzureTable.PageProvider
{
    public class PageEntity : TableServiceEntity
    {
        public PageEntity(string siteName, string fullName)
        {
            this.SiteName = siteName;
            this.FullName = fullName;
        }

        public PageEntity()
        {
        }

        public string SiteName
        {
            get
            {
                return PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }

        public string FullName
        {
            get
            {
                return this.RowKey;
            }
            set
            {
                this.RowKey = value;
            }
        }

        public string ParentPage { get; set; }

        public bool IsDefault { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class PageEntityHelper
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(PagePosition),
                typeof(ViewPosition),
                typeof(ModulePosition),
                typeof(HtmlPosition),
                typeof(ContentPosition),
                typeof(HtmlBlockPosition),
                typeof(DataRuleBase),
                typeof(FolderDataRule),
                typeof(SchemaDataRule),
                typeof(CategoryDataRule)
                };
        public static PageEntity ToPageEntity(this Page model)
        {
            PageEntity entity = new PageEntity(model.Site.Name, model.FullName);

            entity.IsDefault = model.IsDefault;

            if (model.Parent != null)
            {
                entity.ParentPage = model.Parent.FullName;
            }
            else
            {
                entity.ParentPage = "";
            }
            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);

            return entity;
        }
        public static Page ToPage(this PageEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dummy = new Page(new Site(entity.SiteName), entity.FullName);

            var page = DataContractSerializationHelper.DeserializeFromXml<Page>(entity.ObjectXml, KnownTypes);
            ((IPersistable)page).Init(dummy);

            return page;

        }
    }
}
