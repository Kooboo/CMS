using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Kooboo.Domain
{
    public interface IEntityCommand
    {
        void Execute();
    }
}
