#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.Couchbase.LabelProvider
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ILabelProvider), Order = 100)]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Label>), Order = 100)]
    public class LabelProvider : ILabelProvider
    {
        Func<Site, string, Label> createModel = (Site site, string key) =>
        {
            return new Label(site, key);
        };
        #region GetKey
        private static string GetLabelUUID(string category, string name)
        {
            name = name ?? "";
            category = category ?? "";
            // culture = culture ?? "";

            return string.Format("_NAME_{0}_CATEGORY_{1}",
                   name.ToLower(),
                   category.ToLower());
        }
        private static string GetElementCategoryKey(string category)
        {
            category = category ?? "";

            return string.Format("_CATEGORY_{0}",
                   category.ToLower());
        }

        private static string GetElementKey(Label label)
        {
            var key = label.UUID;
            if (string.IsNullOrEmpty(key))
                key = GetLabelUUID(label.Category, label.Name);
            return key;
        }

        /// <summary>
        /// compatibility reason.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns></returns>
        private static Label ResetLabelUUID(Label label)
        {
            if (label != null)
            {
                //if (string.IsNullOrEmpty(label.UUID))
                label.UUID = GetLabelUUID(label.Category, label.Name);
            }

            return label;
        }
        #endregion

        #region GetLabels
        public IQueryable<Label> GetLabels(Site site, string category)
        {
            var all = All(site);
            if (string.IsNullOrEmpty(category))
            {
                all = all.Where(it => string.IsNullOrEmpty(it.Category));
            }
            else
            {
                all = all.Where(it => it.Category.EqualsOrNullEmpty(category, StringComparison.OrdinalIgnoreCase));
            }

            return all.AsQueryable();
        }

        public IEnumerable<Label> All(Site site)
        {
            return DataHelper.QueryList<Label>(site, ModelExtensions.GetQueryViewName(ModelExtensions.LabelDataType))
                .Select(it => ResetLabelUUID(it)).AsQueryable();
        }

        public IEnumerable<Label> All()
        {
            throw new NotImplementedException();
        }

        public Label Get(Label dummy)
        {
            var key = GetElementKey(dummy);

            key = ModelExtensions.GetBucketDocumentKey(ModelExtensions.LabelDataType, key);

            return ResetLabelUUID(DataHelper.QueryByKey<Label>(dummy.Site, key, createModel));
        }


        private void InsertOrUpdateLabel(Label @new, Label old)
        {
            if (!string.IsNullOrEmpty(@new.Category))
            {
                AddCategory(@new.Site, @new.Category);
            }
            @new = ResetLabelUUID(@new);
            var key = GetElementKey(@new);
            DataHelper.StoreObject(@new.Site, @new, key, ModelExtensions.LabelDataType);
        }

        public void Add(Label item)
        {
            InsertOrUpdateLabel(item, item);
        }

        public void Update(Label @new, Label old)
        {
            InsertOrUpdateLabel(@new, old);
        }

        public void Remove(Label item)
        {
            var key = GetElementKey(item);
            DataHelper.DeleteItemByKey(item.Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.LabelDataType, key));
        }
        #endregion

        #region Category
        public IEnumerable<string> GetCategories(Site site)
        {
            return DataHelper.QueryList<LabelCategory>(site, ModelExtensions.GetQueryViewName(ModelExtensions.LabelCategoryDataType)).Select(it => it.Category).AsQueryable();
        }

        public void AddCategory(Site site, string category)
        {
            var key = GetElementCategoryKey(category);
            var obj = new LabelCategory() { Category = category };
            DataHelper.StoreObject(site, obj, key, ModelExtensions.LabelCategoryDataType);
        }

        public void RemoveCategory(Site site, string category)
        {
            var key = GetElementCategoryKey(category);

            DataHelper.DeleteItemByKey(site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.LabelCategoryDataType, key));

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

        public void InitializeLabels(Site site)
        {
            new Kooboo.CMS.Sites.Persistence.FileSystem.LabelImportExportHelper(this).InitializeLabels(site);
        }

        public void ExportLabelsToDisk(Site site)
        {
            new Kooboo.CMS.Sites.Persistence.FileSystem.LabelImportExportHelper(this).ExportLabelsToDisk(site);
        }

        #endregion

        #region Flush
        public void Flush(Site site)
        {
        }
        #endregion
    }
}
