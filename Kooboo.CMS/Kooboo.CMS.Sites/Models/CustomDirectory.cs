using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Collections;
using System.IO;
namespace Kooboo.CMS.Sites.Models
{
    public static class CustomDirectoryHelper
    {
        public static readonly char Sperator = Path.PathSeparator;
        public static string CombineFullName(IEnumerable<string> pageNames)
        {
            return string.Join(Path.PathSeparator.ToString(), pageNames.ToArray());
        }
        public static string[] SplitFullName(string fullName)
        {
            return fullName.Split(new string[] { "\\", "/" }, StringSplitOptions.RemoveEmptyEntries);
        }
        public static string VirtualPathToFullName(string vPath)
        {
            var nameArr = vPath.Split('/');
            var arr = nameArr.Skip(4);
            return string.Join(Sperator.ToString(), arr);
        }
        public static string Combine(params string[] values)
        {
            return string.Join(Path.PathSeparator.ToString(), values);
        }
    }
    public class CustomDirectory : DirectoryResource, IInheritable<CustomDirectory>
    {
        public CustomDirectory()
        { }
        public CustomDirectory(Site site, string fullName) :
            this(site, CustomDirectoryHelper.SplitFullName(fullName))
        {

        }
        public CustomDirectory(Site site, params string[] names)
            : base(site, names.Count() > 0 ? names.Last() : "")
        {
            SetNamePath(names);
        }

        private void SetNamePath(IEnumerable<string> names)
        {
            if (names.Count() > 1)
            {
                this.Parent = new CustomDirectory(Site, names.Take(names.Count() - 1).ToArray());
            }
        }
        public CustomDirectory(CustomDirectory parent, string name)
            : base(parent.Site, name)
        {
            this.Parent = parent;
        }
        public CustomDirectory(string physicalPath)
            : base(physicalPath)
        {

        }
        public CustomDirectory Parent { get; set; }

        public virtual string FullName
        {
            get
            {
                return CustomDirectoryHelper.CombineFullName(NamePaths);
            }
            set
            {
                var names = CustomDirectoryHelper.SplitFullName(value);

                SetNamePath(names);
            }
        }

        public virtual IEnumerable<string> NamePaths
        {
            get
            {
                if (this.Parent == null)
                {
                    return new string[] { this.Name };
                }
                return this.Parent.NamePaths.Concat(new string[] { this.Name }); ;
            }
        }

        public override IEnumerable<string> RelativePaths
        {
            get
            {
                return new string[] { CustomFile.PATH_NAME }.Concat(NamePaths.Take(NamePaths.Count() - 1));
            }
        }

        internal override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            this.Name = relativePaths.Last();
            var pathNameIndex = relativePaths.IndexOf(CustomFile.PATH_NAME, StringComparer.InvariantCultureIgnoreCase);
            var count = relativePaths.Count();
            if (pathNameIndex + 2 < count)
            {
                this.Parent = new CustomDirectory();
                return this.Parent.ParseObject(relativePaths.Take(count - 1));
            }
            return relativePaths.Take(pathNameIndex);
        }

        public override Site Site
        {
            get
            {
                if (this.Parent != null)
                {
                    return Parent.Site;
                }
                return base.Site;
            }
            set
            {
                base.Site = value;
                if (this.Parent != null)
                {
                    this.Parent.Site = value;
                }
            }
        }

        public bool Create()
        {
            string path = Path.Combine(this.PhysicalPath);
            if (Directory.Exists(path))
            {
                return false;
            }
            Directory.CreateDirectory(path);
            return true;
        }

        public override string ToString()
        {
            return this.FullName;
        }

        public CustomDirectory LastVersion()
        {
            return LastVersion(this.Site);
        }
        public CustomDirectory LastVersion(Site site)
        {
            var lastVersion = new CustomDirectory(site, this.NamePaths.ToArray());
            while (!lastVersion.Exists())
            {
                if (lastVersion.Site.Parent == null)
                {
                    break;
                }
                lastVersion = new CustomDirectory(lastVersion.Site.Parent, this.NamePaths.ToArray());
            }
            return lastVersion;
        }


        public bool HasParentVersion()
        {
            throw new NotImplementedException();
        }

        public bool IsLocalized(Site site)
        {
            throw new NotImplementedException();
        }
    }
}
