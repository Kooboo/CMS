using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Domain
{
    public interface ITransactionContext
    {
        bool IsStarted
        {
            get;
        }

        void Begin();

        void Commit();
     
        void Rollback();
    }
}
