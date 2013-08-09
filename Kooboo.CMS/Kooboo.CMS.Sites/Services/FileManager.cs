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
using System.IO;
using Kooboo.Web.Url;
using Kooboo.CMS.Sites.Persistence.FileSystem;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Ionic.Zip;
namespace Kooboo.CMS.Sites.Services
{
    public static class FileOrderHelper
    {
        private class Comparer : IComparer<FileResource>
        {
            string[] _orders;
            public Comparer(string[] fileOrders)
            {
                this._orders = fileOrders;
            }
            public int Compare(FileResource x, FileResource y)
            {
                var indexX = Array.IndexOf(_orders, x.FileName);
                var indexY = Array.IndexOf(_orders, y.FileName);
                if (indexX == -1 && indexY != -1)
                {
                    return 1;
                }
                if (indexX != -1 && indexY == -1)
                {
                    return -1;
                }
                if (indexX == -1 && indexY == -1)
                {
                    return 1;
                }
                return indexX.CompareTo(indexY);
            }
        }
        public static string OrderFileName = "Order.txt";
        public static IComparer<FileResource> GetComparer(string orderFile)
        {
            if (!string.IsNullOrEmpty(orderFile) && File.Exists(orderFile))
            {
                var lines = File.ReadAllLines(orderFile);
                return new Comparer(lines);
            }
            return null;
        }
        public static IEnumerable<string> OrderFiles(string orderFile, IEnumerable<string> fileNames)
        {
            if (!string.IsNullOrEmpty(orderFile) && File.Exists(orderFile))
            {
                var lines = File.ReadAllLines(orderFile);
                var ordered = lines.Where(it => fileNames.Any(f => f.EqualsOrNullEmpty(it, StringComparison.CurrentCultureIgnoreCase)));
                return ordered.Concat(fileNames.Except(lines, StringComparer.CurrentCultureIgnoreCase));
            }
            return fileNames;
        }

        public static void SaveFilesOrder(string baseDir, IEnumerable<string> filesOrder)
        {
            var orderFile = Path.Combine(baseDir, FileOrderHelper.OrderFileName);
            if (File.Exists(orderFile))
            {
                File.SetAttributes(orderFile, FileAttributes.Normal);
            }
            File.WriteAllLines(orderFile, filesOrder.ToArray());
            File.SetAttributes(orderFile, FileAttributes.Hidden);
        }
        public static string GetOrderFile(string baseDir)
        {
            return Path.Combine(baseDir, OrderFileName);
        }
    }
    public class FileEntry : FileResource
    {
        public class FileNameEqualityComparer : IEqualityComparer<FileEntry>
        {
            public bool Equals(FileEntry x, FileEntry y)
            {
                if (x == y)
                {
                    return true;
                }
                if (x.FileName.EqualsOrNullEmpty(y.FileName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(FileEntry obj)
            {
                return obj.GetHashCode();
            }
        }
        public FileEntry()
        {
        }
        public FileEntry(DirectoryResource rootDir, string relativePath)
            : base("")
        {
            this.physicalPath = Path.Combine(rootDir.PhysicalPath, relativePath);
            this.virtualPath = UrlUtility.GetVirtualPath(physicalPath);

            this.basePhysicalPath = Path.GetDirectoryName(this.physicalPath);
            var paths = this.virtualPath.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            this.baseVirtualPath = UrlUtility.Combine(paths.Take(paths.Count() - 1).ToArray());
            this.FileName = paths.Last();
            if (this.FileName.Contains('.'))
            {
                this.Name = FileName.Substring(0, FileName.IndexOf("."));
                this.FileExtension = FileExtension.Substring(FileExtension.IndexOf(".") + 1);
            }
            else
            {
                this.Name = FileName;
            }

            this.RootDir = rootDir;
            //this.RelativePath = relativeVirtualPath;
        }

        protected FileEntry(Site site, string fileName)
            : base(site, Path.GetFileNameWithoutExtension(fileName))
        {
            this.FileExtension = Path.GetExtension(fileName);
        }

        //public FileEntry(DirectoryResource rootDir, string fileName)
        //    : this(directory.Site, fileName)
        //{
        //    this.virtualPath = UrlUtility.Combine(directory.VirtualPath, fileName);
        //    this.physicalPath = Path.Combine(directory.PhysicalPath, fileName);
        //    this.basePhysicalPath = directory.PhysicalPath;
        //    this.baseVirtualPath = directory.VirtualPath;

        //    this.RootDir = rootDir;
        //}

        public override IEnumerable<string> RelativePaths
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        private string physicalPath = null;
        public override string PhysicalPath
        {
            get { return physicalPath; }
        }
        private string virtualPath = null;
        public override string VirtualPath
        {
            get
            {
                return virtualPath;
            }
        }
        private string basePhysicalPath = null;
        public override string BasePhysicalPath
        {
            get
            {
                return basePhysicalPath;
            }
        }
        private string baseVirtualPath = null;
        public override string BaseVirtualPath
        {
            get
            {
                return baseVirtualPath;
            }
        }

        private string name;
        public override string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = Path.GetFileName(this.physicalPath);
                }
                return name;
            }
            set
            {
                name = value;
            }
        }

        public decimal FileSize { get; set; }
        public DateTime CreateDate { get; set; }

        public DirectoryResource RootDir { get; set; }
        /// <summary>
        /// the relative path from current root dir
        /// <example>
        /// The full virtual path: '..\Cms_Data\Sites\aaa\Sites\cn1\Themes\Theme1', the RelativeVirtualPathFromRoot will be: 'Theme1'
        /// Hide '..\Cms_Data\Sites\aaa\Sites\cn1\Themes\'
        /// </example>
        /// </summary>
        public string RelativePath
        {
            get
            {
                if (RootDir == null)
                {
                    throw new KoobooException("The root dir is null.");
                }
                return this.PhysicalPath.Remove(0, RootDir.PhysicalPath.Length + 1);
            }
        }


        public bool IsImage
        {
            get
            {
                return Kooboo.Drawing.ImageTools.IsImageExtension(this.FileExtension);
            }
        }

        public bool CanBeEdit
        {
            get
            {
                var ex = this.FileExtension.ToLower();
                return ex == ".txt" || ex == ".js" || ex == ".css" || ex == ".rule";
            }
        }

        public override Site Site
        {
            get
            {
                return this.RootDir.Site;
            }
            set
            {
                // this.RootDir.Site = value;
            }
        }
    }
    public class DirectoryEntry : DirectoryResource
    {
        public class DirectoryNameEqualityComparer : IEqualityComparer<DirectoryEntry>
        {
            public bool Equals(DirectoryEntry x, DirectoryEntry y)
            {
                if (x == y)
                {
                    return true;
                }
                if (x.Name.EqualsOrNullEmpty(y.Name, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return false;
            }

            public int GetHashCode(DirectoryEntry obj)
            {
                return obj.GetHashCode();
            }
        }
        public DirectoryEntry()
        {
        }
        public DirectoryEntry(DirectoryResource rootDir, string relativePath)
            : base("")
        {
            this.physicalPath = Path.Combine(rootDir.PhysicalPath, relativePath);
            this.virtualPath = UrlUtility.GetVirtualPath(physicalPath);
            this.basePhysicalPath = Path.GetDirectoryName(this.physicalPath);
            var paths = this.virtualPath.Split("/".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            this.baseVirtualPath = UrlUtility.Combine(paths.Take(paths.Count() - 1).ToArray());

            this.RootDir = rootDir;
        }
        public DirectoryEntry(DirectoryResource rootDir, DirectoryResource parent, string name)
            : base(parent.Site, name)
        {
            this.virtualPath = UrlUtility.Combine(parent.VirtualPath, name);
            this.physicalPath = Path.Combine(parent.PhysicalPath, name);
            this.basePhysicalPath = parent.PhysicalPath;
            this.baseVirtualPath = parent.VirtualPath;

            this.RootDir = rootDir;
        }
        protected DirectoryEntry(Site site, string name)
            : base(site, name)
        {
        }

        #region override
        public override IEnumerable<string> RelativePaths
        {
            get
            {
                throw new NotSupportedException();
            }
        }

        private string physicalPath = null;
        public override string PhysicalPath
        {
            get { return physicalPath; }
        }
        private string virtualPath = null;
        public override string VirtualPath
        {
            get
            {
                return virtualPath;
            }
        }
        private string basePhysicalPath = null;
        public override string BasePhysicalPath
        {
            get
            {
                return basePhysicalPath;
            }
        }
        private string baseVirtualPath = null;
        public override string BaseVirtualPath
        {
            get
            {
                return baseVirtualPath;
            }
        }

        private string name;
        public override string Name
        {
            get
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    name = Path.GetFileName(this.physicalPath);
                }
                return name;
            }
            set
            {
                name = value;
            }
        }

        public override Site Site
        {
            get
            {
                return this.RootDir.Site;
            }
            set
            {
            }
        }
        #endregion

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            this.Name = relativePaths.Last();
            //this._relativePaths = relativePaths.Take(relativePaths.Count() - 2);
            return new string[0];
        }

        public DirectoryResource RootDir { get; set; }
        /// <summary>
        /// the relative path from current root dir
        /// <example>
        /// The full virtual path: '..\Cms_Data\Sites\aaa\Sites\cn1\Themes\Theme1', the RelativeVirtualPathFromRoot will be: 'Theme1'
        /// Hide '..\Cms_Data\Sites\aaa\Sites\cn1\Themes\'
        /// </example>
        /// </summary>
        public string RelativePath
        {
            get
            {
                if (RootDir == null)
                {
                    throw new KoobooException("The root dir is null.");
                }
                return this.physicalPath.Remove(0, RootDir.PhysicalPath.Length + 1);
            }
        }
    }
    public abstract class FileManager
    {
        protected abstract DirectoryResource GetRootDir(Site site);
        public abstract string Type { get; }

        public string GetRelativePath(string parentRelativePath, string name)
        {
            return string.IsNullOrEmpty(parentRelativePath) ? name : parentRelativePath + Path.DirectorySeparatorChar + name;
        }

        #region Directory
        public virtual DirectoryResource GetDirectory(Site site, string relativePath)
        {
            var dir = GetRootDir(site);
            if (!string.IsNullOrEmpty(relativePath))
                dir = new DirectoryEntry(dir, relativePath);
            return dir;
        }
        public virtual IEnumerable<DirectoryResource> GetDirectories(Site site, string relativePath)
        {
            List<DirectoryEntry> list = new List<DirectoryEntry>();
            while (site != null)
            {
                var dir = GetDirectory(site, relativePath);
                if (dir.Exists())
                {
                    foreach (var item in IO.IOUtility.EnumerateDirectoriesExludeHidden(dir.PhysicalPath))
                    {
                        var dirEntry = new DirectoryEntry(GetRootDir(site), GetRelativePath(relativePath, item.Name)) { LastUpdateDate = item.LastWriteTime };
                        if (!list.Contains(dirEntry, new DirectoryEntry.DirectoryNameEqualityComparer()))
                        {
                            list.Add(dirEntry);
                        }
                    }
                }
                site = site.Parent;
            }
            return list;

        }
        public virtual void DeleteDirectory(Site site, string relativePath)
        {
            var dir = new DirectoryEntry(GetRootDir(site), relativePath);
            dir.Delete();
            FlushWebResourceCache(site, dir);
        }
        public virtual void AddDirectory(Site site, string parentRelativePath, string name)
        {
            DirectoryEntry dir = new DirectoryEntry(GetRootDir(site), GetRelativePath(parentRelativePath, name));
            System.IO.Directory.CreateDirectory(dir.PhysicalPath);
            FlushWebResourceCache(site, dir);
        }

        public virtual void RenameDirectory(Site site, string relativePath, string newName)
        {
            //DirectoryEntry @new = new DirectoryEntry(GetRootDir(site), new_RelativeVirtualPath);
            DirectoryEntry dir = new DirectoryEntry(GetRootDir(site), relativePath);
            dir.Rename(newName);
            FlushWebResourceCache(site, dir);
        }
        public virtual bool IsDirectoryExists(Site site, string parentRelativePath, string name)
        {
            DirectoryEntry dir = new DirectoryEntry(GetRootDir(site), GetRelativePath(parentRelativePath, name));
            return dir.Exists();
        }
        #endregion

        #region File

        public virtual IEnumerable<FileEntry> GetFiles(Site site, string dirRelativePath)
        {
            IComparer<FileResource> comparer = null;
            Site recursiveSite = site;
            while (comparer == null && recursiveSite != null)
            {
                var dir = GetDirectory(recursiveSite, dirRelativePath);
                comparer = FileOrderHelper.GetComparer(FileOrderHelper.GetOrderFile(dir.PhysicalPath));
                recursiveSite = recursiveSite.Parent;
            }

            ICollection<FileEntry> list;
            if (comparer == null)
            {
                list = new List<FileEntry>();
            }
            else
            {
                list = new SortedSet<FileEntry>(comparer);
            }
            recursiveSite = site;
            while (recursiveSite != null)
            {
                var dir = GetDirectory(recursiveSite, dirRelativePath);
                if (dir.Exists())
                {
                    var files = EnumerateFiles(dir.PhysicalPath);
                    var fileEntries = files.Select(it => new FileInfo(Path.Combine(dir.PhysicalPath, it)))
                        .Where(it => !Path.GetFileName(it.FullName).Equals(FileOrderHelper.OrderFileName, StringComparison.OrdinalIgnoreCase))
                        .Select(it => new FileEntry(GetRootDir(recursiveSite), GetRelativePath(dirRelativePath, it.Name))
                    {
                        FileSize = it.Length,
                        Name = it.Name,
                        FileExtension = it.Extension,
                        FileName = Path.GetFileName(it.FullName),
                        CreateDate = it.LastWriteTimeUtc
                    });
                    foreach (var fileEntry in fileEntries)
                    {
                        if (!list.Contains(fileEntry, new FileEntry.FileNameEqualityComparer()))
                        {
                            list.Add(fileEntry);
                        }
                    }
                }
                recursiveSite = recursiveSite.Parent;
            }
            return list;
        }

        protected virtual IEnumerable<string> EnumerateFiles(string dir)
        {
            foreach (var item in System.IO.Directory.EnumerateFiles(dir))
            {
                FileInfo fi = new FileInfo(item);
                if ((fi.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                    yield return Path.GetFileName(item);
            }
        }
        public virtual void SaveFileOrders(Site site, string dirRelativePath, IEnumerable<string> filesOrder)
        {
            var baseDir = GetDirectory(site, dirRelativePath);
            FileOrderHelper.SaveFilesOrder(baseDir.PhysicalPath, filesOrder);
            FlushWebResourceCache(site, null);
        }

        public virtual FileEntry GetFile(Site site, string relativePath)
        {
            FileEntry entry = new FileEntry(GetRootDir(site), relativePath);
            if (entry.Exists())
            {
                var fi = new FileInfo(entry.PhysicalPath);
                entry.Name = fi.Name;
                entry.FileName = fi.FullName;
                entry.FileExtension = fi.Extension;
                entry.Read();
            }
            return entry;
        }

        public virtual FileEntry EditFile(Site site, string relativePath, string body)
        {
            FileEntry entry = new FileEntry(GetRootDir(site), relativePath);
            entry.Body = body;
            entry.Save();

            FlushWebResourceCache(site, entry);
            return entry;
        }

        public virtual void AddFile(Site site, string dirRelativePath, string fileName, Stream fileStream)
        {
            var file = new FileEntry(GetRootDir(site), GetRelativePath(dirRelativePath, fileName));
            if (fileStream != null)
            {
                file.Save(fileStream);
            }
            else
            {
                file.Save();
            }

            FlushWebResourceCache(site, file);
        }

        public virtual void AddFile(Site site, string dirRelativePath, string fileName, string body)
        {
            var file = new FileEntry(GetRootDir(site), GetRelativePath(dirRelativePath, fileName));
            file.Body = body;
            file.Save();

            FlushWebResourceCache(site, file);
        }

        public virtual void DeleteFile(Site site, string fileRelativePath)
        {
            var file = new FileEntry(GetRootDir(site), fileRelativePath);
            file.Delete();
            FlushWebResourceCache(site, file);
        }
        public virtual void RenameFile(Site site, string relativePath, string newName)
        {
            var file = new FileEntry(GetRootDir(site), relativePath);
            file.Rename(newName);
            FlushWebResourceCache(site, file);
        }
        public virtual bool IsFileExists(Site site, string parentRelativePath, string name)
        {
            FileEntry file = new FileEntry(GetRootDir(site), GetRelativePath(parentRelativePath, name));
            return file.Exists();
        }

        public virtual void LocalizeFile(Site site, string fileRelativePath)
        {
            FileEntry targetFileEntry = GetFile(site, fileRelativePath);
            if (!targetFileEntry.Exists())
            {
                FileEntry fileEntry = null;
                Site recursiveSite = site;
                while (fileEntry == null && recursiveSite != null)
                {
                    fileEntry = GetFile(recursiveSite, fileRelativePath);
                    if (!fileEntry.Exists())
                    {
                        fileEntry = null;
                    }
                    recursiveSite = recursiveSite.Parent;
                }

                if (fileEntry != null)
                {
                    File.Copy(fileEntry.PhysicalPath, targetFileEntry.PhysicalPath, true);
                }
            }
        }
        #endregion

        #region Import & Export
        public virtual void Import(Site site, string directoryPath, Stream zipStream, bool @overrided)
        {
            ImportHelper.Import(site, GetDirectory(site, directoryPath).PhysicalPath, zipStream, @overrided);
        }

        public virtual void Export(Site site, string directoryPath, string[] folders, string[] files, Stream outputStream)
        {
            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {
                if (folders != null)
                {
                    foreach (var folder in folders)
                    {
                        var dir = GetDirectory(site, folder);
                        if (dir.Exists())
                        {
                            zipFile.AddDirectory(dir.PhysicalPath, dir.Name);
                        }
                    }
                }
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        var filePath = GetFile(site, file);
                        if (filePath.Exists())
                        {
                            zipFile.AddFile(filePath.PhysicalPath, "");
                        }
                    }
                }
                zipFile.Save(outputStream);
            }
        }
        #endregion

        protected virtual void FlushWebResourceCache(Site site, PathResource resource)
        {
            site = site.AsActual();
            var ticks = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var versions = (string.IsNullOrEmpty(site.Version) ? "1.0.0.0" : site.Version).Split('.');
            versions[versions.Length - 1] = ticks;

            site.Version = string.Join(".", versions);

            ServiceFactory.SiteManager.Update(site);
        }
    }
}
