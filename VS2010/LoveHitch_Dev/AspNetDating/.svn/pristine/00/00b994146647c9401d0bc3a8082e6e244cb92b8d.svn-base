<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditPhotosCtrl" Src="EditPhotosCtrl.ascx" %>
<%@ Page language="c#" Codebehind="EditProfile.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditProfile" %>
<%@ Register TagPrefix="uc1" TagName="EditProfileCtrl" Src="EditProfileCtrl.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD runat="server">
		<title><%= Lang.TransA("Edit Profile") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" enctype="multipart/form-data" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
		    <div id="layout">
				<div id="crupms">
					<img src="images/i_manageuser.jpg"> 
					<div id="crumpstxt"> <%= Lang.TransA("Edit Profile") %></div>
				</div>
				<div id="sidebar"><uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu></div>
				<div id="main">
		        	<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Edit Profile") %></div>
						<div class="ch-description"><%= Lang.TransA("Use this section to edit profile...") %></div>
					</div>
		            <uc1:editprofilectrl id="EditProfileCtrl1" runat="server"></uc1:editprofilectrl>
				</div>
			</div>
		</form>
	</body>
</HTML>
