/// <reference path="jquery-1.4.1-vsdoc.js" />
/// <reference path="extension/js.extension.js" />

(function (window) {
    if (window.kooboo) {
        return kooboo;
    }
    var _k = {},
    kooboo = function (dom) {
        ///<summary>
        /// kooboo JS Frame
        /// by kooboo.namespace("adminJs.global"); adminJs.global is avaliable 
        /// adminJs.global.extend({ sayHello: function(){ alert('hello ') }});
        /// adminJs.global.extend({ sayHi: function(){ alert('hi ') }});
        /// adminJs.global.extend({ ready: function(){ alert('i am going here! ') }});//ready method will execute immediately
        /// kooboo.namespace("adminJs.fileJs");
        /// adminJs.fileJs.extend({ sayHello: function(){ alert('hello ') }});
        /// kooboo.using(adminJs.global,function(global){
        ///     sayHello();
        ///     global.sayHello();
        /// });
        /// kooboo.using([adminJs.global,adminJs.fileJs],function(global,fileJs){
        ///     fileJs.sayHello();
        ///     global.sayHello();
        ///     sayHi();
        /// });
        ///</summary>


        var _kooboo = {};

        _kooboo.ready = (function () {
            dom = dom ? dom : window;

            dom.readyList = dom.readyList ? dom.readyList : [];

            function ready() {
                for (var i = 0; i < dom.readyList.length; i++) {
                    if (typeof (dom.readyList[i]) == 'function') {
                        dom.readyList[i]();
                    } //end if(typeof (dom.readyList[i]) == 'function')
                } //end for
            } //end function ready()

            var tagName = dom.tagName ? dom.tagName.trim().toLocaleLowerCase() : "";
            switch (tagName) {
                case "iframe":
                    {
                        if (window.attachEvent) {
                            dom.onreadystatechange = function () {
                                if (dom.readyState == "complete") {
                                    ready();
                                }
                            };
                        } else if (window.addEventListener) {
                            //dom.addEventListener("onload",ready);
                            dom.onload = ready;
                        }
                        break;
                    } //end case "iframe"
                default:
                    {
                        if (window.attachEvent) {
                            dom.attachEvent("onload", ready); //for ie
                        } else if (window.addEventListener) {
                            dom.addEventListener("load", ready, false); //for fireforx an webkit
                        }
                        break;
                    } //end default
            } //end switch

            return function (func) {
                dom.readyList.push(func);
            }
        })(); //end _kooboo.ready

        return _kooboo;
    },
    _extendMethod = function (obj) {
        if (typeof (obj) == 'function' || typeof (obj) == 'object') {
            obj.extend = function (objx, objx2) {
                if (objx) {
                    if (objx2) {
                        if (typeof (objx) == 'string') {
                            obj[objx] = objx2;
                        }
                    } else {
                        if (typeof (objx) == 'function' || typeof (objx) == 'object') {
                            for (var p in objx) {
                                obj[p] = objx[p];
                                _extendMethod(obj[p]);
                            } // end for
                            if (objx.ready && typeof (objx.ready) == 'function') {
                                kooboo.ready(function () {
                                    objx.ready.call(objx);
                                });
                            }
                        } //end if
                    } //end  if (typeof (objx) == 'string') 
                } //end if(objx)
            } // end extend
        } //end if
    };
    kooboo.namespace = function (spaceName, obj) {
        ///<summary>
        /// registing method or object to kooboo
        ///</summary>
        ///<param type="string" name="spaceName">
        /// create a namespace
        ///</param>

        if (spaceName && typeof (spaceName) == 'string') {
            ///
            function _creatObject(objName, obj) {
                if (objName.indexOf('.') > 0) {
                    var first = objName.substring(0, objName.indexOf("."));
                    if (!obj[first]) {
                        obj[first] = new Object();
                        _extendMethod(obj[first]);
                    }
                    _creatObject(objName.substring(objName.indexOf(".") + 1), obj[first]);
                } else {
                    if (!obj[objName]) {
                        obj[objName] = new Object();
                        _extendMethod(obj[objName]);
                    }
                }
            }
            _creatObject(spaceName, obj || window);
        }
        else {
            throw " typeof(spaceName) must be string!";
        }
    }
    kooboo.using = function (spaces, func) {
        ///<summary>
        /// registing method or object to kooboo
        ///</summary>
        ///<param type="object" name="spaceName">
        /// create a namespace
        ///</param>
        if (spaces instanceof Array) {
            var conflict = {};
            var count = spaces.length;
            for (var s in spaces) {
                for (var p in spaces[s]) {
                    if (window[p]) {
                        conflict[p] = spaces[s][p];
                    } else {
                        window[p] = spaces[s][p];
                    }
                }
            }
            func.apply(this, spaces);
            for (var s in spaces) {
                for (var p in s) {
                    if (!conflict[p]) {
                        delete window[p];
                    }
                }
            }
        } else {
            var conflict = {};
            for (var p in spaces) {
                if (window[p]) {
                    conflict[p] = spaces[p];
                } else {
                    window[p] = spaces[p];
                }
            }
            func(conflict);

            for (var p in spaces) {
                if (!conflict[p]) {
                    delete window[p];
                }
            }
        }
    }
    kooboo.extend = function (obj1, obj2) {
        ///<summary>
        /// this method can extend fJs
        ///</summary>
        ///<param type="object" name="obj1">
        /// fist object to extend if obj2 is null obj1's attribute will be extended to kooboo
        ///</param>
        ///<param type="object" name="obj2">
        /// extend obj2's attribute to obj1
        ///</param>
        ///<returns type="function">

        var obj1 = arguments[0];
        var obj2 = arguments[1];
        if (obj1) {
            if (obj2) {
                if (typeof (obj1) == 'string') {
                    kooboo[obj1] = obj2;
                    _extendMethod(kooboo[obj1]);
                }
                else if (typeof (obj1) == 'object' || typeof (obj1) == 'function') {
                    for (var p in obj2) {
                        obj1[p] = obj2[p];
                    }
                }
            }
            else {
                if (typeof (obj1) == 'object' || typeof (obj1) == 'function') {
                    for (var p in obj1) {
                        kooboo[p] = obj1[p];
                        _extendMethod(kooboo[p]);
                    }
                }
            }
        }


        return kooboo;
    }

    //------useful method in js----------------------
    kooboo.ready = (function () {
        //debugger;
        var readyList = [];

        function ready() {
            for (var i = 0; i < readyList.length; i++) {
                if (typeof (readyList[i]) == 'function') {
                    readyList[i]();
                }
            }
        }
        if (window.addEventListener) {
            window.addEventListener("load", ready, false);

        }
        else if (window.attachEvent) {
            window.attachEvent("onload", ready);
        }

        return function (func) {
            ///<sumary>
            ///this method will execute after dom ready
            ///</sumary>
            readyList.push(func);
        }
    })();

    ///Class kooboo.object
    var koobooObject = {
        getVal: function (obj, pStr) {
            if (pStr.indexOf('.') > 0) {
                var firstProp = pStr.substring(0, pStr.indexOf("."));

                var lastProp = pStr.substring(pStr.indexOf('.') + 1);
                if (firstProp.indexOf('[') >= 0) {
                    var index = firstProp.substring(firstProp.indexOf('[') + 1, firstProp.lastIndexOf(']'));
                    index = parseInt(index);
                    if (firstProp.indexOf('[') == 0) {
                        return this.getVal(obj[index], lastProp);
                    } else if (firstProp.indexOf('[') > 0) {
                        var propertyName = pStr.substring(0, pStr.indexOf('['));
                        return this.getVal(obj[propertyName][index], lastProp);
                    }
                } else {
                    var pObj = obj[firstProp];
                    return this.getVal(pObj, lastProp);
                }
            } else {
                if (pStr.indexOf('[') >= 0) {
                    var index = pStr.substring(pStr.indexOf('[') + 1, pStr.lastIndexOf(']'));
                    index = parseInt(index);
                    if (pStr.indexOf('[') == 0) {
                        return obj[index];
                    } else if (pStr.indexOf('[') > 0) {
                        var propertyName = pStr.substring(0, pStr.indexOf('['));
                        return obj[propertyName][index];
                    }
                } else {
                    return obj[pStr];
                }
            }
        },
        setVal: function (obj, pStr, val) {
            //debugger;
            //pStr = pStr.trim();
            if (pStr.indexOf('.') > 0) {
                var firstProp = pStr.substring(0, pStr.indexOf("."));

                var lastProp = pStr.substring(pStr.indexOf('.') + 1);

                if (firstProp.indexOf('[') >= 0) {
                    var index = firstProp.substring(firstProp.indexOf('[') + 1, firstProp.indexOf(']'));
                    index = parseInt(index);

                    if (firstProp.indexOf('[') == 0) {
                        if (!obj[index]) { obj[index] = {}; };
                        this.setVal(obj[index], lastProp, val);
                    } else if (firstProp.indexOf('[') > 0) {
                        var propertyName = pStr.substring(0, pStr.indexOf('['));

                        if (!obj[propertyName]) { obj[propertyName] = []; };

                        if (!obj[propertyName][index]) { obj[propertyName][index] = {}; };

                        this.setVal(obj[propertyName][index], lastProp, val);
                    }
                } else {
                    if (!obj[firstProp]) {
                        obj[firstProp] = {};
                    }
                    this.setVal(obj[firstProp], lastProp, val);
                }


            } else {
                var arrayReg = /\[\d*\]/;
                if (arrayReg.test(pStr)) {


                    var index = pStr.substring(pStr.indexOf('[') + 1, pStr.lastIndexOf(']'));

                    index = parseInt(index);
                    if (pStr.indexOf('[') == 0) {
                        obj[index] = val;
                    } else if (pStr.indexOf('[') > 0) {
                        var propertyName = pStr.substring(0, pStr.indexOf('['));
                        if (!obj[propertyName]) {
                            obj[propertyName] = [];
                        }
                        obj[propertyName][index] = val;
                    }
                } else {
                    obj[pStr] = val;
                }

            }
            return obj;
        }
    };


    kooboo.object = function (obj) {
        var api = {
            getVal: function (pStr) {
                return koobooObject.getVal(obj, pStr);
            },
            setVal: function (pStr, val) {
                return koobooObject.setVal(obj, pStr, val);
            }
        };
        return api;
    }


    kooboo.extend(kooboo.object, koobooObject);


    var _tempdata = {};
    kooboo.data = function (name, value) {
        if (arguments.length == 1) {
            try {
                var result = kooboo.object.getVal(_tempdata, name);
                return result;
            }
            catch (E) { }
            return null;
        } else if (arguments.length == 2) {
            kooboo.object.setVal(_tempdata, name, value);
        }
    }



    kooboo.log = window.console ? window.console.log : kooboo.console;

    //-------end us -useful method in js------------

    window.kooboo = kooboo;
    if (!top.kooboo) {
        top.kooboo = kooboo;
    }
})(window);        //end kooboo

(function extent_console() {
    kooboo.console = (function () {
        var api = {};

        var msglist = [];
        var index = 0;
        var win = (function initConsole() {
            var consoleWinStr = "<div></div>";
            var consoleWin = $(consoleWinStr);
            consoleWin.css("position", "absolute");
            consoleWin.css("width", "500px");
            consoleWin.css("zIndex", "100000");

            consoleWin.css("left", "30%");
            consoleWin.css("top", "20%");
            consoleWin.css("background-color", "#DDDDDD");
            consoleWin.css("color", "white");

            consoleWin.hide();


            var consoleTitleStr = "<div class='title'>&nbsp;&nbsp;&nbsp; Kooboo JS Console <span><a href='javascript:;' class='clear'><b> clear</b></a> <a href='javascript:;' class='close'><b>close</b>  &nbsp;&nbsp;&nbsp;</a>      </span>  </div>";
            var consoleTitle = $(consoleTitleStr).appendTo(consoleWin);

            consoleTitle.css("width", "100%");
            consoleTitle.css("cursor", "move");
            consoleTitle.css("height", "20px");
            consoleTitle.css("font-weight", "bold");
            consoleTitle.css("background-color", "#000000");
            consoleTitle.find('span').css('float', 'right').find("a.close").css('color', 'yellow').click(function () {
                api.quit();
            });
            consoleTitle.find('span').css('float', 'right').find("a.clear").css('color', 'yellow').click(function () {
                api.clear();
            });

            var consoleContentStr = "<div></div>";
            var consoleContent = $(consoleContentStr).appendTo(consoleWin);
            consoleContent.css("width", "98%");
            consoleContent.css("color", "black");
            consoleContent.css("padding", "5px");
            consoleContent.css("font-weight", "bold");
            consoleContent.css("height", "400px");
            consoleContent.css("overflow", "scroll");
            consoleWin.addClass("console");
            consoleTitle.addClass("title");
            consoleContent.addClass("content");

            $(document).ready(function () {
                consoleWin.appendTo("body");
                consoleWin.resizable();
                consoleWin.draggable({
                    handle: '.title'
                });
            });
            return {
                consloe: consoleWin,
                title: consoleTitle,
                content: consoleContent
            };
        })();

        var table = $("<table></table>").appendTo(win.content);

        table.css("width", "100%");
        api.write = function () {
            var msg = [];
            var tr = $("<tr></tr>").appendTo(table);
            for (var i = 0; i < arguments.length; i++) {
                msg.push(arguments[i]);
                var td = $("<td></td>").appendTo(tr);
                td.html(arguments[i]);
            }
            msglist.push(msg);
            if (win.consloe.css('display') == "none") {
                win.consloe.show();
            }

            win.content.get(0).scrollTop = win.content.get(0).scrollHeight;
            var writer = {
                setColor: function (foreColor, backColor) {
                    if (foreColor) {
                        tr.css('color', foreColor);
                        if (backColor) {
                            tr.css('backgroun-color', backColor);
                        }
                        return api;
                    }
                },
                setRed: function () {
                    return this.setColor('red');
                },
                setBlue: function () {
                    return this.setColor('blue');
                },
                setGreen: function () {
                    return this.setColor('green');
                },
                setYellow: function () {
                    return this.setColor('yellow');
                },
                setBlack: function () {
                    return this.setColor('black');
                }
            };

            return writer;
        }

        api.dump = function (arr) {
            if (arr instanceof Array) {
                api.write('array');
                api.write('index', 'value');
                for (var i = 0; i < arr.length; i++) {
                    api.write(i, arr[i]);
                }
            } else if (typeof arr == 'object') {
                api.write('object');
                api.write('property', 'value');
                for (var a in arr) {
                    api.write(a, arr[a]);
                }
            } else if (arr) {
                api.write('struct value');

                api.write(arr.toString());
            } else {
                api.write("undefine");
            }
        }
        kooboo.dump = api.dump;
        kooboo.print = api.write;
        api.clear = function () {
            msglist = [];
            table.html('');
            index = 0;
        }

        api.show = function () {
            win.consloe.show();
        }

        api.quit = api.exit = function () {
            table.html('');
            win.consloe.hide();
        }

        api.msgList = msglist;


        return api;

    })(); //end kooboo.console
})();

(function extend_aler_confirm() {
    kooboo.extend("alert", function (msg) {
        alert(msg);
    });


    kooboo.extend("confirm", function (msg, callback, title) {
        //Disable confirm if the message is null.
        if (msg == undefined || msg == '') {
            callback(true);
        }
        else
            callback(confirm(msg));
    });

})();

(function extend_date() {
    kooboo.extend("date", function (dateInput) {
        ///<summary>
        /// now only support utc 、 [date] input
        ///</summary>
        ///

        dateInput = dateInput || (new Date());

        var date;

        if (typeof (dateInput) == 'string') {
            date = kooboo.date.parse(dateInput);
        } else if (dateInput instanceof Date) {
            date = dateInput;
        }

        var api = {
            getDate: function () {
                return date;
            },
            format: function (format) {
                return date.toLocaleDateString();
            }
        };
    });

    kooboo.date.extend({
        parse: function (date) {
            for (var r in this.reg) {
                if (this.reg[r].test(date)) {
                    return this.parser[r](date);
                }
            }
        },
        reg: {
            utc: /Date\(\d+\)/    //type of date
        },
        parser: {
            utc: function (date) { // handle date type
            //debugger;
                var dateIntReg = /\d+/;
                var dateInt = parseInt(dateIntReg.exec(date)[0]);

                return new Date(dateInt);
            }
        }
    });
})();

(function extend_gethashcode() {
    var objReference = [];
    if (top.kooboo) {
        kooboo.getHash = top.kooboo.getHash;
    } else {
        kooboo.getHash = function (obj) {
            var query = objReference.where(function (val) { return val.obj == obj; }).first();
            if (query == null) {
                query = {
                    obj: obj,
                    key: Math.random()
                };
                objReference.push(query);
            }

            return query.key;
        }
    }


})();
