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

namespace Kooboo.CMS.Modules.CMIS.Models
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisFaultType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private enumServiceException typeField;

        private string codeField;

        private string messageField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public enumServiceException type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
                this.RaisePropertyChanged("code");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
                this.RaisePropertyChanged("message");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public enum enumServiceException
    {

        /// <remarks/>
        constraint,

        /// <remarks/>
        nameConstraintViolation,

        /// <remarks/>
        contentAlreadyExists,

        /// <remarks/>
        filterNotValid,

        /// <remarks/>
        invalidArgument,

        /// <remarks/>
        notSupported,

        /// <remarks/>
        objectNotFound,

        /// <remarks/>
        permissionDenied,

        /// <remarks/>
        runtime,

        /// <remarks/>
        storage,

        /// <remarks/>
        streamNotSupported,

        /// <remarks/>
        updateConflict,

        /// <remarks/>
        versioning,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisRenditionType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string streamIdField;

        private string mimetypeField;

        private string lengthField;

        private string kindField;

        private string titleField;

        private string heightField;

        private string widthField;

        private string renditionDocumentIdField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string streamId
        {
            get
            {
                return this.streamIdField;
            }
            set
            {
                this.streamIdField = value;
                this.RaisePropertyChanged("streamId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string mimetype
        {
            get
            {
                return this.mimetypeField;
            }
            set
            {
                this.mimetypeField = value;
                this.RaisePropertyChanged("mimetype");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string length
        {
            get
            {
                return this.lengthField;
            }
            set
            {
                this.lengthField = value;
                this.RaisePropertyChanged("length");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string kind
        {
            get
            {
                return this.kindField;
            }
            set
            {
                this.kindField = value;
                this.RaisePropertyChanged("kind");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
                this.RaisePropertyChanged("title");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 5)]
        public string height
        {
            get
            {
                return this.heightField;
            }
            set
            {
                this.heightField = value;
                this.RaisePropertyChanged("height");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 6)]
        public string width
        {
            get
            {
                return this.widthField;
            }
            set
            {
                this.widthField = value;
                this.RaisePropertyChanged("width");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string renditionDocumentId
        {
            get
            {
                return this.renditionDocumentIdField;
            }
            set
            {
                this.renditionDocumentIdField = value;
                this.RaisePropertyChanged("renditionDocumentId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 8)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisListOfIdsType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string[] idField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("id", Order = 0)]
        public string[] id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisAccessControlPrincipalType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string principalIdField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string principalId
        {
            get
            {
                return this.principalIdField;
            }
            set
            {
                this.principalIdField = value;
                this.RaisePropertyChanged("principalId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisAccessControlEntryType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisAccessControlPrincipalType principalField;

        private string[] permissionField;

        private bool directField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisAccessControlPrincipalType principal
        {
            get
            {
                return this.principalField;
            }
            set
            {
                this.principalField = value;
                this.RaisePropertyChanged("principal");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("permission", Order = 1)]
        public string[] permission
        {
            get
            {
                return this.permissionField;
            }
            set
            {
                this.permissionField = value;
                this.RaisePropertyChanged("permission");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool direct
        {
            get
            {
                return this.directField;
            }
            set
            {
                this.directField = value;
                this.RaisePropertyChanged("direct");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisAccessControlListType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisAccessControlEntryType[] permissionField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("permission", Order = 0)]
        public cmisAccessControlEntryType[] permission
        {
            get
            {
                return this.permissionField;
            }
            set
            {
                this.permissionField = value;
                this.RaisePropertyChanged("permission");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChangeEventType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private enumTypeOfChanges changeTypeField;

        private System.DateTime changeTimeField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public enumTypeOfChanges changeType
        {
            get
            {
                return this.changeTypeField;
            }
            set
            {
                this.changeTypeField = value;
                this.RaisePropertyChanged("changeType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public System.DateTime changeTime
        {
            get
            {
                return this.changeTimeField;
            }
            set
            {
                this.changeTimeField = value;
                this.RaisePropertyChanged("changeTime");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumTypeOfChanges
    {

        /// <remarks/>
        created,

        /// <remarks/>
        updated,

        /// <remarks/>
        deleted,

        /// <remarks/>
        security,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisAllowableActionsType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private bool canDeleteObjectField;

        private bool canDeleteObjectFieldSpecified;

        private bool canUpdatePropertiesField;

        private bool canUpdatePropertiesFieldSpecified;

        private bool canGetFolderTreeField;

        private bool canGetFolderTreeFieldSpecified;

        private bool canGetPropertiesField;

        private bool canGetPropertiesFieldSpecified;

        private bool canGetObjectRelationshipsField;

        private bool canGetObjectRelationshipsFieldSpecified;

        private bool canGetObjectParentsField;

        private bool canGetObjectParentsFieldSpecified;

        private bool canGetFolderParentField;

        private bool canGetFolderParentFieldSpecified;

        private bool canGetDescendantsField;

        private bool canGetDescendantsFieldSpecified;

        private bool canMoveObjectField;

        private bool canMoveObjectFieldSpecified;

        private bool canDeleteContentStreamField;

        private bool canDeleteContentStreamFieldSpecified;

        private bool canCheckOutField;

        private bool canCheckOutFieldSpecified;

        private bool canCancelCheckOutField;

        private bool canCancelCheckOutFieldSpecified;

        private bool canCheckInField;

        private bool canCheckInFieldSpecified;

        private bool canSetContentStreamField;

        private bool canSetContentStreamFieldSpecified;

        private bool canGetAllVersionsField;

        private bool canGetAllVersionsFieldSpecified;

        private bool canAddObjectToFolderField;

        private bool canAddObjectToFolderFieldSpecified;

        private bool canRemoveObjectFromFolderField;

        private bool canRemoveObjectFromFolderFieldSpecified;

        private bool canGetContentStreamField;

        private bool canGetContentStreamFieldSpecified;

        private bool canApplyPolicyField;

        private bool canApplyPolicyFieldSpecified;

        private bool canGetAppliedPoliciesField;

        private bool canGetAppliedPoliciesFieldSpecified;

        private bool canRemovePolicyField;

        private bool canRemovePolicyFieldSpecified;

        private bool canGetChildrenField;

        private bool canGetChildrenFieldSpecified;

        private bool canCreateDocumentField;

        private bool canCreateDocumentFieldSpecified;

        private bool canCreateFolderField;

        private bool canCreateFolderFieldSpecified;

        private bool canCreateRelationshipField;

        private bool canCreateRelationshipFieldSpecified;

        private bool canDeleteTreeField;

        private bool canDeleteTreeFieldSpecified;

        private bool canGetRenditionsField;

        private bool canGetRenditionsFieldSpecified;

        private bool canGetACLField;

        private bool canGetACLFieldSpecified;

        private bool canApplyACLField;

        private bool canApplyACLFieldSpecified;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool canDeleteObject
        {
            get
            {
                return this.canDeleteObjectField;
            }
            set
            {
                this.canDeleteObjectField = value;
                this.RaisePropertyChanged("canDeleteObject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canDeleteObjectSpecified
        {
            get
            {
                return this.canDeleteObjectFieldSpecified;
            }
            set
            {
                this.canDeleteObjectFieldSpecified = value;
                this.RaisePropertyChanged("canDeleteObjectSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool canUpdateProperties
        {
            get
            {
                return this.canUpdatePropertiesField;
            }
            set
            {
                this.canUpdatePropertiesField = value;
                this.RaisePropertyChanged("canUpdateProperties");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canUpdatePropertiesSpecified
        {
            get
            {
                return this.canUpdatePropertiesFieldSpecified;
            }
            set
            {
                this.canUpdatePropertiesFieldSpecified = value;
                this.RaisePropertyChanged("canUpdatePropertiesSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public bool canGetFolderTree
        {
            get
            {
                return this.canGetFolderTreeField;
            }
            set
            {
                this.canGetFolderTreeField = value;
                this.RaisePropertyChanged("canGetFolderTree");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetFolderTreeSpecified
        {
            get
            {
                return this.canGetFolderTreeFieldSpecified;
            }
            set
            {
                this.canGetFolderTreeFieldSpecified = value;
                this.RaisePropertyChanged("canGetFolderTreeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public bool canGetProperties
        {
            get
            {
                return this.canGetPropertiesField;
            }
            set
            {
                this.canGetPropertiesField = value;
                this.RaisePropertyChanged("canGetProperties");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetPropertiesSpecified
        {
            get
            {
                return this.canGetPropertiesFieldSpecified;
            }
            set
            {
                this.canGetPropertiesFieldSpecified = value;
                this.RaisePropertyChanged("canGetPropertiesSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool canGetObjectRelationships
        {
            get
            {
                return this.canGetObjectRelationshipsField;
            }
            set
            {
                this.canGetObjectRelationshipsField = value;
                this.RaisePropertyChanged("canGetObjectRelationships");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetObjectRelationshipsSpecified
        {
            get
            {
                return this.canGetObjectRelationshipsFieldSpecified;
            }
            set
            {
                this.canGetObjectRelationshipsFieldSpecified = value;
                this.RaisePropertyChanged("canGetObjectRelationshipsSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool canGetObjectParents
        {
            get
            {
                return this.canGetObjectParentsField;
            }
            set
            {
                this.canGetObjectParentsField = value;
                this.RaisePropertyChanged("canGetObjectParents");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetObjectParentsSpecified
        {
            get
            {
                return this.canGetObjectParentsFieldSpecified;
            }
            set
            {
                this.canGetObjectParentsFieldSpecified = value;
                this.RaisePropertyChanged("canGetObjectParentsSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public bool canGetFolderParent
        {
            get
            {
                return this.canGetFolderParentField;
            }
            set
            {
                this.canGetFolderParentField = value;
                this.RaisePropertyChanged("canGetFolderParent");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetFolderParentSpecified
        {
            get
            {
                return this.canGetFolderParentFieldSpecified;
            }
            set
            {
                this.canGetFolderParentFieldSpecified = value;
                this.RaisePropertyChanged("canGetFolderParentSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public bool canGetDescendants
        {
            get
            {
                return this.canGetDescendantsField;
            }
            set
            {
                this.canGetDescendantsField = value;
                this.RaisePropertyChanged("canGetDescendants");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetDescendantsSpecified
        {
            get
            {
                return this.canGetDescendantsFieldSpecified;
            }
            set
            {
                this.canGetDescendantsFieldSpecified = value;
                this.RaisePropertyChanged("canGetDescendantsSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public bool canMoveObject
        {
            get
            {
                return this.canMoveObjectField;
            }
            set
            {
                this.canMoveObjectField = value;
                this.RaisePropertyChanged("canMoveObject");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canMoveObjectSpecified
        {
            get
            {
                return this.canMoveObjectFieldSpecified;
            }
            set
            {
                this.canMoveObjectFieldSpecified = value;
                this.RaisePropertyChanged("canMoveObjectSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public bool canDeleteContentStream
        {
            get
            {
                return this.canDeleteContentStreamField;
            }
            set
            {
                this.canDeleteContentStreamField = value;
                this.RaisePropertyChanged("canDeleteContentStream");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canDeleteContentStreamSpecified
        {
            get
            {
                return this.canDeleteContentStreamFieldSpecified;
            }
            set
            {
                this.canDeleteContentStreamFieldSpecified = value;
                this.RaisePropertyChanged("canDeleteContentStreamSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public bool canCheckOut
        {
            get
            {
                return this.canCheckOutField;
            }
            set
            {
                this.canCheckOutField = value;
                this.RaisePropertyChanged("canCheckOut");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canCheckOutSpecified
        {
            get
            {
                return this.canCheckOutFieldSpecified;
            }
            set
            {
                this.canCheckOutFieldSpecified = value;
                this.RaisePropertyChanged("canCheckOutSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool canCancelCheckOut
        {
            get
            {
                return this.canCancelCheckOutField;
            }
            set
            {
                this.canCancelCheckOutField = value;
                this.RaisePropertyChanged("canCancelCheckOut");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canCancelCheckOutSpecified
        {
            get
            {
                return this.canCancelCheckOutFieldSpecified;
            }
            set
            {
                this.canCancelCheckOutFieldSpecified = value;
                this.RaisePropertyChanged("canCancelCheckOutSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool canCheckIn
        {
            get
            {
                return this.canCheckInField;
            }
            set
            {
                this.canCheckInField = value;
                this.RaisePropertyChanged("canCheckIn");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canCheckInSpecified
        {
            get
            {
                return this.canCheckInFieldSpecified;
            }
            set
            {
                this.canCheckInFieldSpecified = value;
                this.RaisePropertyChanged("canCheckInSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool canSetContentStream
        {
            get
            {
                return this.canSetContentStreamField;
            }
            set
            {
                this.canSetContentStreamField = value;
                this.RaisePropertyChanged("canSetContentStream");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canSetContentStreamSpecified
        {
            get
            {
                return this.canSetContentStreamFieldSpecified;
            }
            set
            {
                this.canSetContentStreamFieldSpecified = value;
                this.RaisePropertyChanged("canSetContentStreamSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public bool canGetAllVersions
        {
            get
            {
                return this.canGetAllVersionsField;
            }
            set
            {
                this.canGetAllVersionsField = value;
                this.RaisePropertyChanged("canGetAllVersions");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetAllVersionsSpecified
        {
            get
            {
                return this.canGetAllVersionsFieldSpecified;
            }
            set
            {
                this.canGetAllVersionsFieldSpecified = value;
                this.RaisePropertyChanged("canGetAllVersionsSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public bool canAddObjectToFolder
        {
            get
            {
                return this.canAddObjectToFolderField;
            }
            set
            {
                this.canAddObjectToFolderField = value;
                this.RaisePropertyChanged("canAddObjectToFolder");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canAddObjectToFolderSpecified
        {
            get
            {
                return this.canAddObjectToFolderFieldSpecified;
            }
            set
            {
                this.canAddObjectToFolderFieldSpecified = value;
                this.RaisePropertyChanged("canAddObjectToFolderSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 16)]
        public bool canRemoveObjectFromFolder
        {
            get
            {
                return this.canRemoveObjectFromFolderField;
            }
            set
            {
                this.canRemoveObjectFromFolderField = value;
                this.RaisePropertyChanged("canRemoveObjectFromFolder");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canRemoveObjectFromFolderSpecified
        {
            get
            {
                return this.canRemoveObjectFromFolderFieldSpecified;
            }
            set
            {
                this.canRemoveObjectFromFolderFieldSpecified = value;
                this.RaisePropertyChanged("canRemoveObjectFromFolderSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 17)]
        public bool canGetContentStream
        {
            get
            {
                return this.canGetContentStreamField;
            }
            set
            {
                this.canGetContentStreamField = value;
                this.RaisePropertyChanged("canGetContentStream");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetContentStreamSpecified
        {
            get
            {
                return this.canGetContentStreamFieldSpecified;
            }
            set
            {
                this.canGetContentStreamFieldSpecified = value;
                this.RaisePropertyChanged("canGetContentStreamSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 18)]
        public bool canApplyPolicy
        {
            get
            {
                return this.canApplyPolicyField;
            }
            set
            {
                this.canApplyPolicyField = value;
                this.RaisePropertyChanged("canApplyPolicy");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canApplyPolicySpecified
        {
            get
            {
                return this.canApplyPolicyFieldSpecified;
            }
            set
            {
                this.canApplyPolicyFieldSpecified = value;
                this.RaisePropertyChanged("canApplyPolicySpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 19)]
        public bool canGetAppliedPolicies
        {
            get
            {
                return this.canGetAppliedPoliciesField;
            }
            set
            {
                this.canGetAppliedPoliciesField = value;
                this.RaisePropertyChanged("canGetAppliedPolicies");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetAppliedPoliciesSpecified
        {
            get
            {
                return this.canGetAppliedPoliciesFieldSpecified;
            }
            set
            {
                this.canGetAppliedPoliciesFieldSpecified = value;
                this.RaisePropertyChanged("canGetAppliedPoliciesSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 20)]
        public bool canRemovePolicy
        {
            get
            {
                return this.canRemovePolicyField;
            }
            set
            {
                this.canRemovePolicyField = value;
                this.RaisePropertyChanged("canRemovePolicy");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canRemovePolicySpecified
        {
            get
            {
                return this.canRemovePolicyFieldSpecified;
            }
            set
            {
                this.canRemovePolicyFieldSpecified = value;
                this.RaisePropertyChanged("canRemovePolicySpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 21)]
        public bool canGetChildren
        {
            get
            {
                return this.canGetChildrenField;
            }
            set
            {
                this.canGetChildrenField = value;
                this.RaisePropertyChanged("canGetChildren");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetChildrenSpecified
        {
            get
            {
                return this.canGetChildrenFieldSpecified;
            }
            set
            {
                this.canGetChildrenFieldSpecified = value;
                this.RaisePropertyChanged("canGetChildrenSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 22)]
        public bool canCreateDocument
        {
            get
            {
                return this.canCreateDocumentField;
            }
            set
            {
                this.canCreateDocumentField = value;
                this.RaisePropertyChanged("canCreateDocument");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canCreateDocumentSpecified
        {
            get
            {
                return this.canCreateDocumentFieldSpecified;
            }
            set
            {
                this.canCreateDocumentFieldSpecified = value;
                this.RaisePropertyChanged("canCreateDocumentSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 23)]
        public bool canCreateFolder
        {
            get
            {
                return this.canCreateFolderField;
            }
            set
            {
                this.canCreateFolderField = value;
                this.RaisePropertyChanged("canCreateFolder");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canCreateFolderSpecified
        {
            get
            {
                return this.canCreateFolderFieldSpecified;
            }
            set
            {
                this.canCreateFolderFieldSpecified = value;
                this.RaisePropertyChanged("canCreateFolderSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 24)]
        public bool canCreateRelationship
        {
            get
            {
                return this.canCreateRelationshipField;
            }
            set
            {
                this.canCreateRelationshipField = value;
                this.RaisePropertyChanged("canCreateRelationship");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canCreateRelationshipSpecified
        {
            get
            {
                return this.canCreateRelationshipFieldSpecified;
            }
            set
            {
                this.canCreateRelationshipFieldSpecified = value;
                this.RaisePropertyChanged("canCreateRelationshipSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 25)]
        public bool canDeleteTree
        {
            get
            {
                return this.canDeleteTreeField;
            }
            set
            {
                this.canDeleteTreeField = value;
                this.RaisePropertyChanged("canDeleteTree");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canDeleteTreeSpecified
        {
            get
            {
                return this.canDeleteTreeFieldSpecified;
            }
            set
            {
                this.canDeleteTreeFieldSpecified = value;
                this.RaisePropertyChanged("canDeleteTreeSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 26)]
        public bool canGetRenditions
        {
            get
            {
                return this.canGetRenditionsField;
            }
            set
            {
                this.canGetRenditionsField = value;
                this.RaisePropertyChanged("canGetRenditions");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetRenditionsSpecified
        {
            get
            {
                return this.canGetRenditionsFieldSpecified;
            }
            set
            {
                this.canGetRenditionsFieldSpecified = value;
                this.RaisePropertyChanged("canGetRenditionsSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 27)]
        public bool canGetACL
        {
            get
            {
                return this.canGetACLField;
            }
            set
            {
                this.canGetACLField = value;
                this.RaisePropertyChanged("canGetACL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canGetACLSpecified
        {
            get
            {
                return this.canGetACLFieldSpecified;
            }
            set
            {
                this.canGetACLFieldSpecified = value;
                this.RaisePropertyChanged("canGetACLSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 28)]
        public bool canApplyACL
        {
            get
            {
                return this.canApplyACLField;
            }
            set
            {
                this.canApplyACLField = value;
                this.RaisePropertyChanged("canApplyACL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool canApplyACLSpecified
        {
            get
            {
                return this.canApplyACLFieldSpecified;
            }
            set
            {
                this.canApplyACLFieldSpecified = value;
                this.RaisePropertyChanged("canApplyACLSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 29)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyUri))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyString))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyHtml))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyDecimal))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyDateTime))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyInteger))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyId))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyBoolean))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisProperty : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string propertyDefinitionIdField;

        private string localNameField;

        private string displayNameField;

        private string queryNameField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string propertyDefinitionId
        {
            get
            {
                return this.propertyDefinitionIdField;
            }
            set
            {
                this.propertyDefinitionIdField = value;
                this.RaisePropertyChanged("propertyDefinitionId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string localName
        {
            get
            {
                return this.localNameField;
            }
            set
            {
                this.localNameField = value;
                this.RaisePropertyChanged("localName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
                this.RaisePropertyChanged("displayName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string queryName
        {
            get
            {
                return this.queryNameField;
            }
            set
            {
                this.queryNameField = value;
                this.RaisePropertyChanged("queryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyUri : cmisProperty
    {

        private string[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", DataType = "anyURI", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyString : cmisProperty
    {

        private string[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyHtml : cmisProperty
    {

        private string[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyDecimal : cmisProperty
    {

        private decimal[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public decimal[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyDateTime : cmisProperty
    {

        private System.DateTime[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public System.DateTime[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyInteger : cmisProperty
    {

        private string[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", DataType = "integer", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyId : cmisProperty
    {

        private string[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyBoolean : cmisProperty
    {

        private bool[] valueField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public bool[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertiesType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisProperty[] itemsField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("propertyBoolean", typeof(cmisPropertyBoolean), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyDateTime", typeof(cmisPropertyDateTime), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyDecimal", typeof(cmisPropertyDecimal), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyHtml", typeof(cmisPropertyHtml), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyId", typeof(cmisPropertyId), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyInteger", typeof(cmisPropertyInteger), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyString", typeof(cmisPropertyString), IsNullable = true, Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("propertyUri", typeof(cmisPropertyUri), IsNullable = true, Order = 0)]
        public cmisProperty[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisObjectType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisPropertiesType propertiesField;

        private cmisAllowableActionsType allowableActionsField;

        private cmisObjectType[] relationshipField;

        private cmisChangeEventType changeEventInfoField;

        private cmisAccessControlListType aclField;

        private bool exactACLField;

        private bool exactACLFieldSpecified;

        private cmisListOfIdsType policyIdsField;

        private cmisRenditionType[] renditionField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertiesType properties
        {
            get
            {
                return this.propertiesField;
            }
            set
            {
                this.propertiesField = value;
                this.RaisePropertyChanged("properties");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public cmisAllowableActionsType allowableActions
        {
            get
            {
                return this.allowableActionsField;
            }
            set
            {
                this.allowableActionsField = value;
                this.RaisePropertyChanged("allowableActions");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("relationship", Order = 2)]
        public cmisObjectType[] relationship
        {
            get
            {
                return this.relationshipField;
            }
            set
            {
                this.relationshipField = value;
                this.RaisePropertyChanged("relationship");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public cmisChangeEventType changeEventInfo
        {
            get
            {
                return this.changeEventInfoField;
            }
            set
            {
                this.changeEventInfoField = value;
                this.RaisePropertyChanged("changeEventInfo");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public cmisAccessControlListType acl
        {
            get
            {
                return this.aclField;
            }
            set
            {
                this.aclField = value;
                this.RaisePropertyChanged("acl");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool exactACL
        {
            get
            {
                return this.exactACLField;
            }
            set
            {
                this.exactACLField = value;
                this.RaisePropertyChanged("exactACL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool exactACLSpecified
        {
            get
            {
                return this.exactACLFieldSpecified;
            }
            set
            {
                this.exactACLFieldSpecified = value;
                this.RaisePropertyChanged("exactACLSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public cmisListOfIdsType policyIds
        {
            get
            {
                return this.policyIdsField;
            }
            set
            {
                this.policyIdsField = value;
                this.RaisePropertyChanged("policyIds");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("rendition", Order = 7)]
        public cmisRenditionType[] rendition
        {
            get
            {
                return this.renditionField;
            }
            set
            {
                this.renditionField = value;
                this.RaisePropertyChanged("rendition");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 8)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisObjectListType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisObjectType[] objectsField;

        private bool hasMoreItemsField;

        private string numItemsField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("objects", Order = 0)]
        public cmisObjectType[] objects
        {
            get
            {
                return this.objectsField;
            }
            set
            {
                this.objectsField = value;
                this.RaisePropertyChanged("objects");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool hasMoreItems
        {
            get
            {
                return this.hasMoreItemsField;
            }
            set
            {
                this.hasMoreItemsField = value;
                this.RaisePropertyChanged("hasMoreItems");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string numItems
        {
            get
            {
                return this.numItemsField;
            }
            set
            {
                this.numItemsField = value;
                this.RaisePropertyChanged("numItems");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisExtensionType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/ws/200908/", ConfigurationName = "CMISWebServicesReference.DiscoveryServicePort")]
    internal interface DiscoveryServicePort
    {

        // CODEGEN: Generating message contract since the wrapper namespace (http://docs.oasis-open.org/ns/cmis/messaging/200908/) of message queryRequest does not match the default value (http://docs.oasis-open.org/ns/cmis/ws/200908/)
        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(cmisFaultType), Action = "", Name = "cmisFault", Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(cmisProperty))]
        queryResponse query(queryRequest request);

        // CODEGEN: Generating message contract since the wrapper namespace (http://docs.oasis-open.org/ns/cmis/messaging/200908/) of message getContentChangesRequest does not match the default value (http://docs.oasis-open.org/ns/cmis/ws/200908/)
        [System.ServiceModel.OperationContractAttribute(Action = "", ReplyAction = "*")]
        [System.ServiceModel.FaultContractAttribute(typeof(cmisFaultType), Action = "", Name = "cmisFault", Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults = true)]
        [System.ServiceModel.ServiceKnownTypeAttribute(typeof(cmisProperty))]
        getContentChangesResponse getContentChanges(getContentChangesRequest request);
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumIncludeRelationships
    {

        /// <remarks/>
        none,

        /// <remarks/>
        source,

        /// <remarks/>
        target,

        /// <remarks/>
        both,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "query", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class queryRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string statement;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> searchAllVersions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string skipCount;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "", Order = 9)]
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr;

        public queryRequest()
        {
        }

        public queryRequest(string repositoryId, string statement, System.Nullable<bool> searchAllVersions, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, string maxItems, string skipCount, cmisExtensionType extension, System.Xml.XmlAttribute[] AnyAttr)
        {
            this.repositoryId = repositoryId;
            this.statement = statement;
            this.searchAllVersions = searchAllVersions;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.maxItems = maxItems;
            this.skipCount = skipCount;
            this.extension = extension;
            this.AnyAttr = AnyAttr;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "queryResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class queryResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectListType objects;

        public queryResponse()
        {
        }

        public queryResponse(cmisObjectListType objects)
        {
            this.objects = objects;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getContentChanges", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getContentChangesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string changeLogToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeProperties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePolicyIds;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeACL;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getContentChangesRequest()
        {
        }

        public getContentChangesRequest(string repositoryId, string changeLogToken, System.Nullable<bool> includeProperties, string filter, System.Nullable<bool> includePolicyIds, System.Nullable<bool> includeACL, string maxItems, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.changeLogToken = changeLogToken;
            this.includeProperties = includeProperties;
            this.filter = filter;
            this.includePolicyIds = includePolicyIds;
            this.includeACL = includeACL;
            this.maxItems = maxItems;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getContentChangesResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getContentChangesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectListType objects;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string changeLogToken;

        public getContentChangesResponse()
        {
        }

        public getContentChangesResponse(cmisObjectListType objects, string changeLogToken)
        {
            this.objects = objects;
            this.changeLogToken = changeLogToken;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "addObjectToFolder", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class addObjectToFolderRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        public bool allVersions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public addObjectToFolderRequest()
        {
        }

        public addObjectToFolderRequest(string repositoryId, string objectId, string folderId, bool allVersions, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.folderId = folderId;
            this.allVersions = allVersions;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "addObjectToFolderResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class addObjectToFolderResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public addObjectToFolderResponse()
        {
        }

        public addObjectToFolderResponse(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "removeObjectFromFolder", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class removeObjectFromFolderRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public removeObjectFromFolderRequest()
        {
        }

        public removeObjectFromFolderRequest(string repositoryId, string objectId, string folderId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.folderId = folderId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "removeObjectFromFolderResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class removeObjectFromFolderResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public removeObjectFromFolderResponse()
        {
        }

        public removeObjectFromFolderResponse(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisObjectInFolderContainerType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisObjectInFolderType objectInFolderField;

        private cmisObjectInFolderContainerType[] childrenField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisObjectInFolderType objectInFolder
        {
            get
            {
                return this.objectInFolderField;
            }
            set
            {
                this.objectInFolderField = value;
                this.RaisePropertyChanged("objectInFolder");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("children", Order = 1)]
        public cmisObjectInFolderContainerType[] children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
                this.RaisePropertyChanged("children");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisObjectInFolderType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisObjectType objectField;

        private string pathSegmentField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisObjectType @object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
                this.RaisePropertyChanged("object");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string pathSegment
        {
            get
            {
                return this.pathSegmentField;
            }
            set
            {
                this.pathSegmentField = value;
                this.RaisePropertyChanged("pathSegment");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getDescendants", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getDescendantsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string depth;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePathSegment;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getDescendantsRequest()
        {
        }

        public getDescendantsRequest(string repositoryId, string folderId, string depth, string filter, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includePathSegment, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.folderId = folderId;
            this.depth = depth;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includePathSegment = includePathSegment;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getDescendantsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getDescendantsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("objects")]
        public cmisObjectInFolderContainerType[] objects;

        public getDescendantsResponse()
        {
        }

        public getDescendantsResponse(cmisObjectInFolderContainerType[] objects)
        {
            this.objects = objects;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisObjectInFolderListType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisObjectInFolderType[] objectsField;

        private bool hasMoreItemsField;

        private string numItemsField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("objects", Order = 0)]
        public cmisObjectInFolderType[] objects
        {
            get
            {
                return this.objectsField;
            }
            set
            {
                this.objectsField = value;
                this.RaisePropertyChanged("objects");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool hasMoreItems
        {
            get
            {
                return this.hasMoreItemsField;
            }
            set
            {
                this.hasMoreItemsField = value;
                this.RaisePropertyChanged("hasMoreItems");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string numItems
        {
            get
            {
                return this.numItemsField;
            }
            set
            {
                this.numItemsField = value;
                this.RaisePropertyChanged("numItems");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getChildren", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getChildrenRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string orderBy;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePathSegment;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 9)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string skipCount;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 10)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getChildrenRequest()
        {
        }

        public getChildrenRequest(string repositoryId, string folderId, string filter, string orderBy, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includePathSegment, string maxItems, string skipCount, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.folderId = folderId;
            this.filter = filter;
            this.orderBy = orderBy;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includePathSegment = includePathSegment;
            this.maxItems = maxItems;
            this.skipCount = skipCount;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getChildrenResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getChildrenResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectInFolderListType objects;

        public getChildrenResponse()
        {
        }

        public getChildrenResponse(cmisObjectInFolderListType objects)
        {
            this.objects = objects;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getFolderParent", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getFolderParentRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getFolderParentRequest()
        {
        }

        public getFolderParentRequest(string repositoryId, string folderId, string filter, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.folderId = folderId;
            this.filter = filter;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getFolderParentResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getFolderParentResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectType @object;

        public getFolderParentResponse()
        {
        }

        public getFolderParentResponse(cmisObjectType @object)
        {
            this.@object = @object;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getFolderTree", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getFolderTreeRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string depth;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePathSegment;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getFolderTreeRequest()
        {
        }

        public getFolderTreeRequest(string repositoryId, string folderId, string depth, string filter, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includePathSegment, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.folderId = folderId;
            this.depth = depth;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includePathSegment = includePathSegment;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getFolderTreeResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getFolderTreeResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("objects")]
        public cmisObjectInFolderContainerType[] objects;

        public getFolderTreeResponse()
        {
        }

        public getFolderTreeResponse(cmisObjectInFolderContainerType[] objects)
        {
            this.objects = objects;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisObjectParentsType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisObjectType objectField;

        private string relativePathSegmentField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisObjectType @object
        {
            get
            {
                return this.objectField;
            }
            set
            {
                this.objectField = value;
                this.RaisePropertyChanged("object");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string relativePathSegment
        {
            get
            {
                return this.relativePathSegmentField;
            }
            set
            {
                this.relativePathSegmentField = value;
                this.RaisePropertyChanged("relativePathSegment");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectParents", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectParentsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeRelativePathSegment;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getObjectParentsRequest()
        {
        }

        public getObjectParentsRequest(string repositoryId, string objectId, string filter, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includeRelativePathSegment, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includeRelativePathSegment = includeRelativePathSegment;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectParentsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectParentsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("parents")]
        public cmisObjectParentsType[] parents;

        public getObjectParentsResponse()
        {
        }

        public getObjectParentsResponse(cmisObjectParentsType[] parents)
        {
            this.parents = parents;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getCheckedOutDocs", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getCheckedOutDocsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string orderBy;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string skipCount;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getCheckedOutDocsRequest()
        {
        }

        public getCheckedOutDocsRequest(string repositoryId, string folderId, string filter, string orderBy, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, string maxItems, string skipCount, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.folderId = folderId;
            this.filter = filter;
            this.orderBy = orderBy;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.maxItems = maxItems;
            this.skipCount = skipCount;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getCheckedOutDocsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getCheckedOutDocsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectListType objects;

        public getCheckedOutDocsResponse()
        {
        }

        public getCheckedOutDocsResponse(cmisObjectListType objects)
        {
            this.objects = objects;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisContentStreamType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string lengthField;

        private string mimeTypeField;

        private string filenameField;

        private byte[] streamField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 0)]
        public string length
        {
            get
            {
                return this.lengthField;
            }
            set
            {
                this.lengthField = value;
                this.RaisePropertyChanged("length");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string mimeType
        {
            get
            {
                return this.mimeTypeField;
            }
            set
            {
                this.mimeTypeField = value;
                this.RaisePropertyChanged("mimeType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string filename
        {
            get
            {
                return this.filenameField;
            }
            set
            {
                this.filenameField = value;
                this.RaisePropertyChanged("filename");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary", Order = 3)]
        public byte[] stream
        {
            get
            {
                return this.streamField;
            }
            set
            {
                this.streamField = value;
                this.RaisePropertyChanged("stream");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 4)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumVersioningState
    {

        /// <remarks/>
        none,

        /// <remarks/>
        checkedout,

        /// <remarks/>
        minor,

        /// <remarks/>
        major,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createDocument", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createDocumentRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisContentStreamType contentStream;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumVersioningState> versioningState;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute("policies", IsNullable = true)]
        public string[] policies;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createDocumentRequest()
        {
        }

        public createDocumentRequest(string repositoryId, cmisPropertiesType properties, string folderId, cmisContentStreamType contentStream, System.Nullable<enumVersioningState> versioningState, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.properties = properties;
            this.folderId = folderId;
            this.contentStream = contentStream;
            this.versioningState = versioningState;
            this.policies = policies;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createDocumentResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createDocumentResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createDocumentResponse()
        {
        }

        public createDocumentResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createDocumentFromSource", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createDocumentFromSourceRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string sourceId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumVersioningState> versioningState;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute("policies", IsNullable = true)]
        public string[] policies;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createDocumentFromSourceRequest()
        {
        }

        public createDocumentFromSourceRequest(string repositoryId, string sourceId, cmisPropertiesType properties, string folderId, System.Nullable<enumVersioningState> versioningState, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.sourceId = sourceId;
            this.properties = properties;
            this.folderId = folderId;
            this.versioningState = versioningState;
            this.policies = policies;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createDocumentFromSourceResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createDocumentFromSourceResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createDocumentFromSourceResponse()
        {
        }

        public createDocumentFromSourceResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createFolder", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createFolderRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("policies", IsNullable = true)]
        public string[] policies;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createFolderRequest()
        {
        }

        public createFolderRequest(string repositoryId, cmisPropertiesType properties, string folderId, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.properties = properties;
            this.folderId = folderId;
            this.policies = policies;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createFolderResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createFolderResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createFolderResponse()
        {
        }

        public createFolderResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createRelationship", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createRelationshipRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("policies", IsNullable = true)]
        public string[] policies;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createRelationshipRequest()
        {
        }

        public createRelationshipRequest(string repositoryId, cmisPropertiesType properties, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.properties = properties;
            this.policies = policies;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createRelationshipResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createRelationshipResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createRelationshipResponse()
        {
        }

        public createRelationshipResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createPolicy", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createPolicyRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("policies", IsNullable = true)]
        public string[] policies;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createPolicyRequest()
        {
        }

        public createPolicyRequest(string repositoryId, cmisPropertiesType properties, string folderId, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.properties = properties;
            this.folderId = folderId;
            this.policies = policies;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "createPolicyResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class createPolicyResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public createPolicyResponse()
        {
        }

        public createPolicyResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getAllowableActions", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getAllowableActionsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getAllowableActionsRequest()
        {
        }

        public getAllowableActionsRequest(string repositoryId, string objectId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getAllowableActionsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getAllowableActionsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisAllowableActionsType allowableActions;

        public getAllowableActionsResponse()
        {
        }

        public getAllowableActionsResponse(cmisAllowableActionsType allowableActions)
        {
            this.allowableActions = allowableActions;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObject", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePolicyIds;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeACL;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getObjectRequest()
        {
        }

        public getObjectRequest(string repositoryId, string objectId, string filter, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includePolicyIds, System.Nullable<bool> includeACL, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includePolicyIds = includePolicyIds;
            this.includeACL = includeACL;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectType @object;

        public getObjectResponse()
        {
        }

        public getObjectResponse(cmisObjectType @object)
        {
            this.@object = @object;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getProperties", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getPropertiesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getPropertiesRequest()
        {
        }

        public getPropertiesRequest(string repositoryId, string objectId, string filter, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.filter = filter;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getPropertiesResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getPropertiesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisPropertiesType properties;

        public getPropertiesResponse()
        {
        }

        public getPropertiesResponse(cmisPropertiesType properties)
        {
            this.properties = properties;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getRenditions", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getRenditionsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string skipCount;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getRenditionsRequest()
        {
        }

        public getRenditionsRequest(string repositoryId, string objectId, string renditionFilter, string maxItems, string skipCount, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.renditionFilter = renditionFilter;
            this.maxItems = maxItems;
            this.skipCount = skipCount;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getRenditionsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getRenditionsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("renditions")]
        public cmisRenditionType[] renditions;

        public getRenditionsResponse()
        {
        }

        public getRenditionsResponse(cmisRenditionType[] renditions)
        {
            this.renditions = renditions;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectByPath", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectByPathRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string path;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePolicyIds;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeACL;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getObjectByPathRequest()
        {
        }

        public getObjectByPathRequest(string repositoryId, string path, string filter, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includePolicyIds, System.Nullable<bool> includeACL, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.path = path;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includePolicyIds = includePolicyIds;
            this.includeACL = includeACL;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectByPathResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectByPathResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectType @object;

        public getObjectByPathResponse()
        {
        }

        public getObjectByPathResponse(cmisObjectType @object)
        {
            this.@object = @object;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getContentStream", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getContentStreamRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string streamId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string offset;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string length;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getContentStreamRequest()
        {
        }

        public getContentStreamRequest(string repositoryId, string objectId, string streamId, string offset, string length, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.streamId = streamId;
            this.offset = offset;
            this.length = length;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getContentStreamResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getContentStreamResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisContentStreamType contentStream;

        public getContentStreamResponse()
        {
        }

        public getContentStreamResponse(cmisContentStreamType contentStream)
        {
            this.contentStream = contentStream;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "updateProperties", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class updatePropertiesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string changeToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public updatePropertiesRequest()
        {
        }

        public updatePropertiesRequest(string repositoryId, string objectId, string changeToken, cmisPropertiesType properties, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.changeToken = changeToken;
            this.properties = properties;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "updatePropertiesResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class updatePropertiesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string changeToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public updatePropertiesResponse()
        {
        }

        public updatePropertiesResponse(string objectId, string changeToken, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.changeToken = changeToken;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "moveObject", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class moveObjectRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string targetFolderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        public string sourceFolderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public moveObjectRequest()
        {
        }

        public moveObjectRequest(string repositoryId, string objectId, string targetFolderId, string sourceFolderId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.targetFolderId = targetFolderId;
            this.sourceFolderId = sourceFolderId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "moveObjectResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class moveObjectResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public moveObjectResponse()
        {
        }

        public moveObjectResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "deleteObject", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class deleteObjectRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> allVersions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public deleteObjectRequest()
        {
        }

        public deleteObjectRequest(string repositoryId, string objectId, System.Nullable<bool> allVersions, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.allVersions = allVersions;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "deleteObjectResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class deleteObjectResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public deleteObjectResponse()
        {
        }

        public deleteObjectResponse(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumUnfileObject
    {

        /// <remarks/>
        unfile,

        /// <remarks/>
        deletesinglefiled,

        /// <remarks/>
        delete,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class deleteTreeResponseFailedToDelete : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string[] objectIdsField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("objectIds", Order = 0)]
        public string[] objectIds
        {
            get
            {
                return this.objectIdsField;
            }
            set
            {
                this.objectIdsField = value;
                this.RaisePropertyChanged("objectIds");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 1)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "deleteTree", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class deleteTreeRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string folderId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> allVersions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumUnfileObject> unfileObjects;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> continueOnFailure;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public deleteTreeRequest()
        {
        }

        public deleteTreeRequest(string repositoryId, string folderId, System.Nullable<bool> allVersions, System.Nullable<enumUnfileObject> unfileObjects, System.Nullable<bool> continueOnFailure, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.folderId = folderId;
            this.allVersions = allVersions;
            this.unfileObjects = unfileObjects;
            this.continueOnFailure = continueOnFailure;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "deleteTreeResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class deleteTreeResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public deleteTreeResponseFailedToDelete failedToDelete;

        public deleteTreeResponse()
        {
        }

        public deleteTreeResponse(deleteTreeResponseFailedToDelete failedToDelete)
        {
            this.failedToDelete = failedToDelete;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "setContentStream", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class setContentStreamRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> overwriteFlag;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string changeToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        public cmisContentStreamType contentStream;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public setContentStreamRequest()
        {
        }

        public setContentStreamRequest(string repositoryId, string objectId, System.Nullable<bool> overwriteFlag, string changeToken, cmisContentStreamType contentStream, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.overwriteFlag = overwriteFlag;
            this.changeToken = changeToken;
            this.contentStream = contentStream;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "setContentStreamResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class setContentStreamResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string changeToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public setContentStreamResponse()
        {
        }

        public setContentStreamResponse(string objectId, string changeToken, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.changeToken = changeToken;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "deleteContentStream", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class deleteContentStreamRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string changeToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public deleteContentStreamRequest()
        {
        }

        public deleteContentStreamRequest(string repositoryId, string objectId, string changeToken, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.changeToken = changeToken;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "deleteContentStreamResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class deleteContentStreamResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string changeToken;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public deleteContentStreamResponse()
        {
        }

        public deleteContentStreamResponse(string objectId, string changeToken, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.changeToken = changeToken;
            this.extension = extension;
        }
    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "applyPolicy", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class applyPolicyRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string policyId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public applyPolicyRequest()
        {
        }

        public applyPolicyRequest(string repositoryId, string policyId, string objectId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.policyId = policyId;
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "applyPolicyResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class applyPolicyResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public applyPolicyResponse()
        {
        }

        public applyPolicyResponse(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "removePolicy", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class removePolicyRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string policyId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public removePolicyRequest()
        {
        }

        public removePolicyRequest(string repositoryId, string policyId, string objectId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.policyId = policyId;
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "removePolicyResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class removePolicyResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public removePolicyResponse()
        {
        }

        public removePolicyResponse(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getAppliedPolicies", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getAppliedPoliciesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getAppliedPoliciesRequest()
        {
        }

        public getAppliedPoliciesRequest(string repositoryId, string objectId, string filter, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.filter = filter;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getAppliedPoliciesResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getAppliedPoliciesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("objects")]
        public cmisObjectType[] objects;

        public getAppliedPoliciesResponse()
        {
        }

        public getAppliedPoliciesResponse(cmisObjectType[] objects)
        {
            this.objects = objects;
        }
    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumRelationshipDirection
    {

        /// <remarks/>
        source,

        /// <remarks/>
        target,

        /// <remarks/>
        either,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectRelationships", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectRelationshipsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public bool includeSubRelationshipTypes;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumRelationshipDirection> relationshipDirection;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string typeId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string skipCount;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getObjectRelationshipsRequest()
        {
        }

        public getObjectRelationshipsRequest(string repositoryId, string objectId, bool includeSubRelationshipTypes, System.Nullable<enumRelationshipDirection> relationshipDirection, string typeId, string filter, System.Nullable<bool> includeAllowableActions, string maxItems, string skipCount, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.includeSubRelationshipTypes = includeSubRelationshipTypes;
            this.relationshipDirection = relationshipDirection;
            this.typeId = typeId;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.maxItems = maxItems;
            this.skipCount = skipCount;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectRelationshipsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectRelationshipsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectListType objects;

        public getObjectRelationshipsResponse()
        {
        }

        public getObjectRelationshipsResponse(cmisObjectListType objects)
        {
            this.objects = objects;
        }
    }
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisRepositoryEntryType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string repositoryIdField;

        private string repositoryNameField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string repositoryId
        {
            get
            {
                return this.repositoryIdField;
            }
            set
            {
                this.repositoryIdField = value;
                this.RaisePropertyChanged("repositoryId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string repositoryName
        {
            get
            {
                return this.repositoryNameField;
            }
            set
            {
                this.repositoryNameField = value;
                this.RaisePropertyChanged("repositoryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getRepositories", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getRepositoriesRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getRepositoriesRequest()
        {
        }

        public getRepositoriesRequest(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getRepositoriesResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getRepositoriesResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("repositories", IsNullable = true)]
        public cmisRepositoryEntryType[] repositories;

        public getRepositoriesResponse()
        {
        }

        public getRepositoriesResponse(cmisRepositoryEntryType[] repositories)
        {
            this.repositories = repositories;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisRepositoryInfoType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string repositoryIdField;

        private string repositoryNameField;

        private string repositoryDescriptionField;

        private string vendorNameField;

        private string productNameField;

        private string productVersionField;

        private string rootFolderIdField;

        private string latestChangeLogTokenField;

        private cmisRepositoryCapabilitiesType capabilitiesField;

        private cmisACLCapabilityType aclCapabilityField;

        private string cmisVersionSupportedField;

        private string thinClientURIField;

        private bool changesIncompleteField;

        private bool changesIncompleteFieldSpecified;

        private enumBaseObjectTypeIds[] changesOnTypeField;

        private string principalAnonymousField;

        private string principalAnyoneField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string repositoryId
        {
            get
            {
                return this.repositoryIdField;
            }
            set
            {
                this.repositoryIdField = value;
                this.RaisePropertyChanged("repositoryId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string repositoryName
        {
            get
            {
                return this.repositoryNameField;
            }
            set
            {
                this.repositoryNameField = value;
                this.RaisePropertyChanged("repositoryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string repositoryDescription
        {
            get
            {
                return this.repositoryDescriptionField;
            }
            set
            {
                this.repositoryDescriptionField = value;
                this.RaisePropertyChanged("repositoryDescription");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string vendorName
        {
            get
            {
                return this.vendorNameField;
            }
            set
            {
                this.vendorNameField = value;
                this.RaisePropertyChanged("vendorName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string productName
        {
            get
            {
                return this.productNameField;
            }
            set
            {
                this.productNameField = value;
                this.RaisePropertyChanged("productName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string productVersion
        {
            get
            {
                return this.productVersionField;
            }
            set
            {
                this.productVersionField = value;
                this.RaisePropertyChanged("productVersion");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public string rootFolderId
        {
            get
            {
                return this.rootFolderIdField;
            }
            set
            {
                this.rootFolderIdField = value;
                this.RaisePropertyChanged("rootFolderId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string latestChangeLogToken
        {
            get
            {
                return this.latestChangeLogTokenField;
            }
            set
            {
                this.latestChangeLogTokenField = value;
                this.RaisePropertyChanged("latestChangeLogToken");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public cmisRepositoryCapabilitiesType capabilities
        {
            get
            {
                return this.capabilitiesField;
            }
            set
            {
                this.capabilitiesField = value;
                this.RaisePropertyChanged("capabilities");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public cmisACLCapabilityType aclCapability
        {
            get
            {
                return this.aclCapabilityField;
            }
            set
            {
                this.aclCapabilityField = value;
                this.RaisePropertyChanged("aclCapability");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public string cmisVersionSupported
        {
            get
            {
                return this.cmisVersionSupportedField;
            }
            set
            {
                this.cmisVersionSupportedField = value;
                this.RaisePropertyChanged("cmisVersionSupported");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 11)]
        public string thinClientURI
        {
            get
            {
                return this.thinClientURIField;
            }
            set
            {
                this.thinClientURIField = value;
                this.RaisePropertyChanged("thinClientURI");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool changesIncomplete
        {
            get
            {
                return this.changesIncompleteField;
            }
            set
            {
                this.changesIncompleteField = value;
                this.RaisePropertyChanged("changesIncomplete");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool changesIncompleteSpecified
        {
            get
            {
                return this.changesIncompleteFieldSpecified;
            }
            set
            {
                this.changesIncompleteFieldSpecified = value;
                this.RaisePropertyChanged("changesIncompleteSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("changesOnType", Order = 13)]
        public enumBaseObjectTypeIds[] changesOnType
        {
            get
            {
                return this.changesOnTypeField;
            }
            set
            {
                this.changesOnTypeField = value;
                this.RaisePropertyChanged("changesOnType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public string principalAnonymous
        {
            get
            {
                return this.principalAnonymousField;
            }
            set
            {
                this.principalAnonymousField = value;
                this.RaisePropertyChanged("principalAnonymous");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 15)]
        public string principalAnyone
        {
            get
            {
                return this.principalAnyoneField;
            }
            set
            {
                this.principalAnyoneField = value;
                this.RaisePropertyChanged("principalAnyone");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 16)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisRepositoryCapabilitiesType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private enumCapabilityACL capabilityACLField;

        private bool capabilityAllVersionsSearchableField;

        private enumCapabilityChanges capabilityChangesField;

        private enumCapabilityContentStreamUpdates capabilityContentStreamUpdatabilityField;

        private bool capabilityGetDescendantsField;

        private bool capabilityGetFolderTreeField;

        private bool capabilityMultifilingField;

        private bool capabilityPWCSearchableField;

        private bool capabilityPWCUpdatableField;

        private enumCapabilityQuery capabilityQueryField;

        private enumCapabilityRendition capabilityRenditionsField;

        private bool capabilityUnfilingField;

        private bool capabilityVersionSpecificFilingField;

        private enumCapabilityJoin capabilityJoinField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public enumCapabilityACL capabilityACL
        {
            get
            {
                return this.capabilityACLField;
            }
            set
            {
                this.capabilityACLField = value;
                this.RaisePropertyChanged("capabilityACL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool capabilityAllVersionsSearchable
        {
            get
            {
                return this.capabilityAllVersionsSearchableField;
            }
            set
            {
                this.capabilityAllVersionsSearchableField = value;
                this.RaisePropertyChanged("capabilityAllVersionsSearchable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public enumCapabilityChanges capabilityChanges
        {
            get
            {
                return this.capabilityChangesField;
            }
            set
            {
                this.capabilityChangesField = value;
                this.RaisePropertyChanged("capabilityChanges");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public enumCapabilityContentStreamUpdates capabilityContentStreamUpdatability
        {
            get
            {
                return this.capabilityContentStreamUpdatabilityField;
            }
            set
            {
                this.capabilityContentStreamUpdatabilityField = value;
                this.RaisePropertyChanged("capabilityContentStreamUpdatability");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public bool capabilityGetDescendants
        {
            get
            {
                return this.capabilityGetDescendantsField;
            }
            set
            {
                this.capabilityGetDescendantsField = value;
                this.RaisePropertyChanged("capabilityGetDescendants");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public bool capabilityGetFolderTree
        {
            get
            {
                return this.capabilityGetFolderTreeField;
            }
            set
            {
                this.capabilityGetFolderTreeField = value;
                this.RaisePropertyChanged("capabilityGetFolderTree");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public bool capabilityMultifiling
        {
            get
            {
                return this.capabilityMultifilingField;
            }
            set
            {
                this.capabilityMultifilingField = value;
                this.RaisePropertyChanged("capabilityMultifiling");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public bool capabilityPWCSearchable
        {
            get
            {
                return this.capabilityPWCSearchableField;
            }
            set
            {
                this.capabilityPWCSearchableField = value;
                this.RaisePropertyChanged("capabilityPWCSearchable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public bool capabilityPWCUpdatable
        {
            get
            {
                return this.capabilityPWCUpdatableField;
            }
            set
            {
                this.capabilityPWCUpdatableField = value;
                this.RaisePropertyChanged("capabilityPWCUpdatable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public enumCapabilityQuery capabilityQuery
        {
            get
            {
                return this.capabilityQueryField;
            }
            set
            {
                this.capabilityQueryField = value;
                this.RaisePropertyChanged("capabilityQuery");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public enumCapabilityRendition capabilityRenditions
        {
            get
            {
                return this.capabilityRenditionsField;
            }
            set
            {
                this.capabilityRenditionsField = value;
                this.RaisePropertyChanged("capabilityRenditions");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool capabilityUnfiling
        {
            get
            {
                return this.capabilityUnfilingField;
            }
            set
            {
                this.capabilityUnfilingField = value;
                this.RaisePropertyChanged("capabilityUnfiling");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool capabilityVersionSpecificFiling
        {
            get
            {
                return this.capabilityVersionSpecificFilingField;
            }
            set
            {
                this.capabilityVersionSpecificFilingField = value;
                this.RaisePropertyChanged("capabilityVersionSpecificFiling");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public enumCapabilityJoin capabilityJoin
        {
            get
            {
                return this.capabilityJoinField;
            }
            set
            {
                this.capabilityJoinField = value;
                this.RaisePropertyChanged("capabilityJoin");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 14)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCapabilityACL
    {

        /// <remarks/>
        none,

        /// <remarks/>
        discover,

        /// <remarks/>
        manage,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCapabilityChanges
    {

        /// <remarks/>
        none,

        /// <remarks/>
        objectidsonly,

        /// <remarks/>
        properties,

        /// <remarks/>
        all,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCapabilityContentStreamUpdates
    {

        /// <remarks/>
        anytime,

        /// <remarks/>
        pwconly,

        /// <remarks/>
        none,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCapabilityQuery
    {

        /// <remarks/>
        none,

        /// <remarks/>
        metadataonly,

        /// <remarks/>
        fulltextonly,

        /// <remarks/>
        bothseparate,

        /// <remarks/>
        bothcombined,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCapabilityRendition
    {

        /// <remarks/>
        none,

        /// <remarks/>
        read,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCapabilityJoin
    {

        /// <remarks/>
        none,

        /// <remarks/>
        inneronly,

        /// <remarks/>
        innerandouter,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisACLCapabilityType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private enumSupportedPermissions supportedPermissionsField;

        private enumACLPropagation propagationField;

        private cmisPermissionDefinition[] permissionsField;

        private cmisPermissionMapping[] mappingField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public enumSupportedPermissions supportedPermissions
        {
            get
            {
                return this.supportedPermissionsField;
            }
            set
            {
                this.supportedPermissionsField = value;
                this.RaisePropertyChanged("supportedPermissions");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public enumACLPropagation propagation
        {
            get
            {
                return this.propagationField;
            }
            set
            {
                this.propagationField = value;
                this.RaisePropertyChanged("propagation");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("permissions", Order = 2)]
        public cmisPermissionDefinition[] permissions
        {
            get
            {
                return this.permissionsField;
            }
            set
            {
                this.permissionsField = value;
                this.RaisePropertyChanged("permissions");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("mapping", Order = 3)]
        public cmisPermissionMapping[] mapping
        {
            get
            {
                return this.mappingField;
            }
            set
            {
                this.mappingField = value;
                this.RaisePropertyChanged("mapping");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumSupportedPermissions
    {

        /// <remarks/>
        basic,

        /// <remarks/>
        repository,

        /// <remarks/>
        both,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumACLPropagation
    {

        /// <remarks/>
        repositorydetermined,

        /// <remarks/>
        objectonly,

        /// <remarks/>
        propagate,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPermissionDefinition : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string permissionField;

        private string descriptionField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string permission
        {
            get
            {
                return this.permissionField;
            }
            set
            {
                this.permissionField = value;
                this.RaisePropertyChanged("permission");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
                this.RaisePropertyChanged("description");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPermissionMapping : object, System.ComponentModel.INotifyPropertyChanged
    {

        private enumAllowableActionsKey keyField;

        private string[] permissionField;

        private System.Xml.XmlElement[] anyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public enumAllowableActionsKey key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
                this.RaisePropertyChanged("key");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("permission", Order = 1)]
        public string[] permission
        {
            get
            {
                return this.permissionField;
            }
            set
            {
                this.permissionField = value;
                this.RaisePropertyChanged("permission");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumAllowableActionsKey
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetDescendents.Folder")]
        canGetDescendentsFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetChildren.Folder")]
        canGetChildrenFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetParents.Folder")]
        canGetParentsFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetFolderParent.Object")]
        canGetFolderParentObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCreateDocument.Folder")]
        canCreateDocumentFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCreateFolder.Folder")]
        canCreateFolderFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCreateRelationship.Source")]
        canCreateRelationshipSource,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCreateRelationship.Target")]
        canCreateRelationshipTarget,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetProperties.Object")]
        canGetPropertiesObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canViewContent.Object")]
        canViewContentObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canUpdateProperties.Object")]
        canUpdatePropertiesObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canMove.Object")]
        canMoveObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canMove.Target")]
        canMoveTarget,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canMove.Source")]
        canMoveSource,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canDelete.Object")]
        canDeleteObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canDeleteTree.Folder")]
        canDeleteTreeFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canSetContent.Document")]
        canSetContentDocument,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canDeleteContent.Document")]
        canDeleteContentDocument,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canAddToFolder.Object")]
        canAddToFolderObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canAddToFolder.Folder")]
        canAddToFolderFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canRemoveFromFolder.Object")]
        canRemoveFromFolderObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canRemoveFromFolder.Folder")]
        canRemoveFromFolderFolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCheckout.Document")]
        canCheckoutDocument,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCancelCheckout.Document")]
        canCancelCheckoutDocument,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canCheckin.Document")]
        canCheckinDocument,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetAllVersions.VersionSeries")]
        canGetAllVersionsVersionSeries,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetObjectRelationships.Object")]
        canGetObjectRelationshipsObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canAddPolicy.Object")]
        canAddPolicyObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canAddPolicy.Policy")]
        canAddPolicyPolicy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canRemovePolicy.Object")]
        canRemovePolicyObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canRemovePolicy.Policy")]
        canRemovePolicyPolicy,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetAppliedPolicies.Object")]
        canGetAppliedPoliciesObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canGetACL.Object")]
        canGetACLObject,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("canApplyACL.Object")]
        canApplyACLObject,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumBaseObjectTypeIds
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("cmis:document")]
        cmisdocument,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("cmis:folder")]
        cmisfolder,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("cmis:relationship")]
        cmisrelationship,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("cmis:policy")]
        cmispolicy,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getRepositoryInfo", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getRepositoryInfoRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getRepositoryInfoRequest()
        {
        }

        public getRepositoryInfoRequest(string repositoryId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getRepositoryInfoResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getRepositoryInfoResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisRepositoryInfoType repositoryInfo;

        public getRepositoryInfoResponse()
        {
        }

        public getRepositoryInfoResponse(cmisRepositoryInfoType repositoryInfo)
        {
            this.repositoryInfo = repositoryInfo;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisTypeDefinitionListType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisTypeDefinitionType[] typesField;

        private bool hasMoreItemsField;

        private string numItemsField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("types", Order = 0)]
        public cmisTypeDefinitionType[] types
        {
            get
            {
                return this.typesField;
            }
            set
            {
                this.typesField = value;
                this.RaisePropertyChanged("types");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool hasMoreItems
        {
            get
            {
                return this.hasMoreItemsField;
            }
            set
            {
                this.hasMoreItemsField = value;
                this.RaisePropertyChanged("hasMoreItems");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string numItems
        {
            get
            {
                return this.numItemsField;
            }
            set
            {
                this.numItemsField = value;
                this.RaisePropertyChanged("numItems");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 3)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisTypePolicyDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisTypeRelationshipDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisTypeFolderDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisTypeDocumentDefinitionType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisTypeDefinitionType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string idField;

        private string localNameField;

        private string localNamespaceField;

        private string displayNameField;

        private string queryNameField;

        private string descriptionField;

        private enumBaseObjectTypeIds baseIdField;

        private string parentIdField;

        private bool creatableField;

        private bool fileableField;

        private bool queryableField;

        private bool fulltextIndexedField;

        private bool includedInSupertypeQueryField;

        private bool controllablePolicyField;

        private bool controllableACLField;

        private cmisPropertyDefinitionType[] itemsField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        public cmisTypeDefinitionType()
        {
            this.includedInSupertypeQueryField = true;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string localName
        {
            get
            {
                return this.localNameField;
            }
            set
            {
                this.localNameField = value;
                this.RaisePropertyChanged("localName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", IsNullable = true, Order = 2)]
        public string localNamespace
        {
            get
            {
                return this.localNamespaceField;
            }
            set
            {
                this.localNamespaceField = value;
                this.RaisePropertyChanged("localNamespace");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
                this.RaisePropertyChanged("displayName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string queryName
        {
            get
            {
                return this.queryNameField;
            }
            set
            {
                this.queryNameField = value;
                this.RaisePropertyChanged("queryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
                this.RaisePropertyChanged("description");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public enumBaseObjectTypeIds baseId
        {
            get
            {
                return this.baseIdField;
            }
            set
            {
                this.baseIdField = value;
                this.RaisePropertyChanged("baseId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public string parentId
        {
            get
            {
                return this.parentIdField;
            }
            set
            {
                this.parentIdField = value;
                this.RaisePropertyChanged("parentId");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public bool creatable
        {
            get
            {
                return this.creatableField;
            }
            set
            {
                this.creatableField = value;
                this.RaisePropertyChanged("creatable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public bool fileable
        {
            get
            {
                return this.fileableField;
            }
            set
            {
                this.fileableField = value;
                this.RaisePropertyChanged("fileable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public bool queryable
        {
            get
            {
                return this.queryableField;
            }
            set
            {
                this.queryableField = value;
                this.RaisePropertyChanged("queryable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool fulltextIndexed
        {
            get
            {
                return this.fulltextIndexedField;
            }
            set
            {
                this.fulltextIndexedField = value;
                this.RaisePropertyChanged("fulltextIndexed");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool includedInSupertypeQuery
        {
            get
            {
                return this.includedInSupertypeQueryField;
            }
            set
            {
                this.includedInSupertypeQueryField = value;
                this.RaisePropertyChanged("includedInSupertypeQuery");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool controllablePolicy
        {
            get
            {
                return this.controllablePolicyField;
            }
            set
            {
                this.controllablePolicyField = value;
                this.RaisePropertyChanged("controllablePolicy");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 14)]
        public bool controllableACL
        {
            get
            {
                return this.controllableACLField;
            }
            set
            {
                this.controllableACLField = value;
                this.RaisePropertyChanged("controllableACL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("propertyBooleanDefinition", typeof(cmisPropertyBooleanDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyDateTimeDefinition", typeof(cmisPropertyDateTimeDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyDecimalDefinition", typeof(cmisPropertyDecimalDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyHtmlDefinition", typeof(cmisPropertyHtmlDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyIdDefinition", typeof(cmisPropertyIdDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyIntegerDefinition", typeof(cmisPropertyIntegerDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyStringDefinition", typeof(cmisPropertyStringDefinitionType), Order = 15)]
        [System.Xml.Serialization.XmlElementAttribute("propertyUriDefinition", typeof(cmisPropertyUriDefinitionType), Order = 15)]
        public cmisPropertyDefinitionType[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
                this.RaisePropertyChanged("Items");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 16)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyBooleanDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyBoolean defaultValueField;

        private cmisChoiceBoolean[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyBoolean defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceBoolean[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceBoolean : cmisChoice
    {

        private bool[] valueField;

        private cmisChoiceBoolean[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public bool[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceBoolean[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceUri))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceString))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceHtml))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceDecimal))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceDateTime))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceInteger))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceId))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisChoiceBoolean))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoice : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string displayNameField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
                this.RaisePropertyChanged("displayName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceUri : cmisChoice
    {

        private string[] valueField;

        private cmisChoiceUri[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", DataType = "anyURI", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceUri[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceString : cmisChoice
    {

        private string[] valueField;

        private cmisChoiceString[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceString[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceHtml : cmisChoice
    {

        private string[] valueField;

        private cmisChoiceHtml[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceHtml[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceDecimal : cmisChoice
    {

        private decimal[] valueField;

        private cmisChoiceDecimal[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public decimal[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceDecimal[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceDateTime : cmisChoice
    {

        private System.DateTime[] valueField;

        private cmisChoiceDateTime[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public System.DateTime[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceDateTime[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceInteger : cmisChoice
    {

        private string[] valueField;

        private cmisChoiceInteger[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", DataType = "integer", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceInteger[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisChoiceId : cmisChoice
    {

        private string[] valueField;

        private cmisChoiceId[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value", Order = 0)]
        public string[] value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceId[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyUriDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyStringDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyHtmlDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyDecimalDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyDateTimeDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyIntegerDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyIdDefinitionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(cmisPropertyBooleanDefinitionType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyDefinitionType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string idField;

        private string localNameField;

        private string localNamespaceField;

        private string displayNameField;

        private string queryNameField;

        private string descriptionField;

        private enumPropertyType propertyTypeField;

        private enumCardinality cardinalityField;

        private enumUpdatability updatabilityField;

        private bool inheritedField;

        private bool inheritedFieldSpecified;

        private bool requiredField;

        private bool queryableField;

        private bool orderableField;

        private bool openChoiceField;

        private bool openChoiceFieldSpecified;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("id");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string localName
        {
            get
            {
                return this.localNameField;
            }
            set
            {
                this.localNameField = value;
                this.RaisePropertyChanged("localName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 2)]
        public string localNamespace
        {
            get
            {
                return this.localNamespaceField;
            }
            set
            {
                this.localNamespaceField = value;
                this.RaisePropertyChanged("localNamespace");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public string displayName
        {
            get
            {
                return this.displayNameField;
            }
            set
            {
                this.displayNameField = value;
                this.RaisePropertyChanged("displayName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public string queryName
        {
            get
            {
                return this.queryNameField;
            }
            set
            {
                this.queryNameField = value;
                this.RaisePropertyChanged("queryName");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
                this.RaisePropertyChanged("description");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public enumPropertyType propertyType
        {
            get
            {
                return this.propertyTypeField;
            }
            set
            {
                this.propertyTypeField = value;
                this.RaisePropertyChanged("propertyType");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public enumCardinality cardinality
        {
            get
            {
                return this.cardinalityField;
            }
            set
            {
                this.cardinalityField = value;
                this.RaisePropertyChanged("cardinality");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public enumUpdatability updatability
        {
            get
            {
                return this.updatabilityField;
            }
            set
            {
                this.updatabilityField = value;
                this.RaisePropertyChanged("updatability");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 9)]
        public bool inherited
        {
            get
            {
                return this.inheritedField;
            }
            set
            {
                this.inheritedField = value;
                this.RaisePropertyChanged("inherited");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool inheritedSpecified
        {
            get
            {
                return this.inheritedFieldSpecified;
            }
            set
            {
                this.inheritedFieldSpecified = value;
                this.RaisePropertyChanged("inheritedSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public bool required
        {
            get
            {
                return this.requiredField;
            }
            set
            {
                this.requiredField = value;
                this.RaisePropertyChanged("required");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public bool queryable
        {
            get
            {
                return this.queryableField;
            }
            set
            {
                this.queryableField = value;
                this.RaisePropertyChanged("queryable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public bool orderable
        {
            get
            {
                return this.orderableField;
            }
            set
            {
                this.orderableField = value;
                this.RaisePropertyChanged("orderable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public bool openChoice
        {
            get
            {
                return this.openChoiceField;
            }
            set
            {
                this.openChoiceField = value;
                this.RaisePropertyChanged("openChoice");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool openChoiceSpecified
        {
            get
            {
                return this.openChoiceFieldSpecified;
            }
            set
            {
                this.openChoiceFieldSpecified = value;
                this.RaisePropertyChanged("openChoiceSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 14)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumPropertyType
    {

        /// <remarks/>
        boolean,

        /// <remarks/>
        id,

        /// <remarks/>
        integer,

        /// <remarks/>
        datetime,

        /// <remarks/>
        @decimal,

        /// <remarks/>
        html,

        /// <remarks/>
        @string,

        /// <remarks/>
        uri,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumCardinality
    {

        /// <remarks/>
        single,

        /// <remarks/>
        multi,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumUpdatability
    {

        /// <remarks/>
        @readonly,

        /// <remarks/>
        readwrite,

        /// <remarks/>
        whencheckedout,

        /// <remarks/>
        oncreate,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyUriDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyUri defaultValueField;

        private cmisChoiceUri[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyUri defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceUri[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyStringDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyString defaultValueField;

        private string maxLengthField;

        private cmisChoiceString[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyString defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string maxLength
        {
            get
            {
                return this.maxLengthField;
            }
            set
            {
                this.maxLengthField = value;
                this.RaisePropertyChanged("maxLength");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 2)]
        public cmisChoiceString[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyHtmlDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyHtml defaultValueField;

        private cmisChoiceHtml[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyHtml defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceHtml[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyDecimalDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyDecimal defaultValueField;

        private decimal maxValueField;

        private bool maxValueFieldSpecified;

        private decimal minValueField;

        private bool minValueFieldSpecified;

        private enumDecimalPrecision precisionField;

        private bool precisionFieldSpecified;

        private cmisChoiceDecimal[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyDecimal defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public decimal maxValue
        {
            get
            {
                return this.maxValueField;
            }
            set
            {
                this.maxValueField = value;
                this.RaisePropertyChanged("maxValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxValueSpecified
        {
            get
            {
                return this.maxValueFieldSpecified;
            }
            set
            {
                this.maxValueFieldSpecified = value;
                this.RaisePropertyChanged("maxValueSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public decimal minValue
        {
            get
            {
                return this.minValueField;
            }
            set
            {
                this.minValueField = value;
                this.RaisePropertyChanged("minValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minValueSpecified
        {
            get
            {
                return this.minValueFieldSpecified;
            }
            set
            {
                this.minValueFieldSpecified = value;
                this.RaisePropertyChanged("minValueSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public enumDecimalPrecision precision
        {
            get
            {
                return this.precisionField;
            }
            set
            {
                this.precisionField = value;
                this.RaisePropertyChanged("precision");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool precisionSpecified
        {
            get
            {
                return this.precisionFieldSpecified;
            }
            set
            {
                this.precisionFieldSpecified = value;
                this.RaisePropertyChanged("precisionSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 4)]
        public cmisChoiceDecimal[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumDecimalPrecision
    {

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("32")]
        Item32,

        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("64")]
        Item64,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyDateTimeDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyDateTime defaultValueField;

        private enumDateTimeResolution resolutionField;

        private bool resolutionFieldSpecified;

        private cmisChoiceDateTime[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyDateTime defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public enumDateTimeResolution resolution
        {
            get
            {
                return this.resolutionField;
            }
            set
            {
                this.resolutionField = value;
                this.RaisePropertyChanged("resolution");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool resolutionSpecified
        {
            get
            {
                return this.resolutionFieldSpecified;
            }
            set
            {
                this.resolutionFieldSpecified = value;
                this.RaisePropertyChanged("resolutionSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 2)]
        public cmisChoiceDateTime[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumDateTimeResolution
    {

        /// <remarks/>
        year,

        /// <remarks/>
        date,

        /// <remarks/>
        time,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyIntegerDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyInteger defaultValueField;

        private string maxValueField;

        private string minValueField;

        private cmisChoiceInteger[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyInteger defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 1)]
        public string maxValue
        {
            get
            {
                return this.maxValueField;
            }
            set
            {
                this.maxValueField = value;
                this.RaisePropertyChanged("maxValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", Order = 2)]
        public string minValue
        {
            get
            {
                return this.minValueField;
            }
            set
            {
                this.minValueField = value;
                this.RaisePropertyChanged("minValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 3)]
        public cmisChoiceInteger[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisPropertyIdDefinitionType : cmisPropertyDefinitionType
    {

        private cmisPropertyId defaultValueField;

        private cmisChoiceId[] choiceField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisPropertyId defaultValue
        {
            get
            {
                return this.defaultValueField;
            }
            set
            {
                this.defaultValueField = value;
                this.RaisePropertyChanged("defaultValue");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("choice", Order = 1)]
        public cmisChoiceId[] choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
                this.RaisePropertyChanged("choice");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisTypePolicyDefinitionType : cmisTypeDefinitionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisTypeRelationshipDefinitionType : cmisTypeDefinitionType
    {

        private string[] allowedSourceTypesField;

        private string[] allowedTargetTypesField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("allowedSourceTypes", Order = 0)]
        public string[] allowedSourceTypes
        {
            get
            {
                return this.allowedSourceTypesField;
            }
            set
            {
                this.allowedSourceTypesField = value;
                this.RaisePropertyChanged("allowedSourceTypes");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("allowedTargetTypes", Order = 1)]
        public string[] allowedTargetTypes
        {
            get
            {
                return this.allowedTargetTypesField;
            }
            set
            {
                this.allowedTargetTypesField = value;
                this.RaisePropertyChanged("allowedTargetTypes");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisTypeFolderDefinitionType : cmisTypeDefinitionType
    {
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public partial class cmisTypeDocumentDefinitionType : cmisTypeDefinitionType
    {

        private bool versionableField;

        private enumContentStreamAllowed contentStreamAllowedField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public bool versionable
        {
            get
            {
                return this.versionableField;
            }
            set
            {
                this.versionableField = value;
                this.RaisePropertyChanged("versionable");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public enumContentStreamAllowed contentStreamAllowed
        {
            get
            {
                return this.contentStreamAllowedField;
            }
            set
            {
                this.contentStreamAllowedField = value;
                this.RaisePropertyChanged("contentStreamAllowed");
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/core/200908/")]
    public enum enumContentStreamAllowed
    {

        /// <remarks/>
        notallowed,

        /// <remarks/>
        allowed,

        /// <remarks/>
        required,
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getTypeChildren", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getTypeChildrenRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string typeId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePropertyDefinitions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string maxItems;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string skipCount;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getTypeChildrenRequest()
        {
        }

        public getTypeChildrenRequest(string repositoryId, string typeId, System.Nullable<bool> includePropertyDefinitions, string maxItems, string skipCount, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.typeId = typeId;
            this.includePropertyDefinitions = includePropertyDefinitions;
            this.maxItems = maxItems;
            this.skipCount = skipCount;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getTypeChildrenResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getTypeChildrenResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisTypeDefinitionListType types;

        public getTypeChildrenResponse()
        {
        }

        public getTypeChildrenResponse(cmisTypeDefinitionListType types)
        {
            this.types = types;
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisTypeContainer : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisTypeDefinitionType typeField;

        private cmisTypeContainer[] childrenField;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisTypeDefinitionType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("type");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("children", Order = 1)]
        public cmisTypeContainer[] children
        {
            get
            {
                return this.childrenField;
            }
            set
            {
                this.childrenField = value;
                this.RaisePropertyChanged("children");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getTypeDescendants", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getTypeDescendantsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string typeId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer", IsNullable = true)]
        public string depth;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePropertyDefinitions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getTypeDescendantsRequest()
        {
        }

        public getTypeDescendantsRequest(string repositoryId, string typeId, string depth, System.Nullable<bool> includePropertyDefinitions, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.typeId = typeId;
            this.depth = depth;
            this.includePropertyDefinitions = includePropertyDefinitions;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getTypeDescendantsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getTypeDescendantsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("types")]
        public cmisTypeContainer[] types;

        public getTypeDescendantsResponse()
        {
        }

        public getTypeDescendantsResponse(cmisTypeContainer[] types)
        {
            this.types = types;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getTypeDefinition", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getTypeDefinitionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string typeId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getTypeDefinitionRequest()
        {
        }

        public getTypeDefinitionRequest(string repositoryId, string typeId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.typeId = typeId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getTypeDefinitionResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getTypeDefinitionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisTypeDefinitionType type;

        public getTypeDefinitionResponse()
        {
        }

        public getTypeDefinitionResponse(cmisTypeDefinitionType type)
        {
            this.type = type;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkOut", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class checkOutRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public checkOutRequest()
        {
        }

        public checkOutRequest(string repositoryId, string objectId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkOutResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class checkOutResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public bool contentCopied;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public checkOutResponse()
        {
        }

        public checkOutResponse(string objectId, bool contentCopied, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.contentCopied = contentCopied;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "cancelCheckOut", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class cancelCheckOutRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public cancelCheckOutRequest()
        {
        }

        public cancelCheckOutRequest(string repositoryId, string objectId, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "cancelCheckOutResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class cancelCheckOutResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public cancelCheckOutResponse()
        {
        }

        public cancelCheckOutResponse(cmisExtensionType extension)
        {
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkIn", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class checkInRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> major;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisPropertiesType properties;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisContentStreamType contentStream;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string checkinComment;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute("policies", IsNullable = true)]
        public string[] policies;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public checkInRequest()
        {
        }

        public checkInRequest(string repositoryId, string objectId, System.Nullable<bool> major, cmisPropertiesType properties, cmisContentStreamType contentStream, string checkinComment, string[] policies, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.major = major;
            this.properties = properties;
            this.contentStream = contentStream;
            this.checkinComment = checkinComment;
            this.policies = policies;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "checkInResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class checkInResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public checkInResponse()
        {
        }

        public checkInResponse(string objectId, cmisExtensionType extension)
        {
            this.objectId = objectId;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectOfLatestVersion", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectOfLatestVersionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public bool major;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumIncludeRelationships> includeRelationships;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 6)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string renditionFilter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 7)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includePolicyIds;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 8)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeACL;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 9)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getObjectOfLatestVersionRequest()
        {
        }

        public getObjectOfLatestVersionRequest(string repositoryId, string objectId, bool major, string filter, System.Nullable<bool> includeAllowableActions, System.Nullable<enumIncludeRelationships> includeRelationships, string renditionFilter, System.Nullable<bool> includePolicyIds, System.Nullable<bool> includeACL, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.major = major;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.includeRelationships = includeRelationships;
            this.renditionFilter = renditionFilter;
            this.includePolicyIds = includePolicyIds;
            this.includeACL = includeACL;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getObjectOfLatestVersionResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getObjectOfLatestVersionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisObjectType @object;

        public getObjectOfLatestVersionResponse()
        {
        }

        public getObjectOfLatestVersionResponse(cmisObjectType @object)
        {
            this.@object = @object;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getPropertiesOfLatestVersion", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getPropertiesOfLatestVersionRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public bool major;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getPropertiesOfLatestVersionRequest()
        {
        }

        public getPropertiesOfLatestVersionRequest(string repositoryId, string objectId, bool major, string filter, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.major = major;
            this.filter = filter;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getPropertiesOfLatestVersionResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getPropertiesOfLatestVersionResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisPropertiesType properties;

        public getPropertiesOfLatestVersionResponse()
        {
        }

        public getPropertiesOfLatestVersionResponse(cmisPropertiesType properties)
        {
            this.properties = properties;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getAllVersions", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getAllVersionsRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public string filter;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> includeAllowableActions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getAllVersionsRequest()
        {
        }

        public getAllVersionsRequest(string repositoryId, string objectId, string filter, System.Nullable<bool> includeAllowableActions, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.filter = filter;
            this.includeAllowableActions = includeAllowableActions;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getAllVersionsResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getAllVersionsResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("objects")]
        public cmisObjectType[] objects;

        public getAllVersionsResponse()
        {
        }

        public getAllVersionsResponse(cmisObjectType[] objects)
        {
            this.objects = objects;
        }
    }



    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/")]
    public partial class cmisACLType : object, System.ComponentModel.INotifyPropertyChanged
    {

        private cmisAccessControlListType aCLField;

        private bool exactField;

        private bool exactFieldSpecified;

        private System.Xml.XmlElement[] anyField;

        private System.Xml.XmlAttribute[] anyAttrField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public cmisAccessControlListType ACL
        {
            get
            {
                return this.aCLField;
            }
            set
            {
                this.aCLField = value;
                this.RaisePropertyChanged("ACL");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool exact
        {
            get
            {
                return this.exactField;
            }
            set
            {
                this.exactField = value;
                this.RaisePropertyChanged("exact");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool exactSpecified
        {
            get
            {
                return this.exactFieldSpecified;
            }
            set
            {
                this.exactFieldSpecified = value;
                this.RaisePropertyChanged("exactSpecified");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 2)]
        public System.Xml.XmlElement[] Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
                this.RaisePropertyChanged("Any");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public System.Xml.XmlAttribute[] AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
                this.RaisePropertyChanged("AnyAttr");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getACL", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getACLRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<bool> onlyBasicPermissions;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public getACLRequest()
        {
        }

        public getACLRequest(string repositoryId, string objectId, System.Nullable<bool> onlyBasicPermissions, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.onlyBasicPermissions = onlyBasicPermissions;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "getACLResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class getACLResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisACLType ACL;

        public getACLResponse()
        {
        }

        public getACLResponse(cmisACLType ACL)
        {
            this.ACL = ACL;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "applyACL", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class applyACLRequest
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public string repositoryId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 1)]
        public string objectId;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 2)]
        public cmisAccessControlListType addACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 3)]
        public cmisAccessControlListType removeACEs;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public System.Nullable<enumACLPropagation> ACLPropagation;

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 5)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        public cmisExtensionType extension;

        public applyACLRequest()
        {
        }

        public applyACLRequest(string repositoryId, string objectId, cmisAccessControlListType addACEs, cmisAccessControlListType removeACEs, System.Nullable<enumACLPropagation> ACLPropagation, cmisExtensionType extension)
        {
            this.repositoryId = repositoryId;
            this.objectId = objectId;
            this.addACEs = addACEs;
            this.removeACEs = removeACEs;
            this.ACLPropagation = ACLPropagation;
            this.extension = extension;
        }
    }

    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName = "applyACLResponse", WrapperNamespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", IsWrapped = true)]
    public partial class applyACLResponse
    {

        [System.ServiceModel.MessageBodyMemberAttribute(Namespace = "http://docs.oasis-open.org/ns/cmis/messaging/200908/", Order = 0)]
        public cmisACLType ACL;

        public applyACLResponse()
        {
        }

        public applyACLResponse(cmisACLType ACL)
        {
            this.ACL = ACL;
        }
    }
}
