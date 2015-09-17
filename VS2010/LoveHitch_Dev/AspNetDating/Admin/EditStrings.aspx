<%@ Page language="c#" Codebehind="EditStrings.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditStrings" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title>Edit Texts</title>
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
		            <img src="images/i_textmanagement.jpg"> 
		            <div id="crumpstxt"><%= Lang.TransA("Text Management") %></div>
				</div>
				<div id="sidebar">
		            <uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
		                <div class="ch-head"><%= Lang.TransA("Text Management") %></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to edit the text used throughout the site") %></div>
			        </div>
				    <div id="pnlLanguage" runat="server">
				        <table cellSpacing="0" cellPadding="0" class="filter">
					        <tr>
						        <td class="table_header" colSpan="2"><%= Lang.TransA("Languages") %></td>
					        </tr>
					        <tr>
						        <td class="table_cell"><%= Lang.TransA("Language") %>:</td>
						        <td class="table_cell"><asp:dropdownlist id="ddLanguage" Runat="server" AutoPostBack="True" onselectedindexchanged="ddLanguage_SelectedIndexChanged">
								        <asp:ListItem></asp:ListItem>
							        </asp:dropdownlist></td>
					        </tr>
				        </table>
				    </div>
			        <table cellSpacing="0" cellPadding="0" class="filter">
				        <tr>	
				        <td class="table_cell"><%= Lang.TransA("Location") %>:</td>		
				        <td class="table_cell">
				            <asp:DropDownList ID="ddTranslationType" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddTranslationType_SelectedIndexChanged"></asp:DropDownList>
                        </td>	    
                        </tr>
                    </table>
                    <table cellSpacing="0" cellPadding="0" class="filter">
				        <tr>	
				        <td class="table_cell"><%= Lang.TransA("Page") %>:</td>		
				        <td class="table_cell">
				            <asp:DropDownList ID="ddPage" runat="server" AutoPostBack="true"
                            onselectedindexchanged="ddPage_SelectedIndexChanged"></asp:DropDownList>
                        </td>	    
                        </tr>
                    </table>
                    <div class="separator10"></div>				    
			        <asp:datagrid id="dgTranslations" Runat="server" CssClass="btable1" BorderWidth="0" cellspacing="0" cellpadding="0" AutoGenerateColumns="False" AllowPaging="False"
				        Width="100%" GridLines="None">
				        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
				        <Columns>
					        <asp:TemplateColumn>
						        <HeaderStyle CssClass="table_header2"></HeaderStyle>
						        <ItemStyle CssClass="table_cell3" Width="48%"></ItemStyle>
						        <ItemTemplate>
							        <%# Server.HtmlEncode((string)Eval("Key")) %>
						        </ItemTemplate>
					        </asp:TemplateColumn>
					        <asp:TemplateColumn>
						        <HeaderStyle CssClass="table_header2"></HeaderStyle>
						        <ItemStyle CssClass="table_cell3" Width="50%"></ItemStyle>
						        <ItemTemplate>
							        <input id="hidKey" value='<%# Eval("Key") %>' type="hidden" runat="server" />
							        <input id="hidOriginalValue" value='<%# Eval("Value") %>' type="hidden" runat="server" />
							        <asp:TextBox ID="txtValue" Width="98%" Runat="server" Text='<%# Eval("Value") %>'>
							        </asp:TextBox>
						        </ItemTemplate>
					        </asp:TemplateColumn>
				        </Columns>
			        </asp:datagrid>
					<div class="separator10"></div>
			        <div class="add-buttons">
			            <asp:Button ID="btnSave" Visible="False" Runat="server" onclick="btnSave_Click"></asp:Button>
			        </div>
				</div>
			</div>
		</form>
	</body>
</HTML>
