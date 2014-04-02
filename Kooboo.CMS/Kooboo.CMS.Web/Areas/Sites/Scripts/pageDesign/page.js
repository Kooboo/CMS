﻿/*
*
* page
* author: ronglin
* create date: 2010.11.16
*
*/

(function ($) {

    // text resource
    var options = {
        errorMessage: 'network error',
        selectView: 'Please select a view',
        selectFolder: 'Please select a data folder',
        selectModule: 'Please select a module',
        selectHtmlBlock: 'Please select a HTML block'
    };

    // override text resource
    if (window.__pageDesign) { $.extend(options, __pageDesign); }

    var __extend = function (subc, superc, overrides) {
        var F = function () { };
        F.prototype = superc.prototype;
        subc.prototype = new F();
        subc.prototype.constructor = subc;
        subc.superclass = superc.prototype;
        if (superc.prototype.constructor == Object.prototype.constructor) {
            superc.prototype.constructor = superc;
        }
        if (overrides) {
            for (var i in overrides) {
                subc.prototype[i] = overrides[i];
            }
        }
    };

    var __hasScroll = function (el) {
        // test targets
        var elems = el ? [el] : [document.documentElement, document.body];
        var scrollX = false, scrollY = false;
        for (var i = 0; i < elems.length; i++) {
            var o = elems[i];
            // test horizontal
            var sl = o.scrollLeft;
            o.scrollLeft += (sl > 0) ? -1 : 1;
            o.scrollLeft !== sl && (scrollX = scrollX || true);
            o.scrollLeft = sl;
            // test vertical
            var st = o.scrollTop;
            o.scrollTop += (st > 0) ? -1 : 1;
            o.scrollTop !== st && (scrollY = scrollY || true);
            o.scrollTop = st;
        }
        // ret
        return {
            scrollX: scrollX,
            scrollY: scrollY
        };
    };

    var __scrollToView = function (el, complete) {
        if (!el) { return; }
        var scroller, parent = el.parentNode;
        while (parent && parent != document) {
            var ret = __hasScroll(parent);
            if (ret.scrollX || ret.scrollY) {
                scroller = parent;
                break;
            } else {
                parent = parent.parentNode;
            }
        }
        if (scroller) {
            var offset1 = $(el).offset().top;
            var offset2 = $(scroller).offset().top;
            var height = $(scroller).innerHeight();
            var scrollTop = offset1 - offset2 - height / 2;
            if (scrollTop > 0) {
                complete = complete || function () { };
                $(scroller).animate({ 'scrollTop': scrollTop }, 512, complete);
            }
        }
    };


    var IPageDesign = function (config) {
        $.extend(this, config);
    };

    IPageDesign.types = {};
    IPageDesign.register = function (key, type) {
        IPageDesign.types[key] = type;
    };

    IPageDesign.prototype = {

        isEdit: null,

        outerApi: null,

        outerValue: null,

        initialize: function () {
            var self = this;
            this.outerValue = {};
            this.isEdit = ($('#IsEdit').val().toLowerCase() === 'true');
            this.isEdit && (this.outerValue = this.outerApi.onGetValue());

            // cancel btn close
            $('#btnCancel').click(function () {
                self.outerApi.onInnerClose();
            });

            // ok btn submit
            $('form').submit(function (ev) {
                // trigger
                self.triggerSave();
                // post data
                var postData = $(this).serializeArray();
                // valid
                var success = self.validateForm({
                    postData: postData,
                    isEmpty: function (name) {
                        var empty = true;
                        $.each(postData, function () {
                            if (this.value && this.name == name) {
                                empty = false;
                                return false;
                            }
                        });
                        return empty;
                    },
                    form: $(this)
                });
                (success !== false) && (success = self.outerApi.onValid(postData));
                if (success !== false) {
                    self.doPost($(this).attr('action'), postData, function (data) {
                        if (self.outerApi.onInnerOk(data.html) !== false) {
                            // after the success handler executed, there are some global events will be fire in jQuery.ajax,
                            // and when i close the dislog page at success handler, and jQuery.ajax throw errors, 
                            // so i setTimeout to close the dialog page. but only ie9 throw this error, strangely!
                            setTimeout(function () {
                                self.outerApi.onInnerClose();
                            }, 32);
                        }
                    });
                }
                // ret
                return false;
            });
        },

        disableBtns: function (disabled) {
            $('#btnCancel, input[type="submit"]').attr('disabled', disabled);
        },

        doPost: function (url, data, callback) {
            // This is working, but I don't know what is the jquery 1.5.x version changed.
            var dataType = ($.fn.jquery.indexOf('1.5') === 0) ? 'text json' : 'json';
            var self = this;
            $.ajax({
                url: url,
                data: data,
                type: 'post', dataType: dataType, timeout: 30000,
                error: function () { alert(options.errorMessage); },
                beforeSend: function (request) { self.disableBtns(true); },
                complete: function (request, state) { self.disableBtns(false); },
                success: function (d, state) { callback(d); }
            });
        },

        restoreValue: function () {
            if (this.isEdit) {
                $('#Order').val(this.outerValue.Order);
                $('#PagePositionId').val(this.outerValue.PagePositionId);
            } else {
                // set a large value to place this field at last
                // this order value will be reordered after added
                $('#Order').val('999999');
            }
        },

        triggerSave: function () {
            window.tinyMCE && (window.tinyMCE.triggerSave());
        },

        validateForm: function (context) {
            if (context.form) {
                return context.form.valid();
            }
            return true;
        }
    };


    /*
    * view
    */
    var ProcessView = function (config) {
        ProcessView.superclass.constructor.call(this, config);
    };
    __extend(ProcessView, IPageDesign, {
        initialize: function () {
            ProcessView.superclass.initialize.call(this);
            var self = this;
            // view tree
            var tree = $("#J_DataTree");
            // rename all ParameterTemplate's INPUT and SELECT to no_submit.
            var noSubmitParameterFormElements = function (container) {
                (container || $('.parameterTemplate')).each(function () {
                    $(this).find('input,select').each(function () {
                        var name = $(this).attr('name');
                        if (name) {
                            $(this).removeAttr('name');
                            $(this).attr('no_submit_name', name);
                        }
                    });
                });
            };
            setTimeout(function () {
                noSubmitParameterFormElements();
            }, 32);
            // checks
            var checks = tree.find('input[name="ViewName"]');
            checks.change(function () {
                if ($(this).attr('checked')) {
                    checks.attr('checked', false);
                    $(this).attr('checked', true);
                }
            });
            var showTreeNode = function (radio) {
                var $radio = $(radio);
                $radio.parentsUntil('ul#J_DataTree', 'li').toggleClass('active');
                $radio.parentsUntil('ul#J_DataTree', 'ul').show();
            };
            checks.change(function () {
                showTreeNode(this);
                var parameter_tr = $('#parameter_tr').hide();
                var parameterCon = $('#parameter');
                var contentfrom = parameterCon.data('contentfrom');
                if (contentfrom) {
                    contentfrom.append(parameterCon.children());
                    parameterCon.data('contentfrom', undefined);
                    noSubmitParameterFormElements(contentfrom);
                } else {
                    parameterCon.empty();
                }
                // no check
                var current = $('.current-view');
                if (!$(this).attr('checked')) {
                    current.text('');
                    return;
                }
                // show view ParameterTemplate
                var parameterTemplateCon = $(this).siblings('.parameterTemplate'), evalDelayScripts;
                if (parameterTemplateCon.length > 0) {
                    parameter_tr.show();
                    // pure text nodes are ignored in jQuery
                    if (parameterTemplateCon.children().length > 0) {
                        parameterTemplateCon.find('script').remove(); // prevent execute multi times
                        parameterCon.append(parameterTemplateCon.children());
                        parameterCon.data('contentfrom', parameterTemplateCon);
                    } else {
                        // decode html
                        var sourceHtml = parameterTemplateCon.text() || '';
                        sourceHtml = sourceHtml.replace(/\+/ig, ' ');
                        sourceHtml = decodeURIComponent(sourceHtml);
                        // match script tags
                        var rscript = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi;
                        var match = rscript.exec(sourceHtml), scriptBlocks = [];
                        while (match) {
                            scriptBlocks.push(match[0]);
                            match = rscript.exec(sourceHtml);
                        }
                        // delay scripts
                        if (scriptBlocks.length > 0) {
                            evalDelayScripts = function () {
                                parameterCon.append(scriptBlocks.join(''));
                            };
                        }
                        // insert
                        parameterCon.append(sourceHtml.replace(rscript, ''));
                        parameterCon.data('contentfrom', null);
                    }
                }
                // show view parameters
                var definedParametersCon = $(this).siblings('.definedParameters');
                if (definedParametersCon.length > 0) {
                    parameterCon.append(definedParametersCon.html());
                    parameter_tr.show();
                    // bind parameters editor
                    var index = -1, indexName, dataType, dataTypeInput, valueInput;
                    var indexNames = parameterCon.find('[no_submit_name="Parameters.Index"]');
                    while (true) {
                        index++;
                        if (indexNames.length == 0) {
                            indexName = index.toString();
                        } else {
                            if (index >= indexNames.length) {
                                break;
                            } else {
                                indexName = indexNames.eq(index).val();
                            }
                        }
                        dataTypeInput = parameterCon.find('[no_submit_name="Parameters[' + indexName + '].DataType"]');
                        if (dataTypeInput.length > 0) {
                            valueInput = dataTypeInput.siblings('[no_submit_name="Parameters[' + indexName + '].Value"]');
                            dataType = dataTypeInput.val();
                            if (dataType == 'String') {
                                // nothing
                            } else if (dataType == 'Bool') {
                                // nothing
                            } else if (dataType == 'Int') {
                                valueInput.numeric(false);
                            } else if (dataType == 'Decimal') {
                                valueInput.numeric('.');
                            } else if (dataType == 'DateTime') {
                                valueInput.datepicker();
                            }
                        } else {
                            break;
                        }
                    }
                }
                // rename no_submit to normal submit NAME attribute
                parameterCon.find('[no_submit_name]').each(function () {
                    var name = $(this).attr('no_submit_name');
                    if (name) {
                        $(this).removeAttr('no_submit_name');
                        $(this).attr('name', name);
                    }
                });
                // set current select
                var selected = $(this).val();
                current.text(selected);
                // resotre orginal parameters
                if (selected == self.outerValue.ViewName) {
                    self.restorePrameters();
                }
                // eval delayed scripts (call after restorePrameters)
                if (evalDelayScripts) { evalDelayScripts(); }
            });
            // cache duration
            $('#enablecache').change(function () {
                var con = $('#cache_con');
                if ($(this).attr('checked')) {
                    var input = con.show().find('input').focus();
                    if (!input.val()) { input.val('120'); } // default to 120 second
                } else {
                    con.hide();
                }
            });
            // mask input
            $('#cache_con').find('input').numeric(false);
        },

        restorePrameters: function () {
            var params = $.parseJSON(this.outerValue.Parameters);
            if (params) {
                var parameterCon = $('#parameter'), index = -1, indexName, name, nameInput, valueInput, dataItem;
                var indexNames = parameterCon.find('[name="Parameters.Index"]');
                while (true) {
                    index++;
                    if (indexNames.length == 0) {
                        indexName = index.toString();
                    } else {
                        if (index >= indexNames.length) {
                            break;
                        } else {
                            indexName = indexNames.eq(index).val();
                        }
                    }
                    nameInput = parameterCon.find('[name="Parameters[' + indexName + '].Name"]');
                    if (nameInput.length > 0) {
                        dataItem = undefined;
                        name = nameInput.val().toLowerCase();
                        $.each(params, function () {
                            if (this.Name.toLowerCase() == name) {
                                dataItem = this;
                                return false;
                            }
                        });
                        if (dataItem) {
                            valueInput = parameterCon.find('[name="Parameters[' + indexName + '].Value"]');
                            if (valueInput.length > 0) {
                                if (valueInput.attr('type') == 'checkbox' || valueInput.attr('type') == 'radio') {
                                    var sep = ',', value = sep + (dataItem.Value + '').toLowerCase() + sep;
                                    valueInput.each(function () {
                                        if (value.indexOf(sep + $(this).val().toLowerCase() + sep) !== -1) {
                                            //$(this).attr('checked', true);
                                            this.checked = true;
                                        } else {
                                            //$(this).removeAttr('checked');
                                            this.checked = false;
                                        }
                                    });
                                }
                                else if (valueInput.get(0).tagName.toLowerCase() == 'select' && valueInput.attr("multiple")) {
                                    var val = (dataItem.Value ? dataItem.Value.toString() : '').split(',');
                                    var selOptions = valueInput.find("option");
                                    $.each(val, function (ix, it) {
                                        selOptions.filter("[value='" + it + "']").attr("selected", "selected");
                                    });
                                }
                                else {
                                    var strval = dataItem.Value ? dataItem.Value.toString() : '';
                                    if (dataItem.DataType == 'DateTime') { strval = strval.replace(/\+/g, ' '); }
                                    valueInput.val(strval);
                                }
                                valueInput.eq(0).trigger('change');
                            }
                        }
                    } else {
                        break;
                    }
                }
            }
        },

        duration: function (val) {
            var input = $('#cache_con').find('input');
            if (val === undefined) {
                return input.val();
            } else {
                input.val(val);
                //$('#enablecache').attr('checked', val != '').trigger('change');
            }
        },

        expirationPolicy: function (val) {
            var ep = $('#ExpirationPolicy');
            if (val) {
                ep.val(val);
            } else {
                ep.attr('selectIndex', 0);
            }
        },

        restoreValue: function () {
            ProcessView.superclass.restoreValue.call(this);
            // fire select view event
            var viewName = this.outerValue.ViewName;
            if (viewName) {
                var checkInput = $('input[name="ViewName"][value="' + viewName + '"]');
                if (checkInput.length > 0) {
                    __scrollToView(checkInput.get(0));
                    checkInput.attr('checked', true);
                    checkInput.trigger('change');
                }
            }
            if (this.outerValue.SkipError == 'true') {
                $('#skipError').attr('checked', true);
            }
            // OutputCache
            var dur, policy, outputCache = $.parseJSON(this.outerValue.OutputCache);
            if (outputCache) {
                $('#enablecache').attr('checked', outputCache.EnableCaching);
                dur = outputCache.Duration;
                policy = outputCache.ExpirationPolicy;
            }
            this.duration(dur || '');
            this.expirationPolicy(policy);
        },

        triggerSave: function () {
            ProcessView.superclass.triggerSave.call(this);
            var enabled = $('#enablecache').attr('checked');
            if (!enabled) { this.duration(''); }
        },

        validateForm: function (context) {
            var valid = ProcessView.superclass.validateForm.call(this, context);
            if (valid === false) { return false; }
            if (context.isEmpty('ViewName')) {
                alert(options.selectView);
                return false;
            }
        }
    });
    IPageDesign.register('view', ProcessView);


    /*
    * html
    */
    var ProcessHtml = function (config) {
        ProcessHtml.superclass.constructor.call(this, config);
    };
    __extend(ProcessHtml, IPageDesign, {
        textarea: null,
        initialize: function () {
            ProcessHtml.superclass.initialize.call(this);
            var self = this;
            // init tinymce
            this.textarea = $('#Textarea1');
            var mediaLibraryUrl = this.textarea.attr('media_library_url');

            var chooseFileFromMediaLibrary = function (inputId, value, fileType, window) {

                // execute popup
                var topJQ = top._jQuery || top.jQuery;
                var id = new Date().getTime();
                topJQ.pop({
                    id: id,
                    url: mediaLibraryUrl,
                    width: 900,
                    height: 500,
                    dialogClass: 'iframe-dialog',
                    frameHeight: '100%',
                    beforeLoad: function () {
                    },
                    onload: function (handle, pop, config) {
                        top.onFileSelected = function (src, text, option) {
                            var $srcInput = $('#' + inputId);
                            $srcInput.val(src);
                            var $descriptionInput = $srcInput.parent().parent().parent().next().find('input');
                            $descriptionInput.val(text);
                        };
                        top.fileSelectPop = pop;
                    },
                    onclose: function (handle, pop, config) {

                    }
                });
                tinymce.ztopKoobooDialog(id);
            }
            tinyMCE.init($.extend({}, tinyMCE.getKoobooConfig({ autoresize: false }), {
                elements: this.textarea.attr('id'),
                file_browser_callback: chooseFileFromMediaLibrary,
                media_library_url: mediaLibraryUrl,
                media_library_title: this.textarea.attr('media_library_title'),
                editor_selector: "richeditor",
                relative_urls: undefined,
                // load page styles to simulate a real runtime environment
                oninit: function () {
                    // seeto: http://msdn.microsoft.com/en-us/library/aa770023%28VS.85%29.aspx
                    if ($.browser.msie) {
                        try {
                            var doc = tinyMCE.get(self.textarea.attr('id')).dom.doc;
                            doc.execCommand('RespectVisibilityInDesign', true, true);
                        } catch (ex) { }
                    }
                    // inject style
                    self.injectStyle();
                }, setup: function (ed) {
                    ed.on('FullscreenStateChanged', function (e) {
                        $(window.parent.document).find('iframe').toggleClass('fullscreen');
                    });
                    ed.on('BeforeSetContent', function (e) {
                        //console.log(e.content);
                        e.format = 'raw';
                    });
                }
            }));
        },
        injectStyle: function () {
            var $P = this.outerApi.onGetParent();
            var sheets = $P('link[rel="stylesheet"]');
            if (sheets.length > 0) {
                var urls = [];
                sheets.each(function () { urls.push(this.href); });
                tinyMCE.get(this.textarea.attr('id')).dom.loadCSS(urls.join(','));
            }
            var styles = $P('style');
            if (styles.length > 0) {
                var cssText = [];
                styles.each(function () { cssText.push(this.textContent || this.innerHTML); });
                cssText = cssText.join('\n');
                var doc = tinyMCE.get(this.textarea.attr('id')).dom.doc;
                var head = doc.getElementsByTagName('head')[0];
                var node = doc.createElement('style');
                node.setAttribute('type', 'text/css');
                if ($.browser.msie) {
                    head.appendChild(node);
                    node.styleSheet.cssText = cssText;
                } else {
                    try {
                        node.appendChild(doc.createTextNode(cssText));
                    } catch (ex) {
                        node.cssText = cssText;
                    }
                    head.appendChild(node);
                }
            }
        },
        restoreValue: function () {
            ProcessHtml.superclass.restoreValue.call(this);
            this.textarea.val(this.outerValue.Html);
        }
    });
    IPageDesign.register('html', ProcessHtml);


    /*
    * folder
    */
    var ProcessFolder = function (config) {
        ProcessFolder.superclass.constructor.call(this, config);
    };
    __extend(ProcessFolder, IPageDesign, {
        initialize: function () {
            ProcessFolder.superclass.initialize.call(this);
            // Type
            var self = this;
            $('input[name="Type"]').change(function () {
                var t = self.resultType();
                var tp = $('#top_con'), uk = $('#userkey_con');
                if (t.val() == 'List') {
                    tp.show().find('input').focus();
                    uk.hide();
                } else if (top) {
                    uk.show().find('input').focus();
                    tp.hide();
                }
            });
            // UserKey
            var userKey = $('#UserKey');
            var getSource = function () {
                var folder = $('.folder-checkbox:checked').val();
                if (!folder) { return false; }
                var base = userKey.attr('source');
                return userKey.attr('source') + '&folderName=' + encodeURIComponent(folder) + '&term=' + encodeURIComponent(userKey.val());
            };
            userKey.autocomplete({
                search: function (event, ui) {
                    var s = getSource();
                    if (!s) { return false; }
                    $(this).autocomplete('option', 'source', s);
                }
            });
            // Top
            $('#Top').numeric(false);
        },

        resultType: function (type) {
            var inputs = $('input[name="Type"]');
            if (type === undefined) {
                var ret;
                inputs.each(function () {
                    if ($(this).attr('checked')) {
                        ret = $(this);
                        return false;
                    }
                });
                return ret;
            } else {
                return inputs.each(function () {
                    if ($(this).val() == type) {
                        $(this).attr('checked', true);
                        $(this).trigger('change');
                        return false;
                    }
                });
            }
        },

        triggerSave: function () {
            ProcessFolder.superclass.triggerSave.call(this);
            if (this.resultType().val() == 'List') {
                $('#UserKey').val('');
            } else {
                $('#Top').val('');
            }
        },

        restoreValue: function () {
            ProcessFolder.superclass.restoreValue.call(this);
            // type, default to select list
            var type = this.outerValue.ContentPositionType;
            this.resultType(type || 'List');
            // data rule
            var dataRule = $.parseJSON(this.outerValue.DataRule);
            if (!dataRule) { return; }
            if (dataRule.Top) {
                $('#Top').val(dataRule.Top);
            }
            if (dataRule.WhereClauses && dataRule.WhereClauses.length > 0) {
                $('#UserKey').val(dataRule.WhereClauses[0].Value1);
            }

            $(':radio[name="FolderName"][value="' + dataRule.FolderName + '"]').attr('checked', true);
        },

        validateForm: function (context) {
            var valid = ProcessFolder.superclass.validateForm.call(this, context);
            if (valid === false) { return false; }
            if (context.isEmpty('FolderName')) {
                alert(options.selectFolder);
                return false;
            }
        }
    });
    IPageDesign.register('folder', ProcessFolder);


    /*
    * module
    */
    var ProcessModule = function (config) {
        ProcessModule.superclass.constructor.call(this, config);
    };
    __extend(ProcessModule, IPageDesign, {
        EntryActionInput: null,
        EntryControllerInput: null,
        EntryOptionsSelect: null,
        setSelectedOptionByText: function (select, entryName) {
            select.find('option').each(function () {
                if ($(this).text() == entryName) {
                    $(this).attr("selected", "selected");
                }
            });
        },
        initialize: function () {
            ProcessModule.superclass.initialize.call(this);
            this.LinkToEntryNameInput = $('input[id="LinkToEntryName"]');
            this.EntryActionInput = $('input[id="EntryAction"]');
            this.EntryControllerInput = $('input[id="EntryController"]');
            this.ValuesTemplate = $('#DefaultValuesTemplate');
            this.EntryOptionsSelect = $('select[id="EntryOptions"]');
            var checks = $('input[name="ModuleName"]'), self = this;
            checks.change(function () {
                if ($(this).attr('checked')) {
                    checks.attr('checked', false);
                    $(this).attr('checked', true);
                }
            });
            checks.change(function () {
                // no check
                if (!$(this).attr('checked')) {
                    return;
                }
                // set module default settings
                var module = $(this).val();
                self.LinkToEntryNameInput.val($('input[id="' + module + 'LinkToEntryName"]').val());
                self.EntryActionInput.val($('input[id="' + module + 'EntryAction"]').val());
                self.EntryControllerInput.val($('input[id="' + module + 'EntryController"]').val());
                var entryName = $('input[id="' + module + 'EntryName"]').val();


                var optionsHtml = [], options = $.parseJSON($('input[id="' + module + 'EntryOptions"]').val());
                if (options && options.length) {
                    optionsHtml.push('<option></option>');
                    $.each(options, function (index, op) {
                        optionsHtml.push("<option action='" + op.EntryAction + "' controller='" + op.EntryController + "' values='" + ko.toJSON(op.Values) + "' linkToEntryName='" + op.LinkToEntryName + "'>" + op.Name + '</option>');
                    });
                }
                self.EntryOptionsSelect.append(optionsHtml.join(''));

                if (self.ValuesTemplate.data('KO_ViewModel')) {
                    self.ValuesTemplate.data('KO_ViewModel').renew($.parseJSON($('input[id="' + module + 'Values"]').val()));
                }

                if (entryName) {
                    self.setSelectedOptionByText(self.EntryOptionsSelect, entryName);
                }
            });
            this.EntryOptionsSelect.change(function () {
                var op = $(this).children().eq(this.selectedIndex);
                var action = op.attr('action'), controller = op.attr('controller');
                var values = op.attr('values');
                self.LinkToEntryNameInput.val(op.attr('linkToEntryName'));
                self.EntryActionInput.val(action); self.EntryControllerInput.val(controller);
                self.ValuesTemplate.data('KO_ViewModel').renew($.parseJSON(values));
            });
        },

        restoreValue: function () {
            ProcessModule.superclass.restoreValue.call(this);
            var self = this;

            // 
            var moduleName = self.outerValue.ModuleName;
            if (moduleName) { $('input[name="ModuleName"][value="' + moduleName + '"]').attr('checked', true).trigger('change'); }

            if (this.outerValue.SkipError == 'true') {
                $('#skipError').attr('checked', true);
            }
            setTimeout(function () {
                var entryName = self.outerValue.EntryName;
                if (entryName) {
                    self.setSelectedOptionByText(self.EntryOptionsSelect, entryName);
                }

                var linkToEntryName = self.outerValue.LinkToEntryName;
                if (linkToEntryName) {
                    self.LinkToEntryNameInput.val(linkToEntryName);
                }

                //
                var exclusive = self.outerValue.Exclusive;
                if (exclusive == 'true') { $('input[name="Exclusive"]').attr('checked', true); }
                //
                var action = self.outerValue.EntryAction;
                if (action) { self.EntryActionInput.val(action); }
                //
                var controller = self.outerValue.EntryController;
                if (controller) { self.EntryControllerInput.val(controller); }

                var values = self.outerValue.Values;

                if (values) {
                    self.ValuesTemplate.data('KO_ViewModel').renew($.parseJSON(values));
                }
            }, 200);
        },

        validateForm: function (context) {
            var valid = ProcessModule.superclass.validateForm.call(this, context);
            if (valid === false) { return false; }
            if (context.isEmpty('ModuleName')) {
                alert(options.selectModule);
                return false;
            }
        }
    });
    IPageDesign.register('module', ProcessModule);

    /*
    * htmlBlock
    */
    var ProcessHtmlBlock = function (config) {
        ProcessHtmlBlock.superclass.constructor.call(this, config);
    };
    __extend(ProcessHtmlBlock, IPageDesign, {
        initialize: function () {
            ProcessHtmlBlock.superclass.initialize.call(this);
            var checks = $('input[name="BlockName"]');
            checks.change(function () {
                if ($(this).attr('checked')) {
                    checks.attr('checked', false);
                    $(this).attr('checked', true);
                }
            });
        },
        restoreValue: function () {
            ProcessHtmlBlock.superclass.restoreValue.call(this);
            // 
            var blockName = this.outerValue.BlockName;
            if (blockName) { $('input[name="BlockName"][value="' + blockName + '"]').attr('checked', true).trigger('change'); }
        },
        validateForm: function (context) {
            var valid = ProcessHtmlBlock.superclass.validateForm.call(this, context);
            if (valid === false) { return false; }
            if (context.isEmpty('BlockName')) {
                alert(options.selectHtmlBlock);
                return false;
            }
        }
    });
    IPageDesign.register('htmlBlock', ProcessHtmlBlock);

    /*
    * proxy
    */
    var ProcessProxy = function (config) {
        ProcessProxy.superclass.constructor.call(this, config);
    };
    __extend(ProcessProxy, IPageDesign, {
        initialize: function () {
            ProcessProxy.superclass.initialize.call(this);

            // cache duration
            $('#enablecache').change(function () {
                var con = $('#cache_con');
                if ($(this).attr('checked')) {
                    var input = con.show().find('input').focus();
                    if (!input.val()) { input.val('120'); } // default to 120 second
                } else {
                    con.hide();
                }
            });
            // mask input
            $('#cache_con').find('input').numeric(false);
        },
        duration: function (val) {
            var input = $('#cache_con').find('input');
            if (val === undefined) {
                return input.val();
            } else {
                input.val(val);
                $('#enablecache').attr('checked', val != '').trigger('change');
            }
        },

        expirationPolicy: function (val) {
            var ep = $('#ExpirationPolicy');
            if (val) {
                ep.val(val);
            } else {
                ep.attr('selectIndex', 0);
            }
        },
        restoreValue: function () {
            ProcessProxy.superclass.restoreValue.call(this);
            // 
            var host = this.outerValue.Host;
            if (host) { $('input[name="Host"]').val(host); }

            var requestPath = this.outerValue.RequestPath;
            if (requestPath) { $('input[name="RequestPath"]').val(requestPath); }

            var noProxy = this.outerValue.NoProxy;
            if (noProxy == "true") { $('input[name="NoProxy"]').attr('checked', true); }

            var proxyStylesheet = this.outerValue.ProxyStylesheet;
            if (proxyStylesheet == "true") { $('input[name="ProxyStylesheet"]').attr('checked', true); }

            // OutputCache
            var dur, policy, outputCache = $.parseJSON(this.outerValue.OutputCache);
            if (outputCache) {
                dur = outputCache.Duration;
                policy = outputCache.ExpirationPolicy;
            }
            this.duration(dur || '');
            this.expirationPolicy(policy);
        },

        triggerSave: function () {
            ProcessView.superclass.triggerSave.call(this);
            var enabled = $('#enablecache').attr('checked');
            if (!enabled) { this.duration(''); }
        },
    });
    IPageDesign.register('proxy', ProcessProxy);


    /*
    * page load
    */
    if (!window.getDesignerType) {
        // empty implement, this functon must be override in specific page.
        window.getDesignerType = function (types) { };
    }
    $(function () {

        // get content iframe
        var iframe = function () {
            window.__rid = Math.random().toString();
            var frm, frms = window.parent.document.getElementsByTagName('iframe');
            for (var i = 0; i < frms.length; i++) {
                if (frms[i].contentWindow.__rid == window.__rid) {
                    frm = frms[i];
                    break;
                }
            }
            return frm;
        }();
        if (!iframe) { return; }

        // get outer api
        var outerApi = iframe.outerApi;
        if (!outerApi) { return; }

        // init
        var type = getDesignerType(IPageDesign.types);
        if (type) {
            var designer = new type({ outerApi: outerApi });
            designer.initialize();
            designer.restoreValue();
        }
    });

    var IRichEditor = function (config) {
        $.extend(this, config);
        this.initialize();
    };
    IRichEditor.prototype = {
        host: null,
        initialize: function () { },
        triggerSave: function () { },
        getValue: function () { },
        setValue: function (val) { },
        destroy: function () { }
    };

})(jQuery);