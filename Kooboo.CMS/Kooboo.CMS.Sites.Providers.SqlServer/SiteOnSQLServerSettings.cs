using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Kooboo.Runtime.Serialization;
namespace Kooboo.CMS.Sites.Providers.SqlServer
{
    [DataContract]
    public class SiteOnSQLServerSettings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static SiteOnSQLServerSettings instance = null;
        static SiteOnSQLServerSettings()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<SiteOnSQLServerSettings>(settingFile);
            }
            else
            {
                instance = new SiteOnSQLServerSettings()
                {
                    ConnectionString = "Server=.\\SQLExpress;Database=Kooboo_CMS; Trusted_Connection=Yes;"
                };
                Save(instance);
            }
        }
        public static void Save(SiteOnSQLServerSettings instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            return Path.Combine(Kooboo.Settings.BinDirectory, "SiteOnSQLServerSettings.config");
        }
        public static SiteOnSQLServerSettings Instance
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
        [DataMember]
        public string ConnectionString { get; set; }
    }
}
