$(function () {
    //Watermark Init
    $('.watermark input').each(function () {
        if ($(this).val() != "") {
            $(this).siblings('label').hide();
        }
    });
    //Watermark
    $('.watermark input').focus(function () {
        $(this).siblings('label').hide();
    }).blur(function () {
        if ($(this).val() == "") {
            $(this).siblings('label').show();
        }
    });
    //Fancybox
  $('#login-link').fancybox();
})
