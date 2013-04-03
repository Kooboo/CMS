<%@  Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.Web.Mvc.Paging.PagedList<Kooboo.CMS.Content.Models.TextContent>>" %>
<%: Html.Partial(ViewData["Template"].ToString(), Model)%>
