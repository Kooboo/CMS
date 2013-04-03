<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<IEnumerable<Kooboo.CMS.Web.Areas.Contents.Models.FormModel>>" %>

<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%:"Templates".Localize() %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3 class="title">
        <%:"Templates".Localize() %></h3>
    <%:Html.Partial("Nav") %>
    <ul class="tabs clearfix" id="templateTabs" style="margin-top: 20px">
        <%var formTypes = Enum.GetValues(typeof(FormType));

          Dictionary<FormType, string> dic = new Dictionary<FormType, string>();
          dic.Add(FormType.Create, "The page to add a content");
          dic.Add(FormType.Grid, "The CMS content list page");
          dic.Add(FormType.Update, "Content editing page");
          dic.Add(FormType.Selectable, "Category selection page");
          dic.Add(FormType.List, "The front site content list display");
          dic.Add(FormType.Detail, "The front site content detail display");

          foreach (var t in formTypes)
          {
		
        %>
        <li class="tooltip-tab"><a href="#form_<%=t %>" title="<%=t %>">
            <%=t.ToString().Localize() %></a><span class="tooltip-link" title="<%=dic[((FormType)t)].Localize() %>"></span>
        </li>
        <% } %>
    </ul>
    <div class="tab-content">
        <div class="common-form">
            <% using (Html.BeginForm())
               {                 
            %>
            <%:Html.Hidden("CurrentFormType", Request["CurrentFormType"] ?? "Grid")%>
            <%:Html.Hidden("TemplateBuildByMachine", ((Kooboo.CMS.Content.Models.Schema)ViewData["schema"]).TemplateBuildByMachine)%>
            <%  var index = 0; foreach (var form in Model)
                {%>
            <fieldset id="form_<%=form.FormType %>">
                <%:Html.TextArea(string.Format("formList[{0}].Body", index), form.Body, new { style = "width: 100%;min-width: 300px; max-width: 700px", rows = 18, cols = 20 ,formType = form.FormType.ToString()})%>
                <input type="hidden" value="<%=form.FormType %>" name="<%=string.Format("formList[{0}].FormType", index)%>" />
            </fieldset>
            <% index++;
                } %>
            <p class="buttons">
                <%: Html.ActionLink("« Previous".Localize(), "Edit", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button", title = "Create Column".Localize(), target = "_parent" }))%>
                <%: Html.ActionLink("Reset Current".Localize(), "ResetCurrent", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button resetCurrent", title = "Reset".Localize() }))%>
                <%: Html.ActionLink("Reset All".Localize(), "ResetAll", ViewContext.RequestContext.AllRouteValues(), new RouteValueDictionary(new { @class = "button resetAll", title = "ResetAll".Localize(), tip = "Some Templates have been manually Edited !<p>Are you sure you want to continue to reset all template?</p>".Localize() }))%>
                <button type="submit" class="Finish" id="finishBtn">
                    <%:"Finish".Localize() %></button>
            </p>
            <% }%>
        </div>
    </div>
    <script language="javascript" type="text/javascript">



        var onSuccess = function () {
        }

        $(function () {
            function successHandler() {
                this.resetCurrent = function (response) {
                    var textArea = $("textarea:visible");
                    textArea.val(response.Model.Body);
                }
                this.resetAll = function (response) {
                    $('textarea').each(function () {
                        var handle = $(this);
                        handle.data('template-changed', false);
                        var val = response.Model.where(function (o) { return o.FormType == handle.attr('formType'); }).first();
                        handle.val(val.Body);
                    });
                }
                this.finish = function (response) {
                    window.location.href = response.RedirectUrl;
                }
            }

            var finishUrl = $('form').attr('action');

            $('#finishBtn').click(function () {
                onSuccess = (new successHandler()).finish;
                $('form').attr('action', finishUrl);
            });


            $(window).bind('beforeunload', function () {
                if (!kooboo.cms.ui.status().canLeave()) {
                    return '<%:"Are you sure you want to leave this page".Localize() %>';
                }
            });

            $("input,select,textarea").keyup(function () {
                kooboo.cms.ui.status().stop();
            });

            var buildByMachine = $('#TemplateBuildByMachine').val().trim().toLowerCase() == "true";

            $('textarea').change(function () {
                $(this).data('template-changed', true);
                $('#TemplateBuildByMachine').val(false);
                kooboo.data('template-changed', true);
            });
            var templateTabs = $("#templateTabs");

            var currentFormType = $("#CurrentFormType");

            $("#templateTabs li a").click(function () {

                var current = $(this);

                current.parent().siblings().removeClass("current");

                current.parent().addClass("current");

                var currentTabId = current.attr("href");

                $(currentTabId).show().siblings("fieldset").hide();

                currentFormType.val(current.attr("title"));

                return false;

            }).each(function () {

                var current = $(this);

                var currentTabId = current.attr("href");

                if (currentTabId != "#form_" + currentFormType.val()) {
                    $(currentTabId).hide();
                } else {
                    current.parent().addClass("current");
                }

            });

            $("form").submit(function () {
                kooboo.cms.ui.status().pass();
            });


            $("a.resetAll").click(function () {
                var current = $(this);
                if (kooboo.data('template-changed')) {
                    kooboo.confirm('<%:"Some templates has been manually edited! Do you want to continue retset all templates?".Localize() %>', function (result) {
                        if (result) {
                            kooboo.data('template-changed', false);
                            onSuccess = (new successHandler()).resetAll;
                            $("form").attr("action", current.attr("href")).submit();
                        }
                    });
                }
                else {
                    onSuccess = (new successHandler()).resetAll;
                    $("form").attr("action", current.attr("href")).submit();
                }
                return false;
            });

            $("a.resetCurrent").click(function () {
                var current = $(this);

                var textArea = $("textarea:visible");

                if (textArea.data('template-changed')) {
                    kooboo.confirm(
                    '<%:"Are you sure you want to reset this template".Localize() %>',
                    function (r) {
                        if (r) {
                            onSuccess = (new successHandler()).resetCurrent;
                            $("form").attr("action", current.attr("href")).submit();
                            textArea.data('template-changed', false)
                        }
                    });
                } else {
                    onSuccess = (new successHandler()).resetCurrent;
                    $("form").attr("action", current.attr("href")).submit();
                }
                return false;
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SidebarHolder" runat="server">
</asp:Content>
