<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NewPhotos.ascx.cs" Inherits="AspNetDating.Components.Groups.NewPhotos" %>

<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Import namespace="AspNetDating"%>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label></div>
<asp:DataList ID="dlGroupPhotos" CssClass="UsersBoxContent" Runat="server" SkinId="GroupNewPhotos" Width="100%"
	RepeatDirection="Horizontal" RepeatLayout="Table" GridLines="None" CellSpacing="2" CellPadding="0">
	<ItemTemplate>
        <img class="photoframe" src='GroupImage.ashx?gpid=<%# Eval("GroupPhotoID") %>&width=90&height=90&diskCache=1' /> 
        <div class="center usernamefix overflow">
        <%# Eval("Name") %><br />
        <a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"))%>'><%# Eval("Username") %></a></div>
	</ItemTemplate>
</asp:DataList>
<div id="pnlMore" runat="server" visible="false" class="morelink">
	<asp:LinkButton ID="lnkMore" runat="server" OnClick="lnkMore_Click"></asp:LinkButton>
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>