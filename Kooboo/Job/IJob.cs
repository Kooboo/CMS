using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Job
{
    public interface IJob
    {
        void Execute(object executionState);
        void Error(Exception e);
    }
}
