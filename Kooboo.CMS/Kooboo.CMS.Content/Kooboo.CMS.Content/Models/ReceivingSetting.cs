
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace Kooboo.CMS.Content.Models
{
    [DataContract]
    public partial class ReceivingSetting : IRepositoryElement
    {
        public Repository Repository
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        [DataMember(Order = 2)]
        public string SendingRepository { get; set; }

        [DataMember(Order = 3)]
        public string SendingFolder { get; set; }

        //[DataMember(Order = 7)]
        //public bool? Published { get; set; }

        //[DataMember(Order = 8)]
        //public ContentAction AcceptAction { get; set; }

        [DataMember(Order = 11)]
        public string ReceivingFolder { get; set; }

        //[DataMember(Order = 13)]
        //public bool? SetPublished { get; set; }
        [DataMember(Order = 13)]
        public bool KeepStatus { get; set; }



    }

    public partial class ReceivingSetting : IPersistable
    {
        #region IPersistable
        private bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            isDummy = false;
            this.Name = ((ReceivingSetting)source).Name;
            this.Repository = ((ReceivingSetting)source).Repository;
        }

        void IPersistable.OnSaved()
        {
            isDummy = false;
        }

        void IPersistable.OnSaving()
        {
        }
        #endregion
    }
}
