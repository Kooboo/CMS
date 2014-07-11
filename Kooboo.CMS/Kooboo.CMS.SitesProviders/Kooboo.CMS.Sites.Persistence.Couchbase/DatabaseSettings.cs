
using Kooboo.Common;
using Kooboo.Common.Misc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Sites.Persistence.Couchbase
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
                    Username = "sa",
                    Password = "abc123",
                    BucketRAM = 100,//RamQuotaMB must be at least 100                  
                    Urls = new List<Uri>() { new Uri("http://127.0.0.1:8091/pools") }
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
            return Path.Combine(Settings.BinDirectory, "Kooboo.CMS.Sites.Persistence.Couchbase.config");
        }
        [DataMember]
        public int ReplicaNumber { get; set; }
        [DataMember]
        public bool ReplicaIndex { get; set; }
        #region Couchbase Config
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
        [DataMember]
        public string BucketPassword { get; set; }

        private string defaultBucketName;
        [DataMember]
        public string DefaultBucketName
        {
            get
            {
                if (string.IsNullOrEmpty(defaultBucketName))
                {
                    defaultBucketName = "Kooboo_CMS";
                }

                return defaultBucketName;
            }
            set
            {
                defaultBucketName = value;
            }
        }
        private int _bucketRAM;
        [DataMember]
        public int BucketRAM
        {
            get
            {
                return _bucketRAM < 100 ? 100 : _bucketRAM;//RamQuotaMB must be at least 100
            }
            set
            {
                _bucketRAM = value;
            }
        }

        private IList<Uri> _urls;
        [DataMember]
        public IList<Uri> Urls
        {
            get
            {
                return this._urls ?? (this._urls = new List<Uri>());
            }
            set
            {
                this._urls = value;
            }
        }
        #endregion

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
