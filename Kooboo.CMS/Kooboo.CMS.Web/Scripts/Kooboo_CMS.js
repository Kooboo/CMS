//use in sortable
var fixHelper = function (e, ui) {
    ui.children().each(function () {
        $(this).width($(this).width());
    });
    return ui;
};

function parse_JsonResultData(response, statusText, xhr, $form) {
    var form = $form;
    if (response == undefined) {
        return false;
    }
    var responseData = response;
    if (typeof (response) == 'string') {
        try {
            responseData = $.parseJSON(response);
        } catch (e) {
            responseData = $.parseJSON($(response).text());
        }

    }

    if (form) {
        form.find("[type=submit]").removeClass("disabled").removeAttr("disabled");
    }
    if (responseData.Messages.length > 0) {
        var str = responseData.Messages.join("<br />");
        window.info.show(str, responseData.Success);
    }

    if (!responseData.Success) {
        if (form) {
            var validator = form.validate();
            //                            var errors = [];
            for (var i = 0; i < responseData.FieldErrors.length; i++) {
                var obj = {};
                obj[responseData.FieldErrors[i].FieldName] = responseData.FieldErrors[i].ErrorMessage;
                validator.showErrors(obj);
            }
        }
    }
    else {
        window.leaveConfirm.pass();
        if (responseData.ClosePopup) {
            if ($.popContext.getCurrent() != null) {
                $.popContext.getCurrent().close();
            }
        }
        var topJQ = top._jQuery || top.jQuery;
        if (responseData.OpenUrl) {
            window.open(responseData.OpenUrl);
        }
        if (responseData.RedirectUrl) {
            if (responseData.RedirectToOpener == true) {
                top.location.href = responseData.RedirectUrl.replace("&amp;", "&");
            }
            else {
                location.href = responseData.RedirectUrl.replace("&amp;", "&");
            }
        } else {
            //why:当出现错误的时候，刷新会让错误信息马上消失掉。(Module/Install)
            if (responseData.Messages.length == 0 && responseData.ReloadPage == true) {
                if (responseData.RedirectToOpener == true) {
                    top.location.reload(true);
                }
                else {
                    location.reload(true);
                }
            }
        }
    }
};


(function ($) {
    $.fn.dialogLink = function () {
        return this.find('a.dialog-link').one('click', function (e) {
            e.preventDefault();
            // show dialog
            var id = new Date().getTime();
            var handle = $(this).pop({
                id: id,
                //width: 800,
                //height: 580,
                frameHeight: "100%",
                popupOnTop: true,
                onclose: function () {
                }
            }).click();

        });
    };
    $.fn.dropdownMenu = function () {
        var dom = $(this);
        var dropdown = dom.find('.j_DropDown');
        $(document).click(function () {
            dropdown.removeClass('active');
            dropdown.children('.j_DropDownContent').slideUp('fast');
        });
        return dropdown.bind('click', function (e) {
            e.stopPropagation();
            var o = $(this);
            var menu = o.children('.j_DropDownContent');
            if (o.hasClass('active')) {
                o.removeClass('active');
                menu.slideUp('fast');
            } else {
                o.addClass('active');
                menu.slideDown('fast');
            }
        });
    };
    $.fn.dropdownButton = function () {
        var dom = $(this);
        var dropdown = dom.find('.dropdown-button');
        $(document).click(function () {
            dropdown.removeClass('active');
            dropdown.children('ul').slideUp('fast');
        });
        return dropdown.bind('click', function (e) {
            e.stopPropagation();
            var o = $(this);
            var menu = o.children('ul');
            if (o.hasClass('active')) {
                o.removeClass('active');
                menu.slideUp('fast');
            } else {
                o.addClass('active');
                menu.slideDown('fast');
            }
        });
    };
    $.fn.linkPost = function () {
        return this.find('a[data-ajax]').click(function (e) {
            e.preventDefault();
            var $self = $(this), url = $self.attr('href'), post = function () {
                var type = $self.data('ajax');
                if (type.toLowerCase() == "download") {
                    $.fileDownload(url, {
                        httpMethod: "POST"
                    });
                } else {
                    $.ajax({
                        url: url,
                        success: function (response, statusText, xhr) {
                            parse_JsonResultData(response, statusText, xhr);
                        },
                        type: type
                    });
                }
            };
            if ($self.data('confirm')) {
                if (confirm($self.data('confirm'))) {
                    post();
                }
            } else {
                post();
            }
        });
    };
    $.fn.clickableLegend = function () {
        function toggleLegend(legend) {
            var handle = legend, next = handle.next();
            if (handle.hasClass('active')) {
                handle.removeClass('active');
                next.fadeOut();
            } else {
                handle.addClass('active');
                next.fadeIn();
            }
        }
        var legend = this.find('legend.clickable:not(.no-bind)').click(function () {
            toggleLegend($(this));
        }).next().hide();

        toggleLegend(legend);
    };
    $.fn.koobooTab = function (option) {
        var api = this.data('koobooTab');
        if (api == undefined) {
            var config = {
                containerClass: "tab-content",
                currentClass: "active",
                tabClass: "tab-content",
                event: "click",
                tabContentTag: "<div></div>",
                showTabIndex: 0,
                preventDefault: true
            };

            var tabContents = this.children('.' + config.tabClass).hide();
            var tabMap = [];
            $.extend(config, option);
            var ul = this.children("ul");

            function initTab(current, index) {
                var href = current.attr("href");
                if (href.indexOf("#") == 0) {
                    current.bind(config.event, function (e) {
                        document.location.hash = href.substring(href.indexOf('#'));
                        api.show();
                        if (config.preventDefault) {
                            e.preventDefault();
                        }
                    });
                }

                var api = {
                    show: function () {
                        var expire = /#\S+$/;
                        var id = expire.exec(href);
                        if (id && id[0]) {
                            tabContents.hide();
                            current.parent().siblings().removeClass(config.currentClass);
                            current.parent().addClass(config.currentClass);
                            $(id[0]).show();
                        }

                    }
                };
                tabMap.push({ id: href, handle: current, tab: api, index: index });
                return api;
            }

            var index = 0;
            ul.find("li a").each(function (val) {
                var current = $(this);
                var tabApi = initTab(current, index);
                index++;
            });



            api = {
                showTab: function (tab) {
                    tab = tab == undefined ? 0 : tab;
                    var tabObj;
                    if (typeof (tab) == 'string') {
                        tabObj = _.find(tabMap, (function (o, index) {
                            return o.id == tab;
                        }));
                    } else if (!isNaN(tab)) {
                        tabObj = _.find(tabMap, (function (o, index) { return index == tab; }));
                    }
                    tabObj = tabObj ? tabObj : _.first(tabMap);
                    tabObj.tab.show();
                }
            };

            this.data('koobooTab', api);

            function getAddressTab() {
                var url = location.href;
                var expire = /#\w+$/;
                var tab = config.showTabIndex;

                if (expire.test(url)) {
                    var tabId = expire.exec(url);
                    tab = tabId[0];
                }
                return tab;
            }

            $(function () {
                api.showTab(getAddressTab());

                $(window).bind('onhashchange', function () {
                    api.showTab(getAddressTab());
                });

            });
        }

        return api;
    }
    $.fn.checkableTable = function () {
        var $check_relateds = $('[data-show-on-check]');
        var table = $(this);

        table.find('td:not(.checkbox)').children().click(function (e) {
            e.stopPropagation();
        });

        (function sync_header_width() {
            var $thead = table.find('thead');
            var $tbody = table.find('tbody');
            var $th = $thead.first().find('th').toArray();
            var $td = $tbody.find('tr:last-child td');
            if ($th.length == $td.length) {
                _.each($td, function (td, index) {
                    if (index < $th.length) {
                        $($th[index]).css('width', $(td).css('width'));
                    }
                });
            }
        }());

        // var all_checkboxes = table.find("input:checkbox.select");
        var $selectAll = table.find("input:checkbox.select-all");

        function reset_check_relateds() {
            var $all_checkeds = table.find('input:checkbox.select:checked');
            $check_relateds.hide();
            $check_relateds.each(function () {
                var $related = $(this);
                var show_on_check = $related.data('show-on-check');
                switch (show_on_check) {
                    case 'Single':
                        if ($all_checkeds.length == 1) {
                            $related.show();
                        }
                        break;
                    case 'Two':
                        if ($all_checkeds.length == 2) {
                            $related.show();
                        }
                        break;
                    case 'Any':
                        if ($all_checkeds.length > 0) {
                            $related.show();
                        }
                        break;
                }
                var show_on_selector = $related.data('show-on-selector');
                if (show_on_selector) {
                    if ($all_checkeds.closest('tr:not(' + show_on_selector + ')').length > 0) {
                        $related.hide();
                    }
                }
            });
        }
        reset_check_relateds();

        $selectAll.change(function () {
            if ($(this).attr("checked")) {
                table.find("input:checkbox.select:not(:disabled)").attr("checked", true).parents('tr').addClass('active');
            } else {
                table.find("input:checkbox.select").attr("checked", false).parents('tr').removeClass('active');
            }
            reset_check_relateds();
        });

        var selectOptional = function () {
            $('th.checkbox.mutiple div').click(function (e) {
                e.stopPropagation();
                $(this).children('ul').toggleClass("hide");
            }).find("input:checkbox").click(function (e) {
                e.stopPropagation();
            });

            var optionUl = table.find('th.checkbox.mutiple div ul');
            optionUl.find("li.none").click(function () {
                $("input:checkbox").attr("checked", false);
                $("input:checkbox").parents('tbody tr').removeClass('active');
                reset_check_relateds();
            });
            optionUl.find("li.all").click(function () {
                $("input:checkbox").attr("checked", true);
                $("input:checkbox").parents('tbody tr').addClass('active');
                reset_check_relateds();
            });
            optionUl.find("li.docs").click(function () {
                $("input:checkbox").attr("checked", false);
                $("input:checkbox").parents('tbody tr').removeClass('active');
                $("input:checkbox.doc").attr("checked", true);
                $("input:checkbox.doc").parents('tbody tr').addClass('active');
                reset_check_relateds();
            });
            optionUl.find("li.folders").click(function () {
                $("input:checkbox").attr("checked", false);
                $("input:checkbox").parents('tbody tr').removeClass('active');
                $("input:checkbox.folder").attr("checked", true);
                $("input:checkbox.folder").parents('tbody tr').addClass('active');
                reset_check_relateds();
            });
        };
        selectOptional();

        table.find('tbody tr').checkableTR();

        return table;
    };
    $.fn.checkableTR = function () {
        var $tr = $(this);
        var $check_relateds = $('[data-show-on-check]');
        function reset_check_relateds() {
            var $all_checkeds = $tr.closest('tbody').find('input:checkbox.select:checked');
            $check_relateds.hide();
            $check_relateds.each(function () {
                var $related = $(this);
                var show_on_check = $related.data('show-on-check');
                switch (show_on_check) {
                    case 'Single':
                        if ($all_checkeds.length == 1) {
                            $related.show();
                        }
                        break;
                    case 'Two':
                        if ($all_checkeds.length == 2) {
                            $related.show();
                        }
                        break;
                    case 'Any':
                        if ($all_checkeds.length > 0) {
                            $related.show();
                        }
                        break;
                }
                var show_on_selector = $related.data('show-on-selector');
                if (show_on_selector) {
                    if ($all_checkeds.closest('tr:not(' + show_on_selector + ')').length > 0) {
                        $related.hide();
                    }
                }
            });
        }
        $tr.click(function () {
            var $self = $(this);
            var $checkbox = $self.find('input:checkbox');
            if ($checkbox.attr('disabled') != 'disabled') {
                $self.toggleClass('active');
                if ($self.hasClass('active')) {
                    $checkbox.attr('checked', true);
                } else {
                    $checkbox.removeAttr('checked');
                }
                reset_check_relateds();
            }
        });
    }
    $.fn.grid = function () {
        var $table = $(this);
        var getSelecteds = function () {
            var selectedData = {};
            var selected = $table.find("input:checkbox[name=select][checked]");

            selected.each(function (i) {
                var current = $(this);
                var p = 'model[' + i + ']' + '.' + current.data("id-property");
                selectedData[p] = current.val();
            });
            return selectedData;
        }
        var doPost = function (url, data, success, confirmMsg) {
            var postData = getSelecteds();
            $.extend(postData, data);

            if (!_.isEmpty(postData)) {
                if (confirmMsg != undefined && confirmMsg != '') {
                    doit = confirm(confirmMsg);
                    if (!doit) {
                        return;
                    }
                }
                window.loading.show();
                $.post(url, postData, function (data, textStatus, jqXHR) { success(data, textStatus, jqXHR, postData); }, "json");
            }
        }

        window.grid = { doPost: doPost, getSelecteds: getSelecteds };

        $('[data-command-type="AjaxPost"]').click(function (e) {
            e.preventDefault();
            var $button = $(this);
            var message = $button.data("confirm");
            grid.doPost($button.attr("href"), null, function (data, textStatus, jqXHR, postData) {
                parse_JsonResultData(data, textStatus, jqXHR);
            }, message);
        });

        $('[data-command-type="Download"]').click(function (e) {
            var selectedModel = grid.getSelecteds();

            $.fileDownload($(this).attr("href"), {
                httpMethod: "POST",
                data: selectedModel
            });
            return false;
        });

        $('[data-command-type="Redirect"]').click(function () {
            var $self = $(this);
            var $selected = $table.find("input:checkbox[name=select][checked]");
            var id = $selected.data("id-property");
            var selectedValues = [];
            $selected.each(function () {
                selectedValues.push($(this).val());
            });
            var value = selectedValues.join(',');
            window.location.href = ($self.attr('href') + "&" + id + "=" + value);
            return false;
        });
    };
    $.fn.mixedGrid = function () {
        var $table = $(this);
        var getSelectedDocs = function () {
            var files = {};
            var selected = $table.find("input.doc:checkbox[checked]");

            selected.each(function (i) {
                var $self = $(this);
                files[i] = ($self.val());
            });
            return files;
        }
        var getSelectedFolders = function () {
            var folders = {};
            var selected = $table.find("input.folder:checkbox[checked]");

            selected.each(function (i) {
                var $self = $(this);
                folders[i] = ($self.val());
            });
            return folders;
        }
        var doPost = function (url, data, success, confirmMsg) {
            var postData = { folders: getSelectedFolders(), docs: getSelectedDocs() };
            $.extend(postData, data);

            if (!_.isEmpty(postData)) {
                if (confirmMsg != undefined && confirmMsg != '') {
                    doit = confirm(confirmMsg);
                    if (!doit) {
                        return;
                    }
                }
                window.loading.show();
                $.post(url, postData, function (data, textStatus, jqXHR) { success(data, textStatus, jqXHR, postData); }, "json");
            }
        }

        window.grid = { doPost: doPost, getSelectedDocs: getSelectedDocs, getSelectedFolders: getSelectedFolders };

        $('[data-command-type="AjaxPost"]').click(function (e) {
            e.preventDefault();
            var $button = $(this);
            var message = $button.data("confirm");
            grid.doPost($button.attr("href"), null, function (data, textStatus, jqXHR, postData) {
                parse_JsonResultData(data, textStatus, jqXHR);
            }, message);
        });
        $('[data-command-type="Download"]').click(function () {
            var selectedModel = { folders: grid.getSelectedFolders(), docs: grid.getSelectedDocs() };
            $.fileDownload($(this).attr("href"), { httpMethod: "POST", data: selectedModel })
            return false;
        });

        $('[data-command-type="Redirect"]').click(function () {
            var $self = $(this);
            var $selected = $table.find("input:checkbox[checked]");
            var id = $selected.data("id-property");
            var selectedValues = [];
            $selected.each(function () {
                selectedValues.push($(this).val());
            });
            var value = selectedValues.join(',');
            window.location.href = ($self.attr('href') + "&" + id + "=" + value);
            return false;
        });
    };
    $.fn.treeNode = function () {
        var handler = $(this);
        handler.find('.tree-icon').click(function () {
            $(this).siblings('ul').toggle('fast').parent().toggleClass('active');
        });
    };
    $.fn.mapItem = function () {
        var dom = $(this);
        var mapItemArrow = dom.find('.map-item .arrow');
        $(document).click(function () {
            mapItemArrow.siblings('ul').hide('fast');
        });
        return mapItemArrow.bind('click', function (e) {
            e.stopPropagation();
            var menu = $(this).siblings('ul');
            $('.map-item > ul:visible').not(menu).hide('fast');
            menu.toggle('fast');
        });
    };
    $.fn.siteSwitch = function () {
        var dom = $(this);
        var switcher = dom.find('.block.sitemanager .switcher.active');
        $(document).click(function () {
            switcher.children('ul:visible').hide('fast');
        });
        return switcher.bind('click', function (e) {
            e.stopPropagation();
            $(this).children('ul').toggle('fast');
        });
    };
    $.fn.gridInlineEdit = function () {
        var $self = $(this);
        var $inlineActionButton = $self.find('[data-inline-action]');
        var $tr_hover = $self.find('[data-tr-hover]');
        $tr_hover.each(function () {
            var $button = $(this);
            $button.closest('tr').hover(function () {
                $button.toggleClass('hide');
            });
        });
        $inlineActionButton.click(function (e) {
            e.preventDefault();
            var $button = $(this);
            var template = $button.data('inline-action');
            if (template != '') {
                //{show:'id',hide:'id'}
                if (template.show) {
                    $(template.show).removeClass('hide');
                }
                if (template.hide) {
                    $(template.hide).addClass('hide');
                }
            }

        });
    };
    $.fn.disableSubmit = function () {
        $(this).keypress(function (e) {
            if (e.which == 13 && e.target.nodeName != "TEXTAREA") return false;
        })
    };
    $.fn.checkableMetro = function () {
        var grid = $(this);
        var $check_relateds = $('[data-show-on-check]');
        $check_relateds.hide();
        function reset_check_relateds() {
            var $all_checkeds = grid.find('input:checkbox.select:checked');
            if ($all_checkeds.length > 0) {
                grid.addClass('active');
            }
            else {
                grid.removeClass('active');
            }
            $check_relateds.hide();
            $check_relateds.each(function () {
                var $related = $(this);
                var show_on_check = $related.data('show-on-check');
                switch (show_on_check) {
                    case 'Single':
                        if ($all_checkeds.length == 1) {
                            $related.show();
                        }
                        break;
                    case 'Two':
                        if ($all_checkeds.length == 2) {
                            $related.show();
                        }
                        break;
                    case 'Any':
                        if ($all_checkeds.length > 0) {
                            $related.show();
                        }
                        break;
                }
                var show_on_selector = $related.data('show-on-selector');
                if (show_on_selector) {
                    if ($all_checkeds.closest('tr:not(' + show_on_selector + ')').length > 0) {
                        $related.hide();
                    }
                }
            });
        }
        grid.find('input:checkbox[name="select"]').click(function (e) {
            e.stopPropagation();
            $(this).parents('li').toggleClass('active');
            reset_check_relateds();
        });
    };
    (function popup_ext() {
        ///-------------jQuery.fn.pop------------
        var __apiMap = {};
        var __configMap = {};
        var __divStr = "<div></div>";
        var __iframe = '<iframe  frameborder="0" allowTransparency="true"/>';

        $.fn.pop = function (option) {
            this.each(function () {
                var handle = $(this), cfg = $.extend({}, option);
                if (handle.attr('pop-config')) {
                    var popConfig = eval('(' + handle.attr('pop-config') + ')');
                    $.extend(cfg, popConfig);
                }
                koobooPop.call(handle, option);
            });
            return this;
        }

        var koobooPop = function (option) {
            var config = {
                draggable: false,
                resizable: false,
                id: '',
                url: '', // url will being getted from href prior
                title: '', // you dont need to config this if you link element has title attribute
                isIframe: true,
                useUrl: true, // 
                useContent: false,
                contentId: '', // may be a function like function(handle,pop,config){  return contentID ;};
                useClone: false, //this will clone pop content every time

                events: 'click',

                maxAble: true,

                cloneId: 'pop-clone-' + Math.random().toString().replace('.', '-'),

                reload: true, // reload content every time when open

                popupOnTop: false, // show pop on parent page

                openOnclick: true,

                allowTransparency: true,
                frameborder: '0',
                border: '0',

                frameWidth: '100%',
                frameHeight: '100%',
                unbindEvents: 'click',

                show: 'fade', //jquery dialog option
                autoOpen: false, //jquery dialog option
                modal: true, //jquery dialog option
                closeOnEscape: false, //close on esc

                onopen: function (currentHandle, pop, config) { },
                beforeLoad: function (currentHandle, pop, config) { }, // before pop content loaded
                onload: function (currentHandle, pop, config) { }, // on pop content loaded 
                onclose: function (currentHandle, pop, config) { } // on pop closed
            };

            if (_.isString($(option))) {
                config.id = option;
            } else if (_.isObject($(option))) {
                $.extend(config, option);
            }

            config.id = (config.id != '' ? config.id : Math.random().toString().replace('.', '_'));
            if (__apiMap[config.id]) {
                return __apiMap[config.id];
            }

            var current = this;
            current.data("POPHANDLE", true);
            var pop = $(__divStr);
            config.close = function () {

                config.onclose(current, pop, config);
                pop.find('iframe').attr('src', 'about:blank');
                $.popContext.popList.pop();
            }
            var topJQ = top._jQuery || top.jQuery;
            if (config.popupOnTop && topJQ) {
                pop = topJQ(__divStr).appendTo(top.document.getElementsByTagName("body").item(0));
            } else {
                pop.appendTo("body");
            }

            pop.attr('id', config.id);
            pop.hide();

            if (this.attr('title') && this.attr('title').length > 0) {
                config.title = this.attr('title');
            }
            pop.dialog(config);
            //if (config.maxAble) {
            //    pop.dialogExtend({
            //        "maximize": true,
            //        "dblclick": "maximize",
            //        "icons": {
            //            "maximize": "ui-icon-extlink",
            //            "restore": "ui-icon-newwin"
            //        },
            //        events: {
            //            restore: function (evt, dlg) {
            //                $(dlg).dialog("option", "position", 'center');
            //            }
            //        }
            //    });
            //}
            if (config.openOnclick) {
                function showPop() {
                    show();
                    $.popContext.popList.push(pop);
                    pop.dialog('open').dialog("option", "position", 'center');
                }
                this.unbind(config.unbindEvents).keydown(function () {
                    showPop();
                    return false;
                }).mousedown(function (e) {
                    if (e.which != 3) {
                        showPop()
                    }
                    return false;
                })
            .bind("contextmenu", function () { return false; }).click(function () {
                showPop();
                return false;
            });
            } else {
                show();
                $.popContext.popList.push(pop);
            }
            function loadContent() {
                config.beforeLoad(current, pop, config);
                if (current.attr('href')) {
                    config.url = current.attr('href');
                }

                current.data('loaded', true);
                if (config.url == '') {
                    if (current.attr('url') && current.attr('url').length > 0) {
                        config.url = current.attr('url');
                    } else if (current.attr('href') && current.attr('href').length > 0) {
                        config.url = current.attr('href');
                    } else if (current.attr('src') && current.attr('src').length > 0) {
                        config.url = current.attr('src');
                    }
                }

                if (config.url != '' && config.useUrl && !config.useContent && !config.useText) {
                    if (!config.isIframe) {
                        $.ajax({
                            url: config.url,
                            success: function (data) {
                                pop.html(data);
                                config.onload(current, pop, config);
                                config.onopen(current, pop, config);
                            },
                            error: function () {
                                pop.html('some errors occur during request!');
                            }
                        });
                    } else {
                        var iframe = $();

                        if (pop.find('iframe').length == 0) {
                            iframe = $(__iframe).appendTo(pop);
                            iframe.css('width', config.frameWidth);
                            iframe.attr('frameborder', config.frameborder);
                            iframe.attr('allowTransparency', config.allowTransparency);

                            iframe.attr('border', config.border);
                            iframe.attr('src', config.url);
                            iframe.attr('id', "pop_iframe" + config.id);

                            if (config.frameConfig) {
                                iframe.attr(config.frameConfig);
                            }

                            if (config.height <= 200) {

                                iframe.css("height", "90%");
                            }

                            if (config.frameHeight == 'auto') {
                                var loadCount = 0;

                                iframe.load(function () {

                                    var height = iframe.contents().height();
                                    iframe.height(height);
                                    loadCount++;
                                });
                            } else {
                                iframe.load(function () {
                                    iframe.height(config.frameHeight);
                                });
                            }


                            iframe.load(function () {
                                var $fromPop = iframe.contents().find("input:hidden[name=FromPop]");
                                if ($fromPop.length == 0) {
                                    $fromPop = $('<input type="hidden" name="FromPop" value="true"/>').appendTo(iframe.contents().find("form"));
                                }
                                pop.iframe = iframe;
                                config.onload(current, pop, config);
                                config.onopen(current, pop, config);
                            });
                        } else {
                            iframe = pop.children('iframe');
                            iframe.attr('src', config.url);
                            config.onload(current, pop, config);
                            config.onopen(current, pop, config);
                        }
                    }
                } else if (config.useContent) {
                    if (typeof (config.contentId) == 'function') {
                        config.contentId = config.contentId(current, pop, config);
                    }
                    var source = $('#' + config.contentId);
                    if (config.useClone) {
                        source = source.clone();

                        if (typeof (config.cloneId) == 'function') {
                            source.attr('id', config.cloneId(source));
                            source.find('[id]').each(function () {
                                $(this).attr('id', config.cloneId($(this)));
                            });

                        } else {
                            var sid = source.attr('id');
                            source.attr('id', sid + config.cloneId);
                            source.find('[id]').each(function () {
                                var id = $(this).attr('id');
                                $(this).attr('id', id + '-' + config.cloneId);
                            });
                        }
                        source.appendTo(pop).show();
                    } else {
                        if (!current.data('pop-content-loaded')) {
                            source.appendTo(pop).show();
                            current.data('pop-content-loaded', true);
                        }
                    }
                    config.onload(current, pop, config);
                }
            }
            pop.close = close;
            function close() {
                pop.dialog('close');
            }
            function destory() {
                try {
                    CollectGarbage(); //
                } catch (E) {
                }
            }
            pop.destory = function () {
                pop.dialog("destory");
            }
            pop.open = function () {
                pop.dialog('open');
            }
            function show() {

                if (config.reload || !current.data('loaded')) {
                    if (current.attr('href')) {
                        config.url = current.attr('href') || config.url;
                    }
                    loadContent();
                }
            }


            var api = {};
            api.show = show;
            api.reload = loadContent;
            api.close = close;
            api.destory = destory;
            api.getIframe = function () {
                ///return juery object
                return pop.children('iframe');
            }
            api.$ = pop.dialog();
            __apiMap[config.id] = api;
            return api;
        }

        ///TODO new method , the order one is too much complex , i dont like it
        function popDialog(option) {
            if (!(this instanceof popDialog)) {
                return new popDialog(option);
            }

            function init() {

            }

            function create() {

            }

            $.extend(this, {
                close: function () { },
                open: function () { },
                reload: function () { },
                destory: function () { }
            });
        }

        var topJQ = top._jQuery || top.jQuery;
        if (topJQ && topJQ.popContext) {
            $.popContext = topJQ.popContext;
        } else {
            $.popContext = {
                popList: [],
                getCurrent: function () {
                    if (this.popList.length > 0) {
                        return this.popList[this.popList.length - 1];
                    }
                    return null;
                }
            };
        }

        $.pop = function (option) {
            option = option || {};
            option.autoOpen = true;
            var handle = $('<a></a>');
            if (option.url) {
                handle.attr('href', option.url);
            }

            var ret = handle.pop(option);

            setTimeout(function () {
                handle.click();
            }, 10);

            return ret;
        };
        $(function () {
            //regist esc key 
            $(window).keyup(function (e) {
                if (e.keyCode == 27) {
                    if (!$(e.target).is('input,textarea,select')) {
                        var cur = $.popContext.getCurrent();
                        if (cur != null) {
                            cur.close();
                        }
                    }
                }

            });
        });
    })();
    (function jquery_validation_filesize() {
        if ($.validator) {
            $.validator.addMethod('filesize', function (value, element, param) {
                if (element.files && element.files.length > 0) {
                    // param = size (en bytes) 
                    // element = element to validate (<input>)
                    // value = value of the element (file name)
                    return this.optional(element) || (element.files[0].size <= param)
                }
                else {
                    return true;
                }
            });
            $.validator.unobtrusive.adapters.addSingleVal("filesize", "value");
        }
    })();
    $(function () {

        (function loading() {
            var show = function () {
                $(document.body).addClass('loading');
            }
            var hide = function () {
                $(document.body).removeClass('loading');
            }
            window.loading = { show: show, hide: hide };
        })();

        (function info() {
            var $notification = $('.notification');
            var $msgBox = $notification.find('p');
            var $close = $notification.find('a.close');
            var mouse_overing = false;
            var timeoutHide = function () {
                if (!mouse_overing) {
                    hide();
                }
                else {
                    setTimeout(timeoutHide, 5000);
                }
            };
            var hide = function () {
                $notification.fadeOut('normal', function () {
                    $notification.css('right', '-18%').show();
                });
            };
            var show = function (msg, success, timeout) {
                $msgBox.html(msg);
                if (success) {
                    $notification.removeClass('error');
                } else {
                    $notification.addClass('error');
                }
                $notification.animate({ right: 0 }, 'fast');
                setTimeout(timeoutHide, timeout || 3000);
            };
            $close.click(function () {
                hide();
            });
            $notification.mouseover(function () {
                mouse_overing = true;
            });
            $notification.mouseout(function () {
                mouse_overing = false;
            });
            window.info = { show: show, hide: hide };
        })();

        (function leaveConfirm() {
            var $window = $(window);
            var canLeave = true;
            var bind = function (msg) {
                $window.bind('beforeunload', function () {
                    if (canLeave == false) {
                        return msg;
                    }
                });
            }
            var stop = function () {
                canLeave = false;
            }
            var pass = function () {
                canLeave = true;
            }
            window.leaveConfirm = { bind: bind, stop: stop, pass: pass };
        })();
        if (typeof (ko) != 'undefined') {
            ko.bindingHandlers.uniqueId = {
                init: function (element) {
                    element.id = ko.bindingHandlers.uniqueId.prefix + (++ko.bindingHandlers.uniqueId.counter);
                },
                counter: 0,
                prefix: "unique"
            };

            ko.bindingHandlers.uniqueFor = {
                init: function (element, valueAccessor) {
                    var after = ko.bindingHandlers.uniqueId.counter + (ko.utils.unwrapObservable(valueAccessor()) === "after" ? 0 : 1);
                    element.setAttribute("for", ko.bindingHandlers.uniqueId.prefix + after);
                }
            };

        }

        //$.validator.methods.number = function (value, element) {
        //    return value == "" || !isNaN(Globalize.parseFloat(value));
        //}

        //#region make first input focus
        $('input.defaultFocus:visible').first().focus();
        //#endregion


        $('form').disableSubmit();
        $(document).dropdownMenu();
        $(document).dropdownButton();
        $(document).mapItem();
        $(document).siteSwitch();
        $(document).dialogLink();
        $(document).linkPost();
        $(document).clickableLegend();

        //lanauge selection


        //#region sidebar menu
        $('.block.menu a').click(function () {
            $(this).siblings('span.arrow').click();
        });
        $('.block.menu span.arrow').click(function () {
            $(this).parent().toggleClass("active");
        });
        if ($.fn.filestyle) {
            $("input[type=file].filestyle").filestyle();
        }

        window.loading.hide();
        $(window).ready(function () {
            window.loading.hide();
        });
    });

    // form post
    $(function () {
        //check form changed
        setTimeout(function () {
            $('form:not(.no-stop) input,form:not(.no-stop) select:not(.select)').change(function () {
                window.leaveConfirm.stop();
            });
        }, 1000);

        $(document).ajaxStop(function () {
            window.loading.hide();
        });

        $(document).ajaxError(function () {
            window.loading.hide();
        });

        $(".upload-button input:file").change(function () {
            $(this).parent().submit();
        });
        $('[data-ajaxform]').click(function (e) {
            e.preventDefault();
            var ajaxFormParam = {
                async: true,
                beforeSend: function () {
                    window.loading.show();
                },
                success: function (response, statusText, xhr, $form) {
                    parse_JsonResultData.call(this, response, statusText, xhr, $form)
                },
                error: function () {
                    window.loading.hide();
                }
            };


            var $self = $(this);
            var formId = $self.data('ajaxform');
            var form = [];
            if (formId != '') {
                form = $('#' + formId);
            }
            if (form.length == 0) {
                form = $self.closest('form:not(:submit)');
            }
            if (form.length == 0) {
                form = $('form:not(:submit)').closest('form');
            }

            if ($self.data('ajax-async') != undefined) {
                ajaxFormParam.async = $self.data('ajax-async');
            }
            if ($self.attr('href') != undefined && $self.attr('href') != '') {
                ajaxFormParam.url = $self.attr('href');
            }
            if (window.ajaxFormParam) {
                ajaxFormParam = $.extend(ajaxFormParam, window.ajaxFormParam);
            }
            $('.field-validation-error').empty();
            if (!form.valid()) {
                if (form.find('.tabs').length) {
                    var selector = 'input.input-validation-error,select.input-validation-error';
                    $(selector).parents('div.tab-content')
                            .each(function () {
                                var tab = $(this);
                                var li = $('a[href="#' + tab.attr('id') + '"]')
                                .hide().show('pulsate', {}, 100)
                                .show('highlight', {}, 200)
                                .show('pulsate', {}, 300)
                                .show('highlight', {}, 400);
                            });
                }
            }
            else {
                window.leaveConfirm.pass();
                var confirmMsg = $self.data('confirm');
                if (confirmMsg) {
                    if (!confirm(confirmMsg)) {
                        return false;
                    }
                }
                form.ajaxForm(ajaxFormParam);
                form.submit();
            }
        });

        $('[data-download-form]').click(function (e) {
            var $self = $(this);
            var formId = $self.data('download-form');
            var _retrun = $self.data('return');
            var form = [];
            if (formId != '') {
                form = $('#' + formId);
            }
            if (form.length == 0) {
                form = $self.closest('form:not(:submit)');
            }
            if (form.length == 0) {
                form = $('form:not(:submit)').closest('form');
            }

            if ($self.attr('href') != undefined && $self.attr('href') != '') {
                ajaxFormParam.url = $self.attr('href');
            }

            $('.field-validation-error').empty();
            if (!form.valid()) {
                if (form.find('.tabs').length) {
                    var selector = 'input.input-validation-error,select.input-validation-error';
                    $(selector).parents('div.tab-content')
                            .each(function () {
                                var tab = $(this);
                                var li = $('a[href="#' + tab.attr('id') + '"]')
                                .hide().show('pulsate', {}, 100)
                                .show('highlight', {}, 200)
                                .show('pulsate', {}, 300)
                                .show('highlight', {}, 400);
                            });
                }
            }
            else {
                window.leaveConfirm.pass();
                var confirmMsg = $self.data('confirm');
                if (confirmMsg) {
                    if (!confirm(confirmMsg)) {
                        return false;
                    }
                }
                form.submit();
                setTimeout(function () {
                    if (_retrun) {
                        location.href = _retrun;
                    }
                }, 100);
            }
        });
    });
})(jQuery);

