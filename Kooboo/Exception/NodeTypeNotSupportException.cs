using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace Kooboo
{
    public class NodeTypeNotSupportException : Exception, IKoobooException
    {
        public NodeTypeNotSupportException(ExpressionType expressionType)
            : base(expressionType.ToString() + "is not supported yet.")
        {
        }
    }
}
