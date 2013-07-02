using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.Default
{
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMembershipProvider))]
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IProvider<Membership>))]
    public class MembershipProvider : SettingFileProviderBase<Membership>, IMembershipProvider
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
        public IEnumerable<Membership> All()
        {
            string baseDir = GetBasePath();
            List<Membership> list = new List<Membership>();
            if (Directory.Exists(baseDir))
            {
                foreach (var dir in IO.IOUtility.EnumerateDirectoriesExludeHidden(baseDir))
                {
                    if (File.Exists(Path.Combine(dir.FullName, _membershipPath.BaseDir.SettingFileName)))
                    {
                        list.Add(new Membership() { Name = dir.Name });
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
        protected override string GetDataFilePath(Membership o)
        {
            return _membershipPath.GetMembershipSettingFilePath(o);
        }
        #endregion
    }

    #region MembershipPath
    public class MembershipPath
    {
        protected static string Dir_Name = "Members";
        public IBaseDir BaseDir { get; private set; }
        public MembershipPath(IBaseDir baseDir)
        {
            this.BaseDir = baseDir;
        }

        public virtual string GetBasePath()
        {
            return Path.Combine(BaseDir.Cms_DataPhysicalPath, Dir_Name);
        }

        public virtual string GetMembershipPath(Membership o)
        {
            var baseDir = GetBasePath();

            return Path.Combine(baseDir, o.Name);
        }

        public virtual string GetMembershipSettingFilePath(Membership o)
        {
            var membershipPath = GetMembershipPath(o);

            return Path.Combine(membershipPath, BaseDir.SettingFileName);
        }
    }
    #endregion
}
