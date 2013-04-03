/*
*
*   map line
*   author: ronglin
*   create date: 2010.11.08
*
*/

/*
* config parameters:
* context, points
* typeset, showArrow
*/

(function ($) {

    var mapLine = function (config) {
        $._flowmap.apply(config, this);
        this.initialize();
    };

    mapLine.prototype = {

        el: null,

        context: null,

        points: null,

        typeset: null, showArrow: false,

        lineThick: 2, // this must larger than 1

        initialize: function () {
            this.innerHtml = [];
            this.lineThick = this.context.data('lineThick');
            this.el = $(this.buildHtml()).appendTo(this.context);
        },

        buildHtml: function () {
            return '<div class="fm-line"></div>';
        },

        remove: function () {
            this.el && this.el.remove();
        },

        cache: null, lastCache: null, innerHtml: null,

        pushHtml: function (css, cls) {
            var html = this.innerHtml;
            html.push('<p');
            if (cls) { html.push(' class="' + cls + '"'); }
            html.push(' style="');
            for (var key in css) { html.push(key + ':' + css[key] + 'px;'); }
            html.push('"></p>');
        },

        renderContent: function (x, y, v) {
            // cache
            var c = this.cache, lc = this.lastCache;
            if (!c || !lc) {
                this.cache = this.lastCache = { x: x, y: y, v: v };
                return;
            }
            if (((v === false && x === lc.x) ||
                 (v === true && y === lc.y)) && v === lc.v) {
                this.lastCache = { x: x, y: y, v: v };
                return;
            }
            this.cache = this.lastCache = { x: x, y: y, v: v };
            // draw
            var css = {};
            if (c.v) {
                css.width = 1;
                css.height = this.lineThick;
                var w = lc.x - c.x;
                if (w) { css.width = Math.abs(w) + 1; }
                css.left = (w < 0 ? lc.x : c.x);
                css.top = c.y;
            } else {
                css.width = this.lineThick;
                css.height = 1;
                var h = lc.y - c.y;
                if (h) { css.height = Math.abs(h) + 1; }
                css.left = c.x;
                css.top = (h <= 0 ? lc.y : c.y);
            }
            // push
            this.pushHtml(css);
        },

        renderArrow: function () {
            var len = this.points.length,
            p1 = this.points[len - 1],
            p2 = this.points[len - 2],
            cls, css = { left: p1[0] - 6, top: p1[1] - 6 };
            // dir
            if (p1[0] === p2[0]) { cls = (p1[1] > p2[1]) ? 'down' : 'up'; }
            if (p1[1] === p2[1]) { cls = (p1[0] > p2[0]) ? 'right' : 'left'; }
            // push
            this.pushHtml(css, 'arrow ' + cls);
        },

        render: function () {
            if (!this.points || this.points.length < 2) { return; }
            if (this.el) { this.el.get(0).innerHTML = ''; }
            for (var i = 0; i < this.points.length; i++) {
                if (this.points[i + 1]) {
                    var from = this.points[i], to = this.points[i + 1];
                    var fromX = from[0], fromY = from[1], toX = to[0], toY = to[1];
                    var signX = (fromX > toX) ? -1 : 1, signY = (fromY > toY) ? -1 : 1;
                    var loopX = parseInt(Math.abs(fromX - toX), 10), loopY = parseInt(Math.abs(fromY - toY), 10);
                    if (loopX > loopY) {
                        for (var x = 0; x < loopX; x++) {
                            this.renderContent(fromX + x * signX, fromY + (loopY / loopX) * x * signY, true);
                        }
                    } else {
                        for (var y = 0; y < loopY; y++) {
                            this.renderContent(fromX + (loopX / loopY) * y * signX, fromY + y * signY, false);
                        }
                    }
                }
            }
            // commit
            this.renderContent();
            if (this.showArrow === true) { this.renderArrow(); }
            this.el.get(0).innerHTML = this.innerHtml.join('');
            this.cache = this.lastCache = null;
            this.innerHtml = [];
        }

    };


    // outer call entrance
    mapLine.connect = function (fromNode, toNode, config) {
        var context = fromNode.context, points;
        // line points set
        var lineSet = toNode.line || toNode.el.attr('line');
        if (lineSet) {
            var obj = (typeof (lineSet) === 'string') ? $.parseJSON(lineSet) : lineSet;
            if (obj) {
                if (obj.length !== undefined) {// is array
                    points = obj;
                } else if (toNode.parent) {
                    points = obj[toNode.parent.id];
                }
            }
        }
        // line turn set
        if (!points) {
            var turn = fromNode.turn || fromNode.el.attr('turn');
            if (turn) { turn = parseInt(turn); }
            points = pointLayout[context.data('typeset')](fromNode.getBox(), toNode.getBox(), turn);
        }
        // create
        var setting = $._flowmap.apply(config, {
            context: context,
            points: points
        })
        var line = new mapLine(setting);
        line.render();
        return line;
    };

    var direction = function (fromBox, toBox) {
        var vertical = Math.abs(fromBox.x - toBox.x) < Math.abs(fromBox.y - toBox.y);
        if (vertical) {
            return (fromBox.y < toBox.y) ? 'down' : 'up';
        } else {
            return (fromBox.x < toBox.x) ? 'right' : 'left';
        }
    };

    var pointLayout = {

        /*************** typesets **************/

        fixed: function (fromBox, toBox, turn) {
            return pointLayout[direction(fromBox, toBox)](fromBox, toBox, turn);
        },

        horizontal: function (fromBox, toBox, turn) {
            return this.right(fromBox, toBox, turn);
        },

        vertical: function (fromBox, toBox, turn) {
            return this.down(fromBox, toBox, turn);
        },

        /*************** directions **************/

        left: function (fromBox, toBox, turn) {
            turn = turn || (Math.abs(fromBox.x - toBox.x) - toBox.w) / 2;
            return [
                [fromBox.x, fromBox.y + fromBox.h / 2],
                [fromBox.x - turn, fromBox.y + fromBox.h / 2],
                [fromBox.x - turn, toBox.y + toBox.h / 2],
                [toBox.x + toBox.w, toBox.y + toBox.h / 2]
            ];
        },

        right: function (fromBox, toBox, turn) {
            turn = turn || (Math.abs(toBox.x - fromBox.x) - fromBox.w) / 2;
            return [
                [fromBox.x + fromBox.w, fromBox.y + fromBox.h / 2],
                [fromBox.x + fromBox.w + turn, fromBox.y + fromBox.h / 2],
                [fromBox.x + fromBox.w + turn, toBox.y + toBox.h / 2],
                [toBox.x, toBox.y + toBox.h / 2]
            ];
        },

        up: function (fromBox, toBox, turn) {
            turn = turn || (Math.abs(fromBox.y - toBox.y) - toBox.h) / 2;
            return [
                [fromBox.x + fromBox.w / 2, fromBox.y],
                [fromBox.x + fromBox.w / 2, fromBox.y - turn],
                [toBox.x + toBox.w / 2, fromBox.y - turn],
                [toBox.x + toBox.w / 2, toBox.y + toBox.h]
            ];
        },

        down: function (fromBox, toBox, turn) {
            turn = turn || (Math.abs(toBox.y - fromBox.y) - fromBox.h) / 2;
            return [
                [fromBox.x + fromBox.w / 2, fromBox.y + fromBox.h],
                [fromBox.x + fromBox.w / 2, fromBox.y + fromBox.h + turn],
                [toBox.x + toBox.w / 2, fromBox.y + fromBox.h + turn],
                [toBox.x + toBox.w / 2, toBox.y]
            ];
        }
    };

    // register
    $._flowmap.mapLine = mapLine;

})(jQuery);
