<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<Kooboo.CMS.Content.Services.CategoryContents>>" %>
<%@ Import Namespace="Kooboo.CMS.Content.Models" %>
<% 
    var textFolder = new Kooboo.CMS.Content.Models.TextFolder();

    if (Model.Count() > 0)
    { %>
<% foreach (var item in Model)
   {%>
<%: Html.Partial("~/Areas/Contents/Views/TextContent/EditorTemplates/Category.ascx", item,new ViewDataDictionary())%>
<%} %>
<script type="text/javascript">
    $(function () {
        var mapCacheKey = 'category_selected_cache' + Math.random()
            , map = kooboo.data(mapCacheKey);
        kooboo.data(mapCacheKey, map = {});

        function initMapData(currentHandle) {
            var $parentTd = currentHandle.parents('td:eq(0)'),
            $display = $parentTd.find('[name$="_display"]'),
            $value = $parentTd.find('[name$="_value"]'),
            isSingle = $parentTd.find('[name$="_single_choice"]').val(),
            rootFolder = $.request(currentHandle.attr('href')).queryString["folderName"];

            if (!map[rootFolder]) {
                map[rootFolder] = [];
            }

            if ($value.val()) {
                var selected = $value.val().split(',');
                var displays = $display.val().split(',');

                selected.each(function (folderAndValue, index) {

                    var displayArr = displays[index].split(':');

                    var valArr = folderAndValue.split(':');

                    var currentFolder = valArr[1] || rootFolder;

                    var val = valArr[0];

                    var query = map[rootFolder].where(function (o, i) {
                        return o.key == val;
                    });

                    if (query.length == 0) {
                        map[rootFolder].push({
                            key: val,
                            text: displayArr[0],
                            currentFolder: currentFolder,
                            folderText: displayArr[1] || currentFolder,
                            selected: true
                        });
                    }
                });
            }
        }

        function toggleBackLink($table, rootFolder, currentFolder, prevLink) {
            var str = '<tbody class="back-to-parent">\
                            <tr class="folderTr initialized" style="cursor: pointer;">\
                                <td>\
                                </td>\
                                <td>\
                                        <a class="f-icon folder" href="'+ prevLink + '">...</a>\
                                </td>\
                                <td colspan="2">\
                                </td> \
                            </tr>\
                    </tbody>';

            if ($table.find('tbody.back-to-parent').length == 0
                && (rootFolder != currentFolder || !rootFolder)) {
                $(str).prependTo($table);
            }
        }

        $('.categoryButton').each(function () {

            var link = $(this);

            var rootFolder = $.request(link.attr('href')).queryString["folderName"];

            initMapData(link);

            initDisplayAndValueInput(link);

            var id = new Date().getTime()
            , destroyMethod = function () { }
            , counter = 0
            , _serializeJSON
            , _prevLink;

            link.pop({
                id: id,

                onclose: function () {
                    counter = 0;

                    var serialized = $.parseJSON(_serializeJSON);

                    map[rootFolder].each(function (o) {
                        var old = serialized.where(function (s) {
                            return o.key == s.key;
                        })[0];

                        if (old) {
                            o.selected = old.selected;
                        } else {
                            o.selected = false;
                        }
                    });
                    setTimeout(function () { destroyMethod(); }, 16);
                },

                width: 700,

                height: 506,

                popupOnTop: true,

                onload: function (currentHandle, pop, config) {

                    if (iframeSrc == 'about:blank') {
                        return false;
                    }

                    if (counter++ == 0) {
                        _serializeJSON = $.toJSON(map[rootFolder]);
                    }

                    var iframe = pop.children('iframe'),

                    iframeSrc = iframe[0].contentWindow.document.location.href,

                    currentFolder = $.request(iframeSrc).queryString["folderName"],

                    selectedDatas = map[rootFolder] = (map[rootFolder] || []).where(function (o) {
                        return o.selected;
                    }),

                    isSingle = $("[name='cat_" + rootFolder + "_single_choice']").val() == 'True',

                    contents = iframe.contents(),

                    folderText = contents.find('input:hidden[name="FriendlyFolderName"]').val()

                    selectedTable = contents.find('table.selected tbody'),

                    datasourceTable = contents.find('table.datasource tbody'),

                    datasourceTable.find('tr').css({
                        cursor: 'pointer'
                    });

                    toggleBackLink(datasourceTable.parent(), rootFolder, currentFolder, _prevLink);

                    _prevLink = iframeSrc;

                    checkedSelector = selectedDatas.select(function (o) {
                        return '[value="' + o.key + '"]';
                    }).join(',');

                    var initCheckStatus = function (element) {

                        element.find(checkedSelector).attr('checked', 'checked');

                        if (isSingle) {
                            handleRadio(element);
                        } else {
                            handleCheckbox(element);
                        }
                    }
                    initCheckStatus(datasourceTable);
                    datasourceTable.parent().bind('treeNodeLoaded', function (e, treeNode) {
                        initCheckStatus(treeNode);
                    });
                    function checkBoxChange() {
                        var handle = $(this),
                            tr = handle.parents('tr:eq(0)'),
                            key = handle.val(),
                            text = tr.find('td:eq(1)').text();

                        var query = map[rootFolder].where(function (o, i) {
                            return o.key == key;
                        });

                        if (handle.attr('checked')) {
                            if (query.length == 0) {
                                map[rootFolder].push({
                                    key: key,
                                    text: text,
                                    currentFolder: currentFolder,
                                    folderText: folderText,
                                    selected: true
                                });
                            }
                        } else {
                            if (query.length > 0) {
                                query[0].selected = false;
                            }
                        }
                    }

                    function handleCheckbox(parent) {
                        var checkboxs = parent.find('input:checkbox')
                        .change(checkBoxChange);
                    }

                    function radioChange() {
                        var handle = $(this),
                            tr = handle.parents('tr:eq(0)'),
                            text = tr.find('td:eq(1)').text(),
                             key = handle.val(),
                            query = map[rootFolder].where(function (o, i) {
                                if (o.key == key) {
                                    o.selected = true;
                                } else {
                                    o.selected = false;
                                }
                                return o.key == key;
                            });
                        if (query.length == 0) {
                            map[rootFolder].push({
                                key: key
                                , text: text
                                , selected: true
                                , currentFolder: currentFolder
                                , folderText: folderText
                            });
                        }
                    }

                    function handleRadio(parent) {
                        var radios = parent.find('input:radio')
                        .change(radioChange);
                    }

                    (function initButton() {

                        var saveBtn = contents.find('.save'),

                        cancelBtn = contents.find('.cancel');

                        cancelBtn.click(function () {
                            pop.close();
                        });

                        saveBtn.click(function () {
                            _serializeJSON = $.toJSON(map[rootFolder]);
                            initDisplayAndValueInput(link);
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
                    destroyMethod = function () { contentCon.remove(); };
                }
            }
        });

        function initDisplayAndValueInput($link) {
            var $cagetoryTd = $link.parents('td:eq(0)')
            , $container = $cagetoryTd.find('div.category-container')
            , $template = $container.find('div.category-item-template')
            , $clone = function () {
                $template = $template.length ? $template : $container.data('template');
                if (!$container.data('template')) {
                    $template.data('template', $template.clone());
                }
                return $template.clone()
                    .removeClass('category-item-template hide').addClass('category-item-data');
            }
            , $displays = $cagetoryTd.find('[name$="_display"]')
            , $values = $cagetoryTd.find('[name$="_value"]')
            , rootFolder = $.request($link.attr('href')).queryString['folderName']
            , selectedDatas = map[rootFolder] = (map[rootFolder] || []).where(function (o) {
                return o.selected;
            })
            ;

            $container.find('.category-item-data').remove();
            selectedDatas.each(function (val, index) {
                if (!val) { return false; }

                var $item = $clone().appendTo($container);

                var folderText = val.folderText || val.currentFolder;

                var displayStr = val.text.split(':')[0] + (folderText ? (' ( ' + folderText + ' )') : '').replace(/\~/g, '/');

                $item.attr('title', displayStr);

                $item.find('label').html(displayStr);

                $item.find('a.remove').click(function () {

                    val.selected = false;

                    var values = selectedDatas
                        .where(function (o) {
                            return o.selected;
                        })
                        .select(function (o) {
                            return o.key + ':' + o.currentFolder;
                        }).join(',');

                    $values.val(values);

                    $displays.val(values);

                    $item.remove();


                });
            });

            $values.val(selectedDatas.select(function (o) {
                return o.key + ':' + o.currentFolder;
            }).join(','));
        }

    });    //end document.ready
</script>
<%}  %>