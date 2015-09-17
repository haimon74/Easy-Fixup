<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ApproveGroups.aspx.cs" Inherits="ezFixUp.Admin.ApproveGroups" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title>
	        <%= Lang.TransA("Approve Groups") %>
	    </title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="Form1" method="post" runat="server">
       	<uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
       	<div id="layout">
        	<div id="crupms">
				<img src="images/i_approveanswers.jpg">
	            <div id="crumpstxt"><%= Lang.TransA("Approve Groups") %></div>
            </div>
            <div id="sidebar">
                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
            </div>
            <div id="main">
        		<div id="content-head">
					<div class="ch-head"><%= Lang.TransA("Approve Groups") %></div>
                	<div class="ch-description">
                    <%= Lang.TransA("Use this section to approve or reject pending groups...") %>
                	</div>
               	</div>
               	<asp:UpdatePanel ID="UpdatePanelPendingApproval" runat="server">
                    <ContentTemplate>
                        <uc1:MessageBox ID="MessageBox" runat="server"></uc1:MessageBox>
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <tr>
                                <td align="right" class="perpage">
                                    <asp:Label ID="lblGroupsPerPage" Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddGroupsPerPage" CssClass="pages" Font-Name="Verdana"
                                        runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddGroupsPerPage_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <div class="separator06"></div>
                                </td>
                            </tr>
                        </table>
                        <asp:DataGrid ID="dgPendingApproval" runat="server" Width="100%"
                            AutoGenerateColumns="False" CssClass="btable1" CellPadding="0" GridLines="None" CellSpacing="0"
                            BorderWidth="0" OnItemCommand="dgPendingApproval_ItemCommand" OnItemCreated="dgPendingApproval_ItemCreated" OnItemDataBound="dgPendingApproval_ItemDataBound">
                            <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                            <HeaderStyle></HeaderStyle>
                            <Columns>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Wrap="False" HorizontalAlign="Center" Width="15%">
                                    </ItemStyle>
                                    <ItemTemplate>
                                        <img src='../GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=120&height=120&diskCache=1' />
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2" Wrap="False">
                                    </HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Width="65%"></ItemStyle>
                                    <ItemTemplate>
                                        <b><%= Lang.TransA("Name") %>:</b>&nbsp;<%# Eval("Name")%>
                                        <div class="clear"></div>
                                        <b><%= Lang.TransA("Categories") %>:</b>&nbsp;<%# Eval("Categories")%>
                                        <div class="clear"></div>
                                        <b><%= Lang.TransA("Description") %>:</b>&nbsp;<%# Eval("Description")%>
                                        <div class="clear"></div>
                                        <b><%= Lang.TransA("Approved") %>:</b>&nbsp;<%# Eval("Approved")%>
                                        <div class="clear"></div>
                                        <b><%= Lang.TransA("Access level") %>:</b>&nbsp;<%# Eval("AccessLevel")%>
                                        <div class="clear"></div>
                                        <b><%= Lang.TransA("Owner") %>:</b>&nbsp;<%# Eval("Owner")%>
                                        <div class="clear"></div>
                                        <b><%= Lang.TransA("Created on") %>:</b>&nbsp;<%# Eval("DateCreated")%>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Width="10%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Button ID="btnApprove" runat="server" CommandName="Approve" CommandArgument='<%# Eval("GroupID")%>' />
                                        <br />
                                        <br />
                                        <asp:Button ID="btnReject" runat="server" CommandName="Reject" CommandArgument='<%# Eval("GroupID")%>' />
                                        <br />
                                        <asp:TextBox ID="txtReason" runat="server"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:TemplateColumn>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                    <ItemStyle CssClass="table_cell3" Width="10%"></ItemStyle>
                                    <ItemTemplate>
                                        <a href="EditGroup.aspx?id=<%# Eval("GroupID")%>">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%# Lang.TransA("Edit")%></a>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="Center"
                                Mode="NumericPages"></PagerStyle>
                        </asp:DataGrid>
                        <asp:Panel ID="pnlPaginator" runat="server">
                    	    <div class="separator06"></div>
                            <table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
                        	    <tr>
                            	    <td>
                                	    <asp:LinkButton ID="lnkFirst" CssClass="pagelink" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkPrev" CssClass="pagelink" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
                                        <asp:Label ID="lblPager" runat="server"></asp:Label>
                                        <asp:LinkButton ID="lnkNext" CssClass="pagelink" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkLast" CssClass="pagelink" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                </div>
	        </div>
	    </form>
	</body>
</html>
