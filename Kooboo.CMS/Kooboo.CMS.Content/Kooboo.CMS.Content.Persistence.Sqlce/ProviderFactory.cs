using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Sqlce
{
    public class ProviderFactory : Default.ProviderFactory
    {
        public ProviderFactory()
        {
            RegisterProvider<ISchemaProvider>(new SchemaProvider());
            RegisterProvider<ITextContentProvider>(new TextContentProvider());
        }

        #region IProviderFactory Members

        public override string Name
        {
            get { return "Sqlce"; }
        }

     
        #endregion        
    }
}
