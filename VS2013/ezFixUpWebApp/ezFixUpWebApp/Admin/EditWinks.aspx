﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditWinks.aspx.cs" Inherits="ezFixUp.Admin.EditWinks" %>
<%@ Import Namespace="ezFixUp"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit winks types") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
	                <img src="images/i_editpages.jpg" /> 
					<div id="crumpstxt"><%= Lang.TransA("Edit winks types")%></div>
			    </div>
				<div id="sidebar">
		            <uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				</div>
			    <div id="main">
			    	<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Edit winks types")%></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to edit wink types")%></div>	
					</div>
					<uc1:messagebox id="Messagebox1" runat="server"></uc1:messagebox>				
				    <table cellSpacing="0" cellPadding="0" class="filter">
					    <tr>
						    <td class="table_header" colSpan="2"><%= Lang.TransA("Winks") %></td>
					    </tr>
					    <tr id="trLanguage" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Language:") %></td>
						    <td class="table_cell">
                                <asp:dropdownlist id="ddLanguage" AutoPostBack="True" 
                                    Runat="server" onselectedindexchanged="ddLanguage_SelectedIndexChanged"></asp:dropdownlist></td>
					    </tr>
					    <tr id="trCategory" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Category:") %></td>
						    <td class="table_cell">
                                <asp:dropdownlist id="ddCategory" AutoPostBack="True" 
                                    Runat="server" onselectedindexchanged="ddCategory_SelectedIndexChanged"></asp:dropdownlist></td>
					    </tr>
					    <tr id="trName" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select wink:") %></td>
						    <td class="table_cell"><asp:dropdownlist id="ddName" AutoPostBack="True" 
                                    Runat="server" onselectedindexchanged="ddName_SelectedIndexChanged"></asp:dropdownlist></td>
					    </tr>
				    </table>
					<div class="separator10"></div>
				    <div id="pnlEditEcardCategory" runat="server" Visible="False">
				        <table cellspacing="0" id="tblEditCategory" cellpadding="0">
						    <tr>
							    <td class="table_header" align="center" colspan="2"><%= Lang.TransA("Edit category") %></td>
						    </tr>
						    <tr>
							    <td class="table_cell"><%= Lang.TransA("Category name") %></td>
							    <td class="table_cell" align="left">
							    <asp:textbox Width="100%" id="txtCategoryName" Runat="server"></asp:textbox></td>
						    </tr>
						    <tr id="tr1" runat="server">
						        <td class="table_cell"><%= Lang.TransA("Active")%></td>
						        <td class="table_cell"><asp:CheckBox ID="cbCategoryActive" runat="server" /></td>
						    </tr>
						    <tr>
							    <td class="table_cell" colspan="2">
								    <div class="add-buttons">
									    <asp:Button id="btnSaveCategory" Runat="server" onclick="btnSaveCategory_Click"></asp:Button>
									    <asp:Button ID="btnDeleteCategory" Runat="server" onclick="btnDeleteCategory_Click"></asp:Button>
								    </div>
							    </td>
						    </tr>
						</table>
				    </div>
					<div class="separator10"></div>
				    <div id="pnlEditEcardType" runat="server" Visible="False">
					    <table cellSpacing="0" id="stable" cellPadding="0">
						    <tr>
							    <td class="table_header" align="center" colSpan="2"><%= Lang.TransA("Edit Page") %></td>
						    </tr>
						    <tr>
							    <td class="table_cell"><%= Lang.TransA("Name") %></td>
							    <td class="table_cell" align="left"><asp:textbox Width="100%" id="txtName" Runat="server"></asp:textbox></td>
						    </tr>
						    
                            <tr>
                                <td class="table_cell"><%= Lang.TransA("Phrase") %></td>
                                <td class="table_cell" align="left">
                                  <asp:textbox Width="100%" id="txtPhrase" Runat="server"></asp:textbox>
                                </td>
						    </tr>
						    <tr id="trUrlRewriteCheckbox" runat="server">
						        <td class="table_cell"><%= Lang.TransA("Active")%></td>
						        <td class="table_cell"><asp:CheckBox ID="cbActive" runat="server" /></td>
						    </tr>
						    <tr>
							    <td class="table_cell" colSpan="2">
								    <div class="add-buttons">
									    <asp:Button id="btnSave" Runat="server" onclick="btnSave_Click"></asp:Button>
									    <asp:Button ID="btnDelete" Runat="server" onclick="btnDelete_Click"></asp:Button>
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