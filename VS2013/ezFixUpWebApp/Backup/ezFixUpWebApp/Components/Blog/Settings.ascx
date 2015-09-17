<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="Settings.ascx.cs" Inherits="ezFixUp.Components.Blog.SettingsCtrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Import namespace="ezFixUp.Classes"%>
<uc1:largeboxstart id="LargeBoxStart" runat="server"></uc1:largeboxstart>
<uc1:headerline id="hlBlogSettings" runat="server"></uc1:headerline>
<div class="wrap-sized">
	<div class="srvmsg"><asp:Label ID="lblError" CssClass="error" Runat="server"></asp:Label></div>
    <div class="clear"></div>
	<%= Lang.Trans("Title") %>
	<br/>
	<asp:textbox id="txtName" CssClass="titletextbox" Runat="server"></asp:textbox>
    <div class="separator"></div>
	<%= Lang.Trans("Content") %>
	<br/>
	<asp:textbox id="txtDescription" CssClass="multilinetextbox" TextMode="MultiLine" Runat="server"></asp:textbox>
</div>
<br/>
<div class="buttons">
	<asp:Button Runat="server" id="btnSaveChanges" onclick="btnSaveChanges_Click"></asp:Button>
</div>
<uc1:largeboxend runat="server" id="Largeboxend1"></uc1:largeboxend>
