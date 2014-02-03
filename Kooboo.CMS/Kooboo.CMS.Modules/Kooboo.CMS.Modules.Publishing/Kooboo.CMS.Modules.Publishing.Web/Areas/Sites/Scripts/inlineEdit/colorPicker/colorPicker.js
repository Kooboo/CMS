/*
*
* colorPicker
* author: ronglin
* create date: 2010.06.03
*
*/

(function ($) {

    // text resource
    var options = {
        caption: 'Color Picker',
        description: 'Click on a color chip or enter a hex code value.',

        originalColor: 'Original:',
        newColor: 'New:',
        hexValue: 'Hex Value:',

        useNewColorBtn: 'Use New Color',
        cancelBtn: 'Cancel'
    };

    // override text resource
    if (window.__inlineEditVars) { $.extend(options, __inlineEditVars.colorPicker_js); }

    /*
    * color picker core
    */
    var colorPickerCore = function (config) {
        config.width = 368;
        config.height = null;
        config.buildHtml = function () {
        //config.buildFn = function () {
            return '<var class="kb-colorpicker">' +
            '<var class="kb-head"></var>' +
            '<var class="kb-bar">' +
            '<var>' + options.description + '</var>' +
            '<var class="kb-row"><var class="kb-left">' + options.originalColor + '</var><var class="kb-right" ori="1"></var></var>' +
            '<var class="kb-row"><var class="kb-left">' + options.newColor + '</var><var class="kb-right" new="1"></var></var>' +
            '<var class="kb-row"><var class="kb-left">' + options.hexValue + '</var><input type="text" maxlength="7" class="kb-text" /></var>' +
            '<var class="kb-row"><var class="kb-btn">' + options.useNewColorBtn + '</var><var class="kb-btn">' + options.cancelBtn + '</var></var>' +
            '</var>' +
            '<var class="kb-grid" ' +
            // disabled selection
            'unselectable="on" onselectstart="return false;" style="-moz-user-select: none;">' +
                '<var style="background-color: rgb(0, 0, 0);" cval="#000000"></var>' +
                '<var style="background-color: rgb(0, 0, 51);" cval="#000033"></var>' +
                '<var style="background-color: rgb(0, 0, 102);" cval="#000066"></var>' +
                '<var style="background-color: rgb(0, 0, 153);" cval="#000099"></var>' +
                '<var style="background-color: rgb(0, 0, 204);" cval="#0000CC"></var>' +
                '<var style="background-color: rgb(0, 0, 255);" cval="#0000FF"></var>' +
                '<var style="background-color: rgb(51, 0, 0);" cval="#330000"></var>' +
                '<var style="background-color: rgb(51, 0, 51);" cval="#330033"></var>' +
                '<var style="background-color: rgb(51, 0, 102);" cval="#330066"></var>' +
                '<var style="background-color: rgb(51, 0, 153);" cval="#330099"></var>' +
                '<var style="background-color: rgb(51, 0, 204);" cval="#3300CC"></var>' +
                '<var style="background-color: rgb(51, 0, 255);" cval="#3300FF"></var>' +
                '<var style="background-color: rgb(102, 0, 0);" cval="#660000"></var>' +
                '<var style="background-color: rgb(102, 0, 51);" cval="#660033"></var>' +
                '<var style="background-color: rgb(102, 0, 102);" cval="#660066"></var>' +
                '<var style="background-color: rgb(102, 0, 153);" cval="#660099"></var>' +
                '<var style="background-color: rgb(102, 0, 204);" cval="#6600CC"></var>' +
                '<var style="background-color: rgb(102, 0, 255);" cval="#6600FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(153, 0, 0);" cval="#990000"></var>' +
                '<var style="background-color: rgb(153, 0, 51);" cval="#990033"></var>' +
                '<var style="background-color: rgb(153, 0, 102);" cval="#990066"></var>' +
                '<var style="background-color: rgb(153, 0, 153);" cval="#990099"></var>' +
                '<var style="background-color: rgb(153, 0, 204);" cval="#9900CC"></var>' +
                '<var style="background-color: rgb(153, 0, 255);" cval="#9900FF"></var>' +
                '<var style="background-color: rgb(204, 0, 0);" cval="#CC0000"></var>' +
                '<var style="background-color: rgb(204, 0, 51);" cval="#CC0033"></var>' +
                '<var style="background-color: rgb(204, 0, 102);" cval="#CC0066"></var>' +
                '<var style="background-color: rgb(204, 0, 153);" cval="#CC0099"></var>' +
                '<var style="background-color: rgb(204, 0, 204);" cval="#CC00CC"></var>' +
                '<var style="background-color: rgb(204, 0, 255);" cval="#CC00FF"></var>' +
                '<var style="background-color: rgb(255, 0, 0);" cval="#FF0000"></var>' +
                '<var style="background-color: rgb(255, 0, 51);" cval="#FF0033"></var>' +
                '<var style="background-color: rgb(255, 0, 102);" cval="#FF0066"></var>' +
                '<var style="background-color: rgb(255, 0, 153);" cval="#FF0099"></var>' +
                '<var style="background-color: rgb(255, 0, 204);" cval="#FF00CC"></var>' +
                '<var style="background-color: rgb(255, 0, 255);" cval="#FF00FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(0, 51, 0);" cval="#003300"></var>' +
                '<var style="background-color: rgb(0, 51, 51);" cval="#003333"></var>' +
                '<var style="background-color: rgb(0, 51, 102);" cval="#003366"></var>' +
                '<var style="background-color: rgb(0, 51, 153);" cval="#003399"></var>' +
                '<var style="background-color: rgb(0, 51, 204);" cval="#0033CC"></var>' +
                '<var style="background-color: rgb(0, 51, 255);" cval="#0033FF"></var>' +
                '<var style="background-color: rgb(51, 51, 0);" cval="#333300"></var>' +
                '<var style="background-color: rgb(51, 51, 51);" cval="#333333"></var>' +
                '<var style="background-color: rgb(51, 51, 102);" cval="#333366"></var>' +
                '<var style="background-color: rgb(51, 51, 153);" cval="#333399"></var>' +
                '<var style="background-color: rgb(51, 51, 204);" cval="#3333CC"></var>' +
                '<var style="background-color: rgb(51, 51, 255);" cval="#3333FF"></var>' +
                '<var style="background-color: rgb(102, 51, 0);" cval="#663300"></var>' +
                '<var style="background-color: rgb(102, 51, 51);" cval="#663333"></var>' +
                '<var style="background-color: rgb(102, 51, 102);" cval="#663366"></var>' +
                '<var style="background-color: rgb(102, 51, 153);" cval="#663399"></var>' +
                '<var style="background-color: rgb(102, 51, 204);" cval="#6633CC"></var>' +
                '<var style="background-color: rgb(102, 51, 255);" cval="#6633FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(153, 51, 0);" cval="#993300"></var>' +
                '<var style="background-color: rgb(153, 51, 51);" cval="#993333"></var>' +
                '<var style="background-color: rgb(153, 51, 102);" cval="#993366"></var>' +
                '<var style="background-color: rgb(153, 51, 153);" cval="#993399"></var>' +
                '<var style="background-color: rgb(153, 51, 204);" cval="#9933CC"></var>' +
                '<var style="background-color: rgb(153, 51, 255);" cval="#9933FF"></var>' +
                '<var style="background-color: rgb(204, 51, 0);" cval="#CC3300"></var>' +
                '<var style="background-color: rgb(204, 51, 51);" cval="#CC3333"></var>' +
                '<var style="background-color: rgb(204, 51, 102);" cval="#CC3366"></var>' +
                '<var style="background-color: rgb(204, 51, 153);" cval="#CC3399"></var>' +
                '<var style="background-color: rgb(204, 51, 204);" cval="#CC33CC"></var>' +
                '<var style="background-color: rgb(204, 51, 255);" cval="#CC33FF"></var>' +
                '<var style="background-color: rgb(255, 51, 0);" cval="#FF3300"></var>' +
                '<var style="background-color: rgb(255, 51, 51);" cval="#FF3333"></var>' +
                '<var style="background-color: rgb(255, 51, 102);" cval="#FF3366"></var>' +
                '<var style="background-color: rgb(255, 51, 153);" cval="#FF3399"></var>' +
                '<var style="background-color: rgb(255, 51, 204);" cval="#FF33CC"></var>' +
                '<var style="background-color: rgb(255, 51, 255);" cval="#FF33FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(0, 102, 0);" cval="#006600"></var>' +
                '<var style="background-color: rgb(0, 102, 51);" cval="#006633"></var>' +
                '<var style="background-color: rgb(0, 102, 102);" cval="#006666"></var>' +
                '<var style="background-color: rgb(0, 102, 153);" cval="#006699"></var>' +
                '<var style="background-color: rgb(0, 102, 204);" cval="#0066CC"></var>' +
                '<var style="background-color: rgb(0, 102, 255);" cval="#0066FF"></var>' +
                '<var style="background-color: rgb(51, 102, 0);" cval="#336600"></var>' +
                '<var style="background-color: rgb(51, 102, 51);" cval="#336633"></var>' +
                '<var style="background-color: rgb(51, 102, 102);" cval="#336666"></var>' +
                '<var style="background-color: rgb(51, 102, 153);" cval="#336699"></var>' +
                '<var style="background-color: rgb(51, 102, 204);" cval="#3366CC"></var>' +
                '<var style="background-color: rgb(51, 102, 255);" cval="#3366FF"></var>' +
                '<var style="background-color: rgb(102, 102, 0);" cval="#666600"></var>' +
                '<var style="background-color: rgb(102, 102, 51);" cval="#666633"></var>' +
                '<var style="background-color: rgb(102, 102, 102);" cval="#666666"></var>' +
                '<var style="background-color: rgb(102, 102, 153);" cval="#666699"></var>' +
                '<var style="background-color: rgb(102, 102, 204);" cval="#6666CC"></var>' +
                '<var style="background-color: rgb(102, 102, 255);" cval="#6666FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(153, 102, 0);" cval="#996600"></var>' +
                '<var style="background-color: rgb(153, 102, 51);" cval="#996633"></var>' +
                '<var style="background-color: rgb(153, 102, 102);" cval="#996666"></var>' +
                '<var style="background-color: rgb(153, 102, 153);" cval="#996699"></var>' +
                '<var style="background-color: rgb(153, 102, 204);" cval="#9966CC"></var>' +
                '<var style="background-color: rgb(153, 102, 255);" cval="#9966FF"></var>' +
                '<var style="background-color: rgb(204, 102, 0);" cval="#CC6600"></var>' +
                '<var style="background-color: rgb(204, 102, 51);" cval="#CC6633"></var>' +
                '<var style="background-color: rgb(204, 102, 102);" cval="#CC6666"></var>' +
                '<var style="background-color: rgb(204, 102, 153);" cval="#CC6699"></var>' +
                '<var style="background-color: rgb(204, 102, 204);" cval="#CC66CC"></var>' +
                '<var style="background-color: rgb(204, 102, 255);" cval="#CC66FF"></var>' +
                '<var style="background-color: rgb(255, 102, 0);" cval="#FF6600"></var>' +
                '<var style="background-color: rgb(255, 102, 51);" cval="#FF6633"></var>' +
                '<var style="background-color: rgb(255, 102, 102);" cval="#FF6666"></var>' +
                '<var style="background-color: rgb(255, 102, 153);" cval="#FF6699"></var>' +
                '<var style="background-color: rgb(255, 102, 204);" cval="#FF66CC"></var>' +
                '<var style="background-color: rgb(255, 102, 255);" cval="#FF66FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(0, 153, 0);" cval="#009900"></var>' +
                '<var style="background-color: rgb(0, 153, 51);" cval="#009933"></var>' +
                '<var style="background-color: rgb(0, 153, 102);" cval="#009966"></var>' +
                '<var style="background-color: rgb(0, 153, 153);" cval="#009999"></var>' +
                '<var style="background-color: rgb(0, 153, 204);" cval="#0099CC"></var>' +
                '<var style="background-color: rgb(0, 153, 255);" cval="#0099FF"></var>' +
                '<var style="background-color: rgb(51, 153, 0);" cval="#339900"></var>' +
                '<var style="background-color: rgb(51, 153, 51);" cval="#339933"></var>' +
                '<var style="background-color: rgb(51, 153, 102);" cval="#339966"></var>' +
                '<var style="background-color: rgb(51, 153, 153);" cval="#339999"></var>' +
                '<var style="background-color: rgb(51, 153, 204);" cval="#3399CC"></var>' +
                '<var style="background-color: rgb(51, 153, 255);" cval="#3399FF"></var>' +
                '<var style="background-color: rgb(102, 153, 0);" cval="#669900"></var>' +
                '<var style="background-color: rgb(102, 153, 51);" cval="#669933"></var>' +
                '<var style="background-color: rgb(102, 153, 102);" cval="#669966"></var>' +
                '<var style="background-color: rgb(102, 153, 153);" cval="#669999"></var>' +
                '<var style="background-color: rgb(102, 153, 204);" cval="#6699CC"></var>' +
                '<var style="background-color: rgb(102, 153, 255);" cval="#6699FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(153, 153, 0);" cval="#999900"></var>' +
                '<var style="background-color: rgb(153, 153, 51);" cval="#999933"></var>' +
                '<var style="background-color: rgb(153, 153, 102);" cval="#999966"></var>' +
                '<var style="background-color: rgb(153, 153, 153);" cval="#999999"></var>' +
                '<var style="background-color: rgb(153, 153, 204);" cval="#9999CC"></var>' +
                '<var style="background-color: rgb(153, 153, 255);" cval="#9999FF"></var>' +
                '<var style="background-color: rgb(204, 153, 0);" cval="#CC9900"></var>' +
                '<var style="background-color: rgb(204, 153, 51);" cval="#CC9933"></var>' +
                '<var style="background-color: rgb(204, 153, 102);" cval="#CC9966"></var>' +
                '<var style="background-color: rgb(204, 153, 153);" cval="#CC9999"></var>' +
                '<var style="background-color: rgb(204, 153, 204);" cval="#CC99CC"></var>' +
                '<var style="background-color: rgb(204, 153, 255);" cval="#CC99FF"></var>' +
                '<var style="background-color: rgb(255, 153, 0);" cval="#FF9900"></var>' +
                '<var style="background-color: rgb(255, 153, 51);" cval="#FF9933"></var>' +
                '<var style="background-color: rgb(255, 153, 102);" cval="#FF9966"></var>' +
                '<var style="background-color: rgb(255, 153, 153);" cval="#FF9999"></var>' +
                '<var style="background-color: rgb(255, 153, 204);" cval="#FF99CC"></var>' +
                '<var style="background-color: rgb(255, 153, 255);" cval="#FF99FF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(0, 204, 0);" cval="#00CC00"></var>' +
                '<var style="background-color: rgb(0, 204, 51);" cval="#00CC33"></var>' +
                '<var style="background-color: rgb(0, 204, 102);" cval="#00CC66"></var>' +
                '<var style="background-color: rgb(0, 204, 153);" cval="#00CC99"></var>' +
                '<var style="background-color: rgb(0, 204, 204);" cval="#00CCCC"></var>' +
                '<var style="background-color: rgb(0, 204, 255);" cval="#00CCFF"></var>' +
                '<var style="background-color: rgb(51, 204, 0);" cval="#33CC00"></var>' +
                '<var style="background-color: rgb(51, 204, 51);" cval="#33CC33"></var>' +
                '<var style="background-color: rgb(51, 204, 102);" cval="#33CC66"></var>' +
                '<var style="background-color: rgb(51, 204, 153);" cval="#33CC99"></var>' +
                '<var style="background-color: rgb(51, 204, 204);" cval="#33CCCC"></var>' +
                '<var style="background-color: rgb(51, 204, 255);" cval="#33CCFF"></var>' +
                '<var style="background-color: rgb(102, 204, 0);" cval="#66CC00"></var>' +
                '<var style="background-color: rgb(102, 204, 51);" cval="#66CC33"></var>' +
                '<var style="background-color: rgb(102, 204, 102);" cval="#66CC66"></var>' +
                '<var style="background-color: rgb(102, 204, 153);" cval="#66CC99"></var>' +
                '<var style="background-color: rgb(102, 204, 204);" cval="#66CCCC"></var>' +
                '<var style="background-color: rgb(102, 204, 255);" cval="#66CCFF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(153, 204, 0);" cval="#99CC00"></var>' +
                '<var style="background-color: rgb(153, 204, 51);" cval="#99CC33"></var>' +
                '<var style="background-color: rgb(153, 204, 102);" cval="#99CC66"></var>' +
                '<var style="background-color: rgb(153, 204, 153);" cval="#99CC99"></var>' +
                '<var style="background-color: rgb(153, 204, 204);" cval="#99CCCC"></var>' +
                '<var style="background-color: rgb(153, 204, 255);" cval="#99CCFF"></var>' +
                '<var style="background-color: rgb(204, 204, 0);" cval="#CCCC00"></var>' +
                '<var style="background-color: rgb(204, 204, 51);" cval="#CCCC33"></var>' +
                '<var style="background-color: rgb(204, 204, 102);" cval="#CCCC66"></var>' +
                '<var style="background-color: rgb(204, 204, 153);" cval="#CCCC99"></var>' +
                '<var style="background-color: rgb(204, 204, 204);" cval="#CCCCCC"></var>' +
                '<var style="background-color: rgb(204, 204, 255);" cval="#CCCCFF"></var>' +
                '<var style="background-color: rgb(255, 204, 0);" cval="#FFCC00"></var>' +
                '<var style="background-color: rgb(255, 204, 51);" cval="#FFCC33"></var>' +
                '<var style="background-color: rgb(255, 204, 102);" cval="#FFCC66"></var>' +
                '<var style="background-color: rgb(255, 204, 153);" cval="#FFCC99"></var>' +
                '<var style="background-color: rgb(255, 204, 204);" cval="#FFCCCC"></var>' +
                '<var style="background-color: rgb(255, 204, 255);" cval="#FFCCFF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(0, 255, 0);" cval="#00FF00"></var>' +
                '<var style="background-color: rgb(0, 255, 51);" cval="#00FF33"></var>' +
                '<var style="background-color: rgb(0, 255, 102);" cval="#00FF66"></var>' +
                '<var style="background-color: rgb(0, 255, 153);" cval="#00FF99"></var>' +
                '<var style="background-color: rgb(0, 255, 204);" cval="#00FFCC"></var>' +
                '<var style="background-color: rgb(0, 255, 255);" cval="#00FFFF"></var>' +
                '<var style="background-color: rgb(51, 255, 0);" cval="#33FF00"></var>' +
                '<var style="background-color: rgb(51, 255, 51);" cval="#33FF33"></var>' +
                '<var style="background-color: rgb(51, 255, 102);" cval="#33FF66"></var>' +
                '<var style="background-color: rgb(51, 255, 153);" cval="#33FF99"></var>' +
                '<var style="background-color: rgb(51, 255, 204);" cval="#33FFCC"></var>' +
                '<var style="background-color: rgb(51, 255, 255);" cval="#33FFFF"></var>' +
                '<var style="background-color: rgb(102, 255, 0);" cval="#66FF00"></var>' +
                '<var style="background-color: rgb(102, 255, 51);" cval="#66FF33"></var>' +
                '<var style="background-color: rgb(102, 255, 102);" cval="#66FF66"></var>' +
                '<var style="background-color: rgb(102, 255, 153);" cval="#66FF99"></var>' +
                '<var style="background-color: rgb(102, 255, 204);" cval="#66FFCC"></var>' +
                '<var style="background-color: rgb(102, 255, 255);" cval="#66FFFF"></var>' +
            '<br/>' +
                '<var style="background-color: rgb(153, 255, 0);" cval="#99FF00"></var>' +
                '<var style="background-color: rgb(153, 255, 51);" cval="#99FF33"></var>' +
                '<var style="background-color: rgb(153, 255, 102);" cval="#99FF66"></var>' +
                '<var style="background-color: rgb(153, 255, 153);" cval="#99FF99"></var>' +
                '<var style="background-color: rgb(153, 255, 204);" cval="#99FFCC"></var>' +
                '<var style="background-color: rgb(153, 255, 255);" cval="#99FFFF"></var>' +
                '<var style="background-color: rgb(204, 255, 0);" cval="#CCFF00"></var>' +
                '<var style="background-color: rgb(204, 255, 51);" cval="#CCFF33"></var>' +
                '<var style="background-color: rgb(204, 255, 102);" cval="#CCFF66"></var>' +
                '<var style="background-color: rgb(204, 255, 153);" cval="#CCFF99"></var>' +
                '<var style="background-color: rgb(204, 255, 204);" cval="#CCFFCC"></var>' +
                '<var style="background-color: rgb(204, 255, 255);" cval="#CCFFFF"></var>' +
                '<var style="background-color: rgb(255, 255, 0);" cval="#FFFF00"></var>' +
                '<var style="background-color: rgb(255, 255, 51);" cval="#FFFF33"></var>' +
                '<var style="background-color: rgb(255, 255, 102);" cval="#FFFF66"></var>' +
                '<var style="background-color: rgb(255, 255, 153);" cval="#FFFF99"></var>' +
                '<var style="background-color: rgb(255, 255, 204);" cval="#FFFFCC"></var>' +
                '<var style="background-color: rgb(255, 255, 255);" cval="#FFFFFF"></var>' +
            '</var>' +
            '</var>';
        };
        colorPickerCore.superclass.constructor.call(this, config);
    };

    yardi.extend(colorPickerCore, yardi.pickerPanel, {

        caption: options.caption,

        defaultColor: 'transparent',

        onSelect: null,

        onPreview: null,

        onCancel: null,

        bindEvents: function () {
            colorPickerCore.superclass.bindEvents.call(this);
            var self = this;
            // caption
            var head = this.el.find('.kb-head');
            if (this.caption) {
                head.html(this.caption);
            } else {
                head.remove();
            }
            // text
            var text = this.el.find('input[type="text"]');
            var newColor = this.el.find('var[new="1"]');
            var original = this.el.find('var[ori="1"]');
            var last_hl_td;
            this.el.find('.kb-grid').click(function (ev) {
                var cval = $(ev.target).attr('cval');
                if (cval) {
                    text.val(cval);
                    newColor.css('backgroundColor', cval).attr('cval', cval);
                    if (self.onPreview) { self.onPreview(cval, ev); }
                }
            }).dblclick(function (ev) {
                var cval = $(ev.target).attr('cval');
                if (cval) { btnOk.click(); }
            }).mousemove(function (ev) {
                setTimeout(function () {
                    var cval = $(ev.target).attr('cval');
                    if (cval) {
                        if (last_hl_td != ev.target) {
                            $(ev.target).addClass('hl');
                            $(last_hl_td).removeClass('hl');
                            last_hl_td = ev.target;
                        }
                    } else {
                        $(last_hl_td).removeClass('hl');
                    }
                }, yardi.isIE ? 50 : 100);
            });
            original.css('cursor', 'pointer').click(function (ev) {
                var val = $(this).attr('cval');
                self.setColor(val);
                self.onPreview && self.onPreview(val, ev);
            });
            text.change(function (ev) {
                //if (this.value.length == 7 || this.value.length == 4) {
                try {
                    newColor.css('backgroundColor', this.value).attr('cval', this.value);
                    self.onPreview && self.onPreview(this.value, ev);
                } catch (ex) { }
                //}
            });
            // btns
            var btns = this.el.find('.kb-btn');
            btns.hover(function () {
                $(this).addClass('kb-btn-hl');
            }, function () {
                $(this).removeClass('kb-btn-hl');
            });
            var btnOk = btns.eq(0);
            var btnCancel = btns.eq(1);
            btnOk.click(function (ev) {
                self.onSelect(newColor.attr('cval'), ev);
                colorPickerCore.superclass.hide.call(self);
            });
            btnCancel.click(function (ev) {
                colorPickerCore.superclass.hide.call(self);
                self.onCancel && self.onCancel(original.attr('cval'), ev);
            });
        },

        setColor: function (cval) {
            if (cval) {
                cval = yardi.colorHex(cval);
                this.el.find('input[type="text"]').val(cval);
                this.el.find('var[new="1"]').css('backgroundColor', cval).attr('cval', cval);
                this.el.find('var[ori="1"]').css('backgroundColor', cval).attr('cval', cval);
            }
        },

        hide: function () {
            colorPickerCore.superclass.hide.call(this);
            if (this.onCancel) {
                var original = this.el.find('var[ori="1"]');
                this.onCancel(original.attr('cval'), null);
            }
        },

        show: function (refEl, color) {
            colorPickerCore.superclass.show.call(this, refEl);
            this.setColor(color || this.defaultColor);
        }
    });

    var colorPicker = function () {
        var _upper = function (c) { return (c || '').toUpperCase(); };
        var select, preview, cancel;
        var picker = new colorPickerCore({
            onSelect: function (color, ev) { if (select) select(_upper(color), ev); },
            onPreview: function (color, ev) { if (preview) preview(_upper(color), ev); },
            onCancel: function (color, ev) { if (cancel) cancel(_upper(color), ev); }
        });
        return {
            show: function (refEl, color, refSelect, refPreview, refCancel) {
                picker.show(refEl, _upper(color));
                select = refSelect;
                preview = refPreview;
                cancel = refCancel;
            },
            hide: function () {
                picker.hide();
                select = null;
                preview = null;
                cancel = null;
            }
        };
    } ();

    // register
    yardi.colorPicker = colorPicker;

})(jQuery);
