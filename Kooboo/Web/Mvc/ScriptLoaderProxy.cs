
namespace Kooboo.Web.Mvc
{

    public class ScriptLoaderProxy : ScriptLoader
    {
        ScriptLoader Loader
        {
            get;
            set;
        }
        public ScriptLoaderProxy(ScriptLoader loader)
        {
            this.Loader = loader;
        }

        public override void Require(string name)
        {
            this.Loader.Require(name);
        }

        public override void Run(string code,params object[] paras)
        {
            this.Loader.Run(code,paras);
        }

    }
}
