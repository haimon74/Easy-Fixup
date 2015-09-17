jQuery(function($) {
    var op = {};
    jQuery('.tipsys').each(function() {
        jQuery(this).tipsy(op);
    });
    jQuery('.ThumbsWrapMF').unbind('mouseenter');
    jQuery('.ThumbsWrapMF').live('mouseenter', function() {
        jQuery('.ThumbIconsWrapMF').hide();
        jQuery(this).find('.ThumbIconsWrapMF').show();
    });
    //    $('.ThumbsWrapMF').unbind('mouseleave');
    //    $('.ThumbsWrapMF').live('mouseleave', function() {
    //        $('.ThumbIconsWrapMF').hide();
    //    });
    //    $('.ThumbsWrap, .ThumbsWrapGrid').live('mouseout', function() {
    //        $('.ThumbIconsWrap').hide();
    //    });
});
function matchmakingRequestClick(e, lbreq) {
    //debugger;
    var reqSelector = "#" + lbreq;
    var lbReq = jQuery(reqSelector);
    var mfName = lbReq.attr('mf_username');
    var viewed = lbReq.attr('viewed');
    var viewed_name = lbReq.attr('viewed_name');
    var popIcn = lbReq.parent('.ThumbIconsWrapMF');
    cancelEvent(e);
    if (popIcn.hasClass("MfPopIconsBg0")) {
        confirmMatchmakingRequest(popIcn, mfName, viewed, viewed_name);
    }
    return false;
}