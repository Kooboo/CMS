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
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Persistence
{
    [Dependency(typeof(IEntityFileProvider))]
    public class EntityFileProvider : IEntityFileProvider
    {
        #region Properties
        public ICommerceDataDir DataDir { get; private set; }
        #endregion

        #region .ctor

        public EntityFileProvider(ICommerceDataDir commerceDataDir)
        {
            DataDir = commerceDataDir;
        }
        #endregion

        #region Methods
        public EntityFileOperationResult Save(Models.EntityFile entityFile, string folderName)
        {
            var result = new EntityFileOperationResult();
            if (entityFile.Data.Length > 0)
            {
                var physicalPath = Path.Combine(DataDir.DataPhysicalPath, folderName, entityFile.FileName);
                var virtualPath = UrlUtility.Combine(DataDir.DataVirutalPath, folderName, entityFile.FileName);
                entityFile.Data.SaveAs(physicalPath);

                result.PhysicalPath = physicalPath;
                result.VirtualPath = virtualPath;

            }
            return result;

        }

        public void DeleteFolder(string folderName)
        {
            var physicalPath = Path.Combine(DataDir.DataPhysicalPath, folderName);
            IOUtility.DeleteDirectory(physicalPath, true);
        }

        public void Delete(string virutalPath)
        {
            var physicalPath = Kooboo.Web.Url.UrlUtility.MapPath(virutalPath);
            if (File.Exists(physicalPath))
            {
                File.Delete(physicalPath);
            }
        }
        #endregion
    }
}
