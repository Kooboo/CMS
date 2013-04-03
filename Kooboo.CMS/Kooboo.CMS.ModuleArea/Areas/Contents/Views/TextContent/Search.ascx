<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%@ Import Namespace="Kooboo.CMS.Sites.DataRule" %>
<%
    
    var action = ViewContext.RequestContext.AllRouteValues()["action"].ToString();
    var schema = (Kooboo.CMS.Content.Models.Schema)ViewData["Schema"];
    var folder = (Kooboo.CMS.Content.Models.TextFolder)ViewData["Folder"];
    var guid = Guid.NewGuid();
    var operatorList = new List<System.Web.Mvc.SelectListItem> { 
    new System.Web.Mvc.SelectListItem{ Text = "Equals", Value="0"},
    new System.Web.Mvc.SelectListItem{ Text = "NotEquals", Value="1"},
    new System.Web.Mvc.SelectListItem{ Text = "Greater than", Value="2"},
    new System.Web.Mvc.SelectListItem{ Text = "Less than", Value="3"},
    new System.Web.Mvc.SelectListItem{ Text = "Contains", Value="4"},
    new System.Web.Mvc.SelectListItem{ Text = "Start with", Value="5"},
    new System.Web.Mvc.SelectListItem{ Text = "End with", Value="6"}
  };

    var logicalList = new List<System.Web.Mvc.SelectListItem> { 
    new System.Web.Mvc.SelectListItem{ Text = "AND", Value="0"},
    new System.Web.Mvc.SelectListItem{ Text = "OR", Value="1"},
    new System.Web.Mvc.SelectListItem{ Text = "Then AND", Value="2"},
    new System.Web.Mvc.SelectListItem{ Text = "Then OR", Value="3"}
  };
%>
<div class="search-panel">
    <div class="search-form">
        <form id="search" action="<%=Url.Action(action,ViewContext.RequestContext.AllRouteValues().Merge("page","1")) %>"
        method="get" class="no-ajax">
         <% foreach (var routeValue in ViewContext.RequestContext.AllRouteValues())
           {
               if (routeValue.Key.ToLower() != "page" && routeValue.Key.ToLower() != "search")
               {%>
           <input type="hidden" name="<%: routeValue.Key %>" value="<%:routeValue.Value %>" />
        <%}
           }%>
        
        <input type="text" name="search" id="searchBox" value="<%=ViewContext.RequestContext.GetRequestValue("search") %>" /><%--<img id="removeSearch" alt="remove search!" src="/Areas/Contents/Styles/icons/cross.png" />--%>
        <button type="submit">
            <%="Search".Localize() %></button>
        </form>
    </div>
    <%if (folder != null)
      {%>
    <a class="more" href="javascript:;">
        <%:"Advanced".Localize()%></a>
    <div class="more-search common-form" style="display: none">
        <h3>
            <%= "Advanced search".Localize() %></h3>
        <form action="<%=Url.Action(action,ViewContext.RequestContext.AllRouteValues().Merge("page","1")) %>"
        class="no-ajax" method="get">
        <input type="hidden" name="SiteName" value="<%:Request["SiteName"] %>" />
        <input type="hidden" name="RepositoryName" value="<%:Request["RepositoryName"] %>" />
        <input type="hidden" name="FolderName" value="<%:Request["FolderName"] %>" />
        <input type="hidden" name="Folder" value="<%:Request["Folder"] %>" />
        <input type="hidden" name="ParentFolder" value="<%:Request["ParentFolder"] %>" />
        <input type="hidden" name="ParentUUID" value="<%:Request["ParentUUID"] %>" />
        <fieldset>
            <table>
                <tbody>
                  <%--  <tr>
                        <th>
                            <label>
                                <%:"Key words".Localize() %></label>
                        </th>
                        <td>
                            <input type="text" name="search" value="<%=ViewContext.RequestContext.GetRequestValue("search") %>" />
                        </td>
                    </tr>--%>
                    <tr>
                        <th>
                            <label>
                                <%:"Filters".Localize() %></label>
                        </th>
                        <td>
                            <div>
                                <div class="filter-list clearfix" id="container-<%:guid %>" style="width: 400px;">
                                    <div id="template-<%:guid %>" class="filter-form clearfix" style="margin-bottom: 5px;">
                                        <a class="o-icon remove del right" href="javascipr:;"></a>
                                        <div class="clearfix" style="margin-bottom: 5px;">
                                            <%:Html.DropDownList("FieldName",
                                            schema.AllColumns.Select(o =>
                                                new System.Web.Mvc.SelectListItem
                                                {
                                                    Text = o.Name,
                                                    Value = o.Name
                                                }), new { @class="medium"})%>
                                            <%:Html.DropDownList("Operator", operatorList,
                                            new { @class="short" })%>
                                        </div>
                                        <div class="clearfix" style="margin-bottom: 5px;">
                                            <%:Html.TextBox("Value1", null, new {@class="medium" })%>
                                            <%:Html.DropDownList("Logical", logicalList,
                                            new { @class="short" })%>
                                        </div>
                                    </div>
                                </div>
                                <a href="javascript:;" class="o-icon add" id="add-<%:guid %>"></a>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%: "Search".Localize() %></button>
            <button type="button" class="cancel">
                <%: "Cancel".Localize() %></button>
        </p>
        </form>
        <script language="javascript" type="text/javascript">
            $(function () {
                var searchPanel = $('div.search-panel'),
        panel = searchPanel.find('div.more-search'),
        moreLink = searchPanel.find('a.more').click(function () {
                    if (panel.is(':visible')) {
                        $(this).removeClass('active');
                        panel.slideUp();
                    } else {
                        $(this).addClass('active');
                        panel.slideDown();
                    }
                });
                searchPanel.find('.cancel').click(function () {
                    $(this).removeClass('active');
                    panel.slideUp();
                });
                kooboo.cms.ui.dynamicListInstance({
                    containerId: 'container-<%:guid %>',
                    templateId: 'template-<%:guid %>',
                    addButtonId: 'add-<%:guid %>',
                    propertyName: 'WhereClause',
                    data: eval('<%=( ViewData["WhereClause"] ?? new List<WhereClause>{ new WhereClause() }).ToJSON() %>')
                });
            });
        </script>
    </div>
    <%} %>
</div>
