<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditGoogleAnalytics.aspx.cs" Inherits="AspNetDating.Admin.EditGoogleAnalytics" %>

<%@ Import Namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit Google Analytics") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
	                <img src="images/i_editpages.jpg" /> 
					<div id="crumpstxt"><%= Lang.TransA("Edit Google Analytics")%></div>
			    </div>
				<div id="sidebar">
		            <uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				</div>
			    <div id="main">
			    	<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Edit Google Analytics")%></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to edit google anlytics")%></div>	
					</div>
					<uc1:messagebox id="Messagebox1" runat="server"></uc1:messagebox>				
					    <table cellSpacing="0" id="stable" cellPadding="0">
						    <tr>
							    <td class="table_header" align="center" colSpan="2"><%= Lang.TransA("Google Analytics")%></td>
						    </tr>
						    <tr>
							    <td class="table_cell" align="center" colSpan="2"><%= Lang.TransA("Paste your google analytics tracking code in the textbox below") %></td>
						    </tr>
						    <tr>
						        <td class="table_cell" align="left">
						            <asp:textbox TextMode="MultiLine" Rows="20" Columns="50" Width="100%" id="txtGoogleAnalyticsCode" Runat="server"></asp:textbox>
						        </td>
						    </tr>
						    <tr>
							    <td class="table_cell" colSpan="2">
								    <div class="add-buttons">
									    <asp:Button id="btnSave" Runat="server" onclick="btnSave_Click"></asp:Button>
									    <asp:Button ID="btnClear" Runat="server" onclick="btnClear_Click"></asp:Button>
								    </div>
							    </td>
						    </tr>
					    </table>
				</div>
			</div>
		</form>
	</body>
</HTML>
