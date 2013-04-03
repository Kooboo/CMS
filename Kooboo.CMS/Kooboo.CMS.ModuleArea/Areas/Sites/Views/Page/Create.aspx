<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Page>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Create page".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Sites.Models.Page>(Model);
    %>
    <h3 class="title">
        <%:"Create page".Localize()%><%:Html.ActionLink("Back to list".Localize(),"Index",ViewContext.RequestContext.AllRouteValues()) %></h3>
    <div class="common-form">
        <% using (Html.BeginForm("Create", "Page", ViewContext.RequestContext.AllRouteValues(), FormMethod.Post, new RouteValueDictionary(new { id = "page-form" })))
           { %>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <%:Html.Hidden("ParentPage", ViewContext.RequestContext.GetRequestValue("ParentPage"))%>
        <%:Html.ValidationSummary(true)%>
        <div id="page-tab">
            <ul class="tabs clearfix">
                <li class="current"><a href="#basicInfo">
                    <%:"Designer".Localize() %></a></li>
                <li><a href="#setting">
                    <%:"Settings".Localize() %></a></li>
                <li><a href="#navigation">
                    <%:"Navigation".Localize() %></a></li>
                <li><a href="#htmlmeta">
                    <%:"HTML meta".Localize() %></a></li>
                <li class="hide"><a href="#permission">
                    <%:"Permission".Localize() %></a></li>
                <li><a href="#route">
                    <%:"URL route".Localize() %></a></li>
                <%if (!string.IsNullOrEmpty(Kooboo.CMS.Sites.Models.Site.Current.Repository))
                  {%><li><a href="#datarules">
                      <%:"DataRules".Localize() %></a></li><%} %>
                <li><a href="#plugins">
                    <%:"Plug-ins".Localize() %></a></li>
                <% foreach (var tab in Kooboo.CMS.Web.Areas.Sites.PageCustomTabs.Tabs(model.Layout))
                   {%>
                <li><a href="#<%= tab.Title %>">
                    <%: tab.Title.Localize()%></a></li>
                <%} %>
                <%--<li><a href="#CustomFields">
                    <%:"CustomFields".Localize() %></a></li>--%>
            </ul>
            <div class="tab-content" id="basicInfo">
                <fieldset>
                    <table>
                        <tbody>
                            <%:Html.EditorFor(m => m.Name, new { @class = "medium" })%>
                            <%--  <%:Html.EditorFor(m => m.Plugins, new { @class = "medium" })%>--%>
                        </tbody>
                    </table>
                </fieldset>
                <%:Html.Partial("PageDesign",true)%>
            </div>
            <div class="tab-content " id="setting">
                <fieldset>
                    <table>
                        <%:Html.EditorFor(m => m.Layout, new { @class = "medium" })%>
                        <%: Html.EditorFor(m=>m.ContentTitle,new { @class = "medium" }) %>
                        <%if (Request["fullName"] == null)
                          {%>
                        <%:Html.EditorFor(m => m.IsDefault)%>
                        <% } %>
                        <%:Html.EditorFor(m => m.EnableTheming)%>
                        <%:Html.EditorFor(m => m.EnableScript)%>
                        <%:Html.EditorFor(m => m.PageType)%>
                        <%:Html.EditorFor(m=>m.OutputCache) %>
                        <%:Html.EditorFor(m=>m.Searchable) %>
                    </table>
                </fieldset>
            </div>
            <div class="tab-content " id="navigation">
                <%:Html.EditorFor(m => m.Navigation)%>
            </div>
            <div class="tab-content " id="htmlmeta">
                <%:Html.EditorFor(m => m.HtmlMeta)%>
            </div>
            <div class="tab-content " id="permission">
                <%:Html.EditorFor(m => m.Permission)%>
            </div>
            <div class="tab-content " id="route">
                <%:Html.EditorFor(m => m.Route)%>
            </div>
            <div class="tab-content " id="datarules">
                <%if (!string.IsNullOrEmpty(Kooboo.CMS.Sites.Models.Site.Current.Repository))
                  {%><%:Html.EditorFor(m => m.DataRules)%><%} %>
            </div>
            <div class="tab-content " id="plugins">
                <%:Html.EditorFor(o=>o.Plugins) %>
            </div>
           <% foreach (var tab in Kooboo.CMS.Web.Areas.Sites.PageCustomTabs.Tabs(model.Layout))
               {%>
            <div class="tab-content " id="<%= tab.Title %>">
                <%:Html.Partial(tab.VirualPath,Model.CustomFields,new ViewDataDictionary()) %>
            </div>
            <%} %>
            <%--<div class="tab-content " id="CustomFields">
                <%:Html.EditorFor(o=>o.CustomFields) %>
            </div>--%>
        </div>
        <p class="buttons">
            <button type="submit" name="Published" value="false">
                <%="Save & return".Localize()%></button>
            <%if (ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current, ViewContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission))
              {%>
            <button type="submit" name="Published" value="true">
                <%="Save & publish".Localize()%></button>
            <%} %>
        </p>
        <% } %>
    </div>
    <%:Html.Partial("DataRule.Script") %>
    <%if (!string.IsNullOrWhiteSpace(Kooboo.CMS.Sites.Models.Site.Current.Repository))
      {%>
    <div id="DataRuleStep1Div" style="display: none;">
        <%:Html.Partial("DataRuleStep1", ViewData["FolderTree"])%>
    </div>
    <div id="DataRuleStep2Div" style="display: none">
        <%:Html.Partial("DataRuleStep2",new Kooboo.CMS.Sites.Models.DataRuleSetting()) %>
    </div>
    <div id="grid-form-container">
        <%:Html.Partial("DataRuleGridForms",model.DataRules==null? new List<Kooboo.CMS.Sites.Models.DataRuleSetting>():model.DataRules) %>
    </div>
    <% }%>
    <script type="text/javascript">
        $(function () {
            $("#AddDataRuleBtn").pop({
                useContent: true,
                contentId: 'DataRuleStep1Div'
            });

            kooboo.cms.sites.datarule.step2.initDataRuleGrid();

            kooboo.cms.sites.datarule.initFormEvent('#page-form');

            $('#page-tab').koobooTab();

            var enableScript = $('#EnableScript').change(function () {
                var handler = $(this);
                if (!handler.attr('checked')) {
                    enableJquery.attr('checked', false).attr('disabled', true);
                } else {
                    enableJquery.removeAttr('disabled');
                }
            });

            var enableJquery = $('#EnableJquery').change(function () {
                var hanlder = $(this);
                if (hanlder.attr('checked')) {
                    enableScript.attr('checked', true);
                }
            });


            if (!enableScript.attr('checked')) {
                enableJquery.attr('checked', false).attr('disabled', true);
            } else {
                enableJquery.removeAttr('disabled');
            }

        });
        

    </script>
</asp:Content>
