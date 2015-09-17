<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroupEvents.aspx.cs"
    Inherits="ezFixUp.ShowGroupEvents" %>

<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditEvent" Src="~/Components/Groups/EditEvent.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewEvents" Src="~/Components/Groups/ViewEvents.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewEvent" Src="~/Components/Groups/ViewEvent.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="sidebar">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkGroupHome" runat="server" CssClass="SideMenuLink" OnClick="lnkGroupHome_Click"></asp:LinkButton></div>
        <div id="pnlGroupGallery" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkGroupGallery" runat="server" CssClass="SideMenuLink" OnClick="lnkGroupGallery_Click"></asp:LinkButton></div>
        <div id="pnlGroupMembers" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkGroupMembers" runat="server" CssClass="SideMenuLink" OnClick="lnkGroupMembers_Click"></asp:LinkButton></div>
        <div id="pnlMessageBoard" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkMessageBoard" runat="server" CssClass="SideMenuLink" OnClick="lnkMessageBoard_Click"></asp:LinkButton></div>
        <div id="pnlGroupEvents" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkGroupEvents" runat="server" CssClass="SideMenuLink" OnClick="lnkGroupEvents_Click"></asp:LinkButton></div>
        <div id="pnlAjaxChat" runat="server" class="SideMenuItem">
            <asp:HyperLink ID="lnkOpenAjaxChat" runat="server" CssClass="SideMenuLink"></asp:HyperLink></div>
        <div id="pnlNavLine" runat="server" class="navline">
        </div>
        <div id="pnlAddEvent" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkAddEvent" runat="server" CssClass="SideMenuLink" OnClick="lnkAddEvent_Click"></asp:LinkButton></div>
        <div id="pnlEditEvent" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkEditEvent" runat="server" CssClass="SideMenuLink" OnClick="lnkEditEvent_Click"></asp:LinkButton></div>
        <div id="pnlDeleteEvent" runat="server" class="SideMenuItem">
            <asp:LinkButton ID="lnkDeleteEvent" runat="server" CssClass="SideMenuLink" OnClick="lnkDeleteEvent_Click"></asp:LinkButton></div>
        <div id="divAddThis" visible="false" runat="server" class="SideMenuItem BookmarkLink">
        </div>
        <div class="navline">
        </div>
        <div class="SideMenuItem" id="pnlBrowseGroups" runat="server">
            <asp:LinkButton ID="lnkBrowseGroups" runat="server" CssClass="SideMenuLink" OnClick="lnkBrowseGroups_Click"></asp:LinkButton></div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
    </div>
    <div id="main">
        <asp:MultiView ID="mvGroupEvents" runat="server">
            <asp:View ID="viewEvents" runat="server">
                <uc1:ViewEvents ID="ViewEvents1" runat="server" />
            </asp:View>
            <asp:View ID="viewEdit" runat="server">
                <uc1:EditEvent ID="EditEvent1" runat="server"></uc1:EditEvent>
            </asp:View>
            <asp:View ID="viewEvent" runat="server">
                <uc1:ViewEvent ID="ViewEvent1" runat="server"></uc1:ViewEvent>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
