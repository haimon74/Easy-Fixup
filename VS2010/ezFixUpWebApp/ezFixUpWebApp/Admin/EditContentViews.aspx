<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor" TagPrefix="HTMLEditor" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditContentViews.aspx.cs" Inherits="ezFixUp.Admin.EditContentViews" %>
<%@ Import Namespace="ezFixUp.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<HEAD runat="server">
		<title><%= Lang.TransA("Edit Content Views") %></title>
		<meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
<body>
    <form id="form1" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
	                <img src="images/i_editpages.jpg" /> 
					<div id="crumpstxt"><%= Lang.TransA("Edit Content") %></div>
			    </div>
				<div id="sidebar">
		            <uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				</div>
			    <div id="main">
			    	<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Edit Content") %></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to edit content in pages") %></div>	
					</div>				
				    <table cellSpacing="0" cellPadding="0" class="filter">
					    <tr>
						    <td class="table_header" colSpan="2"><%= Lang.TransA("Languages/Content") %></td>
					    </tr>
					    <tr id="trLanguage" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Language:") %></td>
						    <td class="table_cell"><asp:dropdownlist id="ddLanguage" AutoPostBack="True" Runat="server" OnSelectedIndexChanged="ddLanguage_SelectedIndexChanged"></asp:dropdownlist></td>
					    </tr>
					    <tr id="trPageName" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Content:") %></td>
						    <td class="table_cell"><asp:dropdownlist id="ddContentKey" AutoPostBack="True" Runat="server" OnSelectedIndexChanged="ddContentKey_SelectedIndexChanged"></asp:dropdownlist></td>
					    </tr>
				    </table>
					<div class="separator10"></div>
					<uc1:messagebox id="Messagebox1" runat="server"></uc1:messagebox>
				    <div id="divEditContentView" runat="server" Visible="False">
					    <table cellSpacing="0" cellPadding="0" width="100%">
						    <tr>
							    <td class="table_header2" align="center" colSpan="2"><%= Lang.TransA("Edit Content") %></td>
						    </tr>
						    <tr>
							    <td class="table_cell3"><%= Lang.TransA("Content") %></td>
							    <td class="table_cell3">
								    <div class="fckeditor">
                                        <asp:PlaceHolder ID="phEditor" runat="server"></asp:PlaceHolder>
								    </div>
							    </td>
						    </tr>
						    <tr>
							    <td class="table_cell3" colSpan="2">
								    <div class="add-buttons">
									    <asp:Button id="btnSave" Runat="server" OnClick="btnSave_Click"></asp:Button>
								    </div>
							    </td>
						    </tr>
					    </table>
				    </div>
				</div>
			</div>
    </form>
</body>
</html>
