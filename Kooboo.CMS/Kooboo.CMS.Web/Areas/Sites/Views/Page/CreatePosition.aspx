<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.PagePosition>" %>
    <%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Create page position for "<%:ViewContext.RequestContext.GetRequestValue("fullName") %>"</h2>
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
           <%:Html.Hidden("sucess",Request.HttpMethod.ToLower() == "post" && ViewData.ModelState.IsValid) %>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <legend></legend>
            <table id="positionFormTable">
                <tbody>
                    <%:Html.EditorFor(m=>m.PagePositionId) %>
                    <%:Html.EditorFor(m=>m.LayoutPositionId) %>
                    <%--<%:Html.EditorFor(m=>m.ViewName) %>--%>
                    <%:Html.EditorFor(m=>m.Order) %>
                    <%=Html.Partial("PagePositionInstance",Model) %>
                </tbody>
            </table>
        </fieldset>
        <button type="submit">Save</button>
        
        <% } %>
    </div>
        <script language="javascript" type="text/javascript">
            adminJs.popForm.initFormSubmit();
    </script>
</asp:Content>
