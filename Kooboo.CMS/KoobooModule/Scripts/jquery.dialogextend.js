/*!
 * jQuery DialogExtend 1.0
 *
 * Copyright (c) 2010 Shum Ting Hin
 *
 * Licensed under MIT
 *   http://www.opensource.org/licenses/mit-license.php
 *
 * Project Home:
 *   http://code.google.com/p/jquery-dialogextend/
 *
 * Depends:
 *   jQuery 1.4
 *   jQuery UI Dialog 1.8.0
 *
 * History:
 *   0.9   / 2010-11-04 / hin / creation of plugin
 *   0.9.1 / 2010-11-16 / hin / fix bug of zero-config
 *   0.9.2 / 2010-12-16 / hin / fix bug of not firing <load.dialogExtend> event
 *                              apply <events> as init option for defining event-callback
 *   1.0   / 2010-01-05 / hin / fix bug of button-pane in 'minimized' state
 *                              fix bug of title-bar word-wrap in 'minimized' state
 *                              apply <titlebar=none|transparent> as init option for enhancing title-bar feature
 *                              apply <dblclick=collapse> as init option for enhancing double-click feature
 *
 */
(function($){

	//default settings
	var defaults = {
		"maximize" : false,
		"minimize" : false,
		"dblclick" : false,
		"titlebar" : false,
		"icons" : {
			"maximize" : "ui-icon-extlink",
			"minimize" : "ui-icon-minus",
			"restore" : "ui-icon-newwin"
		},
		"events" : {
			"load" : null,
			"beforeCollapse" : null,
			"beforeMaximize" : null,
			"beforeMinimize" : null,
			"beforeRestore" : null,
			"collapse" : null,
			"maximize" : null,
			"minimize" : null,
			"restore" : null
		}
	};

	//plugin settings (will be modified during init)
	var settings;

	//plubic methods
	var methods = {

		"init" : function( options ){
			var self = this;
			//validation
			if ( !$(self).dialog ) {
				$.error( "jQuery.dialogExtend Error : Only jQuery UI Dialog element is accepted" );
			}
			//merge defaults & options, without modifying the defaults
			options = options || {};
			options.icons = options.icons || {};
			options.events = options.events || {};
			settings = $.extend({}, defaults, options);
			settings.icons = $.extend({}, defaults.icons, options.icons);
			settings.events = $.extend({}, defaults.events, options.events);
			//initiate plugin...
			$(self).each(function(){
				$(this)
					//set default dialog state
					.data("dialog-state", "normal")
					//do bunch of things...
					.dialogExtend("_verifySettings")
					.dialogExtend("_initEvents")
					.dialogExtend("_initStyles")
					.dialogExtend("_initButtons")
					.dialogExtend("_initTitleBar")
					//trigger custom event when done
					.dialogExtend("_trigger", "load");
			});
			//maintain chainability
			return self;
		},

		"collapse" : function(){
			var self = this;
			//calculate new dimension
			var newHeight = $(this).dialog("widget").find(".ui-dialog-titlebar").height()+15;
			//start!
			$(self)
				//trigger custom event
				.dialogExtend("_trigger", "beforeCollapse")
				//remember original state
				.dialogExtend("_saveSnapshot")
				//mark new state
				.data("dialog-state", "collapsed")
				//modify dialog size (after hiding content)
				.dialog("option", {
					"height" : newHeight,
					"maxHeight" : newHeight
				})
				//hide content
				//hide button-pane
				//make title-bar no-wrap
				.hide()
				.dialog("widget")
					.find(".ui-dialog-buttonpane:visible").hide().end()
					.find(".ui-dialog-titlebar").css("white-space", "nowrap").end()
				.find(".ui-dialog-content")
				//trigger custom event
				.dialogExtend("_trigger", "collapse");
			//maintain chainability
			return self;
		},

		"maximize" : function(){
			var self = this;
			//caculate new dimension
			var newHeight = $(window).height()-11;
			var newWidth = $(window).width()-11;
			//start!
			$(self)
				//trigger custom event
				.dialogExtend("_trigger", "beforeMaximize")
				//remember original state
				.dialogExtend("_saveSnapshot")
				//mark new state
				.data("dialog-state", "maximized")
				//modify dialog button
				.dialogExtend("_toggleButtons")
				//fix dialog from scrolling
				.dialog("widget")
					.css("position", "fixed")
				.find(".ui-dialog-content")
				//show content
				//show button-pane (when minimized/collapsed)
				.show()
				.dialog("widget")
					.find(".ui-dialog-buttonpane").show().end()
				.find(".ui-dialog-content")
				//modify dialog with new config
				.dialog("option", {
					"resizable" : false,
					"draggable" : false,
					"height" : newHeight,
					"width" : newWidth,
					"position" : [1, 1]
				})
				//disable draggable-handle (for <titlebar=none> only)
				.dialog("widget")
					.draggable("option", "handle", null)
					.find(".ui-dialog-draggable-handle").css("cursor", "text").end()
				.find(".ui-dialog-content")
				//trigger custom event
				.dialogExtend("_trigger", "maximize");
			//maintain chainability
			return self;
		},

		"minimize" : function(){
			var self = this;
			var container = "#minimized-dialog-container";
			//caculate new dimension
			var newHeight = $(this).dialog("widget").find(".ui-dialog-titlebar").height()+15;
			var newWidth = 200;
			//create container for (multiple) minimized dialogs (when necessary)
			if ( !$(container).length ) {
				$("<div />")
					.attr("id", container.replace("#", ""))
					.css({ "left" : 1, "bottom" : 1, "position" : "fixed" })
					.appendTo("body");
			}
			//start!
			$(self)
				//trigger custom event
				.dialogExtend("_trigger", "beforeMinimize")
				//remember original state
				.dialogExtend("_saveSnapshot")
				//mark new state
				.data("dialog-state", "minimized")
				//modify dialog button
				.dialogExtend("_toggleButtons")
				//move dialog from body to container
				.dialog("widget")
					.css({
						"float" : "left",  //float is essential for stacking
						"margin" : 1,
						"position" : "static"
					})
					.appendTo(container)
				.find(".ui-dialog-content")
				//modify dialog with new config
				.dialog("option", {
					"resizable" : false,
					"draggable" : false,
					"height" : newHeight,
					"width" : newWidth
				})
				//hide content
				//hide button-pane
				//make title-bar no-wrap
				.hide()
				.dialog("widget")
					.find(".ui-dialog-buttonpane:visible").hide().end()
					.find(".ui-dialog-titlebar").css("white-space", "nowrap").end()
				.find(".ui-dialog-content")
				//disable draggable-handle (for <titlebar=none> only)
				.dialog("widget")
					.draggable("option", "handle", null)
					.find(".ui-dialog-draggable-handle").css("cursor", "text").end()
				.find(".ui-dialog-content")
				//trigger custom event
				.dialogExtend("_trigger", "minimize");
			//maintain chainability
			return self;
		},

		"restore" : function(){
			var self = this;
			var beforeState = $(self).data("dialog-state");
			//start!
			$(self)
				//trigger custom event
				.dialogExtend("_trigger", "beforeRestore")
				//mark new state
				.data("dialog-state", "normal")
				//restore dialog button
				.dialogExtend("_toggleButtons")
				//restore dialog according to previous state
				.dialogExtend(
					beforeState == "maximized" ? "_restoreFromMaximized" :
					beforeState == "minimized" ? "_restoreFromMinimized" :
					beforeState == "collapsed" ? "_restoreFromCollapsed" :
					$.error( "jQuery.dialogExtend Error : Cannot restore dialog from unknown state '" + beforeState +"'" )
				)
				//trigger custom event
				.dialogExtend("_trigger", "restore");
			//maintain chainability
			return self;
		},

		"_initButtons" : function(){
			var self = this;
			//start operation on titlebar
			var titlebar = $(self).dialog("widget").find(".ui-dialog-titlebar");
			$(titlebar)
				.append('<a class="ui-dialog-titlebar-maximize ui-corner-all" href="#"><span class="ui-icon '+settings.icons.maximize+'">maximize</span></a>')
				.append('<a class="ui-dialog-titlebar-minimize ui-corner-all" href="#"><span class="ui-icon '+settings.icons.minimize+'">minimize</span></a>')
				.append('<a class="ui-dialog-titlebar-restore ui-corner-all" href="#"><span class="ui-icon '+settings.icons.restore+'">restore</span></a>')
				//add effect to buttons
				.find(".ui-dialog-titlebar-maximize,.ui-dialog-titlebar-minimize,.ui-dialog-titlebar-restore")
					.attr("role", "button")
					.mouseover(function(){ $(this).addClass("ui-state-hover"); })
					.mouseout(function(){ $(this).removeClass("ui-state-hover"); })
					.focus(function(){ $(this).addClass("ui-state-focus"); })
					.blur(function(){ $(this).removeClass("ui-state-focus"); })
				.end()
				//default show buttons
				//set button positions
				//on-click-button
				.find(".ui-dialog-titlebar-maximize")
					.toggle(settings.maximize)
					.css({ "right" : settings.maximize ? "1.4em" : "-9999em" })
					.click(function(e){
						e.preventDefault();
						$(self).dialogExtend("maximize");
					})
				.end()
				.find(".ui-dialog-titlebar-minimize")
					.toggle(settings.minimize)
					.css({ "right" : settings.maximize ? "2.5em" : settings.minimize ? "1.4em" : "-9999em" })
					.click(function(e){
						e.preventDefault();
						$(self).dialogExtend("minimize");
					})
				.end()
				.find(".ui-dialog-titlebar-restore")
					.hide()
					.css({ "right" : "-9999em" })
					.click(function(e){
						e.preventDefault();
						$(self).dialogExtend("restore");
					})
				.end()
				//on-dblclick-titlebar : maximize/minimize/collapse/restore
				.dblclick(function(evt){
					if ( settings.dblclick && settings.dblclick.length ) {
						$(self).dialogExtend( $(self).data("dialog-state") != "normal" ? "restore" : settings.dblclick );
					}
				})
				//avoid text-highlight when double-click
				.each(function(){
					$(this)
						//.attr("unselectable", "on")
						//.css({ "-moz-user-select" : "none", "-khtml-user-select" : "none" })
						.select(function(){ return false; });
				});
			//maintain chainability
			return self;
		},

		"_initEvents" : function(){
			var self = this;
			//bind event callbacks which specified at init
			$.each(settings.events, function(type){
				if ( $.isFunction( settings.events[type] ) ) {
					$(self).bind(type+".dialogExtend", settings.events[type]);
				}
			});
			//maintain chainability
			return self;
		},

		"_initStyles" : function(){
			var self = this;
			//append styles for this plugin to body
			var style = '';
			style += '<style type="text/css">';
			style += '.ui-dialog .ui-dialog-titlebar-maximize,';
			style += '.ui-dialog .ui-dialog-titlebar-minimize,';
			style += '.ui-dialog .ui-dialog-titlebar-restore { position: absolute; top: 50%; width: 19px; margin: -10px 0 0 0; padding: 1px; height: 18px; }';
			style += '.ui-dialog .ui-dialog-titlebar-maximize span,';
			style += '.ui-dialog .ui-dialog-titlebar-minimize span,';
			style += '.ui-dialog .ui-dialog-titlebar-restore span { display: block; margin: 1px; }';
			style += '.ui-dialog .ui-dialog-titlebar-maximize:hover,';
			style += '.ui-dialog .ui-dialog-titlebar-maximize:focus,';
			style += '.ui-dialog .ui-dialog-titlebar-minimize:hover,';
			style += '.ui-dialog .ui-dialog-titlebar-minimize:focus,';
			style += '.ui-dialog .ui-dialog-titlebar-restore:hover,';
			style += '.ui-dialog .ui-dialog-titlebar-restore:focus { padding: 0; }';
			style += '.ui-dialog .ui-dialog-titlebar ::selection { background-color: transparent; }';
			style += '</style>';
			$(style).appendTo("body");
			//maintain chainability
			return self;
		},

		"_initTitleBar" : function(){
			var self = this;
			//modify title bar
			switch ( settings.titlebar ) {
				case false:
					//do nothing
					break;
				case "none":
					//create new draggable-handle as substitute of title bar
					if ( $(self).dialog("option", "draggable") ) {
						var handle = $("<div />").addClass("ui-dialog-draggable-handle").css("cursor", "move").height(5);
						$(self).dialog("widget").prepend(handle).draggable("option", "handle", handle);
					}
					//remove title bar and keep it draggable
					$(self)
						.dialog("widget")
						.find(".ui-dialog-titlebar")
							//clear title text
							.find(".ui-dialog-title").html("&nbsp;").end()
							//keep buttons at upper-right-hand corner
							.css({
								"background-color" : "transparent",
								"background-image" : "none",
								"border" : 0,
								"position" : "absolute",
								"right" : 0,
								"top" : 0,
								"z-index" : 9999
							})
						.end();
					break;
				case "transparent":
					//remove title style
					$(self)
						.dialog("widget")
						.find(".ui-dialog-titlebar")
						.css({
							"background-color" : "transparent",
							"background-image" : "none",
							"border" : 0
						});
					break;
				default:
					$.error( "jQuery.dialogExtend Error : Invalid <titlebar> value '" + settings.titlebar + "'" );
			}
			//maintain chainability
			return self;
		},

		"_loadSnapshot" : function(){
			var self = this;
			return {
				"config" : {
					"resizable" : $(self).data("original-config-resizable"),
					"draggable" : $(self).data("original-config-draggable")
				},
				"size" : {
					"height" : $(self).data("original-size-height"),
					"width"  : $(self).data("original-size-width"),
					"maxHeight" : $(self).data("original-size-maxHeight")
				},
				"position" : {
					"mode" : $(self).data("original-position-mode"),
					"left" : $(self).data("original-position-left"),
					"top"  : $(self).data("original-position-top")
				},
				"titlebar" : {
					"wrap" : $(self).data("original-titlebar-wrap")
				}
			};
		},

		"_restoreFromCollapsed" : function(){
			var self = this;
			var original = $(this).dialogExtend("_loadSnapshot");
			//restore dialog
			$(self)
				//show content
				//show button-pane
				//fix title-bar wrap
				.show()
				.dialog("widget")
					.find(".ui-dialog-buttonpane:hidden").show().end()
					.find(".ui-dialog-titlebar").css("white-space", original.titlebar.wrap).end()
				.find(".ui-dialog-content")
				//restore config & size
				.dialog("option", {
					"height" : original.size.height,
					"maxHeight" : original.size.maxHeight
				});
			//maintain chainability
			return self;
		},

		"_restoreFromMaximized" : function(){
			var self = this;
			var original = $(this).dialogExtend("_loadSnapshot");
			//restore dialog
			$(self)
				//free dialog from scrolling
				//fix title-bar wrap (if dialog was minimized/collapsed)
				.dialog("widget")
					.css("position", original.position.mode)
					.find(".ui-dialog-titlebar").css("white-space", original.titlebar.wrap).end()
				.find(".ui-dialog-content")
				//restore config & size & position
				.dialog("option", {
					"resizable" : original.config.resizable,
					"draggable" : original.config.draggable,
					"height" : original.size.height,
					"width" : original.size.width,
					"maxHeight" : original.size.maxHeight,
					"position" : [ original.position.left, original.position.top ]
				})
				//restore draggable-handle (for <titlebar=none> only)
				.dialog("widget")
					.draggable("option", "handle", $(this).find(".ui-dialog-draggable-handle"))
					.find(".ui-dialog-draggable-handle")
					.css("cursor", "move");
			//maintain chainability
			return self;
		},

		"_restoreFromMinimized" : function(){
			var self = this;
			var original = $(this).dialogExtend("_loadSnapshot");
			var container = "#minimized-dialog-container";
			//restore dialog
			$(self)
				//move dialog back from container to body
				.dialog("widget")
					.appendTo("body")
					.css({
						"float" : "none",
						"margin" : 0,
						"position" : original.position.mode
					})
				.find(".ui-dialog-content")
				//show content
				//show button-pane
				//fix title-bar wrap
				.show()
				.dialog("widget")
					.find(".ui-dialog-buttonpane:hidden").show().end()
					.find(".ui-dialog-titlebar").css("white-space", original.titlebar.wrap).end()
				.find(".ui-dialog-content")
				//restore config & size & position
				.dialog("option", {
					"resizable" : original.config.resizable,
					"draggable" : original.config.draggable,
					"height" : original.size.height,
					"width" : original.size.width,
					"maxHeight" : original.size.maxHeight,
					"position" : [ original.position.left, original.position.top ]
				})
				//restore draggable-handle (for <titlebar=none> only)
				.dialog("widget")
					.draggable("option", "handle", $(this).find(".ui-dialog-draggable-handle"))
					.find(".ui-dialog-draggable-handle")
					.css("cursor", "move");
			//maintain chainability
			return self;
		},

		"_saveSnapshot" : function(){
			var self = this;
			//remember all configs under normal state
			if ( $(self).data("dialog-state") == "normal" ) {
				$(self)
					.data("original-config-resizable", $(self).dialog("option", "resizable"))
					.data("original-config-draggable", $(self).dialog("option", "draggable"))
					.data("original-size-height", $(self).dialog("widget").height())
					.data("original-size-width", $(self).dialog("option", "width"))
					.data("original-size-maxHeight", $(self).dialog("option", "maxHeight"))
					.data("original-position-mode", $(self).dialog("widget").css("position"))
					.data("original-position-left", $(self).dialog("widget").offset().left)
					.data("original-position-top", $(self).dialog("widget").offset().top)
					.data("original-titlebar-wrap", $(self).dialog("widget").find(".ui-dialog-titlebar").css("white-space"));
			}
			//maintain chainability
			return self;
		},

		"_toggleButtons" : function(){
			var self = this;
			//show or hide buttons & decide position
			$(self).dialog("widget")
				.find(".ui-dialog-titlebar-maximize")
					.toggle( $(self).data("dialog-state") != "maximized" && settings.maximize )
				.end()
				.find(".ui-dialog-titlebar-minimize")
					.toggle( $(self).data("dialog-state") != "minimized" && settings.minimize )
				.end()
				.find(".ui-dialog-titlebar-restore")
					.toggle( $(self).data("dialog-state") != "normal" && ( settings.maximize || settings.minimize ) )
					.css({ "right" : $(self).data("dialog-state") == "maximized" ? "1.4em" : $(self).data("dialog-state") == "minimized" ? !settings.maximize ? "1.4em" : "2.5em" : "-9999em" })
				.end();
			//maintain chainability
			return self;
		},

		"_trigger" : function( type ){
			var self = this;
			//trigger event with namespace when user bind to it
			$(self).triggerHandler(type+".dialogExtend", this);
			//maintain chainability
			return self;
		},

		"_verifySettings" : function(){
			var self = this;
			//check <dblclick> option
			if ( !settings.dblclick ) {
			} else if ( settings.dblclick == "maximize" ) {
			} else if ( settings.dblclick == "minimize" ) {
			} else if ( settings.dblclick == "collapse" ) {
			} else {
				$.error( "jQuery.dialogExtend Error : Invalid <dblclick> value '" + settings.dblclick + "'" );
				settings.dblclick = false;
			}
			//check <titlebar> option
			if ( !settings.titlebar ) {
			} else if ( settings.titlebar == "none" ) {
			} else if ( settings.titlebar == "transparent" ) {
			} else {
				$.error( "jQuery.dialogExtend Error : Invalid <titlebar> value '" + settings.titlebar + "'" );
				settings.titlebar = false;
			}
			//maintain chainability
			return self;
		}

	};

	//core method
	$.fn.dialogExtend = function( method ){
		//method calling logic
		if ( methods[ method ] ) {
			return methods[ method ].apply( this, Array.prototype.slice.call( arguments, 1 ) );
    } else if ( typeof method === "object" || ! method ) {
      return methods.init.apply( this, arguments );
    } else {
      $.error( "jQuery.dialogExtend Error : Method <" + method + "> does not exist" );
    }
	};

}(jQuery));