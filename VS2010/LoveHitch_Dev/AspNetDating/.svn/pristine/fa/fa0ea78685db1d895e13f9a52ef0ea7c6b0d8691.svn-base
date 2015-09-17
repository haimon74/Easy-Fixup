<%@ Register TagPrefix="uc1" TagName="WideBoxStart" Src="Components/WideBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="WideBoxEnd" Src="Components/WideBoxEnd.ascx" %>
<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="LostPassword.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.LostPassword" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<uc1:WideBoxStart id="WideBoxStart1" runat="server"></uc1:WideBoxStart>
		<div class="srvmsg"><asp:Label id="lblError" CssClass="InfoMessage" runat="server" Visible="false"></asp:Label></div>
	    <div class="center" >
	        <p style="font-size:x-large;color:teal;"><b><%= Lang.Trans("Forgot Your Password?") %></b></p>
	        <p style="font-size:small;color:teal;">
		    <%= Lang.Trans("Please enter the email address given during the"+
			    " registration in order to change your password") %><br /><br />
		    </p>
		    <asp:TextBox id="txtEmail" runat="server" Width="250px"></asp:TextBox>
		    <br /><br />
		    
	    </div>
		<div class="buttons">
		    <asp:Button id="btnSubmit" tabIndex="1" runat="server" CssClass="recoverPasswordBtn"
		        onclick="btnSubmit_Click" />
		</div>
	<uc1:WideBoxEnd id="WideBoxEnd1" runat="server"></uc1:WideBoxEnd>
</asp:Content>