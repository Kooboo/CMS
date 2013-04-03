using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public partial class StyleFile : ThemeFile
    {
        public StyleFile() { }
        public StyleFile(string physicalPath)
            : base(physicalPath)
        {
        }
        public StyleFile(Theme theme, string fileName)
            : base(theme, fileName)
        {
        }
        public override string FileExtension
        {
            get
            {
                return ".css";
            }
            set
            {

            }
        }        
    }

    public partial class StyleFile :  IInheritable<StyleFile>
    {
        #region IInheritable
        public StyleFile LastVersion()
        {
            return LastVersion(this.Site);
        }
        public StyleFile LastVersion(Site site)
        {
            do
            {
                var theme = new Theme(site, this.Theme.Name);
                var lastVersion = new StyleFile(theme, this.FileName);
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
            return (new StyleFile(new Theme(site, this.Theme.Name), this.FileName)).Exists();
        }
        public bool HasParentVersion()
        {
            Site parentSite = null;
            do
            {
                parentSite = this.Site.Parent;
                var scriptFile = new StyleFile(new Theme(parentSite, this.Theme.Name), this.FileName);
                if (scriptFile.Exists())
                {
                    return true;
                }
            } while (parentSite != null);
            return false;
        }
        #endregion
    }
}
