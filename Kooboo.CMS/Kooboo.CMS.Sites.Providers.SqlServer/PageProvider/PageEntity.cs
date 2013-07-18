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
using Kooboo.CMS.Sites.Models;
using Kooboo.Runtime.Serialization;
using Kooboo.CMS.Sites.DataRule;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kooboo.CMS.Sites.Providers.SqlServer.PageProvider
{
    public interface IPageEntity
    {
        string SiteName
        {
            get;
            set;
        }

        string FullName
        {
            get;
            set;
        }

        string ParentPage { get; set; }

        bool IsDefault { get; set; }

        string ObjectXml { get; set; }
    }
    public class PageEntity : IPageEntity
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
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }

        public string ParentPage { get; set; }

        public bool IsDefault { get; set; }

        public string ObjectXml { get; set; }

        /// <summary>
        /// 只用于缓存PageEntity时用
        /// </summary>
        [NotMapped]
        public Page PageObject { get; set; }
    }

    public class PageDraftEntity : IPageEntity
    {

        public string SiteName
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
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
        public static T ToPageEntity<T>(this Page model)
            where T : IPageEntity, new()
        {
            return ToPageEntity(model, new T());
        }
        public static T ToPageEntity<T>(this Page model, T entity)
            where T : IPageEntity
        {
            if (string.IsNullOrEmpty(entity.SiteName))
            {
                entity.SiteName = model.Site.FullName;
            }
            if (string.IsNullOrEmpty(entity.FullName))
            {
                entity.FullName = model.FullName;
            }

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
        public static Page ToPage(this IPageEntity entity)
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
