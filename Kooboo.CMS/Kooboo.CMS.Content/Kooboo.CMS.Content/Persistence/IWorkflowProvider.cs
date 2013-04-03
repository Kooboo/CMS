using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence
{
    public interface IWorkflowProvider : IProvider<Workflow>
    {
    }

    public interface IPendingWorkflowItemProvider : IProvider<PendingWorkflowItem>
    {
        IQueryable<PendingWorkflowItem> All(Repository repository, string roleName);
    }
    public interface IWorkflowHistoryProvider : IProvider<WorkflowHistory>
    {
        IQueryable<WorkflowHistory> All(TextContent content);
    }
}
