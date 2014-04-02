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
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Name = "Folder")]
    [KnownTypeAttribute(typeof(Folder))]
    public partial class Folder : IRepositoryElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        public Folder()
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="name">The name.</param>
        /// <param name="parent">The parent.</param>
        public Folder(Repository repository, string name, Folder parent)
        {
            Repository = repository;
            this.Name = name;
            this.Parent = parent;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="fullName">The full name.</param>
        public Folder(Repository repository, string fullName) :
            this(repository, FolderHelper.SplitFullName(fullName))
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Folder"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="namePath">The name path.</param>
        public Folder(Repository repository, IEnumerable<string> namePath)
        {
            if (namePath == null || namePath.Count() < 1)
            {
                throw new ArgumentException("The folder name path is invalid.", "namePath");
            }
            if (repository != null)
            {
                this.Repository = new Repository(repository.Name);
            }

            this.Name = namePath.Last();
            if (namePath.Count() > 0)
            {
                foreach (var name in namePath.Take(namePath.Count() - 1))
                {
                    this.Parent = (Folder)Activator.CreateInstance(this.GetType(), repository, name, this.Parent);
                }
            }

        }
        public Repository Repository { get; set; }
        private string _name = "";
        [DataMember(Order = 1)]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                ResetNamePaths();
            }
        }
        [DataMember(Order = 3)]
        public string DisplayName { get; set; }
        [DataMember(Order = 5)]
        public DateTime UtcCreationDate { get; set; }
        [DataMember(Order = 7)]
        public string UserId { get; set; }



        private string[] namePaths = null;
        [DataMember(Order = 8)]
        public string[] NamePaths
        {
            get
            {
                if (namePaths == null)
                {
                    ResetNamePaths();
                }
                return namePaths;

            }
            set
            {
                namePaths = value;
                NamePathsChanged();
            }
        }
    }

    public partial class Folder : IPersistable, IIdentifiable
    {
        #region IPersistable Members
        public string UUID
        {
            get
            {
                return this.FullName;
            }
            set
            {
                this.NamePaths = FolderHelper.SplitFullName(value).ToArray();             
            }
        }
        bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        void IPersistable.Init(IPersistable source)
        {
            isDummy = false;
            this.Name = ((Folder)source).Name;
            this.Repository = ((Folder)source).Repository;
            this.Parent = ((Folder)source).Parent;
        }

        void IPersistable.OnSaved()
        {
            this.isDummy = false;
        }

        void IPersistable.OnSaving()
        {

        }

        #endregion

        #region override object
        public static bool operator ==(Folder obj1, Folder obj2)
        {
            if (object.Equals(obj1, obj2) == true)
            {
                return true;
            }
            if (object.Equals(obj1, null) == true || object.Equals(obj2, null) == true)
            {
                return false;
            }
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Folder obj1, Folder obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Folder))
            {
                return false;
            }
            if (obj != null)
            {
                var folder = (Folder)obj;
                if (this.Repository == folder.Repository && string.Compare(this.Name, folder.Name, true) == 0)
                {
                    return true;
                }
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return this.FriendlyText;
        }
        #endregion

        private Folder parent = null;
        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        public Folder Parent
        {
            get
            {
                return parent;
            }
            set
            {
                parent = value;
                ResetNamePaths();
            }
        }

        private void ResetNamePaths()
        {
            if (Parent == null)
            {
                namePaths = new[] { this.Name };
            }
            else
            {
                namePaths = Parent.NamePaths.Concat(new[] { this.Name }).ToArray();
            }
            
        }
        private void NamePathsChanged()
        {
            this._name = namePaths.Last();
            if (namePaths.Length > 1)
            {
                this.parent = new TextFolder(Repository, namePaths.Take(namePaths.Length - 1));
            }
            else
            {
                this.parent = null;
            }
        }
      
        public string FullName
        {
            get
            {
              
                  return FolderHelper.CombineFullName(this.NamePaths);                
            }
            //set
            //{
            //    fullName = value;
            //}
        }
        public string FriendlyName
        {
            get
            {
                return string.Join("/", NamePaths);
            }
        }
        public string FriendlyText
        {
            get
            {
                if (string.IsNullOrEmpty(this.DisplayName))
                {
                    return this.Name;
                }
                return this.DisplayName;
            }
        }
    }

}
