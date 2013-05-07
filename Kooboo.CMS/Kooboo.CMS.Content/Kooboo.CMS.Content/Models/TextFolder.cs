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
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Models
{

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

        [DataMember(Order = 13)]
        public string[] EmbeddedFolders { get; set; }

        [DataMember(Order = 14)]
        public string WorkflowName { get; set; }

        [DataMember(Order = 15)]
        public string[] Roles { get; set; }

        private OrderSetting orderSetting;
        [DataMember(Order = 16)]
        [Obsolete]
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

        [DataMember()]
        public bool? Sortable
        {
            get;
            set;
        }

        private bool? _enablePaging;
        [DataMember()]
        public bool? EnablePaging
        {
            get
            {
                if (!_enablePaging.HasValue)
                {
                    return true;
                }
                return _enablePaging;
            }
            set
            {
                _enablePaging = value;
            }
        }

        private bool? visibleOnSidebarMenu;
        [Obsolete("please use Hidden.")]
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

        private bool? hidden;
        [DataMember(Order = 20)]
        public bool? Hidden
        {
            get
            {
                if (!hidden.HasValue && VisibleOnSidebarMenu.HasValue)
                {
                    //compatible with VisibleOnSidebarMenu setting
                    return !VisibleOnSidebarMenu.Value;
                }
                return hidden;
            }
            set
            {
                hidden = value;
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


        /// <summary>
        /// Gets a value indicating whether this <see cref="TextFolder" /> is visible.
        /// determine by Repository.ShowHiddenFolder and Hidden value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible
        {
            get
            {
                if (Repository != null)
                {
                    return Repository.AsActual().ShowHiddenFolders || this.AsActual().Hidden == null || this.AsActual().Hidden.Value == false;
                }
                else
                {
                    return true;
                }
            }
        }

    }
}
