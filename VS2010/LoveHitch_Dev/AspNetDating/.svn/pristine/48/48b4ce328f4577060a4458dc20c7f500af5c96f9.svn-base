<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowseGroups.aspx.cs" Inherits="AspNetDating.Admin.BrowseGroups" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Browse Groups") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
        	<uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
            	<div id="crupms">
                	<img src="images/i_browsemembers.jpg">
                	<div id="crumpstxt"><%= Lang.TransA("Group Management") %></div>
                </div>
                <div id="sidebar">
                	<uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
                </div>
                <div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Group Management") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to browse, edit or delete groups of your site...") %>
                        </div>
                    </div>
                    <table id="tblHideSearch" cellpadding="0" cellspacing="0" class="filter" runat="server" visible="false">
                    	<tr>
                        	<td class="table_header">
                            	<%= Lang.TransA("Filter") %>
                        	</td>
                            <td class="table_header">
                            	[&nbsp;<a onclick="document.all['tblSearch'].style.display = 'block'; document.all['tblHideSearch'].style.display = 'none';"
                            	href="javascript: void(0);" style="color: Black;"><%= Lang.TransA("Expand filter") %></a>&nbsp;]&nbsp;
                            </td>
						</tr>
					</table>
                    <table id="tblSearch" cellpadding="0" cellspacing="0" class="filter">
                    	<tr>
                        	<td colspan="2" class="table_header">
                            	<%= Lang.TransA("Filter") %>
                       		</td>
                   		</tr>
                        <tr>
                        	<td class="table_cell">
                            	<%= Lang.TransA("Category") %>:
                            </td>
                            <td class="table_cell">
                            	<asp:DropDownList ID="ddCategory" runat="server"></asp:DropDownList>
                            </td>
						</tr>
                        <tr>
                            <td class="table_cell">
                            	<%= Lang.TransA("Owner") %>:
                            </td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtOwner" runat="server"></asp:TextBox>
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
                        	    <%= Lang.TransA("Access level") %>:
                        	</td>
                            <td class="table_cell">
                            	<asp:DropDownList ID="ddAccessLevel" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="table_cell">
                                <%= Lang.TransA("Approved") %>:
                            </td>
                	        <td class="table_cell">
                        		<asp:DropDownList ID="ddApproved" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="table_footer" align="center">
                                <div class="separator10"></div>
                                    <asp:Button ID="btnSearch" Font-Name="Verdana" runat="server" OnClick="btnSearch_Click"></asp:Button>
                            </td>
                        </tr>
                    </table>
                    <script>
						if (document.all['<%= tblHideSearch.ClientID %>'])
							document.all['tblSearch'].style.display = 'none';
                    </script>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
					<div class="separator10"></div>
                    <table align="center" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="right" class="perpage">
                                <%= Lang.TransA("Groups per page") %>:
                                <asp:DropDownList ID="ddGroupsPerPage" CssClass="pages" Font-Name="Verdana" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddGroupsPerPage_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div class="separator06"></div>
                            </td>
                        </tr>
					</table>
                    <asp:DataGrid Width="100%" CssClass="btable1" ID="dgGroups" GridLines="None" runat="server" AllowPaging="False"
                        AutoGenerateColumns="False" AllowSorting="True" CellPadding="0" CellSpacing="0"
                        BorderWidth="0" OnItemCommand="dgGroups_ItemCommand" OnItemCreated="dgGroups_ItemCreated" OnSortCommand="dgGroups_SortCommand">
                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2" HorizontalAlign="Center"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="5%"></ItemStyle>
                                <ItemTemplate>
                                    <a target="_blank" href="<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>">
                                        <img src='<%= Config.Urls.Home%>/GroupIcon.ashx?groupID=<%# Eval("GroupID") %>&width=50&height=50&diskCache=1' border="0" />
                                    </a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="Name">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="14%"></ItemStyle>
                                <ItemTemplate>
                                    <a target="_blank" href="<%# UrlRewrite.CreateShowGroupUrl((string)Eval("GroupID"))%>">
                                    <%# Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                            <asp:TemplateColumn SortExpression="Owner">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="14%"></ItemStyle>
                                <ItemTemplate>
                                    <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Owner")%>">
                                        <%# Eval("Owner")%></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="True" Width="30%"></ItemStyle>
                                <ItemTemplate>
                                        <%# Eval("Category")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>                            
                            <asp:TemplateColumn SortExpression="DateCreated">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("DateCreated")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn SortExpression="AccessLevel">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("AccessLevel")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="12%"></ItemStyle>
                                <ItemTemplate>
                                    <nobr>
                                        <a href="EditGroup.aspx?id=<%# Eval("GroupID") %>">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%= Lang.TransA("Edit")%></a>
                                            &nbsp;
                                        <asp:LinkButton ID="lnkDeleteGroup" runat="server" CommandName="DeleteGroup" CommandArgument='<%# Eval("GroupID") %>' CssClass="gridlink">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%= Lang.TransA("Delete")%></asp:LinkButton>
                                    </nobr>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
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
                  <asp:Button ID="btnGetCSV" runat="server" Visible="false" 
                        onclick="btnGetCSV_Click" />   	        
 	           </div>
		    </div>
	    </form>
	</body>
</html>
