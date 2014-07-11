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
using System.IO;
using Kooboo.CMS.Sites.Models;
using System.Reflection;
using Kooboo.CMS.Sites.Extension.Management;
using Kooboo.Common;

namespace Kooboo.CMS.Sites.Services
{
    /// <summary>
    /// Plugin 管理
    /// </summary>
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(AssemblyManager))]
    public class AssemblyManager
    {
        #region .ctor
        IAssemblyReferences _assemblyReferences;
        public AssemblyManager(IAssemblyReferences assemblyReferences)
        {
            _assemblyReferences = assemblyReferences;
            foreach (var site in ServiceFactory.SiteManager.AllRootSites())
            {
                EnsureAssembliesExistsInBin(site, false, false, true);
            }
        }
        #endregion

        #region Upload
        public virtual void Upload(Site site, string fileName, Stream stream)
        {
            AssemblyFile assemblyFile = new AssemblyFile(site, fileName);
            //assemblyFile.
            assemblyFile.Save(stream);
            DeleteFromBin(site, fileName);
            EnsureAssembliesExistsInBin(site);
        }
        #endregion

        #region Delete
        public virtual void Delete(Site site, string fileName)
        {
            AssemblyFile assemblyFile = new AssemblyFile(site, fileName);
            if (assemblyFile.Exists())
            {
                assemblyFile.Delete();
            }
            DeleteFromBin(site, fileName);
            EnsureAssembliesExistsInBin(site);
        }

        private void DeleteFromBin(Site site, string fileName)
        {
            var binFile = GetAssemblyBinFilePath(fileName);
            var canDelete = _assemblyReferences.RemoveReference(binFile, GetReferenceName(site));
            if (canDelete && File.Exists(binFile))
            {
                File.Delete(binFile);
            }
        }
        #endregion

        #region GetTypes
        public virtual IEnumerable<Type> GetTypes(Site site)
        {
            List<Type> types = new List<Type>();
            foreach (var assembly in GetAssemblies(site))
            {
                types.AddRange(assembly.GetTypes());
            }
            return types;
        }

        public virtual IEnumerable<Type> GetTypes(Site site, Type type)
        {
            List<Type> types = new List<Type>();
            while (site != null)
            {
                types.AddRange(GetTypes(site)
                .Where(it => type.IsAssignableFrom(it) && !it.IsInterface && !it.IsAbstract));
                site = site.Parent;
            }
            return types.Distinct();
        }

        public virtual IEnumerable<Type> GetTypes(Site site, string fileName)
        {
            var assembly = GetAssembly(site, fileName);
            if (assembly != null)
            {
                return assembly.GetTypes().Where(it => !it.IsInterface && !it.IsAbstract);
            }
            return new Type[0];
        }
        public virtual IEnumerable<Assembly> GetAssemblies(Site site)
        {
            yield return typeof(AssemblyManager).Assembly;

            var assemblyFiles = GetFiles(site);
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assemblyFiles.Any(it => CompareAssembly(item, it.FileName)))
                {
                    yield return item;
                }
            }
        }
        #endregion

        #region GetTypeInstances
        public virtual IEnumerable<object> GetTypeInstances(Site site, Type type)
        {
            return GetTypes(site, type).Select(it => TypeActivator.CreateInstance(it));
        }
        #endregion

        #region EnsureAssembliesExistsInBin(site);
        public virtual Assembly GetAssembly(Site site, string fileName)
        {
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (CompareAssembly(item, fileName))
                {
                    return item;
                }
            }
            return null;
        }
        private bool CompareAssembly(Assembly assembly, string fileName)
        {
            //Path.GetFileName(assembly.Location) does not support in medium trust level.
            try
            {
                return !assembly.IsDynamic && !assembly.GlobalAssemblyCache && Path.GetFileName(assembly.EscapedCodeBase).EqualsOrNullEmpty(fileName, StringComparison.CurrentCultureIgnoreCase);
            }
            catch
            { return false; }
        }
        private string GetReferenceName(Site site)
        {
            return "site:" + site.FullName.ToLower();
        }
        public void EnsureAssembliesExistsInBin(Site site, bool overwrite = true, bool copyParent = true, bool copyChildren = false)
        {
            var files = GetFiles(site);
            foreach (var file in files)
            {
                if (!_assemblyReferences.IsSystemAssembly(file.PhysicalPath))
                {
                    var fileInBin = GetAssemblyBinFilePath(file.FileName);
                    if (overwrite == true || !File.Exists(fileInBin))
                    {
                        File.Copy(file.PhysicalPath, fileInBin, true);
                        _assemblyReferences.AddReference(file.PhysicalPath, GetReferenceName(site));
                    }
                }
            }
            //foreach (var file in files)
            //{
            //    var assembly = GetAssembly(site, file.FileName);
            //    if (assembly == null)
            //    {
            //        var fileInBin = GetAssemblyBinFilePath(file.FileName);
            //        Assembly.LoadFrom(fileInBin);
            //    }
            //}
            if (copyParent && site.Parent != null)
            {
                EnsureAssembliesExistsInBin(site.Parent, copyParent, copyChildren);
            }
            if (copyChildren)
            {
                foreach (var child in ServiceFactory.SiteManager.ChildSites(site))
                {
                    EnsureAssembliesExistsInBin(child, copyParent, copyChildren);
                }
            }
        }
        private string GetAssemblyBinFilePath(string fileName)
        {
            return Path.Combine(Settings.BinDirectory, fileName);
        }
        #endregion

        #region GetFiles
        public virtual IEnumerable<AssemblyFile> GetFiles(Site site)
        {
            List<AssemblyFile> files = new List<AssemblyFile>();

            AssemblyFile dummy = new AssemblyFile(site, "dummy.dll");
            if (Directory.Exists(dummy.BasePhysicalPath))
            {
                foreach (var file in Directory.EnumerateFiles(dummy.BasePhysicalPath, "*.dll"))
                {
                    files.Add(new AssemblyFile(site, Path.GetFileName(file)));
                }
            }

            return files;
        }
        #endregion
    }
}
