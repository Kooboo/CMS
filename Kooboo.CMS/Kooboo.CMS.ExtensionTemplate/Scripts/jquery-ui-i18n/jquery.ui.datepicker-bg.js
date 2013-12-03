
jQuery(function($){
	$.datepicker.regional['bg'] = {"Name":"bg","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["януари","февруари","март","април","май","юни","юли","август","септември","октомври","ноември","декември",""],"monthNamesShort":["ян","февр","март","апр","май","юни","юли","авг","септ","окт","ноември","дек",""],"dayNames":["неделя","понеделник","вторник","сряда","четвъртък","петък","събота"],"dayNamesShort":["нед","пон","вт","ср","четв","пет","съб"],"dayNamesMin":["н","п","в","с","ч","п","с"],"dateFormat":"d.mm.yy \u0027г.\u0027","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['bg']);
});