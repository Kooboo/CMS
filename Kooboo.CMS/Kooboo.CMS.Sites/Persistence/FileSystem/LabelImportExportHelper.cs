#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Ionic.Zip;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kooboo.CMS.Sites.Persistence.FileSystem
{
    public class LabelImportExportHelper
    {
        #region .ctor
        static ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();
        private ILabelProvider _rawLabelProvider;
        public LabelImportExportHelper(ILabelProvider rawLabelProvider)
        {
            _rawLabelProvider = rawLabelProvider;
        }
        #endregion

        #region GetImportExportLabelFile
        private string GetImportExportLabelFile(Site site, string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return Path.Combine(GetImportExportTempFolder(site), LabelProvider.DefaultLabelFile);
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

        #region GetStorage
        protected virtual JsonListFileStorage<Label> GetStorage(string labelFile)
        {
            var storage = new JsonListFileStorage<Label>(labelFile, _locker);
            return storage;
        }
        #endregion

        public void Export(Site site, IEnumerable<Label> labels, IEnumerable<string> categories, Stream outputStream)
        {
            if ((labels == null || labels.Count() == 0) && (categories == null || categories.Count() == 0))
            {
                this.ExportLabelsToDisk(site);

                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddSelectedFiles("*.json", new LabelPath(site).PhysicalPath, "");

                    zipFile.Save(outputStream);
                }
            }
            else
            {
                var tempFolder = GetImportExportTempFolder(site);
                Kooboo.IO.IOUtility.DeleteDirectory(tempFolder, true);

                if (labels != null)
                {
                    foreach (var item in labels)
                    {
                        var label = _rawLabelProvider.Get(item);
                        if (label != null)
                        {
                            var storage = GetStorage(GetImportExportLabelFile(site, label.Category));
                            storage.Add(label);
                        }
                    }
                }

                if (categories != null)
                {
                    foreach (var item in categories)
                    {
                        foreach (var label in _rawLabelProvider.GetLabels(site, item).ToArray())
                        {
                            var storage = GetStorage(GetImportExportLabelFile(site, label.Category));
                            storage.Add(label);
                        }
                    }
                }

                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddSelectedFiles("*.json", tempFolder, "");

                    zipFile.Save(outputStream);
                }
                Kooboo.IO.IOUtility.DeleteDirectory(tempFolder, true);

            }
        }


        public void Import(Site site, Stream zipStream, bool @override)
        {
            var tempFolder = GetImportExportTempFolder(site);
            Kooboo.IO.IOUtility.DeleteDirectory(tempFolder, true);

            using (ZipFile zipFile = ZipFile.Read(zipStream))
            {
                var action = ExtractExistingFileAction.OverwriteSilently;
                zipFile.ExtractAll(tempFolder, action);
            }

            var categories = new[] { "" }.Concat(LabelProvider.GetCategories(tempFolder));
            foreach (var item in categories)
            {
                var storage = GetStorage(GetImportExportLabelFile(site, item));
                foreach (var label in storage.GetList(site).ToArray())
                {
                    _rawLabelProvider.Add(label);
                }
            }
            Kooboo.IO.IOUtility.DeleteDirectory(tempFolder, true);
        }

        public void InitializeLabels(Site site)
        {
            if (!(_rawLabelProvider is LabelProvider))
            {
                var fileLabelProvider = new LabelProvider();
                foreach (var item in fileLabelProvider.All(site))
                {
                    _rawLabelProvider.Add(item);
                }
            }
        }

        public void ExportLabelsToDisk(Site site)
        {
            if (!(_rawLabelProvider is LabelProvider))
            {
                var fileLabelProvider = new LabelProvider();
                fileLabelProvider.Flush(site);

                foreach (var item in _rawLabelProvider.All(site))
                {
                    fileLabelProvider.Add(item);
                }
            }
        }
    }
}
