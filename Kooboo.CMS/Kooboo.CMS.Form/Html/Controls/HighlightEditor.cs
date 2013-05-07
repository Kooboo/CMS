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

namespace Kooboo.CMS.Form.Html.Controls
{
    public class HighlightEditor : ControlBase
    {
        public override string Name
        {
            get { return "HighlightEditor"; }
        }

        protected override string RenderInput(IColumn column)
        {
            return string.Format(@"
<textarea name=""{0}"" id=""{0}"" class=""{0} codemirror"" media_library_url=""@Url.Action(""Selection"",""MediaContent"",ViewContext.RequestContext.AllRouteValues()))""  media_library_title =""@(""Selected Files"".Localize())"" rows=""10"" cols=""100"">@( Model.{0} ?? """")</textarea>
", column.Name);
        }
    }
}
