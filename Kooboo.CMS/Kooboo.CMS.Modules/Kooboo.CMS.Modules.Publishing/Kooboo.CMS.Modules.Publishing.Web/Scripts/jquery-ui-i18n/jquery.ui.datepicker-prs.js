
jQuery(function($){
	$.datepicker.regional['prs'] = {"Name":"prs","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["محرم","صفر","ربيع الأول","ربيع الثاني","جمادى الأولى","جمادى الثانية","رجب","شعبان","رمضان","شوال","ذو القعدة","ذو الحجة",""],"monthNamesShort":["محرم","صفر","ربيع الأول","ربيع الثاني","جمادى الأولى","جمادى الثانية","رجب","شعبان","رمضان","شوال","ذو القعدة","ذو الحجة",""],"dayNames":["الأحد","الإثنين","الثلاثاء","الأربعاء","الخميس","الجمعة","السبت"],"dayNamesShort":["الأحد","الإثنين","الثلاثاء","الأربعاء","الخميس","الجمعة","السبت"],"dayNamesMin":["ح","ن","ث","ر","خ","ج","س"],"dateFormat":"dd/mm/yy","firstDay":5,"isRTL":true};
	$.datepicker.setDefaults($.datepicker.regional['prs']);
});