<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroup.aspx.cs" Inherits="AspNetDating.ShowGroup" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewGroup" Src="~/Components/Groups/ViewGroup.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupMembers" Src="~/Components/Groups/GroupMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="PendingMembers" Src="~/Components/Groups/PendingMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditGroup" Src="~/Components/Groups/EditGroup.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SendAnnouncement" Src="~/Components/Groups/SendAnnouncement.ascx" %>
<%@ Register TagPrefix="uc1" TagName="InviteFriends" Src="~/Components/Groups/InviteFriends.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupBans" Src="~/Components/Groups/GroupBans.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar"><uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="SideMenuItem" id="pnlGroupHome" runat="server"><asp:linkbutton id="lnkGroupHome" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupHome_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupGallery" runat="server"><asp:linkbutton id="lnkGroupGallery" Runat="server" CssClass="SideMenuLink"  OnClick="lnkGroupGallery_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupMembers" runat="server"><asp:linkbutton id="lnkGroupMembers" Runat="server" CssClass="SideMenuLink"  OnClick="lnkGroupMembers_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlMessageBoard" runat="server"><asp:linkbutton id="lnkMessageBoard" Runat="server" CssClass="SideMenuLink" OnClick="lnkMessageBoard_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupEvents" runat="server"><asp:linkbutton id="lnkGroupEvents" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupEvents_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlAjaxChat" runat="server"><asp:HyperLink id="lnkOpenAjaxChat" Runat="server" CssClass="SideMenuLink"></asp:HyperLink>
		</div>
		<div class="navline" id="pnlNavLine" runat="server"></div>
		<div class="SideMenuItem" id="pnlInviteFriends" runat="server"><asp:linkbutton id="lnkInviteFriends" Runat="server" CssClass="SideMenuLink" OnClick="lnkInviteFriends_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlPendingMembers" runat="server"><asp:linkbutton id="lnkPendingMembers" Runat="server" CssClass="SideMenuLink" OnClick="lnkPendingMembers_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlManageGroup" runat="server"><asp:linkbutton id="lnkManageGroup" Runat="server" CssClass="SideMenuLink" OnClick="lnkManageGroup_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupBans" runat="server"><asp:linkbutton id="lnkGroupBans" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupBans_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlSendAnnouncement" runat="server"><asp:linkbutton id="lnkSendAnnouncement" Runat="server" CssClass="SideMenuLink" OnClick="lnkSendAnnouncement_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlJoinGroup" runat="server"><asp:linkbutton id="lnkJoinGroup" Runat="server" CssClass="SideMenuLink" OnClick="lnkJoinGroup_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlLeaveGroup" runat="server"><asp:linkbutton id="lnkLeaveGroup" Runat="server" CssClass="SideMenuLink" OnClick="lnkLeaveGroup_Click"></asp:linkbutton>
		</div>
        <div id="divAddThis" visible="false" runat="server" class="SideMenuItem BookmarkLink">
        </div>    
		<div class="navline"></div>
		<div class="SideMenuItem" id="pnlBrowseGroups" runat="server"><asp:linkbutton id="lnkBrowseGroups" Runat="server" CssClass="SideMenuLink" OnClick="lnkBrowseGroups_Click"></asp:linkbutton>
		</div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <div class="srvmsg"><asp:Label ID="lblError" runat="server" EnableViewState="False"></asp:Label></div>
        <asp:MultiView ID="mvGroup" runat="server">
            <asp:View ID="viewGroupHome" runat="server">
                <uc1:ViewGroup ID="ViewGroup1" runat="server" />
            </asp:View>
            <asp:View ID="viewGroupMembers" runat="server">
                <uc1:GroupMembers ID="GroupMembers1" runat="server" />
            </asp:View>
            <asp:View ID="viewPendingMembers" runat="server">
                <uc1:PendingMembers ID="PendingMembers1" runat="server" />
            </asp:View>
            <asp:View ID="viewManageGroup" runat="server">
                <uc1:EditGroup ID="EditGroup1" runat="server" />
            </asp:View>
            <asp:View ID="viewGroupBans" runat="server">
                <uc1:GroupBans id="GroupBans1" runat="server"></uc1:GroupBans>
            </asp:View>
            <asp:View ID="viewSendAnnouncement" runat="server">
                <uc1:SendAnnouncement id="SendAnnoucement1" runat="server"></uc1:SendAnnouncement>
            </asp:View>
            <asp:View ID="viewInviteFriends" runat="server">
                <uc1:InviteFriends id="InviteFriends1" runat="server"></uc1:InviteFriends>
            </asp:View>
            <asp:View ID="viewJoinGroup" runat="server">
                <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
                <div class="srvmsg"><asp:Label ID="lblJoinToGroupMessage" runat="server" EnableViewState="False"></asp:Label></div>
                <div class="buttons">
	                <asp:Button ID="btnJoinToGroup" runat="server" OnClick="btnJoinToGroup_Click" />
                </div>
                <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
