var fbsdkIsLoaded = false;
var fbIsInitialized = false;
var fbUserIsLoggedIn = false;
var fbUserIsAuthorized = false;

window.fbAsyncInit = function () {
    fbsdkIsLoaded = true;
    FB.init({
        appId: facebookAppId, // App ID
        channelURL: './Facebook/xd_receiver.htm', // Channel File
        status: true, // check login status
        cookie: true, // enable cookies to allow the server to access the session
        oauth: true, // enable OAuth 2.0
        //authResponse: true,
        frictionlessRequests: true,
        xfbml: true  // parse XFBML
    });
//    FB.getLoginStatus(function (response) {
//        if (response.status === 'connected') {
//            var uid = response.authResponse.userID;
//            var accessToken = response.authResponse.accessToken;
//            //alert(uid + ' ' + accessToken);
//            fbUserIsLoggedIn = true;
//            fbUserIsAuthorized = true;
//        } else if (response.status === 'not_authorized') {
//            fbUserIsLoggedIn = true;
//            fbUserIsAuthorized = false;
//        } else {
//            fbUserIsLoggedIn = false;
//            fbUserIsAuthorized = false;
//        }
//    });
    fbIsInitialized = true;
    var layoutHeight = Sys.UI.DomElement.getBounds($get('layout')).height;
    jQuery(function ($) {
        FB.Canvas.setSize({ width: 960, height: layoutHeight + 30 });
        FB.Canvas.setAutoGrow(1000);
    });
    FB.Canvas.setDoneLoading();
    FB.Event.subscribe('xfbml.render', function (response) {
        window.setTimeout("showHeaderButton()", 1000);
    });
    //debugger;
    if (fbTriesElapsed > 0 && !fbUserIsAuthorized) {
        facebookLoginTry();
    }
};
function showHeaderButton() {
    jQuery('td.facebookbtn').show();
}
jQuery(function($) {
    // Load the SDK Asynchronously
    (function(d) {
        var js, id = 'facebook-jssdk'; if (d.getElementById(id)) { return; }
        js = d.createElement('script'); js.id = id; js.async = true;
        //js.src = "//connect.facebook.net/en_US/all.js#appId=" + facebookAppId + "&xfbml=1";
        js.src = "//connect.facebook.net/en_US/all.js";
        d.getElementsByTagName('head')[0].appendChild(js);
        //d.getElementById('fb-root').appendChild(js);
    } (document));
});


function facebookLoginTry() {
    if (console) console.log('fb tries = ' + fbTriesElapsed);
    fbTriesElapsed--;
    var setSessionStateServiceUrl = "./handlers/SetSessionState.ashx";
    $.get(setSessionStateServiceUrl, { 'key': 'FacebookConnectTriesElapsed', 'val': fbTriesElapsed }, function(x) { });
    FB.getLoginStatus(function (response) {
        if (response.status == "connected") {
            var src = './LoginThroughFacebook.aspx?access_token='
                                         + response.authResponse.accessToken
                                         + '&fbUserID=' + response.authResponse.userID
                                         + '&signed=' + response.authResponse.signedRequest;
            fbUserIsAuthorized = true;
            fbUserIsLoggedIn = true;
            if (window != window.top || top !== self) {
                window.location = src;
            }
            else {
                var name = response.authResponse.userID;
                FB.api('/me', function (response) {
                    name = response.name;
                    jQuery('#fbLoginModal').html('<br\>Are you ' + name + ' ? <br/><br/>' + 'Do you want to login with this facebook account ?');
                    jQuery("#fbLoginModal").dialog({
                        resizable: false,
                        height: 250,
                        width: 600,
                        modal: true,
                        buttons: {
                            "YES - Use Facebook": function () {
                                $(this).dialog("close");
                                window.location = src;
                            },
                            "NO - Logout from Facebook": function () {
                                FB.logout(function (response) { });
                                $(this).dialog("close");
                            },
                            "CANCEL - Do Nothing": function () {
                                $(this).dialog("close");
                            }
                        }
                    });
                });
            }
        }
        else {
            FB.login(function (response) {
                //showResponse(response);
                if (response.authResponse) {
                    if (response.authResponse.accessToken) {
                        var src = './LoginThroughFacebook.aspx?access_token='
                                         + response.authResponse.accessToken
                                         + '&fbUserID=' + response.authResponse.userID
                                         + '&signed=' + response.authResponse.signedRequest;
                        fbUserIsAuthorized = false;
                        fbUserIsLoggedIn = true;
                        if (window != window.top) {
                            window.location = src;
                        }
                    } else {
                        //showResponse(response);
                        window.location = './Login.aspx?facebook=0';
                    }
                } else {
                    //showResponse(response);
                    window.location = './Login.aspx?facebook=0';
                }
            }, { scope: fbBasicPermissionScope });
        }
    });
}
function showResponse(response) {
    //debugger;
    var outStr = "response.status = " + response.status + " \n" +
                  "response.authResponse.accessToken = " + response.authResponse.accessToken + "\n" +
                  "response.authResponse.userID = " + response.authResponse.userID;
    //alert(outStr);
    if (console) console.log(outStr);
}
function fbEnsureInit(callback) {
    if (!window.fbApiInit) {
        setTimeout(function() { fbEnsureInit(callback); }, 50);
    } else {
        if (callback) {
            callback();
        }
    }
}