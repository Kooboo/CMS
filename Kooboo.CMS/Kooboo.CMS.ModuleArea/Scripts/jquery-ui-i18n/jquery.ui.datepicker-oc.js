
jQuery(function($){
	$.datepicker.regional['oc'] = {"Name":"oc","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["genier","febrier","març","abril","mai","junh","julh","agost","setembre","octobre","novembre","desembre",""],"monthNamesShort":["gen.","feb.","mar.","abr.","mai.","jun.","jul.","ag.","set.","oct.","nov.","des.",""],"dayNames":["dimenge","diluns","dimars","dimècres","dijòus","divendres","dissabte"],"dayNamesShort":["dim.","lun.","mar.","mèc.","jòu.","ven.","sab."],"dayNamesMin":["di","lu","ma","mè","jò","ve","sa"],"dateFormat":"dd/mm/yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['oc']);
});