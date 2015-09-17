<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ezFixUp.Affiliates.Home" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
	<head>
		<title> <%= Lang.Trans("Home") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AffiliateHeader id="AdminHeader1" runat="server"></uc1:AffiliateHeader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_home.jpg"> 
					<div id="crumpstxt"> <%= Lang.Trans("Welcome!") %></div>
				</div>
				<div id="sidebar">
					<uc1:AffiliateMenu id="Adminmenu1" runat="server"></uc1:AffiliateMenu>
				</div>
			    <div id="main">
					<div id="content-head">
						<div class="ch-head"><%= Lang.Trans("") %></div>
						<div class="ch-description"><%= Lang.Trans("") %></div>
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