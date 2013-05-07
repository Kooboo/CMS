
jQuery(function($){
	$.datepicker.regional['hu'] = {"Name":"hu","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["január","február","március","április","május","június","július","augusztus","szeptember","október","november","december",""],"monthNamesShort":["jan.","febr.","márc.","ápr.","máj.","jún.","júl.","aug.","szept.","okt.","nov.","dec.",""],"dayNames":["vasárnap","hétfő","kedd","szerda","csütörtök","péntek","szombat"],"dayNamesShort":["V","H","K","Sze","Cs","P","Szo"],"dayNamesMin":["V","H","K","Sze","Cs","P","Szo"],"dateFormat":"yy.mm.dd.","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['hu']);
});