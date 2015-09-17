<%@ Import namespace="AspNetDating"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewMembers.ascx.cs" Inherits="AspNetDating.Components.Groups.NewMembers" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<asp:DataList ID="dlGroupMembers" CssClass="UsersBoxContent" Runat="server" SkinId="GroupMembers" Width="100%"
	RepeatDirection="Horizontal" RepeatLayout="Table" GridLines="None" CellSpacing="2" CellPadding="0">
	<ItemTemplate>
		<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'>
			<%# ImageHandler.RenderImageTag((int)Eval("ImageID"), 90, 90, "photoframe", false, true, true) %></a><div class="clear"></div>
			<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# DataBinder.Eval(Container, "DataItem.Username") %></a>
	</ItemTemplate>
</asp:DataList>
<div id="pnlMore" runat="server" class="morelink" visible="false">
	<asp:LinkButton ID="lnkMore" runat="server" OnClick="lnkMore_Click"></asp:LinkButton>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>		