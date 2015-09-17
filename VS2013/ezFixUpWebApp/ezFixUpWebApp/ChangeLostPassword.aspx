<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="ChangeLostPassword.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.ChangeLostPassword" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<asp:Content id="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
		<div class="srvmsg"><asp:label id="lblError" CssClass="InfoMessage" runat="server" Visible="false"></asp:label></div>
		<div class="center">
			<%= ezFixUp.Classes.Lang.Trans("New password") %><br /><br />
			<%= ezFixUp.Classes.Lang.Trans("Please enter a new password:") %><br /><br />
			<input id="txtNewPassword" type="password" runat="server" name="txtNewPassword" /><br /><br />
			<%= ezFixUp.Classes.Lang.Trans("Confirm your new password:") %><br /><br />
			<input id="txtConfirmedPassword" type="password" runat="server" name="txtConfirmedPassword" /><br /><br />
		</div>
		<div class="buttons"><asp:button id="btnSubmit" tabIndex="1" runat="server" onclick="btnSubmit_Click"></asp:button></div>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>
