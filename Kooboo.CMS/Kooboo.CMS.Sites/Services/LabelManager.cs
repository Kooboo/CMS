﻿#region License
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
using Kooboo.CMS.Sites.Globalization;
using Kooboo.Globalization;
using Kooboo.Extensions;
using Ionic.Zip;

namespace Kooboo.CMS.Sites.Services
{
    public class LabelManager
    {
        #region .ctor
        IElementRepositoryFactory _elementRepositoryFactory = null;
        public LabelManager(IElementRepositoryFactory elementRepositoryFactory)
        {
            this._elementRepositoryFactory = elementRepositoryFactory;
        }
        #endregion

        #region GetCategories
        public virtual IEnumerable<ElementCategory> GetCategories(Site site)
        {
            return SiteLabel.GetElementRepository(site).Categories();
        }
        #endregion

        #region GetLabels
        public virtual IQueryable<Element> GetLabels(Site site, string category)
        {
            var query = SiteLabel.GetElementRepository(site).Elements();
            if (string.IsNullOrEmpty(category))
            {
                query = query.Where(it => it.Category == null || it.Category == "");
            }
            else
            {
                query = query.Where(it => it.Category == category);
            }
            return query;
        }
        #endregion

        #region Add
        public virtual void Add(Site site, Element element)
        {
            var oldElement = Get(site, element.Category, element.Name, element.Culture);
            if (oldElement != null)
            {
                throw new ItemAlreadyExistsException();
            }
            SiteLabel.GetElementRepository(site).Add(element);
        }
        #endregion

        #region Get
        public virtual Element Get(Site site, string category, string name, string culture)
        {
            return SiteLabel.GetElementRepository(site).Get(name, category, culture);
        }
        #endregion

        #region Update
        public virtual void Update(Site site, Element element)
        {
            element.Culture = null;
            SiteLabel.GetElementRepository(site).Update(element);

            SiteLabel.ClearCache(site);
        }
        #endregion

        #region Remove
        public virtual void Remove(Site site, Element element)
        {
            element.Culture = null;
            SiteLabel.GetElementRepository(site).Remove(element);
            SiteLabel.ClearCache(site);
        }
        #endregion

        #region RemoveCategory
        public virtual void RemoveCategory(Site site, string category)
        {
            SiteLabel.GetElementRepository(site).RemoveCategory(category, null);
            SiteLabel.ClearCache(site);
        }
        #endregion

        #region AddCategory
        public virtual void AddCategory(Site site, string category)
        {
            SiteLabel.GetElementRepository(site).AddCategory(category, null);
        }
        #endregion

        #region Export

        public virtual void Export(Site site, System.IO.Stream outputStream)
        {
            ExportLabels(site);
            using (ZipFile zipFile = new ZipFile())
            {
                zipFile.AddSelectedFiles("*.*", new Label(site).PhysicalPath, "");

                zipFile.Save(outputStream);
            }
        }

        public virtual void Import(Site site, System.IO.Stream packageStream)
        {
            using (ZipFile zipFile = ZipFile.Read(packageStream))
            {
                var action = ExtractExistingFileAction.OverwriteSilently;
                zipFile.ExtractAll(new Label(site).PhysicalPath, action);
            }
            InitializeLabels(site);
        }

        private void InitializeLabels(Site site)
        {
            var labelRepository = _elementRepositoryFactory.CreateRepository(site);
            if (labelRepository.GetType() != typeof(SiteLabelRepository))
            {
                labelRepository.Clear();
                SiteLabelRepository fileRepository = new SiteLabelRepository(site);
                foreach (var item in fileRepository.Elements())
                {
                    labelRepository.Add(item);
                }
            }
        }

        private void ExportLabels(Site site)
        {
            var labelRepository = _elementRepositoryFactory.CreateRepository(site);
            if (labelRepository.GetType() != typeof(SiteLabelRepository))
            {
                SiteLabelRepository fileRepository = new SiteLabelRepository(site);
                fileRepository.Clear();
                foreach (var item in labelRepository.Elements())
                {
                    fileRepository.Add(item);
                }
            }
        }
        #endregion

    }
}
