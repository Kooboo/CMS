__ctx__.clickedTag = __ctx__.editorWrapper;
console.log( __ctx__.editorWrapper);
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
        __parser__.analyseAllBinding();
        self.boundTags(__ctx__.boundTags);
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
                    break;
                case dataTypeEnum.dynamicImg:
                    break;
                case dataTypeEnum.partial:
                    break;
                case dataTypeEnum.position:
                    self.position.init();
                    break;
                case dataTypeEnum.nothing:
                    break;
                default:
                    break;
            }
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
                if (temp.is(__ctx__.iframeObj.$("body"))) {
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

    self.position = {
        existedPositions: ko.observableArray([]),
        init: function () {
            var pos = __parser__.analysePosition();
            $("#txt-postion-name").val(pos);
        },
        getPositions: function () {

        },
        _getPositions: function () {

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
        _.delay(function(){
            self.clickedTag(tag);
        },200);
        var dataType = __parser__.analyseDataType(tag);
        dataType = dataType || dataTypeEnum.nothing;
        self.dataItem.dataType(dataType);
        self.dataItem.setDataType(dataType, true);
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

    self.displayCallout = function (show, $tag) {
        $tag = $tag || self.tag();
        var id = utils.getRandomId('callout-');
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
                utils.messageFlash(__msgs__.save_binding_success, true);
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
                utils.messageFlash(__msgs__.save_binding_success, true);
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
                utils.messageFlash(__msgs__.save_binding_success, true);
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
            case dataTypeEnum.position:
                var pos = $.trim($("#txt-postion-name").val());
                if (!pos) {
                    utils.messageFlash(__msgs__.not_empty, false);
                } else if (_.hasItem(self.position.existedPositions(), pos)) {
                    utils.messageFlash(__msgs__.position_existed, false);
                } else {
                    __binder__.setPosition(pos);
                    self.position.existedPositions.push(pos);
                    self.displayCallout(true);
                    __ctx__.editorWrapper[0].click();
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
            if (__conf__.isLayout) {
                var posName = $(event.target).closest("li").find("span").html();
                var temp = _.removeItem(self.position.existedPositions(), posName);
                self.position.existedPositions(temp);
            } else {
                /* if (data.tag.is(__ctx__.clickedTag)) {
                     self.dataItem.dataType(dataTypeEnum.nothing);
                     $("#data-type-nothing").prop('checked', true);
                 }*/
            }
            if (data.tag.is(__ctx__.clickedTag)) {
                __ctx__.clickedTag[0].click();
            }
            self.displayCallout(false, data.tag);
            self.resetBoundTags();
        }
    };

    self.editListItem = function (data, event) {
        data.tag[0].click();
    };
};



