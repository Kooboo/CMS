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

namespace Kooboo.CMS.Sites.Models
{
    public class AssemblyFile : FileResource
    {
        public AssemblyFile() { }
        public AssemblyFile(string physicalPath)
            : base(physicalPath)
        {
        }
        public AssemblyFile(Site site, string fileName)
            : base(site, fileName)
        {

        }

        public override IEnumerable<string> RelativePaths
        {
            get { return new string[] { "Assemblies" }; }
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
}
