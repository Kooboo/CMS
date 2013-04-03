<script type="text/javascript" language="javascript">
    $(function () {
        function getOpener() {
            var opener = window.parent || window.top;

            var hasPop = false;
            if ($.popContext.popList.where(function (o) { return o.is(':visible'); }).length >= 1) {
                opener = $.popContext.getCurrent();
                hasPop = true;
            }

            var api = {
                reload: function (callback) {
                    if (hasPop) {
                        opener.iframe.get(0).src = opener.iframe.get(0).src;
                    }
                    else {
                        opener.location.reload(true);
                    }
                    if (callback) { callback(); }
                },
                redirectTo: function (href, callback) {
                    if (hasPop) {
                        opener.iframe.get(0).src = href;
                    }
                    else {
                        opener.location.href = href;
                    }
                    if (callback) { callback(); }
                }
            }

            return api;
        }
        kooboo.cms.ui.getOpener = getOpener;

        $(document).ajaxStop(function () {
            kooboo.cms.ui.loading().hide();
        });

        $(document).ajaxError(function () {
            kooboo.cms.ui.loading().hide();
        });

        $('a.boolean-ajax-link').click(function () {
            var handle = $(this), confirmMsg;

            if (handle.hasClass('cross')) {
                confirmMsg = handle.attr('confirmMsg');
            } else {
                confirmMsg = handle.attr('unConfirmMsg');
            }

            kooboo.confirm(confirmMsg, function (r) {
                if (r) {
                    var dataField = handle.attr('data');

                    $.ajax({
                        url: handle.attr('href'),
                        type: 'post',
                        beforeSend: function () {
                            kooboo.cms.ui.loading().show();
                        },
                        success: function (response) {
                            if (response.Success) {
                                if (handle.hasClass('cross')) {
                                    handle.removeClass('cross');
                                    handle.addClass('tick');
                                } else {
                                    handle.removeClass('tick');
                                    handle.addClass('cross');
                                }
                            } else {
                                kooboo.cms.ui.messageBox().showResponse(response);
                            }
                            kooboo.cms.ui.loading().hide();
                        },
                        error: function () {
                            kooboo.cms.ui.messageBox().show('<%:"There is an error occurs".Localize() %>', 'error');
                        }
                    });
                }
            });

            return false;
        });

        if (!kooboo.data('kooboo-formHandle-executed')) {
            kooboo.data('kooboo-formHandle-executed', true);

            var onSumitSuccess = function (response, statusText, xhr, $form) {
                var form = $form;
                if (response == undefined) {
                    return false;
                }

                var responseData = response;
                if (typeof (responseData) == 'string') {
                    responseData = $.parseJSON($(responseData).text());
                }
                kooboo.cms.ui.messageBox().hide();
                kooboo.cms.ui.event.onSuccess(form, responseData);

                kooboo.cms.ui.messageBox().showResponse(responseData);
                form.find("[type=submit]").removeClass("disabled").removeAttr("disabled");
                if (!responseData.Success) {
                    var validator = form.validate();
                    //                            var errors = [];
                    for (var i = 0; i < responseData.FieldErrors.length; i++) {
                        var obj = {};
                        obj[responseData.FieldErrors[i].FieldName] = responseData.FieldErrors[i].ErrorMessage;
                        validator.showErrors(obj);
                    }
                }
                else {
                    var continueResult = true;
                    top.kooboo.data("parent-page-reload", true);
                    if (window.onSuccess && (typeof (onSuccess) == 'function')) {
                        continueResult = onSuccess(responseData);
                    }
                    if (continueResult == true) {
                        var topJQ = top._jQuery || top.jQuery;
                        if (responseData.RedirectToOpener && topJQ.popContext.getCurrent()) {
                            topJQ.popContext.getCurrent().close();
                            top.kooboo.cms.ui.loading().show();
                        }

                        var pageRedirect = form.data("pageRedirect");

                        //normal
                        if (pageRedirect == undefined) {
                            if (responseData.RedirectUrl) {
                                if (responseData.RedirectToOpener) {
                                    getOpener().redirectTo(responseData.RedirectUrl.replace("&amp;", "&"), top.kooboo.cms.ui.loading().hide);
                                } else {
                                    location.href = responseData.RedirectUrl.replace("&amp;", "&");
                                }
                            } else {
                                //why:当出现错误的时候，刷新会让错误信息马上消失掉。(Module/Install)
                                if (responseData.Messages.length == 0) {
                                    getOpener().reload(top.kooboo.cms.ui.loading().hide);
                                }
                            }
                        } else { //special
                            if (responseData.RedirectUrl) {
                                if (responseData.RedirectToOpener) {
                                    getOpener().redirectTo(responseData.RedirectUrl.replace("&amp;", "&"), top.kooboo.cms.ui.loading().hide);
                                } else {
                                    location.href = responseData.RedirectUrl.replace("&amp;", "&");
                                }
                            }
                            if (response.Messages[0]) {
                                kooboo.cms.ui.messageBox().show(response.Messages[0], 'info');
                            }

                        }


                    }

                }
            };
            var ajaxFormParam = {
                async: true,
                beforeSerialize: function ($form, options) {
                    var globResult = kooboo.cms.ui.event.ajaxSubmit($form, this);
                    //var selfResult = kooboo.cms.ui.event.ajaxSubmit(form);
                    return globResult; //!= false && selfResult != false;
                },
                beforeSend: function () {
                    var form = $(this);
                    kooboo.cms.ui.loading().show();
                    form.find("[type=submit]").addClass("disabled").attr("disabled", true);
                },
                beforeSubmit: function (arr, $form, options) {
                },
                success: function (response, statusText, xhr, $form) {
                    onSumitSuccess.call(this, response, statusText, xhr, $form)
                },
                error: function () {
                    var form = $(this);
                    kooboo.cms.ui.loading().hide();
                    form.find("[type=submit]").removeClass('disabled').removeAttr('disabled');
                }
            };


            $('form').submit(function () {
                // reset all the validation messer elements. (Change for textContent form.)
                $('.field-validation-error').empty();
                var form = $(this);
                if (!form.valid()) {
                    if (form.find('.tabs').length) {
                        var selector = 'input.input-validation-error,select.input-validation-error';
                        $(selector).parents('div.tab-content')
								.each(function () {
								    var tab = $(this);
								    var li = $('a[href="#' + tab.attr('id') + '"]')
									.hide().show('pulsate', {}, 100)
									.show('highlight', {}, 200)
									.show('pulsate', {}, 300)
									.show('highlight', {}, 400);
								});
                    }
                }
            }).each(function () {
                var form = $(this);
                form.find('[name=pageRedirect]').click(function () {
                    form.data('pageRedirect', $(this).val());
                });
            });
            window.ajaxFormParam = ajaxFormParam;
            if (window.formHandle) {
                window.formHandle();
            } else {
                $('form:not(.no-ajax)').each(function () {
                    var form = $(this);
                    form.ajaxForm(ajaxFormParam);
                    form.submit(function () {
                        kooboo.cms.ui.event.afterSubmit(this);
                    });
                })
            }
        }
    });
</script>
