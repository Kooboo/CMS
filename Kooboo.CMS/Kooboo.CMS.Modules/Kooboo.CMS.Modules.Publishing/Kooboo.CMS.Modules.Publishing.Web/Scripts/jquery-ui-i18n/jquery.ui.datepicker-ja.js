
jQuery(function($){
	$.datepicker.regional['ja'] = {"Name":"ja","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["1月","2月","3月","4月","5月","6月","7月","8月","9月","10月","11月","12月",""],"monthNamesShort":["1","2","3","4","5","6","7","8","9","10","11","12",""],"dayNames":["日曜日","月曜日","火曜日","水曜日","木曜日","金曜日","土曜日"],"dayNamesShort":["日","月","火","水","木","金","土"],"dayNamesMin":["日","月","火","水","木","金","土"],"dateFormat":"yy/mm/dd","firstDay":0,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['ja']);
});