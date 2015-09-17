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
* the code is quoted from "http://www.shawnolson.net/a/503/altering-css-class-attributes-with-javascript.html" with some modifications.
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
		//http://www.shawnolson.net/a/503/altering-css-class-attributes-with-javascript.html
		var cssRules;

		var added = false;
		for (var S = 0; S < document.styleSheets.length; S++) {

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

             customStyle: function(options) {
                 if (!$.browser.msie || ($.browser.msie && $.browser.version > 6)) {
                     return this.each(function() {
                         if (!$(this).parent().next().hasClass('customStyleSelectBox')) {
                             var currentSelected = $(this).find(':selected');
                             $(this).after('<div class="customStyleSelectBox"><span class="customStyleSelectBoxInner">' + currentSelected.text() + '</span></span>').css({ position: 'absolute', opacity: 0, fontSize: $(this).next().css('font-size') });
                             var selectBoxSpan = $(this).next();
                             //var x = selectBoxSpan.position.right - $(this).position.right;
                             var selectBoxWidth = parseInt($(this).width()) - parseInt(selectBoxSpan.css('padding-left')) - parseInt(selectBoxSpan.css('padding-right'));
                             var selectBoxSpanInner = selectBoxSpan.find(':first-child');
                             selectBoxSpan.css({ display: 'inline-block' });
                             //selectBoxSpan.attr('')
                             selectBoxSpanInner.css({ width: selectBoxWidth, display: 'inline-block' });
                             var selectBoxHeight = parseInt(selectBoxSpan.height()) + parseInt(selectBoxSpan.css('padding-top')) + parseInt(selectBoxSpan.css('padding-bottom'));
                             $(this).height(selectBoxHeight).change(function() {
                                 // selectBoxSpanInner.text($(this).val()).parent().addClass('changed');   This was not ideal
                                 selectBoxSpanInner.text($(this).find(':selected').text()).parent().addClass('changed');
                                 // Thanks to Juarez Filho & PaddyMurphy
                             });
                             var wrapDiv = "<div style='position:absolute;'/>";
                             $(this).wrap(wrapDiv);
                             $(this).position({ of: selectBoxSpan, my: "left top", at: "left top", colision:"fit fit", offset: "0 0" });
                         }
                     });
                 }
             }
         });
     })(jQuery);

     jQuery.fn.prettyCheckboxes = function(settings) {
         settings = jQuery.extend({
             checkboxWidth: 17,
             checkboxHeight: 17,
             className: 'prettyCheckbox',
             display: 'list'
         }, settings);

         $(this).each(function() {
             if (!$(this).hasClass('hiddenCheckbox')) {
                 // Find the label
                 $label = $('label[for="' + $(this).attr('id') + '"]');

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