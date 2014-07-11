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
using Kooboo.CMS.Content.Models;
using System.Runtime.Serialization;
using Kooboo.CMS.Content.Models.Paths;
using System.IO;

using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.IO;

namespace Kooboo.CMS.Search.Models
{

    [DataContract]
    public class SearchSetting : IPersistable, IRepositoryElement, IIdentifiable
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
                IOUtility.RenameDirectory(this.PhysicalPath, @newName);
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

        [DataMember(Order = 4)]
        public string LinkPage { get; set; }

        [DataMember(Order = 5)]
        public Dictionary<string, string> RouteValueFields
        {
            get;
            set;
        }

        #region IPersistable
        public string UUID
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }
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
