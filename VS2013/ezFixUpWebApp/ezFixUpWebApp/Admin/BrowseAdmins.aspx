<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Page language="c#" Codebehind="BrowseAdmins.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Admin.BrowseAdmins" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<HTML>
	<HEAD>
		<title><%= Lang.TransA("Browse Admins") %></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
				<div id="crupms">
		            <img src="images/i_admin.jpg"> 
		            <div id="crumpstxt"><%= Lang.TransA("Admin Management") %></div>
				</div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
				</div>
				<div id="main">
					<div id="content-head">
		                <div class="ch-head"><%= Lang.TransA("Admin Management") %></div>
		                <div class="ch-description"><%= Lang.TransA("Use this section to browse, edit or delete administrators...") %></div>
					</div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
	                <asp:DataGrid CssClass="btable1" Width="100%" BorderWidth="0" id="gridAdmins" Runat="server" AllowPaging="False" AutoGenerateColumns="False"
						cellpadding="0" cellspacing="0" GridLines="None" >
						<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
						<Columns>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2"></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Wrap="False" Width="20%"></ItemStyle>
								<ItemTemplate>
									<asp:Literal Text='<%# Eval("Username")%>' ID="litUsername" Runat="server">
									</asp:Literal>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2"></HeaderStyle>
								<ItemStyle CssClass="table_cell3" Wrap="False" Width="20%"></ItemStyle>
								<ItemTemplate>
									<%# Eval("LastLogin") %>
								</ItemTemplate>
							</asp:TemplateColumn>
							<asp:TemplateColumn>
								<HeaderStyle CssClass="table_header2c"></HeaderStyle>
								<ItemStyle CssClass="table_cell3c" Wrap="False" Width="20%"></ItemStyle>
								<ItemTemplate>
									<nobr>
										<asp:LinkButton ID="lnkEdit" CommandName="Edit" CommandArgument='<%# Eval("Username")%>' Runat="server">
											<%# Lang.TransA("Edit") %></asp:LinkButton>
										&nbsp;&nbsp;&nbsp;
										<asp:LinkButton ID="lnkDelete" CommandName="Delete" CommandArgument='<%# Eval("Username")%>' Runat="server">
											<%# Lang.TransA("Delete") %></asp:LinkButton>
									</nobr>
								</ItemTemplate>
							</asp:TemplateColumn>
						</Columns>
						<PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
					</asp:DataGrid>
					<div class="separator10"></div>
					<div class="add-buttons">
						<asp:Button ID="btnAddAdmin" Runat="server" onclick="AddAdmin_Click"></asp:Button>
					</div>
					<div class="separator10"></div>
				</ContentTemplate>
				</asp:UpdatePanel>
				</div>
			</div>
		</form>
	</body>
</HTML>
