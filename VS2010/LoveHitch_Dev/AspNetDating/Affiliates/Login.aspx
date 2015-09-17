<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AspNetDating.Affiliates.Login" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title><%= Lang.Trans("Login") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	<form id="Form1" method="post" runat="server">
	<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
	<div id="layout">
			    <center>
			    <table id="loginbox" border=0 cellspacing="0" cellpadding="0">
					<tr>
						<td colspan="2" class="loginbox-head"><%= Lang.Trans("Authorize") %></td>
				    </tr>
				    <tr>
						<td class="loginbox-label"><%= Lang.Trans("Username") %></td>
						<td class="loginbox-txt"><asp:TextBox id="txtUsername" runat="server"></asp:TextBox></td>
				   	</tr>
				    <tr>
						<td class="loginbox-label"><%= Lang.Trans("Password") %></td>
					    <td class="loginbox-txt"><asp:TextBox id="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
				    </tr>
				    <tr>
						<td colspan="2" class="add-buttons">
							<asp:Button id="btnLogin" runat="server" OnClick="btnLogin_Click"></asp:Button>
					    </td>
				    </tr>
				    <tr>
						<td colspan="2">
							<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>					    
						</td>
				    </tr>
				</table>
 	            </center>
		    </div>
		</form>
	</body>
</html>
