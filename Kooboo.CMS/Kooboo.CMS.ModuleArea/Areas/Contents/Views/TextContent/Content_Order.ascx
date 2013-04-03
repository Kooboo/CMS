<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<PagedList<Kooboo.CMS.Content.Models.TextContent>>" %>
<%var folder = ViewData["Folder"] as Kooboo.CMS.Content.Models.TextFolder;
  var hasPrevPage = Model.CurrentPageIndex != 1;
  var hasNextPage = Model.CurrentPageIndex != Model.TotalPageCount;
  var schema = (Kooboo.CMS.Content.Models.Schema)ViewData["Schema"];

  var enableSort = folder != null && folder.OrderBySequence && ViewData["WhereClause"] == null;
%>

<%--  draggable Begin --%>
<script language="javascript" type="text/javascript">
    $(function () {
        var enableSort= <%= enableSort.ToString().ToLower() %>;
        var isTree = <%:schema.IsTreeStyle.ToString().ToLower() %>;
        if(enableSort){
            (function () {
            var asc = '<%:folder.OrderSetting.Direction == Kooboo.CMS.Content.Models.OrderDirection.Ascending %>' == 'True',
            table = $("div.table-container table"),
            sortListUrl = '<%=Url.Action("Sort",Request.RequestContext.AllRouteValues()) %>',
            crossPageUrl = '<%=Url.Action("CrossPageSort",Request.RequestContext.AllRouteValues()) %>',
            request = $.request(crossPageUrl),
            hasPrev = false,
            hasNext = false,
            hasPrevPage = <%:hasPrevPage.ToString().ToLower() %>,
            hasNextPage = <%:hasNextPage.ToString().ToLower() %>,            
            isUp = false;
            table.tableSorter({
                //dragable: true,
                cancel: 'tr.folderTr',
                prevLevel: function (handle) {
                    var pid = handle.attr("pid");                    
                    if (pid == "") { 
                        return handle.prevAll().filter("[pid='']");
                        
                    }else {
                        var tmp = ".child-of-node-" + pid;
                        return handle.prevAll().filter(tmp);
                    }
                },
                nextLevel: function (handle) {
                    var pid = handle.attr("pid");
                    var tmp = ".child-of-node-" + pid;
                    if (pid == "") {              
                        return handle.nextAll().filter("[pid='']");                       
                    }else {
                        return handle.nextAll().filter(tmp)
                    }
                },
                isTopLevel:function(handle){
                     var pid = handle.attr("pid");
                     return !pid;
                },
                beforeUp: function (handle) {
                    //hasPrev = handle.prev().length > 0;
                },
                up: function (handle) {
                    isUp = true;                  
                    request.queryString["up"] = 'true';
                    sortChange({
                        UUID: handle.find('input:checkbox').val(),
                        Sequence: handle.find('input:hidden[name=Sequence]').val()
                    }, request.getUrl());
                    
                },
                beforeDown: function (handle) {
//                    hasNext = handle.next().length > 0;
                },
                down: function (handle) {
                    isUp = false;                    
                    request.queryString["up"] = 'false';
                    sortChange({
                        UUID: handle.find('input:checkbox').val(),
                        Sequence: handle.find('input:hidden[name=Sequence]').val()
                    }, request.getUrl());
                },
                showUp: function (handle) {
                    var show = false;
                    var hasPrev = this.prevLevel(handle).length > 0;
                    show = hasPrev && (isTree || hasPrevPage);
                    return show;
                },
                showDown: function (handle) {
                    var show = false;
                    var hasNext = this.nextLevel(handle).length > 0;
                    show = hasNext && (isTree || hasNextPage);
                    return show;
                }
            });

            var tbody = table.find('tbody:eq(1)').sortable({
                revert: true,
                handle: 'td:eq(0)',
                cancel: 'tr.folderTr',               
                start: function (event, ui) {
                    ui.placeholder.html('<td colspan="100"></td>');
                },
                change: function (event, ui) {
                },
                stop: function () {
                    sortChange();
                },
                placeholder: "ui-state-highlight holder",
                cursor: 'move',
                helper: 'clone'
            });

            var sortList = getSortlist();

            function sortChange(data, url, dataname) {
                if (data) {
                    post(data, url || crossPageUrl, dataname || 'sourceContent', function (response) {
                        if (response.Success && (!hasNext && !isUp || !hasPrev && isUp)) {
                            setTimeout(function () {
                                document.location.reload(true);
                            }, 400);
                        }
                    });
                } else {
                    var oldList = getSortlist();
                    initSort(sortList);
                    var newList = getSortlist();
                    var changedList = getChangedList(oldList, newList);
                    post(changedList, url || sortListUrl, 'list');
                }
                $('tr').removeClass('even').filter(':odd').addClass('even');
            }

            function getSortlist() {
                var list = [];
                tbody.find('tr').each(function () {
                    var handle = $(this),
                    sequenceInput = handle.find('input[name=Sequence]'),
                    uuidInput = handle.find('input:checkbox.select.docs'),
                     sequence = sequenceInput.val() || 0,
                     uuid = uuidInput.val();
                    list.push({
                        Sequence: sequence,
                        UUID: uuid
                    });
                });
                return list;
            }

            function initSort(sortList) {
                sortList = sortList || getSortlist();
                if (asc) {
                    sortList = sortList.sort(function (a, b) { return a.Sequence - b.Sequence; });
                } else {
                    sortList = sortList.sort(function (a, b) { return b.Sequence - a.Sequence; });
                }
                tbody.find('tr').each(function (index) {
                    var tr = $(this);
                    tr.find('input[name=Sequence]').val(sortList[index].Sequence);
                });
            }

            function getChangedList(oldList, newList) {
                if (!(oldList && newList)) {
                    return [];
                }
                var list = newList.where(function (o) {
                    return oldList.where(function (x) {
                        return x.UUID == o.UUID;
                    }).first().Sequence != o.Sequence;
                });
                return list;
            }

            function post(changedList, url, dataName, callBack) {
                var tempForm = kooboo.cms.ui.formHelper.tempForm(changedList, url, dataName);
                tempForm.ajaxSubmit({
                    success: function (response) {
                        kooboo.cms.ui.messageBox().showResponse(response);
                        if (typeof callBack == 'function') {
                            callBack(response);
                        }
                    },
                    error: function () {

                    }
                });

            }
        })();
        }
        else{
            (function () { $('td.draggable,td.undraggable,th.draggable').removeClass('draggable undraggable'); })();
        }
        if(isTree){
            (function () {
            $(".table-container table").treeTable({ persist:true, expandable: true, treeColumn: 1 });

            $(".treeTable tr:not(.folderTr)").draggable({
                helper: "clone",
                opacity: .75,
                refreshPositions: true, // Performance?
                revert: "invalid",
                revertDuration: 300,
                scroll: true
            });
            $(".treeTable  tr:not(.folderTr)").each(function () {
                $($(this)[0]).droppable({
                    accept: "tr",
                    drop: function (e, ui) {
                        var uuid = typeof ($($(ui.draggable)[0])).attr('id') != "undefined" ? $($(ui.draggable)[0]).attr('id').substring(5) : "";
                        var parentUUID = typeof ($(this)).attr('id') != "undefined" ? $(this).attr('id').substring(5) : "";
                        var parentChain = typeof ($(this)).attr('parentChain') != "undefined" ? $(this).attr('parentChain') : "";                        
                        if (parentChain.indexOf(uuid)==-1) {
                            $.ajax({
                                url: '<%= Server.HtmlDecode(this.Url.Action("MoveContent", ViewContext.RequestContext.AllRouteValues().Merge("UUID", "").Merge("ParentUUID", "")))%>',
                                data: { uuid: uuid, parentUUID: parentUUID },
                                type: 'post',
                                beforeSend: function () {
                                    kooboo.cms.ui.loading().show();
                                },
                                success: function (response) {
                                    if (response.Success) {
                                        setTimeout(function () {
                                            document.location.reload(true);
                                            kooboo.cms.ui.messageBox().show('Succeed in moving the item', 'info');
                                        }, 400);
                                    } else {
                                        kooboo.cms.ui.messageBox().showResponse(response);
                                    }
                                    kooboo.cms.ui.loading().hide();
                                },
                                error: function () {
                                    kooboo.cms.ui.messageBox().show('There is an error occurs', 'error');
                                }
                            });
                        }
                        else {
                            //kooboo.cms.ui.messageBox().show('Move failed!', 'error');
                        }
                    },
                    hoverClass: "accept",
                    over: function (e, ui) {
                        if (this.id != $(ui.draggable[0]).id && !$(this).is(".expanded")) {
                            $(this).expand();
                        }
                    }
                });
            });
        })();
        }
    });
</script>
<%-- draggable End --%>

