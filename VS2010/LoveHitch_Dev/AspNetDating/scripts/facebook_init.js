function facebookLoginTry() {
    fbTriesElapsed--;
    var setSessionStateServiceUrl = "./handlers/SetSessionState.ashx";
    $.get(setSessionStateServiceUrl, {'key':'FacebookConnectTriesElapsed', 'val':fbTriesElapsed},function(x) { });
    FB.getLoginStatus(function(response) {
        if (response.status == "connected") {
            //            showResponse(response);
            //            window.location = './LoginThroughFacebook.aspx?access_token='
            //                             + response.authResponse.accessToken
            //                             + '&fbUserID=' + response.authResponse.userID
            //                             + '&signed=' + response.authResponse.signedRequest;
        }
        else {
            FB.login(function(response) {
                //showResponse(response);
                if (response.authResponse) {
                    if (response.authResponse.accessToken) {
                        //debugger;
                        window.location = './LoginThroughFacebook.aspx?access_token='
                                         + response.authResponse.accessToken
                                         + '&fbUserID=' + response.authResponse.userID
                                         + '&signed=' + response.authResponse.signedRequest;
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
    var outStr = "response.status = "+ response.status + " \n" +
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
//fbEnsureInit(function() {
//    if (console) console.log("this will run once FB is initialized");    
//});