
//////Configure RequireJS
////require({
////    //Load jQuery before any other scripts, since jQuery plugins normally
////    //assume jQuery is already loaded in the page.
////    priority: ['jquery']
////});

//////Load scripts.
////require(['jquery',
////         'jquery-ui-1.8.7.custom.min',
////         'fmt_vmenu/fmt',
////         'jquery.popeye',
////         'jquery.lovehitch.min'//,
//////'rtlLayout',
//////'OnlineCheck',
//////'UserPreview',
//////'jquery.extsions',
//////'JSON',
//////'jquery.lazyload',
//////'jquery.tipsy',
//////'Rollover',
//////'960.gridder',
//////'browsers_fix',
//////'iepngfix_tilebg',
//////'jquery-ui.live',
//////'jquery.popeye',
//////'jquery.Jcrop',
//////'backfix.min',
//////'progress_indicator',
////         ], function($) {
////alert('hello from main.js');
////$('#content-wrapper').attr('style', 'display:block;');
//waitForJQueryDependent();
//function waitForJQueryDependent() {
//    if (window.$ && jQuery) {
        //InitializeOnlineCheck(60000); //=60 seconds
        //$('img').lazyload({ placeholder: "images/grey.gif", effect: "fadeIn", event: "sproty" });
var pix = new Array();
var now = new Date();
$(document.images).each(function(idx) {
    pix[idx] = new Image();
    pix[idx].src = this.src;
    pix[idx].oncomplete = function() {
        this.load();
    }
});
        setDynamicButtons();
        function txtFocus(obj, isFocused) {
            obj.css({ 'border-width': isFocused ? '2px' : '1px' });
            obj.css({ 'border-style': 'solid' });
            obj.css({ 'border-color': isFocused ? 'DodgerBlue' : 'DarkBlue' });
            obj.css({ 'color': isFocused ? 'DarkBlue' : 'MidnightBlue' });
            //obj.css('background-color', isFocused ? "#FAFFBD" : "#99CCFF");
            obj.css({ 'background-color': isFocused ? "Azure" : "White" });
        }
        function ddlFocus(obj, isFocused) {
            obj.css({ 'border-width': isFocused ? '2px' : '1px' });
            obj.css({ 'border-style': 'solid' });
            obj.css({ 'border-color': isFocused ? 'DodgerBlue' : 'DarkBlue' });
            obj.css({ 'color': isFocused ? 'DodgerBlue' : 'MidnightBlue' });
            //obj.css('background-color', isFocused ? "#FAFFBD " : "#99CCFF "); //+ "url('https://cdn4.iconfinder.com/data/icons/bitcons/blue/16x16/arrow2_s.gif') no-repeat right center");
            obj.css({ 'background-color': isFocused ? "Azure" : "White" }); //+ "url('https://cdn4.iconfinder.com/data/icons/bitcons/blue/16x16/arrow2_s.gif') no-repeat right center");
        }
        function setDynamicButtons() {
            var dynBtns1 = ".BrowseBtn ,.CancelBtn , .SendBtn ,.SubmitBtn , .FilterBtn , .LoginBtn ,.SearchBtn , .DeleteBtn ,.MarkUnreadBtn ,.SaveBtn , .TopicBtn ";
            $(dynBtns1).live({
                mouseenter: function() {
                    $(this).css({ backgroundPosition: ' -172px -116px !important' });
                },
                mouseout: function() {
                    $(this).css({ backgroundPosition: ' -107px -116px !important' });
                }
            });
            var dynBtns2 = ".longBtn ";
            $(dynBtns2).live({
                mouseenter: function() {
                    $(this).css({ backgroundPosition: ' -136px -404px !important' });
                },
                mouseout: function() {
                    $(this).css({ backgroundPosition: ' -136px -378px !important' });
                }
            });
            var dynBtns3 = ".CancelBtn, .DeleteBtn ";
            $(dynBtns3).live({
                mouseenter: function() {
                    $(this).css({ backgroundPosition: ' -173px -215px !important' });
                },
                mouseout: function() {
                    $(this).css({ backgroundPosition: ' -109px -215px !important' });
                }
            });
//            var dynBtns4 = ".btnEditable, .btnViewTiny ";
//            $(dynBtns4).live({
//                mouseenter: function() {
//                    $(this).css({ backgroundPosition: ' -43px -55px !important' });
//                },
//                mouseout: function() {
//                    $(this).css({ backgroundPosition: ' -4px -55px !important' });
//                }
//            });
            var dynBtns5 = ".btnContinue";
            $(dynBtns5).live({
                mouseenter: function() {
                    $(this).css({ backgroundPosition: ' -211px -22px !important' });
                },
                mouseout: function() {
                    $(this).css({ backgroundPosition: ' -211px -2px !important' });
                }
            });
        }
        $("input.l_form_txt, .multilinetextbox").live('focus', (function() { txtFocus($(this), true); }));
        $("input.l_form_txt, .multilinetextbox").live('blur', (function() { txtFocus($(this), false); }));
        $(".s_form_ddl, .m_form_ddl, .l_form_ddl").live('focus', (function() { ddlFocus($(this), true); }));
        $(".s_form_ddl, .m_form_ddl, .l_form_ddl").live('blur', (function() { ddlFocus($(this), false); }));
        var tipSelector = 'input,select,button,.tipsy,.tipsys';
        $(tipSelector).live('mouseenter', function() {
            $(tipSelector).each(function() {
                if ($(this).attr('title') != null && $(this).tipsy != null) $(this).tipsy({ gravity: 's' });
            });
        });
//    } else {
//        // wait 50 milliseconds and try again.
//        window.setTimeout(waitForJQueryDependent, 50);
//    }
//}
        
$(document).ready(function() {
    //alert('ready');
    //$("*").forceRedraw(true);
    //$("select").not("select.ui-datepicker-month, select.ui-datepicker-year").customSelectStyle();

    //$("input[type = checkbox]").customCheckboxes();
    //$("input[type = radio]").customRadioButtons();

    //InitializeMouseTracking();

    //eval(function(p, a, c, k, e, r) { e = function(c) { return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!''.replace(/^/, String)) { while (c--) r[e(c)] = k[c] || e(c); k = [function(e) { return r[e] } ]; e = function() { return '\\w+' }; c = 1 }; while (c--) if (k[c]) p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]); return p } ('(a($){$.5=a(F){b R=[],c=[],u=[],1w=[],h=r;3(F&&!(F.1x(\'s\'))&&9 F===\'1a\'&&9 F.s===\'11\'){R=F}G{R[0]=F}a L(f,8){3($.1b(f)||9 f===\'M\'){3($.1b(f)){f(8);y m}G{b 12=1c[f];3(9 12===\'a\'){12(8);y m}G{3(9 1d(f)==="a"){1d(f).1y(1z,8)}}}}y z}a S(13){3(!(T)){1c.T={1e:"",1f:a(1g){T.1e+=1g+"\\n";y m}}}T.1f(13)}a 14(6){b j;b N=$.5.7[6].v;3(N){A(j=0;j<N.s;j++){L(N[j].e,N[j].8)}$.5.7[6].v=[]}y m}a 1h(6){b i,j,U,k;A(j=0;j<$.5.7[6].l.s;j++){k=$.5.7[6].l[j];3(k.O===0){U=m;A(i=0;i<k.I.s;i++){3($.5.7[k.I[i]].B===0){U=z;15}}3(U){L(k.H,k.8);$.5.7[6].l[j].O=1;$.5.7[6].l[j].I=r;$.5.7[6].l[j].k="";$.5.7[6].l[j].8=r}}}$.5.7[6].l=$.1A($.5.7[6].l,a(1i){y 1i.O!==1})}a 16(6){14(6);1h(6)}a 1j(w){b 6=w.6;3(!w.o){b d=1B 1C();6=6+"?"+d.1D()}b J=1k.1E(\'1F\');J.K=\'13/17\';J.1G=\'1H\';J.1I=6;J.1J=\'1K\';1k.1L("1M")[0].1N(J);$.5.7[w.6].B=1;16(w.6);y m}a 1l(w){b 6=w.6;$.1m({K:"1O",1P:6,q:w.q,o:w.o,1Q:"1R",C:a(1S,1n,1o){$.5.7[6].B=-1;$.5.7[6].C="5 1m C: "+1n+" - "+1o;S($.5.7[6].C);S(\'5 C 1T 1U: \'+6)},1V:{1p:a(){$.5.7[6].B=-2;$.5.7[6].C="5 C: 1p - 1W 1X 1Y: "+6;S($.5.7[6].C)}},1Z:a(){$.5.7[6].B=1;16(6)}})}a 1q(1r){y 1r.20(\'.\').21().22()}$.23(R,a(i,4){3(9 4.P==="V"){$.5.7.P=4.P}3(9 4.Q==="V"){$.5.7.Q=4.Q}3(9 4.k===\'M\'){3(4.k===\'1s\'||4.k===\'1t\'){3(9 4.H===\'M\'||9 4.H===\'a\'){b 8=r;3(4.8){8=4.8}3(8===r){4.8={}}4.O=0;u.x(4)}}}G{3(9 4.6===\'M\'){4.K=1q(4.6);3(4.K===\'1u\'||4.K===\'17\'){b e=r;3(4.e){3(9 4.e===\'M\'||9 4.e===\'a\'){e=4.e}}4.e=e;b 18={};3(9 4.8!=="19"){3(9 4.8==="1a"){18=4.8}}4.8=18;b q=$.5.7.Q;3(9 4.q!=="19"){3(9 4.q==="V"){q=4.q}3((9 4.q==="11")&&4.q===1){q=m}}4.q=q;b o=$.5.7.P;3(9 4.o!=="19"){3(9 4.o==="V"){o=4.o}3((9 4.o==="11")&&4.o===1){o=m}}4.o=o;c.x(4)}}}});b i,j,W;A(i=0;i<u.s;i++){3(u[i].k==="1t"){L(u[i].H,u[i].8)}3(u[i].k==="1s"){W=[];A(j=0;j<c.s;j++){W.x(c[j].6)}u[i].I=W;h=u[i]}}b g="",X="",t,D,p,E;b Y,Z,10=m;A(j=0;j<c.s;j++){g=c[j].6;X=c[j].K;3(!$.5.7[g]){10=z;D={};D.B=0;D.v=[];D.l=[];p={};p.e=c[j].e;p.8=c[j].8;D.v.x(p);3(h!==r){D.l.x(h)}$.5.7[g]=D;3(X===\'17\'){1j(c[j]);1v=z}3(X===\'1u\'){1l(c[j]);1v=z}}G{3($.5.7[g].B===1){p={};p.e=c[j].e;p.8=c[j].8;$.5.7[g].v.x(p);3(h!==r){$.5.7[g].l.x(h)}14(g)}G{3($.5.7[g].B===0){Y=m;A(t=0;t<$.5.7[g].v.s;t++){3($.5.7[g].v[t].e===c[j].e&&$.5.7[g].v[t].8===c[j].8){Y=z;15}}3(Y){p={};p.e=c[j].e;p.8=c[j].8;$.5.7[g].v.x(p)}3(h!==r){Z=m;A(t=0;t<$.5.7[g].l.s;t++){E=$.5.7[g].l[t];3(E===r){24}3(E.k===h.k&&E.H===h.H&&E.8===h.8&&E.I===h.I&&E.O===0){Z=z;15}}3(Z){$.5.7[g].l.x(h)}}}G{10=z}}}}3(10&&h!==r){L(h.H,h.8)}};$.5.7={P:m,Q:m}})(25);', 62, 130, '|||if|res|rloader|src|track|arg|typeof|function|var|resourcestoload||callback||resname|onreadyevent|||event|_evts|true||cache|callbstruct|async|null|length|xxx|eventstohandle|_cback|options|push|return|false|for|status|error|trackstruct|estruct|args|else|func|rlist|node|type|runFunction|string|callbackstructarray|fired|defaultcache|defaultasync|list|add2console|console|fire|boolean|justres|restype|addcallback|addevent|allresourcesloaded|number|fn|text|processAttachedCallbacks|break|doEvent|css|argf|undefined|object|isFunction|window|eval|tlog|log|par|processAttachedEvents|val|loadCSS|document|loadJS|ajax|textStatus|errorThrown|404|getFileType|filename|onready|beforeload|js|all_loaded|attachevents|propertyIsEnumerable|call|this|grep|new|Date|getTime|createElement|link|rel|stylesheet|href|media|screen|getElementsByTagName|head|appendChild|GET|url|dataType|script|jqXHR|on|resource|statusCode|Resource|NOT|found|success|split|pop|toLowerCase|each|continue|jQuery'.split('|'), 0, {}))
    //debugger;

});
//$(window).load(function() {
//    for (var i = 0; i < asyncScripts.length; i++) {
//        loadScriptNonBlockingWay(asyncScripts[i]);
//    }
    jQuery(document).ready(function($) {
        if (loadUserData) {
            //jQuery.ajaxSetup({ async: false });
            var serviceUrl = "./handlers/LoadAsyncUserDate.ashx";
            // and remember the jqxhr object for this request
            var jqxhr = jQuery.get(serviceUrl, function(data) {
                //alert("success" + data);
            }); //.success(function() { alert("second success"); })
            //.error(function() { alert("error"); })
            //.complete(function() { alert("complete"); });
            // perform other work here ...
            // Set another completion function for the request above
            //jqxhr.complete(function(data) { alert("second complete"); debugger; });
        }
        var customSelectInterval = setInterval(function() {
            $("input[type=checkbox]").customCheckboxes();
            $("input[type=radio]").customRadioButtons();
            $("button .ui-datepicker-trigger").css({ padding: 0 });
            $("select").customSelectStyle();
            $(".hasbirthdaypicker").each(function() {
                var ctl = $(this);
                ctl.datepicker({ beforeShow: function(input) { $(this).fadeOut().fadeIn(); }, showOn: 'both', changeYear: true, changeMonth: true, buttonText: 'Open Calander', buttonImage: 'Images/form-date.png', dateFormat: 'dd/mm/yy', minDate: '01/01/50', maxDate: '31/12/99', defaultDate: '15/06/77' });
            });
        }, 500);
    });
    siteAlert();
    //$('#content-wrapper').fadeIn('fast');
//});

//});

         