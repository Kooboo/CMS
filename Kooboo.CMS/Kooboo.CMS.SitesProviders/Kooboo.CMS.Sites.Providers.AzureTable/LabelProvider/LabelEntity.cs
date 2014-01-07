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
using Kooboo.Globalization;

namespace Kooboo.CMS.Sites.Providers.AzureTable.LabelProvider
{
    public class CategoryEntity : TableServiceEntity
    {
        public CategoryEntity() { }
        public CategoryEntity(string siteName, string name)
        {
            this.SiteName = siteName;
            this.CategoryName = name;
        }
        public string SiteName { get { return this.PartitionKey; } set { this.PartitionKey = value; } }
        public string CategoryName { get { return this.RowKey; } set { this.RowKey = value; } }

        public ElementCategory ToElementCategory()
        {
            return new ElementCategory()
            {
                Category = this.CategoryName
            };
        }
    }
    public class LabelEntity : TableServiceEntity
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
            this.Category = cagegory;

            this.RowKey = string.Format("Key:{0}-Category:{1}", Name, Category);
        }
        public string SiteName { get { return this.PartitionKey; } set { this.PartitionKey = value; } }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Category { get; set; }

        public Element ToElement()
        {
            return new Element()
            {
                Category = Category,
                Name = Name,
                Value = Value
            };
        }
    }
}
