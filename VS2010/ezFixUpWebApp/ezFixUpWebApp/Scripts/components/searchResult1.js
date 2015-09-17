jQuery(function($) {
    jQuery('.ThumbsWrapMF').each(function() {
        jQuery(this).hover(
              function () { jQuery(this).find('.ThumbIconsWrapMF').show(); }
            , function () { jQuery(this).find('.ThumbIconsWrapMF').hide(); }
         );
    });
    jQuery('.ThumbsWrap, .ThumbsWrapGrid').each(function () {
        jQuery(this).hover(
              function () { jQuery(this).find('.ThumbIconsWrap').show(); }
            , function () { jQuery(this).find('.ThumbIconsWrap').hide(); }
        );
    });

    //    $('div.ThumbsWrapMF').live('mouseover', function() {
    //        $('.ThumbIconsWrapMF').hide(); 
    //        $(this).find('div.ThumbIconsWrapMF').show();
    //    });
    //    $('.ThumbsWrapMF').live('mouseleave', function() {
    //        $(this).find('.ThumbIconsWrapMF').hide();
    //    });
    //    $('.ThumbsWrap, .ThumbsWrapGrid').live('mouseenter', function() {
    //        $('.ThumbIconsWrap').hide();
    //    });
    //    $('.ThumbsWrap, .ThumbsWrapGrid').live('mouseover', function() {
    //        $(this).find('.ThumbIconsWrap').show();
    //    });
    //    $('.ThumbsWrap, .ThumbsWrapGrid').live('mouseleave', function() {
    //        $(this).find('.ThumbIconsWrap').hide();        
    //    });
});

function matchmakingRequestClick(e, lbreq) {
    var reqSelector = "#" + lbreq;
    var mfName = jQuery(reqSelector).attr('mf_username');
    var popIcn = jQuery(reqSelector).parent('.ThumbIconsWrapMF');
    var viewed = jQuery(reqSelector).attr('viewed');
    var viewed_name = jQuery(reqSelector).attr('viewed_name');
    e.stopImmediatePropagation();
    if (popIcn.hasClass("MfPopIconsBg0")) {
        confirmMatchmakingRequest(popIcn, mfName, viewed, viewed_name);  
    }
    return false;
}

var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_pageLoaded(SearchResultPageLoadedEventHandler);
function SearchResultPageLoadedEventHandler() {
    jQuery(function($) {
        jQuery('.ThumbsWrapMF').each(function () {
            jQuery(this).hover(
              function () { jQuery(this).find('.ThumbIconsWrapMF').show(); }
            , function () { jQuery(this).find('.ThumbIconsWrapMF').hide(); }
         );
        });
        jQuery('.ThumbsWrap, .ThumbsWrapGrid').each(function () {
            jQuery(this).hover(
              function () { jQuery(this).find('.ThumbIconsWrap').show(); }
            , function () { jQuery(this).find('.ThumbIconsWrap').hide(); }
        );
        });

    });
}