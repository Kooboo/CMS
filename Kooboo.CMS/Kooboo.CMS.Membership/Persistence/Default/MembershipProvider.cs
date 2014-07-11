using Ionic.Zip;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Membership.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Kooboo.CMS.Common;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Membership.Persistence.Default
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IMembershipProvider))]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IProvider<Kooboo.CMS.Membership.Models.Membership>))]
    public class MembershipProvider : SettingFileProviderBase<Kooboo.CMS.Membership.Models.Membership>, IMembershipProvider
    {
        #region .ctor
        static System.Threading.ReaderWriterLockSlim locker = new System.Threading.ReaderWriterLockSlim();
        protected static string Dir_Name = "Members";
        MembershipPath _membershipPath = null;
        public MembershipProvider(MembershipPath membershipPath)
        {
            this._membershipPath = membershipPath;
        }
        #endregion

        #region All
        public IEnumerable<Kooboo.CMS.Membership.Models.Membership> All()
        {
            string baseDir = GetBasePath();
            List<Kooboo.CMS.Membership.Models.Membership> list = new List<Kooboo.CMS.Membership.Models.Membership>();
            if (Directory.Exists(baseDir))
            {
                foreach (var dir in IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    if (File.Exists(Path.Combine(dir.FullName, _membershipPath.BaseDir.SettingFileName)))
                    {
                        list.Add(new Kooboo.CMS.Membership.Models.Membership() { Name = dir.Name });
                    }
                }
            }
            return list;
        }
        #endregion

        #region  abstract implementation
        protected override System.Threading.ReaderWriterLockSlim GetLocker()
        {
            return locker;
        }
        protected virtual string GetBasePath()
        {
            return _membershipPath.GetBasePath();
        }
        protected override string GetDataFilePath(Kooboo.CMS.Membership.Models.Membership o)
        {
            return _membershipPath.GetMembershipSettingFilePath(o);
        }
        #endregion

        #region Export
        public Kooboo.CMS.Membership.Models.Membership Import(string membershipName, Stream stream)
        {
            var membership = new Kooboo.CMS.Membership.Models.Membership(membershipName);
            if (Get(membership) != null)
            {
                return membership;
            }
            var path = _membershipPath.GetMembershipPath(membership);
            using (ZipFile zipFile = ZipFile.Read(stream))
            {
                ExtractExistingFileAction action = ExtractExistingFileAction.OverwriteSilently;
                zipFile.ExtractAll(path, action);
            }
            return membership;
        }
        public void Export(Kooboo.CMS.Membership.Models.Membership membership, Stream outputStream)
        {
            var physicalPath = _membershipPath.GetMembershipPath(membership);
            using (ZipFile zipFile = new ZipFile(Encoding.UTF8))
            {
                //zipFile.ZipError += new EventHandler<ZipErrorEventArgs>(zipFile_ZipError);

                zipFile.ZipErrorAction = ZipErrorAction.Skip;


                zipFile.AddSelectedFiles("name != *\\~versions\\*.* and name != *\\.svn\\*.* and name != *\\_svn\\*.*", physicalPath, "", true);

                zipFile.Save(outputStream);
            }
        }
        #endregion
    }

    #region MembershipPath
    public class MembershipPath
    {
        protected static string Dir_Name = "Memberships";
        public IBaseDir BaseDir { get; private set; }
        public MembershipPath(IBaseDir baseDir)
        {
            this.BaseDir = baseDir;
        }

        public virtual string GetBasePath()
        {
            return Path.Combine(BaseDir.Cms_DataPhysicalPath, Dir_Name);
        }

        public virtual string GetMembershipPath(Kooboo.CMS.Membership.Models.Membership o)
        {
            var baseDir = GetBasePath();

            return Path.Combine(baseDir, o.Name);
        }

        public virtual string GetMembershipSettingFilePath(Kooboo.CMS.Membership.Models.Membership o)
        {
            var membershipPath = GetMembershipPath(o);

            return Path.Combine(membershipPath, BaseDir.SettingFileName);
        }
    }
    #endregion
}
