<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAdsCategory.aspx.cs" Inherits="ezFixUp.Admin.EditAdsCategory" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>
			<%= Lang.TransA("Edit Category")%>
		</title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body >
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
					<img src="images/i_question.jpg"> 
					<div id="crumpstxt"><%= Lang.TransA("Edit Category") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
			            <div class="ch-head"><%= Lang.TransA("Edit Category")%></div>
			        	<div class="ch-description">
			        		<%= Lang.TransA("Here you can add new ads for selected category or modify existing ones...") %>
			        	</div>		
			        </div>
					<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
				    <table  cellpadding="0" cellspacing="0" class="stable2">
					    <tr>
						    <td align="center" colspan="2" class="table_header">
							    <%= Lang.TransA("Category Details") %>
						    </td>
					    </tr>
					    <tr>
						    <td class="table_cell"><%= Lang.TransA("Name") %></td>
						    <td class="table_cell"><asp:textbox id="txtCategoryTitle" Width="70%" Runat="server"></asp:textbox></td>
					    </tr>
				    </table>
				    <div class="separator10"></div>
					<div class="separator06"></div>
				    <div class="separator10"></div>
				    <asp:Panel ID="pnlSubcategories" Runat="server">
					    <TABLE cellSpacing="0" cellPadding="0" class="stable2">
						    <TR>
							    <TD class="table_header">
						      <%= Lang.TransA("Sub categories") %></TD>
						    </TR>
						    <TR>
							    <TD>
								    <asp:datagrid id="dgSubcategories" BorderWidth="0" Runat="server" Width="100%"
								     cellspacing="0" cellpadding="0" AutoGenerateColumns="False" AllowPaging="False"
									    PageSize="10" ShowHeader="False" GridLines="None">
									    <AlternatingItemStyle></AlternatingItemStyle>
									    <Columns>
										    <asp:TemplateColumn>
											    <ItemStyle CssClass="table_cell" Wrap="False" HorizontalAlign="Center" Width="1%"></ItemStyle>
											    <ItemTemplate>
												    <input type="checkbox" id="cbSelect" value='<%# Eval("ID") %>' runat="server" NAME="cbSelect" />
											    </ItemTemplate>
										    </asp:TemplateColumn>
										    <asp:TemplateColumn>
											    <ItemStyle CssClass="table_cell" Width="100%"></ItemStyle>
											    <ItemTemplate>
												    <asp:TextBox ID="txtTitle" Text='<%# Eval("Title")%>' Runat=server>
												    </asp:TextBox>
											    </ItemTemplate>
										    </asp:TemplateColumn>
									    </Columns>
									    <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
								    </asp:datagrid>
								    <div class="separator06"></div>
								    <DIV align="center" class="font">
									    <%= Lang.TransA("Add") %><asp:dropdownlist id="dropNewSubcategoriesCount" runat="server" Font-Size="11px" Font-Name="Verdana"></asp:dropdownlist>
									    <%= Lang.TransA("new sub categories") %>&nbsp;
									    <asp:button id="btnAddNewSubcategories" runat="server" 
                                            onclick="btnAddNewSubcategories_Click"></asp:button>
									    <div class="separator10"></div>
									    <asp:button id="btnDeleteSelectedSubcategories" runat="server" 
                                            onclick="btnDeleteSelectedSubcategories_Click"></asp:button></DIV>
							    </TD>
						    </TR>
					    </TABLE>
					</asp:Panel>
					<div class="separator10"></div>
					<div align="center">
					<asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" />
					<asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" /></div>
					<div class="separator10"></div>
			    </div>
			</div>
		</form>
	</body>
</HTML>
