<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script language="javascript" type="text/javascript">
    ///setTimeout(function () {
    $(window).ready(function () {
        kooboo.cms.ui.loading().hide();
        //#region fixed javascript link and make link's tabindex behind all inputs
        /* 
        fixed page unload event in ie 
        under ie javascript link can trigger page unload event.
        sometimes it will show an confirm dialog .   
        */
        var javascriptLinkReg = /javascript:;/i;

        $('a').each(function () {
            var tabIndex = this.tabindex;
            tabIndex = tabIndex != undefined ? tabIndex : 1000;
            this.setAttribute('tabindex', tabIndex);

            if (javascriptLinkReg.test(this.href)) {
                $(this).click(function (e) {
                    e.preventDefault();
                });
            }
        });

        //#endregion
    });
    //}, 50);
</script>
