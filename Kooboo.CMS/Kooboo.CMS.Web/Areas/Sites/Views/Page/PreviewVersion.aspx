<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.Page>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Sites.Models.Page>(Model);
    %>
    <h3 class="title">
        <%: model.FriendlyName%>
    </h3>
    <div class="common-form">
        <% using (Html.BeginForm("Edit", "Page", ViewContext.RequestContext.AllRouteValues(), FormMethod.Post, new RouteValueDictionary(new { id = "page-form" })))
           { %>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <%:Html.Hidden("ParentPage", ViewContext.RequestContext.GetRequestValue("ParentPage"))%>
        <%:Html.Hidden("old_Key", model.FullName)%>
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
                <%--<li><a href="#CustomFields">
                    <%:"CustomFields".Localize() %></a></li>--%>
            </ul>
            <div class="tab-content" id="basicInfo">
                <fieldset>
                    <table>
                        <tbody>
                            <%:Html.DisplayFor(m => m.Name, new { @class = "medium" })%>
                            <%:Html.HiddenFor(m => m.Name, new { @class = "medium" })%>
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
            <%--<div class="tab-content " id="CustomFields">
                <%:Html.EditorFor(o=>o.CustomFields) %>
            </div>--%>
        </div>
        <p class="buttons">
            <a href="<%=Url.Action("Revert",ViewContext.RequestContext.AllRouteValues()) %>"
                class="ajax-post-link button" confirm="<%:"Are you sure you want to rever to this version?".Localize()%>">
                <%:"Revert".Localize() %></a> <a href="javascript:;" class="dialog-close button">
                    <%:"Close".Localize() %></a>
            <%var previewUrl = FrontUrlHelper.Preview(Url, Kooboo.CMS.Sites.Models.Site.Current, (Kooboo.CMS.Sites.Models.Page)model, null); %>
            <%--<a class="button" target="_blank" title="<%:"".Localize() %>" href="<%:previewUrl %>">
                <%="Preview".Localize()%></a>--%>
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

        function onSuccess(responseData) {
            if (responseData.Model && responseData.Model.preview == true) {
                $.pop({
                    url: responseData.RedirectUrl,
                    width: $(window).width() - 20,
                    height: $(window).height() - 50,
                    frameHeight: "100%",
                    popupOnTop: true
                });
                return false;
            }
            return true;
        }


        $(function () {
            $('div.command').remove();

            $('#layout-edit').remove();

            $('a.o-icon.remove,a.o-icon.csharp,a.o-icon.design,a.o-icon.add').remove();

            $(':submit').remove();

            $('input,select').attr('disabled', true);

            window.pageDesignOnLoad(function () {
                $('#iframeMask').show().css('background', 'none');
            });

        });

        
    </script>
</asp:Content>
