
jQuery(function($){
	$.datepicker.regional['ru'] = {"Name":"ru","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["Январь","Февраль","Март","Апрель","Май","Июнь","Июль","Август","Сентябрь","Октябрь","Ноябрь","Декабрь",""],"monthNamesShort":["янв","фев","мар","апр","май","июн","июл","авг","сен","окт","ноя","дек",""],"dayNames":["воскресенье","понедельник","вторник","среда","четверг","пятница","суббота"],"dayNamesShort":["Вс","Пн","Вт","Ср","Чт","Пт","Сб"],"dayNamesMin":["Вс","Пн","Вт","Ср","Чт","Пт","Сб"],"dateFormat":"dd.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['ru']);
});