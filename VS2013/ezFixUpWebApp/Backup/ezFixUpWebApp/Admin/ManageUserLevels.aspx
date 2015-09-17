<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageUserLevels.aspx.cs" Inherits="ezFixUp.Admin.ManageUserLevels" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register Src="MessageBox.ascx" TagName="MessageBox" TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="https://www.w3.org/1999/xhtml" >
<head runat="server">
    <title><%= Lang.TransA("Manage User Levels") %></title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
		<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
		<div id="layout">
			<div id="crupms">
	            <img src="images/i_userlevels.jpg"> 
	            <div id="crumpstxt"><%= Lang.TransA("Manage User Levels") %></div>
            </div>
			<div id="sidebar">
	            <uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
            </div>
			<div id="main">
				<div id="content-head">
                    <div class="ch-head"><%= Lang.TransA("Manage User Levels")%></div>
                    <div class="ch-description"><%= Lang.TransA("Use this section to create user levels and specify restrictions for them ...") %></div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
				<table cellspacing="0" cellpadding="0" width="100%" border="0">
					<tr>
						<td>
							<div class="label">
								<%= Lang.TransA("User Levels") %>
							</div>
							<div class="separator06"></div>
						</td>
					</tr>
					<tr>
						<td><asp:datagrid id="dgUserLevels" CssClass="btable1" Runat="server" 
                                AutoGenerateColumns="False" Width="100%" cellpadding="0" cellspacing="0"  
                                BorderWidth="0" GridLines="None" onitemcommand="dgUserLevels_ItemCommand" 
                                onitemdatabound="dgUserLevels_ItemDataBound">
								<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
								<HeaderStyle></HeaderStyle>
								<Columns>
									<asp:TemplateColumn>
										<HeaderStyle CssClass="table_header2"  Width="10%" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
										<ItemTemplate>
											<%# Eval("Level") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<HeaderStyle CssClass="table_header2" Width="10%" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
										<ItemTemplate>
											<img src='<%# Eval("IconURL") %>' title='<%# String.Format(Lang.TransA("Level {0}"), Eval("Level")) %> - <%# Eval("Name") %>' border='0' />
										</ItemTemplate>
									</asp:TemplateColumn>																	
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
											<%# Eval("MinScore") %>
										</ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<HeaderStyle CssClass="table_header2c" Width="20%" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3c"></ItemStyle>
										<ItemTemplate>
											<asp:LinkButton id="lnkDelete" CommandName="DeleteLevel" CommandArgument='<%# Eval("ID") %>' Runat="server">
												<%# Lang.TransA("Delete")%></asp:LinkButton>&nbsp;
											<asp:LinkButton id="lnkEdit" CommandName="EditLevel" CommandArgument='<%# Eval("ID") %>' Runat="server">
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
								<asp:button id="btnAddNewLevel" Runat="server" onclick="btnAddNewLevel_Click"></asp:button>
							</div>
						</td>
					</tr>
				</table>
				<div class="separator10"></div>
				<asp:panel id="pnlUserLevelInfo" Runat="server">
					<TABLE cellpadding="0" cellspacing="0" class="stable2">
						<TR>
							<TD class="table_header" colSpan="2">
								<%= Lang.TransA("User Level Details") %></TD>								
						</TR>
						<TR>
							<TD class="table_cell">
								<%= Lang.TransA("Name") %></TD>
							
							<TD class="table_cell">
								<asp:TextBox id="txtName" Runat="server">
								</asp:TextBox>
							</TD>
							
						</TR>
						<TR>
							<TD class="table_cell">
								<%= Lang.TransA("Minimum Score") %></TD>
							
							<TD class="table_cell">
								<asp:TextBox id="txtMinScore" Runat="server">
								</asp:TextBox>
							</TD>
							
						</TR>
					</TABLE>
					<TABLE cellpadding="0" cellspacing="0" class="stable2">
					<tr>
					    <td>
					        <asp:PlaceHolder ID="phLevelRestrictions" runat="server"></asp:PlaceHolder>
					    </td>
					</tr>						
					<TR>
						<TD>
							<div class="separator10"></div>
							<div class="add-buttons">
								<asp:Button id="btnCreateUpdate" Runat="server" onclick="btnCreateUpdate_Click"></asp:Button>
								<asp:Button id="btnCancel" runat="server" onclick="btnCancel_Click"></asp:Button>
							</div>
						</TD>
					</TR>
					</TABLE>						
                    <uc2:MessageBox ID="MessageBox1" runat="server" />
					<div class="separator10"></div>
					<input id="hidCurrentLevelID" type="hidden" runat="server" />
				</asp:panel>	
        	</ContentTemplate>
            </asp:UpdatePanel>
        	</div>
		</div>
    </form>
</body>
</html>
