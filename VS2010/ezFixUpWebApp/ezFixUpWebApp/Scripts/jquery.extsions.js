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
            if (hasCss){
                //ATTACH CORRESPONDING STYLE SHEET                         
                var style = document.createElement('link');
                style.type = 'text/css'; 
                style.href = '/css/'+ reqScriptName +'.css';
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
                success:function(ajaxData){                          
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
                             var selectBoxWidth = parseInt($(this).width())-parseInt(selectBoxVisible.css('padding-left')) - parseInt(selectBoxVisible.css('padding-right'));
                             var selectBoxSpanInner = selectBoxVisible.find(':first-child');
                             selectBoxVisible.css({ display: 'inline-block' });
                             selectBoxSpanInner.css({ width: selectBoxWidth, display: 'inline-block' });
                             var selectBoxHeight = parseInt(selectBoxVisible.height()) + parseInt(selectBoxVisible.css('padding-top')) + parseInt(selectBoxVisible.css('padding-bottom'));
                             $(this).height(selectBoxHeight).change(function() {
                                 selectBoxSpanInner.text($(this).find(':selected').text()).parent().addClass('changed');
                             });
                             $(this).css({ display: 'none;' });
                             var wrapWidth = parseInt($(this).width())+10;
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
                 if ($label.length == 1 ) {
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
