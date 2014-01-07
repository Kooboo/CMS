$(function () {
    //Fancybox
    $('#login-link').fancybox({
        showCloseButton: false,
        overlayColor: '#000',
        opacity: 0.3,
        autoDimensions: false,
        height: 260
    });

    $('.ajaxForm').each(function () {
        var form = $(this);
        form.ajaxForm({
            sync: true,
            beforeSerialize: function ($form, options) {
            },
            beforeSend: function () {
                var form = $(this);
                form.find("[type=submit]").addClass("disabled").attr("disabled", true);
            },
            beforeSubmit: function (arr, $form, options) {
            },
            success: function (responseData, statusText, xhr, $form) {
                form.find("[type=submit]").removeClass("disabled").removeAttr("disabled");
                if (!responseData.Success) {
                    var validator = form.validate();
                    //                            var errors = [];
                    for (var i = 0; i < responseData.FieldErrors.length; i++) {
                        var obj = {};
                        var fieldName = responseData.FieldErrors[i].FieldName;
                        if (fieldName == "") {
                            alert(responseData.FieldErrors[i].ErrorMessage);
                        }
                        obj[fieldName] = responseData.FieldErrors[i].ErrorMessage;
                        validator.showErrors(obj);
                    }
                }
                else {
                    if (responseData.RedirectUrl != null) {
                        location.href = responseData.RedirectUrl;
                    }
                    else {
                        location.reload();
                    }

                }
            },
            error: function () {
                var form = $(this);
                form.find("[type=submit]").removeClass('disabled').removeAttr('disabled');
            }

        });
    })

})
