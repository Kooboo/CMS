#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICommerceDataDir
    {
        /// <summary>
        /// Gets the system dir.
        /// </summary>
        /// <value>
        /// The system dir.
        /// </value>
        IBaseDir CMSDir { get; }
        /// <summary>
        /// Gets the name of the data dir.
        /// </summary>
        /// <value>
        /// The name of the data dir.
        /// </value>
        string DataDirName { get; }
        /// <summary>
        /// Gets the data physical path.
        /// </summary>
        /// <value>
        /// The data physical path.
        /// </value>
        string DataPhysicalPath { get; }
        string DataVirutalPath { get; }
    }
    [Dependency(typeof(ICommerceDataDir))]
    public class CommerceDataDir : ICommerceDataDir
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommerceDataDir" /> class.
        /// </summary>
        /// <param name="cmsDir">The CMS dir.</param>
        public CommerceDataDir(IBaseDir cmsDir)
        {
            CMSDir = cmsDir;
            this.DataPhysicalPath = Path.Combine(cmsDir.Cms_DataBasePath, this.DataDirName);
            this.DataVirutalPath = UrlUtility.Combine(cmsDir.Cms_DataVirutalPath, this.DataDirName);
        }
        /// <summary>
        /// Gets the system dir.
        /// </summary>
        /// <value>
        /// The system dir.
        /// </value>
        public IBaseDir CMSDir
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the name of the data dir.
        /// </summary>
        /// <value>
        /// The name of the data dir.
        /// </value>
        public string DataDirName
        {
            get
            {
                return "eCommerce";
            }
        }

        /// <summary>
        /// Gets the data physical path.
        /// </summary>
        /// <value>
        /// The data physical path.
        /// </value>
        public string DataPhysicalPath
        {
            get;
            set;
        }



        public string DataVirutalPath
        {
            get;
            set;
        }
    }
}
