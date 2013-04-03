<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Web.Areas.Sites.Models.CreateSiteModel>" %>

<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    CreateRootSite
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        var guid = Guid.NewGuid();
        using (Html.BeginForm())
        { %>
    <%:Html.Hidden("IsNew",false) %>
    <input type="hidden" name="Parent" value=""/>
    <div class="common-form site-form clearfix">
        <div class="left">
            <div class="command clearfix">
                <select class="short" id="template-category">
                    <option value="">
                        <%:"All".Localize() %></option>
                    <%var categoryList = (IEnumerable<string>)ViewData["TemplateCategorys"]; %>
                    <%foreach (var c in categoryList)
                      {%>
                    <option value="<%:c %>">
                        <%:c %></option>
                    <% } %>
                </select>
                <div class="search-panel">
                    <div class="common-search">
                        <input type="text" name="key" id="template-search" class="short" />
                        <button type="button" id="searchBtn">
                            Search</button>
                    </div>
                </div>
            </div>
            <%:Html.ValidationMessageFor(o=>o.Template) %>
            <div class="template clearfix" id="template-container">
                <div class="template-item hide" id="template-item-display">
                    <label for="template-default img-label">
                        <img src="<%=Url.Content("~/Images/template/default.png") %>" alt="Default template" />
                    </label>
                    <p>
                        <input type="radio" name="Template" id="template-default" />
                        <label for="template-default" class="radio-label text-label">
                            Default template</label>
                    </p>
                </div>
            </div>
            <%Html.ValidateFor(o => o.Template);%>
        </div>
        <div class="right">
            <%:Html.ValidationSummary(true) %>
            <fieldset>
                <legend></legend>
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <label>
                                    <span class="left">
                                        <%:"Name".Localize() %></span><a href="javascript:;" class="tooltip-link" title="<%:"The site name".Localize() %>"></a></label>
                                <%:Html.TextBoxFor(o=>o.Name) %>
                                <%:Html.ValidationMessageFor(o => o.Name)%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <span class="left">
                                        <%:"Database name".Localize() %></span> <a href="###" class="tooltip-link" title="<%:"The name of your database to be created".Localize() %>">
                                        </a>
                                </label>
                                <%:Html.TextBoxFor(o => o.Repository)%>
                                <%:Html.ValidationMessageFor(o=>o.Repository) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <span class="left">
                                        <%:"Display name".Localize() %></span><a href="javascript:;" class="tooltip-link"
                                            title="<%:"The site display name".Localize() %>"></a></label>
                                <%:Html.TextBoxFor(o=>o.DisplayName) %>
                                <%:Html.ValidationMessageFor(o=>o.DisplayName) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <span class="left">
                                        <%:"Culture".Localize() %></span><a href="###" class="tooltip-link" title="<%:"Your front site culture, this is used to display currency,  <br/>date, number or other culture related content".Localize() %>"></a>
                                </label>
                                <%:Html.DropDownListFor(o=>o.Culture,(new Kooboo.Web.Mvc.CultureSelectListDataSource()).GetSelectListItems(ViewContext.RequestContext,null)) %>
                                <%:Html.ValidationMessageFor(o=>o.Culture) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="clearfix">
                                    <span class="left">
                                        <%:"Domains".Localize()%></span> <a href="###" class="tooltip-link" title="<%:"Your website domains".Localize() %>">
                                        </a>
                                </label>
                                <div id="container-<%:guid %>">
                                    <p id="template-<%:guid %>" class="clearfix">
                                        <input type="text" style="float:left;margin-right:5px;width:245px;" /><a  class="o-icon remove form-action" title="remove" href="javascript:;"></a>
                                    </p>
                                </div>
                                <a href="javascript:;" id="add-<%:guid %>" class="o-icon add form-action"></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <span class="left">
                                        <%:"SitePath".Localize() %></span><a href="###" class="tooltip-link" title="<%:"The virtual path of your website.  <br/>You may create a subsite eg. www.kooboo.com/china, <b>china</b> is the SitePath.".Localize() %>"></a>
                                </label>
                                <%:Html.TextBoxFor(o=>o.SitePath) %>
                                <%:Html.ValidationMessageFor(o=>o.SitePath) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <span class="left">
                                        <%:"Version".Localize() %></span><a href="###" class="tooltip-link" title="<%:"Give your wesite a version number, it can be any number as you like".Localize() %>"></a>
                                </label>
                                <%:Html.TextBoxFor(o=>o.Version) %>
                                <%:Html.ValidationMessageFor(o=>o.Version) %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="clearfix">
                                    <span class="left">
                                        <%:"Mode".Localize() %></span><a href="###" class="tooltip-link" title="<%:"Use <b>debug</b> on development and  use <b>release</b> on live site. <br/><b>Release</b> mode utilizes CSS/JS compression and other techniques".Localize() %>"></a>
                                </label>
                                <div class="clearfix">
                                    <p class="left">
                                        <input type="radio" name="Mode" value="0" id="Mode0" checked="checked" />
                                        <label for="Mode0" class="radio-label left">
                                            <%:"Debug".Localize() %></label>
                                    </p>
                                    <p>
                                        <input type="radio" name="Mode" value="1" id="Mode1" />
                                        <label for="Mode1" class="radio-label left">
                                            <%:"Release".Localize() %></label>
                                    </p>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </fieldset>
        </div>
    </div>
    <p class="buttons">
        <button type="submit">
            <%:"Save".Localize() %></button></p>
    <%}%>
    <script language="javascript" type="text/javascript">
		$(function () {
			kooboo.cms.ui.dynamicListInstance({
                containerId: 'container-<%:guid %>',
                templateId: 'template-<%:guid %>',
                addButtonId: 'add-<%:guid %>',
                propertyName:'Domains',
                data:<%=Model!=null&&Model.Domains!=null ?Model.Domains.ToJSON():"[]" %> 
            });
			var json = <%=ViewData["TemplatesJSON"] %>;

			getList(json);

			function getList(templateList,search){
				$('div.generate-by-code').fadeOut(function(){
					$(this).remove();
				});
				if (templateList) {

				templateList
				.where(function(o){ 
					return !search || o.TemplateName.toLowerCase().indexOf(search.toLowerCase()) >= 0 ; 
				}).each(function (item,index) {

						var templateDiv = $('#template-item-display').clone();
						templateDiv.removeClass('hide').addClass('generate-by-code').show('fade');
						var radio = templateDiv.find('input:radio').attr('checked',index == 0);
						var random = Math.random().toString().replace('.', "_");

						var radioId = 'template-radion-' + random;

						radio.attr('id', radioId).val(item.FullName);

						var label = templateDiv.find('label').attr('for', radioId);

						label.filter('.text-label').html(item.TemplateName);

						var img = templateDiv.find('img');

						if(item.Thumbnail){
							img.attr('src',item.Thumbnail);
						}

						$('#template-container').append(templateDiv);
					
				});
			}
			}

			var tempJson = json;
			$('#template-category').change(function(){
				
				var handle = $(this);

				var query = handle.val()? json.where(function(o){ return o.Category == handle.val(); }) : json ;
				tempJson = query;
				getList(query,$('#template-search').val());
			});

			$('#template-search').keydown(function(e) {
				if(e.keyCode == 13){
					return false;
				}
			}).keyup(function(e){
				var val = $(this).val();
				getList(tempJson,val);
			});

			$('form').submit(function(){
				var templateRadio = $('input:radio[name=Template]').change(function(){
					$('span[data-valmsg-for=Template]').hide();
				});
				if(templateRadio.filter('[checked]').length == 0){
					$('#template-container')
					.show('highlight',{},200).delay(200)
					.show('highlight',{},200).delay(200)
					.show('highlight',{},200);
					$('span[data-valmsg-for=Template]').show().html('<%:"Required" %>').addClass('field-validation-error');
					return false;
				}
				
			});


			$('#repository-toggle').toggle(function(){
				$(this).removeClass('add').addClass('cancel');
				$('select[name=Repository]').addClass('hide').attr('disabled',true);
				$('input[name=Repository]').removeClass('hide').removeAttr('disabled');
				$("#IsNew").val(true);
			},function(){
				$(this).removeClass('cancel').addClass('add');
				$('input[name=Repository]').addClass('hide').attr('disabled',true);
				$('select[name=Repository]').removeClass('hide').removeAttr('disabled');
				$("#IsNew").val(false);
			});


			$(function () {
				$("#Version").mask("9.9.9.9");
			});
		});
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
</asp:Content>
