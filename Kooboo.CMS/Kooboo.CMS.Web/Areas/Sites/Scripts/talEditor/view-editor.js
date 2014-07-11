(function () {
    var main=function() {
        if (typeof (jQuery) == 'function') {
            window.parent.loading.hide();
            //editor
            (function (__parent__, __ctx__, __conf__) {
                (function ($) {
                    var Mask = function (target) {
                        var targetLeft = target.offset().left;
                        var targetTop = target.offset().top;
                        var targetWidth = target.outerWidth();
                        var targetRight = $(window).width() - targetLeft - targetWidth;
                        var targetHeight = target.outerHeight();
                        var targetBottom = $(window).height() - targetTop - targetHeight;
                        $('.kooboo-mask .left').css({
                            right: targetWidth + targetRight
                        });
                        $('.kooboo-mask .right').css({
                            left: targetLeft + targetWidth
                        });
                        $('.kooboo-mask .top').css({
                            left: targetLeft,
                            right: targetRight,
                            bottom: targetHeight + targetBottom
                        });
                        $('.kooboo-mask .bottom').css({
                            left: targetLeft,
                            right: targetRight,
                            top: targetTop + targetHeight
                        });
                    }
                    var MaskScroll = function (target) {
                        var scrollHeight = $(window).scrollTop();
                        var scrollWidth = $(window).scrollLeft();
                        $('.kooboo-mask .left, .kooboo-mask .right, .kooboo-mask .bottom').css({
                            bottom: -scrollHeight
                        });
                        $('.kooboo-mask .right').css({
                            right: -scrollWidth
                        });
                    }
                    $.fn.KoobooMask = function () {
                        return this.each(function () {
                            var target = $(this);
                            var koobooDiv=$("#kooboo-stuff-container");
                            if (koobooDiv.find("div.kooboo-mask").length == 0) {
                                var wrap = $('<div class="kooboo-mask">');
                                wrap.append('<div class="left"></div>');
                                wrap.append('<div class="right"></div>');
                                wrap.append('<div class="top"></div>');
                                wrap.append('<div class="bottom"></div>');
                                wrap.appendTo(koobooDiv);
                                Mask(target);
                            }
                            $(window).resize(function () {
                                Mask(target);
                            });
                            $(window).scroll(function () {
                                MaskScroll(target);
                            });
                        })
                    }
                })(jQuery);

                (function ($) {
                    var highlightPos = {};
                    var setHighlighterPos = function (selector) {
                        $(selector).show();
                        var pos = ['left', 'right', 'top', 'bottom'];
                        for (var i = 0; i < pos.length; i++) {
                            $(selector + ' .' + pos[i]).css(highlightPos[pos[i]]);
                        }
                        $(selector + ' span').css(highlightPos.span);
                    };
                    var elementHighlight = function (target, highlighterId) {
                        var borderWidth = $('#kooboo-highlight .left').width();
                        highlightPos.left = {
                            left: target.offset().left - borderWidth,
                            top: target.offset().top - borderWidth,
                            height: target.outerHeight() + borderWidth * 2
                        };
                        highlightPos.right = {
                            left: target.offset().left + target.outerWidth(),
                            top: target.offset().top - borderWidth,
                            height: target.outerHeight() + borderWidth * 2
                        };
                        highlightPos.top = {
                            left: target.offset().left - borderWidth,
                            top: target.offset().top - borderWidth,
                            width: target.outerWidth() + borderWidth * 2
                        };
                        highlightPos.bottom = {
                            left: target.offset().left - borderWidth,
                            top: target.offset().top + target.outerHeight(),
                            width: target.outerWidth() + borderWidth * 2
                        };
                        var span = $('#kooboo-highlight span');
                        var left = target.offset().left + target.outerWidth() + borderWidth;
                        if (target.outerWidth() > 300) {
                            left = left - 10;
                        }
                        highlightPos.span = {
                            left: left,
                            top: target.offset().top - borderWidth
                        };
                        setHighlighterPos('#' + highlighterId);
                    };
                    $.fn.highlight = function () {
                        elementHighlight(this, 'kooboo-highlight');
                        return this;
                    };
                    $.fn.highlightCopy = function () {
                        elementHighlight(this, 'kooboo-highlight-copy');
                        $("#kooboo-highlight").hide();
                        return this;
                    };
                })(jQuery);

                (function ($) {
                    var defaults = {
                        activeEvent: 'mouseover'
                    };
                    var codeDomTagMouseover = function ($this) {
                        for (var id in __ctx__.codeDomTags) {
                            var tag = __ctx__.codeDomTags[id];
                            var cls = 'hover';
                            if (tag && tag.is($this)) {
                                __parent__.$('div.code-viewer span.' + cls).removeClass(cls);
                                __parent__.$('span[name=' + id + ']').addClass(cls);
                                if (tag.is(__ctx__.clickedTag)) {
                                    var clickedNode = __parent__.$("#div-node-path a:last");
                                    clickedNode.addClass(cls);
                                }
                                break;
                            }
                        }
                        for (var id in __ctx__.codePathTags) {
                            if (__ctx__.codePathTags[id].is($this)) {
                                __parent__.$('a[name=' + id + ']').addClass(cls);
                                break;
                            }
                        }
                    };
                    var codeDomTagMouseout = function (target) {
                        var cls = 'hover';
                        try{
                            __parent__.$('div.code-viewer span.' + cls).removeClass(cls);
                            __parent__.$("#div-node-path a." + cls).removeClass(cls);
                        } catch (e) {
                            console.log('leave');
                        }
                    }
                    $.fn.KoobooHighlight = function (options) {
                        var options = $.extend(defaults, options);

                        return this.each(function () {

                            var wrap = $('<div class="kooboo-highlight hover" id="kooboo-highlight">');
                            wrap.append('<div class="left"></div>');
                            wrap.append('<div class="right"></div>');
                            wrap.append('<div class="top"></div>');
                            wrap.append('<div class="bottom">');
                            wrap.append("<span class='kooboo-label' style='display:none;'></span>");
                            wrap.append('</div>');
                            if ($("#kooboo-highlight").length == 0) {
                                wrap.appendTo($("#kooboo-stuff-container"));
                                var copy = wrap.clone().removeClass('hover').attr("id", "kooboo-highlight-copy");
                                copy.appendTo($("#kooboo-stuff-container"));
                            }
                            __ctx__.highlighter = $("#kooboo-highlight");
                            __ctx__.highlighterCopy = $("#kooboo-highlight-copy");

                            $(this).find('*').click(function (e) {
                                $(e.target).trigger('mouseover').highlightCopy();
                                var panelModel = new __parent__.PanelModel();
                                panelModel.elementClick(e.target);
                                e.preventDefault();
                                e.stopPropagation();
                            });
                            if ($(this).find('[data-kooboo="repeat-item"]').length == 0) {
                                $(this).find('*').live(options.activeEvent, function (e) {
                                    var $target = $(e.target);
                                    $target.highlight();
                                    e.preventDefault();
                                    e.stopPropagation();
                                    codeDomTagMouseover($target);
                                }).bind('mouseout', function (e) {
                                    $("#kooboo-highlight").hide();
                                    codeDomTagMouseout($(e.target));
                                });
                            } else {
                                $(this).find('[data-kooboo="repeat-item"] *').bind(options.activeEvent, function (e) {
                                    $(e.target).highlight();
                                });
                            }
                        });
                    };
                })(jQuery);

                (function ($) {
                    var Toolbar = $('<div class="kooboo-toolbar">');
                    var ToolbarWrap = $('<ul>');
                    Toolbar.append(ToolbarWrap);
                    var ForeachButton = $('<li><a href="javascript:;">Repeated element</a></li>');
                    var TextButton = $('<li><a href="javascript:;">Text element</a></li>');
                    ForeachButton.appendTo(ToolbarWrap);
                    TextButton.appendTo(ToolbarWrap);

                    var ToolbarTarget = null;

                    ForeachButton.click(function () {
                        if (ToolbarTarget != null) {
                            $(ToolbarTarget).attr("data-kooboo", "repeat-item");
                            alert("marked");
                        }
                    });
                    $.fn.KoobooToolbar = function () {
                        return this.each(function () {
                            Toolbar.appendTo($("#kooboo-stuff-container"));
                        })
                    }
                    $.fn.KoobooToolbar.show = function (target) {
                        ToolbarTarget = target;
                        var targetLeft = $(target).offset().left;
                        var targetTop = $(target).offset().top;
                        Toolbar.css({
                            left: targetLeft,
                            top: targetTop - Toolbar.outerHeight()
                        });
                        Toolbar.show();
                    };
                })(jQuery);
                (function ($) {
                    var CopyMask = $('<div class="kooboo-copy-mask" >');
                    CopyMaskInit = function (container) {
                        CopyMask.css({
                            left: container.offset().left,
                            right: $(window).width() - container.offset().left - container.outerWidth(),
                            top: container.offset().top + container.find('.kooboo-repeated').outerHeight(),
                            bottom: $(window).height() - container.offset().top - container.outerHeight()
                        });
                    };
                    $.fn.KoobooCopyMask = function () {
                        CopyMask.appendTo($("#kooboo-stuff-container"));
                        return this.each(function () {
                            var Container = $(this);
                            CopyMaskInit(Container);
                            $(window).resize(function () {
                                CopyMaskInit(Container);
                            });
                        });
                    };
                })(jQuery);

            })(window.parent, window.parent.__ctx__, window.parent.__conf__);

            //binding
            (function (__parent__, __ctx__, __conf__) {

                __ctx__.iframeBody = $('body');
                __ctx__.iframeObj = window;
                __ctx__.koobooStuffContainer = $("#kooboo-stuff-container");

                var initViewEditor = function () {
                    var $editorWrapper = $("#view-editor-wrapper");
                    $editorWrapper.find("*").each(function (i, o) {
                        $o = $(o);
                        if ($o.attr(__conf__.tal.content) && $.trim($o.html()) == "") {
                            $o.html(__conf__.contentHolder);
                        }
                    });
                    __ctx__.editorWrapper = $editorWrapper;
                    /*var viewContent = __ctx__.editorWrapper.html();
                    $(":not('#view-editor-wrapper')").css({
                        'cursor': 'not-allowed'
                    });*/
                    $("*").on('click', function (e) {
                        e.stopPropagation();
                        $("#kooboo-highlight-copy").hide();
                        $("#kooboo-highlight").hide();
                        //overview
                        var panelModel = new __parent__.PanelModel();
                        panelModel.initBoundList();
                    });
                    //$editorWrapper.html(viewContent).KoobooHighlight();
                    $editorWrapper.KoobooHighlight();
                    //var style = $editorWrapper.parent().attr('style');
                    $editorWrapper.KoobooMask().attr('style', 'border:1px dashed #CCCCCC;display:block;');
                    $("a").click(function () {
                        return false;
                    });
                    $(":text,textarea,input[type=search]").attr('readonly', 'readonly');
                    if (__ctx__.action != 'create') {
                        $editorWrapper.click();
                    }
                };

                var initLayoutEditor = function () {
                    $("body").KoobooHighlight();
                    $("a").click(function () {
                        return false;
                    });
                    $(":text,textarea,input[type=search]").attr('readonly', 'readonly');
                };
                $(function () {
                    initViewEditor();
                    __ctx__.initEditorHandler = initViewEditor;
                });
            })(window.parent, window.parent.__ctx__, window.parent.__conf__);
        } else {
            //waiting for jQuery object to init.
            setTimeout(main, 500);
        }
    }
    setTimeout(main, 500);
})();


