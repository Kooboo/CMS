using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.DataRule;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class WhereClause_Metadata
    {
        public static IEnumerable<SelectListItem> LogicDropDownList(string selected)
        {
            return ParseEnumToDropDownList(typeof(Logical), selected);
        }
        public static IEnumerable<SelectListItem> OperatorDropDownList(string selected)
        {
            return ParseEnumToDropDownList(typeof(Operator), selected);
        }
        public static IEnumerable<SelectListItem> ParseEnumToDropDownList(Type enumType, string selected)
        {
            return Enum.GetNames(enumType).Select(o => new SelectListItem()
            {
                Text = o,
                Value = ((int)Enum.Parse(enumType, o)).ToString(),
                Selected = o == selected
            });
        }
    }
}