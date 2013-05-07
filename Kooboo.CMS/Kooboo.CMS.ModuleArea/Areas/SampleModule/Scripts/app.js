if (window.jQuery) {
    $(function () {
        //Dropdown button
        $('.module-dropdown-button').click(function (e) {
            e.stopPropagation();
            $(this).toggleClass('active');
        });
        $(document).click(function () {
            $('.module-dropdown-button').removeClass('active');
        })
    })
}