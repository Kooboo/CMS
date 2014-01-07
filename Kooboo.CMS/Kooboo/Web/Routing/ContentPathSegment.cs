#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Routing
{
    public sealed class ContentPathSegment : PathSegment
    {
        // Methods
        public ContentPathSegment(IList<PathSubsegment> subsegments)
        {
            this.Subsegments = subsegments;
        }

        // Properties
        public bool IsCatchAll
        {
            get
            {
                return this.Subsegments.Any<PathSubsegment>(seg => ((seg is ParameterSubsegment) && ((ParameterSubsegment)seg).IsCatchAll));
            }
        }

        public IList<PathSubsegment> Subsegments { get; private set; }

    }


}
