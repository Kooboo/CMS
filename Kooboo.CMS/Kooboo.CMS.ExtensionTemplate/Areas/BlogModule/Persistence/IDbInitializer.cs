using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence
{
    public interface IDbInitializer
    {
        void InitializeDb(string connectionString);
        void DeleteDb(string connectionString);
    }
}
