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
using Microsoft.WindowsAzure.StorageClient;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.Misc;

namespace Kooboo.CMS.Sites.Providers.AzureTable.UserProvider
{
    public class SiteUserEntity : TableServiceEntity
    {
        public SiteUserEntity()
        {

        }
        public SiteUserEntity(string siteName, string userName) : base(siteName, userName) { }
        public string SiteName
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }

        public string UserName
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        public string ObjectXml { get; set; }
    }
    public static class SiteUserHelper
    {
        public static SiteUserEntity ToEntity(this Kooboo.CMS.Sites.Models.User user, SiteUserEntity entity = null)
        {
            if (entity == null)
            {
                entity = new SiteUserEntity();
            }
            entity.SiteName = user.Site.Name;
            entity.UserName = user.UserName;

            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(user);

            return entity;
        }
        public static Kooboo.CMS.Sites.Models.User ToUser(this SiteUserEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            var dummy = new Kooboo.CMS.Sites.Models.User() { Site = new Site(entity.SiteName), UserName = entity.UserName };

            var user = DataContractSerializationHelper.DeserializeFromXml<Kooboo.CMS.Sites.Models.User>(entity.ObjectXml);

            ((IPersistable)user).Init(dummy);

            return user;

        }
    }
}
