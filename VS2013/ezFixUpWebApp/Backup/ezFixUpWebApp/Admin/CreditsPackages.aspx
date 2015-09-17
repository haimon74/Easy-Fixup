<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreditsPackages.aspx.cs" Inherits="ezFixUp.Admin.CreditsPackages" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Credits Packages") %></title>
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
		            <img src="images/i_billingsettings.jpg"> 
		            <div id="crumpstxt"><%= Lang.TransA("Credits Packages")%></div>
	            </div>
				<div id="sidebar">
		            <uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
				<div id="main">
					<div id="content-head">
	                    <div class="ch-head"><%= Lang.TransA("Credits Packages")%></div>
	                    <div class="ch-description"><%= Lang.TransA("Use this section to configure credits packages...")%></div>
	                </div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <uc2:MessageBox ID="MessageBox1" runat="server" />
                    <asp:MultiView ID="mvCreditsPackages" runat="server">
                        <asp:View ID="viewCreditsPackages" runat="server">
                            <table cellspacing="0" cellpadding="0" width="100%" border="0">
						<tr>
							<td>
								<div class="label">
									<asp:Label ID="lblCreditsPackages" runat="server"></asp:Label>
								</div>
								<div class="separator06"></div>
							</td>
						</tr>
						<tr>
							<td><asp:datagrid id="dgCreditsPackages" CssClass="btable1" Runat="server" 
                                    AutoGenerateColumns="False" Width="100%" cellpadding="0" cellspacing="0"  
                                    BorderWidth="0" GridLines="None" 
                                    onitemcommand="dgCreditsPackages_ItemCommand" 
                                    onitemcreated="dgCreditsPackages_ItemCreated">
									<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
									<HeaderStyle></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2"  Width="10%" Wrap="False"></HeaderStyle>
											<ItemStyle CssClass="table_cell3"></ItemStyle>
											<ItemTemplate>
												<%# Eval("Name") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2c" Width="30%" Wrap="False"></HeaderStyle>
											<ItemStyle CssClass="table_cell3c"></ItemStyle>
											<ItemTemplate>
												<%# Eval("Quantity") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2" Width="20%" Wrap="False"></HeaderStyle>
											<ItemStyle CssClass="table_cell3"></ItemStyle>
											<ItemTemplate>
												<%# Eval("Price") %>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2c" Width="20%" Wrap="False"></HeaderStyle>
											<ItemStyle CssClass="table_cell3c"></ItemStyle>
											<ItemTemplate>
												<asp:LinkButton id="lnkDelete" CommandName="Delete" CommandArgument='<%# Eval("ID") %>' Runat="server">
													<%# Lang.TransA("Delete")%></asp:LinkButton>&nbsp;
												<asp:LinkButton id="lnkEdit" CommandName="Edit" CommandArgument='<%# Eval("ID") %>' Runat="server">
													<%# Lang.TransA("Edit")%></asp:LinkButton>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid>
							</td>
						</tr>
						<tr>
							<td>
								<div class="add-buttons">
									<asp:button id="btnAddNewPackage" Runat="server" 
                                        onclick="btnAddNewPackage_Click"></asp:button>
								</div>
							</td>
						</tr>
					</table>
                        </asp:View>
                        <asp:View ID="viewAddEditCreditsPackage" runat="server">
				            <table cellpadding="0" cellspacing="0" class="filter">
			                	<tr>
			                    	<td colspan="2" class="table_header">
			                        	<%= Lang.TransA("Credit Package") %>
			                    	</td>
			                    </tr>
			                    <tr>
				                     <td class="table_cell">
				                     	<%= Lang.TransA("Name") %>:
									</td>
				                     <td class="table_cell">
				                     	<asp:TextBox ID="txtName" runat="server"></asp:TextBox>
									</td>
								</tr>
			                    <tr>
				                     <td class="table_cell">
				                     	<%= Lang.TransA("Quantity") %>:
									</td>
				                     <td class="table_cell">
				                     	<asp:TextBox ID="txtQuantity" runat="server"></asp:TextBox>
									</td>
								</tr>
			                    <tr>
				                     <td class="table_cell">
				                     	<%= Lang.TransA("Price") %>:
									</td>
				                     <td class="table_cell">
				                     	<asp:TextBox ID="txtPrice" runat="server"></asp:TextBox>
				                     </td>
								</tr>
			                    <tr>
			                    	<td colspan="2" class="table_footer" align="center">
			                        	<div class="separator06"></div>
			                            <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click" />
			                            <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click" />
			                        </td>
								</tr>
							</table>
							<table width="100%"><tr><td></td></tr></table>
							<div class="separator"></div>
                        </asp:View>
                    	</asp:MultiView>
					</ContentTemplate>
				</asp:UpdatePanel>
            	</div>
			</div>
		</form>
	</body>
</HTML>
