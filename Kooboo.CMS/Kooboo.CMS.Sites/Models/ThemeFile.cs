using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Kooboo.CMS.Sites.Models
{
    public partial class ThemeFile : FileResource
    {
        public ThemeFile() { }
        public ThemeFile(string physicalPath)
            : base(physicalPath)
        {
        }
        public ThemeFile(Theme theme, string fileName)
            : base(theme.Site, fileName)
        {
            this.Theme = theme;
        }
        public override IEnumerable<string> RelativePaths
        {
            get
            {
                //
                return Theme.RelativePaths.Concat(new string[] { Theme.Name });
            }
        }

        public Theme Theme { get; set; }


        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="relativePaths">The relative paths. <example>{"site1","themes","default","style1.css"}</example></param>
        /// <returns>
        /// the remaining paths.<example>{"site1"}</example>
        /// </returns>
        internal override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            //call base return {"site1","themes","default"}
            relativePaths = base.ParseObject(relativePaths);

            this.Theme = new Theme();

            // return {"site1"}
            return this.Theme.ParseObject(relativePaths);

        }
    }


    public partial class ThemeFile : IInheritable<ThemeFile>
    {
        #region IInheritable<Theme> Members
        public ThemeFile LastVersion()
        {
            return LastVersion(this.Site);
        }
        public ThemeFile LastVersion(Site site)
        {
            while (!this.Exists() && this.Site.Parent != null)
            {
                this.Site = this.Site.Parent;
            }
            return this;
        }

        public bool IsLocalized(Site site)
        {
            return this.Site.Equals(site);
        }
        public bool HasParentVersion()
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
