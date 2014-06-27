(function () {
    var container = document.getElementById("kooboo-stuff-container");
    if (typeof (jQuery) != 'function') {
        var s = document.createElement('script');
        s.src = "/Scripts/jquery.js";
        container.appendChild(s);
    }
    if (typeof (ko) != 'object') {
        var s = document.createElement('script');
        s.src = "/Scripts/knockout.js";
        container.appendChild(s);
    }
    var editorJs = document.createElement('script');
    if (window.parent.__isLayout__) {
        editorJs.src = '/Areas/Sites/Scripts/talEditor/layout-editor.js';
    } else {
        editorJs.src = '/Areas/Sites/Scripts/talEditor/view-editor.js';
    }
    container.appendChild(editorJs);
})();