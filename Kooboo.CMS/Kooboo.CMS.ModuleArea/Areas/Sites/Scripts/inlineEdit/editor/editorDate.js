/*
*
* editorDate
* author: ronglin
* create date: 2012.3.5
*
*/

(function ($) {

    var editorDate = function (config) {
        editorDate.superclass.constructor.call(this, config);
    };

    yardi.extend(editorDate, yardi.editorPlain, {

        pickerHolder: null, positionMonitor: null, pickerEnable: true, _posKeydown: null,

        initialize: function () {
            var self = this;
            editorDate.superclass.initialize.call(this);
            // create holder
            this.pickerHolder = $('<div class="kb-editordate-pickerholder"></div>').appendTo('body');
            yardi.zTop(this.pickerHolder);
            this.positionPicker();
            // bind datepicker
            this.pickerHolder.datepicker({
                onSelect: function (text) {
                    self.setHtml(text);
                }
            });
            // monitor holder's position
            this.positionMonitor = new yardi.monitor({
                handler: function () { self.positionPicker(); },
                interval: 200, tester: function () { return true; }
            }).start();
            // sync position when keydown
            this.el.keydown(this._posKeydown = function () {
                setTimeout(function () {
                    self.positionPicker();
                }, 0);
            });
        },

        positionPicker: function () {
            var css = this.el.offset();
            css.left += this.el.outerWidth();
            this.pickerHolder.css(css);
        },

        setEnable: function (enable) {
            editorDate.superclass.setEnable.call(this, enable);
            // set datepicker mask
            this.pickerEnable = (enable !== false);
            if (this.pickerEnable) {
                this.pickerHolder.children('.kb-pickerholder-mask').remove();
            } else {
                this.pickerHolder.append('<div class="kb-pickerholder-mask"></div>');
            }
        },

        setHtml: function (str) {
            //if (!this.pickerEnable) { return; }
            editorDate.superclass.setHtml.call(this, str);
            this.positionPicker();
        },

        remove: function () {
            editorDate.superclass.initialize.call(this);
            this.el.unbind('keydown', this._posKeydown);
            this.positionMonitor.stop();
            this.pickerHolder.remove();
        }

    });

    // register
    yardi.editorDate = editorDate;

})(jQuery);
