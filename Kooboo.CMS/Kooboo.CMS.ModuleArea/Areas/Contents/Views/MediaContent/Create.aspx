<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= "Create".Localize()%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <form method="post" enctype="multipart/form-data" action="<%=Url.Action("Create",ViewContext.RequestContext.AllRouteValues()) %>">
        <%:Html.Hidden("success",Request.HttpMethod.ToLower() == "post" && ViewData.ModelState.IsValid) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <table>
                <tbody id="filesContainer">
                    <tr id="filesTemplate">
                        <td>
                            <input type="file" name="files" />
                        </td>
                        <td>
                            <img class="del"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="file" name="files[0]" />
                        </td>
                        <td class="checkbox">
                        </td>
                    </tr>
                </tbody>
                <tfoot>
                    <tr>
                        <td>
                        <a href="javascript:;" class="o-icon add" id="addFile"></a>
                        </td>
                        <td>
                        </td>
                    </tr>
                </tfoot>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
        </p>
        </form>
    </div>
    <script language="javascript" type="text/javascript">
    	kooboo.cms.ui.dynamicListInstance({
            containerId: 'filesContainer',
            templateId: 'filesTemplate',
            addButtonId: 'addFile',
            propertyName:'files',
            data:[]
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
