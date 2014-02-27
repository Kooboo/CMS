#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kooboo.CMS.Sites.Models
{
    public partial class Label : ISiteObject, IPersistable
    {
        #region .ctor
        public Label()
        {
            
        }
        public Label(Site site, string uuid)
        {
            this.Site = site;
            this.UUID = uuid;
        }
        public Label(Site site, string name, string value)
            : this(site, null, name, value)
        {          
        }
        public Label(Site site, string category, string name, string value)
            : this()
        {
            this.Site = site;
            this.Category = category;
            this.Name = name;
            this.Value = value;           
        }
        #endregion

        #region ISiteObject
        public Site Site
        {
            get;
            set;
        }
        #endregion

        #region IPersistable

        private bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            isDummy = false;
            this.Site = ((Label)source).Site;
        }

        void IPersistable.OnSaved()
        {
            isDummy = false;
        }

        void IPersistable.OnSaving()
        {

        }
        #endregion

        #region Comparable override
        public static bool operator ==(Label obj1, Label obj2)
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
        public static bool operator !=(Label obj1, Label obj2)
        {
            return !(obj1 == obj2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Label))
            {
                return false;
            }
            if (obj != null)
            {
                var o = (Label)obj;
                if (this.UUID.EqualsOrNullEmpty(o.UUID, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }

                if (this.Name.EqualsOrNullEmpty(o.Name, StringComparison.OrdinalIgnoreCase) && this.Category.EqualsOrNullEmpty(o.Category, StringComparison.OrdinalIgnoreCase))
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
        #endregion

    }
    #region Persistence data
    [DataContract]
    public partial class Label : IIdentifiable
    {

        [DataMember]
        public string UUID
        {
            get;
            set;
        }

        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Value { get; set; }
        [DataMember]
        public string Category { get; set; }

        [DataMember]
        public DateTime? UtcCreationDate { get; set; }

        [DataMember]
        public DateTime? UtcLastestModificationDate { get; set; }

        [DataMember]
        public string LastestEditor { get; set; }
    }
    #endregion
}
