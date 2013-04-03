using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Globalization
{
    public class ElementTicket
    {
        public string Key
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.Value;
        }
    }
}
