//$(window).load(function() {
//    var options = { stageW: '90px', stageH: '90px' };
//    $('.popeye').popeye();
//});

//$('.popeye').load(function() {
//    var options = { stageW: '90px', stageH: '90px' };
//    $(this).popeye();
//});
//var prm = Sys.WebForms.PageRequestManager.getInstance();
//prm.add_pageLoaded(PageLoadedEventHandler);
function PageLoadedEventHandler() {    
//    $('#fmt').addClass('js-isactive');
//    $('#fmt li').hover(
//        function() { $(this).addClass('selected'); $(this).find('ul.fmtDrpDwn:first').slideDown(200); },
//        function() { $(this).removeClass('selected'); $(this).find('ul.fmtDrpDwn:first').fadeOut(800); }
//    );
//    $("select").customSelectStyle();
//    $("input[type = checkbox]").customCheckboxes();
//    $("input[type = radio]").customRadioButtons();
//    $(".hasbirthdaypicker").each(function(i) { var ctl = $(this); 
//        ctl.datepicker({ beforeShow: function(input) { $(this).fadeOut().fadeIn(); }
//            , showOn: 'both', changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy'
//            , minDate: '01/01/50', maxDate: '12/31/99', defaultDate: '06/15/77' });
//    });
//    $(".hasdatepicker").each(function(i) { var ctl = $(this); 
//        ctl.datepicker({ beforeShow: function(input) { $(this).fadeOut().fadeIn(); }
//            , showOn: 'both', changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy'
//    });
//    $(".jqDatePicker").datepicker();
//    $(".jqDialog").dialog();
//    $(".jqDraggable").draggable();
//    $(".jqResizable").resizable();
//    $(".jqAccordion").accordion();
//    $(".jqButton").button();
//    $(".jqSlider").slider();
//    $(".jqSelectable").selectable();
//    $(".jqSortable").sortable();
//    $(".jqSortable").disableSelection();
//    $(".jqProgressbar").progressbar({ value: $(this).attr('pct') });            
//});
}
function contentPageLoad() {   //PageLoadedEventHandler
    //$("select").not("select.ui-datepicker-month, select.ui-datepicker-year").customSelectStyle();
    //$("select:disabled").closest('div').next('div.customStyleSelectBox').addClass('disabled');
    //$("select").customSelectStyle();
    //$("input[type = checkbox]").customCheckboxes();
    //$("input[type = radio]").customRadioButtons();
    //if (jQuery == undefined) return;
    jQuery(function($) {
        jQuery('#fmt').addClass('js-isactive');
        jQuery('#fmt li').hover(
            function() { jQuery(this).addClass('selected'); jQuery(this).find('ul.fmtDrpDwn:first').slideDown(200); },
            function() { jQuery(this).removeClass('selected'); jQuery(this).find('ul.fmtDrpDwn:first').fadeOut(800); }
        );
        var options1 = {};
        jQuery.getScript('scripts/jquery.popeye.js', function () {
            jQuery('#ppy1.ppy').popeye(options1);
        });
        //$("select").customSelectStyle();
        //$("input[type = checkbox]").customCheckboxes();
        //$("input[type = radio]").customRadioButtons();
//        $("button .ui-datepicker-trigger").css({ padding: 0 });
//        $(".hasbirthdaypicker").each(function(i) {
//            var ctl = $(this);
//            ctl.datepicker({ beforeShow: function(input) { $(this).fadeOut().fadeIn(); }
//            , showOn: 'both', changeYear: true, changeMonth: true, dateFormat: 'dd/mm/yy'
//            , minDate: '01/01/50', maxDate: '12/31/99', defaultDate: '06/15/77'
//            });
//        });
    });
}
//$(window).load(function() 
//{
//    $('#imgPhoto').imgNotes();
//});

//$('#imgPhoto').load(function() 
//{
//    $(this).imgNotes();
//});

//$(window).resize(function() 
//{
//    $('#imgPhoto').imgNotes();
//});
//$(window).load(function() 
//{
//    var options = { stageW: '90px', stageH: '90px' };
//    $('.popeye').popeye();
//});

//$('.popeye').load(function()
//{
// var options = { stageW: '90px', stageH: '90px' };
// $(this).popeye();
//});


function textCounter(fieldId, cntfieldId) {
    document.getElementById(cntfieldId).value = document.getElementById(fieldId).value.length;
}

function processEventInfo() {
    $get("hfIsNext").Value = isNextText;
}
//var hfID = '#' + '<%# hfIsNext.ClientID%>';
var isNextText = '';
function setIsNextHiddenField(isNext) {
    isNextText = isNext;
}
