using System.Collections.Generic;

namespace Kooboo.Web.Mvc
{
    static class ScriptCollectionPool
    {
        static ScriptCollectionPool()
        {
            Collections = new Dictionary<string, ScriptCollection>();
        }

        static Dictionary<string, ScriptCollection> Collections
        {
            get;
            set;
        }

        internal static ScriptCollection FindScripts(string folder)
        {
            if (Collections.ContainsKey(folder))
            {
                return Collections[folder];
            }
            else
            {
                var collection = new ScriptCollection(folder);
                Collections.Add(folder, collection);
                return collection;
            }
        }
    }
}
