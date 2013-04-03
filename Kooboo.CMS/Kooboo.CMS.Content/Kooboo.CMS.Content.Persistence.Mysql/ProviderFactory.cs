using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Kooboo.CMS.Content.Persistence.Default;
using Kooboo.CMS.Content.Models;

namespace Kooboo.CMS.Content.Persistence.Mysql
{
    public class ProviderFactory : Default.ProviderFactory
    {
        public const string ProviderName = "Mysql";
        public ProviderFactory()
        {
            RegisterProvider<IRepositoryProvider>(new RepositoryProvider());
            RegisterProvider<ISchemaProvider>(new SchemaProvider());
            RegisterProvider<ITextContentProvider>(new TextContentProvider());
        }

        #region IProviderFactory Members

        public override string Name
        {
            get { return ProviderName; }
        }

        #endregion

    }
}
