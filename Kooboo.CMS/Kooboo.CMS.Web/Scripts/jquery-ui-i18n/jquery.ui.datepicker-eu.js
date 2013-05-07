
jQuery(function($){
	$.datepicker.regional['eu'] = {"Name":"eu","closeText":"Close","prevText":"Prev","nextText":"Next","currentText":"Today","monthNames":["urtarrila","otsaila","martxoa","apirila","maiatza","ekaina","uztaila","abuztua","iraila","urria","azaroa","abendua",""],"monthNamesShort":["urt.","ots.","mar.","api.","mai.","eka.","uzt.","abu.","ira.","urr.","aza.","abe.",""],"dayNames":["igandea","astelehena","asteartea","asteazkena","osteguna","ostirala","larunbata"],"dayNamesShort":["ig.","al.","as.","az.","og.","or.","lr."],"dayNamesMin":["ig","al","as","az","og","or","lr"],"dateFormat":"yy/mm/dd","firstDay":1,"isRTL":false};
	$.datepicker.setDefaults($.datepicker.regional['eu']);
});