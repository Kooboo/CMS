using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Routing
{
    public sealed class ParameterSubsegment : PathSubsegment
    {
        // Methods
        public ParameterSubsegment(string parameterName)
        {
            if (parameterName.StartsWith("*", StringComparison.Ordinal))
            {
                this.ParameterName = parameterName.Substring(1);
                this.IsCatchAll = true;
            }
            else
            {
                this.ParameterName = parameterName;
            }
        }

        // Properties
        public bool IsCatchAll { get; private set; }

        public string ParameterName { get; private set; }

    }


}
