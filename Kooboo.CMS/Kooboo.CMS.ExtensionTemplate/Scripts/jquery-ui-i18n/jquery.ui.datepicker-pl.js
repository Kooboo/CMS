
jQuery(function($){
	$.datepicker.regional['pl'] = {"Name":"pl","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["styczeń","luty","marzec","kwiecień","maj","czerwiec","lipiec","sierpień","wrzesień","październik","listopad","grudzień",""],"monthNamesShort":["sty","lut","mar","kwi","maj","cze","lip","sie","wrz","paź","lis","gru",""],"dayNames":["niedziela","poniedziałek","wtorek","środa","czwartek","piątek","sobota"],"dayNamesShort":["N","Pn","Wt","Śr","Cz","Pt","So"],"dayNamesMin":["N","Pn","Wt","Śr","Cz","Pt","So"],"dateFormat":"yy-mm-dd","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['pl']);
});