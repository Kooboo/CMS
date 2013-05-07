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

namespace Kooboo.CMS.Sites.Versioning
{
    public class VersionInfo
    {
        public int Version { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }
    }
    public interface IVersionLogger<T>
        where T : DirectoryResource
    {
        void LogVersion(T o);
        IEnumerable<VersionInfo> AllVersions(T o);
        T GetVersion(T o, int version);

        void Revert(T o, int version);
    }
    public abstract class FileVersionLogger<T> : IVersionLogger<T>
         where T : DirectoryResource, IVersionable
    {

        public abstract void LogVersion(T o);

        public IEnumerable<VersionInfo> AllVersions(T o)
        {
            var versionBasePath = new VersionBasePath(o);
            List<VersionInfo> versions = new List<VersionInfo>();
            if (versionBasePath.Exists())
            {
                var dirInfo = new DirectoryInfo(versionBasePath.PhysicalPath);
                foreach (var versionItem in dirInfo.EnumerateDirectories())
                {
                    int version = 0;
                    if (int.TryParse(versionItem.Name, out version))
                    {
                        var data = GetVersion(o, version);
                        if (data!=null)
                        {
                            versions.Add(new VersionInfo() { Version = version, Date = versionItem.CreationTimeUtc, UserName = data.UserName });
                        }                        
                    }
                }
            }
            return versions.OrderByDescending(it => it.Version);
        }
        protected int NextVersionId(T o)
        {
            var versionId = 0;
            var versions = AllVersions(o).ToArray();
            if (versions.Length == 0)
            {
                versionId = 1;
            }
            else
                versionId = AllVersions(o).Max(it => it.Version) + 1;

            return versionId;
        }



        public abstract T GetVersion(T o, int version);


        public abstract void Revert(T o, int version);
    }
}
