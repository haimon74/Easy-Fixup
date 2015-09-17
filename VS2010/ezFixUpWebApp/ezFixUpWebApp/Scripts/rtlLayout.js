jQuery(document).ready(function ($) {
    if (false && jQuery(document.body).attr('dir') == 'rtl') {
        jQuery('body').css({ 'fontSize': '13px', 'lineHeight': '17px' });
        jQuery('#wrap').css({ 'textAlign': 'right', 'direction': 'rtl' });
        jQuery('.tar').css({ 'textAlign': 'left' });
        jQuery('.tal').css({ 'textAlign': 'right' });
        //jQuery('#main').css({ 'float': 'left', 'margin': '0 0 0 1%' });
        //jQuery('#sidebar').css({ 'float': 'right', 'margin': '0 1% 0 0' });
        jQuery('ul#navi li').css({ 'float': 'right' });
        jQuery('.fl').css({ 'float': 'right' });
        jQuery('.fr').css({ 'float': 'left' });

        changecss('.SideMenuBoxContent', 'font-size', '13px');
        
        changecss('div.labelHdr', 'float', 'right !important');
        changecss('label.customUnchecked', 'background-position', 'right -1px !important');
        changecss('label.customChecked', 'background-position', 'right -81px !important');
        changecss('label.radioUnchecked', 'background-position', 'right -161px !important');
        changecss('label.radioChecked', 'background-position', 'right -241px !important');

        changecss('div.logout', 'float', 'left !important');
        changecss('div.labelHdr a', 'float', 'left !important');
        changecss('#registration_rt', 'float', 'left !important');

        changecss('.plus1', 'background-position', 'right center !important');
        changecss('.minus1', 'background-position', 'right center !important');
        changecss('div.expander a', 'padding', '0 18px 0 0');
        changecss('span.customStyleSelectBoxInner', 'background-position', 'left center !important');
        changecss('span.customStyleSelectBoxInner', 'text-align', 'right');
        changecss('select', 'background-position', 'left top !important');

        changecss('.logo_header > tr', 'text-align', 'right');
        changecss('#fmt ul li ul.fmtDrpDwn', 'left', '-100%');
        changecss('#fmt ul li ul.fmtDrpDwn ul', 'left', '-100%');
        //    changecss('div.language', 'text-align', 'right !important');
        //    changecss('div.SectionContent', 'text-align', 'right !important');
        //    changecss('.SideMenuWebpartContent', 'text-align', 'right !important');
        //    changecss('table .ItemOfSearchResults1 td', 'text-align', 'right !important');
        //    changecss('.SideMenuContent', 'text-align', 'right !important');
        //    changecss('div.SideMenuBoxContent', 'text-align', 'right !important');
        //    changecss('.BoxHeader', 'text-align', 'right !important');
        //    changecss('.tar', 'text-align', 'left !important');
        //    changecss('.tal', 'text-align', 'right !important');

        //    changecss('.SideMenuContent', 'float', 'right !important');
        //    changecss('.SideMenuBoxContent', 'float', 'right !important');
        //    changecss('div#pnlLanguage', 'float', 'right !important');
        //    changecss('#registration_lt', 'float', 'right !important');
        //    changecss('div#pnlLanguage', 'float', 'right !important');
        //    changecss('#main .StandardWebpartsWrap', 'float', 'right !important');
        //    changecss('#sidebar .StandardWebpartsWrap', 'float', 'right !important');
        //    changecss('#sidebar', 'float', 'right !important');
        //    changecss('#sidebar', 'margin', '0 1% 0 0 !important');
        //    changecss('ul#navi li', 'float', 'right !important');
        //    changecss('div.labelHdr', 'float', 'right !important');
        //    //changecss('label.prettyCheckbox span.holderWrap','float', 'right !important'); 
        //    //changecss('label.prettyCheckbox','float', 'right !important'); 
        //    changecss('label.customUnchecked', 'background-position', 'right -1px !important');
        //    changecss('label.customChecked', 'background-position', 'right -81px !important');
        //    changecss('label.radioUnchecked', 'background-position', 'right -161px !important');
        //    changecss('label.radioChecked', 'background-position', 'right -241px !important');
        //    changecss('.fl', 'float', 'right !important');
        //    changecss('.fr', 'float', 'left !important');

        //    changecss('div.logout', 'float', 'left !important');
        //    changecss('div.labelHdr a', 'float', 'left !important');
        //    changecss('#registration_rt', 'float', 'left !important');
        //    changecss('#main', 'float', 'left !important');
        //    changecss('#main', 'margin', '0 0 0 1% !important');

        //    changecss('.plus1', 'background-position', 'right center !important');
        //    changecss('.minus1', 'background-position', 'right center !important');
        //    changecss('div.expander a', 'padding', '0 18px 0 0');
        //    changecss('span.customStyleSelectBoxInner', 'background-position', 'left center !important');
        //    changecss('span.customStyleSelectBoxInner', 'text-align', 'right');
        //    changecss('select', 'background-position', 'left top !important');

        //    changecss('.logo_header > tr', 'text-align', 'right');

        //    changecss('#fmt ul li ul.fmtDrpDwn', 'left', '-100%');
        //    changecss('#fmt ul li ul.fmtDrpDwn ul', 'left', '-100%');
    }
    else if (false && jQuery(document.body).attr('dir') == 'ltr') {
        jQuery('body').css({ 'fontSize': '11px', 'lineHeight': '15px' });
        jQuery('#wrap').css({ 'text-align': 'left', 'direction': 'ltr' });
        jQuery('.tar').css({ 'text-align': 'right' });
        jQuery('.tal').css({ 'text-align': 'left' });
        //jQuery('#main').css({ 'float': 'right', 'margin': '0 1% 0 0' });
        //jQuery('#sidebar').css({ 'float': 'left', 'margin': '0 0 0 1%' });
        jQuery('ul#navi li').css({ 'float': 'left' });
        jQuery('div#navi ul').first('li').css({ marginLeft: '12px !important', marginRight: '0px !important' });
        jQuery('.fl').css({ 'float': 'left' });
        jQuery('.fr').css({ 'float': 'right' });

        changecss('div.labelHdr', 'float', 'left !important');
        changecss('label.customUnchecked', 'background-position', 'left -1px !important');
        changecss('label.customChecked', 'background-position', 'left -81px !important');
        changecss('label.radioUnchecked', 'background-position', 'left -161px !important');
        changecss('label.radioChecked', 'background-position', 'left -241px !important');
        changecss('div.logout', 'float', 'right !important');
        changecss('div.labelHdr a', 'float', 'right !important');
        changecss('#registration_rt', 'float', 'right !important');

        changecss('.SideMenuBoxContent', 'font-size', '11px');
        changecss('.plus1', 'background-position', 'left center !important');
        changecss('.minus1', 'background-position', 'left center !important');
        changecss('div.expander a', 'padding', '0 0 0 18px');
        changecss('span.customStyleSelectBoxInner', 'background-position', 'right center !important');
        changecss('span.customStyleSelectBoxInner', 'text-align', 'left');
        changecss('select', 'background-position', 'right top !important');

        changecss('#fmt ul li ul.fmtDrpDwn', 'left', '100%');
        changecss('#fmt ul li ul.fmtDrpDwn ul', 'left', '100%');

        //    changecss('div.language', 'text-align', 'left !important');
        //    changecss('div.SectionContent', 'text-align', 'left !important');
        //    changecss('.SideMenuWebpartContent', 'text-align', 'left !important');
        //    changecss('table .ItemOfSearchResults1 td', 'text-align', 'left !important');
        //    changecss('.SideMenuContent', 'text-align', 'left !important');
        //    changecss('div.SideMenuBoxContent', 'text-align', 'left !important');
        //    changecss('.BoxHeader', 'text-align', 'left !important');
        //    changecss('.tar', 'text-align', 'right !important');
        //    changecss('.tal', 'text-align', 'left !important');

        //    changecss('.SideMenuContent', 'float', 'left !important');
        //    changecss('.SideMenuBoxContent', 'float', 'left !important');
        //    changecss('div#pnlLanguage', 'float', 'left !important');
        //    changecss('#registration_lt', 'float', 'left !important');
        //    changecss('div#pnlLanguage', 'float', 'left !important');
        //    changecss('#main .StandardWebpartsWrap', 'float', 'left !important');
        //    changecss('#sidebar .StandardWebpartsWrap', 'float', 'left !important');
        //    changecss('#sidebar', 'float', 'left !important');
        //    changecss('#sidebar', 'margin', '0 0 0 1%  !important');
        //    changecss('ul#navi li', 'float', 'left !important');
        //    changecss('div.labelHdr', 'float', 'left !important');
        //    //changecss('label.prettyCheckbox span.holderWrap','float', 'left !important'); 
        //    //changecss('label.prettyCheckbox','float', 'left !important'); 
        //    changecss('label.customUnchecked', 'background-position', 'left -1px !important');
        //    changecss('label.customChecked', 'background-position', 'left -81px !important');
        //    changecss('label.radioUnchecked', 'background-position', 'left -161px !important');
        //    changecss('label.radioChecked', 'background-position', 'left -241px !important');
        //    changecss('.fl', 'float', 'left !important');
        //    changecss('.fr', 'float', 'right !important');

        //    changecss('div.logout', 'float', 'right !important');
        //    changecss('div.labelHdr a', 'float', 'right !important');
        //    changecss('#registration_rt', 'float', 'right !important');
        //    changecss('#main', 'float', 'right !important');
        //    changecss('#main', 'margin', '0 1% 0 0 !important');

        //    changecss('.plus1', 'background-position', 'left center !important');
        //    changecss('.minus1', 'background-position', 'left center !important');
        //    changecss('div.expander a', 'padding', '0 0 0 18px');
        //    changecss('span.customStyleSelectBoxInner', 'background-position', 'right center !important');
        //    changecss('span.customStyleSelectBoxInner', 'text-align', 'left');
        //    changecss('select', 'background-position', 'right top !important');

        //    changecss('#fmt ul li ul.fmtDrpDwn', 'left', '100%');
        //    changecss('#fmt ul li ul.fmtDrpDwn ul', 'left', '100%');
    }
});