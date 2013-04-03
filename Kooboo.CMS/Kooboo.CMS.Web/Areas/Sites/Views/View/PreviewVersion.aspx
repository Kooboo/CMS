<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.View>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<Kooboo.CMS.Sites.Models.View>(Model);
    %>
    <%ViewContext.RequestContext.AllRouteValues()["prevController"] = ViewContext.RequestContext.AllRouteValues()["controller"]; %>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <%:Html.ValidationSummary(true) %>
        <%: Html.HiddenFor(m=>m.EngineName) %>
        <%:Html.Hidden("SiteName",ViewContext.RequestContext.GetRequestValue("SiteName")) %>
        <%: Html.Hidden("old_Key", Model.Name)%>
        <fieldset>
            <table>
                <tbody>
                    <%:Html.DisplayFor(m=>m.Name) %>
                </tbody>
            </table>
            <table>
                <tbody>
                    <%:Html.Partial("TemplateEditor",model) %>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <a href="<%=Url.Action("Revert",ViewContext.RequestContext.AllRouteValues()) %>"
                class="button ajax-post-link" confirm="<%:"Are you sure you want to rever to this version?".Localize()%>">
                <%:"Revert".Localize() %>
            </a><a href="javascript:;" class="button dialog-close">
                <%:"Close".Localize() %>
            </a>
        </p>
        <% } %>
    </div>
    <%:Html.Partial("View.Script") %>
    <%if (!string.IsNullOrEmpty(Kooboo.CMS.Sites.Models.Site.Current.Repository))
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
    <%} %>
    <script language="javascript" type="text/javascript">
        //fiexed --hide or remove some elements
        $('.dialog-close').click(function () {
            kooboo.cms.ui.status().pass();
        });

        $(function () {
            $('div.task-block.codehelper,div.task-block.views').remove();
            $('div.task-block.parameters,div.task-block.datarules,div.task-block.plugins').find('p.buttons').remove();

            $('a.o-icon.remove,a.o-icon.csharp,a.o-icon.design,a.o-icon.add').remove();

            $(':submit').remove();


            $('input,select').attr('disabled', true);

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
