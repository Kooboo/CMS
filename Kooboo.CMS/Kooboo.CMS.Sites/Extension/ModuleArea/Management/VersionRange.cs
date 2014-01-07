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

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    public class VersionRange : IComparable<VersionRange>
    {
        #region .ctor
        private Version _sourceVersion;
        private Version _targetVersion;
        public VersionRange()
        { }
        public VersionRange(string targetVersion)
        {
            this.TargetVersion = targetVersion;
        }
        public VersionRange(string sourceVersion, string targetVersion)
            : this(targetVersion)
        {
            this.SourceVersion = sourceVersion;
        }
        #endregion

        #region Properties
        public string SourceVersion
        {
            get
            {
                if (_sourceVersion == null)
                {
                    return null;
                }
                return _sourceVersion.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._sourceVersion = null;
                }
                else
                {
                    this._sourceVersion = Version.Parse(value);
                }
            }
        }
        public string TargetVersion
        {
            get
            {
                if (_targetVersion == null)
                {
                    return null;
                }
                return _targetVersion.ToString();
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    this._targetVersion = null;
                }
                else
                {
                    this._targetVersion = Version.Parse(value);
                }
            }
        }
        public InstallationType InstallationType
        {
            get
            {
                if (string.IsNullOrEmpty(SourceVersion))
                {
                    return Management.InstallationType.New;
                }
                var sourceVersion = Version.Parse(SourceVersion);
                var targetVersion = Version.Parse(TargetVersion);
                if (sourceVersion == targetVersion)
                {
                    return InstallationType.Repair;
                }
                else if (sourceVersion > targetVersion)
                {
                    return InstallationType.Downgrade;
                }
                else
                {
                    return InstallationType.Upgrade;
                }
            }
        }
        #endregion

        #region Operations
        public bool In(VersionRange versionRange)
        {
            if (this.InstallationType != versionRange.InstallationType)
            {
                return false;
            }
            switch (InstallationType)
            {
                case InstallationType.New:
                    if (this._sourceVersion == null)
                    {
                        return true;
                    }
                    return InUpgrade(versionRange);
                case InstallationType.Repair:
                    return false;
                case InstallationType.Upgrade:
                    return InUpgrade(versionRange);
                case InstallationType.Downgrade:
                    return InDowngrade(versionRange);
                default:
                    return false;
            }
        }
        private bool InUpgrade(VersionRange versionRange)
        {
            if (this._sourceVersion == null)
            {
                return false;
            }

            return this._sourceVersion >= versionRange._sourceVersion && this._targetVersion <= versionRange._targetVersion;
        }
        private bool InDowngrade(VersionRange versionRange)
        {
            if (this._sourceVersion == null)
            {
                return false;
            }
            return this._sourceVersion <= versionRange._sourceVersion && this._targetVersion >= versionRange._targetVersion;
        }
        #endregion

        #region IComparable
        public int CompareTo(VersionRange other)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region ToString
        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.SourceVersion))
            {
                return this.TargetVersion;
            }
            else
            {
                return string.Format("{0} - {1}", this.SourceVersion, this.TargetVersion);
            }
        }
        #endregion

        #region CreateVersionRange
        public static VersionRange Create(string versionRange)
        {
            if (string.IsNullOrEmpty(versionRange))
            {
                throw new ArgumentNullException(versionRange);
            }
            string[] versions = versionRange.Split(new[] { "-" }, StringSplitOptions.RemoveEmptyEntries);
            if (versions.Length == 1)
            {
                return new VersionRange(versions[0].Trim());
            }
            else
            {
                return new VersionRange(versions[0].Trim(), versions[1].Trim());
            }
        }
        #endregion
    }
}
