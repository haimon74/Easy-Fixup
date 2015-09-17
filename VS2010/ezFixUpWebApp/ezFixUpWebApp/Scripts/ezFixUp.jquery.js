var updateOnlineTimer;
var updateOnlineLock = false;
var currentNotificationDivOffset = 0;
var scrollSpeed = 1;
var scrollSign = +1;
var scrollStarted = false;

function InitializeOnlineCheck(interval) {
    updateOnlineTimer = setInterval("UpdateOnline()", interval);
    UpdateOnline();
}

function ShowNotification() {
    document.getElementById("divNotificationInner").scrollTop = 0;
    $get('divNotificationHeader').style.visibility = 'visible';
    $get('divNotificationInner').style.visibility = 'visible';
    if ($get('ctl00_divNotification'))
        $get('ctl00_divNotification').style.visibility = 'visible';
    if ($get('ctl00_ctl00_divNotification'))
        $get('ctl00_ctl00_divNotification').style.visibility = 'visible';
}

function HideNotification() {
    $get('divNotificationInner').innerHTML = '';
    $get('divNotificationHeader').style.visibility = 'hidden';
    $get('divNotificationInner').style.visibility = 'hidden';
    if ($get('ctl00_divNotification'))
        $get('ctl00_divNotification').style.visibility = 'hidden';
    if ($get('ctl00_ctl00_divNotification'))
        $get('ctl00_ctl00_divNotification').style.visibility = 'hidden';
}

function UpdateOnline() {
    if (updateOnlineLock == true) return;
    updateOnlineLock = true;

    ezFixUp.Services.OnlineCheck.UpdateOnline(OnUpdateOnlineSucceeded, OnUpdateOnlineError);
}

function OnUpdateOnlineSucceeded(result) {
    try {
        if (result == null) return;

        if (result.Notifications != null && result.Notifications.length) {
            for (var i = 0; i < result.Notifications.length; i++) {
                var notification = result.Notifications[i];
                if (notification.Type == "NewMessageNotification") {
                    var eventHtml = "<table cellpadding=\"0\" cellspacing=\"0\" class=\"innerwrap\"><tr><td class=\"tdleft\" align=\"center\" valign=\"middle\"><img src=\"" + notification.ThumbnailUrl +
                        "\" /></td><td class=\"tdright\" valign=\"middle\">" + notification.Text + "</td></tr></table><div class=\"clear\"></div>";
                    eventHtml = "<div class=\"NotificationInactive\" onmouseover=\"OnNotificationMouseOver(this);\" onmouseout=\"OnNotificationMouseOut(this);\"" +
                        " onclick=\"window.location.href = 'ShowMessage.aspx?mid=" + notification.MessageId + "'\">" + eventHtml + "</div>";
                    $get('divNotificationInner').innerHTML += eventHtml;
                    ShowNotification();
                }
                if (notification.Type == "GenericEventNotification") {
                    var eventHtml = "<table cellpadding=\"0\" cellspacing=\"0\" class=\"innerwrap\"><tr><td class=\"tdleft\" align=\"center\" valign=\"middle\"><img src=\"" + notification.ThumbnailUrl +
                        "\" /></td><td class=\"tdright\" valign=\"middle\">" + notification.Text + "</td></tr></table><div class=\"clear\"></div>";
                    eventHtml = "<div class=\"NotificationInactive\" onmouseover=\"OnNotificationMouseOver(this);\" onmouseout=\"OnNotificationMouseOut(this);\"" +
                        " onclick=\"window.location.href = '" + notification.RedirectUrl + "'\">" + eventHtml + "</div>";
                    $get('divNotificationInner').innerHTML += eventHtml;
                    ShowNotification();
                }
            }

            if (!scrollStarted) {
                scrollStarted = true;
                ScrollNotificationDiv();
            }
        }
    }
    finally {
        updateOnlineLock = false;
    }
}

function OnNotificationMouseOver(element) {
    element.className = 'NotificationHover';
}

function OnNotificationMouseOut(element) {
    element.className = 'NotificationInactive';
}

function OnUpdateOnlineError() {
    // Do nuthin'
}

function ScrollNotificationDiv() {
    var div = document.getElementById("divNotificationInner");
    var timerDelay = 1000;
    if (div.scrollHeight > 65) {
        currentNotificationDivOffset = currentNotificationDivOffset + (scrollSpeed * scrollSign);
        div.scrollTop = currentNotificationDivOffset;
        if (scrollSign == 1 && currentNotificationDivOffset > div.scrollHeight - 65) {
            scrollSign = -1;
            timerDelay = 1000;
        }
        else if (scrollSign == -1 && currentNotificationDivOffset <= 0) {
            scrollSign = +1;
            timerDelay = 1000;
        }
        else {
            timerDelay = (currentNotificationDivOffset % 65 == 0) ? 1000 : 50;
        }
    }
    setTimeout("ScrollNotificationDiv()", timerDelay);
}



/// <reference name="MicrosoftAjax.js"/>

var mouseX, mouseY;
var lastPreviewedUsername;
var userPreviewLoading = false;
var userPreviewCancelled = false;

function InitializeMouseTracking() {
    $addHandler($get(bodyId), "mousemove", trackMouse);
}

function trackMouse(eventElement) {
    mouseX = eventElement.clientX;
    mouseY = eventElement.clientY;
}

function showUserPreview(username) {
    if (typeof ezFixUp == 'undefined' || userPreviewLoading) return;
    userPreviewLoading = true;
    ezFixUp.Services.OnlineCheck.LoadUserPreviewInfo(username, OnLoadUserPreviewInfoSucceeded,
        OnLoadUserPreviewInfoError);
}

function OnLoadUserPreviewInfoSucceeded(result) {
    if (userPreviewCancelled) {
        userPreviewCancelled = false;
        userPreviewLoading = false;
        return;
    }
    if (result == null || lastPreviewedUsername == result.Username) return;

    // Compute the initial offset
    var offsetX, offsetY;
    if (document.documentElement && document.documentElement.scrollTop) {
        offsetX = document.documentElement.scrollLeft;
        offsetY = document.documentElement.scrollTop;
    } else {
        offsetX = document.body.scrollLeft;
        offsetY = document.body.scrollTop;
    }

    lastPreviewedUsername = result.Username;
    if (result.ThumbnailUrl != null && result.ThumbnailUrl != '') {
        $get("divUserPreviewImage").innerHTML = String.format("<img src=\"{0}\" class=\"photoframe\">",
            result.ThumbnailUrl);
    }
    else {
        $get("divUserPreviewImage").innerHTML = "";
    }
    $get("divUserPreviewDetails").innerHTML = String.format("<b>{0}</b><br>{1}/{2}<br>{3}",
        result.Username, result.Gender, result.Age, result.LastOnline);
    $get("divUserPreview").style.left = (offsetX + mouseX + 10) + 'px';
    $get("divUserPreview").style.top = (offsetY + mouseY - 20) + 'px';
    $get("divUserPreview").style.display = '';

    userPreviewLoading = false;
}

function OnLoadUserPreviewInfoError() {
    // Do nuthin'
}

function hideUserPreview(username) {
    lastPreviewedUsername = null;
    $get("divUserPreview").style.display = 'none';
    if (userPreviewLoading)
        userPreviewCancelled = true;
}


/**
* jQuery changecss plug-in v0.1
* @requires jQuery v1.2 
*
* Copyright (c) 2009 Amr Shahin amrnablus at gmail dot com
*
*
* @param string className - the class name that you want to change ( with . ). For example .myclass
* @param string propertyName  - the property name you wany to change. For example font-weight
* @param string value - the new value for the property
*
* Examples:
*
* jQuery().changecss( '.amrstyle',  'color', 'blue' );
* jQuery().changecss(  '.amrstyle', 'font-size','100px' ) ;
*
* Note:
* the code is quoted from "https://www.shawnolson.net/a/503/altering-css-class-attributes-with-javascript.html" with some modifications.
***************************************************************************************************/
(function($) {
    //plugin name - changecss
    jQuery.fn.changecss = function(className, propertyName, value) {

        return this.each(function() {

            if (
                (className == '') ||
                (propertyName == '') ||
                (value == '')) {
                return;
            }

            var propertyIndexName = false;
            var falg = false;
            var numberOfStyles = document.styleSheets.length

            if (document.styleSheets[0]['rules']) {
                propertyIndexName = 'rules';
            } else if (document.styleSheets[0]['cssRules']) {
                propertyIndexName = 'cssRules';
            }

            for (var i = 0; i < numberOfStyles; i++) {
                for (var j = 0; j < document.styleSheets[i][propertyIndexName].length; j++) {
                    if (document.styleSheets[i][propertyIndexName][j].selectorText == className) {
                        if (document.styleSheets[i][propertyIndexName][j].style[propertyName]) {
                            document.styleSheets[i][propertyIndexName][j].style[propertyName] = value;
                            falg = true;
                            break;
                        }
                    }
                }
                if (!falg) {
                    if (document.styleSheets[i].insertRule) {
                        document.styleSheets[i].insertRule(className + ' { ' + propertyName + ': ' + value + '; }', document.styleSheets[i][propertyIndexName].length);
                    } else if (document.styleSheets[i].addRule) {
                        document.styleSheets[i].addRule(className, propertyName + ': ' + value + ';');
                    }
                }
            }
        });
    };
})(jQuery);


(function($) {
    jQuery.fn.getAJAJ =
        function(serviceUrl, verb, proccessJsonCallback, reqScriptName, postScriptName, hasCss) {
            if (reqScriptName != "") {
                //ATTACH AND RUN CORRESPONDING JAVASCRIPT
                var script = "/scripts/" + reqScriptName + '.js';
                $.getScript(script);
            }
            if (hasCss) {
                //ATTACH CORRESPONDING STYLE SHEET                         
                var style = document.createElement('link');
                style.type = 'text/css';
                style.href = '/css/' + reqScriptName + '.css';
                style.rel = 'stylesheet';
                style.media = 'screen';
                document.getElementsByTagName('head')[0].appendChild(style);
            }
            jQuery.ajax({
                type: verb,
                //cache: false,
                //async: true,
                contentType: "application/json; charset=utf-8",
                url: serviceUrl,
                data: "{" + params + "}",
                //error: function(e, xhr) { alert("Error: " + xhr); },
                dataFilter: function(data) {
                    var msg; var res = JSON.parseWithDate(jsonString);

                    if (typeof (JSON) !== 'undefined' && typeof (JSON.parse) === 'function')
                        msg = JSON.parse(data);
                    else msg = eval('(' + data + ')');
                    if (msg.hasOwnProperty('d')) return msg.d;
                    else return msg;
                },
                success: function(ajaxData) {
                    if (typeof (proccessJsonCallback) === 'function')
                        proccessJsonCallback(ajaxData);
                    if (postScriptName != "") {
                        //ATTACH AND RUN CORRESPONDING New data post script 
                        script = "/scripts/" + postScriptName + '.js';
                        $.getScript(script);
                    }
                }
            });
        };
})(jQuery);

function changecss(theClass, element, value) {
    //Last Updated on October 10, 1020
    //documentation for this script at
    //https://www.shawnolson.net/a/503/altering-css-class-attributes-with-javascript.html
    var cssRules;

    var added = false;
    for (var S = 0; S < document.styleSheets.length; S++) {
        try {
            if (document.styleSheets[S]['rules']) {
                cssRules = 'rules';
            } else if (document.styleSheets[S]['cssRules']) {
                cssRules = 'cssRules';
            } else {
                //no rules found... browser unknown
            }

            for (var R = 0; document.styleSheets[S][cssRules] != null && R < document.styleSheets[S][cssRules].length; R++) {
                if (document.styleSheets[S][cssRules][R].selectorText == theClass) {
                    if (document.styleSheets[S][cssRules][R].style[element]) {
                        document.styleSheets[S][cssRules][R].style[element] = value;
                        added = true;
                        break;
                    }
                }
            }
        }
        catch (errr) { }
        if (!added) {
            try {
                document.styleSheets[S].insertRule(theClass + ' { ' + element + ': ' + value + '; }', document.styleSheets[S][cssRules].length);

            } catch (err) {
                try { document.styleSheets[S].addRule(theClass, element + ': ' + value + ';'); } catch (err) { }

            }

            //if(document.styleSheets[S].insertRule){
            //document.styleSheets[S].insertRule(theClass+' { '+element+': '+value+'; }',document.styleSheets[S][cssRules].length);
            //} else if (document.styleSheets[S].addRule) {
            //document.styleSheets[S].addRule(theClass,element+': '+value+';');
            //}
        }
    }
}

function exchangecss(classNameLeft, classNameRight) {

    if ((classNameLeft == '') || (classNameRight == '')) {
        return;
    }
    var objNameLeft = "." + classNameLeft;
    var objNameRight = "." + classNameRight;
    $(objNameLeft).addClass('toAddRightCssName');
    $(objNameLeft).removeClass(classNameLeft);
    $(objNameRight).addClass('toAddLeftCssName');
    $(objNameRight).removeClass(classNameRight);
    $('.toAddRightCssName').addClass(classNameRight);
    $('.toAddLeftCssName').addClass(classNameLeft);
    $('.toAddRightCssName').removeClass('toAddRightCssName');
    $('.toAddLeftCssName').removeClass('toAddLeftCssName');
}


// force redraw plugin
(function($) {

    $.fn.forceRedraw = function(brutal) {

        //this fix works for most browsers. it has the same effect as el.className = el.className.
        $(this).addClass('forceRedraw').removeClass('forceRedraw');

        //sometimes for absolute positioned elements the above fix does not work.
        //there's still a "brutal" way to force a redraw by changing the padding.
        if (brutal) {
            var paddingLeft = $(this).css('padding-left');
            var parsedPaddingLeft = parseInt(paddingLeft, 10);
            $(this).css('padding-left', ++parsedPaddingLeft);

            //give it some time to redraw
            window.setTimeout($.proxy(function() {
                //change it back
                $(this).css('padding-left', paddingLeft);
            }, this), 1);
        }

        return this;

    }

})(jQuery);


(function($) {

    $.fn.extend({

        customSelectStyle: function(options) {
            if (!$.browser.msie || ($.browser.msie && $.browser.version > 6)) {
                return this.each(function() {
                    if ($(this).hasClass('ui-datepicker-month') || $(this).hasClass('ui-datepicker-year'))
                        return;
                    var divId = "div" + $(this).attr('id');
                    var visibleDiv = $(this).parent().next();
                    if (!visibleDiv.hasClass('customStyleSelectBox')) {
                        var currentSelected = $(this).find(':selected');
                        $(this).after('<div class="customStyleSelectBox"><span class="customStyleSelectBoxInner">' + currentSelected.text() + '</span></span>').css({ position: 'relative', opacity: 0, fontSize: $(this).next().css('font-size') });
                        //id="' + divId + 'Visible" 
                        var selectBoxVisible = $(this).next();
                        var selectBoxWidth = parseInt($(this).width()) - parseInt(selectBoxVisible.css('padding-left')) - parseInt(selectBoxVisible.css('padding-right'));
                        var selectBoxSpanInner = selectBoxVisible.find(':first-child');
                        selectBoxVisible.css({ display: 'inline-block' });
                        selectBoxSpanInner.css({ width: selectBoxWidth, display: 'inline-block' });
                        var selectBoxHeight = parseInt(selectBoxVisible.height()) + parseInt(selectBoxVisible.css('padding-top')) + parseInt(selectBoxVisible.css('padding-bottom'));
                        $(this).height(selectBoxHeight).change(function() {
                            selectBoxSpanInner.text($(this).find(':selected').text()).parent().addClass('changed');
                        });
                        $(this).css({ display: 'none;' });
                        var wrapWidth = parseInt($(this).width()) + 10;
                        var wrapDiv = "<div  style='display:none;position:absolute;z-index:2000;width:" + wrapWidth + "px;height:" + selectBoxHeight + "px;'/>";
                        //id='" + divId + "Hidden'
                        $(this).wrap(wrapDiv);
                        visibleDiv = selectBoxVisible;
                    }
                    var selectBoxHidden = $(this).parent();
                    selectBoxHidden.position({ of: visibleDiv, my: "center bottom", at: "center bottom", colision: "none none", offset: "0 0" });
                    selectBoxHidden.css('display', 'inline-block');
                });
            }
        }
    });
})(jQuery);

function findPos(element) {
    if (element) {
        var parentPos = findPos(element.offsetParent);
        return [
              parentPos.X + element.offsetLeft,
              parentPos.Y + element.offsetTop
            ];
    } else {
        return [0, 0];
    }
}
jQuery.fn.prettyCheckboxes = function(settings) {
    settings = jQuery.extend({
        checkboxWidth: 17,
        checkboxHeight: 17,
        className: 'prettyCheckbox',
        display: 'list'
    }, settings);

    $(this).each(function() {
        var cbId = $(this).attr('id');
        if (!$(this).hasClass('hiddenCheckbox') && cbId != null && cbId != 'undefined') {
            $label = $('label[for="' + $(this).attr('id') + '"]');
            if ($label.length == 1) {
                // Add the checkbox holder to the label
                $label.prepend("<span class='holderWrap'><span class='holder'></span></span>");

                // If the checkbox is checked, display it as checked
                if ($(this).is(':checked')) { $label.addClass('checked'); };

                // Assign the class on the label
                $label.addClass(settings.className).addClass($(this).attr('type')).addClass(settings.display);

                // Assign the dimensions to the checkbox display
                $label.find('span.holderWrap').width(settings.checkboxWidth).height(settings.checkboxHeight);
                $label.find('span.holder').width(settings.checkboxWidth);

                // Hide the checkbox
                $(this).addClass('hiddenCheckbox');

                // Associate the click event
                $label.bind('click', function() {
                    $('input#' + $(this).attr('for')).triggerHandler('click');
                    $(this).toggleClass('checked');

                    if ($('input#' + $(this).attr('for')).is(':checkbox')) {
                        //$('input#' + $(this).attr('for')).attr('checked',$(this).hasClass('checked') ? "checked" : "");
                        $(this).find('span.holder').css('top', 0);
                    } else {
                        $toCheck = $('input#' + $(this).attr('for'));

                        // Uncheck all other radio in the group
                        $('input[name="' + $toCheck.attr('name') + '"]').each(function() {
                            $('label[for="' + $(this).attr('id') + '"]').removeClass('checked');
                        });

                        $(this).addClass('checked');
                        $toCheck.checked = true;
                    };
                });

                $('input#' + $label.attr('for')).bind('keypress', function(e) {
                    if (e.keyCode == 32) {
                        if ($.browser.msie) {
                            $('label[for="' + $(this).attr('id') + '"]').toggleClass("checked");
                        } else {
                            $(this).trigger('click');
                        }
                        return false;
                    };
                });
            }
        }
    });
};

checkAllPrettyCheckboxes = function(caller, container) {
    if ($(caller).is(':checked')) {
        // Find the label corresponding to each checkbox and click it
        $(container).find('input[type=checkbox]:not(:checked)').each(function() {
            $('label[for="' + $(this).attr('id') + '"]').trigger('click');
            if ($.browser.msie) {
                $(this).attr('checked', 'checked');
            } else {
                $(this).trigger('click');
            };
        });
    } else {
        $(container).find('input[type=checkbox]:checked').each(function() {
            $('label[for="' + $(this).attr('id') + '"]').trigger('click');
            if ($.browser.msie) {
                $(this).attr('checked', '');
            } else {
                $(this).trigger('click');
            };
        });
    };
};



jQuery.fn.customCheckboxes = function() {
    $(this).each(function() {
        var cbId = $(this).attr('id');
        if (!$(this).hasClass('myHiddenCheckbox') && cbId != null && cbId != 'undefined') {
            $label = $('label[for="' + $(this).attr('id') + '"]');
            if ($label.length == 1) {

                cbId = "#" + cbId;

                if ($(cbId).is(':checked')) $label.addClass('customChecked');
                else $label.addClass('customUnchecked');

                $label.click(function() {
                    $(cbId).trigger('click');
                    if ($(cbId).is(':checked')) {
                        //$(this).css('background-position', 'right -81px !important');
                        $(this).addClass('customChecked');
                        $(this).removeClass('customUnchecked');
                    }
                    else {
                        //$(this).css('background-position', 'right -21px  !important');
                        $(this).addClass('customUnchecked');
                        $(this).removeClass('customChecked');
                    }
                    return false;
                });
                $(cbId).addClass('myHiddenCheckbox');
            }
        }
    });
};
jQuery.fn.customRadioButtons = function() {
    $(this).each(function() {
        var rId = $(this).attr('id');
        if (!$(this).hasClass('myHiddenRadio') && rId != null && rId != 'undefined') {
            $label = $('label[for="' + $(this).attr('id') + '"]');
            if ($label.length == 1) {
                rId = "#" + rId;
                if ($(rId).is(':checked')) $label.addClass('radioChecked');
                else $label.addClass('radioUnchecked');

                $label.click(function() {
                    $(rId).trigger('click');
                    //$toCheck = $('input#' + $(this).attr('for'));
                    // Uncheck all other radio in the group
                    $('input[name="' + $(rId).attr('name') + '"]').each(function() {
                        $('label[for="' + $(this).attr('id') + '"]').removeClass('radioChecked');
                        $('label[for="' + $(this).attr('id') + '"]').addClass('radioUnchecked');
                    });

                    $(this).addClass('radioChecked');
                    $(this).removeClass('radioUnchecked');
                    return false;
                });
                $(rId).addClass('myHiddenRadio');
            }
        }
    });
};


var JSON;
if (!JSON) {
    JSON = {};
}

(function() {
    "use strict"; function f(n) { return n < 10 ? '0' + n : n; }
    if (typeof Date.prototype.toJSON !== 'function') {
        Date.prototype.toJSON = function(key) {
            return isFinite(this.valueOf()) ? this.getUTCFullYear() + '-' +
f(this.getUTCMonth() + 1) + '-' +
f(this.getUTCDate()) + 'T' +
f(this.getUTCHours()) + ':' +
f(this.getUTCMinutes()) + ':' +
f(this.getUTCSeconds()) + 'Z' : null;
        }; String.prototype.toJSON = Number.prototype.toJSON = Boolean.prototype.toJSON = function(key) { return this.valueOf(); };
    }
    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g, gap, indent, meta = { '\b': '\\b', '\t': '\\t', '\n': '\\n', '\f': '\\f', '\r': '\\r', '"': '\\"', '\\': '\\\\' }, rep; function quote(string) { escapable.lastIndex = 0; return escapable.test(string) ? '"' + string.replace(escapable, function(a) { var c = meta[a]; return typeof c === 'string' ? c : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4); }) + '"' : '"' + string + '"'; }
    function str(key, holder) {
        var i, k, v, length, mind = gap, partial, value = holder[key]; if (value && typeof value === 'object' && typeof value.toJSON === 'function') { value = value.toJSON(key); }
        if (typeof rep === 'function') { value = rep.call(holder, key, value); }
        switch (typeof value) {
            case 'string': return quote(value); case 'number': return isFinite(value) ? String(value) : 'null'; case 'boolean': case 'null': return String(value); case 'object': if (!value) { return 'null'; }
                gap += indent; partial = []; if (Object.prototype.toString.apply(value) === '[object Array]') {
                    length = value.length; for (i = 0; i < length; i += 1) { partial[i] = str(i, value) || 'null'; }
                    v = partial.length === 0 ? '[]' : gap ? '[\n' + gap +
partial.join(',\n' + gap) + '\n' +
mind + ']' : '[' + partial.join(',') + ']'; gap = mind; return v;
                }
                if (rep && typeof rep === 'object') { length = rep.length; for (i = 0; i < length; i += 1) { k = rep[i]; if (typeof k === 'string') { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ': ' : ':') + v); } } } } else { for (k in value) { if (Object.hasOwnProperty.call(value, k)) { v = str(k, value); if (v) { partial.push(quote(k) + (gap ? ': ' : ':') + v); } } } }
                v = partial.length === 0 ? '{}' : gap ? '{\n' + gap + partial.join(',\n' + gap) + '\n' +
mind + '}' : '{' + partial.join(',') + '}'; gap = mind; return v;
        } 
    }
    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function(value, replacer, space) {
            var i; gap = ''; indent = ''; if (typeof space === 'number') { for (i = 0; i < space; i += 1) { indent += ' '; } } else if (typeof space === 'string') { indent = space; }
            rep = replacer; if (replacer && typeof replacer !== 'function' && (typeof replacer !== 'object' || typeof replacer.length !== 'number')) { throw new Error('JSON.stringify'); }
            return str('', { '': value });
        };
    }
    if (typeof JSON.parse !== 'function') {
        JSON.parse = function() {
            var state, stack, container, key, value, escapes = { '\\': '\\', '"': '"', '/': '/', 't': '\t', 'n': '\n', 'r': '\r', 'f': '\f', 'b': '\b' }, string = { go: function() { state = 'ok'; }, firstokey: function() { key = value; state = 'colon'; }, okey: function() { key = value; state = 'colon'; }, ovalue: function() { state = 'ocomma'; }, firstavalue: function() { state = 'acomma'; }, avalue: function() { state = 'acomma'; } }, number = { go: function() { state = 'ok'; }, ovalue: function() { state = 'ocomma'; }, firstavalue: function() { state = 'acomma'; }, avalue: function() { state = 'acomma'; } }, action = { '{': { go: function() { stack.push({ state: 'ok' }); container = {}; state = 'firstokey'; }, ovalue: function() { stack.push({ container: container, state: 'ocomma', key: key }); container = {}; state = 'firstokey'; }, firstavalue: function() { stack.push({ container: container, state: 'acomma' }); container = {}; state = 'firstokey'; }, avalue: function() { stack.push({ container: container, state: 'acomma' }); container = {}; state = 'firstokey'; } }, '}': { firstokey: function() { var pop = stack.pop(); value = container; container = pop.container; key = pop.key; state = pop.state; }, ocomma: function() { var pop = stack.pop(); container[key] = value; value = container; container = pop.container; key = pop.key; state = pop.state; } }, '[': { go: function() { stack.push({ state: 'ok' }); container = []; state = 'firstavalue'; }, ovalue: function() { stack.push({ container: container, state: 'ocomma', key: key }); container = []; state = 'firstavalue'; }, firstavalue: function() { stack.push({ container: container, state: 'acomma' }); container = []; state = 'firstavalue'; }, avalue: function() { stack.push({ container: container, state: 'acomma' }); container = []; state = 'firstavalue'; } }, ']': { firstavalue: function() { var pop = stack.pop(); value = container; container = pop.container; key = pop.key; state = pop.state; }, acomma: function() { var pop = stack.pop(); container.push(value); value = container; container = pop.container; key = pop.key; state = pop.state; } }, ':': { colon: function() {
                if (Object.hasOwnProperty.call(container, key)) { throw new SyntaxError('Duplicate key "' + key + '"'); }
                state = 'ovalue';
            } 
            }, ',': { ocomma: function() { container[key] = value; state = 'okey'; }, acomma: function() { container.push(value); state = 'avalue'; } }, 'true': { go: function() { value = true; state = 'ok'; }, ovalue: function() { value = true; state = 'ocomma'; }, firstavalue: function() { value = true; state = 'acomma'; }, avalue: function() { value = true; state = 'acomma'; } }, 'false': { go: function() { value = false; state = 'ok'; }, ovalue: function() { value = false; state = 'ocomma'; }, firstavalue: function() { value = false; state = 'acomma'; }, avalue: function() { value = false; state = 'acomma'; } }, 'null': { go: function() { value = null; state = 'ok'; }, ovalue: function() { value = null; state = 'ocomma'; }, firstavalue: function() { value = null; state = 'acomma'; }, avalue: function() { value = null; state = 'acomma'; } }
            }; function debackslashify(text) { return text.replace(/\\(?:u(.{4})|([^u]))/g, function(a, b, c) { return b ? String.fromCharCode(parseInt(b, 16)) : escapes[c]; }); }
            return function(source, reviver) {
                var r, tx = /^[\x20\t\n\r]*(?:([,:\[\]{}]|true|false|null)|(-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)|"((?:[^\r\n\t\\\"]|\\(?:["\\\/trnfb]|u[0-9a-fA-F]{4}))*)")/; state = 'go'; stack = []; try {
                    for (; ; ) {
                        r = tx.exec(source); if (!r) { break; }
                        if (r[1]) { action[r[1]][state](); } else if (r[2]) { value = +r[2]; number[state](); } else { value = debackslashify(r[3]); string[state](); }
                        source = source.slice(r[0].length);
                    } 
                } catch (e) { state = e; }
                if (state !== 'ok' || /[^\x20\t\n\r]/.test(source)) { throw state instanceof SyntaxError ? state : new SyntaxError('JSON'); }
                return typeof reviver === 'function' ? (function walk(holder, key) {
                    var k, v, value = holder[key]; if (value && typeof value === 'object') { for (k in value) { if (Object.hasOwnProperty.call(value, k)) { v = walk(value, k); if (v !== undefined) { value[k] = v; } else { delete value[k]; } } } }
                    return reviver.call(holder, key, value);
                } ({ '': value }, '')) : value;
            };
        };
    }
} ());



/*
* Lazy Load - jQuery plugin for lazy loading images
*
* Copyright (c) 2007-2009 Mika Tuupola
*
* Licensed under the MIT license:
*   https://www.opensource.org/licenses/mit-license.php
*
* Project home:
*   https://www.appelsiini.net/projects/lazyload
*
* Version:  1.4.0
*
*/
(function($) {

    $.fn.lazyload = function(options) {
        var settings = {
            threshold: 0,
            failurelimit: 0,
            event: "scroll",
            effect: "show",
            container: window
        };

        if (options) {
            $.extend(settings, options);
        }

        /* Fire one scroll event per scroll. Not one scroll event per image. */
        var elements = this;
        if ("scroll" == settings.event) {
            $(settings.container).bind("scroll", function(event) {
                var counter = 0;
                elements.each(function() {
                    if (!$.belowthefold(this, settings) &&
                        !$.rightoffold(this, settings)) {
                        $(this).trigger("appear");
                    } else {
                        if (counter++ > settings.failurelimit) {
                            return false;
                        }
                    }
                });
                /* Remove image from array so it is not looped next time. */
                var temp = $.grep(elements, function(element) {
                    return !element.loaded;
                });
                elements = $(temp);
            });
        }

        return this.each(function() {
            var self = this;
            /* TODO: use .data() instead of .attr() */
            $(self).attr("original", $(self).attr("src"));
            if ("scroll" != settings.event
                         || $.belowthefold(self, settings)
                         || $.rightoffold(self, settings)) {
                if (settings.placeholder) {
                    $(self).attr("src", settings.placeholder);
                } else {
                    $(self).removeAttr("src");
                }
                self.loaded = false;
            } else {
                self.loaded = true;
            }

            /* When appear is triggered load original image. */
            $(self).one("appear", function() {
                if (!this.loaded) {
                    $("<img />")
                        .bind("load", function() {
                            $(self)
                                .hide()
                                .attr("src", $(self).attr("original"))
                                [settings.effect](settings.effectspeed);
                            self.loaded = true;
                        })
                        .attr("src", $(self).attr("original"));
                };
            });

            /* When wanted event is triggered load original image */
            /* by triggering appear.                              */
            if ("scroll" != settings.event) {
                $(self).bind(settings.event, function(event) {
                    if (!self.loaded) {
                        $(self).trigger("appear");
                    }
                });
            }
        });

    };

    /* Convenience methods in jQuery namespace.           */
    /* Use as  $.belowthefold(element, {threshold : 100, container : window}) */

    $.belowthefold = function(element, settings) {
        if (settings.container === undefined || settings.container === window) {
            var fold = $(window).height() + $(window).scrollTop();
        }
        else {
            var fold = $(settings.container).offset().top + $(settings.container).height();
        }
        return fold <= $(element).offset().top - settings.threshold;
    };

    $.rightoffold = function(element, settings) {
        if (settings.container === undefined || settings.container === window) {
            var fold = $(window).width() + $(window).scrollLeft();
        }
        else {
            var fold = $(settings.container).offset().left + $(settings.container).width();
        }
        return fold <= $(element).offset().left - settings.threshold;
    };

    /* Custom selectors for your convenience.   */
    /* Use as $("img:below-the-fold").something() */

    $.extend($.expr[':'], {
        "below-the-fold": "$.belowthefold(a, {threshold : 0, container: window})",
        "above-the-fold": "!$.belowthefold(a, {threshold : 0, container: window})",
        "right-of-fold": "$.rightoffold(a, {threshold : 0, container: window})",
        "left-of-fold": "!$.rightoffold(a, {threshold : 0, container: window})"
    });

})(jQuery);


(function($) {
    $.fn.tipsy = function(options) {

        options = $.extend({}, $.fn.tipsy.defaults, options);

        return this.each(function() {

            var opts = $.fn.tipsy.elementOptions(this, options);

            $(this).hover(function() {

                if ($(this).attr('title') || typeof ($(this).attr('original-title')) != 'string') {
                    $(this).attr('original-title', $(this).attr('title') || '').removeAttr('title');
                }
                if ($(this).attr('original-title').length > 2) { // ensure not an empty string

                    $.data(this, 'cancel.tipsy', true);

                    var tip = $.data(this, 'active.tipsy');
                    if (!tip) {
                        tip = $('<div class="tipsy"><div class="tipsy-inner"/></div>');
                        tip.css({ position: 'absolute' });
                        tip.css("z-index", "100000");
                        $.data(this, 'active.tipsy', tip);
                    }

                    var title;
                    if (typeof opts.title == 'string') {
                        title = $(this).attr(opts.title == 'title' ? 'original-title' : opts.title);
                    } else if (typeof opts.title == 'function') {
                        title = opts.title.call(this);
                    }

                    tip.find('.tipsy-inner')[opts.html ? 'html' : 'text'](title || opts.fallback);

                    var pos = $.extend({}, $(this).offset(), { width: this.offsetWidth, height: this.offsetHeight });
                    tip.get(0).className = 'tipsy'; // reset classname in case of dynamic gravity
                    tip.remove().css({ top: 0, left: 0, visibility: 'hidden', display: 'block' }).appendTo(document.body);
                    var actualWidth = tip[0].offsetWidth, actualHeight = tip[0].offsetHeight;
                    var gravity = (typeof opts.gravity == 'function') ? opts.gravity.call(this) : opts.gravity;

                    switch (gravity.charAt(0)) {
                        case 'n':
                            tip.css({ top: pos.top + pos.height, left: pos.left + pos.width / 2 - actualWidth / 2 }).addClass('tipsy-north');
                            break;
                        case 's':
                            tip.css({ top: pos.top - actualHeight, left: pos.left + pos.width / 2 - actualWidth / 2 }).addClass('tipsy-south');
                            break;
                        case 'e':
                            tip.css({ top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left - actualWidth }).addClass('tipsy-east');
                            break;
                        case 'w':
                            tip.css({ top: pos.top + pos.height / 2 - actualHeight / 2, left: pos.left + pos.width }).addClass('tipsy-west');
                            break;
                    }

                    if (opts.fade) {
                        tip.css({ opacity: 0, display: 'block', visibility: 'visible' }).animate({ opacity: 0.8 });
                    } else {
                        tip.css({ visibility: 'visible' });
                    }
                }

            }, function() {
                $.data(this, 'cancel.tipsy', false);
                var self = this;
                setTimeout(function() {
                    if ($.data(self, 'cancel.tipsy')) return;
                    var tip = $.data(self, 'active.tipsy');
                    if (opts.fade) {
                        tip.stop().fadeOut(function() { $(this).remove(); });
                    } else {
                        tip.remove();
                    }
                }, 0);

            });

        });

    };

    // Overwrite this method to provide options on a per-element basis.
    // For example, you could store the gravity in a 'tipsy-gravity' attribute:
    // return $.extend({}, options, {gravity: $(ele).attr('tipsy-gravity') || 'n' });
    // (remember - do not modify 'options' in place!)
    $.fn.tipsy.elementOptions = function(ele, options) {
        return $.metadata ? $.extend({}, options, $(ele).metadata()) : options;
    };

    $.fn.tipsy.defaults = {
        fade: false,
        fallback: '',
        gravity: 'n',
        html: false,
        title: 'title'
    };

    $.fn.tipsy.autoNS = function() {
        return $(this).offset().top > ($(document).scrollTop() + $(window).height() / 2) ? 's' : 'n';
    };

    $.fn.tipsy.autoWE = function() {
        return $(this).offset().left > ($(document).scrollLeft() + $(window).width() / 2) ? 'e' : 'w';
    };

})(jQuery);


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

$(function() {
    //if ($.browser.msie) 
    {
        $('div, table, ul, li').each(function(i) {
            if ($(this).css('position') != 'absolute')
            //&& $(this).css('z-index') == 0) 
            {
                $(this).css('z-index', 999 - i);
            }
            //            var minWidth = $(this).css('min-width').replace('px', '');
            //            if (minWidth > 0) {
            //                $(this).css('width','expression(' + $(this).clientWidth < minWidth ? minWidth + "px !important" : "auto" + ')');
            //            }
            //            var minHeight = $(this).css('min-height').replace('px', '');
            //            if (minHeight > 0) {
            //                $(this).css('height', 'expression(' + $(this).clientHeight < minHeight ? minHeight + "px !important" : "auto" + ')');
            //            }
        });
    }
});

/*
* jQuery Popeye 2.1 - https://dev.herr-schuessler.de/jquery/popeye/
*
* converts a HTML image list in image gallery with inline enlargement
*
* Copyright (C) 2008 - 2011 Christoph Schuessler (schreib@herr-schuessler.de)
* 
* This program is free software; you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation; either version 2 of the License, or
* (at your option) any later version.
*
*/

(function($) {


    ////////////////////////////////////////////////////////////////////////////
    //
    // $.fn.popeye
    // popeye definition
    //
    ////////////////////////////////////////////////////////////////////////////
    $.fn.popeye = function(options) {

        // build main options before element iteration
        //----------------------------------------------------------------------
        var opts = $.extend({}, $.fn.popeye.defaults, options);

        ////////////////////////////////////////////////////////////////////////////////
        //
        // firebug console output
        // @param text String the debug message
        // @param type String the message type [info | warn] (optional)
        //
        ////////////////////////////////////////////////////////////////////////////////
        function debug(text, type) {
            if (window.console && window.console.log && opts.debug) {
                if (type == 'info' && window.console.info) {
                    window.console.info(text);
                }
                else if (type == 'warn' && window.console.warn) {
                    window.console.warn(text);
                }
                else {
                    window.console.log(text);
                }
            }
        }

        // let's go!
        //----------------------------------------------------------------------
        return this.each(function() {

            // first thing to do: make ppy html visible           
            $(this).addClass('ppy-active');

            // cache object
            var $self = $(this),

            // images
            img = $self.find('.ppy-imglist > li > a > img'),
            a = $self.find('.ppy-imglist > li > a'),
            tot = img.length,

            // single image mode
            singleImageMode = (tot == 1) ? true : false,

            // flag for moueover check
            ismouseover = false,

            // start in compact mode
            enlarged = false,

            // counter vars
            cur = 0,                // array index of currently displayed image

            // extra classes
            eclass = 'ppy-expanded',       //class to be applied to enlarged popeye-box
            lclass = 'ppy-loading',        //class to be applied to stage while loading image
            sclass = 'ppy-single-image',   //class to be applied to popeye-box if there's only one image to display

            // html nodes
            ppyPlaceholder = $('<div class="ppy-placeholder"></div>'),
            ppyStageWrap = $('<div class="ppy-stagewrap"></div>'),
            ppyCaptionWrap = $('<div class="ppy-captionwrap"></div>'),
            ppyOuter = $self.find('.ppy-outer'),
            ppyStage = $self.find('.ppy-stage'),
            ppyNav = $self.find('.ppy-nav'),
            ppyPrev = $self.find('.ppy-prev'),
            ppyNext = $self.find('.ppy-next'),
            ppySwitchEnlarge = $self.find('.ppy-switch-enlarge'),
            ppySwitchCompact = $self.find('.ppy-switch-compact').addClass('ppy-hidden'),
            ppyPlay = $self.find('.ppy-play'),
            ppyPause = $self.find('.ppy-pause').addClass('ppy-hidden'),
            ppyCaption = $self.find('.ppy-caption'),
            ppyText = $self.find('.ppy-text'),
            ppyCounter = $self.find('.ppy-counter'),
            ppyCurrent = $self.find('.ppy-current'),
            ppyTotal = $self.find('.ppy-total'),

            // css objects
            cssSelf = {
                position: 'absolute',
                width: 'auto',
                height: 'auto',
                margin: 0,
                top: 0,
                left: (opts.direction == 'right') ? 0 : 'auto',
                right: (opts.direction == 'left') ? 0 : 'auto'
            },
            cssStage = {
                height: ppyStage.height(),
                width: ppyStage.width()
            },
            cssCaption = {
                height: ppyCaption.height()
            },
            cssPlaceholder = {
                'height': (opts.caption == 'hover' || false) ? ppyOuter.outerHeight() : $self.outerHeight(),
                'width': (opts.caption == 'hover' || false) ? ppyOuter.outerWidth() : $self.outerWidth(),
                'float': $("body").attr('dir') == "ltr" ? 'left' : 'right', //$self.css('float'),
                'margin-top': $self.css('margin-top'),
                'margin-right': $self.css('margin-right'),
                'margin-bottom': $self.css('margin-bottom'),
                'margin-left': $self.css('margin-left')
            };

            // make caption array from caption element or alt tag           
            var cap = [];
            for (var i = 0; i < img.length; i++) {
                var extcap = $self.find('.ppy-imglist li').eq(i).find('.ppy-extcaption');
                cap[i] = extcap.length > 0 ? extcap.html() : img[i].alt;
            }

            // check for html errors
            if (!ppyStage.length || !ppyNav.length || !ppyOuter.length) {
                debug('$.fn.popeye: Incorrect HTML structure', 'warn');
            }

            // check for images
            else if (tot === 0) {
                debug('$.fn.popeye: No images found', 'warn');
            }
            // no errors, setup done! 
            //------------------------------------------------------------------
            else {
                singleImageMode ? debug('$.fn.popeye -> SingleImageMode started') : debug('$.fn.popeye -> ' + tot + ' thumbnails found.');
                init();
            }

            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.showThumb
            // show thumbnail
            // @param i Int the index of the thumbnail to show (optional)
            // @param transition Bool show transition between images (optional)
            //
            ////////////////////////////////////////////////////////////////////
            function showThumb(i, transition) {

                // optional parameters
                transition = transition || false;
                i = i || cur;

                // set selected thumb as background image of stage
                var cssStageImage = {
                    backgroundImage: 'url(' + img[i].src + ')'
                };
                // bogus animation css for IE 
                var cssTemp = {
                    height: '+=0'
                };

                // if we are in enlarged mode, return to thumb mode
                if (enlarged) {

                    hideCaption();

                    // fade image out and compact stage with transition
                    ppyStage.fadeTo((opts.duration / 2), 0).animate(cssStage, {
                        queue: false,
                        duration: opts.duration,
                        easing: opts.easing,
                        complete: function() {

                            enlarged = false;
                            debug('$.fn.showThumb: Entering COMPACT MODE', 'info');

                            // remove extra styling and reset z-index
                            $self.removeClass(eclass);
                            $self.css('z-index', '').parent().css('z-index', '');

                            // switch buttons
                            ppySwitchEnlarge.removeClass('ppy-hidden');
                            ppySwitchCompact.addClass('ppy-hidden');

                            // recursive function call
                            showThumb();

                            // fade the stage back in
                            $(this).fadeTo((opts.duration / 2), 1);
                        }
                    });
                }
                else {

                    // if we navigate from one image to the next, fade out the stage
                    if (transition) {

                        // fade out image so that background shines through
                        // background can contain loading gfx
                        ppyStageWrap.addClass(lclass);
                        ppyStage.fadeTo((opts.duration / 2), 0);

                        // once thumb has loadded...
                        var thumbPreloader = new Image();
                        thumbPreloader.onload = function() {
                            debug('$.fn.popeye.showThumb: Thumbnail ' + i + ' loaded', 'info');

                            // remove loading indicator
                            ppyStageWrap.removeClass(lclass);

                            // add all upcoming animations to the queue so that 
                            // they won't start when the preolader has loaded but when the fadeOut has finished
                            ppyStage.animate(cssTemp, 1, 'linear', function() {

                                // set the new image
                                ppyStage.css(cssStageImage);
                                // fade the stage back in
                                $(this).fadeTo((opts.duration / 2), 1);

                                // update counter and caption
                                if (opts.caption == 'hover' && ismouseover) {
                                    showCaption(cap[i]);
                                }
                                else if (opts.caption == 'permanent') {
                                    updateCaption(cap[i]);
                                }
                                updateCounter();
                            });

                            //  fix IE animated gif bug
                            thumbPreloader.onload = function() { };
                        };
                        // preload thumb
                        thumbPreloader.src = img[i].src;
                    }

                    // or just drag the image to the stage
                    else {
                        ppyStage.css(cssStageImage);
                        updateCounter();

                        //if(ismouseover) {
                        showCaption(cap[i], true);
                        //}
                    }

                    // preload big image for instant availability
                    var preloader = new Image();

                    preloader.onload = function() {
                        debug('$.fn.popeye.showThumb: Image ' + i + ' loaded', 'info');
                        preloader.onload = function() { };
                    };

                    preloader.src = a[i].href;
                }
            }


            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.showImage
            // show large image
            // @param i Int the index of the image to show (optional)
            //
            ////////////////////////////////////////////////////////////////////
            function showImage(i) {

                // optional parameter i
                i = i || cur;

                // fade out image so that background shines through
                // background can contain loading gfx
                ppyStageWrap.addClass(lclass);
                ppyStage.fadeTo((opts.duration / 2), 0);

                // if there are multiple popeyes opened at the same time,
                // make sure the current one gets a higher z-index
                var allPpy = $('.' + eclass);
                allPpy.css('z-index', opts.zindex - 1);

                //both ppy and placeholder (for IE7) need high z-index
                $self.css('z-index', opts.zindex).parent().css('z-index', opts.zindex);

                // once image has loadded...
                var preloader = new Image();
                preloader.onload = function() {

                    // remove loading class
                    ppyStageWrap.removeClass(lclass);

                    // set css
                    var cssStageTo = {
                        width: preloader.width,
                        height: preloader.height
                    };
                    var cssStageIm = {
                        backgroundImage: 'url(' + a[i].href + ')',
                        backgroundPosition: 'left top'
                    };

                    hideCaption();

                    // show transitional animation
                    ppyStage.animate(cssStageTo, {
                        queue: false,
                        duration: opts.duration,
                        easing: opts.easing,
                        complete: function() {

                            if (opts.navigation == 'hover' && ismouseover) {
                                showNav();
                            }

                            enlarged = true;
                            debug('$.fn.popeye.showImage: Entering ENLARGED MODE', 'info');

                            // add extra class, expanded box can be styled accordingly
                            $self.addClass(eclass);

                            // switch buttons
                            ppySwitchCompact.removeClass('ppy-hidden');
                            ppySwitchEnlarge.addClass('ppy-hidden');

                            updateCounter();

                            // set new bg image and fade it in
                            $(this).css(cssStageIm).fadeTo((opts.duration / 2), 1);

                            // show caption
                            showCaption(cap[i]);

                            preloadNeighbours();
                        }
                    });
                };

                // preload image
                preloader.src = a[i].href;

            }


            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.updateCounter
            // update image counter
            // @param i Int the index of the image (optional)
            //
            ////////////////////////////////////////////////////////////////////
            function updateCounter(i) {

                // optional parameter
                i = i || cur;

                ppyTotal.text(tot);        // total images
                ppyCurrent.text(i + 1);    // current image number
                debug('$.fn.popeye.updateCounter: Displaying image ' + (i + 1) + ' of ' + tot);
            }

            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.preloadNeighbours
            // preload next and previos image
            // @param i Int the index of the current image (optional)
            //
            ////////////////////////////////////////////////////////////////////
            function preloadNeighbours(i) {

                // optional parameter
                i = i || cur;

                var preloaderNext = new Image();
                var preloaderPrev = new Image();

                var neighbour = i;

                // next image
                if (neighbour < (tot - 1)) {
                    neighbour++;
                } else {
                    neighbour = 0;
                }
                preloaderNext.src = a[neighbour].href;

                // previous image
                neighbour = i;
                if (neighbour <= 0) {
                    neighbour = tot - 1;
                } else {
                    neighbour--;
                }
                preloaderPrev.src = a[neighbour].href;
            }


            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.showNav
            //
            ////////////////////////////////////////////////////////////////////
            function showNav() {
                ppyNav.stop().fadeTo(150, opts.opacity);
            }


            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.hideNav
            //
            ////////////////////////////////////////////////////////////////////
            function hideNav() {
                ppyNav.stop().fadeTo(150, 0);
            }


            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.updateCaption
            // @param caption String the caption string
            //
            ////////////////////////////////////////////////////////////////////
            function updateCaption(caption) {

                if (opts.caption) {
                    // update text box
                    ppyText.html(caption);
                }
            }

            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.showCaption
            // @param caption String the caption string
            // @param force Boolean force caption display even if caption string is empty
            //
            ////////////////////////////////////////////////////////////////////
            function showCaption(caption, force) {

                // if caption string is not empty...
                if (caption && opts.caption) {
                    updateCaption(caption);

                    debug('$.fn.popeye.showCaption -> ppyCaptionWrap.outerHeight(true): ' + ppyCaptionWrap.outerHeight(true));

                    // make caption box visible
                    var cssTempCaption = {
                        visibility: 'visible'
                    };
                    ppyCaption.css(cssTempCaption);

                    if (opts.caption === 'permanent' && !enlarged) {

                        // return to original caption height
                        ppyCaption.css(cssCaption);
                    }
                    else {

                        // or animate it to its childs height
                        ppyCaption.animate({ 'height': ppyCaptionWrap.outerHeight(true) }, {
                            queue: false,
                            duration: 90,
                            easing: opts.easing
                        });
                    }
                }
                // if there's no caption to show...
                else if (!caption && !force) {
                    hideCaption();
                }
            }


            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.hideCaption
            //
            ////////////////////////////////////////////////////////////////////
            function hideCaption() {

                // css to hide caption but allow its inner text box to expand to content height
                var cssTempCaption = {
                    visibility: 'hidden',
                    overflow: 'hidden'
                };

                // slide up caption box and hide it when done
                ppyCaption.animate({ 'height': '0px' }, {
                    queue: false,
                    duration: 90,
                    easing: opts.easing,
                    complete: function() {
                        ppyCaption.css(cssTempCaption);
                    }
                });
            }

            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.previous
            // show previous image
            //
            ////////////////////////////////////////////////////////////////////
            function previous() {
                if (cur <= 0) {
                    cur = tot - 1;
                } else {
                    cur--;
                }
                if (enlarged) {
                    showImage(cur);
                }
                else {
                    showThumb(cur, true);
                }
                return cur;
            }

            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.next
            // show next image
            //
            ////////////////////////////////////////////////////////////////////
            function next() {
                if (cur < (tot - 1)) {
                    cur++;
                } else {
                    cur = 0;
                }
                if (enlarged) {
                    showImage(cur);
                }
                else {
                    showThumb(cur, true);
                }
                return cur;
            }

            ////////////////////////////////////////////////////////////////////
            //
            // $.fn.popeye.init
            // setup of popeye DOM and events
            //
            ////////////////////////////////////////////////////////////////////
            function init() {

                // popeye dom setup
                //--------------------------------------------------------------

                // add css 
                ppyPlaceholder.css(cssPlaceholder);
                $self.css(cssSelf);

                // wrap popeye in placeholder 
                $self.wrap(ppyPlaceholder);

                // wrap stage in container for extra styling (e.g. loading gfx)
                ppyStageWrap = ppyStage.wrap(ppyStageWrap).parent();

                // wrap caption contents in wrapper (can't use wrap() here...)
                ppyCaptionWrap = ppyCaption.wrapInner(ppyCaptionWrap).children().eq(0);

                // display first image
                showThumb();

                // add event handlers
                //--------------------------------------------------------------
                // hover behaviour for navigation
                if (opts.navigation == 'hover') {
                    hideNav();
                    $self.hover(
                            function() {
                                showNav();
                            },
                            function() {
                                hideNav();
                            }
                        );
                    ppyNav.hover(
                            function() {
                                showNav();
                            },
                            function() {
                                hideNav();
                            }
                        );
                }
                if (!singleImageMode) {

                    // previous image button
                    ppyPrev.click(function(e) {
                        e.stopPropagation();
                        previous();
                    });

                    // next image button
                    ppyNext.click(function(e) {
                        e.stopPropagation();
                        next();
                    });

                }
                else {
                    $self.addClass(sclass);
                    ppyPrev.remove();
                    ppyNext.remove();
                    ppyPlay.remove();
                    ppyPause.remove();
                    ppyCounter.remove();
                }

                // hover behaviour for caption
                if (opts.caption == 'hover') {
                    hideCaption();
                    $self.hover(
                            function() {
                                showCaption(cap[cur]);
                            },
                            function() {
                                hideCaption(true);
                            }
                        );
                }

                // enlarge image button
                ppySwitchEnlarge.click(function(e) {
                    e.stopPropagation();
                    showImage();
                    return false;
                });

                // compact image button                          
                ppySwitchCompact.click(function(e) {
                    e.stopPropagation();
                    showThumb(cur);
                    return false;
                });

                ppyStage.click(function() {
                    if (enlarged) {
                        showThumb(cur);
                    }
                    else {
                        showImage();
                    }

                });

                //play button
                ppyPlay.click(function(e) {
                    e.stopPropagation();

                    // switch buttons
                    ppyPause.removeClass('ppy-hidden');
                    ppyPlay.addClass('ppy-hidden');

                    // initiate Slideshow
                    slideshow = window.setInterval(function() {
                        next();
                    }, opts.slidespeed);
                    return false;
                });

                //pause button
                ppyPause.click(function(e) {
                    e.stopPropagation();

                    // switch buttons
                    ppyPlay.removeClass('ppy-hidden');
                    ppyPause.addClass('ppy-hidden');

                    // stop Slideshow
                    window.clearInterval(slideshow);
                    return false;
                });

                // mouseover flag
                $self.mouseenter(function() {
                    ismouseover = true;
                }).mouseleave(function() {
                    ismouseover = false;
                });

                // start autoslide
                if (opts.autoslide) {
                    ppyPlay.trigger('click');
                }

            }
        });
    };

    ////////////////////////////////////////////////////////////////////////////
    //
    // $.fn.popeye.defaults
    // set default  options
    //
    ////////////////////////////////////////////////////////////////////////////
    $.fn.popeye.defaults = {

        navigation: 'hover',            //visibility of navigation - can be 'permanent' or 'hover'
        caption: 'hover',            //visibility of caption, based on image title - can be false, 'permanent' or 'hover'

        zindex: 10000,              //z-index of the expanded popeye-box. enter a z-index that works well with your site and doesn't overlay your site's navigational elements like dropdowns

        direction: $("body").attr('dir') == "ltr" ? 'right' : 'left',            //direction that popeye-box opens, can be 'left' or 'right'
        duration: 240,                //duration of transitional effect when enlarging or closing the box
        opacity: 0.8,                //opacity of navigational overlay (only applicable if 'navigation' is set to 'hover'
        easing: 'swing',            //easing type, can be 'swing', 'linear' or any of jQuery Easing Plugin types (Plugin required)

        slidespeed: 2000,               //timespan that one image is shown in a slideshow
        autoslide: false,              //should the slideshow start automatically?

        debug: false               //turn on console output (slows down IE8!)

    };

    // end of closure, bind to jQuery Object
})(jQuery);


////////////////////////////////////////////////////////////////////////////////
//
// avoid content flicker for non-js user agents
// (in order to use this, the js-files have to be included in the head of the
// html file!)
//
////////////////////////////////////////////////////////////////////////////////
jQuery('head').append('<style type="text/css"> .ppy-imglist { position: absolute; top: -1000em; left: -1000em; } </style>');


/**
* Jcrop v.0.9.8 (minimized)
* (c) 2008 Kelly Hallman and DeepLiquid.com
* More information: https://deepliquid.com/content/Jcrop.html
* Released under MIT License - this header must remain with code
*/


(function($) {
    $.Jcrop = function(obj, opt) {
        var obj = obj, opt = opt; if (typeof (obj) !== 'object') obj = $(obj)[0]; if (typeof (opt) !== 'object') opt = {}; if (!('trackDocument' in opt)) {
            opt.trackDocument = $.browser.msie ? false : true; if ($.browser.msie && $.browser.version.split('.')[0] == '8')
                opt.trackDocument = true;
        }
        if (!('keySupport' in opt))
            opt.keySupport = $.browser.msie ? false : true; var defaults = { trackDocument: false, baseClass: 'jcrop', addClass: null, bgColor: 'black', bgOpacity: .6, borderOpacity: .4, handleOpacity: .5, handlePad: 5, handleSize: 9, handleOffset: 5, edgeMargin: 14, aspectRatio: 0, keySupport: true, cornerHandles: true, sideHandles: true, drawBorders: true, dragEdges: true, boxWidth: 0, boxHeight: 0, boundary: 8, animationDelay: 20, swingSpeed: 3, allowSelect: true, allowMove: true, allowResize: true, minSelect: [0, 0], maxSize: [0, 0], minSize: [0, 0], onChange: function() { }, onSelect: function() { } }; var options = defaults; setOptions(opt); var $origimg = $(obj); var $img = $origimg.clone().removeAttr('id').css({ position: 'absolute' }); $img.width($origimg.width()); $img.height($origimg.height()); $origimg.after($img).hide(); presize($img, options.boxWidth, options.boxHeight); var boundx = $img.width(), boundy = $img.height(), $div = $('<div />').width(boundx).height(boundy).addClass(cssClass('holder')).css({ position: 'relative', backgroundColor: options.bgColor }).insertAfter($origimg).append($img); ; if (options.addClass) $div.addClass(options.addClass); var $img2 = $('<img />').attr('src', $img.attr('src')).css('position', 'absolute').width(boundx).height(boundy); var $img_holder = $('<div />').width(pct(100)).height(pct(100)).css({ zIndex: 310, position: 'absolute', overflow: 'hidden' }).append($img2); var $hdl_holder = $('<div />').width(pct(100)).height(pct(100)).css('zIndex', 320); var $sel = $('<div />').css({ position: 'absolute', zIndex: 300 }).insertBefore($img).append($img_holder, $hdl_holder); var bound = options.boundary; var $trk = newTracker().width(boundx + (bound * 2)).height(boundy + (bound * 2)).css({ position: 'absolute', top: px(-bound), left: px(-bound), zIndex: 290 }).mousedown(newSelection); var xlimit, ylimit, xmin, ymin; var xscale, yscale, enabled = true; var docOffset = getPos($img), btndown, lastcurs, dimmed, animating, shift_down; var Coords = function() {
                var x1 = 0, y1 = 0, x2 = 0, y2 = 0, ox, oy; function setPressed(pos)
                { var pos = rebound(pos); x2 = x1 = pos[0]; y2 = y1 = pos[1]; }; function setCurrent(pos)
                { var pos = rebound(pos); ox = pos[0] - x2; oy = pos[1] - y2; x2 = pos[0]; y2 = pos[1]; }; function getOffset()
                { return [ox, oy]; }; function moveOffset(offset)
                { var ox = offset[0], oy = offset[1]; if (0 > x1 + ox) ox -= ox + x1; if (0 > y1 + oy) oy -= oy + y1; if (boundy < y2 + oy) oy += boundy - (y2 + oy); if (boundx < x2 + ox) ox += boundx - (x2 + ox); x1 += ox; x2 += ox; y1 += oy; y2 += oy; }; function getCorner(ord) {
                    var c = getFixed(); switch (ord)
                    { case 'ne': return [c.x2, c.y]; case 'nw': return [c.x, c.y]; case 'se': return [c.x2, c.y2]; case 'sw': return [c.x, c.y2]; } 
                }; function getFixed() {
                    if (!options.aspectRatio) return getRect(); var aspect = options.aspectRatio, min_x = options.minSize[0] / xscale, min_y = options.minSize[1] / yscale, max_x = options.maxSize[0] / xscale, max_y = options.maxSize[1] / yscale, rw = x2 - x1, rh = y2 - y1, rwa = Math.abs(rw), rha = Math.abs(rh), real_ratio = rwa / rha, xx, yy; if (max_x == 0) { max_x = boundx * 10 }
                    if (max_y == 0) { max_y = boundy * 10 }
                    if (real_ratio < aspect) {
                        yy = y2; w = rha * aspect; xx = rw < 0 ? x1 - w : w + x1; if (xx < 0)
                        { xx = 0; h = Math.abs((xx - x1) / aspect); yy = rh < 0 ? y1 - h : h + y1; }
                        else if (xx > boundx)
                        { xx = boundx; h = Math.abs((xx - x1) / aspect); yy = rh < 0 ? y1 - h : h + y1; } 
                    }
                    else {
                        xx = x2; h = rwa / aspect; yy = rh < 0 ? y1 - h : y1 + h; if (yy < 0)
                        { yy = 0; w = Math.abs((yy - y1) * aspect); xx = rw < 0 ? x1 - w : w + x1; }
                        else if (yy > boundy)
                        { yy = boundy; w = Math.abs(yy - y1) * aspect; xx = rw < 0 ? x1 - w : w + x1; } 
                    }
                    if (xx > x1) {
                        if (xx - x1 < min_x) { xx = x1 + min_x; } else if (xx - x1 > max_x) { xx = x1 + max_x; }
                        if (yy > y1) { yy = y1 + (xx - x1) / aspect; } else { yy = y1 - (xx - x1) / aspect; } 
                    } else if (xx < x1) {
                        if (x1 - xx < min_x) { xx = x1 - min_x } else if (x1 - xx > max_x) { xx = x1 - max_x; }
                        if (yy > y1) { yy = y1 + (x1 - xx) / aspect; } else { yy = y1 - (x1 - xx) / aspect; } 
                    }
                    if (xx < 0) { x1 -= xx; xx = 0; } else if (xx > boundx) { x1 -= xx - boundx; xx = boundx; }
                    if (yy < 0) { y1 -= yy; yy = 0; } else if (yy > boundy) { y1 -= yy - boundy; yy = boundy; }
                    return last = makeObj(flipCoords(x1, y1, xx, yy));
                }; function rebound(p)
                { if (p[0] < 0) p[0] = 0; if (p[1] < 0) p[1] = 0; if (p[0] > boundx) p[0] = boundx; if (p[1] > boundy) p[1] = boundy; return [p[0], p[1]]; }; function flipCoords(x1, y1, x2, y2) {
                    var xa = x1, xb = x2, ya = y1, yb = y2; if (x2 < x1)
                    { xa = x2; xb = x1; }
                    if (y2 < y1)
                    { ya = y2; yb = y1; }
                    return [Math.round(xa), Math.round(ya), Math.round(xb), Math.round(yb)];
                }; function getRect() {
                    var xsize = x2 - x1; var ysize = y2 - y1; if (xlimit && (Math.abs(xsize) > xlimit))
                        x2 = (xsize > 0) ? (x1 + xlimit) : (x1 - xlimit); if (ylimit && (Math.abs(ysize) > ylimit))
                        y2 = (ysize > 0) ? (y1 + ylimit) : (y1 - ylimit); if (ymin && (Math.abs(ysize) < ymin))
                        y2 = (ysize > 0) ? (y1 + ymin) : (y1 - ymin); if (xmin && (Math.abs(xsize) < xmin))
                        x2 = (xsize > 0) ? (x1 + xmin) : (x1 - xmin); if (x1 < 0) { x2 -= x1; x1 -= x1; }
                    if (y1 < 0) { y2 -= y1; y1 -= y1; }
                    if (x2 < 0) { x1 -= x2; x2 -= x2; }
                    if (y2 < 0) { y1 -= y2; y2 -= y2; }
                    if (x2 > boundx) { var delta = x2 - boundx; x1 -= delta; x2 -= delta; }
                    if (y2 > boundy) { var delta = y2 - boundy; y1 -= delta; y2 -= delta; }
                    if (x1 > boundx) { var delta = x1 - boundy; y2 -= delta; y1 -= delta; }
                    if (y1 > boundy) { var delta = y1 - boundy; y2 -= delta; y1 -= delta; }
                    return makeObj(flipCoords(x1, y1, x2, y2));
                }; function makeObj(a)
                { return { x: a[0], y: a[1], x2: a[2], y2: a[3], w: a[2] - a[0], h: a[3] - a[1] }; }; return { flipCoords: flipCoords, setPressed: setPressed, setCurrent: setCurrent, getOffset: getOffset, moveOffset: moveOffset, getCorner: getCorner, getFixed: getFixed };
            } (); var Selection = function() {
                var start, end, dragmode, awake, hdep = 370; var borders = {}; var handle = {}; var seehandles = false; var hhs = options.handleOffset; if (options.drawBorders) { borders = { top: insertBorder('hline').css('top', $.browser.msie ? px(-1) : px(0)), bottom: insertBorder('hline'), left: insertBorder('vline'), right: insertBorder('vline') }; }
                if (options.dragEdges) { handle.t = insertDragbar('n'); handle.b = insertDragbar('s'); handle.r = insertDragbar('e'); handle.l = insertDragbar('w'); }
                options.sideHandles && createHandles(['n', 's', 'e', 'w']); options.cornerHandles && createHandles(['sw', 'nw', 'ne', 'se']); function insertBorder(type)
                { var jq = $('<div />').css({ position: 'absolute', opacity: options.borderOpacity }).addClass(cssClass(type)); $img_holder.append(jq); return jq; }; function dragDiv(ord, zi)
                { var jq = $('<div />').mousedown(createDragger(ord)).css({ cursor: ord + '-resize', position: 'absolute', zIndex: zi }); $hdl_holder.append(jq); return jq; }; function insertHandle(ord)
                { return dragDiv(ord, hdep++).css({ top: px(-hhs + 1), left: px(-hhs + 1), opacity: options.handleOpacity }).addClass(cssClass('handle')); }; function insertDragbar(ord) {
                    var s = options.handleSize, o = hhs, h = s, w = s, t = o, l = o; switch (ord)
                    { case 'n': case 's': w = pct(100); break; case 'e': case 'w': h = pct(100); break; }
                    return dragDiv(ord, hdep++).width(w).height(h).css({ top: px(-t + 1), left: px(-l + 1) });
                }; function createHandles(li)
                { for (i in li) handle[li[i]] = insertHandle(li[i]); }; function moveHandles(c)
                { var midvert = Math.round((c.h / 2) - hhs), midhoriz = Math.round((c.w / 2) - hhs), north = west = -hhs + 1, east = c.w - hhs, south = c.h - hhs, x, y; 'e' in handle && handle.e.css({ top: px(midvert), left: px(east) }) && handle.w.css({ top: px(midvert) }) && handle.s.css({ top: px(south), left: px(midhoriz) }) && handle.n.css({ left: px(midhoriz) }); 'ne' in handle && handle.ne.css({ left: px(east) }) && handle.se.css({ top: px(south), left: px(east) }) && handle.sw.css({ top: px(south) }); 'b' in handle && handle.b.css({ top: px(south) }) && handle.r.css({ left: px(east) }); }; function moveto(x, y)
                { $img2.css({ top: px(-y), left: px(-x) }); $sel.css({ top: px(y), left: px(x) }); }; function resize(w, h)
                { $sel.width(w).height(h); }; function refresh()
                { var c = Coords.getFixed(); Coords.setPressed([c.x, c.y]); Coords.setCurrent([c.x2, c.y2]); updateVisible(); }; function updateVisible()
                { if (awake) return update(); }; function update()
                { var c = Coords.getFixed(); resize(c.w, c.h); moveto(c.x, c.y); options.drawBorders && borders['right'].css({ left: px(c.w - 1) }) && borders['bottom'].css({ top: px(c.h - 1) }); seehandles && moveHandles(c); awake || show(); options.onChange(unscale(c)); }; function show()
                { $sel.show(); $img.css('opacity', options.bgOpacity); awake = true; }; function release()
                { disableHandles(); $sel.hide(); $img.css('opacity', 1); awake = false; }; function showHandles() {
                    if (seehandles)
                    { moveHandles(Coords.getFixed()); $hdl_holder.show(); } 
                }; function enableHandles() {
                    seehandles = true; if (options.allowResize)
                    { moveHandles(Coords.getFixed()); $hdl_holder.show(); return true; } 
                }; function disableHandles()
                { seehandles = false; $hdl_holder.hide(); }; function animMode(v)
                { (animating = v) ? disableHandles() : enableHandles(); }; function done()
                { animMode(false); refresh(); }; var $track = newTracker().mousedown(createDragger('move')).css({ cursor: 'move', position: 'absolute', zIndex: 360 })
                $img_holder.append($track); disableHandles(); return { updateVisible: updateVisible, update: update, release: release, refresh: refresh, setCursor: function(cursor) { $track.css('cursor', cursor); }, enableHandles: enableHandles, enableOnly: function() { seehandles = true; }, showHandles: showHandles, disableHandles: disableHandles, animMode: animMode, done: done };
            } (); var Tracker = function() {
                var onMove = function() { }, onDone = function() { }, trackDoc = options.trackDocument; if (!trackDoc)
                { $trk.mousemove(trackMove).mouseup(trackUp).mouseout(trackUp); }
                function toFront() {
                    $trk.css({ zIndex: 450 }); if (trackDoc)
                    { $(document).mousemove(trackMove).mouseup(trackUp); } 
                }
                function toBack() {
                    $trk.css({ zIndex: 290 }); if (trackDoc)
                    { $(document).unbind('mousemove', trackMove).unbind('mouseup', trackUp); } 
                }
                function trackMove(e)
                { onMove(mouseAbs(e)); }; function trackUp(e) {
                    e.preventDefault(); e.stopPropagation(); if (btndown)
                    { btndown = false; onDone(mouseAbs(e)); options.onSelect(unscale(Coords.getFixed())); toBack(); onMove = function() { }; onDone = function() { }; }
                    return false;
                }; function activateHandlers(move, done)
                { btndown = true; onMove = move; onDone = done; toFront(); return false; }; function setCursor(t) { $trk.css('cursor', t); }; $img.before($trk); return { activateHandlers: activateHandlers, setCursor: setCursor };
            } (); var KeyManager = function() {
                var $keymgr = $('<input type="radio" />').css({ position: 'absolute', left: '-30px' }).keypress(parseKey).blur(onBlur), $keywrap = $('<div />').css({ position: 'absolute', overflow: 'hidden' }).append($keymgr); function watchKeys() {
                    if (options.keySupport)
                    { $keymgr.show(); $keymgr.focus(); } 
                }; function onBlur(e)
                { $keymgr.hide(); }; function doNudge(e, x, y)
                { if (options.allowMove) { Coords.moveOffset([x, y]); Selection.updateVisible(); }; e.preventDefault(); e.stopPropagation(); }; function parseKey(e) {
                    if (e.ctrlKey) return true; shift_down = e.shiftKey ? true : false; var nudge = shift_down ? 10 : 1; switch (e.keyCode)
                    { case 37: doNudge(e, -nudge, 0); break; case 39: doNudge(e, nudge, 0); break; case 38: doNudge(e, 0, -nudge); break; case 40: doNudge(e, 0, nudge); break; case 27: Selection.release(); break; case 9: return true; }
                    return nothing(e);
                }; if (options.keySupport) $keywrap.insertBefore($img); return { watchKeys: watchKeys };
            } (); function px(n) { return '' + parseInt(n) + 'px'; }; function pct(n) { return '' + parseInt(n) + '%'; }; function cssClass(cl) { return options.baseClass + '-' + cl; }; function getPos(obj)
            { var pos = $(obj).offset(); return [pos.left, pos.top]; }; function mouseAbs(e)
            { return [(e.pageX - docOffset[0]), (e.pageY - docOffset[1])]; }; function myCursor(type) {
                if (type != lastcurs)
                { Tracker.setCursor(type); lastcurs = type; } 
            }; function startDragMode(mode, pos) {
                docOffset = getPos($img); Tracker.setCursor(mode == 'move' ? mode : mode + '-resize'); if (mode == 'move')
                    return Tracker.activateHandlers(createMover(pos), doneSelect); var fc = Coords.getFixed(); var opp = oppLockCorner(mode); var opc = Coords.getCorner(oppLockCorner(opp)); Coords.setPressed(Coords.getCorner(opp)); Coords.setCurrent(opc); Tracker.activateHandlers(dragmodeHandler(mode, fc), doneSelect);
            }; function dragmodeHandler(mode, f) {
                return function(pos) {
                    if (!options.aspectRatio) switch (mode)
                    { case 'e': pos[1] = f.y2; break; case 'w': pos[1] = f.y2; break; case 'n': pos[0] = f.x2; break; case 's': pos[0] = f.x2; break; }
                    else switch (mode)
                    { case 'e': pos[1] = f.y + 1; break; case 'w': pos[1] = f.y + 1; break; case 'n': pos[0] = f.x + 1; break; case 's': pos[0] = f.x + 1; break; }
                    Coords.setCurrent(pos); Selection.update();
                };
            }; function createMover(pos) {
                var lloc = pos; KeyManager.watchKeys(); return function(pos)
                { Coords.moveOffset([pos[0] - lloc[0], pos[1] - lloc[1]]); lloc = pos; Selection.update(); };
            }; function oppLockCorner(ord) {
                switch (ord)
                { case 'n': return 'sw'; case 's': return 'nw'; case 'e': return 'nw'; case 'w': return 'ne'; case 'ne': return 'sw'; case 'nw': return 'se'; case 'se': return 'nw'; case 'sw': return 'ne'; };
            }; function createDragger(ord)
            { return function(e) { if (options.disabled) return false; if ((ord == 'move') && !options.allowMove) return false; btndown = true; startDragMode(ord, mouseAbs(e)); e.stopPropagation(); e.preventDefault(); return false; }; }; function presize($obj, w, h) {
                var nw = $obj.width(), nh = $obj.height(); if ((nw > w) && w > 0)
                { nw = w; nh = (w / $obj.width()) * $obj.height(); }
                if ((nh > h) && h > 0)
                { nh = h; nw = (h / $obj.height()) * $obj.width(); }
                xscale = $obj.width() / nw; yscale = $obj.height() / nh; $obj.width(nw).height(nh);
            }; function unscale(c)
            { return { x: parseInt(c.x * xscale), y: parseInt(c.y * yscale), x2: parseInt(c.x2 * xscale), y2: parseInt(c.y2 * yscale), w: parseInt(c.w * xscale), h: parseInt(c.h * yscale) }; }; function doneSelect(pos) {
                var c = Coords.getFixed(); if (c.w > options.minSelect[0] && c.h > options.minSelect[1])
                { Selection.enableHandles(); Selection.done(); }
                else
                { Selection.release(); }
                Tracker.setCursor(options.allowSelect ? 'crosshair' : 'default');
            }; function newSelection(e)
            { if (options.disabled) return false; if (!options.allowSelect) return false; btndown = true; docOffset = getPos($img); Selection.disableHandles(); myCursor('crosshair'); var pos = mouseAbs(e); Coords.setPressed(pos); Tracker.activateHandlers(selectDrag, doneSelect); KeyManager.watchKeys(); Selection.update(); e.stopPropagation(); e.preventDefault(); return false; }; function selectDrag(pos)
            { Coords.setCurrent(pos); Selection.update(); }; function newTracker()
            { var trk = $('<div></div>').addClass(cssClass('tracker')); $.browser.msie && trk.css({ opacity: 0, backgroundColor: 'white' }); return trk; }; function animateTo(a) {
                var x1 = a[0] / xscale, y1 = a[1] / yscale, x2 = a[2] / xscale, y2 = a[3] / yscale; if (animating) return; var animto = Coords.flipCoords(x1, y1, x2, y2); var c = Coords.getFixed(); var animat = initcr = [c.x, c.y, c.x2, c.y2]; var interv = options.animationDelay; var x = animat[0]; var y = animat[1]; var x2 = animat[2]; var y2 = animat[3]; var ix1 = animto[0] - initcr[0]; var iy1 = animto[1] - initcr[1]; var ix2 = animto[2] - initcr[2]; var iy2 = animto[3] - initcr[3]; var pcent = 0; var velocity = options.swingSpeed; Selection.animMode(true); var animator = function() {
                    return function()
                    { pcent += (100 - pcent) / velocity; animat[0] = x + ((pcent / 100) * ix1); animat[1] = y + ((pcent / 100) * iy1); animat[2] = x2 + ((pcent / 100) * ix2); animat[3] = y2 + ((pcent / 100) * iy2); if (pcent < 100) animateStart(); else Selection.done(); if (pcent >= 99.8) pcent = 100; setSelectRaw(animat); };
                } (); function animateStart()
                { window.setTimeout(animator, interv); }; animateStart();
            }; function setSelect(rect)
            { setSelectRaw([rect[0] / xscale, rect[1] / yscale, rect[2] / xscale, rect[3] / yscale]); }; function setSelectRaw(l)
            { Coords.setPressed([l[0], l[1]]); Coords.setCurrent([l[2], l[3]]); Selection.update(); }; function setOptions(opt) {
                if (typeof (opt) != 'object') opt = {}; options = $.extend(options, opt); if (typeof (options.onChange) !== 'function')
                    options.onChange = function() { }; if (typeof (options.onSelect) !== 'function')
                    options.onSelect = function() { };
            }; function tellSelect()
            { return unscale(Coords.getFixed()); }; function tellScaled()
            { return Coords.getFixed(); }; function setOptionsNew(opt)
            { setOptions(opt); interfaceUpdate(); }; function disableCrop()
            { options.disabled = true; Selection.disableHandles(); Selection.setCursor('default'); Tracker.setCursor('default'); }; function enableCrop()
            { options.disabled = false; interfaceUpdate(); }; function cancelCrop()
            { Selection.done(); Tracker.activateHandlers(null, null); }; function destroy()
            { $div.remove(); $origimg.show(); }; function interfaceUpdate(alt) {
                options.allowResize ? alt ? Selection.enableOnly() : Selection.enableHandles() : Selection.disableHandles(); Tracker.setCursor(options.allowSelect ? 'crosshair' : 'default'); Selection.setCursor(options.allowMove ? 'move' : 'default'); $div.css('backgroundColor', options.bgColor); if ('setSelect' in options) { setSelect(opt.setSelect); Selection.done(); delete (options.setSelect); }
                if ('trueSize' in options) { xscale = options.trueSize[0] / boundx; yscale = options.trueSize[1] / boundy; }
                xlimit = options.maxSize[0] || 0; ylimit = options.maxSize[1] || 0; xmin = options.minSize[0] || 0; ymin = options.minSize[1] || 0; if ('outerImage' in options)
                { $img.attr('src', options.outerImage); delete (options.outerImage); }
                Selection.refresh();
            }; $hdl_holder.hide(); interfaceUpdate(true); var api = { animateTo: animateTo, setSelect: setSelect, setOptions: setOptionsNew, tellSelect: tellSelect, tellScaled: tellScaled, disable: disableCrop, enable: enableCrop, cancel: cancelCrop, focus: KeyManager.watchKeys, getBounds: function() { return [boundx * xscale, boundy * yscale]; }, getWidgetSize: function() { return [boundx, boundy]; }, release: Selection.release, destroy: destroy }; $origimg.data('Jcrop', api); return api;
    }; $.fn.Jcrop = function(options) {
        function attachWhenDone(from)
        { var loadsrc = options.useImg || from.src; var img = new Image(); img.onload = function() { $.Jcrop(from, options); }; img.src = loadsrc; }; if (typeof (options) !== 'object') options = {}; this.each(function() {
            if ($(this).data('Jcrop'))
            { if (options == 'api') return $(this).data('Jcrop'); else $(this).data('Jcrop').setOptions(options); }
            else attachWhenDone(this);
        }); return this;
    };
})(jQuery);

/**
* Jcrop v.0.9.8 (minimized)
* (c) 2008 Kelly Hallman and DeepLiquid.com
* More information: https://deepliquid.com/content/Jcrop.html
* Released under MIT License - this header must remain with code
*/


(function($) {
    $.Jcrop = function(obj, opt) {
        var obj = obj, opt = opt; if (typeof (obj) !== 'object') obj = $(obj)[0]; if (typeof (opt) !== 'object') opt = {}; if (!('trackDocument' in opt)) {
            opt.trackDocument = $.browser.msie ? false : true; if ($.browser.msie && $.browser.version.split('.')[0] == '8')
                opt.trackDocument = true;
        }
        if (!('keySupport' in opt))
            opt.keySupport = $.browser.msie ? false : true; var defaults = { trackDocument: false, baseClass: 'jcrop', addClass: null, bgColor: 'black', bgOpacity: .6, borderOpacity: .4, handleOpacity: .5, handlePad: 5, handleSize: 9, handleOffset: 5, edgeMargin: 14, aspectRatio: 0, keySupport: true, cornerHandles: true, sideHandles: true, drawBorders: true, dragEdges: true, boxWidth: 0, boxHeight: 0, boundary: 8, animationDelay: 20, swingSpeed: 3, allowSelect: true, allowMove: true, allowResize: true, minSelect: [0, 0], maxSize: [0, 0], minSize: [0, 0], onChange: function() { }, onSelect: function() { } }; var options = defaults; setOptions(opt); var $origimg = $(obj); var $img = $origimg.clone().removeAttr('id').css({ position: 'absolute' }); $img.width($origimg.width()); $img.height($origimg.height()); $origimg.after($img).hide(); presize($img, options.boxWidth, options.boxHeight); var boundx = $img.width(), boundy = $img.height(), $div = $('<div />').width(boundx).height(boundy).addClass(cssClass('holder')).css({ position: 'relative', backgroundColor: options.bgColor }).insertAfter($origimg).append($img); ; if (options.addClass) $div.addClass(options.addClass); var $img2 = $('<img />').attr('src', $img.attr('src')).css('position', 'absolute').width(boundx).height(boundy); var $img_holder = $('<div />').width(pct(100)).height(pct(100)).css({ zIndex: 310, position: 'absolute', overflow: 'hidden' }).append($img2); var $hdl_holder = $('<div />').width(pct(100)).height(pct(100)).css('zIndex', 320); var $sel = $('<div />').css({ position: 'absolute', zIndex: 300 }).insertBefore($img).append($img_holder, $hdl_holder); var bound = options.boundary; var $trk = newTracker().width(boundx + (bound * 2)).height(boundy + (bound * 2)).css({ position: 'absolute', top: px(-bound), left: px(-bound), zIndex: 290 }).mousedown(newSelection); var xlimit, ylimit, xmin, ymin; var xscale, yscale, enabled = true; var docOffset = getPos($img), btndown, lastcurs, dimmed, animating, shift_down; var Coords = function() {
                var x1 = 0, y1 = 0, x2 = 0, y2 = 0, ox, oy; function setPressed(pos)
                { var pos = rebound(pos); x2 = x1 = pos[0]; y2 = y1 = pos[1]; }; function setCurrent(pos)
                { var pos = rebound(pos); ox = pos[0] - x2; oy = pos[1] - y2; x2 = pos[0]; y2 = pos[1]; }; function getOffset()
                { return [ox, oy]; }; function moveOffset(offset)
                { var ox = offset[0], oy = offset[1]; if (0 > x1 + ox) ox -= ox + x1; if (0 > y1 + oy) oy -= oy + y1; if (boundy < y2 + oy) oy += boundy - (y2 + oy); if (boundx < x2 + ox) ox += boundx - (x2 + ox); x1 += ox; x2 += ox; y1 += oy; y2 += oy; }; function getCorner(ord) {
                    var c = getFixed(); switch (ord)
                    { case 'ne': return [c.x2, c.y]; case 'nw': return [c.x, c.y]; case 'se': return [c.x2, c.y2]; case 'sw': return [c.x, c.y2]; } 
                }; function getFixed() {
                    if (!options.aspectRatio) return getRect(); var aspect = options.aspectRatio, min_x = options.minSize[0] / xscale, min_y = options.minSize[1] / yscale, max_x = options.maxSize[0] / xscale, max_y = options.maxSize[1] / yscale, rw = x2 - x1, rh = y2 - y1, rwa = Math.abs(rw), rha = Math.abs(rh), real_ratio = rwa / rha, xx, yy; if (max_x == 0) { max_x = boundx * 10 }
                    if (max_y == 0) { max_y = boundy * 10 }
                    if (real_ratio < aspect) {
                        yy = y2; w = rha * aspect; xx = rw < 0 ? x1 - w : w + x1; if (xx < 0)
                        { xx = 0; h = Math.abs((xx - x1) / aspect); yy = rh < 0 ? y1 - h : h + y1; }
                        else if (xx > boundx)
                        { xx = boundx; h = Math.abs((xx - x1) / aspect); yy = rh < 0 ? y1 - h : h + y1; } 
                    }
                    else {
                        xx = x2; h = rwa / aspect; yy = rh < 0 ? y1 - h : y1 + h; if (yy < 0)
                        { yy = 0; w = Math.abs((yy - y1) * aspect); xx = rw < 0 ? x1 - w : w + x1; }
                        else if (yy > boundy)
                        { yy = boundy; w = Math.abs(yy - y1) * aspect; xx = rw < 0 ? x1 - w : w + x1; } 
                    }
                    if (xx > x1) {
                        if (xx - x1 < min_x) { xx = x1 + min_x; } else if (xx - x1 > max_x) { xx = x1 + max_x; }
                        if (yy > y1) { yy = y1 + (xx - x1) / aspect; } else { yy = y1 - (xx - x1) / aspect; } 
                    } else if (xx < x1) {
                        if (x1 - xx < min_x) { xx = x1 - min_x } else if (x1 - xx > max_x) { xx = x1 - max_x; }
                        if (yy > y1) { yy = y1 + (x1 - xx) / aspect; } else { yy = y1 - (x1 - xx) / aspect; } 
                    }
                    if (xx < 0) { x1 -= xx; xx = 0; } else if (xx > boundx) { x1 -= xx - boundx; xx = boundx; }
                    if (yy < 0) { y1 -= yy; yy = 0; } else if (yy > boundy) { y1 -= yy - boundy; yy = boundy; }
                    return last = makeObj(flipCoords(x1, y1, xx, yy));
                }; function rebound(p)
                { if (p[0] < 0) p[0] = 0; if (p[1] < 0) p[1] = 0; if (p[0] > boundx) p[0] = boundx; if (p[1] > boundy) p[1] = boundy; return [p[0], p[1]]; }; function flipCoords(x1, y1, x2, y2) {
                    var xa = x1, xb = x2, ya = y1, yb = y2; if (x2 < x1)
                    { xa = x2; xb = x1; }
                    if (y2 < y1)
                    { ya = y2; yb = y1; }
                    return [Math.round(xa), Math.round(ya), Math.round(xb), Math.round(yb)];
                }; function getRect() {
                    var xsize = x2 - x1; var ysize = y2 - y1; if (xlimit && (Math.abs(xsize) > xlimit))
                        x2 = (xsize > 0) ? (x1 + xlimit) : (x1 - xlimit); if (ylimit && (Math.abs(ysize) > ylimit))
                        y2 = (ysize > 0) ? (y1 + ylimit) : (y1 - ylimit); if (ymin && (Math.abs(ysize) < ymin))
                        y2 = (ysize > 0) ? (y1 + ymin) : (y1 - ymin); if (xmin && (Math.abs(xsize) < xmin))
                        x2 = (xsize > 0) ? (x1 + xmin) : (x1 - xmin); if (x1 < 0) { x2 -= x1; x1 -= x1; }
                    if (y1 < 0) { y2 -= y1; y1 -= y1; }
                    if (x2 < 0) { x1 -= x2; x2 -= x2; }
                    if (y2 < 0) { y1 -= y2; y2 -= y2; }
                    if (x2 > boundx) { var delta = x2 - boundx; x1 -= delta; x2 -= delta; }
                    if (y2 > boundy) { var delta = y2 - boundy; y1 -= delta; y2 -= delta; }
                    if (x1 > boundx) { var delta = x1 - boundy; y2 -= delta; y1 -= delta; }
                    if (y1 > boundy) { var delta = y1 - boundy; y2 -= delta; y1 -= delta; }
                    return makeObj(flipCoords(x1, y1, x2, y2));
                }; function makeObj(a)
                { return { x: a[0], y: a[1], x2: a[2], y2: a[3], w: a[2] - a[0], h: a[3] - a[1] }; }; return { flipCoords: flipCoords, setPressed: setPressed, setCurrent: setCurrent, getOffset: getOffset, moveOffset: moveOffset, getCorner: getCorner, getFixed: getFixed };
            } (); var Selection = function() {
                var start, end, dragmode, awake, hdep = 370; var borders = {}; var handle = {}; var seehandles = false; var hhs = options.handleOffset; if (options.drawBorders) { borders = { top: insertBorder('hline').css('top', $.browser.msie ? px(-1) : px(0)), bottom: insertBorder('hline'), left: insertBorder('vline'), right: insertBorder('vline') }; }
                if (options.dragEdges) { handle.t = insertDragbar('n'); handle.b = insertDragbar('s'); handle.r = insertDragbar('e'); handle.l = insertDragbar('w'); }
                options.sideHandles && createHandles(['n', 's', 'e', 'w']); options.cornerHandles && createHandles(['sw', 'nw', 'ne', 'se']); function insertBorder(type)
                { var jq = $('<div />').css({ position: 'absolute', opacity: options.borderOpacity }).addClass(cssClass(type)); $img_holder.append(jq); return jq; }; function dragDiv(ord, zi)
                { var jq = $('<div />').mousedown(createDragger(ord)).css({ cursor: ord + '-resize', position: 'absolute', zIndex: zi }); $hdl_holder.append(jq); return jq; }; function insertHandle(ord)
                { return dragDiv(ord, hdep++).css({ top: px(-hhs + 1), left: px(-hhs + 1), opacity: options.handleOpacity }).addClass(cssClass('handle')); }; function insertDragbar(ord) {
                    var s = options.handleSize, o = hhs, h = s, w = s, t = o, l = o; switch (ord)
                    { case 'n': case 's': w = pct(100); break; case 'e': case 'w': h = pct(100); break; }
                    return dragDiv(ord, hdep++).width(w).height(h).css({ top: px(-t + 1), left: px(-l + 1) });
                }; function createHandles(li)
                { for (i in li) handle[li[i]] = insertHandle(li[i]); }; function moveHandles(c)
                { var midvert = Math.round((c.h / 2) - hhs), midhoriz = Math.round((c.w / 2) - hhs), north = west = -hhs + 1, east = c.w - hhs, south = c.h - hhs, x, y; 'e' in handle && handle.e.css({ top: px(midvert), left: px(east) }) && handle.w.css({ top: px(midvert) }) && handle.s.css({ top: px(south), left: px(midhoriz) }) && handle.n.css({ left: px(midhoriz) }); 'ne' in handle && handle.ne.css({ left: px(east) }) && handle.se.css({ top: px(south), left: px(east) }) && handle.sw.css({ top: px(south) }); 'b' in handle && handle.b.css({ top: px(south) }) && handle.r.css({ left: px(east) }); }; function moveto(x, y)
                { $img2.css({ top: px(-y), left: px(-x) }); $sel.css({ top: px(y), left: px(x) }); }; function resize(w, h)
                { $sel.width(w).height(h); }; function refresh()
                { var c = Coords.getFixed(); Coords.setPressed([c.x, c.y]); Coords.setCurrent([c.x2, c.y2]); updateVisible(); }; function updateVisible()
                { if (awake) return update(); }; function update()
                { var c = Coords.getFixed(); resize(c.w, c.h); moveto(c.x, c.y); options.drawBorders && borders['right'].css({ left: px(c.w - 1) }) && borders['bottom'].css({ top: px(c.h - 1) }); seehandles && moveHandles(c); awake || show(); options.onChange(unscale(c)); }; function show()
                { $sel.show(); $img.css('opacity', options.bgOpacity); awake = true; }; function release()
                { disableHandles(); $sel.hide(); $img.css('opacity', 1); awake = false; }; function showHandles() {
                    if (seehandles)
                    { moveHandles(Coords.getFixed()); $hdl_holder.show(); } 
                }; function enableHandles() {
                    seehandles = true; if (options.allowResize)
                    { moveHandles(Coords.getFixed()); $hdl_holder.show(); return true; } 
                }; function disableHandles()
                { seehandles = false; $hdl_holder.hide(); }; function animMode(v)
                { (animating = v) ? disableHandles() : enableHandles(); }; function done()
                { animMode(false); refresh(); }; var $track = newTracker().mousedown(createDragger('move')).css({ cursor: 'move', position: 'absolute', zIndex: 360 })
                $img_holder.append($track); disableHandles(); return { updateVisible: updateVisible, update: update, release: release, refresh: refresh, setCursor: function(cursor) { $track.css('cursor', cursor); }, enableHandles: enableHandles, enableOnly: function() { seehandles = true; }, showHandles: showHandles, disableHandles: disableHandles, animMode: animMode, done: done };
            } (); var Tracker = function() {
                var onMove = function() { }, onDone = function() { }, trackDoc = options.trackDocument; if (!trackDoc)
                { $trk.mousemove(trackMove).mouseup(trackUp).mouseout(trackUp); }
                function toFront() {
                    $trk.css({ zIndex: 450 }); if (trackDoc)
                    { $(document).mousemove(trackMove).mouseup(trackUp); } 
                }
                function toBack() {
                    $trk.css({ zIndex: 290 }); if (trackDoc)
                    { $(document).unbind('mousemove', trackMove).unbind('mouseup', trackUp); } 
                }
                function trackMove(e)
                { onMove(mouseAbs(e)); }; function trackUp(e) {
                    e.preventDefault(); e.stopPropagation(); if (btndown)
                    { btndown = false; onDone(mouseAbs(e)); options.onSelect(unscale(Coords.getFixed())); toBack(); onMove = function() { }; onDone = function() { }; }
                    return false;
                }; function activateHandlers(move, done)
                { btndown = true; onMove = move; onDone = done; toFront(); return false; }; function setCursor(t) { $trk.css('cursor', t); }; $img.before($trk); return { activateHandlers: activateHandlers, setCursor: setCursor };
            } (); var KeyManager = function() {
                var $keymgr = $('<input type="radio" />').css({ position: 'absolute', left: '-30px' }).keypress(parseKey).blur(onBlur), $keywrap = $('<div />').css({ position: 'absolute', overflow: 'hidden' }).append($keymgr); function watchKeys() {
                    if (options.keySupport)
                    { $keymgr.show(); $keymgr.focus(); } 
                }; function onBlur(e)
                { $keymgr.hide(); }; function doNudge(e, x, y)
                { if (options.allowMove) { Coords.moveOffset([x, y]); Selection.updateVisible(); }; e.preventDefault(); e.stopPropagation(); }; function parseKey(e) {
                    if (e.ctrlKey) return true; shift_down = e.shiftKey ? true : false; var nudge = shift_down ? 10 : 1; switch (e.keyCode)
                    { case 37: doNudge(e, -nudge, 0); break; case 39: doNudge(e, nudge, 0); break; case 38: doNudge(e, 0, -nudge); break; case 40: doNudge(e, 0, nudge); break; case 27: Selection.release(); break; case 9: return true; }
                    return nothing(e);
                }; if (options.keySupport) $keywrap.insertBefore($img); return { watchKeys: watchKeys };
            } (); function px(n) { return '' + parseInt(n) + 'px'; }; function pct(n) { return '' + parseInt(n) + '%'; }; function cssClass(cl) { return options.baseClass + '-' + cl; }; function getPos(obj)
            { var pos = $(obj).offset(); return [pos.left, pos.top]; }; function mouseAbs(e)
            { return [(e.pageX - docOffset[0]), (e.pageY - docOffset[1])]; }; function myCursor(type) {
                if (type != lastcurs)
                { Tracker.setCursor(type); lastcurs = type; } 
            }; function startDragMode(mode, pos) {
                docOffset = getPos($img); Tracker.setCursor(mode == 'move' ? mode : mode + '-resize'); if (mode == 'move')
                    return Tracker.activateHandlers(createMover(pos), doneSelect); var fc = Coords.getFixed(); var opp = oppLockCorner(mode); var opc = Coords.getCorner(oppLockCorner(opp)); Coords.setPressed(Coords.getCorner(opp)); Coords.setCurrent(opc); Tracker.activateHandlers(dragmodeHandler(mode, fc), doneSelect);
            }; function dragmodeHandler(mode, f) {
                return function(pos) {
                    if (!options.aspectRatio) switch (mode)
                    { case 'e': pos[1] = f.y2; break; case 'w': pos[1] = f.y2; break; case 'n': pos[0] = f.x2; break; case 's': pos[0] = f.x2; break; }
                    else switch (mode)
                    { case 'e': pos[1] = f.y + 1; break; case 'w': pos[1] = f.y + 1; break; case 'n': pos[0] = f.x + 1; break; case 's': pos[0] = f.x + 1; break; }
                    Coords.setCurrent(pos); Selection.update();
                };
            }; function createMover(pos) {
                var lloc = pos; KeyManager.watchKeys(); return function(pos)
                { Coords.moveOffset([pos[0] - lloc[0], pos[1] - lloc[1]]); lloc = pos; Selection.update(); };
            }; function oppLockCorner(ord) {
                switch (ord)
                { case 'n': return 'sw'; case 's': return 'nw'; case 'e': return 'nw'; case 'w': return 'ne'; case 'ne': return 'sw'; case 'nw': return 'se'; case 'se': return 'nw'; case 'sw': return 'ne'; };
            }; function createDragger(ord)
            { return function(e) { if (options.disabled) return false; if ((ord == 'move') && !options.allowMove) return false; btndown = true; startDragMode(ord, mouseAbs(e)); e.stopPropagation(); e.preventDefault(); return false; }; }; function presize($obj, w, h) {
                var nw = $obj.width(), nh = $obj.height(); if ((nw > w) && w > 0)
                { nw = w; nh = (w / $obj.width()) * $obj.height(); }
                if ((nh > h) && h > 0)
                { nh = h; nw = (h / $obj.height()) * $obj.width(); }
                xscale = $obj.width() / nw; yscale = $obj.height() / nh; $obj.width(nw).height(nh);
            }; function unscale(c)
            { return { x: parseInt(c.x * xscale), y: parseInt(c.y * yscale), x2: parseInt(c.x2 * xscale), y2: parseInt(c.y2 * yscale), w: parseInt(c.w * xscale), h: parseInt(c.h * yscale) }; }; function doneSelect(pos) {
                var c = Coords.getFixed(); if (c.w > options.minSelect[0] && c.h > options.minSelect[1])
                { Selection.enableHandles(); Selection.done(); }
                else
                { Selection.release(); }
                Tracker.setCursor(options.allowSelect ? 'crosshair' : 'default');
            }; function newSelection(e)
            { if (options.disabled) return false; if (!options.allowSelect) return false; btndown = true; docOffset = getPos($img); Selection.disableHandles(); myCursor('crosshair'); var pos = mouseAbs(e); Coords.setPressed(pos); Tracker.activateHandlers(selectDrag, doneSelect); KeyManager.watchKeys(); Selection.update(); e.stopPropagation(); e.preventDefault(); return false; }; function selectDrag(pos)
            { Coords.setCurrent(pos); Selection.update(); }; function newTracker()
            { var trk = $('<div></div>').addClass(cssClass('tracker')); $.browser.msie && trk.css({ opacity: 0, backgroundColor: 'white' }); return trk; }; function animateTo(a) {
                var x1 = a[0] / xscale, y1 = a[1] / yscale, x2 = a[2] / xscale, y2 = a[3] / yscale; if (animating) return; var animto = Coords.flipCoords(x1, y1, x2, y2); var c = Coords.getFixed(); var animat = initcr = [c.x, c.y, c.x2, c.y2]; var interv = options.animationDelay; var x = animat[0]; var y = animat[1]; var x2 = animat[2]; var y2 = animat[3]; var ix1 = animto[0] - initcr[0]; var iy1 = animto[1] - initcr[1]; var ix2 = animto[2] - initcr[2]; var iy2 = animto[3] - initcr[3]; var pcent = 0; var velocity = options.swingSpeed; Selection.animMode(true); var animator = function() {
                    return function()
                    { pcent += (100 - pcent) / velocity; animat[0] = x + ((pcent / 100) * ix1); animat[1] = y + ((pcent / 100) * iy1); animat[2] = x2 + ((pcent / 100) * ix2); animat[3] = y2 + ((pcent / 100) * iy2); if (pcent < 100) animateStart(); else Selection.done(); if (pcent >= 99.8) pcent = 100; setSelectRaw(animat); };
                } (); function animateStart()
                { window.setTimeout(animator, interv); }; animateStart();
            }; function setSelect(rect)
            { setSelectRaw([rect[0] / xscale, rect[1] / yscale, rect[2] / xscale, rect[3] / yscale]); }; function setSelectRaw(l)
            { Coords.setPressed([l[0], l[1]]); Coords.setCurrent([l[2], l[3]]); Selection.update(); }; function setOptions(opt) {
                if (typeof (opt) != 'object') opt = {}; options = $.extend(options, opt); if (typeof (options.onChange) !== 'function')
                    options.onChange = function() { }; if (typeof (options.onSelect) !== 'function')
                    options.onSelect = function() { };
            }; function tellSelect()
            { return unscale(Coords.getFixed()); }; function tellScaled()
            { return Coords.getFixed(); }; function setOptionsNew(opt)
            { setOptions(opt); interfaceUpdate(); }; function disableCrop()
            { options.disabled = true; Selection.disableHandles(); Selection.setCursor('default'); Tracker.setCursor('default'); }; function enableCrop()
            { options.disabled = false; interfaceUpdate(); }; function cancelCrop()
            { Selection.done(); Tracker.activateHandlers(null, null); }; function destroy()
            { $div.remove(); $origimg.show(); }; function interfaceUpdate(alt) {
                options.allowResize ? alt ? Selection.enableOnly() : Selection.enableHandles() : Selection.disableHandles(); Tracker.setCursor(options.allowSelect ? 'crosshair' : 'default'); Selection.setCursor(options.allowMove ? 'move' : 'default'); $div.css('backgroundColor', options.bgColor); if ('setSelect' in options) { setSelect(opt.setSelect); Selection.done(); delete (options.setSelect); }
                if ('trueSize' in options) { xscale = options.trueSize[0] / boundx; yscale = options.trueSize[1] / boundy; }
                xlimit = options.maxSize[0] || 0; ylimit = options.maxSize[1] || 0; xmin = options.minSize[0] || 0; ymin = options.minSize[1] || 0; if ('outerImage' in options)
                { $img.attr('src', options.outerImage); delete (options.outerImage); }
                Selection.refresh();
            }; $hdl_holder.hide(); interfaceUpdate(true); var api = { animateTo: animateTo, setSelect: setSelect, setOptions: setOptionsNew, tellSelect: tellSelect, tellScaled: tellScaled, disable: disableCrop, enable: enableCrop, cancel: cancelCrop, focus: KeyManager.watchKeys, getBounds: function() { return [boundx * xscale, boundy * yscale]; }, getWidgetSize: function() { return [boundx, boundy]; }, release: Selection.release, destroy: destroy }; $origimg.data('Jcrop', api); return api;
    }; $.fn.Jcrop = function(options) {
        function attachWhenDone(from)
        { var loadsrc = options.useImg || from.src; var img = new Image(); img.onload = function() { $.Jcrop(from, options); }; img.src = loadsrc; }; if (typeof (options) !== 'object') options = {}; this.each(function() {
            if ($(this).data('Jcrop'))
            { if (options == 'api') return $(this).data('Jcrop'); else $(this).data('Jcrop').setOptions(options); }
            else attachWhenDone(this);
        }); return this;
    };
})(jQuery);


