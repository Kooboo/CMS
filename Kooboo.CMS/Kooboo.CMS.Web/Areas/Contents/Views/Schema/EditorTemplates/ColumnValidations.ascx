<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<Kooboo.CMS.Form.ColumnValidation>>" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%
	var guid = Guid.NewGuid();
	var fullPropertyName = ViewData.TemplateInfo.HtmlFieldPrefix;
	var ulId = fullPropertyName.Replace(".", "_");

	ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
	var preProperty = ViewData["id"] + "Validations";
%>
<fieldset class="flex-list">
	<legend>
		<%:"Validation rules".Localize() %></legend>
	<table id="columnValidationGroup<%:guid %>">
		<tbody id="columnValidationTemplate<%:guid %>">
			<tr>
				<th class="ValidationType">
					<label>
						<%:"Type".Localize() %>
					</label>
				</th>
				<td>
					<select name="ValidationType" class="validationTypeSelect medium">
						<%
							foreach (var a in Enum.GetNames(typeof(Kooboo.CMS.Form.ValidationType)))
							{ %>
						<option value="<%:(int)Enum.Parse(typeof(Kooboo.CMS.Form.ValidationType),a) %>">
							<%:a %></option>
						<%} 
						%>
					</select>
					<div class="Range Parameters">
						<label>
							<%:"Start".Localize() %>
						</label>
						<input type="text" name="Start" title="Start" class="mini" />
						<label>
							<%:"End".Localize() %></label><input type="text" name="End" title="End" class="mini" />
					</div>
					<div class="Regex Parameters">
						<label>
							<%:"Pattern".Localize() %></label>
						<input type="text" name="Pattern" title="Pattern" class="short" />
					</div>
					<div class="StringLength Parameters">
						<label>
							<%:"Min".Localize() %></label><input type="text" name="Min" title="Min" class="mini" />
						<label>
							<%:"Max".Localize() %>
						</label>
						<input type="text" name="Max" title="Max" class="mini" />
					</div>
				</td>
			</tr>
			<tr>
				<th class="ErrorMessage">
					<label>
						<%:"Error message".Localize() %></label>
				</th>
				<td>
					<input type="text" name="ErrorMessage" />
					<a href="javascript:;" class="o-icon remove form-action">Remove</a>
				</td>
			</tr>
		</tbody>
	</table>
	<a href="javascript:;" class="o-icon add form-action" id="addValidationBtn<%:guid %>">Add</a>
</fieldset>
<script type="text/javascript">
    $(document).ready(function () {
        kooboo.cms.ui.dynamicListInstance({
            containerId: 'columnValidationGroup<%:guid %>', //the container that you put the new item in. 
            templateId:'columnValidationTemplate<%:guid %>',
            propertyName: '<%:ViewData["id"]%>', //str 
            addButtonId: 'addValidationBtn<%:guid %>', //add button
            data: <%=Model.ToJSON() %>, //json data to fill the form field.
			delClass: 'remove',
			parentSelector:'tbody',
			onInit:function () {

		///<summary>

		///init which validatetype to show

		///</summary>


		var selects = $("select.validationTypeSelect");


		initShow();

		//change events

		selects.change(function () {

			showSelect($(this));

		});


		function showSelect(current) {

			var validateType = current.find("option:selected").html();


			current.siblings('.Parameters').hide().find('input').attr('disabled', true);

            if(validateType){
                current.siblings('.' + validateType.replace(/(^\s*)|(\s*$)/g, "")).show().find('input').attr('disabled', false);
            }
		}

		///init which validateType to show

		function initShow() {

			selects.each(function () {

				showSelect($(this));

			});

		}

	},
    onAdd:function () {
                                kooboo.cms.ui.status().stop();
    },
    onRemove:function(){
        kooboo.cms.ui.status().stop();
    }
        });
        //kooboo.cms.ui.flexList();
    });
</script>
