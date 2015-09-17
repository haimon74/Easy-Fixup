<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InviteFriends.ascx.cs" Inherits="ezFixUp.Components.Groups.InviteFriends" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
<div class="clear"></div>
<div class="wrpa-sized">
	<div id="pnlInviteFriends" runat="server">
	    <%= Lang.Trans("Enter your friends' nicknames:") %>
	    <br />
	    <asp:TextBox ID="txtFriend1" runat="server"></asp:TextBox>
	    <asp:TextBox ID="txtFriend2" runat="server"></asp:TextBox>
	    <asp:TextBox ID="txtFriend3" runat="server"></asp:TextBox>
	    <asp:TextBox ID="txtFriend4" runat="server"></asp:TextBox>
	    <asp:TextBox ID="txtFriend5" runat="server"></asp:TextBox>
	    <asp:TextBox ID="txtFriend6" runat="server"></asp:TextBox>
	</div>
</div>
<div class="separator"></div>
<div class="buttons">
    <asp:Button ID="btnSend" runat="server" OnClick="btnSend_Click" />
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>