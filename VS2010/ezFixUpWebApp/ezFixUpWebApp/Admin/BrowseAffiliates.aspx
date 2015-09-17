<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowseAffiliates.aspx.cs" Inherits="ezFixUp.Admin.BrowseAffiliates" %>

<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Browse Affiliates") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	    <form id="form1" runat="server">
        	<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
            	<div id="crupms">
                	<img src="images/i_browsemembers.jpg" />
                	<div id="crumpstxt"><%= Lang.TransA("Affiliate Management") %></div>
                </div>
                <div id="sidebar">
                	<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
                <div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Affiliate Management") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to browse, edit or delete affiliates of your site...") %>
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
                            	<%= Lang.TransA("Username") %>:
                            </td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
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
                        	    <%= Lang.TransA("Email") %>:
                        	</td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                        	<td class="table_cell">
                        	    <%= Lang.TransA("Site URL") %>:
                        	</td>
                            <td class="table_cell">
                            	<asp:TextBox ID="txtSiteURL" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="table_cell">
                                <%= Lang.TransA("Deleted") %>:
                            </td>
                	        <td class="table_cell">
                        		<asp:DropDownList ID="ddDeleted" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="table_cell">
                                <%= Lang.TransA("Active") %>:
                            </td>
                	        <td class="table_cell">
                        		<asp:DropDownList ID="ddActive" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="table_cell">
                                <%= Lang.TransA("Request payment") %>:
                            </td>
                	        <td class="table_cell">
                        		<asp:DropDownList ID="ddRequestPayment" runat="server"></asp:DropDownList>
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
                                <%= Lang.TransA("Affiliates per page") %>:
                                <asp:DropDownList ID="ddAffiliatesPerPage" CssClass="pages" Font-Name="Verdana" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddAffiliatesPerPage_SelectedIndexChanged">
                                </asp:DropDownList>
                                <div class="separator06"></div>
                            </td>
                        </tr>
					</table>
                    <asp:GridView Width="100%" CssClass="btable1" ID="gvAffiliates" GridLines="None" runat="server" AllowPaging="False"
                        AutoGenerateColumns="False" AllowSorting="True" CellPadding="0" CellSpacing="0"
                        BorderWidth="0" OnRowCommand="gvAffiliates_RowCommand" OnRowCreated="gvAffiliates_RowCreated" OnSorting="gvAffiliates_Sorting">
                        <AlternatingRowStyle CssClass="btable2"/>
                        <Columns>
                            <asp:TemplateField SortExpression="Username">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Username") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Name">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="14%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Name") %>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                        <%# Eval("SiteURL")%></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Balance">
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="True"></ItemStyle>
                                <ItemTemplate>
                                        <%# Eval("Balance")%>
                                </ItemTemplate>
                            </asp:TemplateField>                            
                            <asp:TemplateField>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Convert.ToBoolean(Eval("RequestPayment")) ? Lang.TransA("Yes") : Lang.TransA("No")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Commission")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="12%"></ItemStyle>
                                <ItemTemplate>
                                    <nobr>
                                        <a href="EditAffiliate.aspx?id=<%# Eval("AffiliateID") %>">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%= Lang.TransA("Edit")%></a>
                                            &nbsp;
                                        <asp:LinkButton ID="lnkDeleteGroup" runat="server" CommandName="DeleteAffiliate" CommandArgument='<%# Eval("AffiliateID") %>' CssClass="gridlink">
                                            <img src="images/edit_icon.jpg" border="0">
                                            <%= Lang.TransA("Delete")%></asp:LinkButton>
                                    </nobr>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Position="Bottom" Mode="Numeric"/>
                    </asp:GridView>
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