jQuery(function($) {
    var customSelectInterval = setInterval(function() {
        $("select").customSelectStyle();      
    }, 1000);
});

jQuery(function($) {
    var t; var t_flag = 1;
    function periodicJQueryUI() {
        clearTimeout(t);
        t_flag = 1;
        //$("select").customSelectStyle();
        //$("input[type=checkbox]").prettyCheckboxes();
        //$("input[type=radio]").prettyCheckboxes();
        $(".jqDatePicker").datepicker();
        $(".jqDialog").dialog();
        $(".jqDraggable").draggable();
        $(".jqResizable").resizable();
        $(".jqAccordion").accordion();
        $(".jqButton").button();
        $(".jqSlider").slider();
        $(".jqProgressbar20").progressbar({ value: 20 });
        $(".jqProgressbar40").progressbar({ value: 40 });
        $(".jqProgressbar60").progressbar({ value: 60 });
        $(".jqProgressbar80").progressbar({ value: 80 });
        $(".jqSelectable").selectable();
        $(".jqSortable").sortable();
        $(".jqSortable").disableSelection();
//        $("div.jqRT").each(function() { $(this).position({ of: $(this).parent(1), my: "right top", at: "right top", offset: "0 0", collision: "fit fit" }); });
//        $(".jqRTCT").each(function() { $(this).position({ of: $(this).parent(1), my: 'right top', at: 'center top', offset: '0 0', collision: 'fit fit' });});
//        $(".jqLT").each(function() { $(this).position({ of: $(this).parent(1), my: 'left top', at: 'left top', offset: '0 0', collision: 'fit fit' });});
//        $(".jqLTCT").each(function() { $(this).position({ of: $(this).parent(1), my: 'left top', at: 'center top', offset: '0 0', collision: 'fit fit' });});
//        $(".jqRB").each(function() { $(this).position({ of: $(this).parent(1), my: 'right bottom', at: 'right bottom', offset: '0 0', collision: 'fit fit' });});
//        $(".jqRBCB").each(function() { $(this).position({ of: $(this).parent(1), my: 'right bottom', at: 'center bottom', offset: '0 0', collision: 'fit fit' });});
//        $(".jqLB").each(function() { $(this).position({ of: $(this).parent(1), my: 'left bottom', at: 'left bottom', offset: '0 0', collision: 'fit fit' });});
//        $(".jqLBCB").each(function() { $(this).position({ of: $(this).parent(1), my: 'left bottom', at: 'center bottom', offset: '0 0', collision: 'fit fit' });});
//        $(".jqCC").each(function() { $(this).position({ of: $(this).parent(1), my: 'center center', at: 'center center', offset: '0 0', collision: 'fit fit' }); });
//        $('.jqShow').one('jqShowEvent', function(event) {
//            clearTimeout(t);
//            var effect = $(this).attr('effect');
//            $(this).show(effect, {}, 500, function() {
//                $(this).removeClass('jqShow'); $(this).addClass('jqHide');
//                t = setTimeout(periodicJQueryUI, 200);
//            });
//        });
//        $('.jqHide').one('jqHideEvent', function(event) {
//            clearTimeout(t);
//            var effect = $(this).attr('effect');
//            $(this).hide(effect, {}, 500, function() {
//                $(this).removeClass('jqHide'); $(this).addClass('jqShow');
//                t = setTimeout(periodicJQueryUI, 200);
//            });
//        });
        if ($('.jqShow , .jqHide')) {
            t_flag = 0;
            $('.jqShow').trigger('jqShowEvent');
            $('.jqHide').trigger('jqHideEvent');
        }
        else if (t_flag == 1)
            t = setTimeout(periodicJQueryUI, 1000);
    }
    periodicJQueryUI();
});