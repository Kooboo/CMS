
jQuery(function($){
	$.datepicker.regional['es-US'] = {"Name":"es-US","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["enero","febrero","marzo","abril","mayo","junio","julio","agosto","septiembre","octubre","noviembre","diciembre",""],"monthNamesShort":["ene","feb","mar","abr","may","jun","jul","ago","sep","oct","nov","dic",""],"dayNames":["domingo","lunes","martes","miércoles","jueves","viernes","sábado"],"dayNamesShort":["dom","lun","mar","mié","jue","vie","sáb"],"dayNamesMin":["do","lu","ma","mi","ju","vi","sa"],"dateFormat":"mm/d/yy","firstDay":0,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['es-US']);
});