using Kooboo.Web.Mvc.Grid2;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2
{
    public class TooltipGridItemColumn : GridItemColumn
    {
        public TooltipGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }



        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            var text = System.Web.HttpUtility.HtmlEncode(this.PropertyValue == null ? string.Empty : this.PropertyValue.ToString());
            var isOverstep = text.Length > this.MaxLength;
            var data = isOverstep ? text.Substring(0, this.MaxLength) + "..." : text;
            return new MvcHtmlString(isOverstep ? string.Format("<span class=\"tooltip\" title=\"{0}\">{1}</span>",
                text,
                data) : text);
        }

        private int _maxLength = 15;
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                _maxLength = value;
            }
        }
    }
}