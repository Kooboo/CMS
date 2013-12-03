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

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.LabelProvider
{
    public class LabelRepository : IElementRepository
    {
        #region _dbContext
        public string SiteName { get; set; }
        private SiteDBContext _dbContext;
        public LabelRepository(string siteName, SiteDBContext dbContext)
        {
            this.SiteName = siteName;
            this._dbContext = dbContext;
        }
        #endregion

        #region EnabledLanguages
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return new System.Globalization.CultureInfo[0].AsQueryable();
        }
        #endregion

        #region Elements
        public IQueryable<Element> Elements()
        {
            return _dbContext.Labels
                   .Where(it => it.SiteName == SiteName)
                   .Select(it => new Element()
                   {
                       Category = it.Category,
                       Name = it.Name,
                       Value = it.Value
                   }).ToArray().AsQueryable();
        }
        #endregion

        #region Get
        public Element Get(string name, string category, string culture)
        {
            return _dbContext.Labels
               .Where(it => it.SiteName == SiteName && it.Name == name && it.Category == (category ?? ""))
               .Select(it => new Element()
               {
                   Category = it.Category,
                   Name = it.Name,
                   Value = it.Value
               })
               .FirstOrDefault();
        } 
        #endregion

        #region Categories
        public IQueryable<ElementCategory> Categories()
        {
            return _dbContext.LabelCategories
               .Where(it => it.SiteName == SiteName)
               .Select(it => new ElementCategory()
               {
                   Category = it.CategoryName
               }).ToArray().AsQueryable();
        } 
        #endregion

        #region Add/Update
        public bool Add(Element element)
        {
            InsertOrUpdateLabel(element, element);
            return true;
        }
        private void InsertOrUpdateLabel(Element @new, Element old)
        {
            var entity = _dbContext.Labels
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
                _dbContext.Labels.Add(entity);
            }
            _dbContext.SaveChanges();
        }

        public bool Update(Element element)
        {
            InsertOrUpdateLabel(element, element);
            return true;
        } 
        #endregion

        #region Remove
        public bool Remove(Element element)
        {
            var entity = _dbContext.Labels
                   .Where(it => it.SiteName == SiteName && it.Name == element.Name && it.Category == (element.Category ?? ""))
                   .FirstOrDefault();
            if (entity != null)
            {
                _dbContext.Labels.Remove(entity);
                _dbContext.SaveChanges();
            }
            return true;
        } 
        #endregion

        #region Clear
        public void Clear()
        {
            var entities = _dbContext.Labels
                   .Where(it => it.SiteName == SiteName);
            foreach (var item in entities)
            {
                _dbContext.Labels.Remove(item);
            }
            _dbContext.SaveChanges();
        } 
        #endregion

        #region AddCategory
        public void AddCategory(string category, string culture)
        {
            var entity = _dbContext.LabelCategories
                  .Where(it => it.SiteName == SiteName && it.CategoryName == category)
                  .FirstOrDefault();
            if (entity != null)
            {
            }
            else
            {
                entity = new CategoryEntity(SiteName, category);
                _dbContext.LabelCategories.Add(entity);
                _dbContext.SaveChanges();
            }

        }        
        #endregion

        #region RemoveCategory
        public bool RemoveCategory(string category, string culture)
        {
            var entity = _dbContext.LabelCategories
                  .Where(it => it.SiteName == SiteName && it.CategoryName == category)
                  .FirstOrDefault();
            if (entity != null)
            {
                _dbContext.LabelCategories.Remove(entity);
                var labels = _dbContext.Labels
                   .Where(it => it.SiteName == SiteName && it.Category == category);
                foreach (var item in labels)
                {
                    _dbContext.Labels.Remove(item);
                }
                _dbContext.SaveChanges();
            }
            return true;
        } 
        #endregion
    }
}
