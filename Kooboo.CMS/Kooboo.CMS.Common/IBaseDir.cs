#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.IO;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Common
{
    #region IBaseDir
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseDir
    {
        /// <summary>
        /// Gets the CMS base dir.
        /// </summary>
        /// <value>
        /// The CMS base dir.
        /// </value>
        string CMSBaseDir { get; }
        /// <summary>
        /// Gets the name of the CMS_ data path.
        /// </summary>
        /// <value>
        /// The name of the CMS_ data path.
        /// </value>
        string Cms_DataPathName { get; }
        /// <summary>
        /// Gets the CMS_data base path.
        /// </summary>
        /// <value>
        /// The CMS_data base path.
        /// </value>
        string Cms_DataPhysicalPath { get; }

        /// <summary>
        /// Gets the CMS_data virutal path.
        /// </summary>
        /// <value>
        /// The CMS_ data virutal path.
        /// </value>
        string Cms_DataVirtualPath { get; }

        /// <summary>
        /// Gets the name of the setting file.
        /// </summary>
        /// <value>
        /// The name of the setting file.
        /// </value>
        string SettingFileName { get; }

        [Obsolete]
        void UpdateFileLink(string sitePath, string newSiteName, string newDatabaseName);
    }
    #endregion

    #region BaseDir
    /// <summary>
    /// 
    /// </summary>
    [Dependency(typeof(IBaseDir))]
    public class BaseDir : IBaseDir
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseDir" /> class.
        /// </summary>
        public BaseDir()
        {
            this.CMSBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            this.Cms_DataPathName = "Cms_Data";
            this.Cms_DataPhysicalPath = Path.Combine(CMSBaseDir, this.Cms_DataPathName);
            Cms_DataVirtualPath = UrlUtility.Combine("~/", this.Cms_DataPathName);
        }

        /// <summary>
        /// Gets the CMS base dir.
        /// </summary>
        /// <value>
        /// The CMS base dir.
        /// </value>
        public string CMSBaseDir
        {
            get;
            private set;
        }
        /// <summary>
        /// Gets the name of the CMS_ data path.
        /// </summary>
        /// <value>
        /// The name of the CMS_ data path.
        /// </value>
        public string Cms_DataPathName
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the CMS_ data base path.
        /// </summary>
        /// <value>
        /// The CMS_ data base path.
        /// </value>
        public string Cms_DataPhysicalPath
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the name of the setting file.
        /// </summary>
        /// <value>
        /// The name of the setting file.
        /// </value>
        public string SettingFileName
        {
            get { return "setting.config"; }
        }


        /// <summary>
        /// Gets or sets the CMS_ data virutal path.
        /// </summary>
        public string Cms_DataVirtualPath
        {
            get;
            set;
        }


        public virtual void UpdateFileLink(string sitePath, string newSiteName, string newDatabaseName)
        {
            string sitesBaseVirtualPath = "/" + Cms_DataPathName + "/Sites";
            string siteFilePathPattern = sitesBaseVirtualPath + "/[^/]+/";
            string siteFileReplacement = sitesBaseVirtualPath + "/" + (newSiteName ?? "") + "/";

            string databaseBaseVirtualPath = "/" + Cms_DataPathName + "/Contents";
            string databaseFilePathPattern = databaseBaseVirtualPath + "/[^/]+/";
            string databaseFilePathReplacement = databaseBaseVirtualPath + "/" + (newDatabaseName ?? "") + "/";

            foreach (var file in GetFiles(sitePath, new[] { "*.cshtml", "*.html", "*.xml" }, SearchOption.AllDirectories))
            {
                if (!string.IsNullOrEmpty(newSiteName))
                {
                    ReplaceFile(file, siteFilePathPattern, siteFileReplacement);
                }
                if (!string.IsNullOrEmpty(newDatabaseName))
                {
                    ReplaceFile(file, databaseFilePathPattern, databaseFilePathReplacement);
                }
            }
        }
        protected virtual IEnumerable<string> GetFiles(string path, string[] searchPatterns, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            return searchPatterns.AsParallel().SelectMany(searchPattern => Directory.EnumerateFiles(path, searchPattern, searchOption));
        }
        private void ReplaceFile(string filePath, string pattern, string replacement)
        {
            string fileBody = IOUtility.ReadAsString(filePath);
            fileBody = Regex.Replace(fileBody, pattern, replacement, RegexOptions.IgnoreCase);
            IOUtility.SaveStringToFile(filePath, fileBody);
        }
    }
    #endregion
}