<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Page language="c#" Codebehind="EditContentPages.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditContentPages" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD runat="server">
		<title><%= Lang.TransA("Edit Content Pages") %></title>
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
	                <img src="images/i_editpages.jpg" /> 
					<div id="crumpstxt"><%= Lang.TransA("Edit Pages") %></div>
			    </div>
				<div id="sidebar">
		            <uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				</div>
			    <div id="main">
			    	<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Edit Pages") %></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to edit content pages") %></div>	
					</div>
					<uc1:messagebox id="Messagebox1" runat="server"></uc1:messagebox>				
				    <table cellSpacing="0" cellPadding="0" class="filter">
					    <tr>
						    <td class="table_header" colSpan="2"><%= Lang.TransA("Languages/Pages") %></td>
					    </tr>
					    <tr id="trLanguage" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Language:") %></td>
						    <td class="table_cell"><asp:dropdownlist id="ddLanguage" AutoPostBack="True" Runat="server"></asp:dropdownlist></td>
					    </tr>
					    <tr id="trPageName" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Page:") %></td>
						    <td class="table_cell"><asp:dropdownlist id="ddPageName" AutoPostBack="True" Runat="server"></asp:dropdownlist></td>
					    </tr>
				    </table>
					<div class="separator10"></div>
				    <div id="divEditPage" runat="server" Visible="False">
					    <table cellSpacing="0" cellPadding="0" width="100%">
						    <tr>
							    <td class="table_header2" align="center" colSpan="2"><%= Lang.TransA("Edit Page") %></td>
						    </tr>
						    <tr>
							    <td class="table_cell3" width="100px"><%= Lang.TransA("Page Title") %></td>
							    <td class="table_cell3"><asp:textbox Width="98%" id="txtPageTitle" Runat="server"></asp:textbox></td>
						    </tr>
						    <tr>
						        <td class="table_cell3"><asp:CheckBox ID="cbURL" runat="server" AutoPostBack="true" /></td>
						        <td class="table_cell3"><%= Lang.TransA("Redirect to Url")%></td>
						    </tr>
						    <tr id="trURL" runat="server" visible="false">
						        <td class="table_cell3"><%= Lang.TransA("URL") %></td>
						        <td class="table_cell3"><asp:TextBox ID="txtURL" runat="server"></asp:TextBox></td>
						    </tr>
						    <tr id="trUrlRewriteCheckbox" runat="server">
						        <td class="table_cell3"><asp:CheckBox ID="cbRewriteUrl" runat="server" AutoPostBack="true" /></td>
						        <td class="table_cell3"><%= Lang.TransA("Rewrite Url")%></td>
						    </tr>
						    <tr id="trUrlRewriteTextbox" runat="server" visible="false">
						        <td class="table_cell3"><%= Lang.TransA("URL") %></td>
						        <td class="table_cell3"><asp:TextBox ID="txtUrlRewrite" runat="server"></asp:TextBox> <%= Lang.TransA("(must end with .aspx)") %></td>
						    </tr>
						    <tr id="trPageContent" runat="server">
							    <td class="table_cell3"><%= Lang.TransA("Page Content") %></td>
							    <td class="table_cell3">
							        <div class="fckeditor">
							            <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
								    </div>
							    </td>
						    </tr>
						    <tr>
							    <td class="table_cell3"><%= Lang.TransA("Header position") %></td>
							    <td class="table_cell3"><asp:DropDownList ID="ddHeaderPosition" Runat="server"></asp:DropDownList></td>
						    </tr>
						    <tr>
							    <td class="table_cell3"><%= Lang.TransA("Footer position") %></td>
							    <td class="table_cell3"><asp:DropDownList ID="ddFooterPosition" Runat="server"></asp:DropDownList></td>
						    </tr>
						    <tr>
							    <td class="table_cell3"><%= Lang.TransA("Visible for") %></td>
							    <td class="table_cell3"><asp:DropDownList ID="ddVisibleFor" Runat="server"></asp:DropDownList></td>
						    </tr>
						    <tr id="trMetaDescription" runat="server">
						        <td class="table_cell3"><%= Lang.TransA("Meta description") %></td>
						        <td class="table_cell3"><asp:TextBox ID="txtMetaDescription" runat="server"></asp:TextBox></td>
						    </tr>
						    <tr id="trMetaKeyword" runat="server">
						        <td class="table_cell3"><%= Lang.TransA("Meta keyword") %></td>
						        <td class="table_cell3"><asp:TextBox ID="txtMetaKeyword" runat="server"></asp:TextBox></td>
						    </tr>
						    <tr>
							    <td class="table_cell3" colSpan="2">
								    <div class="add-buttons">
									    <asp:Button id="btnSave" Runat="server"></asp:Button>
									    <asp:Button ID="btnDelete" Runat="server"></asp:Button>
								    </div>
							    </td>
						    </tr>
					    </table>
				    </div>
				</div>
			</div>
		</form>
	</body>
</HTML>
