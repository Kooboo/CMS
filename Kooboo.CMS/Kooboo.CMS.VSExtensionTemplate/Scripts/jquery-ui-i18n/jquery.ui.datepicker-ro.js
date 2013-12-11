
jQuery(function($){
	$.datepicker.regional['ro'] = {"Name":"ro","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["ianuarie","februarie","martie","aprilie","mai","iunie","iulie","august","septembrie","octombrie","noiembrie","decembrie",""],"monthNamesShort":["ian.","feb.","mar.","apr.","mai.","iun.","iul.","aug.","sep.","oct.","nov.","dec.",""],"dayNames":["duminică","luni","marţi","miercuri","joi","vineri","sâmbătă"],"dayNamesShort":["D","L","Ma","Mi","J","V","S"],"dayNamesMin":["D","L","Ma","Mi","J","V","S"],"dateFormat":"dd.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['ro']);
});