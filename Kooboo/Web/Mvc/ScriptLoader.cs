using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.ComponentModel.Composition;
using Kooboo.IoC;

namespace Kooboo.Web.Mvc
{
    [Export(typeof(ScriptLoader))]
    public class ScriptLoader:IDisposable
    {

        public ScriptLoader()
        {
            this.Loaded = new List<ScriptObject>();
            this.CodeBlocks = new List<string>();
        }

        public List<ScriptObject> Loaded
        {
            get;
            set;
        }

        ScriptCollection Scripts
        {
            get;
            set;
        }

        public virtual void Require(string name)
        {
            var lowerName = name.ToLowerInvariant();
            if (this.Scripts.ContainsKey(lowerName))
            {
                var script = this.Scripts.Get(lowerName);

                if(this.Loaded.Contains(script) == false)                
                {
                    if (script != null)
                    {
                        foreach (var i in script.References)
                        {
                            Require(i.Name);
                        }

                        Loaded.Add(script);
                    }
                }
            }
        }

        public string Load()
        {
            StringBuilder builder = new StringBuilder();

            foreach (var i in Loaded)
            {
                builder.AppendFormat("<script type=\"text/javascript\" src=\"{0}\" charset=\"utf-8\" ></script>", this.ResolveClientUrl(i.FullName));
                builder.AppendLine();
            }

            builder.AppendLine("<script type=\"text/javascript\">");

            foreach (var i in this.CodeBlocks)
            {
                builder.Append(i);
                builder.AppendLine();
            }

            builder.AppendLine("</script>");


            return builder.ToString();
        }

        List<string> CodeBlocks
        {
            get;
            set;
        }

        public virtual void Run(string code,params object[] paras)
        {
            if (code != null)
            {
                if (paras != null && paras.Length > 0)
                {
                    this.CodeBlocks.Add(string.Format(code, paras));
                }
                else
                {
                    this.CodeBlocks.Add(code);
                }
            }
        }



        string ResolveClientUrl(string PhysicalPath)
        {
            var root = HttpContext.Current.Server.MapPath("~");

            var relativePath = PhysicalPath.Substring(root.Length - 1, PhysicalPath.Length - root.Length + 1);

            return relativePath.Replace("\\", "/");

        }

        public static ScriptLoader RegisterInstance(string folder)
        {
            var loader = ContextContainer.Current.Resolve<ScriptLoader>(folder);

            if (loader.IsRegistered)
            {
                return new ScriptLoaderProxy(loader);
            }
            else
            {
                loader.RegisterThis(folder);
                return loader;
            }
        }

        void RegisterThis(string folder)
        {
            this._IsRegistered = true;

            this.Loaded = new List<ScriptObject>();

            var fullName = HttpContext.Current.Server.MapPath(folder);

            this.Scripts = ScriptCollectionPool.FindScripts(fullName);
        }

        bool _IsRegistered = false;
        public bool IsRegistered
        {
            get
            {
                return this._IsRegistered;
            }
            private set
            {
                this._IsRegistered = value;
            }
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
                var writer = HttpContext.Current.Response;

                writer.Write(this.Load());
            }
        }

 

 


 

 

    }
}
