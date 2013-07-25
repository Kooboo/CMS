#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.EntityFramework
{
    [DataContract]
    public class MemberEntitySetting
    {
        #region static
        [System.Xml.Serialization.XmlIgnoreAttribute]
        private static MemberEntitySetting instance = null;
        static MemberEntitySetting()
        {
            string settingFile = GetSettingFile();
            if (File.Exists(settingFile))
            {
                instance = DataContractSerializationHelper.Deserialize<MemberEntitySetting>(settingFile);
            }
            else
            {
                instance = new MemberEntitySetting()
                {
                    ConnectionString = "Server=.\\SQLExpress;Database=Kooboo_CMS; Trusted_Connection=Yes;",
                    ProviderInvariantName = "System.Data.SqlClient",
                    ProviderManifestToken = "2008"
                };
                Save(instance);
            }
        }
        public static void Save(MemberEntitySetting instance)
        {
            string settingFile = GetSettingFile();
            DataContractSerializationHelper.Serialize(instance, settingFile);
        }
        private static string GetSettingFile()
        {
            return Path.Combine(Kooboo.Settings.BinDirectory, "MemberEFProviderSetting.config");
        }
        public static MemberEntitySetting Instance
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
