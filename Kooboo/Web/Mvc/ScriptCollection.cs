using System.Collections.Generic;

namespace Kooboo.Web.Mvc
{
    class ScriptCollection
    {
        internal ScriptCollection(string folder)
        {
            Scripts = new Dictionary<string, ScriptObject>();

            var directory = System.IO.Directory.CreateDirectory(folder);

            foreach (var i in directory.GetFiles("*.js", System.IO.SearchOption.AllDirectories))
            {
                var script = new ScriptObject(i);
                if (Scripts.ContainsKey(script.Name) == false)
                {
                    Scripts.Add(script.Name.ToLowerInvariant(), script);
                }

            }

            foreach (var i in Scripts.Values)
            {
                i.ScanReferences(this);
            }
        }

        Dictionary<string,ScriptObject> Scripts
        {
            get;
            set;
        }

        public bool ContainsKey(string key)
        {
            return this.Scripts.ContainsKey(key);
        }

        public ScriptObject Get(string key)
        {
            return this.Scripts[key];
        }
    }
}
