<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% var previewUrl = ViewData["PreviewUrl"] as string;
       var sourceUrl = ViewData["SourceUrl"] as string;
       if (!string.IsNullOrEmpty(sourceUrl))
       {
           sourceUrl = Url.Content(sourceUrl);
       }
       object cropParam = ViewData["CropParam"];
       var cropJSON = Kooboo.Web.Script.Serialization.JsonHelper.ToJSON(cropParam);
       
    %>
    <form action="<%=Url.Action("TempFile","TextContent", Request.RequestContext.AllRouteValues()) %>"
    method="post" enctype="multipart/form-data">
    <div class="common-form">
        <fieldset>
            <table>
                <tr>
                    <th>
                        <label>
                            <%:"Please upload an image".Localize()%>:</label>
                    </th>
                    <td>
                        <input type="file" name="File" id="file" />
                    </td>
                </tr>
            </table>
        </fieldset>
        <ul class="image-info">
            <li>
                <label>
                    <%= "X".Localize()%>:</label>
                <input type="text" id="X" readonly="readonly" /></li>
            <li>
                <label>
                    <%="Y".Localize()%>:</label>
                <input type="text" id="Y" readonly="readonly" /></li>
            <li>
                <label>
                    <%="Width".Localize()%>:</label>
                <input type="text" id="Width" readonly="readonly" /></li>
            <li>
                <label>
                    <%= "Height".Localize() %>:</label>
                <input type="text" id="Height" readonly="readonly" /></li>
        </ul>
        <div class="image-container">
            <img src="" style="display: none" alt="<%:"Crope".Localize() %>" id="crop-image" />
        </div>
        <p class="buttons clearfix">
            <button id="save">
                <%:"Save".Localize()%>
            </button>
            <button class="dialog-close">
                <%:"Cancel".Localize()%>
            </button>
        </p>
    </div>
    </form>
    <script language="javascript" type="text/javascript">
        kooboo.namespace('kooboo.cms.content.textcontent.crop');
        var jcropAPI, img;
        kooboo.cms.content.textcontent.crop.extend({
            init: function () {
                var _cropCls = this,
                counter = 0,
            imgStr = '<img/>',
            saveBtn = $('#save'),
            cropParam = top.kooboo.data('crop-param'),
            file = $('#file'),
            form = $('form'), img = $('#crop-image'),
            param = {},
            sourceWidth,
            sourceHeight,
            cropChange = function (c) {
                param['Width'] = parseInt(c.w / percent);
                param['Height'] = parseInt(c.h / percent);
                param['X'] = parseInt(c.x / percent);
                param['Y'] = parseInt(c.y / percent);
                $("#X").val(c.x);
                $("#Y").val(c.y);
                $("#Height").val(param['Height']);
                $("#Width").val(param['Width']);
            },
            imgContainerWidth = 640,
            imgContainerHeight = 400,
            jcropAPI,
            percent = 1,
            defaultImageUrl = '<%= sourceUrl%>',
            getSelect = function (percent, width, height) {
                return [width * 0.25, height * 0.25, width * 0.75, height * 0.75];
            },
            ValidateNum = function isNum(s) {
                return !isNaN(s);
            },
            uploadSuccess = function (response) {
                setTimeout(function () {
                    kooboo.cms.ui.loading().show();
                }, 30);
                loadImage(response.Model);
                return false;
            };
                loadImage = function (imageUrl) {
                    $('<img/>').hide().appendTo('body').attr('src', imageUrl)
                .load(function () {
                    kooboo.cms.ui.loading().hide();

                    img.attr('src', imageUrl).show();
                    var source = $(this);
                    setTimeout(function () {
                        sourceHeight = source.height(),
                    sourceWidth = source.width(),
                    param.Url = imageUrl,
                    croperWidth = img.width(),
                    croperHeight = img.height(),
                    sXY = sourceWidth / sourceHeight,
                    crXY = croperWidth / croperHeight,
                    ctXY = imgContainerWidth / imgContainerHeight;

                        if (sXY > ctXY) {
                            percent = croperWidth / sourceWidth;
                        } else {
                            if (sXY > 1) {
                                percent = croperWidth / sourceWidth;
                            } else {
                                percent = croperHeight / sourceHeight;
                            }
                        }

                        if (counter > 0) {
                            jcropAPI.destroy();
                        }

                        var cropSetting = _cropCls.settings.crop || {};
                        cropSetting['onChange'] = cropChange;
                        cropSetting['onSelect'] = cropChange;
                        if (cropSetting.minSize
                    && cropSetting.minSize[0]
                    && cropSetting.minSize[1]) {
                            cropSetting.minSize[0] = cropSetting.minSize[0] * percent;
                            cropSetting.minSize[1] = cropSetting.minSize[1] * percent;
                        }

                        jcropAPI = $.Jcrop(img, cropSetting);

                        var select = getSelect(percent, croperWidth, croperHeight);
                        if (select) {
                            jcropAPI.setSelect(select);
                        }


                        counter++;
                    }, 200);
                });
                };
                if (defaultImageUrl) {
                    loadImage(defaultImageUrl);
                }
                window.onSuccess = uploadSuccess;
                file.change(function () {
                    var file = $('input:file[name="File"]'), reg = /.jpg$|.png$/;
                    if (!reg.test(file.val())) {
                        kooboo.cms.ui.messageBox().show('<%:"Please upload jpg or png file.".Localize() %>', 'error');
                        return false;
                    }
                    form.submit();
                });
                saveBtn.click(function () {
                    var handle = top.kooboo.data('onCroped');
                    if (handle) {
                        handle(param);
                    }
                    $.popContext.getCurrent().close();
                });
                if (top.kooboo.data('crop-param')) {
                    uploadSuccess(top.kooboo.data('crop-param'));
                }
            },
            settings: {
                crop: {
                    //aspectRatio: 370 / 300,
                    //minSize: [370, 300]

                }
            }

        });
        $(function () {
            kooboo.cms.content.textcontent.crop.init();
        });
        $(function () {
            setTimeout(function () {
                window.ajaxFormParam.error = function () {
                    kooboo.cms.ui.messageBox().show('Upload image failed.', 'error');
                }
            }, 100);

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
    <link href="<%=Url.Content("~/Styles/jQuery.JCrop/jquery.Jcrop.css") %>" rel="Stylesheet"
        type="text/css" />
    <script language="javascript" type="text/javascript" src="<%=Url.Content("~/Scripts/jquery.Jcrop.js") %>"></script>
</asp:Content>
