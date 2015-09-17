﻿var popIconsSelctorIds = {
    modalMoreIconsId: "#modalMoreIcons",
    modalHeaderId: "#modalHeader",
    hfViewId: "#hfViewed",
    lbModalBlockId: "#lbModalBlock",
    lbModalFavoriteId: "#lbModalFavorite",
    lbModalRemoveFavoriteId: "#lbModalRemoveFavorite",
    lbModalInviteFriendId: "#lbModalInviteFriend",
    lbModalRemoveFriendId: "#lbModalRemoveFriend",
    lbModalAcceptFriendId: "#lbModalAcceptFriend",
    lbModalDenyFriendId: "#lbModalDenyFriend",
    lbModalSendMsgId: "#lbModalSendMsg",
    lbModalSendGiftId: "#lbModalSendGift",
    lbModalWinkId: "#lbModalWink",
    lbModalMatchRequestId: "#lbModalMatchRequest",
    modalPrimaryImageId: "#modalPrimaryImage",
    modalBasicDetailsId: "#modalBasicDetails",
    lblModalUsernameId: "#lblModalUsername",
    lblModalGenderId: "#lblModalGender",
    lblModalInterestedInId: "#lblModalInterestedIn",
    lblModalAgeId: "#lblModalAge",
    lblModalCountryId: "#lblModalCountry",
    lblModalLastOnlineId: "#lblModalLastOnline"
};
var sId = popIconsSelctorIds;

function PopulateModalIcons(viewer, viewed, imageId) {
    //debugger;
    var username = viewed.trim();
    jQuery(function ($) {
        $(sId.lblModalUsernameId).html(username);
        $(sId.modalPrimaryImageId).attr('src', './Image.ashx?id=' + imageId);
        //$(sId.hfViewId).val(username);
        
        $(sId.modalMoreIconsId).unbind('mouseenter');
        $(sId.modalMoreIconsId).bind('mouseenter', function () {

            jQuery(sId.lbModalBlockId).unbind('click');
            jQuery(sId.lbModalBlockId).bind('click', function () {
                window.location = "AddRemoveFavorite.aspx?cmd=block&src=profile&uid=" + username;
            });

            jQuery(sId.lbModalFavoriteId).unbind('click');
            jQuery(sId.lbModalFavoriteId).bind('click', function () {
                window.location = "AddRemoveFavorite.aspx?cmd=add&src=profile&uid=" + username;
            });
            jQuery(sId.lbModalRemoveFavoriteId).unbind('click');
            jQuery(sId.lbModalRemoveFavoriteId).bind('click', function () {
                window.location = "AddRemoveFavorite.aspx?cmd=remove&src=profile&uid=" + username;
            });
            jQuery(sId.lbModalInviteFriendId).unbind('click');
            jQuery(sId.lbModalInviteFriendId).bind('click', function () {
                window.location = "AddRemoveFriend.aspx?cmd=add&src=profile&uid=" + username;
            });
            jQuery(sId.lbModalRemoveFriendId).unbind('click');
            jQuery(sId.lbModalRemoveFriendId).bind('click', function () {
                window.location = "AddRemoveFriend.aspx?cmd=remove&src=profile&uid=" + username;
            });
            jQuery(sId.lbModalAcceptFriendId).unbind('click');
            jQuery(sId.lbModalAcceptFriendId).bind('click', function () {
                window.location = "AddRemoveFriend.aspx?cmd=add&src=profile&uid=" + username;
            });
            jQuery(sId.lbModalDenyFriendId).unbind('click');
            jQuery(sId.lbModalDenyFriendId).bind('click', function () {
                window.location = "AddRemoveFriend.aspx?cmd=remove&src=profile&uid=" + username;
            });
            jQuery(sId.lbModalSendMsgId).unbind('click');
            jQuery(sId.lbModalSendMsgId).bind('click', function () {
                window.location = "SendMessage.aspx?src=profile&to_user=" + username;
            });
            jQuery(sId.lbModalSendGiftId).unbind('click');
            jQuery(sId.lbModalSendGiftId).bind('click', function () {
                window.location = "SendMessage.aspx?src=profile&type=gift&to_user=" + username;
            });
            jQuery(sId.lbModalWinkId).unbind('click');
            jQuery(sId.lbModalWinkId).bind('click', function () {
                window.location = "SendMessage.aspx?src=profile&type=wink&to_user=" + username;
            });
            jQuery(sId.lbModalMatchRequestId).unbind('mouseenter');
            jQuery(sId.lbModalMatchRequestId).bind('mouseenter', function () {
                jQuery(this).attr('image_id', imageId);
                jQuery(this).attr('username', viewed);
                var actionUrl;
                //jQuery(this).attr('href', ""/*actionUrl + username*/);
            });
            for (var i = 0; i < 3; i++) {
                var divId = "#mf" + (i + 1) + "div";
                if (!jQuery(divId).attr('title')) jQuery(divId).hide();
            }
        });
    });
}
function clearSessionStatusMessage(redirectUrl) {
    var setSessionHandler = "./Handlers/SetSessionState.ashx";
    jQuery.get(setSessionHandler, { key: "LastViewedUsername", val: "" }, function() { });
    jQuery.get(setSessionHandler, { key: "ShowStatus_Message", val: "" }, function() { });
    jQuery.get(setSessionHandler, { key: "ShowStatus_LinkURL", val: "" }, function() { });
    jQuery.get(setSessionHandler, { key: "ShowStatus_LinkText", val: "" }, function () { });
    if (redirectUrl != null && redirectUrl != '')
        window.location = redirectUrl;
}

function PopulateMutualFriendsModal(jsonData, viewed) {
    //var modal = $find("programmaticModalPopupBehavior");
    //debugger;
    jQuery(sId.modalHeaderId).html(jsonData.QryUserDisplayName);
    jQuery(sId.hfViewId).val(jsonData.QryUsername);

    SetModalLinksVisibility(jsonData);
    var mfList = jsonData.MutualFriends;
    if (mfList.length == 0) jQuery("#pnlPopMF").hide();
    else jQuery("#pnlPopMF").show();
    for (var i = 0; i < mfList.length; i++) {
        //if (jsonData.length > 0) {
        var divId = "#mf" + (i + 1) + "div";
        var popId = "#mf" + (i + 1) + "pop";
        //alert(mfList[i].AlreadyRequested);
        jQuery(popId).removeClass("MfPopIconsBg0");
        jQuery(popId).removeClass("MfPopIconsBg1");
        var bgClassName = (mfList[i].AlreadyRequested) ? "MfPopIconsBg1" : "MfPopIconsBg0";
        jQuery(popId).addClass(bgClassName);
        jQuery(popId).attr('viewed', viewed);
        jQuery(popId).attr('viewed_name', jsonData.QryUserDisplayName);
        var mfName = mfList[i].Name;
        var mfUsername = mfList[i].Username;
        jQuery(popId).attr('mf_name', mfName);
        jQuery(popId).attr('mf_username', mfUsername);
        if (mfList[i].PhotoId != -99) {
            //$(divId).attr('title', mfList[i].Username);
            jQuery(divId).attr('title', mfList[i].Name);
            jQuery(divId).attr('style', 'background:url(' + mfList[i].PhotoUrl + ') no-repeat');
            jQuery(divId).attr('onclick', "location.href = 'ShowUser.aspx?uid=" + mfList[i].Username + "';");
            jQuery(lbAskIds[i]).bind('mousedown', function () { window.location = "Ask_" + mfUsername + "_about_" + viewed + ".aspx"; });
            var matchReqHandler = "./handlers/MatchmakingRequest.ashx"; //?req=" + mfList[i].Username + "&with=" + viewed;
            jQuery(lbReqIds[i]).click(function (event) {
            cancelEvent(event)
            if (jQuery(popId).hasClass("MfPopIconsBg0")) {
                    jQuery.get(matchReqHandler, { mf: mfUsername, un: viewed }, function (result) {
                        //debugger;
                        jQuery(popId).removeClass("MfPopIconsBg0");
                        jQuery(popId).addClass("MfPopIconsBg1");
                    });
                }
                return false;
            });
        }
        else {
            jQuery(divId).attr('style', 'visibility:hidden');
        }
    }
    jQuery('div.ThumbsWrapModal').unbind('mouseenter');
    jQuery('div.ThumbsWrapModal').live('mouseenter',function() {
        jQuery('.ThumbIconsWrap').hide();
        jQuery(this).find('.ThumbIconsWrap').show();
    });
    
}
function closeMutualFriendsModal() {
    for (var i = 0; i < 3; i++) {
        var divId = "#mf" + (i + 1) + "div";
        jQuery(divId).attr('title', '');
        jQuery(divId).attr('style', '');
        jQuery(lbAskIds[i]).attr('href', "");
        jQuery(lbReqIds[i]).attr('href', "");
    }
    $(sId.modalMoreIconsId).hide('fold');
}

function SetModalLinksVisibility(jsonData) {
    var viewer = jsonData.CurrentUsername;
    var viewed = jsonData.QryUsername;
    var isFriend = jsonData.IsFriend;
    var hasRequest = jsonData.InFriendshipRequest;
    var isFavorite = jsonData.IsFavorite;
    var isBlocked = jsonData.IsBlocked;
    jQuery(sId.lbModalInviteFriendId).css('display', isFriend ? "none" : "");
    jQuery(sId.lbModalRemoveFriendId).css('display', isFriend ? "" : "none");
    jQuery(sId.lbModalAcceptFriendId).css('display', hasRequest ? "" : "none");
    jQuery(sId.lbModalDenyFriendId).css('display', hasRequest ? "" : "none");
    jQuery(sId.lbModalFavoriteId).css('display', isFavorite ? "none" : "");
    jQuery(sId.lbModalRemoveFavoriteId).css('display', isFavorite ? "" : "none");
    jQuery(sId.lbModalBlockId).css('display', isBlocked ? "none" : "");
}


jQuery(function ($) {
    //$('div.ThumbsWrap, div.ThumbsWrapGrid').unbind('mouseenter');
    jQuery('div.ThumbsWrap, div.ThumbsWrapGrid').live('mouseenter', function () {
        jQuery('.ThumbIconsWrap').hide();
        jQuery('div.ThumbsWrap, div.ThumbsWrapGrid').css('border', 'solid 1px Lavender');
        jQuery(this).css('border', 'solid 1px DarkCyan'); //#12657f
        jQuery(this).find('.ThumbIconsWrap').show();
        jQuery(function () { jQuery('.tipsy').tipsy(); jQuery('.tipsys').tipsy({ gravity: 's' }); });
    });
    //$('div.MoreIconWrap').unbind('mouseenter');
    jQuery('div.MoreIconWrap').live('mouseenter', function () {
        jQuery(function () { jQuery('.tipsy').tipsy(); jQuery('.tipsys').tipsy({ gravity: 's' }); });
    });
    //$('.ThumbIconsWrap, .MoreIconWrap').unbind('click');
    //    jQuery('.ThumbIconsWrap, .MoreIconWrap').live('click', function (e) {
    //        //cancelEvent(e);
    //        //e.preventDefault();
    //        //e.stopImmediatePropagation();
    //        //return false;
    //    });
    var viewed = '';
    //$('.ThumbIconsWrap, .MoreIconWrap').unbind('mousedown');
    var imageId = -1;
    
    jQuery('.ThumbIconsWrap, .MoreIconWrap').live('mousedown', function (e) {
        
        viewed = jQuery(this).attr("viewed");
        imageId = jQuery(this).attr("image_id");
        
        if (viewer == null || viewer == "" || viewed == null || viewed == "" || imageId == null || imageId == "")
            return true; //window.location = "Registration.aspx";
        else {
            cancelEvent(e);
            $(this).bind('mouseup, click', function (e) {
                cancelEvent(e);
                e.stopImmediatePropagation();
                $(this).unbind('mouseup, click');
                return false;
            });
        //e.stopImmediatePropagation();
            var serviceUrl = "./handlers/GetJsonMutualFriends.ashx?viewer=" + viewer + "&viewed=" + viewed;
            //$(document).getAJAJ(serviceUrl, "GET", PopulateMutualFriendsModal, "", "", false);
            var url = "./handlers/GetJsonMutualFriends.ashx";
            jQuery.ajax({
                //type: "POST",
                type: "GET",
                //url: "./services/SearchService.asmx/GetMutualFriends",
                url: serviceUrl,
                async: false,
                contentType: "application/json; charset=utf-8",
                cache: false,
                dataType: "json",
                beforeSend: function (x) {
                    if (x && x.overrideMimeType) {
                        x.overrideMimeType("application/json;charset=UTF-8");
                    }
                },
                error: function (e, xhr) { alert("Error: " + xhr); },
                success: function (str, status, jqxhr) {
                    var jsonString = jqxhr.responseText;
                    var jsonObj; //= JSON.parseWithDate(jsonString);
                    debugger;
                    if (typeof (JSON) !== 'undefined' && typeof (JSON.parse) === 'function')
                        try { jsonObj = jsonString.parseJSON(); }
                        catch (e) { jsonObj = eval('(' + jsonString + ')'); }
                    else jsonObj = eval('(' + jsonString + ')');
                    if (jsonObj.hasOwnProperty('d')) jsonObj = jsonObj.d;
                    debugger;
                    PopulateMutualFriendsModal(jsonObj, viewed);
                }
            });
            //alert(viewed);
            //ShowModalPopup(viewer, viewed, imageId);
        }
        //e.stopPropagation();
        return false;
    });
    $(sId.modalMoreIconsId).dialog({
        autoOpen: false,
        minWidth: 470,
        minHeight: 340,
        show: "fold",
        hide: "slide",
        modal: false
    });
    jQuery('.ThumbIconsWrap, .MoreIconWrap').live('mousedown', function (e) {
        cancelEvent(e);
        e.stopImmediatePropagation();
        if (viewer == null || viewer == "")
            window.location = "Registration.aspx";
        else {
            $(sId.modalMoreIconsId).dialog('open');
            PopulateModalIcons(viewer, viewed, imageId);            
        }
    });
});    