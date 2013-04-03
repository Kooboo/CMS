
/// <reference path="../../../Scripts/extension/js.extension.js" />

/// <reference path="../../../Scripts/kooboo.js" />
/// <reference path="../../../Scripts/kooboo.cms.ui.js" />
/// <reference path="../../../Scripts/jquery-1.4.1-vsdoc.js" />

/// <reference path="../../../Scripts/jQueryUI/jquery-ui-1.8.4.all.min.js" />
/// <reference path="../../../Scripts/extension/jquery.form.min.js" />
/// <reference path="../../../Scripts/extension/jquery.json-2.2.js" />
/// <reference path="../../../Scripts/extension/jquery.extension.js" />

///#>
kooboo.namespace("adminJs.global");
kooboo.namespace("adminJs.fileJs");
kooboo.namespace("adminJs.page");
///<#


adminJs.global.extend({
	commomPopFormLoad: function (handle, pop, config) {
		var iframe = pop.iframe.contents();
		iframe = $(iframe);
		var form = iframe.find("form");
		var success = form.find("#success");
		if (success.val() && success.val().toLocaleLowerCase() == "true") {
			pop.close();
			document.location.reload();
		}
	}
});


adminJs.extend({
	ready: function () {
		//初始化Import Dialog
		///init import dialog
		(function initImportDialog() {
			
			$(".Relations").each(function () {
				$(this).pop({
					width: 600,
					height: 370,
					reload: true,
					isIframe: false
				});
			});

			$(".task-panel .block-list li.has-sub").hover(function () {
				$(this).addClass('active');
			}, function () {
				$(this).removeClass('active');
			});
		})();

		//初始化站点选中下拉框点击事件
		(function initSitesDropDownList() {
			$("#MenuItems").change(function () {
				window.location.href = $(this).val();
			});
		})();

	}
});

///label
kooboo.namespace("adminJs.label");
adminJs.label.extend({
	_init: function (option) {
		var config = {
			width: 500,
			height: 310,
			frameHeight: "98%",
			onload: adminJs.global.commomPopFormLoad
		};
		$.extend(config, option);


		$("table tr").each(function () {
			var current = $(this);
			var trigger = current.find("a.edit").hide();
			var binder = current.find("td:eq(2) span").inlineEditor({
				triggerSelector: trigger,
				url: trigger.attr('href'),
				dataName: "Value",
				autoExpand: true,
				oninit: function () { trigger.hide(); binder.hide(); },
				onsave: function () { trigger.show(); binder.show(); },
				oncancel: function () { trigger.show(); binder.show(); },
				dataType: "text"
			});
			current.hover(function () { if (!binder.data("status")) { trigger.show(); } }, function () { trigger.hide(); });
		});
	},
	init: function (option) {
		$(function () { adminJs.label._init(option); });
	}
});

kooboo.namespace("kooboo.cms.viewengine.defaultEngine");
kooboo.cms.viewengine.defaultEngine.extend({
    webform: {
        getPositionList: function (body) {
            var list = [];

            var reg = /Html.FrontHtml\(\).Position\("\w*"\)/g;

            var r = reg.exec(body);

            var i = 0;
            while (r) {
                i++;

                r = r.toString();

                var position = r.substr(r.indexOf('"') + 1, r.lastIndexOf('"') - r.indexOf('"') - 1);

                list.push(position);

                r = reg.exec(body);
            }

            return list;

        },
        getPositionCodeSnippet: function (name) {
            return '<%:Html.FrontHtml().Position("' + name + '")%>';
        },
        removePosition: function (name, body) {
            var reg = /\<\%[:|=]\s*Html\.FrontHtml\(\s*\)\.Position\(\s*\"(\w+)\"\s*\)\s*%>/g;
            return body.replace(reg, function (a, b) { if (b.toLowerCase() == name.toLowerCase()) { return ""; } else return a; });
        }
    },
    razor: {
        getPositionList: function (body) {
            var list = [], reg = /Html.FrontHtml\(\).Position\("\w*"\)/g, r = reg.exec(body);
            while (r) {
                r = r.toString();
                var position = r.substr(r.indexOf('"') + 1, r.lastIndexOf('"') - r.indexOf('"') - 1);
                list.push(position);
                r = reg.exec(body);
            }
            return list;
        },
        getPositionCodeSnippet: function (name) {
            return '<%:Html.FrontHtml().Position("' + name + '")%>';
        },
        removePosition: function (name, body) {
            var reg = /\<\%[:|=]\s*Html\.FrontHtml\(\s*\)\.Position\(\s*\"(\w+)\"\s*\)\s*%>/g;
            return body.replace(reg, function (a, b) { if (b.toLowerCase() == name.toLowerCase()) { return ""; } else return a; });
        }
    },
    nvelocity: {
        getPositionList: function (body) {
            var list = [];

            var reg = /Html.FrontHtml\(\).Position\("\w*"\)/g;

            var r = reg.exec(body);

            var i = 0;
            while (r) {
                i++;

                r = r.toString();

                var position = r.substr(r.indexOf('"') + 1, r.lastIndexOf('"') - r.indexOf('"') - 1);

                list.push(position);

                r = reg.exec(body);
            }

            return list;

        },
        getPositionCodeSnippet: function (name) {
            return '<%:Html.FrontHtml().Position("' + name + '")%>';
        },
        removePosition: function (name, body) {
            var reg = /\<\%[:|=]\s*Html\.FrontHtml\(\s*\)\.Position\(\s*\"(\w+)\"\s*\)\s*%>/g;
            return body.replace(reg, function (a, b) { if (b.toLowerCase() == name.toLowerCase()) { return ""; } else return a; });
        }
    }
});
