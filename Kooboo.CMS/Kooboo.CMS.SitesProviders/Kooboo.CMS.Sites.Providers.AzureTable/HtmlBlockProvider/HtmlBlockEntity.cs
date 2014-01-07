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
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Sites.Providers.AzureTable.HtmlBlockProvider
{
    public class HtmlBlockEntity : TableServiceEntity
    {
        public HtmlBlockEntity() { }
        public HtmlBlockEntity(string siteName, string name)
        {
            this.SiteName = siteName;
            this.Name = name;
        }
        public HtmlBlockEntity(Kooboo.CMS.Sites.Models.HtmlBlock htmlBlock)
            : base(htmlBlock.Site.FullName, htmlBlock.Name)
        {
            this.Body = htmlBlock.Body;
        }
        public string SiteName
        {
            get
            {
                return this.PartitionKey;
            }
            set
            {
                this.PartitionKey = value;
            }
        }
        public string Name
        {
            get { return this.RowKey; }
            set
            {
                this.RowKey = value;
            }
        }
        public string Body { get; set; }

        public Kooboo.CMS.Sites.Models.HtmlBlock ToHtmlBlock()
        {
            Kooboo.CMS.Sites.Models.HtmlBlock htmlBlock = new Kooboo.CMS.Sites.Models.HtmlBlock(new Site(this.SiteName), this.Name);
            htmlBlock.Body = this.Body;
            ((IPersistable)htmlBlock).Init(htmlBlock);
            return htmlBlock;
        }
    }
}
