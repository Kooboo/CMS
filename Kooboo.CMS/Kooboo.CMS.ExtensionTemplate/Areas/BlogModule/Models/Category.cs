
using Kooboo.CMS.Common.Persistence.Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models
{
    public class Category : IEntity, ISiteEntity
    {
        public Category()
        {
            this.CreateionDate = DateTime.Now;
        }
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string SiteName { get; set; }
        public virtual ICollection<Blog> Blogs { get; set; }

        public DateTime CreateionDate { get; set; }
    }
}
