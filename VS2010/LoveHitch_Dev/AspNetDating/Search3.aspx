<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Search3.aspx.cs"
    Inherits="AspNetDating.Search3" Async="true" %>

<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchBoxStart" Src="Components/SearchBoxStart.ascx" %>
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
<%@ Register Src="Components/Search/SearchGenderPicker.ascx" TagName="SearchGenderPicker"
    TagPrefix="uc2" %>
<%@ Register Src="Components/Search/AgeRangePicker.ascx" TagName="AgeRangePicker"
    TagPrefix="uc3" %>
<%@ Register Src="Components/SearchLocationPicker.ascx" TagName="SearchLocationPicker"
    TagPrefix="uc4" %>
<%@ Register TagPrefix="mm" TagName="MatchToFriend" Src="Components/Matchmaker/MatchToFriend.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <script type="text/javascript">
        var displayAdvanceSearchId = "input#" + "<%= hfDisplayAdvancedSearch.ClientID%>";
        asyncScripts.push('scripts/pages/Search3.js');
    </script>
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference Path="~/scripts/pages/Search3.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCountry" runat="server" TargetControlID="dropCountry"
        Category="Country" ServicePath="~/Services/Service.asmx" PromptText=" " ServiceMethod="GetCountries" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownState" runat="server" TargetControlID="dropRegion"
        Category="Region" ServicePath="~/Services/Service.asmx" PromptText=" " ServiceMethod="GetRegionsByCountry"
        ParentControlID="dropCountry" />
    <ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
        Category="City" ServicePath="~/Services/Service.asmx" 
        PromptText=" " ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" />
    <div id="sidebar">
        <mm:MatchToFriend ID="MatchToFriend1" runat="server" IsVisible="false" />
        <uc1:SearchBoxStart ID="SmallBoxStart1" runat="server"></uc1:SearchBoxStart>
        <div class="SearchFilterWrap">
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
                    <td>
                        <%= Lang.Trans("Search gender")%><br />
                        <asp:DropDownList ID="dropGender" runat="server" AutoPostBack="false" EnableViewState="true"
                            CssClass="m_search_ddl">
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <%= Lang.Trans("Interested in")%><br />
                        <asp:DropDownList ID="dropInterestedIn" runat="server" AutoPostBack="false" EnableViewState="true"
                            CssClass="m_search_ddl">
                            <asp:ListItem Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Lang.Trans("Country") %><br />
                        <asp:DropDownList ID="dropCountry" CssClass="m_search_ddl" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="w50">
                        <%= Lang.Trans("Region/State") %><br />
                        <asp:DropDownList ID="dropRegion" CssClass="m_search_ddl" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Lang.Trans("City") %><br />
                        <asp:DropDownList ID="dropCity" CssClass="m_search_ddl" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="white-space: nowrap; text-indent: 0; ">
                        <%= Lang.Trans("Age Range") %><br />
                        <span class="fl" style="padding-left: 1px;">
                            <asp:UpdatePanel ID="up1" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:DropDownList ID="comboAgeFrom" runat="server" AutoPostBack="false" OnSelectedIndexChanged="comboAgeFromSelectIndexChange"
                                        CssClass="s_search_ddl" EnableViewState="true">
                                        <asp:ListItem Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </span>
                        <%--<span style="display:inline;margin:0 4px"><b><%= Lang.Trans("~") %></b></span>--%>
                        <span class="fr" style="padding: 0 13px;">
                            <asp:UpdatePanel ID="up2" runat="server" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:DropDownList ID="comboAgeTo" runat="server" AutoPostBack="false" OnSelectedIndexChanged="comboAgeToSelectIndexChange"
                                        CssClass="s_search_ddl" EnableViewState="true">
                                        <asp:ListItem Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </span>
                    </td>
                </tr>
                <tr valign='top'>
                    <td><br />
                        <asp:CheckBox ID="cbPhotoReq" CssClass="checkbox" runat="server" Checked="False">
                        </asp:CheckBox>
                    </td>
                    <td><br />
                        <asp:CheckBox ID="cbOnlineOnly" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="w100">
                        <%= Lang.Trans("Username") %><br />
                        <asp:TextBox ID="txtUsername" CssClass="m_search_txt" runat="server"></asp:TextBox>
                    </td>
                    <td><br />
                        <uc1:FlexButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" RenderAs="LinkButton"
                            CssClass="medium red zurbtn fl" />
                    </td>
                </tr>
                
            </table>
        </div>
        <asp:HiddenField ID="hfDisplayAdvancedSearch" runat="server" />
        <div id="advancedSearchAccordionDiv">
            <asp:PlaceHolder ID="plhCustomSearch" runat="server" />
        </div>
        <div>
        </div>
        <%--<div class="buttons" style="background: rgb(210,229,235) ;">--%>
        <div class="buttons">
            
            <%--<asp:ImageButton ID="btnSearch" runat="server" ImageUrl="~/App_Themes/Colors2/search_btn.png" OnClick="btnSearch_Click" />--%>
        </div><br />
        <script type="text/javascript">
            jQuery("document").ready(function () {
                jQuery("document").on('keydown', '*', function () {
                    if (event.which || event.keyCode) {
                        if ((event.which == 13) || (event.keyCode == 13)) {
                            //event.preventDefault();
                            cancelEvent(event);
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
            <asp:CheckBox ID="cbSaveSearch" Visible="false" onclick="$('#divSaveSearch').toggle(); //SetHeight();"
                runat="server" />
            <div id="divSaveSearch" style="display: none;">
                <p>
                    <asp:TextBox ID="txtSavedSearchName" runat="server" CssClass="m_search_txt fr" Style="margin-left: 10px;
                        margin-right: 10px;" />
                </p>
                <br />
                <p style="padding: 5px 0 5px 3px;">
                    <asp:CheckBox ID="cbEmailSavedSearch" onclick="$('#divEmailFrequency').toggle(); //SetHeight();"
                        runat="server" />
                </p>
                <div id="divEmailFrequency" style="display: none;">
                    <table>
                        <tr>
                            <td class="w40">
                            </td>
                            <td class="w60">
                                <asp:DropDownList CssClass="m_search_ddl" ID="ddEmailFrequency" runat="server">
                                </asp:DropDownList>
                            </td>
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
                <uc1:SideMenuStart ID="SmallBoxStart2" runat="server"></uc1:SideMenuStart>
                <div id="fmt">
                    <ul class="fmt">
                        <li>
                            <asp:LinkButton ID="lnkQuickSearch" runat="server" OnClick="lnkQuickSearch_Click"
                                OnClientClick="setAdvanced('False')" /></li>
                        <li>
                            <asp:LinkButton ID="lnkAdvancedSearch" runat="server" OnClick="lnkAdvancedSearch_Click"
                                OnClientClick="setAdvanced('True')" />
                        </li>
                        <li id="liShowOnlineUsers" visible="false" runat="server">
                            <asp:LinkButton ID="lnkShowOnlineUsers" runat="server" OnClick="lnkShowOnlineUsers_Click" />
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkShowProfileViewers" runat="server" OnClick="lnkShowProfileViewers_Click" />
                        </li>
                        <li style='display:none;'>
                            <asp:LinkButton ID="lnkShowNewUsers" runat="server" Visible="false" OnClick="lnkShowNewUsers_Click" />
                        </li>
                    </ul>
                </div>
                <uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server"></uc1:SmallBoxEnd>
                <div class="clear">
                </div>
                <div style="height: 200px; position: static;">
                    <components:BannerView ID="bvSearchLeftBottom" runat="server" Key="SearchLeftBottom">
                    </components:BannerView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <div id="main">
            <style type="text/css">
                div .StandardBoxContent
                {
                    padding: 0;
                    width: 100%;
                }
            </style>
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <uc1:SearchResults ID="SearchResults" runat="server" PaginatorEnabled="True" EnableGridSupport="true">
            </uc1:SearchResults>
            <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
        </div>
    </div>
</asp:Content>
