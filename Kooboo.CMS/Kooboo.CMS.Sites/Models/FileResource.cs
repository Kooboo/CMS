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
using Kooboo.Common.IO;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public abstract class FileResource : PathResource
    {

        protected FileResource()
        {
        }
        protected FileResource(string physicalPath)
            : base(physicalPath)
        {

        }
        protected FileResource(Site site, string fileName)
            : base(site, Path.GetFileNameWithoutExtension(fileName))
        {
            this.FileExtension = Path.GetExtension(fileName);
        }
        public virtual string FileExtension { get; set; }

        //public virtual string StringBody { get; set; }

        public virtual string FileName
        {
            get
            {
                return this.Name + this.FileExtension;
            }
            set
            {
                this.Name = Path.GetFileNameWithoutExtension(value);
                this.FileExtension = Path.GetExtension(value);
            }
        }

        public override string PhysicalPath
        {
            get
            {
                return base.PhysicalPath + FileExtension;
            }
        }
        public override string VirtualPath
        {
            get
            {
                return base.VirtualPath + FileExtension;
            }
        }

        private string body = null;
        public string Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }

        }

        /// <summary>
        /// Parses the object.
        /// </summary>
        /// <param name="relativePaths">The relative paths. <example>{"site1","themes","default","style.css"}</example></param>
        /// <returns>
        /// the remaining paths.<example>{"site1","themes","default"}</example>
        /// </returns>
        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            this.FileName = relativePaths.Last();

            return relativePaths.Take(relativePaths.Count() - 1);
        }

        #region IO

        public override void Rename(string newName)
        {
            var newPath = Path.Combine(this.BasePhysicalPath, newName);
            IOUtility.RenameFile(this.PhysicalPath, newPath);
            this.Name = newName;
        }
        public override bool Exists()
        {
            return File.Exists(this.PhysicalPath);
        }
        public void Save(byte[] data)
        {
            using (FileStream fs = new FileStream(this.PhysicalPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                fs.Write(data, 0, data.Length);
            }
        }

        public void Save(Stream data)
        {
            var dir = Path.GetDirectoryName(this.PhysicalPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (FileStream fs = new FileStream(this.PhysicalPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                data.CopyTo(fs);
            }
        }

        public void Save(string content)
        {
            Kooboo.Common.IO.IOUtility.EnsureDirectoryExists(Path.GetDirectoryName(this.PhysicalPath));
            using (FileStream fs = new FileStream(this.PhysicalPath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                fs.WriteString(content);
            }
        }
        public void Save()
        {
            this.Save(this.body);
        }

        public string Read()
        {
            if (this.Exists())
            {
                using (FileStream fs = new FileStream(this.PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    this.body = fs.ReadString();
                }
            }
            return this.body;
        }

        public override void Delete()
        {
            if (File.Exists(this.PhysicalPath))
            {
                File.Delete(this.PhysicalPath);
            }
            else
            {
                if (Directory.Exists(this.PhysicalPath))
                {
                    Directory.Delete(this.PhysicalPath, true);
                }                
            }

        }
        #endregion
    }
}
