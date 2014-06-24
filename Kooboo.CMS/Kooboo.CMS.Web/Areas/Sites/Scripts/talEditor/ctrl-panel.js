//version:0.2

//conf
var __ctx__ = {
    clickedTag: null,
    editorWrapper: null,
    iframeBody: null,
    initEditorHandler: null,
    isPreview: true,
    highlighter: null,
    highlighterCopy: null,
    boundTags: [],
    codeDomTags: {},
    codePathTags: {},
    calloutTags: {}
};

var langEnum = {
    csharp: 'csharp',
    py: 'python'
};

var __conf__ = {
    contentHolder: '[Data binding ...]',
    defaultOption: { name: '--------', value: '--------' },
    repeatItemName: "{ds}.$Item",//ds$Item.Id
    labelMethodName: 'RawLabel',
    tmplCtxObjName: 'ViewBag',//ViewBag
    isStaticView: true,//no ds
    editorPosRight: 0,
    pageUrlApi: 'Url.FrontUrl().PageUrl',
    tal: {
        define: 'tal:define',
        switch: 'tal:switch',
        case: 'tal:case',
        condition: 'tal:condition',
        repeat: 'tal:repeat',
        content: 'tal:content',
        replace: 'tal:replace',
        omit: 'tal:omit-tag',
        attrs: 'tal:attributes',
        error: 'tal:on-error',
        string: 'string ',
        structure: 'structure ',
        prefix: 'tal:'
    },
    lang: {
        for: langEnum.csharp
    }
};

var __re__ = {
    url: /^(https|http):\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\:+!]*([^<>])*$/
};

var dataTypeEnum = {
    label: 'Label',
    data: 'Data',
    repeater: 'RepeatedItem',
    staticImg: 'StaticImg',
    dynamicImg: 'DynamicImg',
    partial: 'Partial',
    nothing: 'Nothing'
};
var calloutEnum = {
    Label: 'L',
    Data: 'D',
    RepeatedItem: 'R'
};

//utils
var utils = {
    getRandomId: function (prefix) {
        var ran = String(Math.random()).replace('0.', '');
        var id = prefix + ran;
        return id;
    },
    messageFlash: function (msg,success) {
        window.info.show(msg, success);
    },
    t: function (str) {
        return str;
    },
    mixin: function (cls, mixins) {
        var prototype = cls.prototype;
        _.each(mixins, function (mx) {
            if (mx) {
                var _prototype = mx.prototype;
                for (var p in _prototype) {
                    prototype[p] = _prototype[p];
                }
            }
        });
        return cls;
    },
    unescapeHTML:function (str) {
        str = String(str).replace(/&gt;/g, '>').
        replace(/&lt;/g, '<').
        replace(/&quot;/g, '"').
        replace(/&amp;/g, '&');
        return str;
    },
    escapeHTML:function (str) {
        str = String(str).replace(/&/g, '&amp;').
        replace(/>/g, '&gt;').
        replace(/</g, '&lt;').
        replace(/"/g, '&quot;');
        return str;
    }
};
//end conf

//ext
(function ($) {
    $.fn.applyBindings = function (viewModel) {
        ko.applyBindings(viewModel, this[0]);
    };
})(jQuery);

(function ($) {
    var highlightPos = {};
    var setHighlighterPos = function ($obj) {
        $obj.show();
        var pos = ['left', 'right', 'top', 'bottom'];
        for (var i = 0; i < pos.length; i++) {
            var div = $obj.find('.' + pos[i]);
            div.css(highlightPos[pos[i]]);
        }
    };
    var elementHighlight = function (target) {
        var borderWidth = __ctx__.highlighter.find('.left').width();
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
        setHighlighterPos(__ctx__.highlighter);
    };
    $.fn.highlight = function () {
        elementHighlight(this);
        return $(this);
    };
})(jQuery);

(function ($) {
    $.fn.xchildren = function (selector) {
        selector = selector || '>';
        var children = [];
        if (__parser__.isLabel($(this))) {
            //return children;
        }
        var prefix = 'code-node-';
        _.each(this.find(selector), function (child) {
            var data = { id: utils.getRandomId(prefix), jqtag: $(child), tag: child };
            __ctx__.codeDomTags[data.id] = data.jqtag;
            children.push(data);
        });
        return children;
    };
})(jQuery);

_.mixin({
    removeItem: function (list, item) {//rename
        var arr = [];
        _.each(list, function (e) {
            if (!_.isEqual(e, item)) {
                arr.push(e);
            }
        });
        return arr;
    },
    hasItem: function (list, item) {
        var has = false;
        for (var i = 0; i < list.length; i++) {
            if (list[i].is) {
                if (list[i].is(item)) {
                    has = true;
                    break;
                }
            } else {
                if (_.isEqual(list[i], item)) {
                    has = true;
                    break;
                }
            }
        }
        return has;
    }
});

(function () {
    if (typeof String.prototype.startsWith != 'function') {
        String.prototype.startsWith = function (str) {
            return this.slice(0, str.length) == str;
        };
    }
    if (typeof String.prototype.endsWith != 'function') {
        String.prototype.endsWith = function (str) {
            return this.slice(-str.length) == str;
        };
    }
})();
// end ext

//language parser
var LangParser = function () {
    this.repeatItemHolder = 'Item';
};
LangParser.prototype = {
    formatFieldName: function (ds, field) {
        //list: ds.$item.Id,ds$Item.Id
        //object: ds.Id,ctx.ds.Id
        var name = ds.name + "." + field;
        var val = __conf__.tmplCtxObjName + "." + ds.name + "." + field;
        if (ds.islist) {
            var holder = this.repeatItemHolder;
            name = ds.name + ".$" + holder + "." + field;
            val = __lang__.generateRepeatItem(ds.name) + "." + field;
        }
        return { 'fullName': name, 'name': field, 'value': val, 'ds': ds.name };
    },
    generateDataSource: function (ds) {
        return __conf__.tmplCtxObjName + '.'+ds;
    },
    generateRepeatItem:function(ds){
        var item=ds.replace('.','') + this.repeatItemHolder;
        return item;
    },
    labelFlag: function () {
        return __conf__.labelMethodName + "(";
    },
    generateRepeatExpr:function (ds) {
        //'dsItem ctx.ds'
        var expr= __lang__.generateRepeatItem(ds) + ' ' + __lang__.generateDataSource(ds)
        return expr;
    }
    //data field
    //repeater
};
var SharpParser = function () { };
SharpParser.prototype = {
    generateLabelExpr: function (text) {
        return __conf__.tal.structure + "\"" + text + "\"." + __conf__.labelMethodName + "()";
    },
    generatePageLink: function (page, params) {
        var paramString = [];
        _.each(params, function (p) {
            if (p.value) {
                paramString.push(p.name + "=" + p.value);
            }
        });
        var _paramstr = "";
        if (paramString.length > 0) {
            _paramstr = ",new {" + paramString.join(',') + "}";
        }
        var pageUrl = __conf__.pageUrlApi + "(\"" + page + "\"" + _paramstr + ")";
        return pageUrl;
    },
    analyseLink: function (href) {
        var pageName = '';
        if (href.indexOf("'") != -1) {
            pageName = href.split("'")[1];
        } else if (href.indexOf('"')) {
            pageName = href.split('"')[1];
        } else {
            console.log("what the hell?");
        }
        var start = href.indexOf("{") + 1;
        var end = href.indexOf("}");
        var keyValues = href.substring(start, end).split(',');
        var params = [];
        _.each(keyValues, function (item) {
            var s = item.split('=');
            var name = $.trim(s[0]);
            var val = $.trim(s[1]);
            params.push({ name: name, value: val });
        });
        return { page: pageName, params: params };
    }
};
var PyParser = function () { };
PyParser.prototype = {
    generateLabelExpr: function () {
        return __conf__.labelMethodName + "('" + labelText + "')";
    },
    generatePageLink: function (page, params) {
        var paramString = [];
        _.each(params, function (p) {
            if (p.value) {
                paramString.push("'" + p.name + "'=" + p.value);
            }
        });
        var _paramstr = paramString.length > 0 ? "," + paramString.join(',') : "";
        var pageUrl = __conf__.pageUrlApi + "('" + page + "'" + _paramstr + ")";
        return pageUrl;
    },
    analyseLink: function (href) {
        var re = new RegExp("('|\")", 'g');
        var pageName = '';
        if (href.indexOf("'") != -1) {
            pageName = href.split("'")[1];
        } else if (href.indexOf('"')) {
            pageName = href.split('"')[1];
        } else {
            console.log("what the hell?");
        }
        var keyValues = href.split(',').slice(1);
        var params = [];
        _.each(keyValues, function (item) {
            var s = item.split('=');
            var _name = s[0].replace(re, '');
            var _val = s[1].replace(')', '');
            params.push({ name: _name, value: _val });
        });
        return { page: pageName, params: params };
    }

};
if (__conf__.lang.for == langEnum.csharp) {
    utils.mixin(LangParser, [SharpParser]);
} else {
    utils.mixin(LangParser, [PyParser]);
}
var __lang__=new LangParser();

//tal parser
var TalParser = function () {
    var self = this;
    self.tag = function () {
        return __ctx__.clickedTag;
    };
    self.isLabel = function ($tag) {
        var attr = $tag.attr(__conf__.tal.content);
        return attr && (attr.indexOf(__lang__.labelFlag()) != -1);//re
    }
    self.analyseDataType = function ($tag) {
        $tag = $tag || self.tag();
        var type = dataTypeEnum.nothing;
        var attr = $tag.attr(__conf__.tal.content);
        if (attr) {
            if (self.isLabel($tag)) {//re
                type = dataTypeEnum.label;
            } else {
                type = dataTypeEnum.data;
            }
        }
        attr = $tag.attr(__conf__.tal.repeat);
        if (attr) {
            type = dataTypeEnum.repeater;
        }
        if ($tag[0].tagName.toLowerCase() == 'img') {
            //static
            //dynamic
        }
        return type;
    };
    self.wrapLabel = function (labelText) {
        labelText = labelText || tag.html();
        var expr = __lang__.generateLabelExpr(labelText);//__conf__.tal.string+
        return expr;
    };

    self.analyseDataSource = function ($tag) {
        $tag = $tag || self.tag();
        var repeat = $tag.attr(__conf__.tal.repeat);
        if (repeat) {
            var ds = repeat.split('.');
            return ds.slice(1).join('.');
        } else {
            return '';
        }
    };
    self.analyseDataField = function ($tag) {
        $tag = $tag || self.tag();
        var attr = $.trim($tag.attr(__conf__.tal.content));
        if (attr.startsWith(__conf__.tal.structure)) {
            attr = attr.replace(__conf__.tal.structure, '');
        }
        if (attr.startsWith(__conf__.tal.string)) {
            attr = attr.replace(__conf__.tal.string, '');
        }
        return attr || '';
    };
    self.analyseLink = function ($tag) {
        $tag = $tag || self.tag();
        var attrstr = $tag.attr(__conf__.tal.attrs);
        if (attrstr) {
            var attrs = attrstr.split(';');
            var href = '';
            _.each(attrs, function (attr) {
                if ($.trim(attr).startsWith('href')) {
                    href = $.trim(attr);
                }
            });
            if (href) {
                if (href.indexOf(__conf__.pageUrlApi) != -1) {
                    var ret = __lang__.analyseLink(href);
                    return { isext: false, name: ret.page, params: ret.params };
                } else {
                    var re = new RegExp("('|\")", 'g');
                    var result = $.trim(href.replace('href', ''));
                    result = result.replace(re, '');
                    return { isext: true, name: result };//ext link
                }
            } else {
                return '';
            }
        } else {
            return '';
        }
    };
    self.hasBoundDataValue = function ($tag) {
        var attrs = $tag[0].attributes;
        var is = false;
        for (var i = 0; i < attrs.length; i++) {
            var name = attrs[i].name;
            var val = attrs[i].value;
            if (val && name.startsWith(__conf__.tal.prefix)
                && name != __conf__.tal.repeat) {
                is = true;
                break;
            }
        }
        if ($tag.children().length == 0
            && $tag.text().indexOf("${") != -1) {//todo:re
            is = true;
        }
        return is;
    };
    self.hasBoundRepeater = function ($tag) {
        var attrs = $tag[0].attributes;
        var is = false;
        for (var i = 0; i < attrs.length; i++) {
            var name = attrs[i].name;
            if (name == __conf__.tal.repeat) {
                is = true;
                break;
            }
        }
        return is;
    };
    self.analyseAllBinding = function () {
        __ctx__.boundTags = [];
        var tags = __ctx__.editorWrapper.find("*");
        $.each(tags, function () {
            var $this = $(this);
            var dataType = self.analyseDataType($this);
            if (dataType != dataTypeEnum.nothing) {
                var obj = { type: dataType, tag: $this };
                __ctx__.boundTags.push(obj);
            }
        });
    }
}
var __parser__ = new TalParser();


//binder
var TalBinder = function () {
    var self = this;
    self.contentAttr = __conf__.tal.content;
    self.repeatAttr = __conf__.tal.repeat;
    self.tag = function () {
        return __ctx__.clickedTag;
    };
    self.notEmptyDsName = function (name) {
        return name && name != __conf__.defaultOption.name;
    };
    self.notEmptyDataField = function (field) {
        return field && field != __conf__.defaultOption.value;
    };
    self.bindData = function (value) {
        var $tag = self.tag();
        if (self.notEmptyDataField(value)) {
            value = __conf__.tal.structure + value;
            $tag.attr(self.contentAttr, value);
        } else {
            if (!__parser__.isLabel($tag)) {
                self.unbindData();
            }
        }
    };
    self.unbindContent = function ($tag) {
        $tag = $tag || self.tag();
        $tag.removeAttr(self.contentAttr);
    };
    self.unbindData = function () {
        self.unbindContent(self.tag());
    };
    self.clearLabel = function () {
        self.unbindContent();
    };
    self.setLabel = function (text, $tag) {
        if (!text && !$tag) {
            self.clearLabel();
            return;
        }
        $tag = $tag || self.tag();
        text = text || $tag.html();
        $tag.html(utils.escapeHTML(text));
        var expr = __parser__.wrapLabel(text);
        $tag.attr(__conf__.tal.content, expr);
        return expr;
    };

    self.bindRepeater = function (datasrcName, $tag) {
        if (self.notEmptyDsName(datasrcName)) {
            $tag = $tag || self.tag();
            var attr = __conf__.tal.repeat;
            var rptExpr = __lang__.generateRepeatExpr(datasrcName);
            $tag.attr(attr,rptExpr);
        } else {
            self.unbindRepeater();
        }
    };
    self.unbindRepeater = function ($tag) {
        $tag = $tag || self.tag();
        $tag.removeAttr(__conf__.tal.repeat);
    };
    self.bindLink = function (page, params, extLinkValue, $tag) {
        if (page != __conf__.defaultOption.name) {
            $tag = $tag || self.tag();
            var url = __lang__.generatePageLink(page, params);
            if (page == externalLink) {
                url = extLinkValue ? extLinkValue : "#";
                if (url.indexOf("'") == -1 && url.indexOf('"') == -1) {
                    url = "'" + url + "'";
                }
            }
            var href = "href " + url;
            var attrs = $tag.attr(__conf__.tal.attrs);
            if (attrs) {
                var newattr = self.unbindLink($tag);
                attrs = newattr ? (newattr + ";") : '';
            } else {
                attrs = '';
            }
            $tag.attr(__conf__.tal.attrs, attrs + href);
        } else {
            self.unbindLink();
        }
    };
    self.unbindLink = function ($tag) {
        $tag = $tag || self.tag();
        var attrstr = $.trim($tag.attr(__conf__.tal.attrs));
        if (attrstr) {
            var attrs = attrstr.split(';');
            var temp = [];
            _.each(attrs, function (attr) {
                if (!$.trim(attr).startsWith('href')) {
                    temp.push(attr);
                }
            })
            var newattr = temp.join(';');
            $tag.attr(__conf__.tal.attrs, newattr);
            if (!$.trim($tag.attr(__conf__.tal.attrs))) {
                $tag.removeAttr(__conf__.tal.attrs);
            }
            return newattr;
        } else {
            return '';
        }
    };
    self.unbindAll = function ($tag) {
        $tag = $tag || self.tag();
        var attrs = $tag.clone()[0].attributes;
        for (var i = 0; i < attrs.length; i++) {
            if (attrs[i].name.indexOf(__conf__.tal.prefix) != -1) {
                $tag.removeAttr(attrs[i].name);
            }
        }
    };
};
var __binder__ = new TalBinder();

var DataSet = function () {
    var self = this;
    self._allNames = [];
    self._allDataFields = [];
    self.allNames = function () {
        return self._allNames;
    };
    self.allDataFields = function () {
        return self._allDataFields
    };
    self.init = function (dataSources) {
        if (dataSources) {
            __datasrc__ = dataSources;
        }
        self._allNames = [];
        self._allDataFields = [];
        var dsNames = [];
        _.each(__datasrc__, function (ds) {
            self._allNames.push(ds.name);
            self.prepareDataFields(ds);
        });
        return self;
    };
    self.prepareDataFields = function (ds) {
        _.each(ds.fields, function (f) {
            var field = __lang__.formatFieldName(ds, f);
            self._allDataFields.push(field);
        });
    };
    self.add = function (ds) {
        __datasrc__.push(ds);
        self._allNames.push(ds.name);
        self.prepareDataFields(ds);
    };
    self.remove = function (dsname) {
        var temp = [];
        self._allNames = [];
        self._allDataFields = [];
        _.each(__datasrc__, function (ds) {
            if (ds.name != dsname) {
                temp.push(ds);
                self._allNames.push(ds.name);
                self.prepareDataFields(ds);
            }
        });
        __datasrc__ = temp;
    };
};
var __dataset__ = new DataSet();

var PageSet = function () {
    var self = this;
};

__ctx__.clickedTag = __ctx__.editorWrapper;
var PanelModel = function () {
    //base
    var self = this;
    self.tag = function () {
        return __ctx__.clickedTag;
    };
    self.pagesForSelect = _.union(
        [
            {
                name: __conf__.defaultOption.name,
                external: false,
                params: []
            }
        ],
        __pages__,
        [
            {
                name: externalLink,
                external: true,
                params: []
            }
        ]
    );
    self.boundTags = ko.observableArray(__ctx__.boundTags);
    self.clickedTag = ko.observable(__ctx__.clickedTag);
    self.wrappedRepeater = ko.observable(null);
    self.isLinkTag = ko.observable(false);
    self.isImgTag = ko.observable(false);
    self.hasChildren = ko.observable(false);
    self._hasChildren = function ($tag) {
        $tag = $tag || self.tag();
        if ($tag) {
            return $tag.children().length > 0;
        } else {
            return false;
        }
    };
    self.hasClickedTag = ko.observable(false);
    self.isClickedTag = function (tag) {
        return tag.is(__ctx__.clickedTag);
    };
    self.resetBoundTags = function () {
        __parser__.analyseAllBinding();
        self.boundTags(__ctx__.boundTags);
    };

    //groups
    self.dataSource = {
        availableDataSources: ko.observableArray([]),
        chosenDataSource: ko.observable(__conf__.defaultOption.name),
        fillDataSource:function(datasrc){
            __datasrc__ = datasrc;
            $("#span-fill-ds")[0].click();
        },
        _fillDataSource: function (data,event) {
            __dataset__.init();
            self.dataSource.availableDataSources(__dataset__.allNames());
            self.dataItem.dataFields(__dataset__.allDataFields());
        },
        change: function (data, event) {
            var target = $(event.target);
            var dsName = target.val();
            self.dataSource.chosenDataSource(dsName);
        },
        add: function (data, event) {
            var testDs = {
                'name': 'ds',
                'fields': ['field1', 'field2', 'field3']
            }
            __dataset__.add(testDs);
            self.dataItem.dataFields(__dataset__.allDataFields());
            self.dataSource.availableDataSources(__dataset__.allNames());
        },
        remove: function (data, event) {
            var link = $(event.target).parent();
            var toDel = link.attr('ds');
            __dataset__.remove(toDel);
            self.dataSource.availableDataSources(self.prepareDsForSelect());
            self.dataItem.dataFields(self.prepareDataFieldForSelect());
        },
        chooseFirst: function () {
            self.dataSource.chosenDataSource(__conf__.defaultOption.name);
        },
        init: function () {
            var ds = __parser__.analyseDataSource();
            if (ds) {
                self.dataSource.chosenDataSource(ds);
            } else {
                self.dataSource.chooseFirst();
            }
        },
        getWrappedRepeater: function ($tag) {
            var ds = null;
            var temp = $tag || self.tag();
            while (true) {
                if (!temp || temp.is(__ctx__.editorWrapper)) {
                    break;
                }
                var attr = temp.attr(__conf__.tal.repeat);
                if (attr) {
                    ds = __parser__.analyseDataSource(temp);
                    break
                } else {
                    temp = temp.parent();
                }
            }
            return ds;
        }
    };

    self.dataItem = {
        dataContent: ko.observable('content'),
        dataContentOuter: ko.observable('content'),
        dataType: ko.observable(dataTypeEnum.nothing),
        dataFields: ko.observableArray([]),
        chosenField: ko.observable(__conf__.defaultOption.value),
        chosenPartialField: ko.observable(''),
        chooseFirst: function () {
            self.dataItem.chosenField(__conf__.defaultOption.value);
            $('select[flag=datafield]').val(self.dataItem.chosenField());
        },
        chooseThis: function (val) {
            self.dataItem.chosenField(val);
            $('select[flag=datafield]').val(val);
        },
        initData: function () {
            var field = __parser__.analyseDataField();
            if (field) {
                self.dataItem.chooseThis(field);
            } else {
                self.dataItem.chooseFirst();
            }
        },
        initLabel: function (isinit) {
            if (self.hasChildren()) {
                if (isinit) {
                    self.dataItem.dataContent(__ctx__.clickedTag.html());
                } else {
                    if (confirm(__msgs__.convert_to_label_confirm)) {
                        self.dataItem.dataContent(__ctx__.clickedTag.html());
                    } else {
                        __ctx__.clickedTag.click();
                    }
                }
            } else {
                self.dataItem.dataContent(__ctx__.clickedTag.html());
            }
        },
        dataTypeChange: function (data, event) {
            var isinit = false;
            self.dataItem.setDataType($(event.target).attr('value'), isinit);
        },
        setDataType: function (dataType, isinit) {
            self.dataItem.dataType(dataType);
            var radio = $("input[flag='data-type'][value=" + dataType + "]");
            radio.prop('checked', true);
            switch (dataType) {
                case dataTypeEnum.label:
                    self.dataItem.initLabel(isinit);
                    break;
                case dataTypeEnum.data:
                    self.dataItem.initData();
                    break;
                case dataTypeEnum.repeater:
                    self.dataSource.init();
                    break;
                case dataTypeEnum.staticImg:
                    break;
                case dataTypeEnum.dynamicImg:
                    break;
                case dataTypeEnum.partial:
                    break;
                case dataTypeEnum.nothing:
                    break;
                default:
                    break;
            }
        },
        labelTextChange: function (data, event) {
            var text = event.target.value;
            self.dataItem.dataContent(text);
        },
        dataFieldChange: function (data, event) {
            self.dataItem.chooseThis($(event.target).val());
        }
    };

    self.linkTo = {
        pages: ko.observableArray(self.pagesForSelect),
        chosenPage: ko.observable(__conf__.defaultOption.name),
        parameters: ko.observableArray([]),
        chosenParams: [],
        extLinkValue: ko.observable(''),
        isVisible: ko.computed(function () {
            var show = self.isLinkTag();
            var type = self.dataItem.dataType();
            show = show && (type == dataTypeEnum.label || type == dataTypeEnum.data);
            return show;
        }),
        chooseFirst: function () {
            self.linkTo.chosenPage(__conf__.defaultOption.name);
            $('select[paramname]').val(__conf__.defaultOption.value);
        },
        chooseThis: function (pageName) {
            self.linkTo.chosenPage(pageName);
            var page = _.find(self.pagesForSelect, function (p) {
                return p.name == pageName;
            });
            self.linkTo.parameters(page.params);
        },
        init: function () {
            self.linkTo.chooseFirst();
            self.linkTo.extLinkValue('');
            self.linkTo.chosenParams = [];
            var types = [dataTypeEnum.label, dataTypeEnum.data, dataTypeEnum.nothing];
            if (_.include(types, self.dataItem.dataType())) {
                var page = __parser__.analyseLink();
                if (page) {
                    if (page.isext) {
                        self.linkTo.chooseThis(externalLink);
                        self.linkTo.extLinkValue(page.name);
                    } else {
                        self.linkTo.chooseThis(page.name);
                        self.linkTo.extLinkValue('');
                        //set page params
                        _.each(page.params, function (p) {
                            $('select[paramname=' + p.name + ']').val(p.value);
                        });
                        self.linkTo.chosenParams = page.params;
                    }
                }
            }
        },
        pageChange: function (data, event) {
            var name = $(event.target).val();
            self.linkTo.chooseThis(name);
            self.linkTo.extLinkValue('');
        },
        pageParamChange: function (data, event) {
            var target = $(event.target);
            var paramName = target.attr('paramname');
            var params = _.filter(self.linkTo.chosenParams, function (p) { return p.name != paramName });
            if (target.val() != __conf__.defaultOption.value) {
                params.push({ name: paramName, value: target.val() });
            }
            self.linkTo.chosenParams = params;
        },
        extLinkChange: function (data, event) {
            var val = $.trim($(event.target).val());
            if (!__re__.url.test(val)) {
                self.linkTo.extLinkValue(val);
                utils.messageFlash(__msgs__.url_invalid,false);
            }
            self.linkTo.extLinkValue(val);
        },
        bindLink: function () {
            if (self.linkTo.isVisible()) {
                if (self.linkTo.chosenPage() == externalLink && !__re__.url.test(self.linkTo.extLinkValue())) {
                    utils.messageFlash(__msgs__.url_invalid,false);
                    return false;
                }
                var extLink = self.linkTo.extLinkValue();
                __binder__.bindLink(self.linkTo.chosenPage(), self.linkTo.chosenParams, extLink);
            }
            return true;
        }
    };

    self.codeDom = {
        itemClick: function (data, event) {
            //$("div.code-viewer").find("span.active").removeClass('active');
            //$(event.target).addClass('active');
            var tag = data.tag || data.jqtag || data[0];
            tag.click();
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
                if (temp.is(__ctx__.editorWrapper)) {
                    break;
                } else {
                    var name = utils.getRandomId('code-path-');
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

    //tag click events
    self.elementClick = function (tag) {
        var $tag = $(tag);
        __ctx__.clickedTag = $tag;
        $("#tab-data-binding").trigger('click');
        $("#span-main-process").trigger('click');
    };

    self.mainProcess = function (data, event) {
        //init
        self.hasClickedTag(true);
        var tag = __ctx__.clickedTag;
        __ctx__.codeDomTags = { 'code-node-top': tag };
        self.clickedTag(tag);
        self.wrappedRepeater(self.dataSource.getWrappedRepeater());
        self.isImgTag(tag && tag[0].tagName.toLowerCase() == 'img');
        self.hasChildren(self._hasChildren());
        var islink = (tag[0].tagName.toLowerCase() === 'a');
        self.isLinkTag(islink);
        self.dataItem.dataContent(utils.unescapeHTML(tag.html()));
        self.dataItem.dataContentOuter(tag[0].outerHTML);
        $("#label-textarea").autosize().trigger('autosize.resize');
        //data type
        var dataType = __parser__.analyseDataType(tag);
        dataType = dataType || dataTypeEnum.nothing;
        self.dataItem.dataType(dataType);
        self.dataItem.setDataType(dataType, true);
        //link to
        self.linkTo.init();
        //render list
        self.resetBoundTags();
    };

    self.clearProcess = function (data, event) {
        __ctx__.clickedTag = null;
        self.clickedTag(null);
        self.hasClickedTag(false);
        //data binding overview
        $("#tab-data-binding").click();
        $("#div-repeat-item-setting").show();
        self.resetBoundTags();
    };

    //edit events
    self.cancelEdit = function (data, event) {
        __ctx__.editorWrapper[0].click();
    };

    self.displayCallout = function (show) {
        var id = utils.getRandomId('callout-');
        for (var _id in __ctx__.calloutTags) {
            var temp = __ctx__.calloutTags[_id];
            if (temp.is(__ctx__.clickedTag)) {
                id = _id;
                break;
            }
        }
        var callout = __ctx__.iframeBody.find('#' + id);
        if (show) {
            var text = calloutEnum[self.dataItem.dataType()];

            if (callout.length == 0) {
                callout = __ctx__.highlighterCopy.clone().addClass('mark').attr('id', id);
            }
            callout.find('span').show().text(text);
            callout.show().appendTo(__ctx__.iframeBody);
            __ctx__.calloutTags[id] = __ctx__.clickedTag;
        } else {
            callout.remove();
            delete __ctx__.calloutTags[id];
        }
    };

    self.saveBindings = function () {
        switch (self.dataItem.dataType()) {
            case dataTypeEnum.label:
                if (!self.linkTo.bindLink()) { return; }
                __binder__.unbindRepeater();
                __binder__.setLabel(self.dataItem.dataContent());
                utils.messageFlash(__msgs__.save_binding_success,true);
                var showCallout = true;
                if (!self.dataItem.dataContent()) {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.data:
                if (!self.linkTo.bindLink()) { return; }
                __binder__.unbindRepeater();
                __binder__.bindData(self.dataItem.chosenField());
                utils.messageFlash(__msgs__.save_binding_success,true);
                var showCallout = true;
                if (self.dataItem.chosenField() == __conf__.defaultOption.value &&
                    self.linkTo.chosenPage() == __conf__.defaultOption.name) {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.repeater:
                __binder__.unbindContent();
                __binder__.bindRepeater(self.dataSource.chosenDataSource());
                utils.messageFlash(__msgs__.save_binding_success,true);
                var showCallout = true;
                if (self.dataSource.chosenDataSource() == __conf__.defaultOption.name) {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.staticImg:
                break;
            case dataTypeEnum.dynamicImg:
                break;
            case dataTypeEnum.partial:
                break;
            case dataTypeEnum.nothing:
                break;
        };
    };

    //list events
    self.removeDataBinding = function (data, event) {
        if (confirm(__msgs__.remove_data_binding_confrim)) {
            __binder__.unbindAll(data.tag);
            if (data.tag.is(__ctx__.clickedTag)) {
                self.dataItem.dataType(dataTypeEnum.nothing);
                $("#data-type-nothing").prop('checked', true);
            }
            self.resetBoundTags();
        }
    };

    self.editListItem = function (data, event) {
        data.tag[0].click();
    };
};



