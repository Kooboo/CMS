#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence;
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.eCommerce.Persistence.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.eCommerce.Models.Catalog
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Category : IRelationEntity, ISiteObject
    {
        #region .ctor
        /// <summary>
        /// 
        /// </summary>
        public Category()
        {
            this.UtcCreationDate = DateTime.UtcNow;
            this.UtcUpdateDate = DateTime.UtcNow;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public virtual int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public virtual string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public virtual string Image
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        public virtual int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is published
        /// </summary>
        public virtual bool Published { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public virtual bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        /// <value>
        /// The display order.
        /// </value>
        public virtual int DisplayOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the UTC creation date.
        /// </summary>
        /// <value>
        /// The UTC creation date.
        /// </value>
        public virtual DateTime UtcCreationDate { get; set; }

        /// <summary>
        /// Gets or sets the UTC update date.
        /// </summary>
        /// <value>
        /// The UTC update date.
        /// </value>
        public virtual DateTime UtcUpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual string Site { get; set; }

        public virtual string FullName
        {
            get;
            set;
        }
        #endregion

        #region Relation

        /// <summary>
        /// Gets or sets the parent.
        /// </summary>
        /// <value>
        /// The parent.
        /// </value>
        public virtual Category Parent
        {
            get;
            set;
        }

        private ICollection<Category> _children;
        public virtual ICollection<Category> Children
        {
            get {
                return _children ?? (_children = new List<Category>());
            }
            set
            {
                _children = value;
            }
        }
        #endregion

    }

    /// <summary>
    /// 
    /// </summary>
    public partial class Category : ISaveObservable
    {
        /// <summary>
        /// Get or set ParentId
        /// </summary>
        public virtual int? ParentId
        {
            get
            {
                return Parent == null ? null : new Nullable<int>(Parent.Id);
            }
            set
            {
                this.Parent = value == null ? null : Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<ICategoryProvider>().QueryById(value.Value);
            }
        }

        /// <summary>
        /// Get or set ImageFile
        /// </summary>
        public virtual EntityFile ImageFile { get; set; }

        #region Methods
        void SyncFullName()
        {
            if (Parent == null)
            {
                this.FullName = this.Name;
            }
            else
            {
                this.FullName = Parent.FullName + " / " + this.Name;
            }
        }
        #endregion

        #region ISavable
        void ISaveObservable.OnSaving()
        {
            SyncFullName();
        }

        void ISaveObservable.OnSaved()
        {

        }
        #endregion
    }
}
