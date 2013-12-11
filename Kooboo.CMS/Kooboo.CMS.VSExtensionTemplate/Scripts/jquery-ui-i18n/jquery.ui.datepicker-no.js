
jQuery(function($){
	$.datepicker.regional['no'] = {"Name":"no","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["januar","februar","mars","april","mai","juni","juli","august","september","oktober","november","desember",""],"monthNamesShort":["jan","feb","mar","apr","mai","jun","jul","aug","sep","okt","nov","des",""],"dayNames":["søndag","mandag","tirsdag","onsdag","torsdag","fredag","lørdag"],"dayNamesShort":["sø","ma","ti","on","to","fr","lø"],"dayNamesMin":["sø","ma","ti","on","to","fr","lø"],"dateFormat":"dd.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['no']);
});