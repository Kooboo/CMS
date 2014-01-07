
jQuery(function($){
	$.datepicker.regional['fr-CA'] = {"Name":"fr-CA","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["janvier","février","mars","avril","mai","juin","juillet","août","septembre","octobre","novembre","décembre",""],"monthNamesShort":["janv.","févr.","mars","avr.","mai","juin","juil.","août","sept.","oct.","nov.","déc.",""],"dayNames":["dimanche","lundi","mardi","mercredi","jeudi","vendredi","samedi"],"dayNamesShort":["dim.","lun.","mar.","mer.","jeu.","ven.","sam."],"dayNamesMin":["di","lu","ma","me","je","ve","sa"],"dateFormat":"yy-mm-dd","firstDay":0,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['fr-CA']);
});