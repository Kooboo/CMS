<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
   var htmlAttr = Html.GetUnobtrusiveValidationAttributes(propertyName, ViewData.ModelMetadata).Merge("h", "h");
%>
<%:Html.Hidden("AlertMessage", ViewData["AlertMessage"])%>
<%:Html.Hidden("IsValid", ViewData.ModelState.IsValid) %>
<%:Html.Hidden("IsPostBack", Request.HttpMethod.ToLower() == "post" )%>
<%:Html.Hidden("IsNew",ViewData["IsNew"]==null?false:(bool)ViewData["IsNew"]) %>
<tr class="repository">
    <th>
        <label for="<%: ViewData.ModelMetadata.PropertyName%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <%             
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="javascript:;" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>
        
        <span id="createRepository" class="hide">
            <%: Html.TextBox(propertyName, Model, htmlAttr.Merge("class", ViewData["class"]).Merge("checkurl", Url.Action("CheckRepository", "Site")).Merge("id","newRepository").Merge("disabled","disabled"))%>
        </span>
        
        <%: Html.DropDownList(ViewData.ModelMetadata.PropertyName, ViewData.ModelMetadata.GetDataSource().GetSelectListItems(ViewContext.RequestContext).SetActiveItem(Model), new RouteValueDictionary().Merge("class", ViewData["class"]))%>
        <a href="<%:this.Url.Action("Create",ViewContext.RequestContext.AllRouteValues()) %>"
            class="create-repository">
            <%:"Create new".Localize() %>
        </a><span class="field-validation-error" id="repositoryError"></span>
        <%: Html.ValidationMessage(ViewData.ModelMetadata, null)%>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    var cfg = {
        createTxt: '<%:"Create new".Localize() %>',
        cancelTxt: '<%:"Cancel".Localize() %>',
        repositoryNull: '<%:"The database field is required." %>'
    };

    $(function () {
        var canSubmit = true;

        var errMsg = $("#AlertMessage").val();
        var repositoryError = $("#repositoryError");
        var isNew = $("#IsNew");


        $("tr.repository").find("a")
            .css("font-weight", "bold")
            .css("line-height", "2");
        $("#newRepository").val('');

        $("a.create-repository")
            .toggle(function () {
                isNew.val(true);
                $("#createRepository").removeClass('hide').find("input").attr("disabled", false);
                repositoryError.show();
                $("#Repository").hide().attr("disabled", true);
                $(this).html(cfg.cancelTxt);
                return false;
            },
            function () {
                isNew.val(false);
                canSubmit = true;
                $("#createRepository").addClass('hide').find("input").attr("disabled", true); ;
                $("#Repository").show().attr("disabled", false);
                repositoryError.hide();
                $(this).html(cfg.createTxt);
                return false;
            });

        kooboo.cms.ui.event.ajaxSubmit(function () {
            if (!kooboo.data('new-repository-name')) {
                if ($('input[name=Repository]:enabled').val() == '<%:"YourDatabaseName".Localize() %>') {
                    kooboo.cms.ui.messageBox().show(cfg.repositoryNull, 'error');
                    return false;
                }
            }
            if ($("input[name=Repository]").attr("disabled") == false && $("input[name=Repository]").val().toString().trim() == "") {
                repositoryError.html(cfg.repositoryNull);
                return false;
            }
            return canSubmit;
        });

    });


    $(function () {
        //$('#newRepository').Watermark('<%:"YourDatabaseName".Localize() %>');
    });
</script>
