using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Kooboo.CMS.Sites.Models
{
    public partial class Theme : DirectoryResource
    {
        public Theme()
        {
        }

        public Theme(string physicalPath)
            : base(physicalPath)
        {
        }

        public Theme(Site site, string name)
            : base(site, name)
        {

        }
        public static string PATH_NAME = "Themes";
        public override IEnumerable<string> RelativePaths
        {
            get { return new string[] { PATH_NAME }; }
        }

        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="relativePaths">The relative paths.  <example>{"site1","themes","default"}</example></param>
        /// <returns>the remaining paths. <example>{"site1"}</example></returns>
        internal override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            var index = Array.LastIndexOf(relativePaths.Select(it => it.ToLower()).ToArray(), PATH_NAME.ToLower());
            relativePaths = relativePaths.Take(index + 2);
            this.Name = relativePaths.Last();
            return relativePaths.Take(relativePaths.Count() - 2);
        }
    }

    public partial class Theme : IInheritable<Theme>
    {
        #region IInheritable<Theme> Members
        public Theme LastVersion()
        {
            return LastVersion(this.Site);
        }
        public Theme LastVersion(Site site)
        {
            var lastVersion = new Theme(site, this.Name);
            while (!lastVersion.Exists())
            {
                if (lastVersion.Site.Parent == null)
                {
                    break;
                }
                lastVersion = new Theme(lastVersion.Site.Parent, this.Name);
            }
            return lastVersion;
        }

        public bool IsLocalized(Site site)
        {
            return this.Site.Equals(site);
        }
        public bool HasParentVersion()
        {
            var parentSite = this.Site.Parent;
            while (parentSite != null)
            {
                var theme = new Theme(parentSite, this.Name);
                if (theme.Exists())
                {
                    return true;
                }
                parentSite = parentSite.Parent;
            }
            return false;
        }
        #endregion
    }
}
