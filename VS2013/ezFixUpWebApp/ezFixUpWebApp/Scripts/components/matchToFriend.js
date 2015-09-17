function setFriendToMatchWith(linkBtnId) {
    var jqSelector = '#' + linkBtnId;
    var imgId = jQuery(jqSelector).attr('image_id');
    var username = jQuery(jqSelector).attr('username');
    var setSessionStateService1 = "./handlers/MatchWith.ashx";
    jQuery.get(setSessionStateService1, { 'with_user': username }, function(toUsername) {
        if (toUsername != null && toUsername != "false") {
            //alert(toUsername);
            window.location = "./Matchmaking_" + toUsername + "_With_" + username + ".aspx";
        }
    });
}
function setFriendToMatch(linkBtnId) {
    var jqSelector = '#' + linkBtnId;
    var imgId = jQuery(jqSelector).attr('image_id');
    var username = jQuery(jqSelector).attr('username');
    //var setSessionStateService1 = "./handlers/SetSessionState.ashx?key=MatchToFriendImageId&val=" + imgId;
    //var setSessionStateService2 = "./handlers/SetSessionState.ashx?key=MatchToUsername&val=" + username;
    //var setSessionStateService3 = "./handlers/SetSessionState.ashx?key=ToUsername&val=" + username;
    var setSessionStateService = "./handlers/MatchTo.ashx";
    //var getSessionStateService = "./handlers/GetSessionState.ashx?key=MatchToFriendImageId";
    //$.post(setSessionStateService1, function(x) { });
    //$.post(setSessionStateService3, function(x) { });
    //$.post(setSessionStateService2, function(x) {
    jQuery.get(setSessionStateService, { 'imgId': imgId, 'username': username }, function(x) {
        //$.get(getSessionStateService, function(data) {
        jQuery("#mmFriendImg").attr('style', "width:200px;height:200px; border:solid 1px DodgerBlue;" +
                    "background:white url(./Image.ashx?id=" + imgId + "&width=200&height=200&findFace=1) center center no-repeat;");
        //});
        jQuery('#mm_popup').show();
        jQuery('#mm_popup').focus();
    });
}
