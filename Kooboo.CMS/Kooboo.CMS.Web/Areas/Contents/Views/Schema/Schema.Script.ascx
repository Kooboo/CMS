<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script language="javascript" type="text/javascript">

    kooboo.namespace('kooboo.cms.content.schema.column');

    kooboo.cms.content.schema.column.extend({

        registerControl: function (controlName, controlClass) {
            /*
            controlClass should implement 
            {
            datatype : '0' , // '0' is the datatype's value
            action : function(form) { // when this control is selected, you may want it to do something,for example you want to change the ui element.
                    
            }
            }
            you can register control like 
            kooboo.cms.content.schema.column.registerControl('CustomControl',{
            datatype : '0',
            action : function(form) {
            //do something here , may be you want to change the display logic.
            }
            });
            */
            this._sysControltype[controlName] = controlClass;
        },

        registerDatatype: function (type, value) {
            this._datatype[type] = value;
        },

        validMethods: {},

        _datatype: {
            none: null,

            string: "0",

            bool: "4",

            int: "1",

            decimal: "2",

            datetime: "3"
        },

        _sysControltype: {},

        init: function (formName) {

            $('.tooltip-link').yardiTip({ offsetX: -20 });

            var form = $('#Column' + formName + '-form').find('form'),

             nameInput = form.find('input[name$=Name]').keyup(function () {
                 setTimeout(function () {
                     validName();
                 }, 50);
             }),

             oldVal = nameInput.val(),

             tabs = $("#" + formName + "tabs").koobooTab({
                 showTabIndex: 0,
                 preventDefault: true
             }),

             fromFolderSwitcher = form.find('input:radio[name$=SelectionSourceRadio]');

            fromFolderSwitcher.change(function () {
                handleFromFolderSwitcher(fromFolderSwitcher);
            });


            function handleFromFolderSwitcher(switcher) {
                var effect = 'fade';
                var value = switcher.filter('[checked]').val();

                form.find("input:hidden[name$=SelectionSource]").val(value);

                if (value == '0') {
                    validMethods['validFolderName'] = undefined;
                    form.find('fieldset.from-folder').hide(effect);
                    form.find('fieldset.manual').removeClass('hide').show(effect);
                } else {
                    validMethods['validFolderName'] = validFolderName;
                    form.find('fieldset.manual').hide(effect);
                    form.find('fieldset.from-folder').removeClass('hide').show(effect);
                }
            }

            function validFolderName() {
                var folders = form.find('fieldset.from-folder').find('input:checkbox');

                var selectedFolder = form.find('fieldset.from-folder').find('input:checkbox[checked]');
                if (selectedFolder.length == 0) {
                    folders.addClass('input-validation-error');
                    kooboo.cms.ui.messageBox().show('<%:"Please select a folder.".Localize() %>', 'error');
                    return false;
                } else {
                    folders.removeClass('input-validation-error');
                }
            }

            var validMethods = {};

            function validName() {

                var inputVal = nameInput.val();
                var schema = kooboo.data('schema-info');

                var nameExist = false;
                var columns = schema.Columns.each(function (value, index) {
                    nameExist = nameExist || (value.Name == inputVal);
                    if (nameExist) {
                        return false;
                    }
                });

                var result = true;

                if (nameExist) {
                    if (inputVal == oldVal) {
                        result = true;
                    } else {
                        result = false;
                    }
                }

                if (result) {
                    nameInput.removeClass('input-validation-error').next().addClass('field-validation-valid');
                } else {
                    nameInput.addClass('input-validation-error').next().show().html('<span><%:"This name is being used by other column.".Localize() %></span>').addClass('field-validation-error').removeClass('field-validation-valid');
                }

                return result;
            }

            validMethods.validName = validName;
            form.unbind("submit").submit(function () {
                if (form.valid()) {
                    var r = true;
                    for (var m in validMethods) {
                        if (typeof (validMethods[m]) == 'function') {
                            r = r && (validMethods[m](form) != false);
                        }
                    }
                    if (!r) {
                        return false;
                    }

                    var handle = form;

                    var tempForm = $('<form></form>');

                    var selector = "div.ColumnForm:not(.Column)";

                    if (handle.parent().hasClass('Column')) {
                        selector = "div.ColumnForm";
                    }

                    kooboo.cms.ui.formHelper.clearHidden(tempForm).copyForm(selector, tempForm);

                    tempForm.ajaxSubmit({
                        url: handle.attr('action'),
                        type: 'post',
                        beforeSend: function () {
                            kooboo.cms.ui.loading().show();
                        },
                        success: function (html) {
                            kooboo.cms.ui.status().stop();
                            if ($.popContext.getCurrent() != null) {
                                $.popContext.getCurrent().close();
                                var gridHolder = $("#grid-field-template");
                                var formHolder = $("#form-fields");

                                gridHolder.remove();
                                formHolder.remove();

                                var oldPops = $('div.edit-column-form');
                                oldPops.remove();

                                $("body").append(html);

                                gridHandle();

                                if ($.validator)
                                    $.validator.unobtrusive.parse(document);
                                kooboo.cms.ui.messageBox().hide();
                            }

                        }
                    });
                } else {
                    if (form.find('.tabs').length) {
                        $('input.input-validation-error,select.input-validation-error')
							.parents('div.tab-content').each(function () {
							    var tab = $(this);
							    var li = $('a[href="#' + tab.attr('id') + '"]').show('pulsate', {}, 200);
							})
                    }
                }
                return false;
            });

            var datatypeDropdown = form.find("[id$=DataType]"),

            controltypeDropdowm = form.find("[id$=ControlType]"),

            datatype = this._datatype,

            action = {
                hideLengthSetting: function () {
                    form.find("[id$=Length]").parents('tr').hide();
                },

                showLengthSetting: function () {
                    if (!form.find("[id$=Length]").val())
                        form.find("[id$=Length]").parents('tr').show();
                },

                setDefaultLength: function () {
                    if (!form.find("[id$=Length]").val())
                        form.find("[id$=Length]").val(256);

                },

                setLargeLength: function () {
                    if (!form.find("[id$=Length]").val())
                        form.find("[id$=Length]").val(0);

                },

                setShortLength: function () {
                    if (!form.find("[id$=Length]").val())
                        form.find("[id$=Length]").val(16);

                },

                showSelectListItemTab: function () {

                    form.find("[id$=selectItemListTab]").removeClass("hide");

                },

                hideSelectListItemTab: function () {
                    form.find("[id$=selectItemListTab]").addClass("hide");
                }

            },

            sysControltype = $.extend({

                File: {
                    datatype: datatype.string,
                    action: function () {
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                TextArea: {
                    datatype: datatype.string,
                    action: function () {
                        action.setLargeLength();
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                CheckBox: {
                    datatype: datatype.bool,
                    action: function () {
                        action.hideLengthSetting(); datatypeDropdown.parents('tr').hide();
                    }
                },

                DropDownList: {
                    action: function () {
                        action.showSelectListItemTab();
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                RadioList: {
                    action: function () {
                        action.showSelectListItemTab();
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                CheckBoxList: {
                    action: function () {
                        action.showSelectListItemTab();
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                DateTime: {
                    datatype: datatype.datetime,
                    action: function () {
                        action.hideLengthSetting(); datatypeDropdown.parents('tr').hide();
                    }
                },

                CLEditor: {
                    datatype: datatype.string,
                    action: function () {
                        action.setLargeLength();
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                Tinymce: {
                    datatype: datatype.string,
                    action: function () {
                        action.setLargeLength();
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                Date: {
                    datatype: datatype.datetime,
                    action: function () {
                        action.hideLengthSetting(); datatypeDropdown.parents('tr').hide();
                    }
                },

                Number: {
                    datatype: datatype.decimal,
                    action: function () {
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                Int32: {
                    datatype: datatype.int,
                    action: function () {
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                Float: {
                    datatype: datatype.decimal,
                    action: function () {
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                },

                MultiFiles: {
                    datatype: datatype.string,
                    action: function () {
                        action.hideLengthSetting();
                        datatypeDropdown.parents('tr').hide();
                    }
                }

            }, this._sysControltype),

            selectedDataType = datatypeDropdown.val();

            controltypeDropdowm.change(function () {
                initControltypeDropdown();
            });

            initControltypeDropdown();

            kooboo.data('init-control-type', initControltypeDropdown);

            kooboo.data('show-datatype-tr', function () { datatypeDropdown.parents('tr').show(); });

            function initControltypeDropdown() {
                datatypeDropdown.parents('tr').show();
                var val = controltypeDropdowm.val();

                action.hideSelectListItemTab();

                action.setDefaultLength();

                datatypeDropdown.find("option").show();

                action.showLengthSetting();

                datatypeDropdown.val(selectedDataType || datatype.string);

                var allowDatatype = sysControltype[val];

                if (allowDatatype) {

                    if (allowDatatype.action) {
                        allowDatatype.action(form);
                    }

                    if (allowDatatype.datatype) {

                        var hideOptionSelector = '';

                        var showOptionSelector = '';

                        for (var p in datatype) {
                            if (typeof datatype[p] == 'string') {
                                hideOptionSelector += "option[value=" + datatype[p] + "] ,";

                                if (allowDatatype.datatype instanceof Array) {
                                    if (allowDatatype.datatype.length < 2) {

                                    } else {
                                        datatypeDropdown.parents('tr').show();
                                    }
                                } else {

                                    if (datatype[p] == allowDatatype.datatype) {

                                        datatypeDropdown.val(datatype[p]);
                                        showOptionSelector += "option[value=" + datatype[p] + "] ,";
                                    }
                                }
                            }

                        }

                        datatypeDropdown.find(hideOptionSelector).hide();

                        datatypeDropdown.find(showOptionSelector).show();
                    }
                }
            } // end initControlTypeDown
        }

    });

    $(function () {
        kooboo.cms.ui.status().bind(window, '<%:"Are you sure you want to leave current page".Localize() %>');



        var newColumnFormId = $('div.Column.ColumnForm').attr('id');
        $("#btn_CreateColumn").unbind('click').pop({
            useContent: true,
            contentId: newColumnFormId,
            onload: function (handle, pop, config) {
                var form = pop.find('form');

                form.find('div[koobooTab]').data("koobooTab").showTab(0);

                try {
                    form.get(0).reset();
                    kooboo.data('init-control-type')();
                    pop.find("[id$=DataType]").parents('tr:eq(0)').show();
                }
                catch (E) { }

                form.find('.datafield').remove();

                var orderInput = form.find('input[name$=Order]');
                var schema = kooboo.data('schema-info');
                var order = 1;

                schema.Columns.each(function (value, index) {
                    if (value.Order >= order) {
                        order = value.Order + 1;
                    };
                });

                orderInput.val(order);

                form.find('input:checkbox[name$=Summarize]').attr('checked', schema.Columns.length <= 0);
                form.find('input:checkbox[name$=ShowInGrid]').attr('checked', schema.Columns.length <= 0);
            }
        });

        $("#finishBtn").click(function () {
            $("#Finish").val(true);
            $("#Next").val(false);
        });

        $("#schemaForm").find('input').keydown(function (e) {
            if (e.keyCode == 13) {
                return false;
            }
        });
        kooboo.cms.ui.event.ajaxSubmit(function () {
            if (!this.is('#schemaForm')) {
                return true;
            }
            var handle = this;
            if (handle.valid()) {
                kooboo.cms.ui.status().pass();
                if ($('#TemplateBuildByMachine').val() && ($('#TemplateBuildByMachine').val().toLowerCase() == 'false')) {
                    kooboo.confirm('<%:"Some Templates has been edited manually! Do you want to reset these templates?" %>', function (result) {
                        if (result) {
                            $("#ResetAll").val(true);
                        } else {
                            $("#ResetAll").val(false);
                        }
                    });
                }

                var selector = 'div.ColumnForm:not(.Column)';

                kooboo.cms.ui.formHelper.clearHidden(handle).copyForm(selector, handle);

            } else {
                return false;
            }
        }); // second param you can get in your bind method by this.data
    });

    function gridHandle() {
        $('.table-container table').tableFlex({
            hiddenTd: [2, 4, 5, 6, 7, 9, 10, 12]
        });

        var tbody = $('.table-container table tbody');

        tbody.sortable({
            revert: true,
            cancel: 'tr.folderTr',
            start: function (event, ui) {
                ui.placeholder.html('<td colspan="100"></td>');
            },
            change: function (event, ui) {
            },
            stop: function () {
                adjustOrder();
            },
            placeholder: "ui-state-highlight holder",
            cursor: 'move',
            helper: 'clone'
        });

        adjustOrder();

        function adjustOrder() {
            tbody.children(':not(.scroll-bar)').each(function (index) {
                var tr = $(this).removeClass('even');
                if (index % 2 != 0) {
                    tr.addClass('even');
                }
                tr.find('td:eq(0)').css({ cursor: 'move' });
                tr.find('td:eq(11)').html(index + 1);
                var formId = tr.find("a.dialog-link-column").attr('idValue') + '-detail-form';
                $('#' + formId).find('input[name$=Order]').val(index + 1);
            });
        }

        $("#grid-field-template").appendTo("#grid-field");

        $("a.dialog-link-column").unbind('click').pop({
            useContent: true,
            contentId: function (handle, pop, config) { return handle.attr('idValue') + '-detail-form'; },
            width: 780,
            height: 560,
            frameHeight: "100%",
            onload: function (handle, pop, config) {
                pop.find('input:visible').first().focus();
            }
        });

        $('a.action-deletecolumn').unbind('click').click(function () {

            var handle = $(this);
            var idValue = handle.attr('idValue');

            kooboo.confirm(handle.attr('confirm'), function (r) {
                if (r) {
                    handle.parents('tr').fadeOut(function () {
                        var columnName = $.request(handle.attr('href')).queryString['columnName'];

                        var schemaInfo = kooboo.data('schema-info');
                        schemaInfo.Columns = schemaInfo.Columns.where(function (o) { return o.Name != columnName; });

                        $('div.edit-column-form.' + idValue).remove();
                        handle.remove();
                        kooboo.cms.ui.status().stop();
                    });
                }
            });
            return false;
        });

        if ($.validator) {
            $.validator.unobtrusive.parse(document);
        }
    }
</script>
<style type="text/css">
    .holder {
        width: 100%;
        height: 30px;
    }
</style>
