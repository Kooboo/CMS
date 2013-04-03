<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<bool?>" %>
<% 
    string prefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
    ViewData.TemplateInfo.HtmlFieldPrefix = prefix;

%>
<script language="javascript" type="text/javascript">
    $(function () {
        var next = $("input:radio[name=Regex]").parent().parent().next();

        var lebel1 = next.find("th label");
        var lebel2 = next.next().find("th label");

        htm1 = lebel1.html();
        htm2 = lebel2.html();

        initLabel($("input:radio[name=Regex]"));
        function initLabel(obj) {
            if (obj.val().toString().toLocaleLowerCase() == "true") {
                lebel1.html('<%="Input Pattern" %>');
                lebel2.html('<%="Output Pattern" %>');

                $('a.tooltip-link').show();
            } else {
                lebel1.html(htm1);
                lebel2.html(htm2);



                $('a.tooltip-link').hide();

            }
        }

        $("input:radio[name=Regex]").click(function () {
            initLabel($(this));
        });

        initLabel($('input:radio[name=Regex]:checked'));
    });
</script>
<tr>
    <th>
        <label class="label-checkbox" for="<%:ViewData.TemplateInfo.GetFullHtmlFieldId(ViewData.ModelMetadata.PropertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description %>">
        </a>
        <%} %>
    </th>
    <td>
        <p class="left">
            <input type="radio" name="<%=ViewData.ModelMetadata.PropertyName %>" id="<%=ViewData.ModelMetadata.PropertyName.Replace(".","_") %>_false"
                <%=(!Model.HasValue||Model.Value.ToString().ToLower()=="false"?"checked":"") %>
                value="false" />
            <label for="<%=ViewData.ModelMetadata.PropertyName.Replace(".","_") %>_false" class="radio-label">
                <%="Normal".Localize() %></label></p>
        <p>
            <input type="radio" name="<%=ViewData.ModelMetadata.PropertyName %>" id="<%=ViewData.ModelMetadata.PropertyName.Replace(".","_") %>_true"
                value="true" <%=(Model.HasValue&&Model.Value.ToString().ToLower()=="true"?"checked":"") %> />
            <label for="<%=ViewData.ModelMetadata.PropertyName.Replace(".","_") %>_true" class="radio-label"
                style="width: 122px;">
                <%="Regular Expression".Localize() %></label></p>
    </td>
</tr>
