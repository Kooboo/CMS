<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<object>" %>
<% ViewData.TemplateInfo.HtmlFieldPrefix = ViewData.TemplateInfo.HtmlFieldPrefix.Replace(ViewData.ModelMetadata.PropertyName, "").Trim('.');
   var guid = Guid.NewGuid();
%>
<% 
    
%>
<tr id="tr-<%:guid %>">
    <td>
        <%: Html.TextArea(ViewData.ModelMetadata.PropertyName, Model == null ? "" : Model.ToString(), new { style="width:95%;", rows="25" })%>
        <% 
            if (!string.IsNullOrEmpty(ViewData.ModelMetadata.Description))
            {%>
        <a href="#" class="tooltip-link" title="<%: ViewData.ModelMetadata.Description.Localize() %>">
        </a>
        <%} %>
        <%: Html.ValidationMessage(ViewData.ModelMetadata,null) %>
    </td>
</tr>
<script language="javascript" type="text/javascript">
    function init() {
        var body = $('#tr-<%:guid %> textarea').codeMirror({
            onChange: function (e) {
                var status;
                if ($.popContext && $.popContext.getCurrent()) {
                    status = $.popContext.getCurrent().data('status');
                }
                if (e && e.historySize().undo > 0) {
                    if (status) {
                        status.stop();
                    }
                } else {
                    if (status)
                        status.pass();
                }
            },
            sizeProvider: false
        });
        function codeMirrorAPI() {
            return body.data("codeMirror");
        }

        function onLoad() {
            kooboo.cms.ui.loading().hide();
        }

        function setTextArea(txt) {
            if (codeMirrorAPI() == undefined) {
                return false;
            }
            kooboo.cms.ui.loading().show();
            if (txt && ((!body.val() || !body.val().trim().length))) {
                codeMirrorAPI().setValue(txt);
            } else {
                if (!codeMirrorAPI().getValue()) {
                    codeMirrorAPI().setValue(' ');
                }
                codeMirrorAPI().save();
            }
            codeMirrorAPI().focus(); codeMirrorAPI().refresh();
            setTimeout(function () { kooboo.cms.ui.loading().hide(); }, 100);
        }

        var fileName = $('#Name');

        var dic = {
            fn: {
                common: function () {
                    fileName.removeAttr('readonly');
                },
                '.rule': function () {
                    fileName.attr({ readonly: true, value: 'Theme' });
                },
                '.js': function () {
                    this.common();
                    codeMirrorAPI().setOption('mode', 'javascript');
                },
                '.css': function () {
                    this.common();
                    codeMirrorAPI().setOption('mode', 'css');
                },
                '.txt': function () {
                    this.common();
                    codeMirrorAPI().setOption('mode', 'text/html');
                }
            },
            option: {
                '.js': {

                    },
                    '.css': {

                },
                '.txt': {
                    path: '<%=Url.Content("~/Areas/Sites/Scripts/codeMirror/") %>'
                },
                '.rule': {
                    valueText: '<!--[if lte IE 6]><link rel="stylesheet" type="text/css" href="msie.css" /><![endif]-->\r\n'
                }
            },
            handle: function (extension) {
                var quickExp = /.js|.css|.txt|.rule/i;

                var group = quickExp.exec(extension);

                setTextArea(this.option[group].valueText);

                onLoad();

                if (codeMirrorAPI() == undefined) {
                    return false;
                }

                this.fn[group]();
            }
        };
        var fileExtension = $('#FileExtension');
        if (fileExtension.length) {
            var quickExp = /.js|.css|.txt|.rule/;

            fileExtension.change(function () {
                dic.handle($(this).val());
            });

            var group = quickExp.exec(fileExtension.val());
            if (group) {
                dic.handle(fileExtension.val());
                var form = fileExtension.parents('form');
                kooboo.cms.ui.event.ajaxSubmit(function () {
                    if (codeMirrorAPI() != undefined) {
                        codeMirrorAPI().save();
                    } else {
                        body.val(body.next().text());
                    }
                });
            }
        }
    };
$(init);
</script>
