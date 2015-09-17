jQuery(function($) {
    jQuery(".expandee div.expander").live('click', function(event) {//expands answers for questions
        jQuery(this).next(".expandee").slideToggle('fast');
        jQuery(this).find("a").toggleClass('plus1');
        jQuery(this).find("a").toggleClass('minus1');
        //SetHeight();
        event.stopImmediatePropagation();  
        event.preventDefault();
        return false;
    });
    jQuery("a.topicexpander").live('click', function(event) {//expends questions for topic
        jQuery(this).next(".expandee").slideToggle('fast');
        //$(this).toggleClass('plus1');
        //$(this).toggleClass('minus1');
        //SetHeight();
        event.stopImmediatePropagation();  
        event.preventDefault();
        return false;
    });
    jQuery("a.expander").live('click', function(event) {//expends answers for questions of topic
        jQuery(this).next(".expandee").slideToggle('fast');
        jQuery(this).toggleClass('plus1');
        jQuery(this).toggleClass('minus1');
        //SetHeight();
        event.stopImmediatePropagation();  
        event.preventDefault();
        return false;
    });
    jQuery(".dropgender").change(function() {
        switch (jQuery(this).val()) {
            case "1":
                jQuery(".visibleformale").show();
                jQuery(".invisibleformale").hide();
                break;
            case "2":
                jQuery(".visibleforfemale").show();
                jQuery(".invisibleforfemale").hide();
                break;
            case "3":
                jQuery(".visibleforcouple").show();
                jQuery(".invisibleforcouple").hide();
                break;
        }
        //SetHeight();
    });
});
function setAdvanced(isAdv) {
    var state = jQuery(displayAdvanceSearchId).attr('value', isAdv);
}
var prm = Sys.WebForms.PageRequestManager.getInstance();
prm.add_pageLoaded(SearchPageLoadedEventHandler);
function SearchPageLoadedEventHandler() {
    jQuery(function($) {
        var state = jQuery(displayAdvanceSearchId).attr('value');
        if (state == 'True')
            jQuery("div#advancedSearchAccordionDiv").show();
        else
            jQuery("div#advancedSearchAccordionDiv").hide();
        jQuery('#fmt').addClass('js-isactive');
        jQuery('#fmt li').hover(
                    function() { jQuery(this).addClass('selected'); jQuery(this).find('ul.fmtDrpDwn:first').slideDown(200); },
                    function() { jQuery(this).removeClass('selected'); jQuery(this).find('ul.fmtDrpDwn:first').fadeOut(200); }
                );
    });
}