/*
*   number
*   author: ronglin
*   create date: 2011.12.28
*/

/*
* config parameters:
* integer
*/

(function ($, ctx) {

    var numberClass = function (config) {
        if (!this.className) { this.className = 'numberjs'; }
        numberClass.superclass.constructor.call(this, config);
    };

    yardi.extend(numberClass, ctx.editors.textClass, {

        integer: false,

        initialize: function () {
            var self = this;
            numberClass.superclass.initialize.call(this);
            this.input.keydown(function (ev) { self.checkKey(ev); });
        },

        checkKey: function (ev) {
            var keyCode = ev.keyCode;
            if (keyCode == 9 || keyCode == 27) { return; } // tab, esc
            if (keyCode == 33 || keyCode == 34 || keyCode == 35 || keyCode == 36) { return; } // page up, page down, end, home
            if (this.integer !== true) {
                if (keyCode == 190 || keyCode == 110) { // dot
                    if (this.input.val().indexOf('.') === -1) {
                        return;
                    }
                }
            }
            if (!(keyCode == 46) && !(keyCode == 8) && !(keyCode == 37) && !(keyCode == 39)) {
                if (!((keyCode >= 48 && keyCode <= 57) || (keyCode >= 96 && keyCode <= 105))) {
                    ev.preventDefault();
                }
            }
        }

    });

    // register
    ctx.editors.numberClass = numberClass;

} (jQuery, visualstyle));
