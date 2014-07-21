//version:0.3

var langEnum = {
    csharp: 'csharp',
    py: 'python'
};
var dataTypeEnum = {
    label: 'Label',
    data: 'Data',
    repeater: 'RepeatedItem',
    staticImg: 'StaticImg',
    dynamicImg: 'DynamicImg',
    partial: 'Partial',
    position: 'Position',
    form:'Form',
    nothing: 'Nothing'
};
var calloutEnum = {
    Label: 'L',
    Data: 'D',
    RepeatedItem: 'R',
    Position: 'P',
    DynamicImg:'I',
    StaticImg:"I",
    Form:"F",
    Field:'P'
};

//conf
var __conf__ = {
    contentHolder: '[Data binding ...]',
    defaultOption: {
        name: typeof (defaultOptionName) != 'undefined' ? defaultOptionName : '--------',
        value: '--------'
    },
    repeatItemName: "{ds}.$Item",//ds$Item.Id
    labelMethodName: 'RawLabel',
    tmplCtxObjName: 'ViewBag',
    isStaticView: true,//no ds
    editorPosRight: 0,
    pageUrlApi: 'Url.FrontUrl().PageUrl',
    positionApi: 'Html.FrontHtml().Position',
    isLayout: __isLayout__,
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
    },
    statics: [
        { type: 'css', url: "/Areas/Sites/Scripts/talEditor/kooboo-editor.css" },
        {type:'js',url:"/Areas/Sites/Scripts/talEditor/import-lib.js"}
    ],
    resizeImageUri: ""
};
//end conf

var __ctx__ = {
    clickedTag: null,
    editorWrapper: null,
    iframeObj: null,
    initEditorHandler: null,
    isPreview: true,
    highlighter: null,
    highlighterCopy: null,
    boundTags: [],
    codeDomTags: {},
    codePathTags: {},
    calloutTags: {},
    showStaticImgsHandler: function () { },
    siteStatics: [],
    siteEnablejQuery: true,
    action:'create'
};


var __re__ = {
    url: /^(https|http):\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\:+!]*([^<>])*$/,
    imgExt:new RegExp("(.png|.gif|.icon|.jpg|.jpeg|.bmp)", 'g')
};


//utils
var __utils__ = {
    getRandomId: function (prefix) {
        var ran = String(Math.random()).replace('0.', '');
        var id = prefix + ran;
        return id;
    },
    messageFlash: function (msg, success) {
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
    unescapeHTML: function (str) {
        str = String(str).replace(/&gt;/g, '>').
            replace(/&lt;/g, '<').
            replace(/&quot;/g, '"').
            replace(/&amp;/g, '&');
        return str;
    },
    escapeHTML: function (str) {
        str = String(str).replace(/&/g, '&amp;').
            replace(/>/g, '&gt;').
            replace(/</g, '&lt;').
            replace(/"/g, '&quot;');
        return str;
    },
    getCookie:function(name){
        var re = new RegExp("(^| )"+name+"=([^;]*)(;|$)");
        var arr = document.cookie.match(re);
        if(arr != null){
            return unescape(arr[2]);
        }else{
            return null;
        }
    },
    delCookie:function(name){
        var exp = new Date();
        exp.setTime(exp.getTime() - 1);
        var val=__utils__.getCookie(name);
        if(val!=null){
            var str = name + "="+escape(val)+";expires="+exp.toGMTString();
            document.cookie= str;
        }
    }
};


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
        $obj.find('span').css(highlightPos.span);
    };
    var elementHighlight = function (target, highlighter) {
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
        var span = $('#kooboo-highlight span');
        var left = target.offset().left + target.outerWidth() + borderWidth;
        if (target.outerWidth() > 300) {
            left = left - 10;
        }
        highlightPos.span = {
            left: left,
            top: target.offset().top - borderWidth
        };
        setHighlighterPos(highlighter);
    };
    $.fn.highlight = function () {
        elementHighlight(this, __ctx__.highlighter);
        return this;
    };
    $.fn.highlightCopy = function () {
        elementHighlight(this, __ctx__.highlighterCopy);
        __ctx__.highlighter.hide();
        return this;
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
            var data = { id: __utils__.getRandomId(prefix), jqtag: $(child), tag: child };
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
        return __conf__.tmplCtxObjName + '.' + ds;
    },
    generateRepeatItem: function (ds) {
        var item = ds.replace('.', '') + this.repeatItemHolder;
        return item;
    },
    labelFlag: function () {
        return __conf__.labelMethodName + "(";
    },
    generateRepeatExpr: function (ds) {
        //'dsItem ctx.ds'
        var expr = __lang__.generateRepeatItem(ds) + ' ' + __lang__.generateDataSource(ds)
        return expr;
    },
    analysePosition: function (attrVal) {
        var re = new RegExp("('|\")", 'g');
        var posName = '';
        if (attrVal.indexOf("'") != -1) {
            posName = attrVal.split("'")[1];
        } else if (attrVal.indexOf('"')) {
            posName = attrVal.split('"')[1];
        } else {
            console.log("what the hell?");
        }
        return posName;
    },
    isStaticImg:function(attrs){
        attrs=(attrs||"").split(";");
        var is=false;
        for(var i=0;i<attrs.length;i++){
            if($.trim(attrs[i]).toLowerCase().startsWith("src ")){
                is=true;
                break;
            }
        }
        return is;
    },
    unquot:function(str){
        if($.trim(str)){
            return str.substring(1,str.length-1);
        }else{
            return str;
        }
    }

    //data field
    //repeater
};
var SharpParser = function () {
};
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
        var pageName = href.split('"')[1];
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
    },
    generatePositionExpr: function (positionName) {
        var pos = __conf__.positionApi + "(\"" + positionName + "\")";
        return pos;
    },
    quot:function(text){
        return "\""+text+"\"";
    }
};
var PyParser = function () {
};
PyParser.prototype = {
    generateLabelExpr: function (text) {
        return __conf__.tal.structure + __conf__.labelMethodName + "('" + text + "')";
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
        var re = new RegExp("('|\")", 'g');
        _.each(keyValues, function (item) {
            var s = item.split('=');
            var _name = s[0].replace(re, '');
            var _val = s[1].replace(')', '');
            params.push({ name: _name, value: _val });
        });
        return { page: pageName, params: params };
    },
    generatePositionExpr: function (positionName) {
        var pos = __conf__.positionApi + "('" + positionName + "')";
        return pos;
    },
    quot:function(text){
        return "'"+text+"'";
    }


};
if (__conf__.lang.for == langEnum.csharp) {
    __utils__.mixin(LangParser, [SharpParser]);
} else {
    __utils__.mixin(LangParser, [PyParser]);
}
var __lang__ = new LangParser();

//tal parser
var TalParser = function () {
    var self = this;
    self.tag = function () {
        return __ctx__.clickedTag;
    };
    self.isLabel = function ($tag) {
        var attr = $tag.attr(__conf__.tal.content);
        return attr && (attr.indexOf(__lang__.labelFlag()) != -1);
    };
    self.isPosition = function ($tag) {
        $tag = $tag || self.tag();
        var attr = $tag.attr(__conf__.tal.content);
        return attr && (attr.indexOf(__conf__.positionApi) != -1);
    };
    self.analyseDataType = function ($tag) {
        $tag = $tag || self.tag();
        var tagName=$tag[0].tagName.toLowerCase();
        var type = dataTypeEnum.nothing;
        if(tagName=='form'){
            if($tag.attr("name")) {
                type = dataTypeEnum.form;
            }
        }
        var attr = $tag.attr(__conf__.tal.content);
        if (attr) {
            if (self.isLabel($tag)) {//re
                type = dataTypeEnum.label;
            } else if (self.isPosition($tag)) {
                type = dataTypeEnum.position;
            } else if(tagName=='img'){
                type=dataTypeEnum.dynamicImg;
            } else{
                type = dataTypeEnum.data;
            }
        }
        attr = $tag.attr(__conf__.tal.repeat);
        if (attr) {
            type = dataTypeEnum.repeater;
        }
        attr = $tag.attr(__conf__.tal.attrs);
        if(attr){
            if($tag[0].tagName.toLowerCase() == 'img'){
                if(__lang__.isStaticImg(attr)){
                    type=dataTypeEnum.staticImg;
                }
            }
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
    self.analyseAllBinding = function (container) {
        if(!container){
            container=__ctx__.editorWrapper;
        }
        __ctx__.boundTags = [];
        var tags = container.find("*");
        $.each(tags, function () {
            var $this = $(this);
            var dataType = self.analyseDataType($this);
            if (dataType != dataTypeEnum.nothing) {
                var obj = { type: dataType, tag: $this };
                __ctx__.boundTags.push(obj);
            }
        });
    };
    self.generatePosition = function (positionName) {
        var val = __conf__.tal.structure + __lang__.generatePositionExpr(positionName);
        var talattr = { name: __conf__.tal.content, val: val };
        return talattr;
    };
    self.analysePosition = function ($tag) {
        $tag = $tag || self.tag();
        var attr = $tag.attr(__conf__.tal.content);
        if (attr && attr.indexOf(__conf__.positionApi) != -1) {
            return __lang__.analysePosition(attr);
        } else {
            return "";
        }
    };
    self.analyseImage = function ($tag) {
        $tag = $tag || self.tag();
        var attrstr=$tag.attr(__conf__.tal.attrs);
        if(__lang__.isStaticImg(attrstr)) {
            var attrs = attrstr.split(';');
            var src = '';
            _.each(attrs, function (attr) {
                if ($.trim(attr).toLowerCase().startsWith('src ')) {
                    src = $.trim(attr);
                }
            });
            if (src) {
                realsrc = __lang__.unquot(src.split(' ')[1]);
                return {type:'static',src:realsrc};
            } else {
                return {type:'nothing',src:''};
            }
        }else{
            return {type:'dynamic',src:self.analyseDataField($tag)};
        }
    };
    self.analyseForm=function($tag){
        $tag = $tag || self.tag();
        var name=$tag.attr("name");
        return "form["+name+"]";
    };
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
        $tag.html(__utils__.escapeHTML(text));
        var expr = __parser__.wrapLabel(text);
        $tag.attr(__conf__.tal.content, expr);
        return expr;
    };

    self.bindRepeater = function (datasrcName, $tag) {
        if (self.notEmptyDsName(datasrcName)) {
            $tag = $tag || self.tag();
            var attr = __conf__.tal.repeat;
            var rptExpr = __lang__.generateRepeatExpr(datasrcName);
            $tag.attr(attr, rptExpr);
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
                url = extLinkValue ? extLinkValue : "";
                if (url.indexOf("'") == -1 && url.indexOf('"') == -1) {
                    url =__lang__.quot(url);
                }
            }
            self.bindAttr("href",url,self.unbindLink,$tag);
        } else {
            self.unbindLink();
        }
    };
    self.unbindLink = function ($tag) {
        $tag=$tag||self.tag();
        self.unbindAttr("href",$tag);
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
    self.setPosition = function (name, $tag) {
        $tag = $tag || self.tag();
        var attr = __parser__.generatePosition(name);
        $tag.attr(attr.name, attr.val);
    };
    self.clearPosition = function ($tag) {
        $tag = $tag || self.tag();
        self.unbindContent($tag);
    };
    self.bindDynamicImg=function(value,$tag){
        $tag=$tag||self.tag();
        if (self.notEmptyDataField(value)) {
            $tag.attr(self.contentAttr, value);
            self.unbindStaticImg($tag);
        } else {
            self.unbindContent($tag);
        }
    };
    self.unbindDynamicImg=function($tag){
        $tag=$tag||self.tag();
        self.unbindContent($tag);
    }
    self.bindStaticImg=function(value,$tag){
        $tag=$tag||self.tag();
        self.unbindDynamicImg($tag);
        value=__lang__.quot(value);
        self.bindAttr("src",value,self.unbindStaticImg,$tag);
    };
    self.bindAttr=function(attrName,attrValue,unbindHandler,$tag){
        var attrs = $tag.attr(__conf__.tal.attrs);
        if (attrs) {
            var newattr = unbindHandler($tag);
            attrs = newattr ? (newattr + ";") : '';
        } else {
            attrs = '';
        }
        if(attrValue){
            var attr = attrName+" " + attrValue
            attrs = attrs+attr;
        }
        if(attrs) {
            $tag.attr(__conf__.tal.attrs, attrs);
        }
    };
    self.unbindStaticImg=function($tag){
        $tag=$tag||self.tag();
        self.unbindAttr('src',$tag);
    };
    self.unbindAttr=function(attrName,$tag){
        $tag = $tag || self.tag();
        var attrstr = $.trim($tag.attr(__conf__.tal.attrs));
        if (attrstr) {
            var attrs = attrstr.split(';');
            var temp = [];
            _.each(attrs, function (attr) {
                if (!$.trim(attr).startsWith(attrName)) {
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
    }
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
        return self._allDataFields;
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

