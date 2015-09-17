﻿
//var prm = Sys.WebForms.PageRequestManager.getInstance();
//prm.add_pageLoaded(PageLoadedEventHandler);
function contentPageLoad() {   //PageLoadedEventHandler
    
}

function triggerAsyncImgUpload() {
    var obj = document.getElementById(asyncImageId);
    var firedObj = $(obj).find("input[type=file]");
    var fireOnThis = document.getElementById(firedObj.attr('id'));
    if (fireOnThis.click) {
        fireOnThis.click();
    }
    else if (document.createEvent) {
        var evObj = document.createEvent('MouseEvents');
        evObj.initMouseEvent('click', true, true, window, 1, 12, 345, 7, 220, false, false, true, false, 0, null);
        fireOnThis.dispatchEvent(evObj);
    }
    else if (document.createEventObject) {
        fireOnThis.fireEvent('onclick');
    }
    return false;
}
var dialog_flag = true;
jQuery(document).ready(function() {
    var objIdSelector = "#" + asyncImageId;
    var btnObj = jQuery(objIdSelector).find("input[type=file]");
    btnObj.css({ background: 'green !important', color: 'red !important', fontWeight: 'bold' });
    var bgPos = (jQuery(document.body).attr('dir') == 'rtl') ? "left" : "right";
    //$("div.StandardBoxContent").attr("style",
    //            "background:url(Images/register_form/reg_couple.png) no-repeat; background-position:" + bgPos +" top;");
    jQuery("#registration_lt").attr("style", "float:left;");
});

function uploadError(sender, args) { return; }
function uploadStarted(sender, args) { return; }
function uploadComplete(sender, args) {
    var filename = "AsyncFileUpload/Uploads/" + args.get_fileName();
    jQuery("#loadedImage").fadeOut(500, function() {
        jQuery(this).attr("src", filename);
        jQuery(this).fadeIn(500);
    });
    return;
}
function openContinuePopup() {
    var jqSelector = '#' + linkBtnId;
    var imgId = jQuery(jqSelector).attr('image_id');
    var username = jQuery(jqSelector).attr('username');
    var getSessionStateService = "./handlers/GetSessionState.ashx?key=IsRegistrationFormValidated";
    jQuery.get(getSessionStateService, function(data) {
        if (data == "true") {
            jQuery('#continue_popup').show();
        }
    });
}

var contentTxt;

$(document).ready(function () {
    
    var setDialogPosition = function () {
        var obj = $('body');
        var W = obj.width();
        var H = obj.height();
        var LeftOff = (W / 2) + 100;
        var TopOff = (H / 2) - 200;
        $('.ui-dialog').css({ left: LeftOff, top: TopOff });
        //alert('width ' + W + ' height ' + H + ' left ' + LeftOff + ' top ' + TopOff);
    };

    //$(window).one("resize", setDialogPosition);

    var objId = "#" + asyncImageId;
    var btnObj = $(objId).find("input[type=file]");
    btnObj.css({ background: 'green !important', color: 'red !important', fontWeight: 'bold' });
    
    //    var $dialog = $("<div style='font-size:0.8em; width:150px;float:none;'></div>")
    //                    .html('<p>' + contentTxt + '</p>')
    //                    .dialog({
    //                        modal: true,
    //                        position: 'center',
    //                        autoOpen: false,
    //                        title: MatchmakerPopTitleTxt,
    //                        draggable: true,
    //                        resizable: false                        
    //                    });
    $(".mmExplainDialog").one('click', function () {
        contentTxt = $("#mmDescContent").html();
        $('#mmmExplainPopup').html(contentTxt);
        $('#mmmExplainPopup').attr('title', MatchmakerPopTitleTxt);
        var $dialog = $('#mmmExplainPopup').dialog({
            width: 350,
            autoOpen: true,
            modal: false,
            draggable: true,
            position: ['center', 'center'],
            resizable: false,
            show: 'fade'
        }); 
        //$dialog.dialog('open');
        setDialogPosition();
        return false;
    });
    //var bgPos = ($(document.body).attr('dir') == 'rtl') ? "left" : "right";
    //$("div.StandardBoxContent").attr("style",
    //            "background:url(Images/register_form/reg_couple.png) no-repeat; background-position:" + bgPos + " top;");
    //$("#registration_lt").attr("style", "float:left;");
});

function uploadError(sender, args) { return; }
function uploadStarted(sender, args) { return; }
function uploadComplete(sender, args) {
    var filename = "AsyncFileUpload/Uploads/" + args.get_fileName();
    $("#loadedImage").fadeOut(500, function() {
        $(this).attr("src", filename);
        $(this).fadeIn(500);
    });
    return;
}



jQuery(function($) {
    $(".jqDialog").dialog();
    var objId = asyncImageId;
    var objIdSelector = "#" + objId;
    var btnObj = $(objIdSelector).find("input[type=file]");
    btnObj.css({ background: 'green !important', color: 'red !important', fontWeight: 'bold' });
    var bgPos = ($(document.body).attr('dir') == 'rtl') ? "left" : "right";
    //$("div.StandardBoxContent").attr("style",
    //            "background:url(Images/register_form/reg_couple.png) no-repeat; background-position:" + bgPos + " top;");
    
    
    //$("#registration_lt").attr("style", "float:left;");



    //        $("input[name='btnRegister']").click(function() {
    //            $("form").validate({
    //                rules: {
    //                    fullname: "required",
    //                    username: {
    //                        required: true,
    //                        minlength: 6
    //                        //,remote: "users.php" 
    //                    },
    //                    password: {
    //                        required: true,
    //                        minlength: 6
    //                    },
    //                    password_confirm: {
    //                        required: true,
    //                        minlength: 6,
    //                        equalTo: "#password"
    //                    },
    //                    email: {
    //                        required: true,
    //                        email: true
    //                        //,remote: "emails.php" 
    //                    },
    //                    dateformat: "required",
    //                    terms: "required"
    //                },
    //                messages: {
    //                    fullname: "Enter your firstname",
    //                    username: {
    //                        required: "Enter a username",
    //                        minlength: jQuery.format("Enter at least {0} characters")
    //                        //,remote: jQuery.format("{0} is already in use") 
    //                    },
    //                    password: {
    //                        required: "Provide a password",
    //                        rangelength: jQuery.format("Enter at least {0} characters")
    //                    },
    //                    password_confirm: {
    //                        required: "Repeat your password",
    //                        minlength: jQuery.format("Enter at least {0} characters"),
    //                        equalTo: "Enter the same password as above"
    //                    },
    //                    email: {
    //                        required: "Please enter a valid email address",
    //                        minlength: "Please enter a valid email address"
    //                        //,remote: jQuery.format("{0} is already in use") 
    //                    },
    //                    dateformat: "Choose your preferred dateformat",
    //                    terms: " "
    //                },
    //                // the errorPlacement has to take the table layout into account 
    //                errorPlacement: function(error, element) {
    //                    if (element.is(":radio"))
    //                        error.appendTo(element.parent().next().next());
    //                    else if (element.is(":checkbox"))
    //                        error.appendTo(element.next());
    //                    else
    //                        error.appendTo(element.parent().next());
    //                },
    //                // specifying a submitHandler prevents the default submit, good for the demo 
    //                submitHandler: function() {
    //                    alert("submitted!");
    //                },
    //                // set this class to error-labels to indicate valid fields 
    //                success: function(label) {
    //                    // set   as text for IE 
    //                    label.html(" ").addClass("checked");
    //                }
    //            });
    //        });
});