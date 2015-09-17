﻿var reqInstance;
function pageLoad() {

    // Get a reference to the PageRequestManager.
    reqInstance = Sys.WebForms.PageRequestManager.getInstance();
    
    Sys.Application.add_init(App_masterInit);    
    Sys.Application.add_init(App_actionPopupInit);

    // If function contentPageLoad exists, execute it.
    if (typeof contentPageLoad == 'function')
        contentPageLoad();
}
function bindEvent(element, type, handler) {
    if (element.addEventListener) {
        element.addEventListener(type, handler, false);
    } else {
        element.attachEvent('on' + type, handler);
    }
}

//to disable selects
//document.onselectstart = new Function('return false');

//function ds(e) { return false; }
//function ra() { return true; }
//document.onmousedown = ds; 
//document.onclick = ra;

function App_masterInit() {
    //alert('App_masterInit');
    //document.onclick = function() { return false };
    // Using that prm reference, hook _initializeRequest
    // and _endRequest, to run our code at the begin and end
    // of any async postbacks that occur.
    reqInstance.add_initializeRequest(InitializeRequest);
    reqInstance.add_pageLoaded(EndRequest);
    reqInstance.add_endRequest(EndRequest);

    //AspNetDating.Services.HeaderService.GetHeader(OnSuccessGetHeader, OnFailure);
}
function ShowProgressHandler(sender, args) {
    //show div with animation
    $get('progressDiv').className = '';
    // Change the Container div's CSS class to .Progress.
    $get('wrap').className = 'container Progress';    
}
function HideProgressHandler(sender, args) {
    //hide div with animation
    $get('progressDiv').className = 'invisible';
    // Change the Container div's class back to .Normal.
    $get('wrap').className = 'container Normal';    
}

// Executed anytime an async postback occurs.
function InitializeRequest(sender, args) {
    //alert('InitializeRequest');
    stopDownloads();
    ShowProgressHandler(sender, args);
    // Get a reference to the element that raised the postback,
    //   and disables it.
    if (sender._postBackSettings != null) {
        $get(args._postBackElement.id).disabled = true;
    }       
}

// Executed when the async postback completes.
function EndRequest(sender, args) {
    HideProgressHandler(sender, args);
    document.onclick = function(){return true};
    //alert('EndRequest');
    // Get a reference to the element that raised the postback
    //   which is completing, and enable it.
    try
    {
        var srcElem = $get(sender._postBackSettings.sourceElement.id);
        if (srcElem != null) srcElem.disabled = false;
    }
    catch(e){}

//    $("img").each(function() {
//        if ($(this).attr('src') == '')
//            $(this).css({ visibility: 'hidden' });
//    });
//    jQuery(function($) {
//        //$('img').lazyload();
//        $("select").each(function() {
//            $("this > option").each(function(index) {
//                if ($(this).text() == null || $(this).text().trim() == "") {
//                    $(this).attr("text", pleaseChooseText);
//                    $(this).attr("style", "color:gray;");
//                    return false;
//                }
//            });
//        });
//        //$("select").customSelectStyle();
//    });
}

function stopDownloads() {
    if (window.stop !== undefined) {
        window.stop();
    }
    else if (document.execCommand !== undefined) {
        document.execCommand("Stop", false);
    }
}
function cancelEvent(e) {
    if (!e) e = window.event;
    if (e.preventDefault) { e.preventDefault(); }
    e.returnValue = false;
    //if (e.stopImmediatePropagation) { e.stopImmediatePropagation(); }
    if (e.stopPropagation) { e.stopPropagation(); }
    e.cancelBubble = true;
    return false;
}
function App_actionPopupInit() {
    
}

function OnSuccessGetHeader(result) {
    // Remove the .loading CSS from the div, to remove the 
    //  progress indicator background.
    Sys.UI.DomElement.removeCssClass($get('HeaderBlock'), 'loading');

    // Fill the div with the HTML generated from the user control.
    $get('HeaderBlock').innerHTML = result;
}

function OnFailure() {
    // Do something if our callback fails.  Retry it, perhaps.
}

window.onbeforeunload = function() {
    window.location.replace('https://apps.facebook.com/lovehitch/default.aspx');
}

function readCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}
function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}
function eraseCookie(name) {
    createCookie(name, "", -1);
}
function getPageName(str) {
    var index = str.lastIndexOf('/');
    var page = str.substr(index + 1, str.length - index);
    return page.toLowerCase();
}
//}); debugger;
//                 var page = getPageName(window.location.pathname);
//                 var historyPage = readCookie('history');
//                 var referrer = readCookie('referrer');
//                 if (historyPage == page && referrer && referrer.length > 0 && referrer != page)
//                     window.location = referrer;
//                 createCookie('history', getPageName(window.document.referrer), 1);
//                 createCookie('referrer', getPageName(window.location.pathname), 1);

function popupMatchmakingRequestClick(event, lbreq) {
    cancelEvent(event);
    //debugger;
    var lbSelector = "#" + lbreq;
    var popIcn = $(lbSelector).parent('.ThumbIconsWrap');
    var mfUsername = popIcn.attr('mf_username');
    var viewed = popIcn.attr('viewed');
    var viewed_name = popIcn.attr('viewed_name');
    if (popIcn.hasClass("MfPopIconsBg0")) {
        confirmMatchmakingRequest(popIcn, mfUsername, viewed, viewed_name);
    }
    return false;
}
String.prototype.format = function () {
    var formatted = this;
    for (var i = 0; i < arguments.length; i++) {
        var regexp = new RegExp('\\{' + i + '\\}', 'gi');
        formatted = formatted.replace(regexp, arguments[i]);
    }
    return formatted;
};
function confirmMatchmakingRequest(popIcn, mfUsername, viewed, viewed_name) {
    jQuery("#errorAlertPopup").html(matchRequestConfirmationPopupContentTemplate.format(viewed_name));
    //debugger;
    jQuery("#errorAlertPopup").dialog({
        width: 300,
        modal: true,
        resizable: false,
        buttons: {
            SEND: function () {
                $(this).dialog("close");
                var matchReqHandler = "./handlers/MatchmakingRequest.ashx";
                jQuery.get(matchReqHandler, { mf: mfUsername, un: viewed }, function (result) {
                    popIcn.removeClass("MfPopIconsBg0");
                    popIcn.addClass("MfPopIconsBg1");
                    postActionInvitationCheck(mfUsername, viewed);
                });
            },
            CANCEL: function () {
                jQuery(this).dialog("close");
            }
        }
    });
}
function postActionInvitationCheck(user1, user2) {
    jQuery.get('./Handlers/PostActionInvitaionCheck.ashx',
                        { un1: user1, un2: user2 },
                        function (inviteFacebookIds) {
                            debugger;
                            if (inviteFacebookIds != '') 
                                sendAppRequestToFacebookIds(inviteFacebookIds);
                            else sendAppRequestToManyRecipients();
                        }
                    );
}
function RedirectToInviteFriends() {
    window.location = './InviteFriends.aspx';
}

function sendAppRequestToFacebookIds(inviteFacebookIds) {
    window.clearInterval(waitForFB);
    var obj = {
        method: 'apprequests',
        email_invite: "true",
        to: inviteFacebookIds,
        link: 'https://lovehitch.com',
        title: invite_after_action_title, 
        message: invite_after_action_message,
        app_id: facebookAppId
    };
    function sendAppRequestToFacebookIdsCallback(response) {
        console.log(response);
        sendAppRequestToManyRecipients();
    }
    FB.ui(obj, sendAppRequestToFacebookIdsCallback);    
}
function sendAppRequestToManyRecipients(filterBy) {
    var obj = {
        method: 'apprequests',
        email_invite: "true",
        //to: strUIDs,
        link: 'https://lovehitch.com',
        title: invite_title, 
        message: invite_message, 
        filters: ["app_non_users"],
        app_id: facebookAppId
    };
    function sendRequestToManyRecipientsCallback(response) {
        console.log(response);
    }
    FB.ui(obj, sendRequestToManyRecipientsCallback);
}
                