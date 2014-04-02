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
using Kooboo.Globalization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.LabelProvider
{
    [Table("Kooboo_CMS_Sites_LabelCategories")]
    public class CategoryEntity
    {
        public CategoryEntity() { }
        public CategoryEntity(string siteName, string name)
        {
            this.SiteName = siteName;
            this.CategoryName = name;
        }
        [Key, Column(Order = 0)]
        public string SiteName { get; set; }
        [Key, Column(Order = 1)]
        public string CategoryName { get; set; }

        public ElementCategory ToElementCategory()
        {
            return new ElementCategory()
            {
                Category = this.CategoryName
            };
        }
    }
    [Table("Kooboo_CMS_Sites_Labels")]
    public class LabelEntity
    {
        public LabelEntity() { }
        public LabelEntity(string siteName, Element element)
            : this(siteName, element.Name, element.Value, element.Category)
        {
        }

        public LabelEntity(string siteName, string name, string value, string cagegory)
        {
            this.SiteName = siteName;
            this.Name = name;
            this.Value = value;
            this.Category = cagegory ?? "";
        }

        [Column(Order = 0)]
        public string SiteName { get; set; }

        [Column(Order = 1)]
        [StringLength(1024)]
        public string Name { get; set; }

        [Column(Order = 2)]
        public string Category { get; set; }

        public string Value { get; set; }

        [Key, StringLength(128)]
        public string UUID
        {
            get;
            set;
        }

        public DateTime? UtcCreationDate { get; set; }


        public DateTime? UtcLastestModificationDate { get; set; }

        [StringLength(128)]
        public string LastestEditor { get; set; }

        public Element ToElement()
        {
            return new Element()
            {
                Category = Category,
                Name = Name,
                Value = Value
            };
        }

        public Label ToLabel(Label dummy)
        {
            var label = new Label()
            {
                UUID = this.UUID,
                Name = this.Name,
                Category = this.Category,
                Value = this.Value,
                UtcCreationDate = this.UtcCreationDate,
                UtcLastestModificationDate = this.UtcLastestModificationDate,
                LastestEditor = this.LastestEditor
            };
            ((IPersistable)label).Init(dummy);

            return label;
        }
    }
}
