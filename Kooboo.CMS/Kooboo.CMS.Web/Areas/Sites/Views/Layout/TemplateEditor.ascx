<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.Layout>" %>
<%
    
    var body = (Model ==null || string.IsNullOrEmpty(Model.Body)) ? (string)(ViewBag.DefaultLayout == null ? "" : ViewBag.DefaultLayout.Template) : Model.Body;
%>
<tr>
    <td>
        <%: Html.TextArea("Body",body, new { rows = 30, cols = 20 } )%>
    </td>
    <td style="width: 250px;">
        <div class="task-panel">
            <div class="task-block">
                <h3 class="title">
                    <span>
                        <%="Positions".Localize() %></span><span class="arrow"></span></h3>
                <div class="content">
                    <p class="buttons clearfix">
                        <a href="#" class="button addPosition">Add</a></p>
                    <ul class="list positions">
                    </ul>
                </div>
            </div>
            <div class="task-block">
                <h3 class="title">
                    <span>
                        <%="Layout helper".Localize() %></span><span class="arrow"></span></h3>
                <div class="content block-list">
                    <ul>
                        <li class="has-sub codeSample"><a href="javascript:;">
                            <%="HTML header".Localize() %></a>
                            <%: Html.Partial("CodeSnippets")%>
                        </li>
                        <li class="has-sub layoutSamples"><a href="javascript:;">
                            <%="Layouts".Localize() %></a>
                            <%:Html.Partial("LayoutSamples", ViewData["LayoutSamples"])%>
                        </li>
                        <li class="has-sub last viewTools"><a href="javascript:;">
                            <%="Add views".Localize() %></a>
                            <%:Html.Partial("ViewList",Kooboo.CMS.Web.Areas.Sites.Models.ViewDataSource.GetNamespace())%>
                        </li>
                    </ul>
                </div>
            </div>
            <%:Html.Partial("CodeHelper") %>
            <%:Html.EditorFor(m=>m.Plugins) %>
        </div>        
    </td>
</tr>
<%:Html.Partial("Layout.Script")%>