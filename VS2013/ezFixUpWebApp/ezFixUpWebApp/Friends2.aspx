<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Friends2.aspx.cs"
    Inherits="AspNetDating.Friends2"  Async="true" %>

<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults1.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Import Namespace="System.Linq" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" Namespace="AspNetDating.Components" %>
<%@ Register src="Components/Search/SearchGenderPicker.ascx" tagname="SearchGenderPicker" tagprefix="uc2" %>
<%@ Register src="Components/Search/AgeRangePicker.ascx" tagname="AgeRangePicker" tagprefix="uc3" %>
<%@ Register src="Components/SearchLocationPicker.ascx" tagname="SearchLocationPicker" tagprefix="uc4" %>
<%@ Register TagPrefix="mm" TagName="MatchToFriend" Src="Components/Matchmaker/MatchToFriend.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">
        jQuery(function($) {
            jQuery(".expandee div.expander").live('click', function() {//expands answers for questions
                $(this).next(".expandee").slideToggle('fast');
                $(this).find("a").toggleClass('plus1');
                $(this).find("a").toggleClass('minus1');
                //SetHeight();
                return false;
            });
            jQuery("a.topicexpander").live('click', function () {//expends questions for topic
                jQuery(this).next(".expandee").slideToggle('fast');
                //$(this).toggleClass('plus1');
                //$(this).toggleClass('minus1');
                //SetHeight();
                return false;
            });
            jQuery("a.expander").live('click', function () {//expends answers for questions of topic
                jQuery(this).next(".expandee").slideToggle('fast');
                jQuery(this).toggleClass('plus1');
                jQuery(this).toggleClass('minus1');
                //SetHeight();
                return false;
            });
            jQuery(".dropgender").change(function () {
                switch ($(this).val()) {
                    case "1":
                        $(".visibleformale").show();
                        $(".invisibleformale").hide();
                        break;
                    case "2":
                        $(".visibleforfemale").show();
                        $(".invisibleforfemale").hide();
                        break;
                    case "3":
                        $(".visibleforcouple").show();
                        $(".invisibleforcouple").hide();
                        break;
                }
                //SetHeight();
            });
        });
        function setAdvanced(isAdv) {
            var objId = "input#" + "<%= hfDisplayAdvancedSearch.ClientID%>";
            var state = jQuery(objId).attr('value',isAdv);
        }
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(PageLoadedEventHandler);
        function PageLoadedEventHandler() {
            jQuery(function($) {
                var objId = "input#" + "<%= hfDisplayAdvancedSearch.ClientID%>";
                var state = jQuery(objId).attr('value');
                if (state == 'True')
                    jQuery("div#advancedSearchAccordionDiv").show();
                else
                    jQuery("div#advancedSearchAccordionDiv").hide();
                jQuery('#fmt').addClass('js-isactive');
                jQuery('#fmt li').hover(
                    function() { $(this).addClass('selected'); $(this).find('ul.fmtDrpDwn:first').slideDown(200); },
                    function() { $(this).removeClass('selected'); $(this).find('ul.fmtDrpDwn:first').fadeOut(200); }
                );
            });
        }
    </script>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </asp:ScriptManagerProxy>
    <div id="sidebar">
        <mm:MatchToFriend ID="MatchToFriend1" runat="server"/>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
        <div class="SideMenuBoxInfo SearchFilterWrap">
            <table class="searchtable w100" cellpadding="0" cellspacing="0">
                <tr>
                    <td colspan="2">                        
                        <uc2:SearchGenderPicker ID="SearchGenderPicker1" runat="server" />
                    </td>
                </tr>                    
                <tr>
                    <td colspan="2">
                        <uc3:AgeRangePicker ID="AgeRangePicker1" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <uc4:SearchLocationPicker ID="SearchLocationPicker1" runat="server" />
                    </td>
                </tr>
                <tr >
	                <td>
	                    <table class="w100">
	                        <tr>
	                            <td class="w40"><%= Lang.Trans("Username") %></td>
		                        <td class="w60">
			                        <asp:TextBox ID="txtUsername" CssClass="m_search_txt" runat="server" ></asp:TextBox>
		                        </td>
		                    </tr>
		                </table>
		            </td>
	            </tr>
	            <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="cbPhotoReq" CssClass="checkbox" runat="server" Checked="False">
                        </asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="cbOnlineOnly" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="hfDisplayAdvancedSearch" runat="server" />
        <%--<div id="advancedSearchAccordionDiv"  >
            <asp:PlaceHolder ID="plhCustomSearch" runat="server" />
        </div>--%>
        <div>
        </div>
        <%--<div class="buttons" style="background: rgb(210,229,235) ;">--%>
        <div class="buttons">
            <uc1:FlexButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" RenderAs="LinkButton"
                CssClass="SearchBtn" />
            <%--<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/Colors2/search_btn.png" OnClick="btnSearch_Click" />--%>
        </div>
        <%--<div style="background: rgb(210,229,235) ; padding: 5px 0 5px 3px;">
            <asp:CheckBox ID="cbSaveSearch" onclick="$('#divSaveSearch').toggle(); //SetHeight();"
                runat="server" />
            <div id="divSaveSearch" style="display: none;">
                <asp:TextBox ID="txtSavedSearchName" runat="server" />
                <br />
                <asp:CheckBox ID="cbEmailSavedSearch" onclick="$('#divEmailFrequency').toggle(); //SetHeight();"
                    runat="server" />
                <div id="divEmailFrequency" style="display: none">
                    <asp:DropDownList CssClass="dropdownlist" ID="ddEmailFrequency" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
        </div>--%>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkQuickFriendsSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkFriendsRequests" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkShowFavorites" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkShowBlockedUsers" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkShowOnlineFriends" EventName="Click" />
                <asp:PostBackTrigger ControlID="SmallBoxStart1" />
            </Triggers>
            <ContentTemplate>
                <uc1:SideMenuStart ID="SmallBoxStart2" runat="server" ></uc1:SideMenuStart>
                <div id="fmt">
                    <ul class="fmt">
                        <li id="liShowFriends" runat="server">
                            <asp:LinkButton ID="lnkQuickFriendsSearch" runat="server" OnClick="lnkQuickSearch_Click"
                                OnClientClick="setAdvanced('False')" /></li>
                        <li id="liShowFriendsRequests" runat="server">
                            <asp:LinkButton ID="lnkFriendsRequests" runat="server" OnClick="lnkFriendsRequests_Click"
                                OnClientClick="setAdvanced('False')" /></li>
                        <li id="liShowSingleMaleFriends" runat="server"  visible="false">
                            <asp:LinkButton ID="lnkSingleMaleFriendsSearch" runat="server" OnClick="lnkQuickSearch_Click"
                                OnClientClick="setAdvanced('False')" /></li>
                        <li id="liShowSingleFemaleFriends" runat="server"  visible="false">
                            <asp:LinkButton ID="lnkSingleFemaleFriendsSearch" runat="server" OnClick="lnkQuickSearch_Click"
                                OnClientClick="setAdvanced('False')" /></li>
                        <li id="liOnlineFriends" Visible="false" runat="server">
                            <asp:LinkButton ID="lnkShowOnlineFriends" runat="server" OnClick="lnkShowOnlineFriends_Click" />
                        </li>
                        <li id="liShowFavorites" runat="server" >
                            <asp:LinkButton ID="lnkShowFavorites" runat="server" OnClick="lnkShowFavorites_Click"
                                OnClientClick="setAdvanced('False')" />
                        </li>
                        <li id="panelShowBlockedUsers" runat="server">
                            <asp:LinkButton ID="lnkShowBlockedUsers" runat="server" OnClick="lnkShowBlockedUsers_Click"
                                OnClientClick="setAdvanced('False')" />
                        </li>
                    </ul>
                </div>
                <uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server"></uc1:SmallBoxEnd>
                <div class="clear">
                </div>
                <div style="height:200px;position:static;">
                    <components:BannerView ID="bvSearchLeftBottom" runat="server" Key="SearchLeftBottom">
                    </components:BannerView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <div id="main">
            <style type="text/css">
                div .StandardBoxContent{padding:0;}
            </style>
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <uc1:SearchResults ID="SearchResults" runat="server" PaginatorEnabled="True" 
                EnableGridSupport="true"></uc1:SearchResults>
            <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
        </div>
    </div>
</asp:Content>
