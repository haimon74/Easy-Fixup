<%@ Page language="c#" Codebehind="Login.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.Login" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%= Lang.TransA("Login") %></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
	<form id="Form1" method="post" runat="server">
	<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
	<div id="layout">
			    <center>
			    <table id="loginbox" border=0 cellspacing="0" cellpadding="0" border="0">
					<tr>
						<td colspan="2" class="loginbox-head"><%= Lang.TransA("Authorize") %></td>
				    </tr>
				    <tr>
						<td class="loginbox-label"><%= Lang.TransA("Username") %></td>
						<td class="loginbox-txt"><asp:TextBox id="txtUsername" runat="server"></asp:TextBox></td>
				   	</tr>
				    <tr>
						<td class="loginbox-label"><%= Lang.TransA("Password") %></td>
					    <td class="loginbox-txt"><asp:TextBox id="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
				    </tr>
				    <tr>
						<td class="loginbox-label"><%= Lang.TransA("Language") %></td>
					    <td class="loginbox-txt"><asp:DropDownList ID="ddLanguage" runat="server"></asp:DropDownList></td>				        
				    </tr>
				    <tr>
						<td colspan="2" class="add-buttons">
							<asp:Button id="btnLogin" runat="server" Text="Button" onclick="btnLogin_Click"></asp:Button>
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
</HTML>
