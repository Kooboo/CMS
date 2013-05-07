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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IWorkflowProvider : IContentElementProvider<Workflow>
    {
    }

    public interface IPendingWorkflowItemProvider : IProvider<PendingWorkflowItem>
    {
        IEnumerable<PendingWorkflowItem> All(Repository repository, string roleName);
    }
    public interface IWorkflowHistoryProvider : IProvider<WorkflowHistory>
    {
        IEnumerable<WorkflowHistory> All(TextContent content);
    }
}
