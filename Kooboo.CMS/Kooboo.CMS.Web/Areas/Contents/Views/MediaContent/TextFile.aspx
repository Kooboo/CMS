<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="common-form">
        <% using (Html.BeginForm())
           { %>
        <fieldset>
            <table>
                <tbody>
                    <tr>
                        <td>
                            <%:Html.TextArea("Body",(string)(Model.Body),new{ @class="codemirror"}) %>
                        </td>
                    </tr>
                </tbody>
            </table>
        </fieldset>
        <p class="buttons">
            <button type="submit">
                <%:"Save".Localize() %></button>
            <button class="dialog-close">
                <%:"Cancel".Localize()%></button>
        </p>
        <%} %>
    </div>
    <script language="javascript" type="text/javascript">
        $(function () {


            


        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
