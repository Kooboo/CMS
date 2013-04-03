using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Content.Query.Expressions;

namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    public static class FolderHelper
    {
        /// <summary>
        /// 合成完整名称
        /// </summary>
        /// <param name="names">The names.</param>
        /// <returns></returns>
        public static string CombineFullName(IEnumerable<string> names)
        {
            return string.Join("~", names.ToArray());
        }
        /// <summary>
        /// 分隔完整名称
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        public static IEnumerable<string> SplitFullName(string fullName)
        {
            return fullName.Split(new char[] { '~', '/' }, StringSplitOptions.RemoveEmptyEntries);
        }
        /// <summary>
        /// 根据完整名称取得目录对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>        
        public static T Parse<T>(Repository repository, string fullName)
            where T : Folder
        {
            var names = SplitFullName(fullName);
            if (typeof(T) == typeof(CMS.Content.Models.TextFolder))
            {
                return (T)((object)new TextFolder(repository, names));
            }
            else
            {
                return (T)((object)new MediaFolder(repository, names));
            }



        }
    }
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
            this.Repository = repository;
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
        [DataMember(Order = 1)]
        public string Name { get; set; }
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
            }
        }
    }

    public partial class Folder : IPersistable
    {
        #region IPersistable Members

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
            fullName = null;
        }

        private string fullName = null;
        public string FullName
        {
            get
            {
                if (fullName == null)
                {
                    fullName = FolderHelper.CombineFullName(this.NamePaths);
                }
                return fullName;
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

    /// <summary>
    /// 
    /// </summary>
    public class CategoryFolder
    {
        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>
        /// The name of the folder.
        /// </value>
        public string FolderName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [single choice].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [single choice]; otherwise, <c>false</c>.
        /// </value>
        public bool SingleChoice { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum OrderDirection
    {
        Ascending,
        Descending
    }
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class OrderSetting
    {
        public static readonly OrderSetting Default = new OrderSetting() { FieldName = "UtcCreationDate", Direction = OrderDirection.Descending };
        [DataMember(Order = 1)]
        public string FieldName { get; set; }
        [DataMember(Order = 2)]
        public OrderDirection Direction { get; set; }
    }
  
    [DataContract(Name = "TextFolder")]
    [KnownTypeAttribute(typeof(TextFolder))]
    public class TextFolder : Folder
    {
        public TextFolder() { }
        public TextFolder(Repository repository, string fullName) : base(repository, fullName) { }
        public TextFolder(Repository repository, string name, Folder parent) : base(repository, name, parent) { }
        public TextFolder(Repository repository, IEnumerable<string> namePath) : base(repository, namePath) { }
        [DataMember(Order = 9)]
        public string SchemaName { get; set; }

        [DataMember(Order = 10)]
        public List<CategoryFolder> Categories { get; set; }

        [Obsolete("Use Categories instead of CategoryFolders")]
        [DataMember(Order = 11)]
        public string[] CategoryFolders
        {
            get
            {
                return new string[0];
            }
            set
            {
                if (value != null && value.Length > 0)
                {
                    Categories = value.Select(it => new CategoryFolder() { FolderName = it, SingleChoice = false }).ToList();
                }
            }
        }

        [DataMember(Order = 13)]
        public string[] EmbeddedFolders { get; set; }

        [DataMember(Order = 14)]
        public string WorkflowName { get; set; }

        [DataMember(Order = 15)]
        public string[] Roles { get; set; }

        private OrderSetting orderSetting;
        [DataMember(Order = 16)]
        public OrderSetting OrderSetting
        {
            get
            {
                if (orderSetting == null)
                {
                    orderSetting = new Models.OrderSetting() { FieldName = OrderSetting.Default.FieldName, Direction = OrderSetting.Default.Direction };
                }
                return orderSetting;
            }
            set
            {
                orderSetting = value;
            }
        }

        private bool? visibleOnSidebarMenu;
        [DataMember(Order = 18)]
        public bool? VisibleOnSidebarMenu
        {
            get
            {
                if (visibleOnSidebarMenu.HasValue == false)
                {
                    return true;
                }
                return visibleOnSidebarMenu;
            }
            set
            {
                visibleOnSidebarMenu = value;
            }
        }
        private int pageSize = 50;
        [DataMember(Order = 20)]
        public int PageSize
        {
            get
            {
                if (pageSize == 0)
                {
                    pageSize = 50;
                }
                return pageSize;
            }
            set
            {
                pageSize = value;
            }
        }

        public bool EnabledWorkflow
        {
            get
            {
                return !string.IsNullOrEmpty(WorkflowName);
            }
        }
        //[DataMember(Order = 12)]
        //public IEnumerable<string> BinaryFolders { get; set; }

        public bool OrderBySequence
        {
            get
            {
                bool orderBySequence = false;
                if (OrderSetting != null)
                {
                    orderBySequence = OrderSetting.FieldName.EqualsOrNullEmpty("Sequence", StringComparison.OrdinalIgnoreCase);
                }
                return orderBySequence;
            }
        }
    }

    [DataContract(Name = "MediaFolder")]
    [KnownTypeAttribute(typeof(MediaFolder))]
    public class MediaFolder : Folder
    {
        public MediaFolder() { }
        public MediaFolder(Repository repository, string fullName) : base(repository, fullName) { }
        public MediaFolder(Repository repository, string name, Folder parent)
            : base(repository, name, parent)
        { }
        public MediaFolder(Repository repository, IEnumerable<string> namePath) : base(repository, namePath) { }

        [DataMember(Order = 5)]
        public string[] AllowedExtensions { get; set; }
    }
}
