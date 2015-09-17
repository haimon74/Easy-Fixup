<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageGroupCategories.aspx.cs" Inherits="AspNetDating.Admin.ManageGroupCategories" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Manage Categories") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
            <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
            	<div id="crupms">
                	<img src="images/i_categories.jpg" />
                    <div id="crumpstxt"><%= Lang.TransA("Manage Categories") %></div>
				</div>
                <div id="sidebar">
                	<uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
                <div id="main">
				    <div id="content-head">
                    	<div class="ch-head"><%= Lang.TransA("Manage Categories")%></div>
                        <div class="ch-description">
                        	<%= Lang.TransA("In this section you can create new categories or modify existing ones...")%>
                        </div>
                    </div>   
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>                
                    <asp:MultiView ID="mvCategories" runat="server">
                    	<asp:View ID="viewCategories" runat="server">    
                    		<asp:DataGrid ID="dgCategories" Width="100%" AutoGenerateColumns="False" AllowPaging="False"
                            PageSize="10" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0" GridLines="None"
                            runat="server" OnItemCommand="dgCategories_ItemCommand" OnItemCreated="dgCategories_ItemCreated" OnItemDataBound="dgCategories_ItemDataBound">
                            	<AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                            	<HeaderStyle></HeaderStyle>
                            	<Columns>
                                	<asp:TemplateColumn>
                                    	<HeaderStyle CssClass="table_header2"></HeaderStyle>
                                        <ItemStyle CssClass="table_cell3" Wrap="False" Width="1%"></ItemStyle>
                                        <ItemTemplate>
                                        	<input type="checkbox" id="cbSelect" value='<%# Eval("CategoryID") %>' runat="server" />
                                        </ItemTemplate>
									</asp:TemplateColumn>
								<asp:TemplateColumn>
									<HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
									<ItemStyle CssClass="table_cell3" Wrap="True" Width="30%"></ItemStyle>
                                		<ItemTemplate>
                                	    	<%# Eval("Name")%>
                                	    </ItemTemplate>
                               		</asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                    	<HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                    	<ItemStyle CssClass="table_cell3c" Width="30%"></ItemStyle>
                                        <ItemTemplate>
                                        	<asp:LinkButton Text="<IMG title=&quot;move up&quot; border=0 SRC=images\up.gif>"
                                            	ID="lnkUp" CommandName="ChangeOrder" CommandArgument="Up" runat="server"></asp:LinkButton>
                                        	<asp:LinkButton Text="<IMG title=&quot;move down&quot; border=0 SRC=images\down.gif>"
                                            	ID="lnkDown" CommandName="ChangeOrder" CommandArgument="Down" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
									</asp:TemplateColumn>
									<asp:TemplateColumn>
										<HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                        <ItemStyle CssClass="table_cell3c" Width="30%"></ItemStyle>
                                        <ItemTemplate>
											<asp:LinkButton ID="lnkEdit" CommandName="EditCategory" runat="server">
												<img src="images/edit_icon.jpg" border="0">
											    <%# Lang.TransA("Edit")%></asp:LinkButton>
											<asp:LinkButton ID="lnkViewGroups" CommandArgument='<%# Eval("CategoryID") %>' CommandName="ViewGroups" runat="server">
												<img src="images/edit_icon.jpg" border="0">
											    <%# Lang.TransA("Groups")%></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                                <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
							</asp:DataGrid>
                            <table width="100%">
                            	<tr>
                                	<td>
	                                	<div class="add-buttons">
	                                    	<asp:Button ID="btnAddNewCategory" runat="server" OnClick="btnAddNewCategory_Click"></asp:Button>
	                                        <asp:Button ID="btnDeleteSelectedCategories" runat="server" OnClick="btnDeleteSelectedCategories_Click">
	                                        </asp:Button>
										</div>
                                    </td>
                                </tr>
                            </table>
						</asp:View>
                        <asp:View ID="viewCategory" runat="server">
                        	<table cellpadding="0" cellspacing="0" width="100%">
                        		<tr>
                        			<td></td>
                        		</tr>
                        	</table>
							<table cellpadding="0" cellspacing="0" class="stable2">
                          		<tr>
                          			<td colspan="2" class="table_header">
                          		    	<asp:Label ID="lblText" runat="server"></asp:Label> </td>
								</tr>
								<tr>
									<td class="table_cell">
                          				<%= Lang.TransA("Name") %>
                          			</td>
									<td class="table_cell" Width="90%">
                          			    <asp:TextBox ID="txtName" CssClass="textbox" Width="100%" runat="server"></asp:TextBox>                                
                          			</td>
								</tr>
								<tr>
									<td class="table_cell" colspan="2">
                          			    <asp:CheckBox ID="cbUsersCanCreateGroups" runat="server" /><%= Lang.TransA("Users can create groups in this category") %>
                          			</td>
								</tr>								
								<tr>
									<td colspan="2" >
										<div class="separator06"></div>
					                    <div class="add-buttons">
				                        	<asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
				                        	<asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
										</div>
									</td>
								</tr>
							</table>
							<div class="separator10"></div>
						</asp:View>
					</asp:MultiView>
	            </div>
	        </div>
	    </form>
	</body>
</html>
