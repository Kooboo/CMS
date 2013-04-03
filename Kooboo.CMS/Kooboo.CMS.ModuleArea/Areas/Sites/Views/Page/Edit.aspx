<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Page>" %>

<asp:Content ID="TitleContent" ContentPlaceHolderID="TitleContent" runat="server">
    <%: "Edit page".Localize()%>"<%: Model.FriendlyName %>"
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Sites.Models.Page>(Model);
    %>
    <h3 class="title">
        <%:"Edit page".Localize()%>
        "<%: Model.FriendlyName%>" <a href="<%:string.IsNullOrEmpty(Request["ReturnUrl"])?Url.Action("Index",ViewContext.RequestContext.AllRouteValues()):Request["ReturnUrl"] %>">
            <%:"Back to list".Localize() %>
        </a>
    </h3>
    <div class="common-form" id="page-form">
        <% using (Html.BeginForm("Edit", "Page", ViewContext.RequestContext.AllRouteValues(), FormMethod.Post, new RouteValueDictionary(new { id = "page-form" })))
           { %>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <%:Html.Hidden("ParentPage", Model.Parent==null? "":Model.Parent.FullName)%>
        <%:Html.Hidden("old_Key", Model.FullName)%>
        <input type="hidden" value="<%:Model.Name %>" name="Name" />
        <%:Html.HiddenFor(m=>m.Published)%>
        <%:Html.ValidationSummary(true)%>
        <div id="page-tab">
            <ul class="tabs clearfix">
                <li class="current"><a href="#basicInfo">
                    <%:"Designer".Localize() %></a></li>
                <li><a href="#setting">
                    <%:"Settings".Localize()%></a></li>
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
                            <%:Html.DisplayFor(m => m.Name, new { @class = "medium" })%>
                        </tbody>
                    </table>
                </fieldset>
                <%:Html.Partial("PageDesign",false)%>
            </div>
            <div class="tab-content " id="setting">
                <fieldset>
                    <table>
                        <%:Html.EditorFor(m => m.Layout, new { @class = "medium" })%>
                        <%: Html.EditorFor(m=>m.ContentTitle,new { @class = "medium" }) %>
                        <%if (model.Parent == null)
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
            <% var publishPermission = ServiceFactory.UserManager.Authorize(Kooboo.CMS.Sites.Models.Site.Current, ViewContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission); %>
            <%if (Model.Published.Value == false || (Model.Published.Value == true && publishPermission))
              {  %>
            <button type="submit" disabled="disabled" name="pageRedirect" value="false">
                <%="Save".Localize()%></button>
            <%} %>
            <button type="submit" disabled="disabled" name="SaveAsDraft" value="true">
                <%="Save as draft".Localize()%></button>
            <%if (publishPermission)
              {  %>
            <button type="submit" disabled="disabled" name="SetPublished" value="true">
                <%="Save & publish".Localize()%></button>
            <button id="save-and-preview" type="submit" title="<%:"Save & preview".Localize() %>"
                value="false" name="pageRedirect" disabled="disabled">
                <%="Save & preview".Localize()%></button>
            <%} %>
            <%var returnUrl = this.Request.RequestContext.GetRequestValue("ReturnUrl");
              if (string.IsNullOrEmpty(returnUrl))
              {
                  returnUrl = this.Url.Action("Index", ViewContext.RequestContext.AllRouteValues());
              } %>
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
        <%:Html.Partial("DataRuleGridForms", model.DataRules == null ? new List<Kooboo.CMS.Sites.Models.DataRuleSetting>() : model.DataRules)%>
    </div>
    <%} %>
    <script type="text/javascript">

        $(function () {

            var async = true, success = false;
            $('p.buttons button').click(function (e) {
                async = !$(this).is('#save-and-preview');
            });

            kooboo.cms.ui.event.ajaxSubmit(function (option) {
                option.async = async;
            });
            var previewUrl = "";
            kooboo.cms.ui.event.onSuccess(function (response) {
                success = response.Success;
                previewUrl = response.Model;
            });
            kooboo.cms.ui.event.afterSubmit(function () {
                if (!async && success && previewUrl) {
                    window.open(previewUrl);
                    success = false;
                }
            });


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
