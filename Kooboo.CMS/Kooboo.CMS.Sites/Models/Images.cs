using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class Images : DirectoryResource
    {
        public Images(string physicalPath)
            : base(physicalPath)
        {
        }
        public Images(Site site)
            : base(site, "")
        {

        }
        public override string Name
        {
            get
            {
                return "images";
            }
            set
            {
                //base.Name = value;
            }
        }
        public override IEnumerable<string> RelativePaths
        {
            get { return new string[] { }; }
        }


        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="relativePaths">The relative paths. <example>{"site1","images","default"}</example></param>
        /// <returns>
        /// the remaining paths.<example>{"site1"}</example>
        /// </returns>
        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            return relativePaths.Take(relativePaths.Count() - 1);
        }
    }
}
