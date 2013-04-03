using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Dynamic.Calculator
{
    public class CalculateExpression : Exception, IKoobooException
    {
        public CalculateExpression(string msg)
            : base(msg)
        {
        }
        public CalculateExpression(string msg, Exception inner)
            : base(msg, inner)
        {
        }
    }
}
