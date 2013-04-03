<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Contents/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%= "Multi-file uploader".Localize() %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <link href="<%:Url.Content("~/Styles/html5uploader/jquery.fileupload-ui.css")%>" type="text/css" />
    <script src="<%: Url.Content("~/Scripts/html5uploader/vendor/jquery.ui.widget.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/html5uploader/jquery.iframe-transport.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/html5uploader/jquery.fileupload.js") %>" type="text/javascript"></script>
    <script src="<%: Url.Content("~/Scripts/knockout-2.2.0.js") %>" type="text/javascript"></script>
    <h3 class="title">
        <%= "Multi-file uploader (" + ViewContext.RequestContext.AllRouteValues()["FolderName"]  +")".Localize() %></h3>
    <div class="block file-uploader">
        <div id="dropzone" class="dropzone">Drop files here<br /> or <br />
            <span class="upload-button">
                Select files
                <input id="fileupload" type="file" name="files[]" data-url="<%: Url.Action("Create",ViewContext.RequestContext.AllRouteValues()) %>" multiple />
            </span>
        </div>
        <ul class="file-list" id="statusTemplate" data-bind="foreach: files">
            <li>
                <span class="left" data-bind="text: filename"></span>
                <!-- ko if: progress() < 100 -->
                <span class="right progress">
                    <span class="value" data-bind="text: progress().toString()+'%'"></span>
                    <span class="bar" data-bind="style: {width:progress().toString()+'%'}"></span>
                </span>
                <!-- /ko -->
                <!-- ko if: progress()==100 -->
                    <!-- ko if: editUrl()!=null -->
                    <a data-bind="attr:{href: editUrl}" class="right o-icon edit dialog-link" title="Edit">Edit</a>
                    <!-- /ko -->
                <!-- /ko -->
            </li>
        </ul>
    </div>

    <%--<table border="1">
        <tbody data-bind="foreach: files">
            <tr>
                <td data-bind="text: filename"></td>
                <!-- ko if: progress<100 -->
                <td data-bind="text: progress"></td>
                <!-- /ko -->
                <!-- ko if: progress()==100 -->
                <td>
                    <!-- ko if: editUrl()!=null -->
                    <a data-bind="attr:{href: editUrl}" class="dialog-link">Edit</a>
                    <!-- /ko -->
                </td>
                <!-- /ko -->
            </tr>
        </tbody>
    </table>--%>
    <script type="text/javascript">
        $(function () {

            function FileListViewModel() {
                // Data
                var self = this;
                self.files = ko.observableArray([]);
            }
            var model = new FileListViewModel();
            ko.applyBindings(model, $('#statusTemplate')[0]);

            $('#fileupload').fileupload({
                autoUpload: true,
                dropZone: $('#dropzone'),
                dataType: 'json',

                add: function (e, data) {
                    $.each(data.files, function (index, file) {
                        var fileModel = { filename: ko.observable(file.name), progress: ko.observable(0), editUrl: ko.observable('') };
                        file.fileModel = fileModel;
                        model.files.push(fileModel);
                    });
                    data.submit();
                },
                progress: function (e, data) {

                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    data.files[0].fileModel.progress(progress);
                },
                done: function (e, data) {                   
                    data.files[0].fileModel.editUrl(data.result.RedirectUrl);
                    $('a.dialog-link').unbind('click').one('click', function (e) {
                        e.preventDefault();
                        var handle = $(this).pop({
                            width: 800,
                            height: 580,
                            frameHeight: "100%",
                            popupOnTop: true,
                            onclose: function () {                                
                            }
                        }).click();
                    });
                }
            });
        });
    </script>
</asp:Content>

