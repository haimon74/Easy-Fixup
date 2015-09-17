<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditAdsCategories.aspx.cs" Inherits="AspNetDating.Admin.EditAdsCategories" %>

<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title><%= Lang.TransA("Edit Classified Categories") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
   		<form id="Form1" method="post" runat="server">
    	    <uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_question.jpg">
                    <div id="crumpstxt"><%= Lang.TransA("Edit Classified Categories")%></div>
                </div>
                <div id="sidebar">
                    <uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
                </div>
                <div id="main">
                	<div id="content-head">
                        <div class="ch-head"><%= Lang.TransA("Edit Classified Categories") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("In this section you can create new categories or modify existing ones...") %>
                        </div>
                    </div>
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
                    <asp:DataGrid ID="dgAdsCategories" Width="100%" AutoGenerateColumns="False" AllowPaging="False"
                        PageSize="10" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0"
                        runat="server" GridLines="None" 
                        onitemcommand="dgAdsCategories_ItemCommand">
                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                        <HeaderStyle></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="2%"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect" value='<%# Eval("ID") %>'
                                        runat="server" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2" Wrap="False">
                                </HeaderStyle>
                                <ItemStyle CssClass="table_cell3"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("Title")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3c" Width="20%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="EditCategory" runat="server">
										<img src="images/edit_icon.jpg" border="0">
										<%# Lang.TransA("Edit")%></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <div class="separator10"></div>
                    <div class="add-buttons">
						<asp:Button ID="btnAddNewCategory" runat="server" 
                            onclick="btnAddNewCategory_Click"></asp:Button>
                        <asp:Button ID="btnDeleteSelectedCategories" runat="server" 
                            onclick="btnDeleteSelectedCategories_Click"></asp:Button>
					</div>
                    <div class="separator10"></div>
            	</div>
	        </div>
	    </form>
	</body>
</html>
