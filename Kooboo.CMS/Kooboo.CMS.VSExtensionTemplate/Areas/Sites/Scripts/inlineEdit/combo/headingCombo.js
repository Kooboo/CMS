/*
 * HeadingCombo
 * author: Raoh
 * create date: 2013.05.02
 */
(function(ctx, $) {
	var HeadingCombo = function(config) {
		config = config || {};
		HeadingCombo.superclass.constructor.call(this, config);
	};
	ctx.extend(HeadingCombo, ctx.combo, {
		// cache
		selectedItem : null,
		// public config
		onSelect : function(item) {},
		// public config
		onClick : function(input) {
			var self = this;
			ctx.HeadingPicker.show(input,function(item){
				self.selectedItem=item;
				self.val(item.value);
				self.onSelect(item);
			});
		},
		// override
		val : function(val) {
			var item = ctx.HeadingPicker.queryItem(val);
			this.selectedItem = item;
			val = item ? item.text : val;
			HeadingCombo.superclass.val.apply(this, arguments);
		}
	});
	// register
	ctx.HeadingCombo = HeadingCombo;
})(yardi, jQuery);
