using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Form.Html.Controls
{
    public class MultiFiles : ControlBase
    {
        public override string Name
        {
            get { return "MultiFiles"; }
        }

        protected override string RenderInput(IColumn column)
        {
            string formater =
@"
@{{ var data{0} = !string.IsNullOrEmpty( Model.{0} )? string.Join( ""|"", ((string)Model.{0}).Split('|').Select(o=>string.IsNullOrEmpty(o)? """": Url.Content(o))) : """" ;}}
<input type=""hidden"" class=""multifile"" name=""{0}"" value=""@Html.Raw(data{0})"" config=""{{ containerId:'container-{0}',templateId:'template-{0}',addButtonId:'add-{0}', propertyName:'{0}', mediaLinkId :'media-{0}' }}""/>
<div class=""multifiles"">
<ul class=""upload-list clearfix"" id=""container-{0}"">
    <li id=""template-{0}"" class=""uploader"">
        <span class=""preview""></span><input type=""file"" class="" no-style""/>  <a href=""javascript:;"" class=""o-icon remove"" title=""@(""Remove"".Localize())"">Del</a>
    </li>
</ul>
<a class=""o-icon add"" href=""javascript:;"" id=""add-{0}"">Add </a><a href=""@Url.Action(""Selection"",""MediaContent"",new {{ SiteName = ViewContext.RequestContext.GetRequestValue(""SiteName""), RepositoryName = ViewContext.RequestContext.GetRequestValue(""RepositoryName"")}})"" class=""o-icon folder-add"" title=""@(""From media library"".Localize())"" id=""media-{0}"">@(""From Medialibrary"".Localize())</a></div>";

            return string.Format(formater, column.Name);
        }
        public override bool IsFile
        {
            get
            {
                return true;
            }
        }
        public override string GetValue(object oldValue, string newValue)
        {
            string value = "";
            if (oldValue == null || string.IsNullOrEmpty(oldValue.ToString()))
            {
                value = newValue;
            }
            else
            {
                value = oldValue.ToString().Trim('|') + "|" + newValue;
            }
            return value;
        }
    }
}
