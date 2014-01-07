
jQuery(function($){
	$.datepicker.regional['fi'] = {"Name":"fi","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["tammikuu","helmikuu","maaliskuu","huhtikuu","toukokuu","kesäkuu","heinäkuu","elokuu","syyskuu","lokakuu","marraskuu","joulukuu",""],"monthNamesShort":["tammi","helmi","maalis","huhti","touko","kesä","heinä","elo","syys","loka","marras","joulu",""],"dayNames":["sunnuntai","maanantai","tiistai","keskiviikko","torstai","perjantai","lauantai"],"dayNamesShort":["su","ma","ti","ke","to","pe","la"],"dayNamesMin":["su","ma","ti","ke","to","pe","la"],"dateFormat":"d.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['fi']);
});