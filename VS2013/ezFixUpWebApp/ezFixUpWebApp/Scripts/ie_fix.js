$(function() {
//if ($.browser.msie) 
    {
        $('div, table, ul, li').each(function(i) {
            if ($(this).css('position') != 'absolute')
            //&& $(this).css('z-index') == 0) 
            {
                $(this).css('z-index', 999 - i);
            }
            var minWidth = $(this).css('min-width').replace('px', '');
            if (minWidth > 0) {
                $(this).css('width','expression(' + $(this).clientWidth < minWidth ? minWidth + "px !important" : "auto" + ')');
            }
            var minHeight = $(this).css('min-height').replace('px', '');
            if (minHeight > 0) {
                $(this).css('height', 'expression(' + $(this).clientHeight < minHeight ? minHeight + "px !important" : "auto" + ')');
            }
        });
    }
})