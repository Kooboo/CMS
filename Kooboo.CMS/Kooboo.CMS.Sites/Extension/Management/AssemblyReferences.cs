using Kooboo.IO;
using Kooboo.CMS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using System.Reflection;

namespace Kooboo.CMS.Sites.Extension.Management
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IAssemblyReferences))]
    public class AssemblyReferences : IAssemblyReferences
    {
        #region .ctor
        private System.Threading.ReaderWriterLockSlim _readerWriterLock = new System.Threading.ReaderWriterLockSlim();
        private IBaseDir _baseDir;
        public AssemblyReferences(IBaseDir baseDir)
        {
            _baseDir = baseDir;
        }
        #endregion

        private string GetAssemblyVersion(string assemblyFile)
        {
            string version = "unknown";
            try
            {
                var assemblyName = AssemblyName.GetAssemblyName(assemblyFile);
                version = assemblyName.Version.ToString();
            }
            catch (Exception e)
            { }
            return version;
        }
        #region GetReferenceCollection
        private AssemblyReferenceCollection GetReferenceCollection()
        {
            try
            {
                _readerWriterLock.EnterUpgradeableReadLock();

                var dataFile = GetReferenceDataFile();
                if (!File.Exists(dataFile))
                {
                    return RebuildReferenceData();
                }

                var jsonData = Kooboo.IO.IOUtility.ReadAsString(dataFile);
                if (string.IsNullOrEmpty(jsonData))
                {
                    return RebuildReferenceData();
                }

                var relationData = JsonConvert.DeserializeObject<AssemblyReferenceCollection>(jsonData);
                return relationData;
            }
            finally
            {
                _readerWriterLock.ExitUpgradeableReadLock();
            }
        }
        #endregion

        #region RebuildReferenceData
        private AssemblyReferenceCollection RebuildReferenceData()
        {
            try
            {
                AssemblyReferenceCollection list = new AssemblyReferenceCollection();
                _readerWriterLock.EnterWriteLock();

                var binFolder = Path.Combine(_baseDir.CMSBaseDir + "Bin");
                foreach (var item in Directory.EnumerateFiles(binFolder, "*.dll"))
                {
                    var fileName = Path.GetFileName(item);
                    var version = GetAssemblyVersion(item);
                    list.Add(new AssemblyReferenceData(fileName, version, "System"));
                }
                return list;
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }
        }
        #endregion

        #region SaveReferenceCollection
        private void SaveReferenceCollection(AssemblyReferenceCollection referenceCollection)
        {
            try
            {
                _readerWriterLock.EnterWriteLock();
                var dataFile = GetReferenceDataFile();
                var jsonData = JsonConvert.SerializeObject(referenceCollection, Formatting.Indented);
                Kooboo.IO.IOUtility.SaveStringToFile(dataFile, jsonData);
            }
            finally
            {
                _readerWriterLock.ExitWriteLock();
            }
        }
        #endregion

        #region GetReferenceDataFile
        private string GetReferenceDataFile()
        {
            var metaDir = Path.Combine(_baseDir.CMSBaseDir, "META");
            if (!Directory.Exists(metaDir))
            {
                Directory.CreateDirectory(metaDir);
            }
            var dataFile = Path.Combine(metaDir, "DllReferences.txt");

            return dataFile;
        }
        #endregion

        #region Check
        public IEnumerable<ConflictedAssemblyReference> Check(IEnumerable<string> assemblies)
        {
            var referenceCollection = GetReferenceCollection();
            List<ConflictedAssemblyReference> conflictedAssemblyReference = new List<ConflictedAssemblyReference>();
            foreach (var item in assemblies)
            {
                var version = GetAssemblyVersion(item);
                var fileName = Path.GetFileName(item);
                var reference = referenceCollection[fileName];
                if (reference != null)
                {
                    if (reference.Version != version.ToString())
                    {
                        conflictedAssemblyReference.Add(new ConflictedAssemblyReference(reference, version.ToString()));
                    }
                }
            }

            return conflictedAssemblyReference;
        }

        #endregion

        #region AddReference
        public void AddReference(string assemblyFile, string user)
        {
            var referenceCollection = GetReferenceCollection();
            var filename = Path.GetFileName(assemblyFile);
            var reference = referenceCollection[filename];
            var version = GetAssemblyVersion(assemblyFile);
            if (reference == null)
            {
                referenceCollection.Add(new AssemblyReferenceData(filename, version, user));
            }
            else
            {
                reference.Version = version;
                reference.UserList.Add(user);
            }

            SaveReferenceCollection(referenceCollection);
        }
        #endregion

        #region RemoveReference
        public bool RemoveReference(string assemblyFile, string user)
        {
            var referenceCollection = GetReferenceCollection();
            var filename = Path.GetFileName(assemblyFile);
            var reference = referenceCollection[filename];
            bool emptyReference = false;
            if (reference == null)
            {
                emptyReference = true;
            }
            else
            {
                reference.UserList.RemoveAll(it => it.EqualsOrNullEmpty(user, StringComparison.OrdinalIgnoreCase));
                if (reference.UserList.Count == 0)
                {
                    referenceCollection.Remove(reference);
                    emptyReference = true;
                }
                SaveReferenceCollection(referenceCollection);
            }

            return emptyReference;
        }
        #endregion
    }
}
