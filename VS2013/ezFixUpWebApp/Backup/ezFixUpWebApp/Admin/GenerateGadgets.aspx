<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Page language="c#" Codebehind="GenerateGadgets.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Admin.GenerateGadgets" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Generate Gadgets") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_gadgets.jpg" /> 
					<div id="crumpstxt"><%= Lang.TransA("Vista Sideshow Gadgets") %></div>
				</div>
				<div id="sidebar">
					<uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				</div>
			    <div id="main">
		        	<div id="content-head">
			        	<div class="ch-head"><%= Lang.TransA("Generate Gadgets") %></div>
			            <div class="ch-description">
			            	<%= Lang.TransA("Use this section to generate and download Vista Sideshow Gadgets") %>
			            </div>
		            </div>
           			<uc1:MessageBox id="Messagebox1" runat="server"></uc1:MessageBox>
				    <table cellSpacing="0" cellPadding="0" class="stable2">
					    <tr>
						    <td class="table_header" align="center" colSpan="2"><%= Lang.TransA("Admin Stats Gadget") %></td>
					    </tr>
					    <tr>
						    <td class="table_cell" align="center" colSpan="2"><asp:Button id="btnGenerateAdminStatsGadget" Runat="server" OnClick="btnGenerateAdminStatsGadget_Click"></asp:Button></td>
					    </tr>
				    </table>
				    <div class="separator10"></div>
				</div>
			</div>
		</form>
	</body>
</HTML>
