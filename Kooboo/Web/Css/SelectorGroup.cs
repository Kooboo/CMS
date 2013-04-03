using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Web.Css
{
    public class SelectorGroup : List<Selector>
    {
        public override string ToString()
        {
            return String.Join(", ", this.Select(o => o.ToString()));
        }
    }
}
