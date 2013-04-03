<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%var action = ViewContext.RequestContext.AllRouteValues()["action"].ToString(); %>
<div class="search-panel">
    <div class="common-search">
        <form id="search" action="<%=Url.Action(action,ViewContext.RequestContext.AllRouteValues().Merge("page","1")) %>"
        method="get" class="no-ajax">
        <%foreach (var key in Request.QueryString.AllKeys)
          {
              if (key.Equals("search", StringComparison.OrdinalIgnoreCase) || key.Equals("page", StringComparison.OrdinalIgnoreCase)) continue;
        %>
        <%:Html.Hidden(key, Request.QueryString[ key ])%>
        <% } %>
        <input type="text" name="search" id="searchBox" value="<%=ViewContext.RequestContext.GetRequestValue("search") %>" /><%--<img id="removeSearch" alt="remove search!" src="/Areas/Contents/Styles/icons/cross.png" />--%>
        <button type="submit">
            <%="Search".Localize() %></button>
        </form>
    </div>
</div>
<%--<script type="text/javascript">
    $(function ($) {
        var searchbox = $("#searchBox").
        focus();

        var removeSearch = $("#removeSearch").css({
            position: "absolute",
            left: "120px",
            top: "1.5px",
            cursor: "pointer"
        }).click(function () {
            searchbox.val('');
            $("#search").submit();
        });

        if (!searchbox.val()) {
            removeSearch.hide();
        }

    });
</script>--%>
