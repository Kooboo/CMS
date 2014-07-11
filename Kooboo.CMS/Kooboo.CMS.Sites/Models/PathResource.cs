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
using System.Runtime.Serialization;
using Kooboo.Common.Web;


namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public abstract class PathResource : IPath
    {
        public static string SettingFileName = "setting.config";

        #region .ctor
        protected PathResource()
        {
        }

        protected PathResource(string physicalPath)
        {
            Parse(physicalPath);
        }
        public PathResource(Site site, string name)
        {
            if (site != null)
            {
                this.Site = new Site(site.FullName);
            }
            this.Name = name;
        } 
        #endregion

        //[DataMember(Name = "Name", Order = 1)]
        public virtual string Name { get; set; }

        #region path
        /// <summary>
        /// <example>new string[] {"themes","default"} </example>
        /// </summary>
        /// <value>The relative paths.</value>
        public abstract IEnumerable<string> RelativePaths { get; }

       
        public virtual string PhysicalPath
        {
            get
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    return this.BasePhysicalPath;
                }
                return Path.Combine(BasePhysicalPath, this.Name);
            }
        }

        public virtual string VirtualPath
        {
            get
            {
                return UrlUtility.Combine(BaseVirtualPath, this.Name);
            }
        }

        public virtual string BasePhysicalPath
        {
            get
            {
                return Path.Combine(Site.PhysicalPath, Path.Combine(RelativePaths.ToArray()));
            }
        }

        public virtual string BaseVirtualPath
        {
            get
            {
                return UrlUtility.Combine(Site.VirtualPath, UrlUtility.Combine(RelativePaths.ToArray()));
            }
        }
      
        #endregion


        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="physicalPath">The physical path. <example>d:\cms\sites\site1\themes\default</example></param>
        private void Parse(string physicalPath)
        {
            //remaining: site1\themes\default
            var relativePaths = Site.TrimBasePhysicalPath(physicalPath).Split(Path.DirectorySeparatorChar);
            //parse themes object, remainingPaths will be: site1
            var remainingPaths = ParseObject(relativePaths);
            //
            this.Site = Site.ParseSiteFromRelativePath(remainingPaths);
        }
        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="relativePaths">The relative paths. <example>{"site1","themes","default"}</example> </param>
        /// <returns>the remaining paths.<example>{"site1"}</example></returns>
        public abstract IEnumerable<string> ParseObject(IEnumerable<string> relativePaths);

        public virtual Site Site { get; set; }

        public abstract bool Exists();
        public abstract void Delete();
        public abstract void Rename(string newName);

        [DataMember]
        public virtual DateTime LastUpdateDate { get; set; }

        #region Override object
        public override string ToString()
        {
            return this.Name;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is PathResource))
            {
                return false;
            }
            if (obj != null)
            {
                PathResource o = (PathResource)obj;
                if (this.Site == o.Site && o.Name.EqualsOrNullEmpty(this.Name, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
