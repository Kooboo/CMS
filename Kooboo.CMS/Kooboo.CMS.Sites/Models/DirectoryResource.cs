using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public abstract class DirectoryResource : PathResource
    {
        protected DirectoryResource()
        {
        }
        protected DirectoryResource(string physicalPath)
            : base(physicalPath)
        {
        }
        protected DirectoryResource(Site site, string name)
            : base(site, name)
        {
        }
        public override bool Exists()
        {
            return Directory.Exists(this.PhysicalPath);
        }
        public override void Delete()
        {
            if (Exists())
            {
                Directory.Delete(this.PhysicalPath, true);
            }
        }
        public override void Rename(string @newName)
        {
            IO.IOUtility.RenameDirectory(this.PhysicalPath, @newName);
            this.Name = @newName;
        }
    }
}
