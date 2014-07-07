//version:0.2

__ctx__.clickedTag = __ctx__.editorWrapper;
__pages__ = typeof(__pages__)=='undefined'?[]:__pages__;
externalLink = typeof(externalLink )=='undefined'?"External Link ":externalLink;
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
    self.clickedTag=ko.observable(null);
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
        fillDataSource: function (datasrc) {
            __datasrc__ = datasrc;
            $("#span-fill-ds")[0].click();
        },
        _fillDataSource: function (data, event) {
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
            };
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
            $("#txt-postion-name").val("");
        },
        setDataType: function (dataType, isinit) {
            self.dataItem.dataType(dataType);
            var radio = $("[flag='data-type'][value=" + dataType + "]");
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
                    self.image.init();
                    break;
                case dataTypeEnum.dynamicImg:
                    self.image.init();
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
            var params = _.filter(self.linkTo.chosenParams, function (p) {
                return p.name != paramName
            });
            if (target.val() != __conf__.defaultOption.value) {
                params.push({ name: paramName, value: target.val() });
            }
            self.linkTo.chosenParams = params;
        },
        extLinkChange: function (data, event) {
            var val = $.trim($(event.target).val());
            if (!__re__.url.test(val)) {
                self.linkTo.extLinkValue(val);
                __utils__.messageFlash(__msgs__.url_invalid, false);
            }
            self.linkTo.extLinkValue(val);
        },
        bindLink: function () {
            if (self.linkTo.isVisible()) {
                if (self.linkTo.chosenPage() == externalLink && !__re__.url.test(self.linkTo.extLinkValue())) {
                    __utils__.messageFlash(__msgs__.url_invalid, false);
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

    self.image = {
        boundStaticImg: ko.observable(""),
        thumbnailSrc:  ko.observable(""),
        init:function(){
            var img = __parser__.analyseImage();
            if(img.type=='static'){
                self.image.boundStaticImg(img.src);
                self.image.thumbnailSrc(self.image.getThumbnailSrc(img.src));
            }else{
                self.image.boundStaticImg("");
                self.image.thumbnailSrc("");
            }
            if(img.type=="dynamic"){
                if (img.src) {
                    self.dataItem.chooseThis(img.src);
                } else {
                    self.dataItem.chooseFirst();
                }
            }else{
                self.dataItem.chooseFirst();
            }
        },
        getThumbnailSrc:function(src){
            return __conf__.resizeImageUri.replace("{url}", src);
        },
        showStaticImgs:function(data,event){
            __ctx__.showStaticImgsHandler();
        },
        unbindStaticImg:function(data,event){
            __binder__.unbindStaticImg();
            self.image.boundStaticImg("");
        },
        saveTempStaticImg:function(src,text){
            var data = { src: src, text: text};
            $("#save-static-img-temp").data("img",data)[0].click();
        },
        _saveTempStaticImg:function(data,event){
            var data = $(event.target).data("img");
            self.image.boundStaticImg(data.src);
            var tsrc = self.image.getThumbnailSrc(data.src);
            self.image.thumbnailSrc(tsrc);
        },
        deleteTempStaticImg: function (data,event) {
            self.image.boundStaticImg("");
            self.image.thumbnailSrc("");
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
        self.hasClickedTag(true);
        var tag = __ctx__.clickedTag;
        __ctx__.codeDomTags = { 'code-node-top': tag };
        self.clickedTag(tag);
        //view editor
        self.wrappedRepeater(self.dataSource.getWrappedRepeater());
        self.isImgTag(tag && tag[0].tagName.toLowerCase() == 'img');
        self.hasChildren(self._hasChildren());
        var islink = (tag[0].tagName.toLowerCase() === 'a');
        self.isLinkTag(islink);
        self.dataItem.dataContent(__utils__.unescapeHTML(tag.html()));
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

    self.initBoundList = function () {
        $("#span-clear-clicked").trigger('click');
    }

    //edit events
    self.cancelEdit = function (data, event) {
        __ctx__.editorWrapper[0].click();
    };

    self.displayCallout = function (show,$tag) {
        $tag=$tag||self.tag();
        var id = __utils__.getRandomId('callout-');
        for (var _id in __ctx__.calloutTags) {
            var temp = __ctx__.calloutTags[_id];
            if (temp.is($tag)) {
                id = _id;
                break;
            }
        }
        var callout = __ctx__.iframeBody.find('#' + id);
        if (show) {
            var text = calloutEnum[self.dataItem.dataType()];

            if (callout.length == 0) {
                callout = __ctx__.highlighterCopy.clone().addClass('mark').attr('id', id)
            }
            callout.find('span').show().text(text);
            callout.show().appendTo(__ctx__.koobooStuffContainer);
            __ctx__.calloutTags[id] = $tag;
        } else {
            callout.remove();
            delete __ctx__.calloutTags[id];
        }
    };

    self.saveBindings = function () {
        switch (self.dataItem.dataType()) {
            case dataTypeEnum.label:
                if (!self.linkTo.bindLink()) {
                    return;
                }
                __binder__.unbindRepeater();
                __binder__.setLabel(self.dataItem.dataContent());
                __utils__.messageFlash(__msgs__.save_binding_success, true);
                var showCallout = true;
                if (!self.dataItem.dataContent()) {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.data:
                if (!self.linkTo.bindLink()) {
                    return;
                }
                __binder__.unbindRepeater();
                __binder__.bindData(self.dataItem.chosenField());
                __utils__.messageFlash(__msgs__.save_binding_success, true);
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
                __utils__.messageFlash(__msgs__.save_binding_success, true);
                var showCallout = true;
                if (self.dataSource.chosenDataSource() == __conf__.defaultOption.name) {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.staticImg:
                __binder__.bindStaticImg(self.image.boundStaticImg());
                __utils__.messageFlash(__msgs__.save_binding_success, true);
                var showCallout = true;
                if (self.image.boundStaticImg()=="") {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.dynamicImg:
                __binder__.bindDynamicImg(self.dataItem.chosenField());
                __utils__.messageFlash(__msgs__.save_binding_success, true);
                var showCallout = true;
                if (self.dataItem.chosenField() == __conf__.defaultOption.value) {
                    showCallout = false;
                }
                self.displayCallout(showCallout);
                __ctx__.editorWrapper[0].click();
                break;
            case dataTypeEnum.partial:
                break;
            case dataTypeEnum.nothing:
                break;
        }

    };

    //list events
    self.removeDataBinding = function (data, event) {
        if (confirm(__msgs__.remove_data_binding_confrim)) {
            __binder__.unbindAll(data.tag);
            if (data.tag.is(__ctx__.clickedTag)) {
                __ctx__.clickedTag[0].click();
            }
            self.displayCallout(false,data.tag);
            self.resetBoundTags();
        }
    };

    self.editListItem = function (data, event) {
        data.tag[0].click();
    };
};



