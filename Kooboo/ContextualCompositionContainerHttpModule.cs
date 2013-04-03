using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo
{
    public class ContextualCompositionContainerHttpModule:IHttpModule
    {
        public void Dispose()
        {
            
        }

        public void Init(HttpApplication context)
        {
            context.EndRequest += new EventHandler(context_EndRequest);
        }

        void context_EndRequest(object sender, EventArgs e)
        {
            var context = HttpContext.Current.Items[CallContext.CONTEXT_CONTAINER] as CallContext;
            //context.Container.Dispose();

            foreach (var item in HttpContext.Current.Items)
            {
                if(item is IDisposable)
                {
                    (item as IDisposable).Dispose();
                }
            }


            HttpContext.Current.Items.Clear();
        }
    }
}
