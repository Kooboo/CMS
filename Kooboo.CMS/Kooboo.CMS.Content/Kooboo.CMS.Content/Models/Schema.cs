#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Form;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Kooboo.Common.Globalization;
using Kooboo.Common.Data;
namespace Kooboo.CMS.Content.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    [Serializable]
    public class Column : IColumn
    {
        #region System Columns
        public static string[] Sys_Fields = new string[] { 
            "Id",
            "UUID",
            "Repository",
            "FolderName",
            "UserKey",
            "UtcCreationDate",
            "UtcLastModificationDate",
            "Published",           
            "SchemaName",
            "ParentFolder",
            "ParentUUID",
            "UserId",
            "OriginalFolder",
            "OriginalUUID",
            "IsLocalized",
            "Sequence"
        };
        public static Column Id = new Column()
        {
            Name = "Id",
            Label = "Id",
            DataType = DataType.Int,
            ControlType = "Hidden"
        };
        public static Column UUID = new Column()
        {
            Name = "UUID",
            Label = "UUID",
            DataType = DataType.String,
            Length = 36,
            ControlType = "Hidden"
        };
        public static Column Repository = new Column()
        {
            Name = "Repository",
            Label = "Repository",
            DataType = DataType.String,
            Length = 256,
            ControlType = "Hidden"
        };
        public static Column FolderName = new Column()
        {
            Name = "FolderName",
            Label = "FolderName",
            DataType = DataType.String,
            Length = 256,
            ControlType = "Hidden"
        };
        public static Column SchemaName = new Column()
        {
            Name = "SchemaName",
            Label = "SchemaName",
            DataType = DataType.String,
            Length = 256,
            ControlType = "Hidden"
        };
        public static Column UserKey = new Column()
        {
            Name = "UserKey",
            Label = "User Key",
            DataType = DataType.String,
            Length = 100,
            ControlType = "TextBox",
            AllowNull = true,
            //ShowInGrid = true,
            DefaultValue = "",
            Order = 100,
            Tooltip = "An user and SEO friendly content key, it is mostly used to customize the page URL"
        };
        public static Column UtcCreationDate = new Column()
        {
            Name = "UtcCreationDate",
            Label = "Creation date",
            AllowNull = true,
            ShowInGrid = true,
            DataType = DataType.DateTime,
            Order = 98,
        };
        public static Column UtcLastModificationDate = new Column()
        {
            Name = "UtcLastModificationDate",
            Label = "UtcLastModificationDate",
            DataType = DataType.DateTime,
            ControlType = "Hidden"
        };
        public static Column Published = new Column()
        {
            Name = "Published",
            Label = "Published",
            DataType = DataType.Bool,
            ControlType = "Checkbox",
            AllowNull = true,
            ShowInGrid = true,
            DefaultValue = "false",
            Order = 99,
        };
        public static Column ParentFolder = new Column()
        {
            Name = "ParentFolder",
            Label = "ParentFolder",
            ControlType = "Hidden",
            DataType = DataType.String,
            Length = 256,
            Order = 99,
        };
        public static Column ParentUUID = new Column()
        {
            Name = "ParentUUID",
            Label = "ParentUUID",
            ControlType = "Hidden",
            DataType = DataType.String,
            Length = 36,
            Order = 99,
        };
        public static Column UserId = new Column()
        {
            Name = "UserId",
            Label = "UserId",
            DataType = DataType.String,
            Length = 36,
            ControlType = "Hidden"
        };

        public static Column OriginalFolder = new Column()
        {
            Name = "OriginalFolder",
            Label = "OriginalFolder",
            ControlType = "Hidden",
            DataType = DataType.String,
            Length = 256,
            Order = 99,
        };
        public static Column OriginalUUID = new Column
        {
            Name = "OriginalUUID",
            Label = "OriginalUUID",
            ControlType = "Hidden",
            DataType = DataType.String,
            Length = 36
        };
        public static Column IsLocalized = new Column
        {
            Name = "IsLocalized",
            Label = "IsLocalized",
            DataType = DataType.Bool,
            ControlType = "Hidden"
        };

        public static Column Sequence = new Column()
        {
            Name = "Sequence",
            Label = "Sequence",
            ControlType = "Hidden",
            DataType = DataType.Int,
            Order = 99,
        };
        #endregion

        #region Persistence field
        /// <summary>
        /// Gets or sets the name.
        /// Unable to update.
        /// </summary>
        /// <value>The name.</value>
        [DataMember(Order = 1)]
        public string Name { get; set; }
        [DataMember(Order = 3)]
        public string Label { get; set; }
        [DataMember(Order = 5)]
        public DataType DataType { get; set; }
        [DataMember(Order = 6)]
        public string ControlType { get; set; }
        private bool allowNull = true;
        [DataMember(Order = 7)]
        public bool AllowNull { get { return allowNull; } set { allowNull = value; } }
        [DataMember(Order = 9)]
        public int Length { get; set; }
        [DataMember(Order = 11)]
        public int Order { get; set; }
        //[DataMember(Order = 13)]
        //public bool Queryable { get; set; }
        private bool modifiable = true;
        [DataMember(Order = 15)]
        public bool Modifiable { get { return modifiable; } set { modifiable = value; } }
        private bool indexable = true;
        [DataMember(Order = 17)]
        public bool Indexable { get { return indexable; } set { indexable = value; } }
        [DataMember(Order = 19)]
        public bool ShowInGrid { get; set; }
        [DataMember(Order = 21)]
        public string Tooltip { get; set; }
        [DataMember(Order = 22)]
        public SelectionSource SelectionSource { get; set; }

        private SelectListItem[] _selectionItems;
        [DataMember(Order = 23)]
        public SelectListItem[] SelectionItems
        {
            get
            {
                return _selectionItems ?? (_selectionItems = new SelectListItem[0]);
            }
            set
            {
                this._selectionItems = value;
            }
        }

        [DataMember(Order = 24)]
        public string SelectionFolder { get; set; }

        private ColumnValidation[] _validations;
        [DataMember(Order = 25)]
        public ColumnValidation[] Validations
        {
            get
            {
                return _validations ?? (_validations = new ColumnValidation[0]);
            }
            set
            {
                this._validations = value;
            }
        }
        [DataMember(Order = 27)]
        public string DefaultValue { get; set; }
        [DataMember(Order = 29)]
        public bool Summarize { get; set; }
        private Dictionary<string, string> customSettings;
        [DataMember(Order = 31)]
        public Dictionary<string, string> CustomSettings
        {
            get { return customSettings ?? (customSettings = new Dictionary<string, string>()); }
            set
            {
                if (value != null)
                {
                    customSettings = new Dictionary<string, string>(value, StringComparer.OrdinalIgnoreCase);
                }
                else
                {
                    customSettings = value;
                }

            }
        }
        #endregion

        #region GetValidations
        public IEnumerable<ColumnValidation> GetValidations()
        {
            List<ColumnValidation> validations = new List<ColumnValidation>();
            if (this.AllowNull == false)
            {
                validations.Add(new RequiredValidation() { ErrorMessage = "The field {0} cannot be null or empty.".Localize() });
            }
            validations.AddRange(this.Validations);
            return validations;
        }
        #endregion

        #region override object
        public static bool operator ==(Column obj1, Column obj2)
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
        public static bool operator !=(Column obj1, Column obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Column))
            {
                return false;
            }
            if (obj == null)
            {
                return false;
            }
            if (string.Compare(this.Name, ((Column)obj).Name, true) == 0)
            {
                return true;
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return base.GetHashCode();
            }
            else
            {
                return this.Name.ToLower().GetHashCode();
            }
        }
        #endregion

        #region Clone
        public Column DeepClone()
        {
            var column = (Column)this.MemberwiseClone();
            return column;
        }
        #endregion

        #region IsSystemField
        public bool IsSystemField
        {
            get
            {
                return Sys_Fields.Where(it => it.EqualsOrNullEmpty(this.Name, StringComparison.OrdinalIgnoreCase)).Count() != 0;
            }
            set { }
        }
        #endregion
    }

    #region ColumnComparer
    /// <summary>
    /// 
    /// </summary>
    public class ColumnComparer : IEqualityComparer<Column>
    {
        public bool Equals(Column x, Column y)
        {
            return x.Name.EqualsOrNullEmpty(y.Name, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(Column obj)
        {
            return obj.GetHashCode();
        }
    }
    #endregion

    [DataContract]
    [Serializable]
    public partial class Schema : IRepositoryElement, ISchema
    {
        public Schema()
        { }
        public Schema(Repository repository, string name)
            : this(repository, name, null)
        {

        }
        public Schema(Repository repository, string name, IEnumerable<Column> columns)
        {
            if (repository != null)
            {
                this.Repository = new Repository(repository.Name);
            }
            this.Name = name;

            if (columns != null)
            {
                this.columns.AddRange(columns);
            }
        }
        [DataMember(Order = 1)]
        public string Name { get; set; }

        //[DataMember(Order = 5)]
        //public bool Attachable { get; set; }

        [XmlIgnore]
        public Repository Repository { get; set; }


        private List<Column> columns = new List<Column>();
        [DataMember(Order = 7)]
        public List<Column> Columns
        {
            get
            {
                return columns;
            }
            set
            {
                columns = value;
            }
        }

        public void AddColumn(Column column)
        {
            this.columns.Add(column);
        }

        public int RemoveColumn(Column column)
        {
            var index = this.columns.IndexOf(column);
            this.columns.Remove(column);
            return index;
        }
        public int UpdateColumn(Column oldColumn, Column newColumn)
        {
            var index = RemoveColumn(oldColumn);
            this.columns.Insert(0, newColumn);
            return index;
        }

        //[DataMember(Order = 9)]
        //public string[] ChildSchemas { get; set; }

        private bool templateBuildByMachine = true;
        [DataMember(Order = 11)]
        [Obsolete()]
        public bool TemplateBuildByMachine
        {
            get
            {
                return templateBuildByMachine;
            }
            set
            {
                templateBuildByMachine = value;
            }
        }


        #region ISchema Members

        IEnumerable<IColumn> ISchema.Columns
        {
            get { return Columns.Concat(new[] { Column.UtcCreationDate, Column.Published }).OrderBy(it => it.Order); }
        }
        IColumn ISchema.this[string columnName]
        {
            get { return this[columnName]; }
        }

        #endregion
        [DataMember(Order = 12)]
        public bool IsTreeStyle
        {
            get;
            set;
        }

    }

    public partial class Schema : IPersistable, IIdentifiable
    {

        #region IPersistable Members
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

        bool isDummy = true;
        public bool IsDummy
        {
            get { return isDummy; }
            set { isDummy = value; }
        }

        void IPersistable.Init(IPersistable source)
        {
            this.isDummy = false;
            this.Name = ((Schema)source).Name;
            this.Repository = ((Schema)source).Repository;
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
        public static bool operator ==(Schema obj1, Schema obj2)
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
        public static bool operator !=(Schema obj1, Schema obj2)
        {
            return !(obj1 == obj2);
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Schema))
            {
                return false;
            }
            if (obj != null)
            {
                var schema = (Schema)obj;
                if (this.Repository == schema.Repository && string.Compare(this.Name, schema.Name, true) == 0)
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
            return this.Name;
        }
        #endregion

        public Column this[string columnName]
        {
            get
            {
                return this.AllColumns.Where(it => string.Compare(it.Name, columnName, true) == 0).FirstOrDefault();
            }
        }

        public Column GetSummarizeColumn()
        {
            var summarizeField = this.columns.OrderBy(it => it.Order).Where(it => it.Summarize == true).FirstOrDefault();
            if (summarizeField == null)
            {
                summarizeField = this.columns.OrderBy(it => it.Order).FirstOrDefault();
            }
            if (summarizeField == null)
            {
                summarizeField = Column.UserKey;
            }
            return summarizeField;
        }

        public Schema DeepClone()
        {
            var schema = (Schema)this.MemberwiseClone();
            if (this.Columns != null)
            {
                schema.columns = new List<Column>();

                foreach (var item in this.Columns)
                {
                    schema.AddColumn(item.DeepClone());
                }
            }
            return schema;
        }


        /// <summary>
        /// =GetSummarizeColumn
        /// </summary>
        public IColumn TitleColumn
        {
            get
            {
                return this.GetSummarizeColumn();
            }
        }

        public IEnumerable<Column> AllColumns
        {
            get
            {
                var sysColumns = new[] {
                    Column.Id,
                    Column.UUID, 
                    Column.Repository,
                    Column.SchemaName,
                    Column.FolderName,
                    Column.UserKey,
                    Column.UtcCreationDate,
                    Column.UtcLastModificationDate,
                    Column.Published,
                    Column.ParentFolder,
                    Column.ParentUUID,
                    Column.UserId,
                    Column.OriginalFolder,
                    Column.OriginalUUID,
                    Column.IsLocalized,
                    Column.Sequence
                };
                if (this.Columns == null)
                {
                    return sysColumns;
                }
                return this.Columns.OrderBy(o => o.Order).Concat(sysColumns).Distinct();
            }
        }
    }
}
