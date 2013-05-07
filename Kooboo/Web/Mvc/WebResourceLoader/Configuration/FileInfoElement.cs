#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Configuration;
using System.Security.Permissions;
using System.Web;

namespace Kooboo.Web.Mvc.WebResourceLoader.Configuration
{
    public sealed class FileInfoElement : ConfigurationElement
    {
        private static ConfigurationProperty fileName;
        private static ConfigurationProperty @if;
        private static ConfigurationPropertyCollection properties;

        internal FileInfoElement()
        {
            EnsureStaticPropertyBag();
        }

        private static ConfigurationPropertyCollection EnsureStaticPropertyBag()
        {
            if (properties == null)
            {
                fileName = new ConfigurationProperty("filename", typeof(string), null, null, new StringValidator(1), ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired);
                @if = new ConfigurationProperty("if", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

                properties = new ConfigurationPropertyCollection
                                  {
                                      fileName,
                                      @if
                                  };
            }
            return properties;
        }

        public FileInfoElement(string filename, string @if)
            : this()
        {
            Filename = filename;
            If = @if;
        }

        public override bool Equals(object namespaceInformation)
        {
            FileInfoElement info = namespaceInformation as FileInfoElement;
            return ((info != null) && (Filename == info.Filename));
        }

        public override int GetHashCode()
        {
            return Filename.GetHashCode();
        }

        [StringValidator(MinLength = 1), ConfigurationProperty("filename", IsRequired = true, IsKey = true)]
        public string Filename
        {
            get { return (string)base[fileName]; }
            set { base[fileName] = value; }
        }

        [ConfigurationProperty("if", DefaultValue = "")]
        public string If
        {
            get { return (string)base[@if]; }
            set { base[@if] = value; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return EnsureStaticPropertyBag(); }
        }
    }
}
