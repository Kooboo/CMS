
jQuery(function($){
	$.datepicker.regional['ko'] = {"Name":"ko","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["1월","2월","3월","4월","5월","6월","7월","8월","9월","10월","11월","12월",""],"monthNamesShort":["1","2","3","4","5","6","7","8","9","10","11","12",""],"dayNames":["일요일","월요일","화요일","수요일","목요일","금요일","토요일"],"dayNamesShort":["일","월","화","수","목","금","토"],"dayNamesMin":["일","월","화","수","목","금","토"],"dateFormat":"yy-mm-dd","firstDay":0,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['ko']);
});