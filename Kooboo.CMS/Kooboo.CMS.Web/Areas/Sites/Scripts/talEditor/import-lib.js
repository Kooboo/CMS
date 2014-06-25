(function () {
    var c = document.getElementById("kooboo-stuff-container");
    if (typeof (jQuery) != 'function') {
        var s = document.createElement('script');
        s.src = "/Scripts/jquery.js";
        c.appendChild(s);
    }
    if (typeof (ko) != 'object') {
        var s = document.createElement('script');
        s.src = "/Scripts/knockout.js";
        c.appendChild(s);
        document.head.app
    }
    setTimeout(function () {
        var editorJs = document.createElement('script');
        if (window.parent.__isLayout__) {
            editorJs.src = '/Areas/Sites/Scripts/talEditor/layout-editor.js';
        } else {
            editorJs.src = '/Areas/Sites/Scripts/talEditor/kooboo-editor.js';
        }
        c.appendChild(editorJs);
    }, 200);
})();