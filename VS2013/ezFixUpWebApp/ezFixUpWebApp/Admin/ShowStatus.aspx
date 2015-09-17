<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowStatus.aspx.cs" Inherits="ezFixUp.Admin.ShowStatus" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title> <%= Lang.TransA("Home") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_home.jpg"> 
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="Adminmenu1" runat="server"></uc1:AdminMenu>
				</div>
			    <div id="main">
					<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("") %></div>
						<div class="ch-description"><%= Lang.TransA("") %></div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<table cellpadding="0" cellspacing="0" width="100%">
						<tr>
							<td></td>
						</tr>
					</table>
				</div>
			</div>
		</form>
	</body>
</html>
