/// <reference path="../jquery-1.4.1-vsdoc.js" />
/// <reference path="../jQueryUI/jquery-ui-1.8.4.all.min.js" />
(function () {
    function innerLineEditor(obj, option) {
        var config = {
            url: '',
            postData: {},
            dataName: '',
            button: true,
            autoExpand: false,
            maxHeight: 200,
            minHeight: 40,
            triggerSelector: null,
            submitSelector: null,
            cancelSelector: null,
            append: 'after', // 'appendAfter','appendPre'
            saveText: 'Save',
            cancelText: "Cancel",
            cls: 'innerLineEditor',
            error: 'Failed to save changes!',
            oninit: function () { },
            beforeinit: function () { },
            onsave: function () { },
            beforesave: function () { },
            oncancel: function () { },
            beforecancel: function () { }
        };
        $.extend(config, option);
        var current = $(obj);

        if (config.triggerSelector != null) {
            var trigger = $(config.triggerSelector);
            trigger.click(function (event) {
                event.stopPropagation();
                config.beforeinit();
                toInput();
                config.oninit();
                return false;
            });
        } else {
            current.click(function () {
                toInput();
            });
        }

        function toInput() {
            if (current.data("status") == "edit") {
                return false;
            }
            current.data("status", "edit");
            var text = config.autoExpand ? current.text() : current.text().trim();

            current.html('');

            var id = "input_" + Math.random().toString().replace('.', '_');

            var inputStr = '<div>' + (config.autoExpand ? '<textarea class="editor left" style="width:80%;" inputField></textarea>' : '<input type="text" class="editor" inputField />') + (config.button ? '<a href="javascript:;" class="o-icon save inline-action"></a><a href="javascript:;" class="o-icon cancel inline-action"></a>' : '') + '</div>';



            var span = $(inputStr).addClass(config.cls);
            var input = span.find("[inputField]").attr("id", id)
			.val(text).click(function () {
			    var current = $(this);

			    setTimeout(function () { current.focus(); }, 100);
			    return false;
			});

            if (config.autoExpand) {
                input.keyup(function () {
                    var adjustedHeight = $(input).attr('clientHeight');
                    var scrollHeight = $(input).attr('scrollHeight');

                    if (adjustedHeight == scrollHeight) {
                        scrollHeight = scrollHeight - 10;
                        if (scrollHeight < config.minHeight) {
                            scrollHeight = config.minHeight;
                        }
                    }

                    if (scrollHeight >= config.maxHeight) {
                        scrollHeight = config.maxHeight;
                    }
                    $(input).height(scrollHeight);
                });
            }

            setTimeout(function () { input.focus(); }, 100);
            try { input[0].select(); }
            catch (E) { }
            input.keydown(function (event) {
                if (event.keyCode == 13 && !config.autoExpand) {
                    submit();
                    return false;
                }
            });

            var $save = span.find("a.save").click(function () {
                submit();
            }).html(config.saveText);
            var $cancel = span.find("a.cancel").click(function () {
                cancel();
            }).html(config.cancelText);

            //current.after(input);

            current[config.append](span);

            api.element.input = input;
            api.element.text = text;
        }

        function toDisplay(txt) {
            current.removeData("status");
            var text = txt || api.element.input.val();
            current.text(text);
            api.element.input.parent().remove();
            api.element.input = null;
            api.element.text = null;
        }

        function cancel() {
            toDisplay(api.element.text);
            config.oncancel();
        }

        function submit() {
            var val = api.element.input.val();

            if (val && val.trim().length > 0) {
                config.postData[config.dataName] = val;
            } else {
                return false;
            }
            $.ajax({
                url: config.url,
                type: "POST",
                data: config.postData,
                beforeSend: function () {
                    window.loading.show();
                },
                success: function (response) {
                    //window.loading.hide();
                    if (response.Success) {
                        toDisplay();
                        config.onsave(response);
                    } else {
                        kooboo.cms.ui.messageBox().showResponse(response);
                    }
                },
                error: function () {
                    alert(config.error);
                    cancel();
                },
                dataType: "json"
            });
        }

        var api = {
            toInput: function () {
                toInput();
            },
            submit: function () {
                submit();
            },
            cancel: function () {
                cancel();
            },
            element: {
                input: $(""),
                saveBtn: $(""),
                cancelBtn: $(""),
                text: ""
            }
        };
        current.data("innerLineEditor", api);
        return api;
    }
    $.fn.inlineEditor = function (option) {
        this.each(function () {
            var current = $(this);
            var api = innerLineEditor(current, option);
            current.data("innerLineEditor", api);
        });
        return this;
    }

})();