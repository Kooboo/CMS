using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using System.Runtime.Serialization;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;
using Kooboo.Web.Url;

namespace Kooboo.CMS.Search.Models
{

    [DataContract]
    public class SearchSetting : IPersistable, IRepositoryElement
    {
        public class FolderSearchSettingPath : IPath
        {
            static string PATH_NAME = "Settings";
            public FolderSearchSettingPath(SearchSetting folderSearchSetting)
            {
                this.PhysicalPath = this.SettingFile = Path.Combine(GetBasePhysicalPath(folderSearchSetting.Repository), folderSearchSetting.FolderName + PathHelper.SettingFileExtension);
            }

            public static string GetBasePhysicalPath(Repository repository)
            {

                return Path.Combine(SearchDir.GetBasePhysicalPath(repository), PATH_NAME);
            }

            #region IPath Members

            public string PhysicalPath
            {
                get;
                private set;
            }

            public string VirtualPath
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            public string SettingFile
            {
                get;
                private set;
            }

            #endregion

            #region IPath Members


            public bool Exists()
            {
                return File.Exists(this.PhysicalPath);
            }

            #endregion

            #region IPath Members


            public void Rename(string newName)
            {
                IO.IOUtility.RenameDirectory(this.PhysicalPath, @newName);
            }

            #endregion
        }
        static SearchSetting()
        {
            PathFactory.Register(typeof(SearchSetting), typeof(FolderSearchSettingPath));
        }

        public SearchSetting()
        {

        }
        public SearchSetting(Repository repository, string folderName)
        {
            this.Repository = repository;
            this.FolderName = folderName;
        }

        public Repository Repository { get; set; }
        [DataMember(Order = 1)]
        public string FolderName { get; set; }

        private bool _useDefalut = true;
        [DataMember(Order = 4)]
        public bool IncludeAllFields
        {
            get
            {
                return _useDefalut;
            }
            set
            {
                _useDefalut = value;
            }
        }
        private List<string> fields = null;
        [DataMember(Order = 2)]
        public List<string> Fields
        {
            get
            {
                if (fields == null)
                {
                    fields = new List<string>();
                }
                return fields;
            }
            set
            {
                fields = value;
            }
        }

        [DataMember(Order = 3)]
        /// <summary>
        /// Gets or sets the URL format.
        /// </summary>
        /// <value>
        /// The URL format. <example>/articles/detail/{userkey}</example>
        /// </value>
        public string UrlFormat { get; set; }

        #region IPersistable
        private bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            this.Repository = ((SearchSetting)source).Repository;
            isDummy = false;
        }

        void IPersistable.OnSaved()
        {

        }

        void IPersistable.OnSaving()
        {

        }

        #endregion
        public string Name
        {
            get
            {
                return this.FolderName;
            }
            set
            {
                this.FolderName = value;
            }
        }
    }
}
