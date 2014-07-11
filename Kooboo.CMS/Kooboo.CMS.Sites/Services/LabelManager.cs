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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Globalization;
using Kooboo.Common.Globalization;

using Ionic.Zip;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Common.Misc;

namespace Kooboo.CMS.Sites.Services
{
    public class LabelManager
    {
        #region .ctor
        ILabelProvider _labelProvider;
        public LabelManager(ILabelProvider labelProvider)
        {
            this._labelProvider = labelProvider;
        }
        #endregion

        #region GetCategories
        public virtual IEnumerable<string> GetCategories(Site site)
        {
            return this._labelProvider.GetCategories(site);
        }
        #endregion

        #region GetLabels
        public virtual IQueryable<Label> All(Site site, string category)
        {
            return this._labelProvider.GetLabels(site, category);
        }
        #endregion

        #region Get
        public Label Get(Site site, string category, string name)
        {
            return this._labelProvider.Get(new Label(site, category, name, null));
        }
        #endregion

        #region Add
        public void Add(Site site, Label label)
        {
            if (string.IsNullOrEmpty(label.UUID))
            {
                label.UUID = UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);
            }
            this._labelProvider.Add(label);
        }
        #endregion

        #region Update
        public virtual void Update(Site site, Label @new, Label old)
        {
            if (@new.Site == null)
            {
                @new.Site = site;
            }
            //renew the UUID when UUID is null
            if (string.IsNullOrEmpty(@new.UUID))
            {
                @new.UUID = UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);
            }
            this._labelProvider.Update(@new, old);
        }
        #endregion

        #region Remove
        public virtual void Remove(Site site, Label label)
        {
            this._labelProvider.Remove(label);
        }
        #endregion

        #region RemoveCategory
        public virtual void RemoveCategory(Site site, string category)
        {
            this._labelProvider.RemoveCategory(site, category);
        }
        #endregion

        #region AddCategory
        public virtual void AddCategory(Site site, string category)
        {
            this._labelProvider.AddCategory(site, category);
        }
        #endregion


        #region UpgradeFromOldLabel
        public void UpgradeFromOldLabel(Site site)
        {
            var elementProvider = SiteLabel.GetElementRepository(site);
            foreach (var item in elementProvider.Elements())
            {
                this._labelProvider.Add(new Label(site, item.Category, item.Name, item.Value) { UtcCreationDate = DateTime.UtcNow });

                elementProvider.Remove(item);
            }
        }

        #endregion

        #region Export

        public virtual void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, System.IO.Stream outputStream)
        {
            this._labelProvider.Export(site, labels, categories, outputStream);
        }

        public virtual void Import(Site site, System.IO.Stream packageStream, bool Override)
        {
            this._labelProvider.Import(site, packageStream, Override);
        }

        //private void InitializeLabels(Site site)
        //{
        //    var labelRepository = _elementRepositoryFactory.CreateRepository(site);
        //    if (labelRepository.GetType() != typeof(SiteLabelRepository))
        //    {
        //        labelRepository.Clear();
        //        SiteLabelRepository fileRepository = new SiteLabelRepository(site);
        //        foreach (var item in fileRepository.Elements())
        //        {
        //            labelRepository.Add(item);
        //        }
        //    }
        //}

        //private void ExportLabels(Site site)
        //{
        //    var labelRepository = _elementRepositoryFactory.CreateRepository(site);
        //    if (labelRepository.GetType() != typeof(SiteLabelRepository))
        //    {
        //        SiteLabelRepository fileRepository = new SiteLabelRepository(site);
        //        fileRepository.Clear();
        //        foreach (var item in labelRepository.Elements())
        //        {
        //            fileRepository.Add(item);
        //        }
        //    }
        //}
        #endregion

    }
}
