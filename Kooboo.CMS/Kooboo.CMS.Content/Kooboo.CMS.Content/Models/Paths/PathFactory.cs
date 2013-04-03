using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Models.Paths
{
    public static class PathFactory
    {
        static IDictionary<Type, Type> paths = new Dictionary<Type, Type>();
        static PathFactory()
        {
            paths.Add(typeof(Repository), typeof(RepositoryPath));
            paths.Add(typeof(Schema), typeof(SchemaPath));
            paths.Add(typeof(Folder), typeof(FolderPath));
            paths.Add(typeof(Workflow), typeof(WorkflowPath));
            paths.Add(typeof(ReceivingSetting), typeof(ReceivingSettingPath));
            paths.Add(typeof(SendingSetting), typeof(SendingSettingPath));
            paths.Add(typeof(PendingWorkflowItem), typeof(PendingWorkflowItemPath));
            paths.Add(typeof(WorkflowHistory), typeof(WorkflowHistoryPath));
        }
        public static void Register(Type type, Type pathType)
        {
            lock (paths)
            {
                paths.Add(type, pathType);
            }            
        }

        public static IPath GetPath<T>(T o)
        {
            Type type = typeof(T);

            foreach (KeyValuePair<Type, Type> item in paths)
            {
                if (item.Key.IsAssignableFrom(type))
                {
                    return (IPath)Activator.CreateInstance(item.Value, o);
                }
            }
            return null;
        }
    }
}
