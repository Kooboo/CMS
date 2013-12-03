
jQuery(function($){
	$.datepicker.regional['tr-TR'] = {"Name":"tr-TR","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["Ocak","Şubat","Mart","Nisan","Mayıs","Haziran","Temmuz","Ağustos","Eylül","Ekim","Kasım","Aralık",""],"monthNamesShort":["Oca","Şub","Mar","Nis","May","Haz","Tem","Ağu","Eyl","Eki","Kas","Ara",""],"dayNames":["Pazar","Pazartesi","Salı","Çarşamba","Perşembe","Cuma","Cumartesi"],"dayNamesShort":["Paz","Pzt","Sal","Çar","Per","Cum","Cmt"],"dayNamesMin":["Pz","Pt","Sa","Ça","Pe","Cu","Ct"],"dateFormat":"dd.mm.yy","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['tr-TR']);
});