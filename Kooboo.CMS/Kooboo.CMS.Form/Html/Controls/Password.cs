using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class Password : Input
    {
        public override string Type
        {
            get { return "Password"; }
        }

        public override string Name
        {
            get { return "Password"; }
        }
    }
}
