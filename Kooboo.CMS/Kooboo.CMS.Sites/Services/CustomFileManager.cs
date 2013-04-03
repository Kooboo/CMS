using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Persistence.FileSystem;
using System.IO;

namespace Kooboo.CMS.Sites.Services
{
    public class CustomFileManager : PathResourceManagerBase<CustomFile, ICustomFileProvider>
    {
        public virtual IEnumerable<CustomFile> All(CustomDirectory directory)
        {
            return ((ICustomFileProvider)Provider).All(directory);
        }
        public override CustomFile Get(Site site, string name)
        {
            throw new NotImplementedException();
        }


        public virtual void SaveFile(Site site, string fullName, string fileName, Stream fileSream)
        {
            if (!string.IsNullOrEmpty(fullName))
            {
                var nameArr = CustomDirectoryHelper.SplitFullName(fullName);
                CustomDirectory di = new CustomDirectory(site, nameArr);
                CustomFile file = new CustomFile(di, fileName);
                file.Save(fileSream);
            }
            else
            {
                CustomFile file = new CustomFile(site, fileName);
                file.Save(fileSream);
            }

        }


        public virtual void Delete(Site site, string fullName, string fileName)
        {
            var nameArr = CustomDirectoryHelper.SplitFullName(fullName);
            var count = nameArr.Count();
            if (count <= 1)//0级目录
            {
                //CustomDirectory di = new CustomDirectory(site, nameArr.Take(count - 1).ToArray());
                CustomFile imgFile = new CustomFile(site, fileName);
                imgFile.Delete();
            }
            else
            {
                CustomDirectory di = new CustomDirectory(site, nameArr.Take(count - 1).ToArray());
                CustomFile imgFile = new CustomFile(di, fileName);
                imgFile.Delete();
            }

        }
        public virtual void CreateDirectory(Site site, CustomDirectory parentDir, string folderName)
        {
            var dir = parentDir != null ? new CustomDirectory(parentDir, folderName) : new CustomDirectory();
            dir.Name = folderName;
            dir.Site = site;
            dir.Create();

        }
        #region Import & Export

        #region Import
        public virtual void Import(Site site, CustomDirectory dir, Stream stream, bool @override)
        {
            ((ICustomFileProvider)Provider).Import(site, dir, stream, @override);
        }
        #endregion

        #region Export
        public virtual void Export(CustomDirectory dir, Stream stream)
        {
            ((ICustomFileProvider)Provider).Export(dir, stream);
        }
        #endregion
        #endregion
    }
    public class CustomFileManagerEx : FileManager
    {
        protected override DirectoryResource GetRootDir(Site site)
        {
            return new CustomFileBaseDirectory(site);
        }
    }
}
