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
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kooboo.CMS.Sites.Providers.SqlServer.HtmlBlockProvider
{
    [Table("HtmlBlocks")]
    public class HtmlBlockEntity
    {
        public HtmlBlockEntity() { }
        public HtmlBlockEntity(string siteName, string name)
        {
            this.SiteName = siteName;
            this.Name = name;
        }
        public HtmlBlockEntity(Kooboo.CMS.Sites.Models.HtmlBlock htmlBlock)
            : this(htmlBlock.Site.FullName, htmlBlock.Name)
        {
            this.Body = htmlBlock.Body;
        }
        [Key, Column(Order = 0)]
        public string SiteName
        {
            get;
            set;
        }
        [Key, Column(Order = 1)]
        public string Name
        {
            get;
            set;
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
