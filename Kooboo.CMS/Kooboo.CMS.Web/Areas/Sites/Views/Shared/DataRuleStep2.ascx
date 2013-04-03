<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Kooboo.CMS.Sites.Models.DataRuleSetting>" %>
<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<%@ Import Namespace="Kooboo.CMS.Web.Areas.Sites.Models" %>
<%@ Import Namespace="Kooboo.CMS.Sites.Models" %>
<%@ Import Namespace="Kooboo.CMS.Sites.View" %>
<%@ Import Namespace="Kooboo.Web.Script.Serialization" %>
<% 
    var viewEngine = Request["Engine"] ?? (ViewData["ViewEngine"] == null ? "" : ((ITemplateEngine)ViewData["ViewEngine"]).Name);
    var guid = Guid.NewGuid();
    var model = Kooboo.CMS.Web.Models.ModelHelper.ParseViewData<DataRuleSetting>(Model);
    var folder = new TextFolder();
    var schema = new Schema();
    IEnumerable<TextFolder> categoryFolders = new List<TextFolder>();

    if (model.DataRule != null)
    {
        var repository = new Repository(Kooboo.CMS.Sites.Models.Site.Current.Repository).AsActual();
        folder = Kooboo.CMS.Content.Models.FolderHelper.Parse<TextFolder>(repository, Model.DataRule.FolderName);

        folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(folder);

        if (folder.Categories != null)
        {
            categoryFolders = folder.Categories.Select(o => Kooboo.CMS.Content.Models.FolderHelper.Parse<TextFolder>(repository, o.FolderName));
        }


        schema = new Kooboo.CMS.Content.Models.Schema(repository, folder.SchemaName).AsActual();
    }

    dynamic datarule = model.DataRule;
%>
<h3 class="title">
    <%:"Edit Filters".Localize() %></h3>
<ul class="step clearfix">
    <li class="hide step-1-enable"><a href="javascript:;">
        <%:"Choose Folder".Localize() %></a> </li>
    <li class="step-1-disable">
        <%:"Choose Folder".Localize() %></li>
    <li>&gt;</li>
    <li class="current">
        <%:"Edit Filters".Localize() %></li>
</ul>
<form method="post" action="<%:this.Url.Action("DataRuleGridForms",ViewContext.RequestContext.AllRouteValues().Merge("Engine",viewEngine)) %>"
id="form<%:guid %>" class="no-ajax">
<input type="hidden" value="<%:datarule!=null? (int)datarule.DataRuleType :0%>" name="DataRule.DataRuleType"
    id="DataRuleType<%:guid %>" />
<input type="hidden" value="<%:(int)model.TakeOperation %>" name="TakeOperation" />
<input type="hidden" name="DataRule.FolderName" value="<%:folder.FullName %>" />
<fieldset id="contentFilterFieldset<%:guid %>" class="hide">
</fieldset>
<fieldset id="categoryFilterFieldset<%:guid %>" class="hide">
</fieldset>
<div class="common-form datarule-form">
    <fieldset>
        <legend></legend>
        <table>
            <tbody>
                <tr>
                    <th>
                        <label>
                            <%:"DataName".Localize() %></label>
                    </th>
                    <td>
                        <input type="text" value="<%:model.DataName %>" name="DataName" data-val-required="<%:"Required".Localize() %>"
                            data-val-regex-pattern="^[a-zA-Z_][a-zA-Z0-9_]{0,50}$" data-val-regex="<%: "Only alphabetical characters allowed".Localize()%>"
                            data-val="true" repeatmsg="<%:"This dataname is being used by other datarule.".Localize() %>" />
                        <span name="" data-valmsg-replace="true" data-valmsg-for="DataName" class="field-validation-valid">
                        </span>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            <%:"Folder".Localize() %></label>
                    </th>
                    <td class="text-folderPath" id="folderName<%:guid %>">
                        <%:folder.FriendlyText %>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            <%:"Content Type".Localize() %></label>
                    </th>
                    <td class="text-contentType" id="schemaName<%:guid %>">
                        <%:schema.Name %>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            <%:"Content filter".Localize() %></label><a href="###" title="<%:"Query condition filter, you may use value from URL string, for example: {querystringkey}".Localize() %>"
                                ttitle="<%:"Query condition filter, you may use value from URL string, for example: {querystringkey}".Localize() %>"
                                class="tooltip-link"></a>
                    </th>
                    <td>
                        <ul class="filter-list contentFilterUl" id="contentFilterUl<%:guid %>">
                        </ul>
                        <div class="filter-form clearfix hide" id="filterTemplate<%:guid %>">
                            <h5>
                                <%:"Filter".Localize() %>:
                            </h5>
                            <table>
                                <tbody>
                                    <tr>
                                        <td>
                                            <select class="medium schemafields" name="FieldName">
                                            </select>
                                            <select class="short" name="Operator">
                                                <option value="0">
                                                    <%: "Equals".Localize()%></option>
                                                <option value="1">
                                                    <%: "NotEquals".Localize()%></option>
                                                <option value="2">
                                                    <%:"Greater than".Localize() %></option>
                                                <option value="3">
                                                    <%:"Less than".Localize()%></option>
                                                <option value="4">
                                                    <%: "Contains".Localize()%></option>
                                                <option value="5">
                                                    <%:"Start with".Localize() %></option>
                                                <option value="6">
                                                    <%:"End with".Localize()%></option>
                                            </select>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input type="text" class="medium" name="Value1" />
                                            <select class="short" name="Logical">
                                                <option value="0">
                                                    <%: "AND".Localize() %></option>
                                                <option value="1">
                                                    <%:"OR".Localize() %></option>
                                                <option value="2">
                                                    <%:"Than AND".Localize() %></option>
                                                <option value="3">
                                                    <%:"Than OR".Localize() %></option>
                                            </select>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <a class="button save" href="javascript:;">
                                <%:"Save".Localize() %></a> <a class="button cancel" href="javascript:;">
                                    <%:"Cancel".Localize() %></a>
                        </div>
                        <a href="javascript:;" id="addContentFilter<%:guid %>" title="Add" class="o-icon add filter-form-link">
                        </a>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label>
                            <%:"Category Folder".Localize() %>
                        </label>
                    </th>
                    <td>
                        <%:Html.Partial("CategoryFolderDropdown",categoryFolders) %>
                    </td>
                </tr>
                <tr style="display: none">
                    <th>
                        <label>
                            <%:"Category filter".Localize() %></label>
                    </th>
                    <td>
                        <ul class="filter-list categoryFilterUl" id="categoryFilterUl<%:guid %>">
                        </ul>
                        <a href="javascript:;" id="addCategoryFilter<%:guid %>" title="Add" class="filter-form-link o-icon add">
                        </a>
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <fieldset>
        <legend class="clickable clearfix no-bind"><span>
            <%:"Advanced".Localize() %>
        </span></legend>
        <table style="display: none;">
            <tbody>
                <tr>
                    <th>
                        <label for="top">
                            <%:"Top".Localize() %></label>
                    </th>
                    <td>
                        <input class="short" id="top<%:guid %>" name="DataRule.Top" value="<%:datarule!=null? datarule.Top : 50%>"
                            type="text" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="sortfiled">
                            Sort filed</label>
                    </th>
                    <td>
                        <select id="sortfield<%:guid %>" name="DataRule.SortField" class="short">
                        </select>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="sortdirection">
                            <%:"Sort direction".Localize() %></label>
                    </th>
                    <td>
                        <select id="sortdirection<%:guid %>" name="DataRule.SortDirection" class="short">
                            <option value="0" <%if(datarule!=null&& (int)datarule.SortDirection==0){%>selected<%} %>>
                                ASC</option>
                            <option value="1" <%if(datarule!=null&& (int)datarule.SortDirection==1||datarule==null){%>selected<%} %>>
                                DESC</option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="paging">
                            <%:"Enable paging".Localize() %></label>
                    </th>
                    <td>
                        <input id="paging<%:guid %>" name="paging" type="checkbox" <%if(datarule!=null&&(!string.IsNullOrWhiteSpace(datarule.PageSize)||!string.IsNullOrWhiteSpace(datarule.PageIndex))){%>
                            checked <%}%> />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="pagesize">
                            <%:"Page size".Localize() %></label>
                    </th>
                    <td>
                        <input class="short" id="pagesize<%:guid %>" name="DataRule.PageSize" type="text"
                            value="<%:(datarule!=null&& !string.IsNullOrEmpty( datarule.PageSize))?datarule.PageSize:"{PageSize}" %>" />
                    </td>
                </tr>
                <tr>
                    <th>
                        <label for="pageindex">
                            <%:"Page index".Localize() %></label>
                    </th>
                    <td>
                        <input class="short" id="pageindex<%:guid %>" name="DataRule.PageIndex" type="text"
                            value="<%:datarule!=null&&!string.IsNullOrEmpty( datarule.PageIndex)?datarule.PageIndex:"{PageIndex}" %>" />
                    </td>
                </tr>
            </tbody>
        </table>
    </fieldset>
    <p class="buttons">
        <button class="hide step-1-enable" type="button">
            «
            <%:"Previous".Localize() %>
        </button>
        <button type="submit" class="dialog-button">
            <%:"Save".Localize() %></button>
    </p>
</div>
</form>
<%if (!string.IsNullOrWhiteSpace(Model.DataName))
  {%>
<script language="javascript" type="text/javascript">

	$(function(){
		$('#form<%:guid %>').find('a.tooltip-link').each(function () {
			$(this).attr('title', $(this).attr('ttitle'));
			$(this).yardiTip({ offsetX: -20 });
		});
	});
	if(!kooboo.data('<%:guid %>-inited')){
		kooboo.data('<%:guid %>-inited',true);
		kooboo.cms.sites.datarule.step2.init(<%=model.ToJSON() %>,
		<%=schema.ToJSON() %>,
		<%=folder.ToJSON() %>,
		{
			categoryFilterNull: '<%:"Please select one category!".Localize() %>',
			failed: '<%:"Failed to save datarule!".Localize() %>',
			$form:'#form<%:guid %>',
			saveCallBack:function(){kooboo.cms.sites.datarule.step2.initDataRuleGrid();}
		});
	}

		
</script>
<% }%>
