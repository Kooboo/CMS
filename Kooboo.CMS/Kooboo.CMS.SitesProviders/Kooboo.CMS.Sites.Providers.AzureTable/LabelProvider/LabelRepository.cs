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
using Kooboo.CMS.Sites.Models;
using Microsoft.WindowsAzure.StorageClient;

namespace Kooboo.CMS.Sites.Providers.AzureTable.LabelProvider
{
    public class LabelRepository : IElementRepository
    {
        static string LabelCategoryTable = "LabelCategories";
        static string LabelTable = "Labels";
        public string SiteName { get; set; }
        public LabelRepository(string siteName)
        {
            this.SiteName = siteName;
        }
        static LabelRepository()
        {
            CloudTableClient tableClient = CloudTableHelper.GetTableClient();

            tableClient.CreateTableIfNotExist<CategoryEntity>(LabelCategoryTable);
            tableClient.CreateTableIfNotExist<LabelEntity>(LabelTable);

        }
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return new System.Globalization.CultureInfo[0].AsQueryable();
        }

        public IQueryable<Element> Elements()
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<LabelEntity>(LabelTable)
                   .Where(it => it.PartitionKey == SiteName)
                   .ToArray()
                   .Select(it => it.ToElement())
                   .AsQueryable();
        }

        public Element Get(string name, string category, string culture)
        {
            var entity = new LabelEntity(SiteName, name, "", category);
            return CloudTableHelper.GetTableServiceContext().CreateQuery<LabelEntity>(LabelTable)
                   .Where(it => it.PartitionKey == entity.SiteName && it.RowKey == entity.RowKey)
                   .ToArray()
                   .Select(it => it.ToElement())
                   .FirstOrDefault();
        }

        public IQueryable<ElementCategory> Categories()
        {
            return CloudTableHelper.GetTableServiceContext().CreateQuery<CategoryEntity>(LabelCategoryTable)
                  .Where(it => it.PartitionKey == SiteName)
                  .ToArray()
                  .Select(it => it.ToElementCategory())
                  .AsQueryable();
        }

        public bool Add(Element element)
        {
            InsertOrUpdateLabel(element, element);
            return true;
        }
        private void InsertOrUpdateLabel(Element @new, Element old)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = new LabelEntity(SiteName, @new);
            if (Get(old.Name, old.Category, old.Culture) == null)
            {
                if (!string.IsNullOrEmpty(@new.Category))
                {
                    AddCategory(@new.Category, @new.Culture);
                }
                serviceContext.AddObject(LabelTable, entity);
            }
            else
            {
                serviceContext.AttachTo(LabelTable, entity, "*");
                serviceContext.UpdateObject(entity);
            }
            serviceContext.SaveChangesWithRetries();
        }

        public bool Update(Element element)
        {
            InsertOrUpdateLabel(element, element);
            return true;
        }

        public bool Remove(Element element)
        {
            var entity = new LabelEntity(SiteName, element);
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            entity = serviceContext.CreateQuery<LabelEntity>(LabelTable)
                   .Where(it => it.PartitionKey == SiteName && it.RowKey == entity.RowKey)
                   .FirstOrDefault();
            if (entity != null)
            {
                serviceContext.DeleteObject(entity);
                serviceContext.SaveChangesWithRetries();
            }
            return true;
        }

        public void Clear()
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entities = serviceContext.CreateQuery<LabelEntity>(LabelTable)
                  .Where(it => it.SiteName == SiteName);
            foreach (var item in entities)
            {

                serviceContext.DeleteObject(item);
            }
            serviceContext.SaveChangesWithRetries();
        }

        public void AddCategory(string category, string culture)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = new CategoryEntity(SiteName, category);
            serviceContext.AddObject(LabelCategoryTable, entity);
            serviceContext.SaveChangesWithRetries();
        }

        public bool RemoveCategory(string category, string culture)
        {
            var serviceContext = CloudTableHelper.GetTableServiceContext();
            var entity = serviceContext.CreateQuery<CategoryEntity>(LabelCategoryTable)
                   .Where(it => it.PartitionKey == SiteName && it.RowKey == category)
                   .FirstOrDefault();
            if (entity != null)
            {
                serviceContext.DeleteObject(entity);

                var labels = serviceContext.CreateQuery<LabelEntity>(LabelTable)
                  .Where(it => it.PartitionKey == SiteName && it.Category == category);
                foreach (var item in labels)
                {
                    serviceContext.DeleteObject(item);
                }

                serviceContext.SaveChangesWithRetries();
            }
            return true;
        }
    }
}
