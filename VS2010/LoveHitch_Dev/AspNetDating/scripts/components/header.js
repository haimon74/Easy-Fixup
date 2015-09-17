jQuery(document).ready(docReady_Header);

function docReady_Header() {
    //alert('headers doc ready')
    jQuery(lnkLogout).hide();
    var idx = document.referrer.toLowerCase().indexOf('apps.facebook.com/lovehitch', 0);
    if (idx >= 0) facebookIframeSetup();
    else jQuery(lnkLogout).show();

    var getSessionStateService = "./handlers/GetSessionState.ashx?key=CurrentRawUrl";
    var currentPage;
    jQuery.get(getSessionStateService, function(data) {
        currentPage = data.toString().toLowerCase();
        jQuery("#navi li").each(function() {
            var li = jQuery(this);
            var childAnchor = li.find('a');
            var href = childAnchor != null ? childAnchor.attr('_href').toString().toLowerCase() : "";
            //debugger;
            if (currentPage.indexOf(href) != -1) {
                li.css({ background: topMenuBgColor });
                //li.css({ color: 'white !important' });                 
            }
            var activeNav = getActiveNavCookie();
            if (activeNav == href) {
                li.css({ background: topMenuBgColor });
            }
//            else {
//                jQuery(this).hover(function() { //Hover over event on list item
//                    jQuery(this).css({ background: topMenuBgColor/*'teal'*/ }); //Add background color + image on hovered list item
//                    jQuery(this).find("span").show(); //Show the navi                        
//                }, function() { //on hover out...
//                    jQuery(this).css({ background: 'none' }); //Ditch the background
//                    jQuery(this).find("span").hide(); //Hide the navi
//                });
//            }
        });
    });
}
function facebookIframeSetup() {
    var idx = document.referrer.indexOf('default.aspx', 0);
    if (idx >= 0) {
        facebookLoggedIn();
    }
    else {
        jQuery(lnkLogout).css({ display: 'none' });
        var fbConnectBtn = "#fbConnectBtn";
        jQuery(fbConnectBtn).css({ visibility: 'hidden' });

    }
}
function facebookLoggedIn() {
    //debugger;
    if (FB._authResponse && FB._authResponse.accessToken) {
        tryLoginThroughFacebook(FB._authResponse.accessToken);
    }
    else {
        var auth = FB.getAuthResponse();
        if (auth && auth.accessToken) {
            tryLoginThroughFacebook(auth.accessToken);
        }
    }
}
function tryLoginThroughFacebook(token) {
    var setSessionStateServiceUrl = "./handlers/SetSessionState.ashx";
    jQuery.get(setSessionStateServiceUrl, { 'key': 'FacebookConnectTriesElapsed', 'val': '1' }, function (x) { });
    window.location = './LoginThroughFacebook.aspx?access_token=' + token + '&back_url=' + window.location;
}
//    function setActiveMenuItem(listItemId){
//        var setSessionStateServiceUrl = "./handlers/SetSessionState.ashx?key=ActiveMenuItem&val=" + listItemId;
//        $.post(setSessionStateServiceUrl, function(x) { });        
//    }
