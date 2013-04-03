using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using Kooboo.Runtime.Serialization;
namespace Kooboo.CMS.Content.Persistence.AzureBlobService
{
    public class ConnectionSetting
    {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
    [DataContract]
    public class AzureBlobServiceSettings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static AzureBlobServiceSettings instance = null;
        static AzureBlobServiceSettings()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<AzureBlobServiceSettings>(settingFile);
            }
            else
            {
                instance = new AzureBlobServiceSettings()
                {
                    Endpoint = "http://127.0.0.1:10000/devstoreaccount1/",
                    AccountName = "devstoreaccount1",
                    AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
                };
                Save(instance);
            }
        }
        public static void Save(AzureBlobServiceSettings instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            return Path.Combine(Kooboo.Settings.BinDirectory, "AzureBlobSettings.config");
        }
        public static AzureBlobServiceSettings Instance
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
