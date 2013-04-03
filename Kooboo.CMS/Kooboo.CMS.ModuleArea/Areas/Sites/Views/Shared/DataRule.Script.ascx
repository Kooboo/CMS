<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script language="javascript" type="text/javascript">

    kooboo.namespace("kooboo.cms.sites.datarule");
    kooboo.cms.sites.datarule.extend({
        initFormEvent: function ($form) {
            var randomCls = 'random-' + Math.random().toString().replace('.', '-');
            kooboo.cms.ui.event.ajaxSubmit(function () {
                var form = this;
                var datarulesName = "DataRules";
                form.find('.' + randomCls).remove();

                
                $('div.pop-form-edit').each(function () {
                    var handleDiv = $(this);
                    var random = Math.random();
                    var postName = "DataRules"
                    var index = $('<input type="hidden" name="DataRules.Index"/>').addClass(randomCls);
                    index.val(random);
                    form.append(index);
                    var inputs = handleDiv.find('input');
                    inputs.each(function () {
                        var input = $(this).clone().removeAttr('id').addClass('hide ' + randomCls);
                        var nameAttr = input.attr('name');
                        input.attr('name', postName + '[' + random + '].' + nameAttr);
                        form.append(input);
                    });
                    var selects = handleDiv.find('select');
                    selects.each(function () {
                        var input = $(this)
                                                , selectedOption = input.find('option:selected').length ? input.find('option:selected') : input.find('option:eq(0)'),
                                                inputVal = selectedOption.get(0) ? (selectedOption.get(0).getAttribute('value') ? selectedOption.get(0).getAttribute('value') : '') : '',
                                                 nameAttr = input.attr('name'),
                                                 hidden = $('<input type="hidden"/>');

                        hidden.attr('name', postName + '[' + random + '].' + nameAttr).val(inputVal).addClass(randomCls);
                        form.append(hidden);
                    });
                });
            });
        }
    });

    kooboo.namespace("kooboo.cms.sites.datarule.step1");
    kooboo.cms.sites.datarule.step1.extend({
        initStep1: function () {
            var host = this;
            $(function () {
                if (!kooboo.data('step1-inited')) {
                    host._initStep1();
                    kooboo.data('step1-inited', true);
                }

            });
        },
        _initStep1: function () {
            $("#DataRuleStep1SubmitBtn")
            .unbind('click')
            .click(function () {
                var handle = $(this);
                var folderInfoUrl = $("#FolderInfoUrl").val();
                var popRandomId = Math.random().toString().replace('.', '-');

                var url = folderInfoUrl + '&f=' + $(':checkbox[name="DataRule.FolderName"][checked]').val();
                var checkbox = $(':checkbox[name="DataRule.FolderName"][checked]');

                function showPop(response) {
                    var parentPop = $.popContext.getCurrent() || kooboo.data("parentPop");
                    kooboo.data("parentPop", parentPop);
                    parentPop.close();
                    $.pop({
                        useContent: true,
                        openOnclick: false,
                        useClone: true,
                        title: '<%:"Add DataRule".Localize() %>',
                        useClone: true,
                        contentId: 'DataRuleStep2Div',
                        onload: function (handle, pop, config) {
                            pop.find('a.tooltip-link').each(function () {
                                $(this).attr('title', $(this).attr('ttitle'));
                                $(this).yardiTip({ offsetX: -20 });
                            });
                            pop.find('input:visible').first().focus();
                            pop.find('.step-1-enable')
								.removeClass('hide')
                                .unbind('click')
								.click(function () {
								    parentPop.open();
								    pop.close();
								});
                            pop.find('li.step-1-disable').addClass('hide');
                            var folderName = $(':checkbox[name="DataRule.FolderName"][checked]').val();

                            var takeOperationInput = $(':radio[name="TakeOperation"][checked]');
                            var takeOperation = takeOperationInput.val();

                            var dataRuleTypeValue = takeOperationInput.attr('dataruleTypeValue');

                            $.validator.unobtrusive.parse(document)
                            var form = pop.find('form');

                            form.find('[name="DataRule.DataRuleType"]').val(dataRuleTypeValue);

                            form.addHidden('DataRule.FolderName', response.Folder.FullName);
                            form.addHidden('TakeOperation', takeOperation);

                            var dataruleModel = {};

                            if (parseInt(takeOperation) == 1) {
                                dataruleModel = {
                                    "DataRule": {
                                        "CategoryClauses": null,
                                        "WhereClauses": [
                                        {
                                            "Logical": 0,
                                            "FieldName": "UserKey",
                                            "Operator": 0,
                                            "Value1": "{UserKey}",
                                            "Value2": ""
                                        }]
                                    }
                                };
                            }

                            kooboo.cms.sites.datarule.step2.init(dataruleModel, response.Schema, response.Folder,
                            {
                                $form: '#' + form.attr('id'),
                                saveCallBack: function () {
                                    kooboo.cms.sites.datarule.step2.initDataRuleGrid();
                                }
                            });

                            form.find("[id^=folderName]").html(response.Folder.FriendlyText);
                            form.find("[id^=schemaName]").html(response.Folder.SchemaName);

                            var categoryFolderOption = form
                            .find("select[name='DataRule.CategoryFolderName']");

                            categoryFolderOption.html('');
                            var optionStr = '<option><%:"Select a category".Localize() %></option>';
                            categoryFolderOption.append(optionStr);
                            if (response.CategoryFolders.length == 0) {
                                categoryFolderOption.parents('tr').hide();
                            } else {
                                categoryFolderOption.parents('tr').show();
                            }
                            response.CategoryFolders.each(function (value, index) {
                                var option = $(optionStr).val(value.FullName);
                                option.text(value.Name);
                                option.appendTo(categoryFolderOption);
                            });
                        }
                    });
                }

                if (kooboo.data(url)) {
                    var response = kooboo.data(url);
                    showPop(response);
                } else {
                    $.ajax({
                        url: url,
                        beforeSend: function () {
                            if (!$(':checkbox[name="DataRule.FolderName"][checked]').val()) {
                                if ($(':checkbox[name="DataRule.FolderName"]').first().length > 0) {
                                    $(':checkbox[name="DataRule.FolderName"]').first().focus();
                                } else {
                                    kooboo.alert('<%:"Current folder is null.".Localize() %>');
                                }
                                return false;
                            }
                            kooboo.cms.ui.loading().show();
                        },
                        data: { folderPath: $(':checkbox[name="DataRule.FolderName"][checked]').val() },
                        dataType: 'json',
                        type: 'post',
                        success: function (response) {
                            kooboo.data(url, response);
                            kooboo.cms.ui.messageBox().hide();
                            showPop(response);
                        },
                        error: function () {
                            kooboo.cms.ui.messageBox().hide();
                        }
                    });
                }

                return false;
            });
        }
    });

    kooboo.namespace("kooboo.cms.sites.datarule.step2");
    kooboo.cms.sites.datarule.step2.extend({
        init: function (dataruleSetting, schema, folderInfo, option) {
            var host = this;
            $(function () {
                host._init(dataruleSetting, schema, folderInfo, option);
            });
        },
        _init: function (dataruleSetting, schenma, folderInfo, option) {
            var config = {
                categoryFilterNull: "Please select one category!",
                failed: "Failed to save datarule!",
                $form: 'form',
                saveCallBack: function () { }
            };
            $.extend(config, option);

            var datarule = dataruleSetting.DataRule || [];
            var form = $(config.$form);

            var host = this;
            var _contentFilter = datarule.WhereClauses || [];
            var _categoryFilter = datarule.CategoryClauses || [];
            var advance = $(config.$form + " legend.clickable").toggle(function () {
                $(this).addClass("active").next().removeClass("hide").show();
            }, function () {
                $(this).removeClass("active").next().addClass("hide").hide();
            });

            var takeOperation = $(config.$form + " [name^=TakeOperation]");
            if (takeOperation.val() == 2 || takeOperation.val() == 1) {
                advance.hide().next().hide();
            }

            var contentFilterUl = $(config.$form + " ul.contentFilterUl");
            contentFilterUl.data("whereClauses", _contentFilter);
            contentFilterUl.data("whereClauseHidden", $(config.$form + " [id^=contentFilterFieldset]"));
            contentFilterUl.data('clauseName', 'DataRule.WhereClauses');
            contentFilterUl.data('form', form);
            contentFilterUl.data('schema', schenma);

            host.initWhereClauseHidden(contentFilterUl);

            var categoryFilterUl = $(config.$form + " ul.categoryFilterUl");
            categoryFilterUl.data("whereClauses", _categoryFilter);
            categoryFilterUl.data("whereClauseHidden", $(config.$form + " [id^=categoryFilterFieldset]"));
            categoryFilterUl.data('clauseName', 'DataRule.CategoryClauses');
            categoryFilterUl.data('form', form);
            categoryFilterUl.data('schema', schenma);

            host.initWhereClauseHidden(categoryFilterUl);



            (function initAdd() {
                $(config.$form + " [id^=addContentFilter]")
                .unbind('click')
                .click(function () {
                    host.generateEditor(contentFilterUl);
                    host.generateFilterFieldDataSource(contentFilterUl);
                    return false;
                });
                $(config.$form + " [id^=addCategoryFilter]").unbind('click').click(function () {
                    host.generateEditor(categoryFilterUl);
                    host.generateFilterFieldDataSource(categoryFilterUl);
                    return false;
                });
            })();
            var filterTemplate = $(config.$form + " [id^=filterTemplate]");

            (function initFilterTemplateSave() {

                filterTemplate.find("input").keydown(function (e) {
                    if (e.keyCode == 13) {
                        templateSaveBtn.click();
                        return false;
                    }
                });

                var templateSaveBtn = filterTemplate.find("a.save.button").unbind('click').bind('click', function () {
                    var whereClause = filterTemplate.data("whereClause");
                    var ul = filterTemplate.data("ul");
                    if (whereClause) {
                        host.getFilterList(ul).where(function (o) {
                            if (o == whereClause) {
                                o.FieldName = filterTemplate.find("select[name=FieldName]").val();
                                o.Operator = filterTemplate.find("select[name=Operator]").val();
                                o.Logical = filterTemplate.find("select[name=Logical]").val();
                                o.Value1 = filterTemplate.find("input[name=Value1]").val();
                            }
                        });
                        filterTemplate.removeData("whereClause");
                    } else {
                        whereClause = {};
                        whereClause.FieldName = filterTemplate.find("select[name=FieldName]").val();
                        whereClause.Operator = filterTemplate.find("select[name=Operator]").val();
                        whereClause.Value1 = filterTemplate.find("input[name=Value1]").val();

                        whereClause.Logical = filterTemplate.find("select[name=Logical]").val();

                        host.getFilterList(ul).push(whereClause);
                    }

                    host.generateFilterDisplay(ul);
                    host.initDataRuleType(ul);
                    host.initWhereClauseHidden(ul);
                    filterTemplate.addClass("hide");

                    return false;
                });
                filterTemplate.find("a.cancel").click(function () {
                    filterTemplate.addClass("hide");
                    return false;
                });
            })();

            (function initFilterDisplay() {
                host.generateFilterDisplay(contentFilterUl);
                host.generateFilterDisplay(categoryFilterUl);
            })();

            var sortfield = $(config.$form + " [id^=sortfield]");


            kooboo.data(sortfield.attr('id'), datarule.SortField || 'Sequence');

            for (var i = 0; i < schenma.AllColumns.length; i++) {
                var value = schenma.AllColumns[i];
                var option = $('<option></option>');

                if (kooboo.data(sortfield.attr('id')) == value.Name) {
                    option.attr('selected', true);
                }

                option.val(value.Name).html(value.Name)
                option.appendTo(sortfield);
            }

            sortfield.find('option[value="' + kooboo.data(sortfield.attr('id')) + '"]').attr('selected', true);


            var resultType = takeOperation.val() || dataruleSetting.TakeOperation;

            this.generateFilterFieldDataSource(categoryFilterUl);
            var cagegoryFolderSelect = $(config.$form + " select[name='DataRule.CategoryFolderName']").change(function () {
                var handle = $(this);
                var selectedCategoryOption = handle.find('option:selected');
                var cagegoryFolderSelectVal = selectedCategoryOption.length ? selectedCategoryOption.get(0).getAttribute('value') : undefined;
                if (!cagegoryFolderSelectVal) {
                    handle.parents('tr:eq(0)').next().hide();
                } else {
                    handle.parents('tr:eq(0)').next().show();
                }
                categoryFilterUl.data("whereClauses", []);
                host.generateFilterDisplay(categoryFilterUl);
                host.initWhereClauseHidden(categoryFilterUl);

                host.generateFilterFieldDataSource(categoryFilterUl);
            });
            cagegoryFolderSelect.find('option[value="' + datarule.CategoryFolderName + '"]').attr('selected', true).attr('s', true);

            var selectedCategoryOption = cagegoryFolderSelect.find('option:selected');
            var cagegoryFolderSelectVal = selectedCategoryOption.length ? selectedCategoryOption.get(0).getAttribute('value') : undefined; //cagegoryFolderSelect.val() || cagegoryFolderSelect.find('option:selected').val();

            if (cagegoryFolderSelectVal) {
                cagegoryFolderSelect.val(cagegoryFolderSelectVal)
                cagegoryFolderSelect.parents('tr:eq(0)').next().show();

            } else {
                cagegoryFolderSelect.parents('tr:eq(0)').next().hide();
            }

            if (cagegoryFolderSelect.children().length == 1) {
                cagegoryFolderSelect.parents('tr:eq(0)').hide();
            }
            (function initForm() {

                var dataNameInput = form.find('input[name=DataName]');

                var prevDataName = '';

                var oldDataName = dataNameInput.val();

                function checkDataName() {
                    var currentDataRuleArray = kooboo.data("datarule-list");
                    var repeat = false;

                    var query = currentDataRuleArray.where(function (value, index) {
                        return (oldDataName != dataNameInput.val()) && (value.DataName == dataNameInput.val());
                    });

                    if (query.length > 0) {
                        dataNameInput.addClass('input-validation-error').focus().next().show().html(dataNameInput.attr('repeatMsg')).addClass('field-validation-error');
                        return false;
                    } else {
                        dataNameInput.removeClass('input-validation-error').next().hide();
                    }

                    return true;

                }

                form.validate();
                form.submit(function () {
                    if (form.valid()) {

                        var validDataName = checkDataName();

                        if (!validDataName) {
                            return false;
                        }

                        form.parent('div').addClass('pop-form-edit');

                        var tempForm = kooboo.cms.ui.formHelper.tempForm({}, form.attr('action'), 'DataRule', { method: 'post' });
                        $('div.pop-form-edit').each(function () {
                            var handleDiv = $(this);
                            var random = Math.random();
                            var postName = "DataRules"
                            var index = $('<input type="hidden" name="DataRules.Index"/>');
                            index.val(random);
                            tempForm.form.append(index);
                            var inputs = handleDiv.find('input');

                            inputs.each(function () {
                                var input = $(this);
                                if (!input.attr('disabled')) {
                                    var nameAttr = input.attr('name');
                                    var hidden = $('<input type="hidden"/>');
                                    hidden.attr('name', postName + '[' + random + '].' + nameAttr).val(input.val());
                                    tempForm.form.append(hidden);
                                }

                            });

                            var selects = handleDiv.find('select:enabled');
                            selects.each(function () {
                                var input = $(this)
                                , selectedOption = input.find('option:selected').length ? input.find('option:selected') : input.find('option:eq(0)'),
                                inputVal = selectedOption.get(0) ? (selectedOption.get(0).getAttribute('value') ? selectedOption.get(0).getAttribute('value') : '') : '',
                                 nameAttr = input.attr('name'),
                                 hidden = $('<input type="hidden"/>');

                                hidden.attr('name', postName + '[' + random + '].' + nameAttr).val(inputVal);
                                tempForm.form.append(hidden);
                            });
                        });
                        tempForm.form.ajaxSubmit({
                            beforeSend: function () {
                                kooboo.cms.ui.loading().show();
                            },
                            success: function (response) {
                                $('div.pop-form-edit').remove();
                                $('#grid-form-container').html(response);

                                kooboo.cms.ui.messageBox().hide();
                                if ($.popContext.getCurrent() != null) {
                                    $.popContext.getCurrent().close();
                                }
                                config.saveCallBack();
                            },
                            error: function (XMLHttpRequest, textStatus, errorThrown) {
                                kooboo.cms.ui.messageBox().hide();
                            }
                        })
                    }
                    return false;
                });
            })();
            (function initPagingCheck() {
                var pageIndex = $(config.$form + " [id^=pageindex]");
                var pageSize = $(config.$form + " [id^=pagesize]");
                var paging = $(config.$form + " [id^=paging]").change(function () {
                    checkChange(this);
                });

                checkChange(paging);

                function checkChange(checkbox) {
                    if ($(checkbox).attr("checked")) {
                        pageIndex.attr({ disabled: false });
                        pageSize.attr({ disabled: false });
                    } else {
                        pageIndex.attr({ disabled: true });
                        pageSize.attr({ disabled: true });
                    }
                }
            })();
        },
        initWhereClauseHidden: function (ul) {
            var data = ul.data("whereClauses");
            var clauseName = ul.data("clauseName");
            var fieldSet = ul.data("whereClauseHidden");
            fieldSet.html('');
            kooboo.cms.ui.formHelper.createHidden(data, clauseName, fieldSet);
        },
        getFilterList: function (ul) {
            return ul.data("whereClauses");
        },
        generateEditor: function (ul, whereClause) {

            ul = $(ul);

            var filterTemplate = ul.parents('form').find('[id^=filterTemplate]').removeClass("hide").hide();

            ul.after(filterTemplate);
            filterTemplate.delay(100).show('highlight', {}, 500);
            setTimeout(function () {
                filterTemplate.find('input,select').first().focus();
            }, 100);

            filterTemplate.data("ul", ul);
            filterTemplate.data("whereClause", whereClause);
            if (whereClause) {
                kooboo.data('set-filter-template', function () {
                    setTimeout(function () {
                        kooboo.cms.ui.formHelper.setForm(whereClause, filterTemplate);
                    }, 100);
                });

                kooboo.data('set-filter-template')();
            } else {
                filterTemplate.find("input[name=Value1]").val(null);
            }
        },
        generateFilterDisplay: function (ul) {
            var host = this;
            ul.html('');
            this.getFilterList(ul).each(function (whereClause, index) {
                ul.append(host.translate(whereClause, ul));
            });
        },
        generateFilterFieldDataSource: function (ul) {

            var fieldList = ul.parents('form').find("[id^=filterTemplate]").find("select[name=FieldName]");
            ///category filter

            if (ul.hasClass('categoryFilterUl')) {
                this.setCategoryFieldRemote(ul);
                /// content  filter
            } else if (ul.hasClass('contentFilterUl')) {
                this.initFieldList(ul.data('schema'), ul);
            }
        },
        setCategoryFieldRemote: function (ul) {
            var form = ul.parents('form');


            var handle = form.find(" select[name='DataRule.CategoryFolderName']");
            var host = this;
            var url = $(handle).attr("folderInfoUrl");

            var handleChange = kooboo.data('CategoryFolderName-handle-val') == handle.val();
            kooboo.data('CategoryFolderName-handle-val', handle.val())

            var dataRuleType = form.find('[name="DataRule.DataRuleType"]').val();
            var sortfield = form.find("[id^=sortfield]");



            kooboo.data(sortfield.attr('id'), sortfield.val());
            //kooboo.dump(kooboo.data(sortfield.attr('id')));

            function initSortField(datasource) {
                if (!datasource) {
                    return false;
                }

                if (parseInt(dataRuleType) == 2) {
                    if (handleChange) {
                        sortfield.html('');
                        datasource = datasource.AllColumns;
                        datasource.each(function (value, index) {
                            var option = $('<option></option>');
                            if (kooboo.data(sortfield.attr('id')) == value.toString()) {
                                option.attr('selected', true);
                            }
                            option.val(value.Name).html(value.Name)
                            option.appendTo(sortfield);
                        });
                        sortfield.find('option[value="' + kooboo.data(sortfield.attr('id')) + '"]').attr('selected', true);

                    }
                }
            }


            url = url + '&f=' + handle.val();

            if (kooboo.data(url)) {
                host.initFieldList(kooboo.data(url).Schema, ul);
                initSortField(kooboo.data(url).Schema);
            } else {
                if (handle.val()) {
                    $.post(url, { folderPath: handle.val() }, function (response) {
                        kooboo.data(url, response);
                        host.initFieldList(kooboo.data(url).Schema, ul);
                        initSortField(kooboo.data(url).Schema);
                    });
                }
            }
            return true;
        },
        translate: function (whereClause, ul) {
            var host = this;
            var li = $('<li class="clearfix"><span></span><a href="javascript:;" title="Edit" class="o-icon edit"></a>   <a href="javascript:;" title="Remove" class="o-icon remove"></a></li>');

            var translateLogic = this.translateLogic(whereClause.Logical, ul);

            li.find("span").html(whereClause.FieldName + "&nbsp;&nbsp;" + this.translateOperator(whereClause.Operator, ul) + "&nbsp;&nbsp;" + whereClause.Value1 + "&nbsp;&nbsp;" + translateLogic.value + "&nbsp;&nbsp;");

            li.data("isGroup", translateLogic.isGroup);

            var action = li.find("a");

            //UNKNOW fixed not so better.if remove and edit link are hide in ie , it will never be showed
            if (!$.browser.msie) { action.hide(); }

            li.hover(function () { action.show(); }, function () { action.hide(); });

            var edit = li.find("a[title=Edit]");

            edit.click(function (e) {
                e.preventDefault();
                host.generateEditor(ul, whereClause);
                host.generateFilterFieldDataSource(ul);
                kooboo.data('edit-filter-tempalte', true);
            });

            var remove = li.find("a[title=Remove]");

            remove.click(function () {
                li.remove();
                var whereClauses = ul.data("whereClauses").removeElement(whereClause);
                ul.data("whereClauses", whereClauses);
                host.initDataRuleType(ul);
                host.initWhereClauseHidden(ul);
            });

            return li;
        },
        translateOperator: function (operator, ul) {
            if (operator == undefined) {
                return null;
            }
            operator = operator.toString();
            var dic = {
                "0": "==",
                "1": "!=",
                "2": "&gt;",
                "3": "&lt;"
            };
            var option = ul.parents('form').find("[id^=filterTemplate]").find("select[name=Operator]").children();
            return dic[operator.trim().toLowerCase()] || $(option.get(parseInt(operator))).text();

        },
        translateLogic: function (logic, ul) {
            if (logic == undefined) {
                return {};
            }
            logic = logic.toString();
            var dic = {
                "0": " AND ",
                "1": " OR ",
                "2": " Than AND ",
                "3": " Than OR "
            };
            var option = ul.parents('form').find("[id^=filterTemplate]").find("select[name=Logic]").children();

            var result = {
                value: dic[logic.toString().trim().toLowerCase()] || $(option.get(parseInt(logic))).text(),
                isGroup: !!(dic[logic.trim().toLowerCase()])
            }

            return result;
        },
        initFieldList: function (schema, ul) {
            if (ul.next().hasClass('filter-form')) {
                var fieldList = ul.parents('form').find("[id^=filterTemplate]").find("select[name=FieldName]");
                fieldList.html('');
                schema.AllColumns.each(function (value, index) {
                    var option = $("<option></option>").val(value.Name).text(value.Name);

                    fieldList.append(option);
                });

                if (kooboo.data('edit-filter-tempalte')) {
                    kooboo.data('set-filter-template')();
                    kooboo.data('edit-filter-tempalte', false);
                }
            }
        },
        initDataRuleType: function (ul) {
            var categoryFilterNullable = true;
            var dataRuleType = ul.parents('form').find("[id^=DataRule_DataRuleType]");
            var takeOperation = parseInt(ul.parents('form').find(" [name=TakeOperation]").val());
            var categoryFilterUl = ul.parents('form').find("ul.categoryFilterUl");
            var whereClauses = categoryFilterUl.data("whereClauses");
            if (takeOperation == 3) {//query category
                dataRuleType.val(2);
                if (!ul.parents('form').find("[name=CategoryFolderName]").val()) {
                    categoryFilterNullable = false;
                }
            } else {//not query 
                if (whereClauses.length > 0) {
                    dataRuleType.val(3);
                } else {
                    dataRuleType.val(0);
                }
                categoryFilterNullAble = true;
            }
            return categoryFilterNullAble;
        },
        initDataRuleGrid: function () {
            var grid = $("#grid-form-container").children('.grid-field');
            var container = $("#grid-field-container").html('');
            grid.appendTo(container);


            grid.find("a.pop-edit").pop({
                useContent: true,
                contentId: function (handle) { return handle.attr('contentid'); },
                width: 600,
                height: 400
            });

            grid.find("a.pop-remove").each(function () {
                var handle = $(this);
                var contentid = handle.attr('contentid');
                var confirmMsg = handle.attr('confirmMsg');
                handle.click(function () {
                    kooboo.confirm(confirmMsg, function (r) {
                        if (r) {

                            var tr = handle.parents('.datarule-row').fadeOut(function () {
                                //remove element from json array
                                var dataruleList = kooboo.data("datarule-list");
                                if (dataruleList) {
                                    dataruleList = dataruleList.remove(function (o) { return o.DataName == tr.attr('dataname'); });
                                }
                                kooboo.data("datarule-list", dataruleList)
                                tr.remove();
                                $('#' + contentid).remove();

                                kooboo.cms.ui.status().stop();
                            });



                        }
                    });
                    return false;
                });
            });
        },
        destory: function () { //unbind event's and clear flags

        }
    });

</script>
