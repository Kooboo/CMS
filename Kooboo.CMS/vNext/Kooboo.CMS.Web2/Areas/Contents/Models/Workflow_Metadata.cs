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
using System.Web;
using Kooboo.CMS.Content.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Web2.Models;

using System.Web.Mvc;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Grid.Design;
using Kooboo.CMS.Web2.Grid2;
using Kooboo.CMS.Web2.Areas.Contents.Models.Grid2;
using System.ComponentModel;

namespace Kooboo.CMS.Web2.Areas.Contents.Models
{
    [MetadataFor(typeof(Workflow))]
    [Grid(Checkable = true, IdProperty = "UUID")]
    public class Workflow_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [Required]
        [RemoteEx("IsNameAvailable", "Workflow", RouteFields = "RepositoryName,SiteName", AdditionalFields = "old_Key")]
        public string Name { get; set; }

        [UIHint("WorkflowItems")]
        public WorkflowItem[] Items { get; set; }
    }



    [MetadataFor(typeof(PendingWorkflowItem))]
    [Grid(Checkable = false, IdProperty = "UUID")]
    [GridColumn(GridColumnType = typeof(ActionGridColumn), GridItemColumnType = typeof(PendingWorflowItem_Edit_GridItemColumn), HeaderText = "Edit content", Order = 4)]
    [GridColumn(GridColumnType = typeof(ActionGridColumn), GridItemColumnType = typeof(PendingWorflowItem_Process_GridItemColumn), HeaderText = "Process", Order = 5)]
    public class PendingWorkflowItem_Metadata
    {
        [GridColumn(HeaderText = "Step name", Order = 1, GridItemColumnType = typeof(PendingWorflowItem_Process_GridItemColumn))]
        [Display(Name = "Step name")]
        public string ItemDisplayName { get; set; }

        [GridColumn(Order = 2, HeaderText = "Workflow name")]
        [Display(Name = "Workflow name")]
        public string WorkflowName { get; set; }

        [Display(Name = "Workflow item sequence")]
        public string WorkflowItemSequence { get; set; }

        [GridColumn(Order = 3, HeaderText = "Role name")]
        [Display(Name = "Role")]
        public string RoleName { get; set; }

        public string ContentFolder { get; set; }

        public string ContentUUID { get; set; }


        public string ContentSummary { get; set; }

        [DisplayName("Previous comments")]
        public string PreviousComment { get; set; }

        [Display(Name = "Creation date")]
        public DateTime CreationUtcDate { get; set; }

        [Display(Name = "Author")]
        [GridColumn(Order = 12, HeaderText = "Author")]
        public string CreationUser { get; set; }


    }

    [MetadataFor(typeof(WorkflowHistory))]
    [Grid(Checkable=false)]
    public class WorkflowHistory_Metadata
    {
        [GridColumn(HeaderText = "Flow order", Order = 1)]
        public int Id { get; set; }

        [GridColumn(HeaderText = "Workflow", Order = 2)]
        public string WorkflowName { get; set; }

        [GridColumn(HeaderText = "Sequence", Order = 3)]
        public string WorkflowItemSequence { get; set; }

        [GridColumn(HeaderText = "Step name", Order = 4)]
        public string ItemDisplayName { get; set; }

        [GridColumn(HeaderText = "Role", Order = 5)]
        public string RoleName { get; set; }

        public string ContentUUID { get; set; }

        [GridColumn(HeaderText = "Content summary", Order = 6)]
        public string ContentSummary { get; set; }

        [GridColumn(GridItemColumnType = typeof(BooleanGridItemColumn), HeaderText = "Passed", Order = 7)]
        public bool Passed { get; set; }

        [GridColumn(HeaderText = "Processer", Order = 8)]
        public string ProcessingUser { get; set; }

        [GridColumn(HeaderText = "Processed date", GridItemColumnType = typeof(DateTimeGridItemColumn), Order = 9)]
        public DateTime ProcessingUtcDate { get; set; }

        [GridColumn(Order = 10)]
        public string Comment { get; set; }

        [GridColumn(GridItemColumnType = typeof(BooleanGridItemColumn), HeaderText = "Finished", Order = 11)]
        public bool Finished { get; set; }
    }
}