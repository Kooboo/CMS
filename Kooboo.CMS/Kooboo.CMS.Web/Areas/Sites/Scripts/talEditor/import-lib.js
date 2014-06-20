(function () {
    if (typeof (jQuery) != 'function') {
        var s = document.createElement('script');
        s.src = "/Scripts/jquery.js";
        document.body.appendChild(s);
    }
    if (typeof (ko) != 'object') {
        var s = document.createElement('script');
        s.src = "/Scripts/knockout.js";
        document.body.appendChild(s);
    }
    var editorJs = document.createElement('script');
    editorJs.src = '/Areas/Sites/Scripts/talEditor/kooboo-editor.js';
    document.body.appendChild(editorJs);
})();