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
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Models
{
    public class WorkflowItem
    {
        public int Sequence { get; set; }
        public string RoleName { get; set; }
        public string DisplayName { get; set; }
    }
    [DataContract]
    public partial class Workflow : IRepositoryElement
    {
        public Workflow() { }
        public Workflow(Repository repository, string name)
        {
            this.Repository = repository;
            this.Name = name;
        }
        [DataMember(Order = 1)]
        public string Name { get; set; }

        [DataMember(Order = 2)]
        public WorkflowItem[] Items { get; set; }

        public Repository Repository
        {
            get;
            set;
        }
    }
    public partial class Workflow : IPersistable, IIdentifiable
    {
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
            isDummy = false;
            this.Repository = ((Workflow)source).Repository;
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

    [DataContract]
    public partial class PendingWorkflowItem : IRepositoryElement, IPersistable
    {
        [DataMember(Order = 1)]
        public string WorkflowName { get; set; }
        [DataMember(Order = 2)]
        public int WorkflowItemSequence { get; set; }
        [DataMember(Order = 3)]
        public string ItemDisplayName { get; set; }

        [DataMember(Order = 4)]
        public string RoleName { get; set; }
        [DataMember(Order = 5)]
        public string ContentFolder { get; set; }
        [DataMember(Order = 6)]
        public string ContentUUID { get; set; }
        [DataMember(Order = 9)]
        public string ContentSummary { get; set; }
        [DataMember(Order = 12)]
        public DateTime CreationUtcDate { get; set; }
        [DataMember(Order = 13)]
        public string CreationUser { get; set; }
        [DataMember(Order = 14)]
        public string PreviousComment { get; set; }

        #region IRepositoryElement

        public string Name
        {
            get
            {
                return this.ContentUUID;
            }
            set
            {
                this.ContentUUID = value;
            }
        }

        public Repository Repository
        {
            get;
            set;
        }
        #endregion

    }

    public partial class PendingWorkflowItem : IPersistable, IIdentifiable
    {
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
            isDummy = false;
            this.Repository = ((PendingWorkflowItem)source).Repository;
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

    [DataContract]
    public partial class WorkflowHistory : IRepositoryElement
    {
        public int Id { get; set; }
        [DataMember(Order = 1)]
        public string WorkflowName { get; set; }
        [DataMember(Order = 2)]
        public int WorkflowItemSequence { get; set; }
        [DataMember(Order = 4)]
        public string ItemDisplayName { get; set; }
        [DataMember(Order = 6)]
        public string ContentFolder { get; set; }
        [DataMember(Order = 8)]
        public string ContentUUID { get; set; }
        [DataMember(Order = 10)]
        public string ContentSummary { get; set; }
        [DataMember(Order = 12)]
        public string RoleName { get; set; }
        [DataMember(Order = 14)]
        public bool Passed { get; set; }
        [DataMember(Order = 16)]
        public bool Published { get; set; }
        [DataMember(Order = 18)]
        public DateTime ProcessingUtcDate { get; set; }
        [DataMember(Order = 20)]
        public string ProcessingUser { get; set; }
        [DataMember(Order = 21)]
        public bool Finished { get; set; }
        [DataMember(Order = 22)]
        public string Comment { get; set; }


        #region IRepositoryElement

        public string Name
        {
            get
            {
                return this.Id.ToString();
            }
            set
            {
            }
        }

        public Repository Repository
        {
            get;
            set;
        }
        #endregion
    }

    public partial class WorkflowHistory : IPersistable, IIdentifiable
    {
        #region IPersistable
        public string UUID
        {
            get
            {
                return this.Id.ToString();
            }
            set
            {
                this.Id = int.Parse(value);
            }
        }
        private bool isDummy = true;
        bool IPersistable.IsDummy
        {
            get { return isDummy; }
        }

        void IPersistable.Init(IPersistable source)
        {
            isDummy = false;
            this.Id = ((WorkflowHistory)source).Id;
            this.Repository = ((WorkflowHistory)source).Repository;
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
