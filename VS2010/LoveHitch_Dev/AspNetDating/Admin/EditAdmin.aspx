<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Page language="c#" Codebehind="EditAdmin.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditAdmin" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Edit Admin") %></title>
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
		            <img src="images/i_admin.jpg"> 
		            <div id="crumpstxt"><%= Lang.TransA("Add/Edit Admin") %></div>
	            </div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
			            <div class="ch-head"><%= Lang.TransA("Add/Edit Admin") %></div>
			            <div class="ch-description"><%= Lang.TransA("Use this section to modify administrator's details...") %></div>
					</div>
					<uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<TABLE id="tblAll" cellSpacing="0" cellPadding="0" width="100%" runat="server">
						<TR>
							<TD>
								<table cellSpacing="0" cellPadding="0" class="filter">
									<TR>
										<TD class="table_header" colSpan="2"><%= Lang.TransA("Account Details") %></TD>
									</TR>
									<TR id="trUsername" runat="server">
										<TD class="table_cell"><%= Lang.TransA("Username") %></TD>
										<TD class="table_cell"><asp:textbox id="txtUsername" Runat="server"></asp:textbox></TD>
									</TR>
									<TR id="trCurrentPassword" runat="server">
										<TD class="table_cell"><%= Lang.TransA("Current password") %></TD>
										<TD class="table_cell"><asp:textbox id="txtCurrentPassword" Runat="server" TextMode="Password"></asp:textbox></TD>
									</TR>
									<TR>
										<TD class="table_cell"><asp:label id="lblNewPassword" Runat="server"></asp:label></TD>
										<TD class="table_cell"><asp:textbox id="txtNewPassword" Runat="server" TextMode="Password"></asp:textbox></TD>
									</TR>
									<TR>
										<TD class="table_cell"><asp:label id="lblConfirmNewPassword" Runat="server"></asp:label></TD>
										<TD class="table_cell"><asp:textbox id="txtConfirmNewPassword" Runat="server" TextMode="Password"></asp:textbox></TD>
									</TR>
								</table>
							</TD>
						</TR>
						<TR id="trPermissions" runat="server">
							<TD>
								<div class="separator10"></div>
								<div class="label">
									<asp:label id="lblAccountPermissions" runat="server"></asp:label>
								</div>
								<div class="separator06"></div>
								<asp:datagrid id="gridPermissions" CssClass="btable1" Width="100%"  BorderWidth="0" Runat="server" 
									cellspacing="0" cellpadding="0" AutoGenerateColumns="False" AllowPaging="False" PageSize="10" GridLines="None">
									<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
									<HeaderStyle></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2"></HeaderStyle>
											<ItemStyle CssClass="table_cell3" Width="50%"></ItemStyle>
											<ItemTemplate>
												<asp:Literal Text='<%# Eval("Section") %>' ID="litSection" Runat=server>
												</asp:Literal>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2c" Wrap="False"></HeaderStyle>
											<ItemStyle CssClass="table_cell3c" Width="25%"></ItemStyle>
											<ItemTemplate>
												<asp:CheckBox ID="cbReadPermission" Runat=server Checked='<%# (bool)Eval("Read")%>'>
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
										<asp:TemplateColumn>
											<HeaderStyle CssClass="table_header2c" Wrap="False"></HeaderStyle>
											<ItemStyle CssClass="table_cell3c" Width="25%"></ItemStyle>
											<ItemTemplate>
												<asp:CheckBox ID="cbWritePermission" Runat=server Checked='<%# (bool)Eval("Write")%>'>
												</asp:CheckBox>
											</ItemTemplate>
										</asp:TemplateColumn>
									</Columns>
								</asp:datagrid>
							</TD>
						</TR>
						<TR>
							<TD>
								<div class="add-buttons">
									<asp:button id="btnSave" runat="server" onclick="btnSave_Click"></asp:button>
								</div>
							</TD>
						</TR>
					</TABLE>  	
                </div>
			</div>
		</form>
	</body>
</HTML>
