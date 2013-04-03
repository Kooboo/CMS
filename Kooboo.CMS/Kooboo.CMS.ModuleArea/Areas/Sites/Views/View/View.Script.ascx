<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%:Html.Partial("DataRule.Script") %>
<script language="javascript" type="text/javascript">
    kooboo.namespace("kooboo.cms.sites.view");
    kooboo.cms.sites.view.extend({
        init: function () {
            $("div.task-block").each(function (index) {
                var current = $(this);
                var content = current.find("div.content");

                var h3 = current.find("h3.title").css({ cursor: 'pointer' });
                h3.click(function () {
                    var span = h3.find('span');
                    if (span.hasClass('close')) {
                        span.removeClass("close");
                        content.slideDown("fast");
                    } else {
                        content.slideUp("fast");
                        span.addClass("close");
                    }
                });
                index++;
            });

            this.initTools();

            this.initDataRule();
        },
        initTools: function (option) {
            var config = {
                positionInputDisplay: '<%:"Position Name".Localize()%>',
                positionNullAlert: '<%:"Please input a new position name.".Localize()%>',
                nameRepeat: '<%:"Please input another name .".Localize()%>',
                wrongPosition: '<%:"Please choose another position".Localize()%>',
                textAreaId: "#Body",
                EditText: '<%:"Edit".Localize()%>',
                EditTitle: '<%:"Edit DataRule".Localize()%>',
                removeConfirm: '<%:"Are you sure you want to remove this item?".Localize()%>',
                itemExist: '<%:"This item is already existed!".Localize()%>'
            };

            $.extend(config, option);

            var textArea = $(config.textAreaId);
            var $form = $("div.common-form form");

            $form.find('input').keydown(function (e) {
                if (e.keyCode == 13) {
                    return false;
                }
            });

            (function initTextarea() {
                textArea.codeMirror({
                    onChange: function (e) {
                        if (e && e.historySize().undo > 0) {
                            kooboo.cms.ui.status().stop();
                        } else {
                            kooboo.cms.ui.status().pass();
                        }
                    },
                    sizeProvider: function (div) {
                        div.width($(window).width() - 500);
                    }
                });

            })();

            (function initViewTools() {
                var viewTools = $("div.viewTools");
                var hasSubLi = viewTools.find("li.has-sub");
                $('div.task-panel').find("span.arrow").toggle(function () {
                    $(this).removeClass("close").parent().next().slideDown();
                }, function () {
                    $(this).addClass("close").parent().next().slideUp();
                });

                $('div.task-block:gt(0)').find("span.arrow").each(function () {
                    $(this).addClass("close").parent().next().hide();
                })

                viewTools.find("a").click(function () {

                    var current = $(this);
                    var codeMirrorAPI = textArea.data("codeMirror");

                    var code = current.attr('title');
                    codeMirrorAPI.insertAtCursor(code);
                });

            })();

            (function initPlugins() {
                var displayUl = $("div.task-block.plugins ul.plugins");
                var mutipleDropdown = $("#Plugins");

                initDisplayList();
                function initDisplayList() {
                    displayUl.html('');
                    if (mutipleDropdown.val() != null) {
                        var valArray = mutipleDropdown.val();
                        valArray.each(function (value, index) {
                            var current = mutipleDropdown.children("option[value='" + value + "']");
                            var li = $('<li><span></span><a href="javascript:;" class="action remove right o-icon remove"></a></li>');
                            var remove = li.find("a.action").click(function () {
                                current.attr("selected", false);
                                li.remove();
                                return false;
                            });
                            li.hover(function () {
                                remove.show();
                            }, function () {
                                remove.hide();
                            });
                            li.find("span").html(current.text());
                            li.appendTo(displayUl);
                            li.data("option", current);
                        });
                    }
                }
                var addBtn = $("div.plugins a.add");

                var select = $("<select></select>");

                select.html(mutipleDropdown.html());

                mutipleDropdown.css({ position: "absolute", left: '-1000px' }).after(select);

                addBtn.click(function () {

                    var added = false;

                    var selectedOption = mutipleDropdown.children("option[value='" + select.val() + "']");

                    displayUl.children().each(function () {
                        added = added || ($(this).data("option").val() == selectedOption.val());
                    });

                    if (added) {
                        alert(config.itemExist);
                    } else {
                        kooboo.cms.ui.status().stop();
                        selectedOption.attr("selected", true);

                        initDisplayList();
                    }


                });

            })();

            kooboo.cms.sites.datarule.initFormEvent($form);

            kooboo.cms.ui.event.ajaxSubmit(function () {
                var codeMirrorAPI = textArea.data("codeMirror");
                var text = codeMirrorAPI.getText();
                textArea.val(text);
            });
        },
        initDataRule: function () {
            $("#AddDataRuleBtn").pop({
                useContent: true,
                contentId: 'DataRuleStep1Div',
                onclose: function (handle, pop, config) {
                    $(':checkbox[name="DataRule.FolderName"][checked]').removeAttr('checked');
                }
            });
            kooboo.cms.sites.datarule.step2.initDataRuleGrid();
        }
    });


    $(function () {
        $('form').submit(function () {
            kooboo.cms.ui.status().pass();
        });
        $('input,textarea,select').change(function () {
            kooboo.cms.ui.status().stop();
        });
        kooboo.cms.ui.status().bind(window, '<%:"Are you sure you want to leave current page".Localize() %>');
        kooboo.cms.sites.view.init();
    });
</script>
