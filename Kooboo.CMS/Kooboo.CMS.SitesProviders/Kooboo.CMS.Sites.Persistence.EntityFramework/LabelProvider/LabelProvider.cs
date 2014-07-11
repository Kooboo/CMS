#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
extern alias EntityFramework;

using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using EntityFramework::System.Data.Entity;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.LabelProvider
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ILabelProvider), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<Label>), Order = 100)]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ISiteExportableProvider), Order = 100, Key = "LabelProvider")]
    public class LabelProvider : ILabelProvider
    {
        #region _dbContext
        private SiteDBContext _dbContext;
        public LabelProvider(SiteDBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        #endregion

        #region Label
        public IQueryable<Label> GetLabels(Site site, string category)
        {
            return _dbContext.Labels
                          .Where(it => it.SiteName == site.FullName)
                          .Where(it => it.Category == (category ?? ""))
                          .ToArray()
                          .Select(it => it.ToLabel(new Label() { Site = site })).AsQueryable();
        }

        public IEnumerable<Label> All(Site site)
        {
            return _dbContext.Labels
                   .Where(it => it.SiteName == site.FullName)
                   .ToArray()
                   .Select(it => it.ToLabel(new Label() { Site = site })).AsQueryable();
        }

        public IEnumerable<Label> All()
        {
            throw new NotImplementedException();
        }

        private void InsertOrUpdateLabel(Label @new, Label old)
        {
            var entity = _dbContext.Labels
                  .Where(it => it.SiteName == @new.Site.FullName && it.Name == @new.Name && it.Category == (@new.Category ?? ""))
                  .FirstOrDefault();
            if (entity != null)
            {
                entity.Value = @new.Value;
            }
            else
            {
                if (!string.IsNullOrEmpty(@new.Category))
                {
                    AddCategory(@new.Site, @new.Category);
                }
                entity = new LabelEntity(@new.Site.FullName, @new.Name, @new.Value, @new.Category);
                entity.UUID = @new.UUID;
                entity.UtcCreationDate = @new.UtcCreationDate;
                _dbContext.Labels.Add(entity);
            }
            entity.LastestEditor = @new.LastestEditor;
            entity.UtcLastestModificationDate = @new.UtcLastestModificationDate;


            _dbContext.SaveChanges();
        }

        public Label Get(Label dummy)
        {
            var labelEntity = GetLabelEntity(_dbContext.Labels, dummy)
                .FirstOrDefault();

            if (labelEntity != null)
            {
                return labelEntity.ToLabel(dummy);
            }

            return null;
        }

        public void Add(Label item)
        {
            InsertOrUpdateLabel(item, item);
        }

        public void Update(Label @new, Label old)
        {
            InsertOrUpdateLabel(@new, old);
        }
        private IQueryable<LabelEntity> GetLabelEntity(DbSet<LabelEntity> labels, Label label)
        {
            if (!string.IsNullOrEmpty(label.Name))
            {
                return labels.Where(it => it.SiteName == label.Site.FullName && it.Name == label.Name && it.Category == (label.Category ?? ""));
            }
            else
            {
                return labels.Where(it => it.SiteName == label.Site.FullName && it.UUID == label.UUID);
            }
        }

        public void Remove(Label item)
        {
            var entity = GetLabelEntity(_dbContext.Labels, item).FirstOrDefault();
            if (entity != null)
            {
                _dbContext.Labels.Remove(entity);
                _dbContext.SaveChanges();
            }
        }
        #endregion

        #region Category
        public IEnumerable<string> GetCategories(Site site)
        {
            return _dbContext.LabelCategories
              .Where(it => it.SiteName == site.FullName)
              .Select(it => it.CategoryName).AsQueryable();
        }
        public void AddCategory(Site site, string category)
        {
            var entity = _dbContext.LabelCategories
                  .Where(it => it.SiteName == site.FullName && it.CategoryName == category)
                  .FirstOrDefault();
            if (entity != null)
            {
            }
            else
            {
                entity = new CategoryEntity(site.FullName, category);
                _dbContext.LabelCategories.Add(entity);
                _dbContext.SaveChanges();
            }
        }

        public void RemoveCategory(Site site, string category)
        {

            var entity = _dbContext.LabelCategories
                  .Where(it => it.SiteName == site.FullName && it.CategoryName == category)
                  .FirstOrDefault();
            if (entity != null)
            {
                _dbContext.LabelCategories.Remove(entity);
                var labels = _dbContext.Labels
                   .Where(it => it.SiteName == site.FullName && it.Category == category);
                foreach (var item in labels)
                {
                    _dbContext.Labels.Remove(item);
                }
                _dbContext.SaveChanges();
            }
        }

        #endregion

        #region Export/Import
        public void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, System.IO.Stream outputStream)
        {
            new Kooboo.CMS.Sites.Persistence.FileSystem.LabelImportExportHelper(this).Export(site, labels, categories, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            new Kooboo.CMS.Sites.Persistence.FileSystem.LabelImportExportHelper(this).Import(site, zipStream, @override);
        }

        public void InitializeToDB(Site site)
        {
            new Kooboo.CMS.Sites.Persistence.FileSystem.LabelImportExportHelper(this).InitializeLabels(site);
        }

        public void ExportToDisk(Site site)
        {
            new Kooboo.CMS.Sites.Persistence.FileSystem.LabelImportExportHelper(this).ExportLabelsToDisk(site);
        }

        #endregion

        public void Flush(Site site)
        {

            var entities = _dbContext.Labels
                   .Where(it => it.SiteName == site.FullName);
            foreach (var item in entities)
            {
                _dbContext.Labels.Remove(item);
            }
            _dbContext.SaveChanges();
        }

    }
}
