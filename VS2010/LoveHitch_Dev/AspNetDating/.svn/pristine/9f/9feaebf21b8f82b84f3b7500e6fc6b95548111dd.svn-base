jQuery(function($) {
    $('.rollover').hover(function() {
        var currentImg = $(this).attr('src');
        $(this).attr('src', $(this).attr('data-hover'));
        $(this).attr('data-hover', currentImg);
    }, function() {
        var currentImg = $(this).attr('src');
        $(this).attr('src', $(this).attr('data-hover'));
        $(this).attr('data-hover', currentImg);
    });
    $('.switchInOut').mouseenter(function() {
        $(this).removeClass($(this).attr('cssOut'));
        $(this).addClass($(this).attr('cssIn'));
    });
   $('.switchInOut').mouseleave(function() {
        $(this).removeClass($(this).attr('cssIn'));
        $(this).addClass($(this).attr('cssOut'));
    });
});