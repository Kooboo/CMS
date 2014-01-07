
jQuery(function($){
	$.datepicker.regional['lt'] = {"Name":"lt","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["sausis","vasaris","kovas","balandis","gegužė","birželis","liepa","rugpjūtis","rugsėjis","spalis","lapkritis","gruodis",""],"monthNamesShort":["Sau","Vas","Kov","Bal","Geg","Bir","Lie","Rgp","Rgs","Spl","Lap","Grd",""],"dayNames":["sekmadienis","pirmadienis","antradienis","trečiadienis","ketvirtadienis","penktadienis","šeštadienis"],"dayNamesShort":["Sk","Pr","An","Tr","Kt","Pn","Št"],"dayNamesMin":["S","P","A","T","K","Pn","Š"],"dateFormat":"yy.mm.dd","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['lt']);
});