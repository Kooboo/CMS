using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Providers.SqlServer.LabelProvider
{
    public class LabelRepository : IElementRepository
    {
        public string SiteName { get; set; }
        public LabelRepository(string siteName)
        {
            this.SiteName = siteName;
        }

        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return new System.Globalization.CultureInfo[0].AsQueryable();
        }

        public IQueryable<Element> Elements()
        {
            return SiteDbContext.CreateDbContext().Labels
                   .Where(it => it.SiteName == SiteName)
                   .Select(it => new Element()
                   {
                       Category = it.Category,
                       Name = it.Name,
                       Value = it.Value
                   }).ToArray().AsQueryable();
        }

        public Element Get(string name, string category, string culture)
        {
            return SiteDbContext.CreateDbContext().Labels
               .Where(it => it.SiteName == SiteName && it.Name == name && it.Category == (category ?? ""))
               .Select(it => new Element()
               {
                   Category = it.Category,
                   Name = it.Name,
                   Value = it.Value
               })
               .FirstOrDefault();
        }

        public IQueryable<ElementCategory> Categories()
        {
            return SiteDbContext.CreateDbContext().LabelCategories
               .Where(it => it.SiteName == SiteName)
               .Select(it => new ElementCategory()
               {
                   Category = it.CategoryName
               }).ToArray().AsQueryable();
        }

        public bool Add(Element element)
        {
            InsertOrUpdateLabel(element, element);
            return true;
        }
        private void InsertOrUpdateLabel(Element @new, Element old)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.Labels
                  .Where(it => it.SiteName == SiteName && it.Name == @new.Name && it.Category == (@new.Category ?? ""))
                  .FirstOrDefault();
            if (entity != null)
            {
                entity.Value = @new.Value;
            }
            else
            {
                if (!string.IsNullOrEmpty(@new.Category))
                {
                    AddCategory(@new.Category, @new.Culture);
                }
                entity = new LabelEntity(SiteName, @new);
                dbContext.Labels.Add(entity);
            }
            dbContext.SaveChanges();
        }

        public bool Update(Element element)
        {
            InsertOrUpdateLabel(element, element);
            return true;
        }

        public bool Remove(Element element)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.Labels
                   .Where(it => it.SiteName == SiteName && it.Name == element.Name && it.Category == (element.Category ?? ""))
                   .FirstOrDefault();
            if (entity != null)
            {
                dbContext.Labels.Remove(entity);
                dbContext.SaveChanges();
            }
            return true;
        }

        public void Clear()
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entities = dbContext.Labels
                   .Where(it => it.SiteName == SiteName);
            foreach (var item in entities)
            {
                dbContext.Labels.Remove(item);
            }
            dbContext.SaveChanges();
        }

        public void AddCategory(string category, string culture)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.LabelCategories
                  .Where(it => it.SiteName == SiteName && it.CategoryName == category)
                  .FirstOrDefault();
            if (entity != null)
            {

            }
            else
            {
                entity = new CategoryEntity(SiteName, category);
                dbContext.LabelCategories.Add(entity);
                dbContext.SaveChanges();
            }

        }

        public bool RemoveCategory(string category, string culture)
        {
            var dbContext = SiteDbContext.CreateDbContext();
            var entity = dbContext.LabelCategories
                  .Where(it => it.SiteName == SiteName && it.CategoryName == category)
                  .FirstOrDefault();
            if (entity != null)
            {
                dbContext.LabelCategories.Remove(entity);
                var labels = dbContext.Labels
                   .Where(it => it.SiteName == SiteName && it.Category == category);
                foreach (var item in labels)
                {
                    dbContext.Labels.Remove(item);
                }
                dbContext.SaveChanges();
            }
            return true;
        }
    }
}
