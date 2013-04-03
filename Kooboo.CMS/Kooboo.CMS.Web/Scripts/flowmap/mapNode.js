/*
*
*   map node
*   author: ronglin
*   create date: 2010.11.08
*
*/

/*
* config parameters:
* el, context, [parent, parentId]
* id, html, items, x, y
*/

(function ($) {

    var mapNode = function (config) {
        $._flowmap.apply(config, this);
        this.initialize();
    };

    mapNode.nodes = {};

    mapNode.register = function (id, obj) {
        mapNode.nodes[id] = obj;
    };

    mapNode.prototype = {

        el: null, context: null,

        id: null, html: null, items: null, x: 0, y: 0, // parameters

        parent: null, childs: null, ownLine: null, // outer reference

        collapsed: false,

        initialize: function () {
            var self = this;
            this.childs = [];

            // core element
            if (!this.el) { this.el = $(this.getHtml()).appendTo(this.context); }

            // parameters
            if (!this.parent && this.el.attr('parentId')) { this.parent = mapNode.nodes[this.el.attr('parentId')]; }
            if (!this.id) { this.id = this.el.attr('id') ? this.el.attr('id') : (Math.random() + '').substr(2); }
            if (!this.items) { this.items = this.el.data('items'); }
            //if (!this.html) { this.html = this.el.html(); }

            // hover
            this.el.hover(function () {
                $(this).addClass('fm-node-hover');
            }, function () {
                $(this).removeClass('fm-node-hover');
            });

            // drag
            if (this.context.data('dragAxis')) {
                // selection
                this.el.css('cursor', 'move').disableSelection().bind('selectstart', function () { return false; });
                // draggable
                this.el.draggable({
                    containment: this.context,
                    refreshPositions: true,
                    distance: 4,
                    axis: this.context.data('dragAxis'),
                    stop: function (event, ui) {
                        var helper = $('.fm-holder', self.context);
                        var pos = helper.position();
                        self.setJson({ x: pos.left, y: pos.top }, true);
                        helper.remove();
                    },
                    helper: function (ev) {
                        var holder = $('<div class="fm-holder"></div>').css(self.el.position());
                        holder.width(self.el.width()).height(self.el.height());
                        self.context.append(holder);
                        return holder;
                    }
                });
            }

            // collapsable
            this.el.bind('collapse', function (ev, data) {
                var compress = true;
                data && (data.compress === false) && (compress = false);
                self.collapse(compress);
            });

            // reg
            mapNode.register(this.id, this);
        },

        getHtml: function () {
            var html = [];
            html.push('<div class="fm-node">');
            html.push('</div>');
            return html.join('');
        },

        getBox: function () {
            return {
                x: this.x,
                y: this.y,
                w: this.el.outerWidth(),
                h: this.el.outerHeight()
            };
        },

        getCompu: function () {
            // level
            var s = this, level = 0;
            while (s.parent) {
                s = s.parent;
                level++;
            }

            // is first
            var self = this, isFirst = false;
            if (this.parent) {
                $.each(this.parent.childs, function (i, node) {
                    (node.id == self.id) && (isFirst = true);
                    return false;
                });
            }

            // has child
            var hasChild = false;
            $.each(this.childs, function () {
                hasChild = true;
                return false;
            });

            // neighbour
            var neighbour = 0;
            if (this.parent) {
                $.each(this.parent.childs, function () {
                    neighbour++;
                });
            }

            // ret
            return {
                level: level,
                isFirst: isFirst,
                hasChild: hasChild,
                neighbour: neighbour
            };
        },

        connect: function (from, to) {
            var line = $._flowmap.mapLine.connect(from, to, {
                showArrow: this.context.data('showArrow'),
                typeset: this.context.data('typeset')
            });
            to.ownLine = line;
            return line;
        },

        drawLine: function (child) {
            // self line
            this.ownLine && this.ownLine.remove();
            this.parent && this.connect(this.parent, this);
            // child lines
            if (child === true) {
                var self = this;
                $.each(this.childs, function (i, node) {
                    if (node.ownLine) {
                        node.ownLine.remove();
                        self.connect(self, node);
                    }
                });
            }
        },

        setJson: function (d, l) {
            var redraw = false;
            if (d.x !== undefined) {
                this.el.css({ left: d.x });
                this.x = d.x;
                redraw = true;
            }
            if (d.y !== undefined) {
                this.el.css({ top: d.y });
                this.y = d.y;
                redraw = true;
            }
            if (d.html !== undefined) { this.el.html(d.html); }
            // items
            if (d.items) {
                var self = this;
                this.clearChilds();
                $.each(d.items, function (i, o) {
                    var settings;
                    if (o.jquery) {
                        settings = { el: o };
                    } else {
                        settings = o;
                    }
                    settings.parent = self;
                    settings.context = self.context;
                    var node = new mapNode(settings);
                    self.childs.push(node);
                });
            }
            // line
            if (redraw === true && l === true) { this.drawLine(true); }
        },

        getJson: function () {
            var json = new nodeData();
            json.html = this.el.html();
            json.id = this.id;
            json.x = this.x;
            json.y = this.y;
            $.each(this.childs, function (i, node) {
                json.items.push(node.getJson());
            });
            return json;
        },

        addChild: function (node) {
            node.ownLine && node.ownLine.remove();
            node.parent = this;
            this.connect(this, node);
            this.childs.push(node);
            //TODO: relocate positions?
        },

        clearChilds: function () {
            $.each(this.childs, function (i, node) {
                node.remove();
            });
            this.childs = [];
        },

        remove: function () {
            // remove reference from parent
            if (this.parent) {
                var self = this;
                $.each(this.parent.childs, function (i, node) {
                    (node.id == self.id) && self.parent.childs.splice(i, 1);
                });
                this.parent = null;
            }
            // remove childs reference
            $.each(this.childs, function (i, node) {
                node.remove();
            });
            this.childs = null;
            // remove line reference
            this.ownLine && this.ownLine.remove();
            // remove self
            this.el.remove();
        },

        hide: function () {
            $.each(this.childs, function (i, node) {
                node.hide();
            });
            this.el.hide();
            this.ownLine && this.ownLine.el.hide();
        },

        show: function () {
            this.el.show();
            this.ownLine && this.ownLine.el.show();
            $.each(this.childs, function (i, node) {
                node.show();
            });
        },

        collapse: function (compress) {
            if (!this.childs || !this.childs.length) { return; }
            this.collapsed = !this.collapsed;
            compress = (compress !== false);
            // hide childs
            var self = this;
            $.each(this.childs, function (i, node) {
                self.collapsed ? node.hide() : node.show();
            });
            // compress layout
            if (compress) {
                //TODO:
                //this.context.data('locateMap')();
            }
        },

        locate: function () {
            //TODO:
        },

        render: function () {
            // force create childs
            this.setJson({ items: this.items || undefined });

            // render childs first
            $.each(this.childs, function (i, node) {
                node.render();
            });

            // show
            this.el.show();

            // typeset
            var xy = nodeLayout[this.context.data('typeset')](this);
            if (xy && xy.length == 2) {
                if (!this.x) { this.x = xy[0]; }
                if (!this.y) { this.y = xy[1]; }
            }

            // json
            this.setJson({
                x: this.x || undefined,
                y: this.y || undefined,
                html: this.html || undefined
            });

            // render line
            $.each(this.childs, function (i, node) {
                node.drawLine(false);
            });
        }
    };


    var nodeLayout = {

        fixed: function (node) {
            var x = 0, y = 0;
            var xy = node.el.attr('xy');
            if (xy) {
                xy = xy.split(','); // eg: '300,100'
                if (xy.length == 2) {
                    x = parseInt(xy[0]);
                    y = parseInt(xy[1]);
                }
            }
            // ret
            return [x, y];
        },

        maxY: null,

        levelMaxY: {},

        horizontal: function (node) {
            var compu = node.getCompu(), ctx = node.context,
            nodeWidth = node.el.outerWidth(),
            nodeHeight = node.el.outerHeight(),
            paddingTop = ctx.data('paddingTop'),
            paddingLeft = ctx.data('paddingLeft'),
            spaceVertical = ctx.data('spaceVertical'),
            spaceHorizontal = ctx.data('spaceHorizontal'),
            compress = ctx.data('compress');

            var x = paddingLeft + (nodeWidth + spaceHorizontal) * compu.level, y = 0;

            if (compu.hasChild) {
                var minY, maxY;
                $.each(node.childs, function (k, n) {
                    if (!minY || !maxY) {
                        minY = maxY = n.y;
                    } else {
                        minY = Math.min(minY, n.y);
                        maxY = Math.max(maxY, n.y);
                    }
                });
                y = (minY + maxY) / 2;
            } else {
                if (compress) {
                    var getParentY = function (level, start) {
                        var minY = start || paddingTop, maxY = minY + (compu.neighbour - 1) * (nodeHeight + spaceVertical);
                        var l = compu.level, n = node;
                        while (l > level) {
                            n = n.parent;
                            var c = n.getCompu();
                            minY = (minY + maxY) / 2;
                            maxY = minY + (c.neighbour - 1) * (nodeHeight + spaceVertical);
                            l--;
                        }
                        // ret
                        return (minY + maxY) / 2;
                    };
                    var offset = 0;
                    var lm = this.levelMaxY[compu.level];
                    if (!lm || compu.isFirst) {
                        var current = compu.level - 1;
                        while (current > -1) {
                            var levelMax = this.levelMaxY[current];
                            if (levelMax) {
                                var parentY = getParentY(current + 1, lm);
                                var offsetY = levelMax + nodeHeight + spaceVertical - parentY;
                                if (offsetY > 0) {
                                    offset = Math.max(offsetY, offset);
                                }
                            }
                            current--;
                        }
                    }
                    if (!lm) {
                        y = paddingTop + offset;
                    } else {
                        y = lm + Math.max(offset, nodeHeight + spaceVertical);
                    }
                } else {
                    if (this.maxY === null) {
                        y = paddingTop;
                    } else {
                        y = this.maxY + nodeHeight + spaceVertical;
                    }
                }
            }

            if (compress) {
                var lm = this.levelMaxY[compu.level];
                if (!lm || lm < y) { this.levelMaxY[compu.level] = y; }
            } else {
                if (!this.maxY || this.maxY < y) { this.maxY = y; }
            }

            // ret
            return [x, y];
        },

        maxX: null,

        levelMaxX: {},

        vertical: function (node) {
            var compu = node.getCompu(), ctx = node.context,
            nodeWidth = node.el.outerWidth(),
            nodeHeight = node.el.outerHeight(),
            paddingTop = ctx.data('paddingTop'),
            paddingLeft = ctx.data('paddingLeft'),
            spaceVertical = ctx.data('spaceVertical'),
            spaceHorizontal = ctx.data('spaceHorizontal'),
            compress = ctx.data('compress');

            var x = 0, y = paddingTop + (spaceVertical + nodeHeight) * compu.level;

            if (compu.hasChild) {
                var minX, maxX;
                $.each(node.childs, function (k, n) {
                    if (!minX || !maxX) {
                        minX = maxX = n.x;
                    } else {
                        minX = Math.min(minX, n.x);
                        maxX = Math.max(maxX, n.x);
                    }
                });
                x = (minX + maxX) / 2;
            } else {
                if (compress) {
                    var getParentX = function (level, start) {
                        var minX = start || paddingLeft, maxX = minX + (compu.neighbour - 1) * (nodeWidth + spaceHorizontal);
                        var l = compu.level, n = node;
                        while (l > level) {
                            n = n.parent;
                            var c = n.getCompu();
                            minX = (minX + maxX) / 2;
                            maxX = minX + (c.neighbour - 1) * (nodeWidth + spaceHorizontal);
                            l--;
                        }
                        // ret
                        return (minX + maxX) / 2;
                    };
                    var offset = 0;
                    var lm = this.levelMaxX[compu.level];
                    if (!lm || compu.isFirst) {
                        var current = compu.level - 1;
                        while (current > -1) {
                            var levelMax = this.levelMaxX[current];
                            if (levelMax) {
                                var parentX = getParentX(current + 1, lm);
                                var offsetX = levelMax + nodeWidth + spaceHorizontal - parentX;
                                if (offsetX > 0) {
                                    offset = Math.max(offsetX, offset);
                                }
                            }
                            current--;
                        }
                    }
                    if (!lm) {
                        x = paddingLeft + offset;
                    } else {
                        x = lm + Math.max(offset, nodeWidth + spaceHorizontal);
                    }
                } else {
                    if (this.maxX === null) {
                        x = paddingLeft;
                    } else {
                        x = this.maxX + nodeWidth + spaceHorizontal;
                    }
                }
            }

            if (compress) {
                var lm = this.levelMaxX[compu.level];
                if (!lm || lm < x) { this.levelMaxX[compu.level] = x; }
            } else {
                if (!this.maxX || this.maxX < x) { this.maxX = x; }
            }

            // ret
            return [x, y];
        },

        reset: function () {
            this.levelMaxX = {};
            this.levelMaxY = {};
            this.maxX = null;
            this.maxY = null;
        }
    };


    var nodeData = function () {
        this.items = [];
    };

    nodeData.prototype = {
        x: 0,
        y: 0,
        id: null,
        html: null,
        items: null
    };

    // register
    $._flowmap.mapNode = mapNode;
    $._flowmap.nodeData = nodeData;
    $._flowmap.nodeLayout = nodeLayout;

})(jQuery);