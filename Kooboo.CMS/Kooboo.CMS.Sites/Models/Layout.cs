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
using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.Sites.Models
{
    [DataContract]
    public partial class Layout : Template
    {
        public Layout() { }
        public Layout(Site site, string name)
            : base(site, name)
        {
        }
        public Layout(string physicalPath)
            : base(physicalPath)
        {
        }
        protected override string TemplatePathName
        {
            get { return "Layouts"; }
        }
        private string fileExtension = string.Empty;
        [DataMember(Order = 1)]
        public override string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(fileExtension))
                {
                    return TemplateEngines.GetEngineByName(this.EngineName).GetFileExtensionForLayout();
                }
                return fileExtension;
            }
            set
            {
                fileExtension = value;
            }
        }

        private List<string> plugins = new List<string>();
        [DataMember(Order = 3)]//
        public List<string> Plugins
        {
            get
            {
                return plugins;
            }
            set
            {
                plugins = value;
            }
        }

        public override string Body
        {
            get
            {
                return base.Body;
            }
            set
            {
                if (base.Body != value)
                {
                    base.Body = value;
                    //Parse(value);
                }
            }
        }

        private void Parse(string body)
        {
            if (!string.IsNullOrEmpty(body))
            {
                var parsedPositions = TemplateEngines.GetEngineByName(EngineName).GetLayoutPositionParser().Parse(body);
                positions = new List<LayoutPosition>();
                foreach (var position in parsedPositions)
                {
                    positions.Add(new LayoutPosition() { ID = position });
                }
            }
        }

        private List<LayoutPosition> positions = new List<LayoutPosition>();
        /// <summary>
        /// public set method is only use in serialization.
        /// </summary>
        /// <value>The positions.</value>
        //[DataMember(Order = 2)] 
        public List<LayoutPosition> Positions
        {
            get
            {
                return this.positions;
            }
        }

        public override void OnSaving()
        {
            base.OnSaving();
            //Parse(this.Body);
        }        
    }


    public partial class Layout : IInheritable<Layout>
    {
        #region IInheritable<Layout> Members
        public Layout LastVersion()
        {
            return LastVersion(this.Site);
        }
        public Layout LastVersion(Site site)
        {
            var lastVersion = new Layout(site, this.Name);
            while (!lastVersion.Exists())
            {
                if (lastVersion.Site.Parent == null)
                {
                    break;
                }
                lastVersion = new Layout(lastVersion.Site.Parent, this.Name);
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
                var layout = new Layout(parentSite, this.Name);
                if (layout.Exists())
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
