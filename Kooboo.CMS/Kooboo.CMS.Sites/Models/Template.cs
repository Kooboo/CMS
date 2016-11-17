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
using Kooboo.CMS.Sites.Models;
using System.IO;
using System.Runtime.Serialization;
using Kooboo.IO;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public abstract class Template : DirectoryResource, ISiteObject, IFilePersistable, IPersistable, IIdentifiable, Kooboo.CMS.Sites.Versioning.IVersionable
    {
        protected Template()
        {
        }
        public Template(Site site, string name)
            : base(site, name)
        {
            this.Name = name;
        }
        public Template(string physicalPath)
            : base(physicalPath)
        {
        }
        #region Path
        const string PATH_NAME = "Templates";
        //const string TEMPLATE_FILE = "template";
        public override IEnumerable<string> RelativePaths
        {
            get { return new string[] { PATH_NAME, TemplatePathName }; }
        }

        protected abstract string TemplatePathName { get; }

        private string engineName;
        [DataMember(Order = 0)]
        public string EngineName
        {
            get
            {
                if (string.IsNullOrEmpty(engineName))
                {
                    return "Razor";
                }
                return engineName;
            }
            set
            {
                engineName = value;
            }
        }
        [DataMember(Order = 1)]
        public abstract string FileExtension { get; set; }


        [DataMember(Order = 2)]
        public string UserName { get; set; }

        const string TemplateName = "template";
        public virtual string PhysicalTemplateFileName
        {
            get
            {
                return Path.Combine(this.PhysicalPath, TemplateFileName);
            }
        }
        public virtual string TemplateFileName
        {
            get
            {
                return TemplateName + FileExtension;
            }
        }
        public virtual string TemplateFileVirutalPath
        {
            get
            {
                return System.Web.HttpUtility.UrlDecode(Kooboo.Web.Url.UrlUtility.Combine(base.VirtualPath, TemplateFileName));
            }
        }
        #endregion

        string body = null;
        public virtual string Body
        {
            get
            {
                if (body == null && this.Site != null)
                {
                    body = IOUtility.ReadAsString(this.PhysicalTemplateFileName);
                }
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
        /// <param name="relativePaths">The relative paths. <example>{"site1","template","layout","template1"}</example></param>
        /// <returns>
        /// the remaining paths.<example>{"site1"}</example>
        /// </returns>
        public override IEnumerable<string> ParseObject(IEnumerable<string> relativePaths)
        {
            this.Name = relativePaths.Last();
            return relativePaths.Take(relativePaths.Count() - 3);
        }

        #region IPersistable Members
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
        private bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            private set { this.isDummy = value; }
        }

        void IPersistable.Init(IPersistable source)
        {
            this.IsDummy = false;
            var template = (Template)source;

            this.Name = template.Name;
            this.Site = template.Site;
        }
        void IPersistable.OnSaved()
        {
            this.IsDummy = false;

            if (Body == null)
            {
                Body = "";
            }
            IOUtility.SaveStringToFile(this.PhysicalTemplateFileName, Body);
        }

        public string DataFile
        {
            get { return Path.Combine(this.PhysicalPath, SettingFileName); }
        }


        public virtual void OnSaving()
        {

        }

        #endregion

        public override bool Exists()
        {
            return File.Exists(this.DataFile);
        }
    }
}
