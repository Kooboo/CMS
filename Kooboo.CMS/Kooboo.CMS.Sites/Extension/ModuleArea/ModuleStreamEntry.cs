using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleStreamEntry : IDisposable
    {
        ZipFile _zip;
        string _moduleDirName;
        ZipEntry _moduleConfigEntry;
        string _moduleName;
        ModuleInfo _moduleInfo;

        #region Properties
        public string ModuleName
        {
            get
            {
                return _moduleName;
            }
        }
        public ModuleInfo ModuleInfo
        {
            get { return _moduleInfo; }
        }
        #endregion

        #region .ctor
        public ModuleStreamEntry(string moduleName, Stream stream)
        {
            _moduleName = moduleName;
            _zip = ZipFile.Read(stream);
            ParseModuleStream();
        }
        #endregion

        #region ParseModuleStream
        private void ParseModuleStream()
        {
            _moduleConfigEntry = ParseZipFile(_zip, out _moduleDirName);
            using (MemoryStream ms = new MemoryStream())
            {
                _moduleConfigEntry.Extract(ms);
                ms.Position = 0;
                _moduleInfo = ModuleInfo.Get(ms);
            }
            if (!string.IsNullOrEmpty(_moduleInfo.ModuleName))
            {
                _moduleName = _moduleInfo.ModuleName;
            }
            else
            {
                _moduleInfo.ModuleName = _moduleName;
            }
        }
        private static ZipEntry ParseZipFile(ZipFile zipFile, out string dirName)
        {
            dirName = null;
            var moduleConfigEntry = zipFile[ModuleInfo.ModuleInfoFileName];
            if (moduleConfigEntry == null)
            {
                if (zipFile.Entries.Count > 0)
                {
                    moduleConfigEntry = zipFile.Entries.Where(it => it.FileName.Contains(ModuleInfo.ModuleInfoFileName, StringComparison.OrdinalIgnoreCase))
                        .FirstOrDefault();
                    if (moduleConfigEntry != null)
                    {
                        var dirs = moduleConfigEntry.FileName.Split('/');
                        if (dirs.Length > 2)
                        {
                            moduleConfigEntry = null;
                        }
                        else
                        {
                            dirName = dirs[0];
                        }
                    }
                }
            }
            return moduleConfigEntry;
        }

        #endregion

        public bool IsValid()
        {
            return _moduleConfigEntry != null;
        }

        #region Extract

        public void Extract(string physicalPath)
        {
            _zip.ExtractAll(physicalPath, ExtractExistingFileAction.OverwriteSilently);

            if (!string.IsNullOrEmpty(_moduleDirName))
            {
                var subDir = Path.Combine(physicalPath, _moduleDirName);
                Kooboo.IO.IOUtility.CopyDirectory(subDir, physicalPath);
                Kooboo.IO.IOUtility.DeleteDirectory(subDir, true);
            }
        }
        #endregion

        public void SaveTo(string physicalPath)
        {
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
            _zip.Save(physicalPath);
        }
        #region Dispose

        public void Dispose()
        {
            if (_zip != null)
            {
                _zip.Dispose();
                _zip = null;
            }
        }
        #endregion
    }
}
