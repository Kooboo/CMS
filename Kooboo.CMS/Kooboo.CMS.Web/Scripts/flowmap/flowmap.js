/*
*
*   flow map
*   author: ronglin
*   create date: 2010.11.08
*
*/

/*
*   // initialization type1: 
*   // by genterate node from the childrens of map board.
*   $('.fm-board').flowmap();
*   
*   // initialization type2: 
*   // by setting the custom nodes.
*   $('#fm-board').flowmap({
*       root: {
*           x: 10,
*           y: 10,
*           html: 'test',
*           items: []
*       }
*   });
*   
*   <!--
*   1. when typeset is 'fixed' (default setting). attribute 'xy' is required, and only when typeset is fixed that allow multiple parent.
*   2. when typeset is 'vertical' or 'horizontal', the 'xy' setting is ignored, and not allow multiple parent.
*   3. if the line auto generated is not meet the requirement, you can define the points yourself by the 'line' attribute.
*   4. the attribute 'turn' use for limit the first turn distance of child lines that is auto generated.
*   5. reference below html code to get the setting formats.
*   -->
*   
*   <div class="fm-board">
*       <div class="fm-node" id="root" parentid="" xy="100,10" turn="10">
*       </div>
*   
*       <div class="fm-node" id="a" parentid="root" xy="300,150" line="[[10,10],[20,10],[20,20],[40,20]]">
*       </div>
*   
*       <div class="fm-node" id="b" parentid="root,a" xy="600,200" line="{root:[..],a:[..]}">
*       </div>
*   </div>
*/

(function ($) {

    // namespace
    $._flowmap = {
        apply: function (source, applyTo) {
            if (source) {
                for (var p in source)
                    applyTo[p] = source[p];
            }
            return applyTo;
        }
    };

    var isArray = function (v) { return Object.prototype.toString.apply(v) === '[object Array]'; };

    /*
    * config parameters:
    * el, root, 
    * typeset, showArrow, dragAxis
    * compress, spaceHorizontal, spaceVertical
    */
    var mapBoard = function (config) {
        $._flowmap.apply(config, this);
        this.initialize();
    };

    mapBoard.prototype = {

        el: null,

        root: null,

        typeset: 'fixed', // fixed, vertical, horizontal

        lineThick: 2, showArrow: true, // true, false

        dragAxis: null, // null, x, y, xy

        compress: true, // true, false

        spaceHorizontal: 30, spaceVertical: 60, // node space

        paddingTop: 10, paddingLeft: 10, // boundary padding

        initialize: function () {
            var el = this.el;
            el.css('position', 'relative');
            if (el.attr('typeset')) { this.typeset = el.attr('typeset'); }
            if (el.attr('lineThick')) { this.lineThick = parseInt(el.attr('lineThick')); }
            if (el.attr('showArrow')) { this.showArrow = (el.attr('showArrow') === 'true'); }
            if (el.attr('dragAxis')) { this.dragAxis = el.attr('dragAxis'); }
            if (el.attr('compress')) { this.compress = el.attr('compress') === 'true'; }
            if (el.attr('spaceVertical')) { this.spaceVertical = parseInt(el.attr('spaceVertical')); }
            if (el.attr('spaceHorizontal')) { this.spaceHorizontal = parseInt(el.attr('spaceHorizontal')); }
            if (el.attr('paddingTop')) { this.paddingTop = parseInt(el.attr('paddingTop')); }
            if (el.attr('paddingLeft')) { this.paddingLeft = parseInt(el.attr('paddingLeft')); }
            // store context settings
            el.data({
                typeset: this.typeset,
                lineThick: this.lineThick,
                showArrow: this.showArrow,
                dragAxis: this.dragAxis,
                compress: this.compress,
                spaceVertical: this.spaceVertical,
                spaceHorizontal: this.spaceHorizontal,
                paddingTop: this.paddingTop,
                paddingLeft: this.paddingLeft
            });
        },

        rootNode: null,

        getJson: function () {
            var json = [];
            this.rootNode && $.each(this.rootNode, function (i, o) { json.push(o.getJson()); });
            return json.join('');
        },

        render: function () {
            // remove old
            this.rootNode && $.each(this.rootNode, function (i, o) { o.remove(); });
            this.rootNode = [];
            // craete new
            var self = this;
            $.each(this.root, function (i, o) {
                var set = o.jquery ? { el: o} : o;
                set.context = self.el;
                var node = new $._flowmap.mapNode(set);
                self.rootNode.push(node);
                node.render();
            });
        }

    };

    /*
    * jquery entrance
    */
    $.extend($.fn, {
        flowmap: function (options) {

            // closure
            options = options || {};

            // check has elements
            if (!this.length) {
                options.debug && window.console && console.warn('nothing selected');
                return this;
            }

            // loop
            this.each(function () {

                // get cache
                var map = $.data(this, 'flowmap');
                if (map) { return map; }

                // get root
                var root = options.root, self = this;
                if (!root) {
                    root = [];
                    $(this).children('.fm-node').each(function () {
                        if (!$(this).attr('parentId')) {
                            root.push($(this));
                        }
                    }).each(function () {
                        var pid = $(this).attr('parentId');
                        if (pid) {
                            var ids = pid.split(',');
                            for (var i = 0; i < ids.length; i++) {
                                if (ids[i]) {
                                    var p = $('.fm-node[id="' + ids[i] + '"]', self);
                                    if (p.length > 0) {
                                        var items = p.data('items') || [];
                                        items.push($(this));
                                        p.data('items', items);
                                    }
                                    // only when typeset is fixed that allow multiple parent.
                                    if (options.typeset && options.typeset != 'fixed') {
                                        break;
                                    }
                                }
                            }
                        }
                    });
                } else {
                    // ensure root is an array object
                    if (!isArray(root)) { root = [root]; }
                }

                // reset layout first
                $._flowmap.nodeLayout.reset();

                // new instance
                var settings = $.extend({}, options, { el: $(this), root: root });
                map = new mapBoard(settings);
                map.render();

                // set cache
                $.data(this, 'flowmap', map);

                // ret
                return map;
            });

            // ret
            return this;
        }
    });

})(jQuery);
