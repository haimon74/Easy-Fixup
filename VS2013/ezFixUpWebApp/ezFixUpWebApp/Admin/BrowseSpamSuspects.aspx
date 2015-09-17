<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BrowseSpamSuspects.aspx.cs" Inherits="ezFixUp.Admin.BrowseSpamSuspects" %>

<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Browse Spam Suspects") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	    <form id="form1" runat="server">
        	<uc1:AdminHeader id="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
            	<div id="crupms">
                	<img src="images/i_browsemembers.jpg" />
                	<div id="crumpstxt"><%= Lang.TransA("Spam Suspects Management") %></div>
                </div>
                <div id="sidebar">
                	<uc1:AdminMenu id="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
                <div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Spam Suspects Management") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to browse, edit or delete spam suspects of your site...") %>
                        </div>
                    </div>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <uc1:messagebox id="MessageBox" runat="server" EnableViewState="false"></uc1:messagebox>
					<div class="separator10"></div>
                    <asp:MultiView ID="mvSpamSuspects" runat="server" ActiveViewIndex="0">
                    <asp:View ID="viewSpamSuspects" runat="server">
                    <table cellpadding="0" cellspacing="0" width="100%">
	                	<tr>
	                    	<td align="right" class="perpage">
	                        	<%= Lang.TransA("Users per page") %>:
	                            <asp:DropDownList ID="ddUsersPerPage" CssClass="pages" Font-Name="Verdana" runat="server"
                                        AutoPostBack="True" 
                                        onselectedindexchanged="ddUsersPerPage_SelectedIndexChanged">
                                    </asp:DropDownList>
	                            <div class="separator06"></div>
	                        </td>
						</tr>
					</table>
                    <asp:GridView ID="gvUsers" runat="server" 
                        AllowPaging="False" AllowSorting="True" AutoGenerateColumns="False" 
                        BorderWidth="0" CellPadding="0" CellSpacing="0" CssClass="btable1" 
                        GridLines="None" onrowcommand="gvUsers_RowCommand" 
                        onrowcreated="gvUsers_RowCreated" onrowdatabound="gvUsers_RowDataBound" 
                        onsorting="gvUsers_Sorting" Width="100%">
                        <AlternatingRowStyle CssClass="btable2" />
                        <Columns>
                            <asp:TemplateField SortExpression="Username">
                                <HeaderStyle CssClass="table_header2" />
                                <ItemStyle CssClass="table_cell3" Wrap="False" />
                                <ItemTemplate>
                                    <a class="gridlink" 
                                        href='<%# Config.Urls.Home + "/ShowUser.aspx?uid=" + Eval("Username") %>' 
                                        target="_blank"><%# Eval("Username") %> </a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="table_header2" />
                                <ItemStyle CssClass="table_cell3" Width="14%" Wrap="False" />
                                <ItemTemplate>
                                    <%# Eval("Message") %>
                                    <br />
                                    <a href='BrowseMessages.aspx?uid=<%# Eval("Username") %>'><%= Lang.TransA("View messages") %></a>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle CssClass="table_header2" />
                                <ItemStyle CssClass="table_cell3" Width="12%" Wrap="False" />
                                <ItemTemplate>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lnkDeleteUser" runat="server" 
                                                CommandArgument='<%# Eval("Username") %>' CommandName="DeleteUser" 
                                                CssClass="gridlink"><%# Lang.TransA("Delete User")%></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkDeleteUserAndMessages" runat="server" 
                                                CommandArgument='<%# Eval("Username") %>' 
                                                CommandName="DeleteUserAndMessages" CssClass="gridlink"><%# Lang.TransA("Delete User and Messages")%></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkMarkAsReviewed" runat="server" 
                                                CommandArgument='<%# Eval("Username") %>' CommandName="MarkAsReviewed" 
                                                CssClass="gridlink"><%# Lang.TransA("Mark as Reviewed")%></asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lnkSendMessage" runat="server" 
                                                CommandArgument='<%# Eval("Username") %>' CommandName="SendMessage" 
                                                CssClass="gridlink"><%# Lang.TransA("Send message")%></asp:LinkButton>
                                        </li>
                                    </ul>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerSettings Mode="Numeric" Position="Bottom" />
                    </asp:GridView>
                        <asp:Panel ID="pnlPaginator" runat="server">
                            <div class="separator06"></div>
                            <table cellspacing="0" cellpadding="0" width="100%" class="pagenumber">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkFirst" CssClass="pagelink" runat="server" 
                                            onclick="lnkFirst_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkPrev" CssClass="pagelink" runat="server" 
                                            onclick="lnkPrev_Click"></asp:LinkButton>
                                        <asp:Label ID="lblPager" runat="server"></asp:Label>
                                        <asp:LinkButton ID="lnkNext" CssClass="pagelink" runat="server" 
                                            onclick="lnkNext_Click"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkLast" CssClass="pagelink" runat="server" 
                                            onclick="lnkLast_Click"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </asp:View>
                    <asp:View ID="viewSendMessage" runat="server">
                        <%= Lang.TransA("Message") %>:
                        <asp:TextBox ID="txtMessage" runat="server"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnSendMessage" runat="server" onclick="btnSendMessage_Click" />
                        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click"/>
                    </asp:View>
                    <asp:View ID="viewDeleteReason" runat="server">
                        <%= Lang.TransA("Reason") %>:
                        <asp:TextBox ID="txtDeleteReason" runat="server"></asp:TextBox>
                        <br />
                        <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" />
                    </asp:View>
                    </asp:MultiView>
                   </ContentTemplate>
                   </asp:UpdatePanel>    
 	           </div>
		    </div>
	    </form>
	</body>
</html>
