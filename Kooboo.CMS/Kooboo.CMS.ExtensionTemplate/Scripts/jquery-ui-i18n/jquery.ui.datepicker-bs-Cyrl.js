
jQuery(function($){
	$.datepicker.regional['bs-Cyrl'] = {"Name":"bs-Cyrl","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["јануар","фебруар","март","април","мај","јун","јул","август","септембар","октобар","новембар","децембар",""],"monthNamesShort":["јан","феб","мар","апр","мај","јун","јул","авг","сеп","окт","нов","дец",""],"dayNames":["недјеља","понедјељак","уторак","сриједа","четвртак","петак","субота"],"dayNamesShort":["нед","пон","уто","сре","чет","пет","суб"],"dayNamesMin":["н","п","у","с","ч","п","с"],"dateFormat":"d.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['bs-Cyrl']);
});