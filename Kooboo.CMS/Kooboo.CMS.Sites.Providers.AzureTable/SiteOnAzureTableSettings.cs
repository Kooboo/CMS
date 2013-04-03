using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Kooboo.Runtime.Serialization;
namespace Kooboo.CMS.Sites.Providers.AzureTable
{ 
    [DataContract]
    public class SiteOnAzureTableSettings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static SiteOnAzureTableSettings instance = null;
        static SiteOnAzureTableSettings()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<SiteOnAzureTableSettings>(settingFile);
            }
            else
            {
                instance = new SiteOnAzureTableSettings()
                {
                    Endpoint = "http://127.0.0.1:10002/devstoreaccount1/",
                    AccountName = "devstoreaccount1",
                    AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
                };
                Save(instance);
            }
        }
        public static void Save(SiteOnAzureTableSettings instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            return Path.Combine(Kooboo.Settings.BinDirectory, "AzureTableSettings.config");
        }
        public static SiteOnAzureTableSettings Instance
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
        public string Endpoint { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AccountKey { get; set; }


    }
}
