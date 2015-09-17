<%@ Page Language="c#" Codebehind="EditTopics.aspx.cs" AutoEventWireup="True" Inherits="AspNetDating.Admin.EditTopics" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >

<html>
	<head>
	    <title><%= Lang.TransA("Edit Topics") %></title>
	    <meta content="Microsoft Visual Studio .NET 7.1" name="GENERATOR">
	    <meta content="C#" name="CODE_LANGUAGE">
	    <meta content="JavaScript" name="vs_defaultClientScript">
	    <meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
   		<form id="Form1" method="post" runat="server">
    	    <uc1:adminheader id="AdminHeader1" runat="server"></uc1:adminheader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_question.jpg">
                    <div id="crumpstxt"><%= Lang.TransA("Edit Topics") %></div>
                </div>
                <div id="sidebar">
                    <uc1:adminmenu id="AdminMenu1" runat="server"></uc1:adminmenu>
                </div>
                <div id="main">
                	<div id="content-head">
                        <div class="ch-head"><%= Lang.TransA("Edit Topics") %></div>
                        <div class="ch-description">
                            <%= Lang.TransA("In this section you can create new topics or modify existing ones...") %>
                        </div>
                    </div>
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
                    <asp:DataGrid ID="dgTopics" Width="100%" AutoGenerateColumns="False" AllowPaging="False"
                        PageSize="10" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0"
                        runat="server" GridLines="None">
                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                        <HeaderStyle></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="2%"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="cbSelect" value='<%# Eval("TopicID") %>'
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
                                <HeaderStyle CssClass="table_header2c" Wrap="False"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3c" Width="20%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("EditColumns")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2c" Wrap="False"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3c" Width="20%"></ItemStyle>
                                <ItemTemplate>
                                    <%# Eval("ViewColumns")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3c" Width="20%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton Text="<IMG title=&quot;move up&quot; border=0 SRC=images\up.gif>"
                                        ID="lnkUp" CommandName="ChangeOrder" CommandArgument="Up" runat="server"></asp:LinkButton>
                                    <asp:LinkButton Text="<IMG title=&quot;move down&quot; border=0 SRC=images\down.gif>"
                                        ID="lnkDown" CommandName="ChangeOrder" CommandArgument="Down" runat="server"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2c"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3c" Width="20%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkEdit" CommandName="EditTopic" runat="server">
										<img src="images/edit_icon.jpg" border="0">
										<%# Lang.TransA("Edit")%></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle HorizontalAlign="Right" Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                    <div class="separator10"></div>
                    <div class="add-buttons">
						<asp:Button ID="btnAddNewTopic" runat="server" OnClick="btnAddNewTopic_Click"></asp:Button>
                        <asp:Button ID="btnDeleteSelectedTopics" runat="server" OnClick="btnDeleteSelectedTopics_Click"></asp:Button>
					</div>
                    <div class="separator10"></div>
            	</div>
	        </div>
	    </form>
	</body>
</html>
