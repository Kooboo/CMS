using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Misc
{
    public static class RenderHelper
    {
        public static string RenderBool(bool? value)
        {
            var tip = "-".Localize();
            if (value.HasValue && value.Value == true)
            {
                tip = "YES".Localize();
            }
            return tip;
        }
    }
}