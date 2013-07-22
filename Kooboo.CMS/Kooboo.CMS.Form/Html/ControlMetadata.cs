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

namespace Kooboo.CMS.Form.Html
{
    public class ControlMetadata
    {
        public string Name { get; set; }

        public string DataType { get; set; }
    }

    public class ControlMetadataComparer : IEqualityComparer<ControlMetadata>
    {
        public bool Equals(ControlMetadata x, ControlMetadata y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(ControlMetadata obj)
        {
            return obj.GetHashCode();
        }
    }
}
