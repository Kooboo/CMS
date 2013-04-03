<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<Kooboo.CMS.Content.Services.CategoryContents>>" %>
<% if (Model.Count() > 0)
   { %>
<% foreach (var item in Model)
   {%>
<%: Html.Partial("~/Areas/Contents/Views/TextContent/EditorTemplates/Category.ascx", item,new ViewDataDictionary())%>
<%} %>
<script type="text/javascript">
    $(function () {

        var map = {};

        function initChecks(currentHandle) {
            var fullName = $.request(currentHandle.attr('href')).queryString["folderName"],
            displayId = "[name='cat_" + fullName + "_display']",
             valueId = "[name='cat_" + fullName + "_value']",
             singleChoiceId = "[name='cat_" + fullName + "_single_choice']",
             display = $(displayId),
             value = $(valueId),
             isSingle = $(singleChoiceId).val();

            if (!map[fullName]) {
                map[fullName] = [];
            }

            if (value.val()) {
                var selected = value.val().split(',');
                var displays = display.val().split(',');
                selected.each(function (val, index) {
                    var query = map[fullName].where(function (o, i) {
                        return o.key == val;
                    });
                    if (query.length == 0) {
                        map[fullName].push({
                            key: val,
                            text: displays[index]
                        });
                    }
                });
            }
        }



        $('.categoryButton').each(function () {
            var link = $(this);
            link.parent().find('input').dblclick(function () {
                link.click();
            });
            var id = new Date().getTime();
            link.pop({
                id: id,
                width: 700,

                height: 506,

                beforeLoad: initChecks,

                popupOnTop: true,

                onload: function (currentHandle, pop, config) {
                    var fullName = $.request(currentHandle.attr('href')).queryString["folderName"],
                     selectedDatas = map[fullName],

                     display = $("[name='cat_" + fullName + "_display']"),

                    value = $("[name='cat_" + fullName + "_value']"),

                    isSingle = $("[name='cat_" + fullName + "_single_choice']").val() == 'True',

                    iframe = pop.children('iframe'),

                    contents = iframe.contents(),

                    selectedTable = contents.find('table.selected tbody'),

                    datasourceTable = contents.find('table.datasource tbody'),

                    checkedSelector = selectedDatas.select(function (o) {
                        return '[value="' + o.key + '"]';
                    }).join(',');

                    datasourceTable.find(checkedSelector).attr('checked', true);

                    datasourceTable.find('tr').css({
                        cursor: 'pointer'
                    });

                    if (isSingle) {
                        handleRadio();
                    } else {
                        handleCheckbox();
                    }

                    function checkBoxChange() {
                        var handle = $(this),
                            tr = handle.parents('tr:eq(0)'),
                            key = handle.val(),
                            text = tr.find('td:eq(1)').text();

                        var query = map[fullName].where(function (o, i) {
                            return o.key == key;
                        });

                        if (handle.attr('checked')) {
                            if (query.length == 0) {
                                map[fullName].push({
                                    key: key,
                                    text: text
                                });
                            }
                        } else {
                            if (query.length > 0) {
                                map[fullName].remove(function (o, i) {
                                    return o.key == key;
                                });
                            }
                        }
                    }

                    function handleCheckbox() {
                        var checkboxs = datasourceTable.find('input:checkbox')
                        .change(checkBoxChange);
//                        datasourceTable.find('tr').each(function () {
//                            var tr = $(this), checkbox = tr.find(':checkbox');
//                            tr.find('td:gt(0)').click(function () {
//                                if (checkbox.attr('checked')) {
//                                    checkbox.attr('checked', false)
//                                } else {
//                                    checkbox.attr('checked', true)
//                                }
//                                checkBoxChange.call(checkbox);
//                            });
//                        });
                    }

                    function radioChange() {
                        var handle = $(this),
                            tr = handle.parents('tr:eq(0)'),
                            text = tr.find('td:eq(1)').text();

                        if (handle.attr('checked')) {
                            map[fullName] = [];
                            map[fullName].push({
                                key: handle.val(),
                                text: text
                            });
                        }
                    }
                    function handleRadio() {
                        var radios = datasourceTable.find('input:radio')
                        .change(radioChange);
//                        datasourceTable.find('tr').each(function () {
//                            var tr = $(this), radio = tr.find(':radio');
//                            tr.find('td:gt(0)').click(function () {
//                                radio.attr('checked', true);
//                                radioChange.call(radio);
//                            });
//                        });
                    }

                    (function initButton() {
                        var saveBtn = contents.find('.save'),
                        cancelBtn = contents.find('.cancel');
                        cancelBtn.click(function () {
                            pop.close();
                        });

                        saveBtn.click(function () {
                            var displayText = map[fullName].select(function (o) {
                                return o.text;
                            }).join(',');
                            var values = map[fullName].select(function (o) {
                                return o.key;
                            }).join(',');
                            value.val(values);
                            display.val(displayText);
                            pop.close();
                        });
                    })();
                }
            }); //end $().pop

            // resolve z-index problem when it's opend in front page by inline edit component.
            var topJQ = top._jQuery || top.jQuery;
            if (topJQ && top.yardi) {
                var contentCon = topJQ('#' + id);
                if (contentCon.length > 0) {
                    var oldZIndex = contentCon.dialog('option', 'zIndex');
                    contentCon.dialog('option', 'zIndex', top.yardi.zindexCenter.getMax(top) + oldZIndex + 1);
                }
            }
        });
    });    //end document.ready
</script>
<%}  %>