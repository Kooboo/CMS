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
using System.IO;
using Kooboo.Collections;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Models
{
    public partial class HtmlBlock : DirectoryResource
    {
        public HtmlBlock() { }
        public HtmlBlock(Site site, string name)
            : base(site, name)
        { }
        public HtmlBlock(string physicalPath)
            : base(physicalPath) { }

        public string Body { get; set; }

        #region DirectoryResource
        static string PATH_NAME = "HtmlBlocks";
        public override IEnumerable<string> RelativePaths
        {
            get { return new[] { PATH_NAME }; }
        }

        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            this.Name = relativePaths.Last();

            return relativePaths.Take(relativePaths.Count() - 2);
        }
        #endregion

        public override bool Exists()
        {
            return Kooboo.CMS.Sites.Persistence.Providers.HtmlBlockProvider.Get(this) != null;
        }
    }

    public partial class HtmlBlock : ISiteObject, IFilePersistable, IPersistable, IIdentifiable, IInheritable<HtmlBlock>, Kooboo.CMS.Sites.Versioning.IVersionable
    {
        #region IPersistable
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
        private bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            this.Name = ((HtmlBlock)source).Name;
            this.isDummy = false;
        }

        void IPersistable.OnSaved()
        {
            isDummy = false;
        }

        void IPersistable.OnSaving()
        {

        }
        public string DataFileName
        {
            get { return "Body.html"; }
        }
        public string DataFile
        {
            get { return Path.Combine(this.PhysicalPath, DataFileName); }
        }
        #endregion

        #region IInheritable
        public HtmlBlock LastVersion()
        {
            return LastVersion(this.Site);
        }
        public HtmlBlock LastVersion(Site site)
        {
            var lastVersion = new HtmlBlock(site, this.Name);
            while (!lastVersion.Exists())
            {
                if (lastVersion.Site.Parent == null)
                {
                    break;
                }
                lastVersion = new HtmlBlock(lastVersion.Site.Parent, this.Name);
            }
            return lastVersion;
        }

        public bool HasParentVersion()
        {
            var parentSite = this.Site.Parent;
            while (parentSite != null)
            {
                var htmlBlock = new HtmlBlock(parentSite, this.Name);
                if (htmlBlock.Exists())
                {
                    return true;
                }
                parentSite = parentSite.Parent;
            }
            return false;
        }
        public bool IsLocalized(Site site)
        {
            return (new HtmlBlock(site, this.Name)).Exists();
        }
        #endregion

        #region IVersionable

        public string UserName
        {
            get;
            set;
        }
        #endregion
    }
}
