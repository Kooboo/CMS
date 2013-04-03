<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<Kooboo.CMS.Sites.Models.Parameter>>" %>
<%
    var fullPropName = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(".", "_");
    ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');

    var guid = Guid.NewGuid();
    //var fullPropertyName = ViewData.TemplateInfo.HtmlFieldPrefix;
%>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.'); %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<%: Html.ValidationMessage(ViewData.ModelMetadata,null) %>
<div class="task-block parameters">
    <h3 class="title">
        <span>
            <%="Parameters".Localize() %></span><span class="arrow"></span></h3>
    <div class="content">
        <p class="buttons clearfix">
            <a class="button add" href="javascript:;" id="add-<%:guid %>">
                <%="Add".Localize()%></a>
        </p>
        <ul class="parameters list" id="container-<%:guid %>">
            <li id="template-<%:guid %>">
                <div class="clearfix">
                    <span class="name">Name</span> <a class="o-icon remove right" href="javascript:;">Edit</a><a
                        class="o-icon edit right" href="javascript:;">Remove</a> <a href="javascript:;" class="o-icon csharp right">
                        </a>
                </div>
                <div class="dialog-form common-form" style="display: none;">
                    <fieldset>
                        <table>
                            <tr>
                                <th>
                                    <label>
                                        <%:"Name".Localize() %></label>
                                </th>
                                <td>
                                    <input type="text" name="Name" fieldname="Name" class="medium" />
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <label>
                                        <%:"DataType".Localize() %></label>
                                </th>
                                <td>
                                    <select name="DataType" class="medium">
                                        <%
                                            foreach (var d in Enum.GetNames(typeof(Kooboo.Data.DataType)))
                                            { %>
                                        <option value="<%:(int)Enum.Parse(typeof(Kooboo.Data.DataType),d) %>">
                                            <%:d%></option>
                                        <% }%>
                                    </select>
                                </td>
                            </tr>
                            <tr class="value-input">
                                <th>
                                    <label>
                                        <%:"Default Value".Localize() %></label>
                                </th>
                                <td>
                                    <input type="text" name="Value" class="medium" />
                                </td>
                            </tr>
                            <tr class="value-input-bool">
                                <th>
                                    <label>
                                        <%:"Default Value".Localize() %></label>
                                </th>
                                <td>
                                    <input type="checkbox" value="True" name="Value" fieldtype="checkbox" />
                                    <input type="hidden" value="False" name="Value" fieldtype="checkbox" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    <p class="buttons">
                        <a class="button save" href="javascript:;">
                            <%:"Save".Localize() %></a> <a class="button cancel" href="javascript:;">
                                <%:"Cancel".Localize()%></a>
                    </p>
                </div>
            </li>
        </ul>
    </div>
</div>
<% var viewEngine = (Kooboo.CMS.Sites.View.ITemplateEngine)ViewData["ViewEngine"]; %>
<script language="javascript" type="text/javascript">
	
    <%=viewEngine.GetCodeHelper().RegisterParameterCode() %>
	$(function () {
			var textArea = $('textarea');
			
			var moveDialogFuncList = [];
			var ul = $('#container-<%:guid %>');

			var dataypeDic = {
				_unbindList:(function(){
					var unbindList = [];

					unbindList.push(function(input){
						input.unbind('input-bind');
					});

					unbindList.push(function(input){
						try{
							input.datepicker('destroy');
							}catch(E){}
					});

					return unbindList;
				})(),
				unbind:function(input){
					this._unbindList.each(function(func){
						func(input);
					});
				},
				/// string 
				'0':{
					display:'tr.value-input',
					action:function(input){
						input.unbind('input-bind');
					}
				},

				/// int 
				'1':{
					display:'tr.value-input',
					action:function(input){
						input.unbind('input-bind').bind('input-bind',function(){
							
						});
					}
				},

				/// decimal
				'2':{
					display:'tr.value-input',
					action:function(input){
						input.unbind('input-bind').bind('input-bind',function(){
							
						});
					}
				},

				/// datetime
				'3':{
					display:'tr.value-input',
					action:function(input){				
						input.unbind('input-bind').datepicker().datepicker('enable');
					}
				},

				/// bool
				'4':{
					display:'tr.value-input-bool',
					action:function(input){
						
					}
				}
			};

            var dyInstance = kooboo.cms.ui.dynamicListInstance({
            containerId: 'container-<%:guid %>', //the container that you put the new item in. 
            templateId:'template-<%:guid %>',
            propertyName: '<%:fullPropName%>', //str 
            addButtonId: 'add-<%:guid %>', //add button
            data:eval('(<%=Model.ToJSON() %>)') , //json data to fill the form field.
			delClass: 'del',
            fixInput:false,
			hideRemoveBtn:false,
			onInit:function(){
                var instance = this;
                $('a[href="javascript:;"]').click(function(){
                    return false;
                });
				$("select[name$=DataType]")
				.unbind('change-datatype')
				.bind('change-datatype',function(){
					var select = $(this);
					var paretnTr = select.parents('tr:eq(0)');
					var dicVal = dataypeDic[select.val()];
					paretnTr.nextAll().hide().find('input').attr('disabled',true);
					var input = paretnTr.nextAll(dicVal.display).show().find('input').removeAttr('disabled');

					dataypeDic.unbind(input);

					dicVal.action(input);

				}).change(function(){
					$(this).trigger('change-datatype');
				}).change();

                instance.getItems().each(function(){
					var li = $(this);

					moveDialogFuncList.push(function(){
						dialog.parents('div.ui-dialog').appendTo(li);
					});

					function removeLi(){
						moveDialogFuncList.each(function(value,index){
							value();
						});
                        dialog.remove();
						instance.remove(li);
					}

					var nameLink = li.find('.name');

                    var sharpLink = li.find('.csharp');

					sharpLink.click(function(){
						var codeMirrorAPI = textArea.data("codeMirror");
						var insertText =getParameterCode( nameLink.html());
						codeMirrorAPI.insertAtCursor(insertText);
					});

					var random = Math.random().toString().replace('.','-');

					var dialog = li.find('div.dialog-form').attr('id','dialog-'+random);

					///edit link
					li.find('a.edit').click(function(){
						li.data('add-parameter-success',true);
						dialog.dialog('open');
					});
					
					///remove link
					li.find('a.remove').click(function(){
						removeLi();
						dialog.remove();
					});

					dialog.dialog({
						autoOpen:false , 
						modal:true,
						width:500,
						height:240,
						title:'<%:"Parameters".Localize() %>',
						close:function(){
						if(!li.data('add-parameter-success')){
							removeLi();
							dialog.remove();
						}} 
					});

					///cancel button
					dialog.find('a.cancel').click(function(){
						dialog.dialog('close');
					});
					var nameField = dialog.find('input[fieldName=Name]').attr('random',random);
					var oldName = nameField.val();
					nameLink.html(oldName);
					/// save button
					dialog.find('a.save').click(function(){
						var nameList = $('input[fieldName=Name]');
						var nameArray = [];
						nameList.each(function(){
							if($(this).attr('random') != nameField.attr('random')){
								nameArray.push($(this).val());
							}
						});

						var val = nameField.val();
						if(!val){
							alert('<%:"Name is required.".Localize() %>');
								return false;
						}
						if(val!=oldName){
							if(nameArray.where(function(o){return o == val;}).length>0){
								alert('<%:"This name is being used by other parameters.".Localize() %>');
								return false;
							}
						}
						

						li.data('add-parameter-success',true);

						li.show().find('.name').html(dialog.find('input[fieldName=Name]').val());

						dialog.dialog('close');
					});

					///cache dialog
					li.data('dialog',dialog);
				});
			}
        });
		$('#add-<%:guid %>').click(function(){
			var li = dyInstance.getItems().last().hide();
			var dialog = li.data('dialog').dialog('open');
            return false;
		});

		var form = $('form');
		kooboo.cms.ui.event.ajaxSubmit(function(){
			form.find('input.generate-by-machine-parameters').remove();

            $('div.dialog-form').each(function(){
                var dialogForm = $(this);
                if(dialogForm.parents('form').length == 0){
                    var inputs =  dialogForm.find('input:enabled,select:enabled');

			        inputs.each(function(){
				        var input = $(this);
				        if(input.attr('fieldtype')=='checkbox'){
					        if(input.attr('checked')){
						        var hidden = $('<input type="hidden"/>').attr('name',input.attr('name')).val(input.val()).addClass('generate-by-machine-parameters');
						        hidden.appendTo(form);
					        }else{
						        //do nothing
					        }
				        }else{
					        var hidden = $('<input type="hidden"/>').attr('name',input.attr('name')).val(input.val()).addClass('generate-by-machine-parameters');
				            hidden.appendTo(form);
				        }
			        });
                }
            });

            

		});

	});

</script>
