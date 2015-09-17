<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Search2.aspx.cs"
    Inherits="AspNetDating.Search2"  Async="true" %>

<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchResults" Src="Components/Search/SearchResults1.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="uc1" Namespace="AspNetDating.Components" %>
<%@ Register src="Components/Search/SearchGenderPicker.ascx" tagname="SearchGenderPicker" tagprefix="uc2" %>
<%@ Register src="Components/Search/AgeRangePicker.ascx" tagname="AgeRangePicker" tagprefix="uc3" %>
<%@ Register src="Components/SearchLocationPicker.ascx" tagname="SearchLocationPicker" tagprefix="uc4" %>
<%@ Register TagPrefix="mm" TagName="MatchToFriend" Src="Components/Matchmaker/MatchToFriend.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">
        var displayAdvanceSearchId = "input#" + "<%= hfDisplayAdvancedSearch.ClientID%>";
        asyncScripts.push('scripts/pages/search2.js');
    </script>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/scripts/pages/search2.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <div id="sidebar">
        <mm:MatchToFriend ID="MatchToFriend1" runat="server" IsVisible="false"/>
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
        <div class="SideMenuBoxInfo SearchFilterWrap">
            <a id="lnkSavedSearches" visible="false" href="#" onclick="return false;" class="expander plus1"
                runat="server">
                <%= "Saved searches".Translate() %></a>
            <div class="expandee" style="display: none">
                <asp:DataList ID="dlSavedSearches" OnItemCommand="dlSavedSearches_ItemCommand" runat="server"
                    Width="100%">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSavedSearch" runat="server" CommandName="SavedSearchView"
                            Text='<%# Eval("Name") %>' CommandArgument='<%# Eval("ID") %>'>
                        </asp:LinkButton>
                        <asp:LinkButton ID="lnkDeleteSavedSearch" runat="server" CommandName="SavedSearchDelete"
                            Text="[x]" CommandArgument='<%# Eval("ID") %>'>
                        </asp:LinkButton>
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <br />
                    </SeparatorTemplate>
                </asp:DataList>
            </div>
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
        <div id="advancedSearchAccordionDiv"  >
            <asp:PlaceHolder ID="plhCustomSearch" runat="server" />
        </div>
        <div>
        </div>
        <%--<div class="buttons" style="background: rgb(210,229,235) ;">--%>
        <div class="buttons">
            <uc1:FlexButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" RenderAs="LinkButton"
                CssClass="SearchBtn" />
            <%--<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/Colors2/search_btn.png" OnClick="btnSearch_Click" />--%>
        </div>
        <script type="text/javascript">
            jQuery("document").ready(function() {
                jQuery("document").on('keydown', '*', function() {
                    if (event.which || event.keyCode) {
                        if ((event.which == 13) || (event.keyCode == 13)) {
                            event.preventDefault();
                            var fireOnThis = document.getElementById("<%= btnSearch.ClientID%>");
                            var evObj = document.createEvent('MouseEvents');
                            //debugger;
                            //evObj.initMouseEvent('click', true, true, window, 1, 12, 345, 7, 220, false, false, true, false, 0, null);
                            evObj.initEvent('click', true, true);
                            fireOnThis.dispatchEvent(evObj);
                        }
                    }
                });
            });
        </script>

        <%--<div style="background: rgb(210,229,235) ; padding: 5px 0 5px 3px;">--%>
        <div style="padding: 5px 0 5px 3px;">
            <asp:CheckBox ID="cbSaveSearch" onclick="$('#divSaveSearch').toggle(); //SetHeight();"
                runat="server" />
            <div id="divSaveSearch" style="display: none;">
                <p>
                    <asp:TextBox ID="txtSavedSearchName" runat="server" CssClass="m_search_txt fr"
                        style="margin-left:10px;margin-right:10px;" />
                </p>
                <br />
                <p style="padding:5px 0 5px 3px;">
                    <asp:CheckBox ID="cbEmailSavedSearch" onclick="$('#divEmailFrequency').toggle(); //SetHeight();" runat="server" />
                </p>
                <div id="divEmailFrequency" style="display: none;">
                    <table>
                        <tr>
                        <td class="w40"></td>
                        <td class="w60"><asp:DropDownList CssClass="m_search_ddl" ID="ddEmailFrequency" runat="server">
                        </asp:DropDownList></td>
                        </tr>
                        
                    </table>
                </div>
            </div>
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkQuickSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkAdvancedSearch" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkShowProfileViewers" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="lnkShowNewUsers" EventName="Click" />
                <asp:PostBackTrigger ControlID="SmallBoxStart1" />
            </Triggers>
            <ContentTemplate>
                <uc1:SideMenuStart ID="SmallBoxStart2" runat="server" ></uc1:SideMenuStart>
                <div id="fmt">
                    <ul class="fmt">
                        <li>
                            <asp:LinkButton ID="lnkQuickSearch" runat="server" OnClick="lnkQuickSearch_Click"
                                OnClientClick="setAdvanced('False')" /></li>
                        <li>
                            <asp:LinkButton ID="lnkAdvancedSearch" runat="server" OnClick="lnkAdvancedSearch_Click"
                                OnClientClick="setAdvanced('True')" />
                        </li>
                        <li id="liShowOnlineUsers" Visible="false" runat="server">
                            <asp:LinkButton ID="lnkShowOnlineUsers" runat="server" OnClick="lnkShowOnlineUsers_Click" />
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkShowProfileViewers" runat="server" OnClick="lnkShowProfileViewers_Click"/>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkShowNewUsers" runat="server" OnClick="lnkShowNewUsers_Click"/>
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
                div .StandardBoxContent{padding:0;width:100%;}
            </style>
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <uc1:SearchResults ID="SearchResults" runat="server" PaginatorEnabled="True" 
                EnableGridSupport="true"></uc1:SearchResults>
            <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
        </div>
    </div>
</asp:Content>
