<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.PagePublishViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <fieldset>
            <table>
                <tbody>
                    <% var page = new Kooboo.CMS.Sites.Models.Page(Kooboo.CMS.Sites.Models.Site.Current, Model.FullName);
                       if (ServiceFactory.PageManager.HasDraft(page))
                       {  %>
                    <%:Html.EditorFor(m => m.PublishDraft, new { @class = "medium draft" })%>
                    <%} %>
                    <tr>
                        <th>
                            <label>
                                <%:"Publish schedule".Localize() %>
                            </label>
                        </th>
                        <td>
                            <p class="clearfix">
                                <input type="radio" value="False" name="PublishSchedule" id="publish-schedule-now"
                                    <%:!Model.PublishSchedule?"checked":"" %> />
                                <label for="publish-schedule-now">
                                    <%:"Publish now".Localize() %>
                                </label>
                            </p>
                            <p class="clearfix">
                                <input type="radio" value="True" name="PublishSchedule" id="publish-schedule-latter"
                                    <%:Model.PublishSchedule?"checked":"" %> />
                                <label for="publish-schedule-latter">
                                    <%:"Publish later".Localize() %></label>
                            </p>
                        </td>
                    </tr>
                    <%:Html.EditorFor(m => m.PublishDate, new { @class = "medium" })%>
                    <%:Html.EditorFor(m => m.PublishTime, new { @class = "medium" })%>
                    <%:Html.EditorFor(m=>m.Period,new { @class = "medium" })%>
                    <%:Html.EditorFor(m=>m.OfflineDate,new { @class = "medium" })%>
                    <%:Html.EditorFor(m=>m.OfflineTime,new { @class = "medium" })%>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        <% } %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {
            var publishDraft = '<%:Request["FromDraft"] %>' == 'True';
            $('#PublishDraft').attr('checked',publishDraft);
            var publishSchedue = $(':radio[name=PublishSchedule]')
            .each(function () {
                initSchedue($(this));
            });
            publishSchedue.change(function () {
                var handle = $(this);
                initSchedue(handle);
            });

            function initSchedue(handle) {
                var nextAll = handle.parents('tr:eq(0)').nextAll();
                if (handle.attr('checked') && handle.val().toString().toLower() == 'true') {
                    nextAll.show('fade');
                    initPeriod(period);
                } else {
                    nextAll.hide();
                }
            }

            var period = $('#Period').click(function () {
                var handle = $(this);
                initPeriod(handle);
            });

            initPeriod(period);

            function initPeriod(handle) {
                var nextAll = handle.parents('tr:eq(0)').nextAll();
                if (handle.attr('checked')) {
                    nextAll.show('fade');
                } else {
                    nextAll.hide();
                }
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
