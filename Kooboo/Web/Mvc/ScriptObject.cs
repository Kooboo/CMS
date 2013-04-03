using System.Collections.Generic;
using System.IO;

namespace Kooboo.Web.Mvc
{
    public class ScriptObject
    {
        public ScriptObject(FileInfo file)
        {
            this.FullName = file.FullName;
            this.Name = file.Name;
            this.References = new List<ScriptObject>();
        }

        internal void ScanReferences(ScriptCollection scripts)
        {
            using (StreamReader reader = File.OpenText(this.FullName))
            {
                while (true)
                {
                    if (reader == null)
                    {
                        break;
                    }

                    var reference = reader.ReadLine();

                    if (reference == null)
                    {
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(reference))//blank line
                    {
                        continue;
                    }

                    // ///<reference path="../jquery.validate.js" />
                    if (reference.StartsWith("///") && reference.IndexOf("<reference path=") > -1)
                    {
                        var path = reference.Split('\"')[1];

                        var segments = path.Split('/');

                        var fileName = segments[segments.Length - 1].ToLowerInvariant();

                        if (scripts.ContainsKey(fileName))
                        {
                            var script = scripts.Get(fileName);
                            this.References.Add(script);
                        }

                        continue;
                    }

                    break;
                }
            }
        }

        public List<ScriptObject> References
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string FullName
        {
            get;
            set;
        }
    }
}
