<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Blank.Master"
    Inherits="System.Web.Mvc.ViewPage<Kooboo.CMS.Content.Models.MediaContent>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%
        System.Drawing.Image image = System.Drawing.Image.FromFile(Model.PhysicalPath);
        var imagePreviewUrl = Url.Action("Preview", "MediaContent", ViewContext.RequestContext.AllRouteValues().Merge("imagePath", Model.VirtualPath));
    %>
    <div class="common-form" id="J_ImageTab">
        <ul class="tabs clearfix">
            <li class="current"><a href="#size"><%:"Size".Localize()%></a></li>
            <li><a href="#metadata"><%:"Metadata".Localize()%></a></li>
        </ul>
        <div class="tab-content" id="size">

            <%using (Html.BeginForm("EditImage", "MediaContent", ViewContext.RequestContext.AllRouteValues(), FormMethod.Post, new RouteValueDictionary(new { id = "editImageForm", @class = "no-ajax" })))
              { %>

            <input type="hidden" id="rotateTypes" name="rotateTypes" />
            <fieldset>
                <legend><%:"Image size".Localize() %></legend>
                <table>
                    <tr>
                        <th>
                            <label><%: "Original dimensions".Localize()%></label></th>
                        <td>
                            <div id="originaldimensions"><%: image.Width%> X <%:image.Height%></div>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label><%:"Image crop".Localize()%></label></th>
                        <td>
                            <div class="image-crop">
                                <div class="actions">
                                    <ul>
                                        <li><a href="#" class="o-icon rotateFlip anticlockwise" rotatetype="1" title="Rotate anticlockwise">Rotate anticlockwise</a></li>
                                        <li><a href="#" class="o-icon rotateFlip clockwise" rotatetype="2" title="Rotate clockwise">Rotate clockwise</a></li>
                                        <li><a href="#" class="o-icon rotateFlip flip-vertically" rotatetype="3" title="Flip vertically">Flip vertically</a></li>
                                        <li><a href="#" class="o-icon rotateFlip flip-horizontally" rotatetype="4" title="Flip horizontally">Flip horizontally</a></li>
                                        <li><a href="#" class="o-icon undo" title="Undo">Undo</a></li>
                                        <li><a href="#" class="o-icon redo" title="Redo">Redo</a></li>
                                    </ul>
                                    <ul class="image-info">
                                        <li>
                                            <label><%="X".Localize()%>:</label>
                                            <input type="text" id="Crop_X" name="Crop.X" readonly="readonly" class="mini" />
                                        </li>
                                        <li>
                                            <label><%= "Y".Localize()%>:</label>
                                            <input type="text" id="Crop_Y" name="Crop.Y" readonly="readonly" class="mini" />
                                        </li>
                                        <li>
                                            <label><%= "Width".Localize()%>:</label>
                                            <input type="text" id="Crop_Width" name="Crop.Width" readonly="readonly" class="mini" />
                                        </li>
                                        <li>
                                            <label><%= "Height".Localize()%>:</label>
                                            <input type="text" id="Crop_Height" name="Crop.Height" readonly="readonly" class="mini" />
                                        </li>
                                        <li><a class="o-icon cross" id="ClearCrop" href="#" title="Clear crop"><%:"Clear crop".Localize()%></a></li>
                                    </ul>
                                </div>
                                <img id="jcrop_target" src="<%= imagePreviewUrl + "&t=" + DateTime.Now.Ticks.ToString()%>"
                                    alt="<%: Model.Metadata == null ? "" : Model.Metadata.AlternateText%>" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <label><%:"Scale Image".Localize()%></label></th>
                        <td>
                            <label>
                                <%="Width".Localize()%>:</label>
                            <input type="text" id="Scale_Width" name="Scale.Width" class="mini" />

                            <label>
                                <%= "Height".Localize()%>:</label>
                            <input type="text" id="Scale_Height" name="Scale.Height" class="mini" />
                            <a class="o-icon cross" id="ClearScale" href="#" title="Clear scale"><%:"Clear scale".Localize()%></a>
                        </td>
                    </tr>
                </table>
            </fieldset>
            <p class="buttons">
                <button type="submit">
                    <%:"Save image".Localize() %></button>
            </p>
            <%} %>
        </div>
        <div class="tab-content" id="metadata">
            <% using (Html.BeginForm(ViewContext.RequestContext.AllRouteValues().Merge("action", "EditMetadata")))
               { %>
            <fieldset>
                <legend><%:"Image metadata".Localize() %></legend>
                <table>
                    <tbody>
                        <%:Html.EditorFor(m => m.Metadata.AlternateText, new { @class = "long" })%>
                        <%:Html.EditorFor(m => m.Metadata.Description)%>
                    </tbody>
                </table>
            </fieldset>
            <p class="buttons">
                <button type="submit">
                    <%:"Save metadata".Localize() %></button>
            </p>
            <%} %>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#J_ImageTab').koobooTab();
            var width = <%:image.Width %> ;
            var height = <%:image.Height%> ;
            var width_height_ratio = width / height;
            var jcrop_api;
            var rotates = [];
            var undoRotates = [];
            var imagePreviewUrl = '<%= imagePreviewUrl%>'
            cropChange = function (c) {
                $('#Crop_X').val(c.x);
                $('#Crop_Y').val(c.y);
                $('#Crop_Width').val(c.w);
                $('#Crop_Height').val(c.h);
            };
            $('#jcrop_target').Jcrop({
                onChange: cropChange,
                trueSize: [width,height]
            }, function () {
                jcrop_api = this;
            });
            $('#ClearCrop').click(function () {
                jcrop_api.release();
                $('#Crop_X').val('');
                $('#Crop_Y').val('');
                $('#Crop_Width').val('');
                $('#Crop_Height').val('');
                return false;
            });
            $('#ClearScale').click(function () {
                $('#Scale_Width').val('');
                $('#Scale_Height').val('');
                return false;
            });

            reloadImage = function () {
                var types = rotates.join(',');
                $('#rotateTypes').val(types);
                jcrop_api.setImage(imagePreviewUrl + "&rotateTypes=" + types + "&t=" + (new Date()).getTime());
            };
            $('.rotateFlip').click(function () {
                var element = $(this);
                var $jcrop_target = $("#jcrop_target");
                rotates.push(element.attr("rotatetype"));
                reloadImage();
            });
            $('.undo').click(function () {
                if (rotates.length > 0) {
                    undoRotates.push(rotates.pop());
                    reloadImage();
                }
            });
            $('.redo').click(function () {
                if (undoRotates.length > 0) {
                    rotates.push(undoRotates.pop());
                    reloadImage();
                }
            });
            $('#Scale_Width').change(function(){
                var input = $(this);
                var val = input.val();
                if (val.trim()!='') {
                    var width = parseFloat(val);                    
                    var height = Math.round( width / width_height_ratio);
                    $('#Scale_Height').val(height);
                }
                
            });
            $('#Scale_Height').change(function(){
                var input = $(this);
                var val = input.val();
                if (val.trim()!='') {
                    var height = parseFloat(val);
                    var width = Math.round(height * width_height_ratio);
                    $('#Scale_Width').val(width);
                }
                
            });
            $('#editImageForm').ajaxForm({
                success: function (response) {
                    kooboo.cms.ui.messageBox().showResponse(response);
                    $("#originaldimensions").html(response.Model.Width + ' X ' + response.Model.Height);
                    jcrop_api.setImage(imagePreviewUrl);
                    $('#J_ImageTab').data('koobooTab').showTab(1);
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ScriptCSS" runat="server">
    <link href="<%=Url.Content("~/Styles/jQuery.JCrop/jquery.Jcrop.css") %>" rel="Stylesheet"
        type="text/css" />
    <script language="javascript" type="text/javascript" src="<%=Url.Content("~/Scripts/jquery.Jcrop.js") %>"></script>
</asp:Content>
