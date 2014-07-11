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
using Kooboo.Common.Misc;
using Kooboo.Common;

namespace Kooboo.CMS.Content.Persistence.FileServerProvider
{ 
    [DataContract]
    public class FileServerProviderSettings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static FileServerProviderSettings instance = null;
        static FileServerProviderSettings()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<FileServerProviderSettings>(settingFile);
            }
            else
            {
                instance = new FileServerProviderSettings()
                {
                    Endpoint = "http://localhost:8626/",
                    AccountName = "devstoreaccount1",
                    AccountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw=="
                };
                Save(instance);
            }
        }
        public static void Save(FileServerProviderSettings instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            return Path.Combine(Settings.BinDirectory, "FileServerProviderSettings.config");
        }
        public static FileServerProviderSettings Instance
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
