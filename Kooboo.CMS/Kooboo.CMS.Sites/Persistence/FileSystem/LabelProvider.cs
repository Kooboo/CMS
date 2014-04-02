#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence.FileSystem.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(ILabelProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Label>))]
    public class LabelProvider : ILabelProvider
    {
        #region Static fields
        public static string DefaultLabelFile = "Label.json";
        static ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        #endregion

        #region GetCategories
        public IEnumerable<string> GetCategories(Site site)
        {
            var labelPath = new LabelPath(site);
            return GetCategories(labelPath.PhysicalPath);
        }

        internal static IEnumerable<string> GetCategories(string labelPath)
        {
            if (Directory.Exists(labelPath))
            {
                var jsonFiles = Directory.EnumerateFiles(labelPath, "*.json");

                return jsonFiles.Select(it => Path.GetFileNameWithoutExtension(it))
                    .Where(it => !it.EqualsOrNullEmpty(Path.GetFileNameWithoutExtension(DefaultLabelFile), StringComparison.OrdinalIgnoreCase));
            }
            return new string[0];
        }

        #endregion

        #region GetLabelFile
        private string GetLabelFile(Site site, string category)
        {
            var labelPath = new LabelPath(site);
            if (string.IsNullOrEmpty(category))
            {
                return Path.Combine(labelPath.PhysicalPath, DefaultLabelFile);
            }
            else
            {
                return Path.Combine(labelPath.PhysicalPath, category + ".json");
            }
        }
        #endregion

        #region GetStorage
        protected virtual JsonListFileStorage<Label> GetStorage(string labelFile)
        {
            var storage = new JsonListFileStorage<Label>(labelFile, _locker);
            return storage;
        }
        #endregion

        #region GetLabels
        public IQueryable<Label> GetLabels(Site site, string category)
        {
            var labelFile = GetLabelFile(site, category);

            var storage = GetStorage(labelFile);

            var list = storage.GetList();

            foreach (var item in list)
            {
                item.Site = site;
            }

            return list.AsQueryable();
        }
        #endregion

        #region AddCategory
        public void AddCategory(Site site, string category)
        {
            //not need to implement for file system provider.
        }
        #endregion

        #region RemoveCategory
        public void RemoveCategory(Site site, string category)
        {
            var categoryFile = GetLabelFile(site, category);

            try
            {
                _locker.EnterWriteLock();

                if (File.Exists(categoryFile))
                {
                    File.Delete(categoryFile);
                }
            }
            finally
            {
                _locker.ExitWriteLock();
            }
        }
        #endregion

        #region All
        public IEnumerable<Label> All(Site site)
        {
            var categories = new string[] { "" }.Concat(this.GetCategories(site));

            return categories.SelectMany(it => GetLabels(site, it)).ToArray();
        }

        #endregion

        #region Add
        public void Add(Label item)
        {
            Add(item, true);
        }
        public void Add(Label item, bool @override)
        {
            var categoryFile = GetLabelFile(item.Site, item.Category);

            var storage = GetStorage(categoryFile);

            storage.Add(item, @override);
        }
        #endregion

        #region All() NotSupportedException
        public IEnumerable<Label> All()
        {
            throw new NotSupportedException();
        }
        #endregion

        #region Get
        public Label Get(Label dummy)
        {
            var categoryFile = GetLabelFile(dummy.Site, dummy.Category);

            var storage = GetStorage(categoryFile);

            return storage.Get(dummy);
        }


        #endregion

        #region Remove
        public void Remove(Label item)
        {
            var categoryFile = GetLabelFile(item.Site, item.Category);

            var storage = GetStorage(categoryFile);

            storage.Remove(item);
        }
        #endregion

        #region Update
        public void Update(Label @new, Label old)
        {
            var categoryFile = GetLabelFile(@new.Site, @new.Category);

            var storage = GetStorage(categoryFile);

            storage.Update(@new, old);
        }
        #endregion

        #region Import/Export
        #region GetImportExportLabelFile
        private string GetImportExportLabelFile(Site site, string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return Path.Combine(GetImportExportTempFolder(site), DefaultLabelFile);
            }
            else
            {
                return Path.Combine(GetImportExportTempFolder(site), category + ".json");
            }
        }
        private string GetImportExportTempFolder(Site site)
        {
            var labelPath = new LabelPath(site);
            return Path.Combine(labelPath.PhysicalPath, "TEMP");
        }
        #endregion

        #region Export
        public void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, Stream outputStream)
        {
            new LabelImportExportHelper(this).Export(site, labels, categories, outputStream);
        }
        #endregion

        public void Import(Site site, Stream zipStream, bool @override)
        {
            new LabelImportExportHelper(this).Import(site, zipStream, @override);
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
            var labelPath = new LabelPath(site);

            Kooboo.IO.IOUtility.DeleteDirectory(labelPath.PhysicalPath, true);
        }
        #endregion

        #region ISiteElementProvider InitializeToDB/ExportToDisk
        public void InitializeToDB(Site site)
        {
            //not need to implement.
        }

        public void ExportToDisk(Site site)
        {
            //not need to implement.
        }
        #endregion
    }
}
