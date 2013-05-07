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
using System.Runtime.Serialization;
using System.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Models
{
    public partial class ScriptFile : FileResource
    {
        public const string PATH_NAME = "Scripts";
        public ScriptFile() { }
        public ScriptFile(string physicalPath)
            : base(physicalPath)
        {
        }
        public ScriptFile(Site site, string fileName)
            : base(site, fileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { return new string[] { PATH_NAME }; }
        }

        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="relativePaths">The relative paths. <example>{"site1","scripts","js.js"}</example></param>
        /// <returns>
        /// the remaining paths.<example>{"site1"}</example>
        /// </returns>
        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            //call base return {"site1","scripts"}
            relativePaths = base.ParseObject(relativePaths);

            return relativePaths.Take(relativePaths.Count() - 1);
        }
    }

    public partial class ScriptFile : IInheritable<ScriptFile>, IIdentifiable
    {
        #region IInheritable
        public ScriptFile LastVersion()
        {
            return LastVersion(this.Site);
        }
        public ScriptFile LastVersion(Site site)
        {
            do
            {
                var lastVersion = new ScriptFile(site, this.FileName);
                if (lastVersion.Exists())
                {
                    return lastVersion;
                }
                site = site.Parent;
            } while (site != null);

            return null;
        }

        public bool IsLocalized(Site site)
        {
            return (new ScriptFile(site, this.FileName)).Exists();
        }
        public bool HasParentVersion()
        {
            var parentSite = this.Site.Parent;
            while (parentSite != null)
            {
                var scriptFile = new ScriptFile(parentSite, this.FileName);
                if (scriptFile.Exists())
                {
                    return true;
                }
                parentSite = parentSite.Parent;
            }
            return false;
        }
        #endregion

        public string UUID
        {
            get
            {
                return this.FileName;
            }
            set
            {
                this.FileName = value;
            }
        }
    }
}
