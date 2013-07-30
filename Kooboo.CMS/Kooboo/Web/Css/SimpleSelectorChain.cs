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

namespace Kooboo.Web.Css
{
    public class SimpleSelectorChain : List<SimpleSelector>
    {
        public int Specificity
        {
            get
            {
                return this.Sum(o => o.Specificity);
            }
        }
        public override string ToString()
        {
            return String.Join(String.Empty, this.Select(o => o.ToString()));
        }

        public string ToStringWithoutPseudoElementSelector()
        {
            return String.Join(String.Empty, this.Where(o => o.Type != SelectorType.PseudoElement).Select(o => o.ToString()));
        }
    }
}
