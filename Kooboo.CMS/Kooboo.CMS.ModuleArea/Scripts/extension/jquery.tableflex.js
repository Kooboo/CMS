/// <reference path="../jquery-1.4.1-vsdoc.js" />
; (function () {
	$.fn.tableFlex = function (option) {
		if (typeof (option) == "string") {
			return this.data("api")[option]();
		}

		var config = {
			hiddenTd: [],
			autoHide: true,
			autoStyle: true
		};

		$.extend(config, option);

		var table = this;
		if (config.autoStyle) {
			table.addClass("tableFlex");
			var thead = table.find("thead");
			var colCount = thead.find("th").length;

			if (thead.length > 0) {
				$("<th></th>").appendTo(thead.find("tr"));
			}

			var tbody = table.find("tbody");
			tbody.prepend("<tr class=\"scroll-bar\"></tr>");
			scrollBar = tbody.find("tr.scroll-bar");
			for(i= 0;i<colCount;i++){
				scrollBar.append("<td></td>");
			}

			var td = $("<td></td>");
			var count = 1;
			count = tbody.find("tr").length;
			td.appendTo(tbody.find("tr:eq(0)"));
			td.addClass("right-arrow").attr("title", "More..")
                .attr("rowspan", count)
                .toggle(function () {
                	api.show();
                	td.removeClass("right-arrow");
                	td.addClass("left-arrow").attr("title", "Less..");
                }, function () {
                	api.hide();
                	td.removeClass("left-arrow");
                	td.addClass("right-arrow").attr("title", "More..");
                }).css({ cursor: "pointer" });
		}



		var columns = [];

		var selector = "";
		for (var i = 0; i < config.hiddenTd.length; i++) {
			selector += " td:eq(" + config.hiddenTd[i] + ") , th:eq(" + config.hiddenTd[i] + ")";
			if (i != (config.hiddenTd.length - 1)) {
				selector += ",";
			}
		}

		table.find("tr").each(function () {
			var column = $(this).find(selector);
			columns.push(column);
			if (config.autoHide) {
				column.addClass("hide");
			}
		});


		var api = {};

		api.show = function () {
			for (var i = 0; i < columns.length; i++) {
				columns[i].removeClass("hide");
			}
			return this;
		}

		api.hide = function () {
			for (var i = 0; i < columns.length; i++) {
				columns[i].addClass("hide");
			}
			return this;
		}

		this.data("api", api);
		return this;
	}
})();