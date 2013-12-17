
jQuery(function($){
	$.datepicker.regional['id'] = {"Name":"id","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["Januari","Februari","Maret","April","Mei","Juni","Juli","Agustus","September","Oktober","Nopember","Desember",""],"monthNamesShort":["Jan","Feb","Mar","Apr","Mei","Jun","Jul","Agust","Sep","Okt","Nop","Des",""],"dayNames":["Minggu","Senin","Selasa","Rabu","Kamis","Jumat","Sabtu"],"dayNamesShort":["Minggu","Sen","Sel","Rabu","Kamis","Jumat","Sabtu"],"dayNamesMin":["M","S","S","R","K","J","S"],"dateFormat":"dd/mm/yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['id']);
});