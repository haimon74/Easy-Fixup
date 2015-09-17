<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveAds.aspx.cs" Inherits="AspNetDating.Admin.ApproveAds" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title>
	        <%= Lang.TransA("Approve Classifieds") %>
	    </title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="Form1" method="post" runat="server">
       	<uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
       	<div id="layout">
        	<div id="crupms">
				<img src="images/i_approveanswers.jpg">
	            <div id="crumpstxt"><%= Lang.TransA("Approve Classifieds")%></div>
            </div>
            <div id="sidebar">
                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
            </div>
            <div id="main">
        		<div id="content-head">
					<div class="ch-head"><%= Lang.TransA("Approve Classifieds")%></div>
                	<div class="ch-description">
                    <%= Lang.TransA("Use this section to approve or reject classifieds...") %>
                	</div>
               	</div>
               	<asp:UpdatePanel ID="UpdatePanelPendingApproval" runat="server">
                    <ContentTemplate>
						<uc1:MessageBox ID="MessageBox" runat="server" />
						<table width="100%" cellpadding="0" cellspacing="0">
							<tr>
								<td align="right" class="perpage">
								<asp:Label ID="lblGroupsPerPage" Font-Name="Verdana" Font-Size="11px" runat="server">
								</asp:Label>
								<asp:DropDownList ID="ddAdsPerPage" CssClass="pages" Font-Name="Verdana" runat="server" AutoPostBack="True">
								</asp:DropDownList>
								<div class="separator06">
								</div>
								</td>
							</tr>
						</table>
						<asp:DataGrid ID="dgPendingApproval" runat="server" Width="100%" PageSize="2" AllowPaging="True" AutoGenerateColumns="False" CssClass="btable1" CellPadding="5" GridLines="None" CellSpacing="0" BorderWidth="0" onitemcommand="dgPendingApproval_ItemCommand" onitemcreated="dgPendingApproval_ItemCreated" onitemdatabound="dgPendingApproval_ItemDataBound" onpageindexchanged="dgPendingApproval_PageIndexChanged" onselectedindexchanged="dgPendingApproval_SelectedIndexChanged">
							<AlternatingItemStyle CssClass="btable2" />
							<HeaderStyle />
							<Columns>
								<asp:templatecolumn>
									<HeaderStyle CssClass="table_header2" />
									<ItemStyle CssClass="table_cell3" Wrap="False" HorizontalAlign="Center" Width="15%" />
									<ItemTemplate>
										<img src='../AdPhoto.ashx?id=<%# Eval("AdPhotoID") %>&amp;width=120&amp;height=120&amp;diskCache=1' />
									</ItemTemplate>
								</asp:templatecolumn>
								<asp:templatecolumn>
									<HeaderStyle CssClass="table_header2" Wrap="False" />
									<ItemStyle CssClass="table_cell3" Width="50%" />
									<ItemTemplate>
										<b><%= Lang.TransA("Category") %>:</b>&nbsp;<%# Eval("CategoryTitle")%>
										<div class="clear">
										</div>
										<b><%= Lang.TransA("Posted by") %>:</b>&nbsp;<%# Eval("PostedBy")%>
										<div class="clear">
										</div>
										<b><%= Lang.TransA("Subject") %>:</b>&nbsp;<%# Eval("Subject")%>
										<div class="clear">
										</div>
										<b><%= Lang.TransA("Description") %>:</b>&nbsp;<%# Eval("Description")%>
										<div class="clear">
										</div>
										<b><%= Lang.TransA("Approved") %>:</b>&nbsp;<%# Eval("Approved")%>
										<div class="clear">
										</div>
										<b><%= Lang.TransA("Created on") %>:</b>&nbsp;<%# Eval("DateCreated")%>
									</ItemTemplate>
								</asp:templatecolumn>
								<asp:templatecolumn>
									<HeaderStyle CssClass="table_header2" />
									<ItemStyle CssClass="table_cell3" Width="30%" HorizontalAlign="Center" />
									<ItemTemplate>
										<div align="center">
											<asp:Button ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("ID")%>' />
											<div class="separator10">
											</div>
											<asp:Button ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("ID")%>' />
										</div>
										<div class="separator06">
										</div>
										<%= Lang.TransA("Reason") %>:&nbsp;
										<asp:TextBox Width="80%" ID="txtReason" runat="server">
										</asp:TextBox>
									</ItemTemplate>
								</asp:templatecolumn>
								<asp:templatecolumn>
									<HeaderStyle CssClass="table_header2" />
									<ItemStyle CssClass="table_cell3" Width="5%" HorizontalAlign="Right" />
									<ItemTemplate>
										<a href='EditAd.aspx?id=<%# Eval("ID")%>&amp;cid=<%# Eval("CategoryID") %>'>
										<img src="images/edit_icon.jpg" border="0">
										<%# Lang.TransA("Edit")%></img></a>
									</ItemTemplate>
								</asp:templatecolumn>
							</Columns>
							<PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="Center" Mode="NumericPages" />
						</asp:DataGrid>
					</ContentTemplate>
                </asp:UpdatePanel>
                </div>
	        </div>
	    </form>
	</body>
</html>
