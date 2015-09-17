<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveBlogPost.aspx.cs" Inherits="ezFixUp.Admin.ApproveBlogPost" %>

<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Approve Blog Post") %></title>
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
					<img src="images/i_approveanswers.jpg"> 
					<div id="crumpstxt"> <%= Lang.TransA("Approve Blog Post")%></div>
				</div>
				<div id="sidebar"><uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu></div>
				<div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Approve Blog Post")%></div>
						<div class="ch-description">
							<%= Lang.TransA("Use this section to approve pending blog posts...") %>
						</div>
					</div>
					<table border="0" cellpadding="0" cellspacing="0" id="stable">
						<tr>
							<td class="table_header" colspan="2"><%= Lang.TransA("Approve Blog Post")%></td>
						</tr>
						<tr>
							<td class="table_cell" width="20%"><%= Lang.TransA("Username") %>:</td>
							<td class="table_cell" width="80%"><asp:Label id="lblUsername" runat="server"></asp:Label></td>
						</tr>
						<tr>
							<td class="table_cell" width="20%"><%= Lang.TransA("Blog post title") %>:</td>
							<td class="table_cell" width="80%"><asp:TextBox CssClass="textbox" Width="100%" Rows="5" id="txtTitle" runat="server" TextMode="MultiLine"></asp:TextBox></td>
						</tr>
						<tr>
							<td class="table_cell" width="20%"><%= Lang.TransA("Blog post content") %>:</td>
							<td class="table_cell" width="80%"><asp:TextBox CssClass="textbox" Width="100%" Rows="5" id="txtContent" runat="server" TextMode="MultiLine"></asp:TextBox></td>
						</tr>
						<tr>
							<td colspan="2" class="table_cell">
								<div class="add-buttons">
									<asp:Button id="btnSaveAndApprove" runat="server" onclick="btnSaveAndApprove_Click"></asp:Button>
									<asp:Button id="btnReject" runat="server" onclick="btnReject_Click"></asp:Button>
								</div>
							</td>
						</tr>
					</table>
					<div class="separator06"></div>
					<div class="add-buttons2">
						<div class="add-buttons">
							<asp:Button id="btnCancel" runat="server" onclick="btnCancel_Click"></asp:Button>	
						</div>
					<div class="separator06"></div>
					</div>
				</div>
			</div>
		</form>
	</body>
</HTML>
