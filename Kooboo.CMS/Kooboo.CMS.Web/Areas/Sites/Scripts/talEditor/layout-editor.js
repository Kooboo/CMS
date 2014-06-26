
//editor
(function (__parent__, __ctx__, __conf__) {

    (function ($) {
        var defaults = {
            activeEvent: 'mouseover'
        };
        var highlightPos = {};
        var setHighlighterPos = function (selector) {
            $(selector).show();
            var pos = ['left', 'right', 'top', 'bottom'];
            for (var i = 0; i < pos.length; i++) {
                $(selector + ' .' + pos[i]).css(highlightPos[pos[i]]);
            }
            $(selector + ' span').css(highlightPos.span);
        };
        var ElementHighlight = function (target) {
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
            //alert(span.width());
            //alert(span.outerWidth());
            var left = target.offset().left + target.outerWidth() + borderWidth;
            if (target.outerWidth() > 300) {
                left = left - 10;
            }
            highlightPos.span = {
                left: left,
                top: target.offset().top - borderWidth
                //width: highlightPos.width
            };
            setHighlighterPos('#kooboo-highlight');
        };
        var fixHighlighterCopy = function () {
            setHighlighterPos('#kooboo-highlight-copy');
            $("#kooboo-highlight").hide();
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
            __parent__.$('div.code-viewer span.' + cls).removeClass(cls);
            __parent__.$("#div-node-path a." + cls).removeClass(cls);
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
                    $(e.target).trigger('mouseover');
                    fixHighlighterCopy();
                    var panelModel = new __parent__.PanelModel();
                    panelModel.elementClick(e.target);
                    e.preventDefault();
                    e.stopPropagation();
                });
                if ($(this).find('[data-kooboo="repeat-item"]').length == 0) {
                    $(this).find('*').live(options.activeEvent, function (e) {
                        var $target = $(e.target);
                        ElementHighlight($target);
                        e.preventDefault();
                        e.stopPropagation();
                        //codeDomTagMouseover($target);
                    }).bind('mouseout', function (e) {
                        $("#kooboo-highlight").hide();
                        //codeDomTagMouseout($(e.target));
                    });
                } else {
                    $(this).find('[data-kooboo="repeat-item"] *').bind(options.activeEvent, function (e) {
                        ElementHighlight($(e.target));
                    });
                }
            });
        };
    })(jQuery);

})(window.parent, window.parent.__ctx__, window.parent.__conf__);

//binding
(function (__parent__, __ctx__, __conf__) {

    __ctx__.iframeBody = $('body');
    __ctx__.iframeObj = window;
    __ctx__.koobooStuffContainer = $("#kooboo-stuff-container");
    //var $editorWrapper = $("#view-editor-wrapper");
    __ctx__.editorWrapper = $("body");

    var initLayoutEditor = function () {
        $("body").KoobooHighlight();
        //$("a").click(function () {
        //    return false;
        //});
        //$(":text,textarea,input[type=search]").attr('readonly', 'readonly');
    }
    $("body").on('click', function (e) {
        e.stopPropagation();
        $("#kooboo-highlight-copy").hide();
        $("#kooboo-highlight").hide();
        __parent__.$("#span-clear-clicked").trigger('click');
        //overview;
    });
    $(function () {
            initLayoutEditor();
            __ctx__.initEditorHandler = initLayoutEditor;
    });

})(window.parent, window.parent.__ctx__, window.parent.__conf__);

function setIframeContent(html) {
    var stuff = $("#kooboo-stuff-container").clone()[0].outerHTML;
    var re = new RegExp("(<html>|<HTML>|</html>|</HTML>)", 'g');
    html = html.replace(re, "");
    window.document.documentElement.innerHTML = html;
    var body = window.document.body.innerHTML;
    window.document.body.innerHTML = body + stuff;
}