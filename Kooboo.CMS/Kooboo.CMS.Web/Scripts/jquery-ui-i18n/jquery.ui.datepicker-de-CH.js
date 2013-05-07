
jQuery(function($){
	$.datepicker.regional['de-CH'] = {"Name":"de-CH","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["Januar","Februar","März","April","Mai","Juni","Juli","August","September","Oktober","November","Dezember",""],"monthNamesShort":["Jan","Feb","Mrz","Apr","Mai","Jun","Jul","Aug","Sep","Okt","Nov","Dez",""],"dayNames":["Sonntag","Montag","Dienstag","Mittwoch","Donnerstag","Freitag","Samstag"],"dayNamesShort":["So","Mo","Di","Mi","Do","Fr","Sa"],"dayNamesMin":["So","Mo","Di","Mi","Do","Fr","Sa"],"dateFormat":"dd.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['de-CH']);
});