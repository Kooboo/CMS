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
namespace Kooboo.CMS.Content.Persistence.MongoDB
{
	[DataContract]
    public class DatabaseSettings
    {
        private static DatabaseSettings instance = null;
        static DatabaseSettings()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<DatabaseSettings>(settingFile);
            }
            else
            {
                instance = new DatabaseSettings()
                {
                    SharingDatabase = false,
                    ConnectionString = "mongodb://localhost"
                };
                instance.Save();
            }
        }
        public void Save()
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize<DatabaseSettings>(this, settingFile);
        }
        private static string GetSettingFile()
        {
            var filePath = Path.Combine(Settings.BaseDirectory, "MongoDB.config");
            if (!File.Exists(filePath))
                filePath = Path.Combine(Settings.BinDirectory, "MongoDB.config");
            return filePath;
        }        
        /// <summary>
        /// http://www.mongodb.org/display/DOCS/Connections
        /// </summary>
        [DataMember]
        public string ConnectionString { get; set; }
        [DataMember]
		public bool SharingDatabase { get; set; }
		
        public static DatabaseSettings Instance
        {
            get
            {
                return instance;
            }
            set
            {
                instance = value;
                instance.Save();
            }
        }
    }
}
