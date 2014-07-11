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

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    [DataContract]
    public class MysqlSettings
    {
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static MysqlSettings instance = null;
        static MysqlSettings()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<MysqlSettings>(settingFile);
            }
            else
            {
                instance = new MysqlSettings()
                {
                    DEFAULT_CHARSET = "utf8",
                    ConnectionString = "Server=127.0.0.1;Database=Kooboo_CMS;Uid=root;Pwd=;"
                };
                Save(instance);
            }
        }
        public static void Save(MysqlSettings instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            var filePath = Path.Combine(Settings.BaseDirectory, "Mysql.config");
            if (!File.Exists(filePath))
                filePath = Path.Combine(Settings.BinDirectory, "Mysql.config");
            return filePath;
        }      
        public static MysqlSettings Instance
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
        public string DEFAULT_CHARSET { get; set; }
        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
        public bool EnableLogging { get; set; }
    }
}
