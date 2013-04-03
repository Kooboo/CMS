<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
	
    var viewEngine = (ITemplateEngine)(ViewData["ViewEngine"]);
    var viewEngineName = viewEngine.Name;

%>
<script language="javascript" type="text/javascript">

</script>
<script language="javascript" type="text/javascript">
	<%:viewEngine.GetLayoutPositionParser().RegisterClientParser() %>
	<%:viewEngine.GetLayoutPositionParser().RegisterClientAddPosition() %>
	<%:viewEngine.GetLayoutPositionParser().RegisterClientRemovePosition()%>
	$(function () {
        $('form input').keydown(function(e){
            if(e.keyCode == 13){
                return false;
            }
        });
		kooboo.cms.ui.status().bind(window, '<%:"Are you sure you want to leave current page".Localize() %>');
		var config = {
			positionInputDisplay: '<%:("Position Name").Localize()%>',
			positionNullAlert: '<%:("Please input a new position name.").Localize()%>',
			nameRepeat: '<%:("The position name already exists.").Localize()%>',
			wrongPosition: '<%:("The position name already exists.").Localize()%>',
			textAreaId: "#Body",
			removeText: '<%:("Remove").Localize()%>',
			removeConfirm: '<%:("Are you sure you want to remove this item?").Localize()%>',
			replaceConfirm: '<%:("Are you sure you want to replace this template?").Localize()%>'
		};
		
		var textArea = $(config.textAreaId).codeMirror({
            onChange:function(e){
                if(e&&e.historySize().undo > 0){
                    kooboo.cms.ui.status().stop();
                }else{
                    kooboo.cms.ui.status().pass();
                }

                if(e){
                    initPositionList();
                    var codeMirrorAPI = textArea.data("codeMirror");
				    var positionList = getPositionList(codeMirrorAPI.getCode());

				    var hasSameName = false;
				    positionList.each(function(s,sIndex){
					    positionList.each(function(d,dIndex){
						    if( sIndex!=dIndex && s == d) { hasSameName = true }
					    });
				    });
				    if(hasSameName) {
					    kooboo.cms.ui.messageBox().show(config.nameRepeat,'error');
                        positionAllRight = false;
				    }else{
                        positionAllRight = true;
					    kooboo.cms.ui.messageBox().hide();
				    }
                }
            },
            sizeProvider: function (div) {
                div.width($(window).width() - 520);
            }
        });

        function initPositionList() {
				var codeMirrorAPI = textArea.data("codeMirror");
				var positionList = getPositionList(codeMirrorAPI.getCode());
				var ul = $("ul.positions");
				ul.find("li.machine").remove();
				positionList.each(function (val, index) {
					var li = $('<li></li>')
                    .text(val)
                    .addClass("machine")
                    .appendTo(ul).append('<a href="javascript:;" class="o-icon action remove right">' + config.removeText + '</a>');
                    var removeBtn = li.find("a.remove").click(function () {
						
						if (!confirm(config.removeConfirm)) {
							return false;
						}
                        kooboo.cms.ui.status().stop();
						var code = codeMirrorAPI.getCode();

						var newCode = removePosition(val,code);

						li.fadeOut("slow", function () {
							codeMirrorAPI.init(newCode);
							li.remove();
						});

                        // prevent to fire the beforeunload event in ie9
                        return false;
					});
				});
			}

        var codeMirrorAPI = textArea.data('codeMirror');

		(function initViewTools() {
			var viewTools = $("div.viewTools,li.viewTools");
			viewTools.find("li").hover(function () {
				$(this).addClass('active');
			}, function () {
				$(this).removeClass('active');
			});
			var hasSubLi = viewTools.find("li.has-sub");

			$("div.task-block").each(function (index) {
                var current = $(this);
                var content = current.find("div.content");

                var h3 = current.find("h3.title").css({cursor:'pointer'});
                h3.click(function () {
                    var span = h3.find('span');
                    if (span.hasClass('close')) {
                        span.removeClass("close");
                        content.slideDown("fast");
                    } else {
                        content.slideUp("fast");
                        span.addClass("close");
                    }
                });
                index++;
            });

			viewTools.find("a").click(function () {
				kooboo.cms.ui.status().stop();
				var current = $(this);
				var codeMirrorAPI = textArea.data("codeMirror");
				var viewName = current.attr('title');
				if (viewName != undefined && viewName != "") {
					var insert = current.attr('title');
					codeMirrorAPI.insertAtCursor(insert);
				}
			});



             (function initPlugins() {
                var displayUl = $("div.task-block.plugins ul.plugins");
                var mutipleDropdown = $("#Plugins");

                initDisplayList();
                function initDisplayList() {
                    displayUl.html('');
                    if (mutipleDropdown.val() != null) {
                        var valArray = mutipleDropdown.val();
                        valArray.each(function (value, index) {
                            var current = mutipleDropdown.children("option[value='" + value + "']");
                            var li = $('<li><span></span><a href="javascript:;" class="action remove right o-icon remove"></a></li>');
                            var remove = li.find("a.action").click(function () {
                                current.attr("selected", false);
                                li.remove();
                                return false;
                            });
                            li.hover(function () {
                                remove.show();
                            }, function () {
                                remove.hide();
                            });
                            li.find("span").html(current.text());
                            li.appendTo(displayUl);
                            li.data("option", current);
                        });
                    }
                }
                var addBtn = $("div.plugins a.add");

                var select = $("<select></select>");

                select.html(mutipleDropdown.html());

                mutipleDropdown.css({ position: "absolute", left: '-1000px' }).after(select);

                addBtn.click(function () {

                    var added = false;

                    var selectedOption = mutipleDropdown.children("option[value='" + select.val() + "']");

                    displayUl.children().each(function () {
                        added = added || ($(this).data("option").val() == selectedOption.val());
                    });

                    if (added) {
//                        alert(config.itemExist);
                    } else {
                        kooboo.cms.ui.status().stop();
                        selectedOption.attr("selected", true);

                        initDisplayList();
                    }


                });

            })();

		})();

        var positionAllRight = true;

        kooboo.cms.ui.event.ajaxSubmit(function(){
            if(!positionAllRight){
                kooboo.cms.ui.messageBox().show('The position name already exists.','error');
            }
            return positionAllRight;
        });

		///init Positions
		var initPositionFunc = (function initPosition() {
			setTimeout(function () {
				initPositionList();
			}, 500);

			var addPositonLink = $('.addPosition').yardiTip({
				content: function () {
					var html = [];
					html.push('<label>' + config.positionInputDisplay + ': </label>');
					html.push('<input type="text" name="name" id="positionName" style="width: 140px;" /> ');
					html.push('<input type="button" id="btnAddPosition" name="btnAddPosition" class="button" value="OK" /><input type="button" id="btnAddCancel" name="btnAddPosition" class="button" value="Cancel" />');
					return html.join('');
				},
				onShow: function () {

					var current = this;

					var positionName = $("#positionName").focus();

					positionName.unbind("keyup").keyup(function (e) {
						if (e.keyCode == 13) {
							submitPosition();
						} else if (e.keyCode == 27) {
							addPositonLink.data('yardiTip').hide();
						}
					});

					var btnAddPosition = $("#btnAddPosition").unbind("click").click(function () {
						submitPosition();
					});

					var btnAddPosition = $("#btnAddCancel").unbind("click").click(function () {
						addPositonLink.data('yardiTip').hide();
					});


					$('<div class="mask"></div>').appendTo('body');
					this.el.css('z-index', 1001);

				},
				onHide: function () { $('.mask').remove(); }
			});

			function submitPosition() {
				var codeMirrorAPI = textArea.data("codeMirror");
				var positionName = $("#positionName").focus();
				var val = positionName.val();

                if(val.indexOf('"')>0||val.indexOf("'")>0||val.indexOf(" ")>0)
                {
                    kooboo.alert('<%:"a name cannot contain a space, a single or double quote.".Localize() %>',function(){
                        setTimeout(function () {
								positionName.focus();
							}, 500);
                    });
                    return false;
                }

                val = val.trimAll();

				if (val.length > 0) {
					var insert = getPositionCodeSnippet(val);
					var query = getPositionList(codeMirrorAPI.getCode()).where(function (o) { return o.toLocaleLowerCase() == val.toLocaleLowerCase(); })
					if (query.length > 0) {
						kooboo.alert(config.nameRepeat, function () {
							addPositonLink.data('yardiTip').show();
							setTimeout(function () {
								positionName.focus();
							}, 500);
						}, "");
						return false;
					}
					codeMirrorAPI.insertAtCursor(insert)

					addPositonLink.data('yardiTip').hide();
					initPositionList();
						//                        codeMirrorAPI.init();

				} else {
					kooboo.alert(config.positionNullAlert, function () {
						addPositonLink.data('yardiTip').show();
						setTimeout(function () {
							positionName.focus();
						}, 500);
					}, "");
				}


			}

			return initPositionList;
		})();

		(function initCodeSample() {
			$("li.codeSample").find("a").click(function () {
				var current = $(this);
				var insert = $(this).attr("title");
				var codeMirrorAPI = textArea.data("codeMirror");
				codeMirrorAPI.insertAtCursor(insert);
			});
		})();

		(function () {
			$("li.layoutSamples").find("a").click(function () {
				if (!confirm(config.replaceConfirm)) {
					return false;
				}
				var insert = $(this).attr("rel");

				var codeMirrorAPI = textArea.data("codeMirror");
				codeMirrorAPI.init(insert);

				initPositionFunc();
			});
		})();

		///init Save
		(function initSave() {

			kooboo.cms.ui.event.ajaxSubmit(function () {
				kooboo.cms.ui.status().pass();
				var codeMirrorAPI = textArea.data("codeMirror");
				var text = codeMirrorAPI.getText();
				textArea.val(text);
			});

		})();

	});
</script>
