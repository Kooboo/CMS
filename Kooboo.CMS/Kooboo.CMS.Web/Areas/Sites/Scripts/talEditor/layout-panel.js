
var PanelModel = function () {
    //base
    var self = this;
    self.tag = function () {
        return __ctx__.clickedTag;
    };
    self.boundTags = ko.observableArray(__ctx__.boundTags);
    self.clickedTag = ko.observable(null);
    self.hasClickedTag = ko.observable(false);
    self.isClickedTag = function (tag) {
        return tag.is(__ctx__.clickedTag);
    };
    self.resetBoundTags = function () {
        var container = __ctx__.iframeObj.$("body");
        __parser__.analyseAllBinding(container);
        self.boundTags(__ctx__.boundTags);
        self.position.getPositions();
    };

    //groups
    self.dataItem = {
        dataContent: ko.observable('content'),
        dataContentOuter: ko.observable('content'),
        dataType: ko.observable(dataTypeEnum.nothing),
        dataTypeChange: function (data, event) {
            var isinit = false;
            self.dataItem.setDataType($(event.target).attr('value'), isinit);
            $("#txt-postion-name").val("");
        },
        setDataType: function (dataType, isinit) {
            self.dataItem.dataType(dataType);
            switch (dataType) {
                case dataTypeEnum.position:
                    self.position.init();
                    break;
                case dataTypeEnum.nothing:
                    break;
            }
        }
    };

    self.codeDom = {
        itemClick: function (data, event) {
            var tag = data.tag || data.jqtag || data[0];
            if (!self.clickedTag().is(tag)) {
                tag.click();
            }
        },
        itemHover: function (data, event) {
            var tag = data.jqtag || data.clickedTag();
            var name = $(event.target).attr("name");
            $.each($("span[name=" + name + "]"), function () {
                var $this = $(this);
                var cls = 'hover';
                var clickedNode = $("#div-node-path a:last");
                if ($this.hasClass(cls)) {
                    $this.removeClass(cls);
                    __ctx__.highlighter.hide();
                    if (self.isClickedTag(tag)) {
                        clickedNode.removeClass(cls);
                    }
                } else {
                    $this.addClass(cls);
                    tag.highlight();
                    if (self.isClickedTag(tag)) {
                        clickedNode.addClass(cls);
                    }
                }
            });
        },
        pathHover: function (data, event) {
            var $this = $(event.target);
            var tag = data.jqtag;
            var cls = 'hover';
            var clickedNode = $("span[name=code-node-top]");
            if ($this.hasClass('ahl')) {
                $this.removeClass('ahl');
                __ctx__.highlighter.hide();
                if (self.isClickedTag(tag)) {
                    clickedNode.removeClass(cls);
                }
            } else {
                $this.addClass('ahl');
                tag.highlight();
                if (self.isClickedTag(tag)) {
                    clickedNode.addClass(cls);
                }
            }
        },
        markupStart: function (tag) {
            var name = '<' + tag.tagName.toLowerCase();
            _.each(['id', 'name', 'class'], function (item) {
                var attr = $.trim($(tag).attr(item));
                if (attr) {
                    name += ' ' + item + '="' + attr + '"';
                }
            });
            name += ">";
            return name;
        },
        markupEnd: function (tag) {
            var name = '</' + tag.tagName + '>';
            return name.toLowerCase();
        },
        markup: function (tag) {
            var singles = ['input', 'img', 'hr'];
            var lower = tag.tagName.toLowerCase();
            var name;
            if (_.include(singles, lower)) {
                name = '<' + lower + ' />';
            } else {
                name = '<' + lower + '>...</' + lower + '>';
            }
            return name;
        },
        parents: function ($tag) {
            var temp = $tag;
            var parents = [];
            __ctx__.codePathTags = {};
            while (temp) {
                if (temp.is(__ctx__.iframeObj.$("body"))) {
                    break;
                } else {
                    var name = __utils__.getRandomId('code-path-');
                    parents.push({ name: name, jqtag: temp });
                    __ctx__.codePathTags[name] = temp;
                    temp = temp.parent();
                }
            }
            return parents.reverse();
        },
        fullTagName: function ($tag) {
            var name = $tag[0].tagName.toLowerCase();
            var id = $.trim($tag.attr('id'));
            var _name = $.trim($tag.attr('name'));
            if (id) {
                name = name + '#' + id;
            } else if (_name) {
                name = name + '[' + _name + ']';
            } else {
                var cls = $.trim($tag.attr('class'));
                if (cls) {
                    name = name + "." + cls.split(' ')[0];
                }
            }
            return name;
        }
    };

    self.position = {
        existedPositions: ko.observableArray([]),
        init: function () {
            var pos = __parser__.analysePosition();
            $("#txt-postion-name").val(pos);
        },
        getPositions: function () {
            var temp=[];
            _.each(self.boundTags(),function(obj){
                if(obj.type==dataTypeEnum.position){
                    var pos = __parser__.analysePosition(obj.tag);
                    if(pos){
                        temp.push(pos);
                    }
                }
            });
            self.position.existedPositions(temp);
        }
    };

    self.initCallout = function () {
        _.each(self.boundTags(), function (obj) {
            if (obj.type == dataTypeEnum.position) {
                obj.tag.highlight().highlightCopy();
                __ctx__.highlighterCopy.hide();
                self.displayCallout(true, obj.tag, obj.type);
            }
        });
    };

    //tag click events
    self.elementClick = function (tag) {
        var $tag = $(tag);
        __ctx__.clickedTag = $tag;
        $("#span-main-process").trigger('click');
    };

    self.mainProcess = function (data, event) {
        var tag = __ctx__.clickedTag;
        /*var koobooId = "#kooboo-stuff-container";
        if (tag.parent(koobooId).length > 0) {
            return;
        } else {
            if (tag.parent()) {
                if (tag.parent().parent(koobooId).length > 0);
                return;
            }
        }*/
        self.hasClickedTag(true);
        __ctx__.codeDomTags = { 'code-node-top': tag };
        self.clickedTag(tag);
        var dataType = __parser__.analyseDataType(tag);
        dataType = dataType || dataTypeEnum.nothing;
        self.dataItem.dataType(dataType);
        self.dataItem.setDataType(dataType, true);
        self.resetBoundTags();
    };

    self.clearProcess = function (data, event) {
        __ctx__.clickedTag = null;
        self.clickedTag(null);
        self.hasClickedTag(false);
        //data binding overview
        self.resetBoundTags();
        self.initCallout();
    };

    self.initBoundList = function () {
        $("#span-clear-clicked").trigger('click');
    };

    //edit events
    self.cancelEdit = function (data, event) {
        __ctx__.iframeObj.document.documentElement.click();
    };

    self.displayCallout = function (show, $tag,dataType) {
        $tag = $tag || self.tag();
        var id = __utils__.getRandomId('callout-');
        for (var _id in __ctx__.calloutTags) {
            var temp = __ctx__.calloutTags[_id];
            if (temp.is($tag)) {
                id = _id;
                break;
            }
        }
        var callout = __ctx__.iframeObj.$('#' + id);
        if (show) {
            var text = calloutEnum[dataType||self.dataItem.dataType()];
            if (callout.length == 0) {
                callout = __ctx__.highlighterCopy.clone().addClass('mark').attr('id', id)
            }
            callout.find('span').show().text(text);
            callout.show().appendTo(__ctx__.iframeObj.$("#kooboo-stuff-container"));
            __ctx__.calloutTags[id] = $tag;
        } else {
            callout.remove();
            delete __ctx__.calloutTags[id];
        }
    };

    self.saveBindings = function () {
        switch (self.dataItem.dataType()) {
            case dataTypeEnum.position:
                var pos = $.trim($("#txt-postion-name").val());
                if (!pos) {
                    __utils__.messageFlash(__msgs__.not_empty, false);
                } else if (_.hasItem(self.position.existedPositions(), pos)) {
                    __utils__.messageFlash(__msgs__.position_existed, false);
                } else {
                    __binder__.setPosition(pos);
                    self.position.existedPositions.push(pos);
                    self.displayCallout(true);
                    __ctx__.iframeObj.document.documentElement.click();
                }
                break;
            case dataTypeEnum.nothing:
                break;
        }
    };

    //list events
    self.removeDataBinding = function (data, event) {
        if (confirm(__msgs__.remove_data_binding_confrim)) {
            __binder__.unbindAll(data.tag);
            var posName = $(event.target).closest("li").find("span").html();
            var temp = _.removeItem(self.position.existedPositions(), posName);
            self.position.existedPositions(temp);
            if (data.tag.is(__ctx__.clickedTag)) {
                __ctx__.clickedTag[0].click();
            }
            self.displayCallout(false, data.tag);
            self.resetBoundTags();
            __ctx__.iframeObj.document.documentElement.click();
        }
    };

    self.editListItem = function (data, event) {
        data.tag[0].click();
    };
};


var __iframe__ = {
    init: function (selector,hideLoadHandler) {
        if (!__ctx__.iframeObj) {
            __ctx__.iframeObj = $(selector)[0].contentWindow;
        }
        $(selector).load(function () {
            var defs = __utils__.getCookie("docdef");
            if (defs) {
                if (__conf__.lang.for == langEnum.py) {
                    defs = __lang__.unquot(defs);
                }
                __iframe__.defs=defs+"\n";
                __utils__.delCookie("docdef");
            }
            __iframe__.loaded(this, hideLoadHandler);
        });
    },
    isLoaded:function(){
        var koobooDiv = __ctx__.iframeObj.document.getElementById("kooboo-stuff-container");
        if (__ctx__.iframeObj.$ && koobooDiv) {
            return true;
        } else {
            return false;
        }
    },
    loaded: function (iframe, callback) {
        var win, body;
        if (__ctx__.iframeObj) {
            win = __ctx__.iframeObj;
        } else {
            win = iframe.contentWindow;
        }
        body = win.document.body;
        var div = win.document.createElement('div');
        div.setAttribute('id', 'kooboo-stuff-container');
        body.appendChild(div);
        var statics = _.union(__ctx__.siteStatics, __conf__.statics);
        _.each(statics, function (s) {
            if (s.type == 'css') {
                var css = win.document.createElement("link");
                css.setAttribute('type', 'text/css');
                css.setAttribute('rel', 'Stylesheet');
                css.setAttribute('href', s.url);
                div.appendChild(css);
            } else if (s.type == 'js') {
                var js = win.document.createElement("script");
                js.src = s.url;
                div.appendChild(js);
            }
        });
        if (callback) {
            callback.call();
        }
    },
    getKoobooStuff: function () {
        var koobooDiv = __ctx__.iframeObj.$("#kooboo-stuff-container");
        var stuff = koobooDiv[0].outerHTML;
        return stuff;
    },
    defs: "",
    getDef: function (code) {
        //doctype and defs
        var edges = code.search(/<html/i);
        if (edges != -1) {
            __iframe__.defs = code.substring(0, edges-1)+"\n";
        }
    },
    getHtml: function () {
        var html = "";
        html += __ctx__.iframeObj.document.documentElement.outerHTML;
        var stuff = __iframe__.getKoobooStuff();
        html = __iframe__.defs + html.replace(stuff, "");
        return html;
    },
    setHtml: function (html) {
        __iframe__.getDef(html);
        var stuff = __iframe__.getKoobooStuff();
        var re=/(<!DOCTYPE[^>]*>)|(<[\s]*html[^>]*>)|(<[\s]*\/[\s]*html[\s]*>)/gi;
        html = html.replace(re, "");
        var doc = __ctx__.iframeObj.document;
        doc.documentElement.innerHTML = html;
        var bodyInner = doc.body.innerHTML;
        doc.body.innerHTML = bodyInner + stuff;
    }
}



