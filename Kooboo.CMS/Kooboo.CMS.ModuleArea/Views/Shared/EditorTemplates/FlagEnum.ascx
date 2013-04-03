<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var propertyName = ViewData["name"] == null ? ViewData.ModelMetadata.PropertyName : ViewData["name"].ToString();
   var defaultValue = ViewData.ModelMetadata.AdditionalValues.ContainsKey("DefaultValue") ? ViewData.ModelMetadata.AdditionalValues["DefaultValue"] : 0;


   var eType = ViewData.ModelMetadata.ModelType;
   var values = Enum.GetValues(eType);
   var guid = Guid.NewGuid();
%>
<tr id="tr-<%:guid %>">
    <th>
        <label for="<%: ViewData.TemplateInfo.GetFullHtmlFieldId(propertyName)%>">
            <%: ViewData.ModelMetadata.GetDisplayName().Localize()%></label>
        <% if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
           {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
    </th>
    <td>
        <%for (var i = 0; i < values.Length; i++)
          {
              var gid = Guid.NewGuid(); %>
        <p class="clearfix">
            <input type="checkbox" value="<%: (int)values.GetValue(i)%>" id='value-<%:gid %>' />
            <label for="value-<%:gid %>">
                <%:  values.GetValue(i) %>
            </label>
        </p>
        <% } %>
        <input id="hidden-<%:guid %>" type="hidden" name="<%:propertyName %>" value="<%:ViewData.ModelMetadata.Model!=null? ((int)(ViewData.ModelMetadata.Model)): 0%>" />
    </td>
</tr>
<script language="javascript" type="text/javascript">
    $(function () {
        var hidden = $('#hidden-<%:guid %>'),
        modelValue = parseInt(hidden.val()),
         tr = $('#tr-<%:guid %>'),
         checkboxs = tr.find('input:checkbox');
        checkboxs.each(function () {
            var checkbox = $(this), val = parseInt(checkbox.val());
            if ((val & modelValue) == val) {
                checkbox.attr('checked', 'checked');
            }
        }).change(function () {
            var selected = checkboxs.filter(':checked'), val = 0;
            selected.each(function () {
                var checkbox = $(this);
                val += parseInt(checkbox.val());
            });
            hidden.val(val);
        });
    });
</script>
