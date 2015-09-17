<%@ Page language="c#" Codebehind="ApprovePhotos.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.ApprovePhotos" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title><%= Lang.TransA("Approve Photos") %></title>
		<meta name="GENERATOR" Content="Microsoft Visual Studio .NET 7.1">
		<meta name="CODE_LANGUAGE" Content="C#">
		<meta name="vs_defaultClientScript" content="JavaScript">
		<meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
		<link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</HEAD>
	<body>
		<form id="Form1" method="post" runat="server">
			<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
    			<div id="layout">
    				<div id="crupms">
					    <img src="images/i_browsephotos.jpg"> 
				        <div id="crumpstxt"> <%= Lang.TransA("Approve Photos") %></div>
			        </div>
				<div id="sidebar">
					<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
			    </div>
				<div id="main">
        			<div id="content-head">
	        			<div class="ch-head">
							<%= Lang.TransA("Approve Photos") %>
						</div>
			            <div class="ch-description"><%= Lang.TransA("Use this section to approve or reject pending photos...") %></div>
		            </div>
		            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
		            <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
		            <asp:CheckBox ID="cbShowManualApprovalPhotos" runat="server" Visible="false" AutoPostBack="true" 
                            oncheckedchanged="cbShowManualApprovalPhotos_CheckedChanged"/>
                    <%= Lang.TransA("Show only photos marked for manual approval") %>
		            <table width="100%" border="0" cellpadding=0 cellspacing=0>
						<tr>
							<td align="right" class="perpage">
								<asp:Panel ID="pnlPhotosPerPage"  runat="server">
									<asp:Label id="lblPhotosPerPage"  Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>:
									<asp:DropDownList id="dropPhotosPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True"></asp:DropDownList>
								</asp:Panel>
							    <div class="separator06"></div>
							</td>
						</tr>
						<tr>
							<td>
								<asp:DataGrid id="gridPendingApproval" Runat="server" PageSize="20" AllowPaging="True" AutoGenerateColumns="False"
								Width="100%" CssClass="btable1" cellpadding="0" cellspacing="0"  BorderWidth="0" GridLines="None">
									<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
									<HeaderStyle></HeaderStyle>
									<Columns>
										<asp:TemplateColumn>
												<HeaderStyle CssClass="table_header2"></HeaderStyle>
												<ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
												<ItemTemplate>
												<a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>"><%# Eval("Username")%></a>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<HeaderStyle CssClass="table_header2"></HeaderStyle>
												<ItemStyle CssClass="table_cell3" Wrap=False></ItemStyle>
												<ItemTemplate>
												<%# Eval("Name")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<HeaderStyle CssClass="table_header2"></HeaderStyle>
												<ItemStyle CssClass="table_cell3" Wrap=False></ItemStyle>
												<ItemTemplate>
												<%# Eval("Description")%>
												</ItemTemplate>
											</asp:TemplateColumn>
											<asp:TemplateColumn>
												<HeaderStyle CssClass="table_header2c"></HeaderStyle>
												<ItemStyle CssClass="table_cell3c" VerticalAlign="Middle" Wrap=False></ItemStyle>
												<ItemTemplate>
												<a href="ApprovePhoto.aspx?pid=<%# Eval("PhotoID")%>">
													<img border=0 src="<%= Config.Urls.Home%>/Image.ashx?id=<%# Eval("PhotoID")%>&width=100&height=100">
												</a></ItemTemplate>
											</asp:TemplateColumn>
										</Columns>
										<PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="Center" Mode="NumericPages"></PagerStyle>
									</asp:DataGrid>								
			                    </td>
		                    </tr>
	                    </table>
	                   </ContentTemplate>
	                   </asp:UpdatePanel>
                    </div>
               </div>
		</form>
	</body>
</HTML>
