<%@ Import namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewEvents.ascx.cs" Inherits="ezFixUp.Components.Groups.NewEvents" %>

<%@ Import namespace="ezFixUp"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<asp:Repeater ID="rptGroupEvents" Runat="server">
	<ItemTemplate>
		<table cellpadding="0" cellspacing="0" width="100%">
			<tr>
				<td valign="top"  width="60">
				    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'>
				        <img src='GroupEventImage.ashx?id=<%# Eval("ID") %>&width=50&height=50&diskCache=1' class="photoframe" border="0" alt=""/>
				    </a>
				</td>
    			<td valign="top">
			 	    <a href='<%# UrlRewrite.CreateShowGroupEventsUrl((string) Eval("GroupID"), (string) Eval("ID")) %>'><%# Eval("Title") %></a><div class="clear"></div>
				    <b><%= Lang.Trans("Event date:") %></b>&nbsp;<%# Eval("Date") %>
				    <div class="clear"></div>
				    <b><%= Lang.Trans("Created by:") %></b>&nbsp;<a href='ShowUser.aspx?uid=<%# Eval("Username") %>'><%# Eval("Username") %></a>
				    <div class="clear"></div>
						    <asp:Label ID="lblAttenders" runat="server" Text='<%# "<b>" + Lang.Trans("Attending:") + "</b>&nbsp;" + Eval("Attenders") %>' Visible='<%# (string) Eval("Attenders") != "0" %>'></asp:Label>
				</td>
			</tr>
		</table>   
	</ItemTemplate>
	<SeparatorTemplate>
		<div class="separator6"></div>
		<div class="line"></div>
		<div class="separator6"></div>
	</SeparatorTemplate>
</asp:Repeater>
<div id="pnlMore" runat="server" class="morelink" visible="false">
	<asp:LinkButton ID="lnkMore" runat="server" OnClick="lnkMore_Click"></asp:LinkButton>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>