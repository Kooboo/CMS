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

namespace Kooboo.CMS.Sites.Persistence.Couchbase.LabelProvider
{
    public class LabelRepository : IElementRepository
    {
        #region .ctor
        public Site Site { get; set; }
        public LabelRepository(Site site)
        {
            this.Site = site;
        }
        #endregion

        #region EnabledLanguages
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return new System.Globalization.CultureInfo[0].AsQueryable();
        }
        #endregion
        #region GetKey
        private static string GetElementKey(string name, string category, string culture)
        {
            name = name ?? "";
            category = category ?? "";
            // culture = culture ?? "";

            return string.Format("_NAME_{0}_CATEGORY_{1}",
                   name.ToLower(),
                   category.ToLower());
        }
        private static string GetElementCategoryKey(string category, string culture)
        {
            category = category ?? "";
            culture = culture ?? "";

            return string.Format("_CATEGORY_{0}",
                   category.ToLower());
        }
        #endregion
        #region Elements
        public IQueryable<Element> Elements()
        {
            return DataHelper.QueryList<Element>(Site, ModelExtensions.GetQueryView(ModelExtensions.LabelDataType)).AsQueryable();
        }
        #endregion

        #region Get
        public Element Get(string name, string category, string culture)
        {
            var key = ModelExtensions.GetBucketDocumentKey(ModelExtensions.LabelDataType, GetElementKey(name, category, culture));

            return DataHelper.QueryByKey<Element>(this.Site, key);
        }
        #endregion

        #region Categories
        public IQueryable<ElementCategory> Categories()
        {
            return DataHelper.QueryList<ElementCategory>(Site, ModelExtensions.GetQueryView(ModelExtensions.LabelCategoryDataType)).AsQueryable();
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
            if (!string.IsNullOrEmpty(@new.Category))
            {
                AddCategory(@new.Category, @new.Culture);
            }
            var key = GetElementKey(@new.Name, @new.Category, @new.Culture);
            DataHelper.StoreObject(Site, @new, key, ModelExtensions.LabelDataType);
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
            var key = GetElementKey(element.Name, element.Category, element.Culture);
            DataHelper.DeleteItemByKey(Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.LabelCategoryDataType, key));
            return true;
        }
        #endregion

        #region Clear
        public void Clear()
        {

        }
        #endregion

        #region AddCategory
        public void AddCategory(string category, string culture)
        {
            var key = GetElementCategoryKey(category, culture);
            var obj = new ElementCategory() { Category = category, Culture = culture };
            DataHelper.StoreObject(Site, obj, key, ModelExtensions.LabelCategoryDataType);

        }
        #endregion

        #region RemoveCategory
        public bool RemoveCategory(string category, string culture)
        {
            var key = GetElementCategoryKey(category, culture);

            DataHelper.DeleteItemByKey(Site, ModelExtensions.GetBucketDocumentKey(ModelExtensions.LabelCategoryDataType, key));

            return true;
        }
        #endregion
    }
}
