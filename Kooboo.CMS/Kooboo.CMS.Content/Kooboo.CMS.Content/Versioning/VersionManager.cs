#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
namespace Kooboo.CMS.Content.Versioning
{
    public interface IContentVersionLogger
    {
        void LogVersion(TextContent content);
        IEnumerable<int> AllVersions(TextContent content);
        VersionInfo GetVersion(TextContent content, int version);
        IEnumerable<VersionInfo> AllVersionInfos(TextContent content);
    }
    public class ContentVersionLogger : IContentVersionLogger
    {

        public void LogVersion(TextContent content)
        {
            VersionInfo versionInfo = new VersionInfo()
            {
                CommitUser = content.UserId,
                UtcCommitDate = content.UtcLastModificationDate,
                TextContent = new Dictionary<string, object>(content)
            };
            if (!string.IsNullOrEmpty(content.FolderName))
            {
                versionInfo.Categories = ServiceFactory.TextContentManager
                                     .QueryCategories(content.GetRepository(), content.FolderName, content.UUID)
                                     .Select(it => new Category()
                                     {
                                         FolderName = it.CategoryFolder.FullName,
                                         Contents = it.Contents.Select(c => new CategoryContent() { UUID = c.UUID, DisplayName = c.GetSummary() }).ToArray()
                                     })
                                     .ToArray();
            }

            DataContractSerializationHelper.Serialize(versionInfo, GetNextVersionFile(content));
        }

        private string GetNextVersionFile(TextContent content)
        {
            var versions = AllVersions(content);
            var nextVersion = versions.Count() == 0 ? 1 : versions.Max() + 1;
            return GetVersionFile(content, nextVersion);
        }

        private string GetVersionFile(TextContent content, int version)
        {
            ContentVersionPath versionPath = new ContentVersionPath(content);
            return Path.Combine(versionPath.PhysicalPath, string.Format("{0}.xml", version));
        }

        public IEnumerable<int> AllVersions(TextContent content)
        {
            ContentVersionPath versionPath = new ContentVersionPath(content);
            if (Directory.Exists(versionPath.PhysicalPath))
            {
                foreach (var file in Directory.EnumerateFiles(versionPath.PhysicalPath))
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    int version;
                    if (int.TryParse(fileName, out version))
                    {
                        yield return version;
                    }
                }
            }
        }

        public VersionInfo GetVersion(TextContent content, int version)
        {
            string versionFile = GetVersionFile(content, version);
            if (File.Exists(versionFile))
            {
                var versionInfo = DataContractSerializationHelper.Deserialize<VersionInfo>(versionFile);
                versionInfo.Version = version;
                return versionInfo;
            }
            return null;
        }

        public IEnumerable<VersionInfo> AllVersionInfos(TextContent content)
        {
            return AllVersions(content).Select(it => GetVersion(content, it)).OrderByDescending(it => it.Version);
        }
    }
    public class VersionManager
    {
        public static IContentVersionLogger VersionLogger = new ContentVersionLogger();
        public static void LogVersion(TextContent content)
        {
            if (content.GetRepository().AsActual().EnableVersioning.Value == true)
            {
                VersionLogger.LogVersion(content);
            }
        }
        public static IEnumerable<int> AllVersions(TextContent content)
        {
            return VersionLogger.AllVersions(content);
        }

        public static VersionInfo GetVersion(TextContent content, int version)
        {
            return VersionLogger.GetVersion(content, version);
        }

        public static IEnumerable<VersionInfo> AllVersionInfos(TextContent content)
        {
            return VersionLogger.AllVersionInfos(content);
        }
    }
}
