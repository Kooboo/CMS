#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.Web.Url;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public class Parameter
    {
        public string Name { get; set; }
        public DataType DataType { get; set; }
        public object Value { get; set; }
    }
    [DataContract]
    public partial class View : Template
    {
        public View() { }
        public View(Site site, string name)
            : base(site, name)
        {
        }
        public View(string physicalPath)
            : base(physicalPath)
        {
        }
        protected override string TemplatePathName
        {
            get { return "Views"; }
        }

        private string fileExtension = string.Empty;
        [DataMember(Order = 1)]
        public override string FileExtension
        {
            get
            {
                if (string.IsNullOrEmpty(fileExtension))
                {
                    return Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(this.EngineName).ViewExtension;
                }
                return fileExtension;
            }
            set
            {
                fileExtension = value;
            }
        }

        private List<DataRuleSetting> dataRules = new List<DataRuleSetting>();
        [DataMember(Order = 2)]
        public List<DataRuleSetting> DataRules
        {
            get
            {
                if (dataRules == null)
                {
                    dataRules = new List<DataRuleSetting>();
                }
                return dataRules;
            }
            set
            {
                dataRules = value;
            }
        }

        private List<string> plugins = new List<string>();
        [DataMember(Order = 27)]//
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

        private List<Parameter> parameters = new List<Parameter>();
        [DataMember(Order = 29)]//
        public List<Parameter> Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        private List<DataSourceSelection> dataSources = new List<DataSourceSelection>();
        [DataMember(Order = 30)]
        public List<DataSourceSelection> DataSources
        {
            get
            {
                return dataSources ?? new List<DataSourceSelection>();
            }
            set
            {
                dataSources = value;
            }
        }

        [DataMember(Order = 31)]
        public FormSetting[] FormSettings { get; set; }

        public static string ParameterTemplateFileName = "ParameterTemplate.cshtml";
        public string ParameterTemplate
        {
            get
            {
                string templateFile = Path.Combine(this.PhysicalPath, ParameterTemplateFileName);
                if (File.Exists(templateFile))
                {
                    return UrlUtility.Combine(this.VirtualPath, ParameterTemplateFileName);
                }
                return null;
            }
        }
    }


    public partial class View : Template, IInheritable<View>
    {
        #region IInheritable<View> Members

        public View LastVersion()
        {
            return LastVersion(this.Site);
        }
        public View LastVersion(Site site)
        {
            var lastVersion = new View(site, this.Name);
            while (!lastVersion.Exists())
            {
                if (lastVersion.Site.Parent == null)
                {
                    break;
                }
                lastVersion = new View(lastVersion.Site.Parent, this.Name);
            }
            return lastVersion;
        }
        public bool HasParentVersion()
        {
            var parentSite = this.Site.Parent;
            while (parentSite != null)
            {
                var view = new View(parentSite, this.Name);
                if (view.Exists())
                {
                    return true;
                }
                parentSite = parentSite.Parent;
            }
            return false;
        }
        public bool IsLocalized(Site site)
        {
            return (new View(site, this.Name)).Exists();
        }
        #endregion

        /// <summary>
        /// 传入的参数跟默认参数合并
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public IDictionary<string, object> CombineDefaultParameters(IDictionary<string, object> parameters)
        {
            parameters = parameters != null ? new Dictionary<string, object>(parameters, StringComparer.OrdinalIgnoreCase) : new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (this.Parameters != null)
            {
                foreach (var p in this.Parameters)
                {
                    if (!parameters.ContainsKey(p.Name))
                    {
                        parameters[p.Name] = p.Value;
                    }
                }
            }
            return parameters;
        }

    }
}
