/*
 * HeadingPicker
 * author: Raoh
 * create date: 2013.05-02
 */
(function(ctx, $) {
	var HeadingPicker = function() {
		// Heading setting
		var headingSetting = [{
			text : 'Paragraph',
			value : 'P',
			itemStyle : 'kb-p'
		},{
			text : 'Heading 1',
			value : 'H1',
			itemStyle : 'kb-h1'
		}, {
			text : 'Heading 2',
			value : 'H2',
			itemStyle : 'kb-h2'
		}, {
			text : 'Heading 3',
			value : 'H3',
			itemStyle : 'kb-h3'
		}, {
			text : 'Heading 4',
			value : 'H4',
			itemStyle : 'kb-h4'
		}, {
			text : 'Heading 5',
			value : 'H5',
			itemStyle : 'kb-h5'
		}, {
			text : 'Heading 6',
			value : 'H6',
			itemStyle : 'kb-h6'
		}];
		// closure parameters
		var callbackFn, picker;
		var resetting = function() {
			if (picker) {
				picker.hide();
				picker.remove();
				picker = null;
			}
		};
		var initialize = function() {
			resetting();
			picker = new ctx.listPicker({
				width : 140,
				dataList : headingSetting,
				onSelect : function(item, ev) {
					callbackFn(item, ev);
				}
			});
			picker.onInitialized.add(function() {
				$('.kb-item', this.el).each(function() {
					var index = $(this).attr('itemIndex');
					var item = headingSetting[parseInt(index)];
					$(this).addClass(item.itemStyle);
				});
			});
		};
		// core func
		return {
			// public
			show : function(refEl, callback) {
				if (!picker) {
					initialize();
				}
				// set fn
				callbackFn = callback;
				// show
				picker.show(refEl);
			},
			// public
			hide : function() {
				picker && picker.hide();
			},
			// public
			queryItem : function(value) {
				var set = null;
				if(value){
					value = value.toUpperCase();
					for (var i = 0; i < headingSetting.length; i++) {
						var set = headingSetting[i];
						if (set.value == value) {
							 break;
						}
					}
				}
				return set;
			},
			// public extensible api
			setting : function(array) {
				if (ctx.isArray(array)) {
					sizeSetting = array;
					resetting();
				} else {
					return sizeSetting;
				}
			},
			getSetting : function() {
				return this.setting();
			},
			setSetting : function(set) {
				return this.setting(set);
			}
		};
	}();
	// register
	ctx.HeadingPicker = HeadingPicker;
})(yardi, jQuery);
