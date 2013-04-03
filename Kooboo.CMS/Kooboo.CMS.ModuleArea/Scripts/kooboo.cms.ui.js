/// <reference path="jquery-1.4.4-vsdoc.js" />
/// <reference path="jquery-ui.js" />
/// <reference path="extension/js.extension.js" />


/// <reference path="kooboo.js" />

/*
*********comment style*****************


///<summary>
///
///
///</summary>
///	<param name="selector" type="String">
///
///	</param>
///	<param name="context" type="jQuery">
///		
///	</param>
///	<returns type="jQuery" />


*/

(function ($) {
    var jQuery = $;

    ; kooboo.namespace("kooboo.cms.ui");

    ///kooboo.cms.ui.dynamicList
    (function () {

        kooboo.cms.ui.extend({
            dynamicList: (function (option) {
                var api = {};
                var _instanceMap = {};
                var _configMap = {};

                api.instance = function (option) {
                    //Singleton Pattern return the same instance
                    if (typeof (option) == 'string') {
                        return _instanceMap[option];
                    }
                    if (!option.containerId) {
                        throw 'container Id is null';
                    }

                    if (_instanceMap[option.containerId]) {
                        return _instanceMap[option.containerId];
                    }

                    var instance = new _dynamicList(option);

                    _instanceMap[option.containerId] = instance;

                    return instance;
                }


                function modifyId(item) {
                    var id = $(item).attr('id'),
                    random = Math.random().toString().replace('.', '-');
                    id = id + 'random-' + random;
                    $(item).attr('id', id);
                }

                function _dynamicList(option) {
                    if (typeof (option) == 'string') {
                        option = _configMap[option];
                    }
                    if (option && !option.containerId) {
                        throw "containerId is null!";
                    }
                    if (option && !option.propertyName) {
                        throw "propertyName is null!";
                    }
                    var config = {
                        containerId: '', // not null
                        propertyName: '', // not null
                        templateClass: 'template', //could be null
                        newItemClass: "datafield-" + Math.random().toString().replace('.', ''),
                        templateId: '', // not null 
                        data: [], //could be null,
                        onInit: function () { },
                        fixInput: true
                    };

                    var data = config.data;

                    var generateItemSelector = '.' + config.newItemClass;

                    $.extend(config, option);
                    var container = $('#' + config.containerId);
                    var template = $('#' + config.templateId);
                    var tagName = template.get(0).tagName; //var tagName = template.attr('tagName'); // fix jQuery v1.7.1 by crl 2012.1.5
                    template.hide();
                    template.find('input,select').attr('disabled', true);

                    if (config.fixInput) {

                        template.find(':checkbox,:radio').each(function () {
                            var handle = $(this), type = handle.attr('type');
                            handle.attr('fieldType', type);
                            if (fixedInput[type]) {
                                fixedInput[type](handle);
                            }
                        });
                    }

                    function add(value, index) {
                        var clone = template.clone().removeAttr('id');
                        clone.data('item-data', value);
                        clone.addClass(config.newItemClass);
                        setTimeout(function () {
                            clone.find('input,select').filter(':enabled').filter(':visible').first().focus();
                        }, 100);

                        clone.find('input:not([indexField]),select:not(indexField)').attr('disabled', false);
                        clone.show();
                        clone.find('input:not([indexField]),select:not(indexField)').each(function () {
                            var input = $(this), inputVal;                            
                            var name = input.attr('name');
                            modifyId(input);
                            var fieldType = input.attr('fieldtype');
                            if (!fieldType) { // handle normal input                                
                                if (name && name.indexOf('.') > 0) {//property is object
                                    var val = kooboo.object.getVal(value, name);
                                    if (kooboo.date.reg.utc.test(val)) {
                                        val = kooboo.date.parser.utc(val).format('yyyy-mm-dd');
                                    }
                                    inputVal = val;
                                } else {//property is value
                                    if (name && value[name] != undefined) {
                                        var val = kooboo.object(value).getVal(name);
                                        if (kooboo.date.reg.utc.test(val)) {
                                            val = kooboo.date.parser.utc(val).format('yyyy-mm-dd');
                                        }
                                        inputVal = val;
                                    } else if (!$.isPlainObject(value)) {
                                        inputVal = value;
                                    }
                                }
                                if (!input.is(':file')) {
                                    input.val(inputVal);
                                    if (input.is('select')) {
                                        input.attr('selectedValue', inputVal);
                                    }
                                } else {
                                    input.attr('valueAttr', inputVal);
                                }
                                ///handle for checkbox \ radio
                            } else {
                                _fieldtype[fieldType](input, value, index);
                            }

                        });
                        container.append(clone);
                        return clone;
                    }

                    function init() {
                        if ($.isArray(config.data)) {
                            $(config.data).each(function (index, value) {
                                add(value, index);
                            }); //end each loop
                        }

                    }

                    init(); //do init 
                    this.getItems = function () {
                        return container.children(generateItemSelector);
                    }
                    this.add = function (value) {
                        ///<summary>
                        ///return new item
                        ///</summary>
                        value = value == undefined ? {} : value;
                        var clone = add(value, this.getItems().length);
                        return clone;
                    }
                    this.remove = function (obj) {
                        ///<summary>
                        ///obj is the item you will delete
                        ///</summary>
                        obj.remove();
                        this.sort();
                    }
                    this.getItemData = function (obj) {
                        return obj.data('item-data');
                    }
                    this.setItemData = function (obj, dataValue) {
                        obj.data('item-data', dataValue);
                    }
                    this.sort = function () {
                        ///<summary>
                        /// Sort index for names
                        ///</summary>
                        container.find(generateItemSelector).each(function (index) {
                            var current = $(this);
                            current.find('input,select').each(function (i) {

                                var name = $(template.find('input,select')[i]).attr('name');

                                name = (name && name.trim().length > 0) ? (config.propertyName + '[' + index + '].' + name) : (config.propertyName + '[' + index + ']');
                              
                                $(this).attr('name', name);
                            });
                        });

                    }
                    this.reset = function (dataSource) {
                        if (dataSource != undefined) {
                            data = dataSource;
                        }
                        container.find(generateItemSelector).remove();
                        init();
                    }
                    this.getData = function () {
                        return data;
                    }
                    this.itemSelector = generateItemSelector;
                }

                return api;
            })() //end dynamicList
        });                         //end kooboo.cms.ui.extend()


        var _fieldtype = {
            checkbox: function (input, value, index) {
                var name = input.attr('name');
                var val = kooboo.object(value).getVal(name);
                ///checkbox for display
                if (input.attr('type') == 'checkbox') {

                    if (val && val.toString().toLower() == 'true') {
                        input.attr('checked', true);
                    } else {
                        input.attr('checked', false);
                    }
                    ///hidden for checkbox
                } else {
                    ///do nothing
                }
            },
            radio: function (input, value, index) {
                var name = input.attr('name');
                var val = kooboo.object(value).getVal(name);
                if (val && val.toLower() == input.val().toLower()) {
                    input.attr('checked', true);
                }
            }
        };

        var fixedInput = {
            checkbox: function (handle) {
                var name = handle.attr('name');
                $('input[name="' + name + '"]').attr('fieldtype', 'checkbox');
            }
        };

        kooboo.cms.ui.dynamicList.fieldType = _fieldtype;

        kooboo.cms.ui.extend({
            dynamicListInstance: function (config) {
                ///<summary>
                /// you can config option like this
                ///option = {
                ///    containerId:'',
                ///    templateId:'',
                ///    delClass:'del',/*default(del)*/
                ///    onInit:function(){},/*default(null)*/
                ///    beforeAdd:function(){},/*default(null)*/
                ///    onAdd:function(){},/*default(null)*/
                ///    beforeRemove:function(){},/*default(null)*/
                ///    onRemove:function(){},/*default(null)*/
                ///    addButtonId:'',
                ///    propertyName:''
                ///}
                ///</summary>

                var option = {
                    containerId: '', //not null
                    templateId: '', //not null
                    addButtonId: '', //not null
                    delClass: 'remove',
                    onInit: function () { },
                    beforeAdd: function () { },
                    onAdd: function () { },
                    beforeRemove: function () { },
                    onRemove: function () { },
                    parentSelector: null,
                    hideRemoveBtn: false
                };

                kooboo.extend(option, config);

                var instance = kooboo.cms.ui.dynamicList.instance(option);

                initItems();

                function initItems() {

                    var items = instance.getItems();
                    ///init remove method
                    var removeBtn = items.find('.' + option.delClass).click(function () {

                        var parent = $(this).parent();
                        if (option.parentSelector != null) {
                            if (typeof option.parentSelector == 'string') {
                                parent = $(this).parents(option.parentSelector);
                            } else if (typeof option.parentSelector == 'function') {
                                parent = option.parentSelector.call(this, instance);
                            } else if (typeof option.parentSelector == 'object') {
                                parent = option.parentSelector;
                            }
                        }

                        if (typeof (option.beforeRemove) == 'function') {
                            var result = option.beforeRemove.call(instance, parent, this);
                            if (result == false) {
                                return false;
                            }
                        }

                        instance.remove(parent);
                        instance.sort();
                        option.onRemove.call(instance);
                        return false;
                    });
                    if (option.hideRemoveBtn) {
                        items.hover(function () {
                            $(this).find('.' + option.delClass).show();
                        }, function () {
                            $(this).find('.' + option.delClass).hide();
                        });
                        removeBtn.hide();
                    }

                    instance.sort();

                    if (typeof (option.onInit) == 'function') {
                        option.onInit.call(instance, instance);
                    }
                }

                ///init add method

                var add = instance.add;
                instance.add = function () {
                    var item = add.apply(instance, arguments);
                    initItems();
                    if (typeof (option.onAdd) == 'function') {
                        option.onAdd.call(instance, item);
                    }
                }

                var addBtn = $('#' + option.addButtonId);

                if (!addBtn.data('dynamiclist-bind')) {
                    addBtn.data('dynamiclist-bind', true);
                    addBtn.click(function () {
                        if (typeof (option.beforeAdd) == 'function') {
                            var result = option.beforeAdd.call(instance);
                            if (result == false) {
                                return false;
                            }
                        }
                        var item = instance.add();
                        return false;
                    });
                }

                return instance;
            }
        });

    })();

    ///kooboo.cms.ui.iframeCheckboxList
    kooboo.cms.ui.extend({
        iframeCheckboxList: (function () {
            var checkedList = [];
            var apiMap = {};

            function iframeCheckboxList(option) {
                var config = {
                    iframeId: '',
                    checkboxListClass: "checkboxList",
                    checkedList: [],
                    initAfterReady: true,
                    cache: true,
                    onItemChecked: function () { },
                    onItemInit: function () { },
                    beforeAdd: function () { },
                    onAdd: function () { },
                    beforeRemove: function () { },
                    onReomve: function () { }
                };

                kooboo.extend(config, option);

                if (apiMap[config.iframeId] && config.cache) {
                    return apiMap[config.iframeId];
                }
                var iframe = $("#" + config.iframeId);

                function readyEvent() {
                    iframe.contents()
                    .find("input:checkbox." + config.checkboxListClass)
                    .each(function () {
                        var current = $(this);
                        current.unbind("click").click(function (event) {
                            event.stopPropagation();
                            if (!current.attr('checked')) {//cancel checked
                                config.checkedList = config.checkedList.removeElement(current.val());
                            } else {//checked
                                config.checkedList.push(current.val());
                            }
                            config.onItemChecked(current.attr('checked'), current);
                        });
                        var checked = config.checkedList.contain(current.val());
                        current.attr('checked', checked);
                        config.onItemInit(checked, current);
                    });
                }

                $(config.checkedList).each(function (index) {
                    config.checkedList[index] = config.checkedList[index].toString();
                });


                readyEvent();
                var api = {};
                api.getCheckedList = function () {
                    return config.checkedList;
                }
                api.reset = (function () {
                    var checkedList = config.checkedList;
                    return function () {
                        config.checkedList = checkedList;
                        readyEvent();
                    }
                })();

                api.add = function (val) {
                    config.checkedList.push(val);
                    readyEvent();
                }
                api.remove = function (val) {
                    config.checkedList = config.checkedList.removeElement(val);
                    readyEvent();
                }
                api.getConfig = function () {
                    return config;
                }
                if (config.cache) {
                    apiMap[config.iframeId] = api;
                }
                return api;

            } //end function checkboxList()
            return iframeCheckboxList;
        })()//end kooboo.cms.ui.iframeCheckboxList
    });            //end kooboo.cms.ui.extend

    kooboo.cms.ui.extend({
        flexList: function (option) {
            var config = {
                containerSelector: ".flex-list",
                btnSelector: ".arrow",
                activeClass: "active",
                itemSelector: ".datafield",
                autoExpand: true
            };

            $.extend(config, option);

            var flexList = $(config.containerSelector);
            flexList.each(function () {
                var current = $(this);

                var btn = current.find(config.btnSelector);

                if (config.autoExpand) {
                    show(btn, current);
                    btn.toggle(function () {
                        hide(btn, current);
                    }, function () {
                        show(btn, current);
                    });
                } else {
                    hide(btn, current);
                    btn.toggle(function () {
                        show(btn, current);
                    }, function () {
                        hide(btn, current);
                    });
                } //end if config.autoExpand

                function hide(btn, current) {
                    btn.removeClass("active");
                    current.find(config.itemSelector).hide();
                }
                function show(btn, current) {
                    btn.addClass("active");
                    current.find(config.itemSelector).show();
                }

            });
        }
    });

    //kooboo.cms.ui.formHelper
    (function ui_formhelper() {
        kooboo.namespace("kooboo.cms.ui.formHelper");
        kooboo.cms.ui.formHelper.extend({

            hiddenCls: 'machine-column-input-field',

            createHidden: function (data, name, $form) {
                var current = this;
                form = $($form);

                if ($.isPlainObject(data)) {
                    for (var p in data) {
                        current.createHidden(data[p], name + '.' + p, form);
                    }
                } else if ($.isArray(data)) {
                    data.each(function (val, index) {
                        current.createHidden(val, name + '[' + index + ']', form);
                    });
                } else {
                    form.addHidden(name, data);
                }
            },

            setForm: function (data, $form, pre) {
                form = $($form);
                var current = this;
                for (var prop in data) {
                    var fullName = pre ? (pre + "." + prop) : prop;
                    if ($.isPlainObject(data[prop])) {
                        current.setForm(data[prop], form, fullName);
                    } else if (!$.isArray(data[prop])) {
                        form.setFormField(fullName, data[prop]);
                    }
                }
            },

            tempForm: function (dataObj, url, pre, formAttr, $form) {
                ///<summary>
                /// make data into hidden field
                /// data can be object or array
                /// 
                ///</summary>
                /// 
                ///	<param name="dataObj" type="object">
                ///	the data you want to put into form
                ///	</param>
                /// 
                ///	<param name="url" type="String">
                ///	the form's url  (can be null)
                ///	</param>
                /// 
                ///	<param name="pre" type="String">
                ///	the data's prefix (can be null)
                ///	</param>
                ///	<param name="formAttr" type="object">
                ///	the form's html attributes
                ///	</param>
                ///	<param name="$form" type="jQuery">
                ///	can be null or jquery object
                ///	</param>
                ///	<returns type="formHelpAPI" />
                var formAttrConfig = {
                    action: url,
                    method: 'post'
                };
                var current = this;
                $.extend(formAttrConfig, formAttr);

                var form = $form || $("<form></form>").attr(formAttrConfig);

                form.appendTo($("body"));

                function init(data) {
                    current.createHidden(data, pre, form);
                }

                init(dataObj);

                var api = {
                    clear: function () {
                        form.html('');
                    },
                    reset: function (data) {
                        form.html('');
                        data = data || dataObj;
                        init(data);
                    },
                    submit: function () {
                        form.submit();
                    },
                    addData: function (data, dataPre) {
                        dataPre = dataPre ? dataPre : pre;
                        current.createHidden(data, dataPre, this.form);
                        return this;
                    },
                    ajaxSubmit: function (option) {
                        option = option ? option : {};
                        option.url = option.url ? option.url : formAttrConfig.action;
                        option.type = option.type ? option.type : formAttrConfig.method;
                        var ajaxConfig = {
                            data: form.serialize()
                        };
                        $.extend(option, ajaxConfig);
                        $.ajax(option);
                    },
                    form: form
                };

                return api;
            },

            clearHidden: function (source) {
                $(source).find('.' + this.hiddenCls).remove();
                return this;
            },

            copyForm: function (source, dest, nameProvider) {
                ///<summary>
                /// copy input or select or hidden value to destination form
                /// UNDONE
                ///</summary>
                ///	<param name="$source" type="String,jQuery">
                /// 
                ///	</param>
                ///	<param name="$form" type="jQuery">
                ///		
                ///	</param>
                ///	<returns type="jQuery" />
                var getName = function (input) {
                    var name = input.attr('name');
                    if (typeof (nameProvider) == 'function') {
                        name = nameProvider.call(input);
                    }
                    return name;
                },
            hiddenStr = "<input type='hidden'/>",
            tempForm = dest || $('<form/>'),
            machineCls = this.hiddenCls;
                source = $(source);

                source.find(':checkbox').each(function () {
                    var input = $(this);
                    var hidden = $(hiddenStr)
                    .attr('name', getName(input))
                    .addClass(machineCls);
                    if (source.find(input.attr('name')).length == 2) {
                        hidden.val(input.attr('checked'));
                        hidden.appendTo(tempForm);
                    } else if (input.attr('checked')) {
                        hidden.val(input.val());
                        hidden.appendTo(tempForm);
                    }
                });

                source.find('input:not(:checkbox),textarea').each(function () {
                    var handle = $(this);
                    var hidden = $(hiddenStr).attr({ name: getName(handle), value: handle.val() }).appendTo(tempForm).addClass(machineCls);
                });

                source.find("select").each(function () {
                    var input = $(this), inputValue;

                    inputValue = input.val() && input.val().length ? input.val() : input.find('option:eq(0)').val();

                    var hidden = $(hiddenStr)
                    .attr('name', getName(input))
                    .val(inputValue)
                    .addClass(machineCls);
                    hidden.appendTo(tempForm);
                });

                return tempForm;
            }

        });
        var _hiddenStr = '<input type="hidden" />';
        function generateHidde() {
            return $(_hiddenStr);
        }
        var formHelper = {
            inputTypeHandle: {
                'radio': function (input, $source, $dest) {
                    var hidden = generateHidde();

                    if (input.attr('checked')) {
                        hidden.val(input.val());
                        return hidden;
                    } else {
                        return false;
                    }
                },
                'checkbox': function (input, $source, $dest) {
                    var hidden = generateHidde();
                    if (hidden.attr('checked')) {
                        hidden.val(true);
                    }
                },
                'select-one': function (input, $source, $dest) {
                    var hidden = generateHidde();

                },
                'select-many': function (input, $source, $dest) {
                    var hidden = generateHidde();
                },
                'hidden': function (input, $source, $dest) {
                    var hidden = generateHidde();
                },
                defaultInput: function (input, $source, $dest) {
                    var hidden = generateHidde();

                }
            }
        };
    })();

    (function uiLoadingbox() {
        var timmer = -1;
        kooboo.cms.ui.extend({
            timeout: (function () {
                var s = 5, ui;
                setInterval(function () {
                    s--;
                    if (s <= 0) {
                        if (ui) {
                            ui.messageBox().hide();
                        }
                    }
                }, 1000);
                return function (UI, t) {
                    ui = UI;
                    s = t ? t / 1000 : 5;
                }
            })(),
            messageBox: function () {
                var ui = kooboo.cms.ui;
                var messageBox = $("#message-box");
                var api = {
                    showResponse: function (response) {
                        var tipClass = "error";
                        if (response.Success)
                            tipClass = "info";
                        var messages = response.Messages;
                        if (messages && messages.length > 0) {
                            var str = messages.join("<br />");
                            this.show(str, tipClass);
                        }
                    },
                    show: function (tip, tipClass, timeout, showOption) {
                        tip = tip || "";
                        timeout = timeout || 5000;
                        tipClass = tipClass || "info";
                        showOption = showOption || 'blind';

                        this.removeWaiting();

                        messageBox.hide().removeClass("hide").show(showOption).removeClass("info warning error").addClass(tipClass);

                        messageBox.find("p.msg").html(tip).removeClass("hide");

                        ui.timeout(ui, timeout);
                    },
                    hide: function (option) {
                        option = option || 'slideUp';
                        messageBox.slideUp(function () { $(this).addClass("hide"); });
                        messageBox.find("p.waiting").add("hide");

                        this.removeWaiting();
                    },
                    removeWaiting: function () {
                        ui.loading().hide();
                    },
                    waiting: function () {
                        ui.loading().show();
                    }
                };
                return api;
            },
            loading: function () {

                var ui = this;
                var loading = $('div.kooboo-cms-waiting');
                var submit = $(':submit,.submit,.button');



                var api = {
                    show: function () {
                        var t1 = new Date();
                        timmer = 0;
                        var host = this;
                        loading.show();
                        submit.attr('diabled', true);
                        setInterval(function () {
                            var checker = timmer != -1 ? timmer++ : timmer;
                            if (timmer > ui.setting.timeout && timmer != -1) {
                                host.hide();
                                //                        kooboo.confirm(ui.setting.timeoutMessage,
                                //                        function (r) {
                                //                            if (r) {
                                //                                top.location.reload(true);
                                //                            }
                                //                        });
                            }
                        }, 1000);
                    },
                    hide: function () {
                        timmer = -1;
                        loading.hide();
                        submit.removeAttr('diabled');
                    }
                }

                return api;
            },
            setting: {
                timeout: 60, //second
                timeoutMessage: 'The bad netword or some unknow reason cause operation timeout, please try to reload the page.'
            }
        });
    })();

    (function ui_status() {
        kooboo.cms.ui.extend({
            status: (function () {
                var status = {}, msg;
                return function (name) {
                    name = name || "kooboo-page-status";
                    if (status[name]) {
                        return status[name];
                    } else {
                        var editStatus = (function () {
                            var canleave = true;
                            var api = {
                                init: function (mesg) {
                                    msg = mesg;
                                    return this;
                                },
                                bind: function (obj, mesg, event) {
                                    obj = obj || window;
                                    msg = mesg;
                                    event = event || 'beforeunload';
                                    $(obj).bind(event, function () {
                                        if (!canleave) {
                                            return msg;
                                        }
                                    });
                                },
                                msg: function (mesg) {
                                    if (mesg) {
                                        msg = mesg;
                                        return this;
                                    }
                                    return msg;
                                },
                                canLeave: function () {
                                    return canleave;
                                },
                                stop: function () {
                                    canleave = false;
                                },
                                pass: function () {
                                    canleave = true;
                                }
                            };

                            status[name] = api;
                        })();
                        return status[name];
                    }

                }
            })()
        });
    })();

    (function ui_event() {
        kooboo.namespace('kooboo.cms.ui.event');
        kooboo.cms.ui.event.extend({
            eventList: {},
            add: function (func, name, data, dataName) {
                name = name || "default";
                dataName = dataName || 'data';
                this.eventList[name] = this.eventList[name] || [];
                var eve = {
                    func: function (param) { return func.call(this, param); },
                    dataName: dataName
                };
                eve[dataName] = data;
                this.eventList[name].push(eve);
            },
            execute: function (name, data, param) {
                name = name || 'default';

                this.eventList[name] = this.eventList[name] || [];

                var result = true;
                this.eventList[name].each(function (fnObj) {
                    result = result && (!(fnObj.func.call(data, param, fnObj[fnObj.dataName]) == false));
                });
                return result;
            }
        });

        var ui = kooboo.cms.ui.event;
        var methods = 'ajaxSubmit,afterSubmit,onSuccess'.split(',');
        $(methods).each(function (index, fnName) {
            ui[fnName] = function (func, data, dataName) {
                if (typeof (func) == 'function') {
                    ui.add(func, fnName + 'submit', data, dataName);
                } else { //func is a param when ajax-submit execute
                    return ui.execute(fnName + 'submit', func, data);
                }
            }
        });
    })();

} (jQuery));