<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Site.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Sites.Models.CustomFile>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
        UploadFile</h2>
    <div class="common-form">
        <%using (Html.BeginForm("Create", "CustomFile", new { siteName = ViewContext.RequestContext.GetRequestValue("siteName"), fullName = Request["fullName"] }, FormMethod.Post, new { enctype = "multipart/form-data" }))
          {%>
        <%:Html.ValidationSummary(true) %>
        <fieldset>
            <legend></legend>
            <table>
                <tbody>
                    <tr>
                        <th>
                            <label for="file">
                                <%:"Select your file".Localize() %>
                            </label>
                        </th>
                        <td>
                            <input type="file" name="image" id="file" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
        <button type="submit">
            <%:"Save".Localize() %></button>
        </p>
        
        <% }%></div>
</asp:Content>
