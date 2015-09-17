<%@ Page Language="C#" AutoEventWireup="true" Codebehind="EditLanguages.aspx.cs" Inherits="AspNetDating.Admin.EditLanguages" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head runat="server">
	    <title><%= Lang.TransA("Edit Languages") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
	</head>
	<body>
	    <form id="form1" runat="server">
	        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
	        <div id="layout">
	            <div id="crupms">
	                <img src="images/i_languages.jpg">
	                <div id="crumpstxt">
	                    <%= Lang.TransA("Edit Languages") %>
	                </div>
	            </div>
	            <div id="sidebar">
	                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
	            </div>
	            <div id="main">
	                <div id="content-head">
	                    <div class="ch-head">
	                        <%= Lang.TransA("Edit Languages") %>
	                    </div>
	                    <div class="ch-description">
	                        <%= Lang.TransA("Use this section to edit your website languages...") %>
	                    </div>
	                </div>
	                <asp:UpdatePanel ID="UpdatePanelSearchLanguages" runat="server">
	                    <ContentTemplate>
                            <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>	                    
	                        <div id="pnlLanguage" runat="server" visible="false">
	                            <table cellpadding="0" cellspacing="0" class="stable2">
	                                <tr>
	                                    <td align="center" colspan="2" class="table_header">
	                                        <%= Lang.TransA("Language details") %></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Language name") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:TextBox ID="txtLanguageName" Width="100%" CssClass="textbox2" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell">
	                                        <%= Lang.TransA("Active") %></td>
	                                    <td class="table_cell">
	                                        <asp:CheckBox ID="cbActive" runat="server" /></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell">
	                                        <%= Lang.TransA("Browser Languages") %></td>
	                                    <td class="table_cell">
	                                        <asp:TextBox ID="txtBrowserLanguages" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell">
	                                        <%= Lang.TransA("IP Countries") %></td>
	                                    <td class="table_cell">
	                                        <asp:TextBox ID="txtIpCountries" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell">
	                                        <%= Lang.TransA("Theme") %></td>
	                                    <td class="table_cell">
	                                        <asp:TextBox ID="txtTheme" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td colspan="2" class="table_cell">
	                                        <div class="add-buttons">
	                                            <div class="separator06">
	                                            </div>
	                                            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
	                                            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
	                                        </div>
	                                    </td>
	                                </tr>
	                            </table>
	                        </div>
	                        <div class="separator10"></div>
	                        <table cellpadding="0" cellspacing="0" width="100%">
	                        	<tr>
	                        		<td></td>
	                        	</tr>
	                        </table>
	                        <div id="pnlLanguages" runat="server" align="center">
	                            <asp:DataGrid ID="dgLanguages" runat="server" AutoGenerateColumns="false" OnItemCommand="dgLanguages_ItemCommand"
	                                OnItemCreated="dgLanguages_ItemCreated" OnItemDataBound="dgLanguages_ItemDataBound"
	                                Width="100%" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0"
	                                GridLines="None">
	                                <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
	                                <HeaderStyle></HeaderStyle>
	                                <Columns>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# Eval("LanguageName") %>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# (bool) Eval("IsActive") ? Lang.TransA("Yes") : Lang.TransA("No") %>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <asp:LinkButton ID="lnkUp" CommandName="MoveUp" CommandArgument='<%# Eval("LanguageID") %>'
	                                                runat="server" Text="<IMG border=0 SRC=images\up.gif>"></asp:LinkButton>
	                                            <asp:LinkButton ID="lnkDown" CommandName="MoveDown" CommandArgument='<%# Eval("LanguageID") %>'
	                                                runat="server" Text="<IMG border=0 SRC=images\down.gif>"></asp:LinkButton>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("LanguageID") %>'
	                                                CommandName="EditLanguage" CssClass="gridlink">
											      <img src="images/edit_icon.jpg" border="0">
											        <%# Lang.TransA("Edit")%>
	                                            </asp:LinkButton>
	                                            &nbsp;&nbsp;
	                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("LanguageID") %>'
	                                                CommandName="DeleteLanguage" Visible='<%# ! (bool) Eval("Predefined") %>'>
											        <%# Lang.TransA("Delete")%>
	                                            </asp:LinkButton>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                </Columns>
	                            </asp:DataGrid>
	                            <div class="separator06"></div>
	                            <div class="add-buttons">
	                                <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" />
	                            </div>
	                            <div class="separator06"></div>
	                        </div>
	                    </ContentTemplate>
	                </asp:UpdatePanel>
	            </div>
	        </div>
	    </form>
	</body>
</html>
