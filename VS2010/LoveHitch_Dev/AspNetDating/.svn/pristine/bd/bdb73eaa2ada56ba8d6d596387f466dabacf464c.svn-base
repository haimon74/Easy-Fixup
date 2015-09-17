<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditEcardTypes.aspx.cs" Inherits="AspNetDating.Admin.EditEcardTypes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc4" %>
<%@ Import Namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit e-card types") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
	                <img src="images/i_editpages.jpg" alt="edit pages image" /> 
					<div id="crumpstxt"><%= Lang.TransA("Edit e-card types")%></div>
			    </div>
				<div id="left">
		            <uc1:adminmenu id="Adminmenu1" runat="server"></uc1:adminmenu>
				</div>
			    <div id="right">
			    	<div id="content-head">
					    <div class="ch-head"><%= Lang.TransA("Edit e-card types")%></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to edit e-card types")%></div>	
					</div>
					<uc1:messagebox id="Messagebox1" runat="server"></uc1:messagebox>				
				    <table cellspacing="0" cellpadding="0" class="filter">
					    <tr>
						    <td class="table_header" colspan="2"><%= Lang.TransA("E-cards") %></td>
					    </tr>
					    <tr id="trCategory" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select Category:") %></td>
						    <td class="table_cell">
                                <asp:dropdownlist id="ddCategory" AutoPostBack="True" 
                                    Runat="server" onselectedindexchanged="ddCategory_SelectedIndexChanged"></asp:dropdownlist></td>
					    </tr>
					    <tr id="trName" runat="server">
						    <td class="table_cell"><%= Lang.TransA("Select e-card:") %></td>
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
					    <table cellspacing="0" id="stable" cellpadding="0">
						    <tr>
							    <td class="table_header" align="center" colspan="2"><%= Lang.TransA("Edit e-card") %></td>
						    </tr>
						    <tr>
							    <td class="table_cell"><%= Lang.TransA("E-card name") %></td>
							    <td class="table_cell" align="left"><asp:textbox Width="100%" id="txtName" Runat="server"></asp:textbox></td>
						    </tr>
						    <tr>
						        <td id="tdImage" runat="server" class="table_cell" align="center" colspan="2">
						        	<div class="photowrap">
						            <img src='..\<%= "EcardContent.ashx?ect=" + ddName.SelectedValue %>' width="80%" height="auto" alt="content image" />
						            </div>
						        </td>
						    </tr>
						    <tr>
						        <td id="tdFlash" class="table_cell" align="center"  runat="server" colspan="2">
						            <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                        id="flvplayer">
                                        <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() %>"/>
                                        <param name="quality" value="high"/>
                                        <param name="bgcolor" value="#FFFFFF"/>
                                        <param name="wmode" value="transparent"/>
                                        <param name="allowfullscreen" value="true"/>
                                        <param name="flashvars" value="file=..\<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() + "&shuffle=false"%>" />
                                        <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() %>"
                                            quality="high" wmode="transparent" bgcolor="#FFFFFF" 
                                            name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                            pluginspage="http://www.macromedia.com/go/getflashplayer" flashvars="file=..\<%= "EcardContent.ashx?ect=" + ddName.SelectedValue + "&seed=" + new Random().NextDouble() + "&shuffle=false" %>"></embed></object>
						        </td>
						    </tr>
						    <tr>
							    <td class="table_cell"><%= Lang.TransA("File") %></td>
							    <td class="table_cell"><asp:FileUpload ID="fuContent" runat="server" /></td>
						    </tr>
						    <tr id="trUrlRewriteCheckbox" runat="server">
						        <td class="table_cell"><%= Lang.TransA("Active")%></td>
						        <td class="table_cell"><asp:CheckBox ID="cbActive" runat="server" /></td>
						    </tr>
						    <tr>
							    <td class="table_cell" colspan="2">
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
