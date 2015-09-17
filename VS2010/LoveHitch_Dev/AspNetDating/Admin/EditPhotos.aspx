<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Page language="c#" Codebehind="EditPhotos.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditPhotos" %>
<%@ Register TagPrefix="uc1" TagName="EditPhotosCtrl" Src="EditPhotosCtrl.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit Photos") %></title>
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
				<div id="crupms">
					<img src="images/i_browsephotos.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Edit Photos") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
		        	<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Edit Photos") %></div>
						<div class="ch-description"><%= Lang.TransA("Use this section to edit") %> <%=Request.Params["username"]%><%= Lang.TransA("'s photos ...") %></div>			
					</div>
		            <uc1:EditPhotosCtrl id="EditPhotosCtrl1" runat="server"></uc1:EditPhotosCtrl>
			    </div>
			</div>
		</form>
	</body>
</HTML>
