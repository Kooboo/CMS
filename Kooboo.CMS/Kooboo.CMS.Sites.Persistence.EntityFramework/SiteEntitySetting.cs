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
using Kooboo.Runtime.Serialization;
namespace Kooboo.CMS.Sites.Persistence.EntityFramework
{
    [DataContract]
    public class SiteEntitySetting
    {
        #region static
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static SiteEntitySetting instance = null;
        static SiteEntitySetting()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<SiteEntitySetting>(settingFile);
            }
            else
            {
                instance = new SiteEntitySetting()
                {
                    ConnectionString = "Server=.\\SQLExpress;Database=Kooboo_CMS; Trusted_Connection=Yes;",
                    ProviderInvariantName = "System.Data.SqlClient",
                    ProviderManifestToken = "2008"
                };
                Save(instance);
            }
        }
        public static void Save(SiteEntitySetting instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            return Path.Combine(Kooboo.Settings.BinDirectory, "Kooboo.CMS.Sites.Persistence.EntityFramework-DbContext.config");
        }
        public static SiteEntitySetting Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                Save(instance);
            }
        } 
        #endregion

        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
        public string ProviderInvariantName { get; set; }
        [DataMember]
        public string ProviderManifestToken { get; set; }
    }
}
