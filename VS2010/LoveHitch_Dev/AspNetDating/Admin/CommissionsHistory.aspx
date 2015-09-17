<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CommissionsHistory.aspx.cs" Inherits="AspNetDating.Admin.CommissionsHistory" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>
        <%= Lang.TransA("Affiliate Commissions") %>
    </title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div id="layout">
            <div id="crupms">
                <img src="images/i_billingsettings.jpg" />
                <div id="crumpstxt">
                    <%= Lang.TransA("Affiliate Commissions")%>
                </div>
            </div>
            <div id="sidebar">
                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
            </div>
            <div id="main">
                <div id="content-head">
                    <div class="ch-head">
                        <%= Lang.TransA("Affiliate Commissions")%>
                    </div>
                    <div class="ch-description">
                        <%= Lang.TransA("Use this section to view affiliate commissions...") %>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <uc1:MessageBox ID="MessageBox" runat="server"></uc1:MessageBox>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right" class="perpage">
                            <asp:Panel ID="pnlAffiliateCommissionsPerPage" runat="server">
                                <asp:Label ID="lblAffiliateCommissionsPerPage" Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>:
                                <asp:DropDownList ID="ddAffiliateCommissionsPerPage" CssClass="pages" Font-Name="Verdana" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddAffiliateCommissionsPerPage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </asp:Panel>
                            <div class="separator06">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="table_cell3">
                            <%= Lang.TransA("Affiliate") %>:
                            <asp:DropDownList ID="ddAffiliates" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddAffiliates_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                            <div class="separator06"></div>
                            <asp:GridView ID="gvCommissions" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="1" PagerStyle-CssClass="table_cell" PagerStyle-Font-Names="Verdana" Width="100%" CssClass="btable1"
                                PagerStyle-Font-Size="11px" HorizontalAlign="Center" PagerSettings-Mode="Numeric" OnPageIndexChanging="gvCommissions_PageIndexChanging"
                                BorderWidth="0" CellPadding="0"
                                 CellSpacing="0" GridLines="None">
                                <AlternatingRowStyle CssClass="btable2"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
									<ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("Username")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
									<ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("Amount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
									<ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("TimeStamp")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
									<ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("Notes") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
               </ContentTemplate>
              </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
