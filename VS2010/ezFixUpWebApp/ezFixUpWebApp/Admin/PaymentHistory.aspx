<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PaymentHistory.aspx.cs"
    Inherits="ezFixUp.Admin.PaymentHistory" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>
        <%= Lang.TransA("Payment History") %>
    </title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
        <div id="layout">
            <div id="crupms">
                <img src="images/i_approveanswers.jpg" />
                <div id="crumpstxt">
                    <%= Lang.TransA("Payment History") %>
                </div>
            </div>
            <div id="sidebar">
                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
            </div>
            <div id="main">
                <div id="content-head">
                    <div class="ch-head">
                        <%= Lang.TransA("Payment History") %>
                    </div>
                    <div class="ch-description">
                        <%= Lang.TransA("Use this section to view affiliate payment history...") %>
                    </div>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                <uc1:MessageBox ID="MessageBox" runat="server"></uc1:MessageBox>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="right" class="perpage">
                            <asp:Panel ID="pnlPaymentHistoryPerPage" runat="server">
                                <asp:Label ID="lblPaymentHistoryPerPage" Font-Name="Verdana" Font-Size="11px" runat="server"></asp:Label>:
                                <asp:DropDownList ID="ddAffiliatesPaymentHistoryPerPage" CssClass="pages" Font-Name="Verdana" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddAffiliatesPaymentHistoryPerPage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </asp:Panel>
                            <div class="separator06">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="1" PagerStyle-CssClass="table_cell" PagerStyle-Font-Names="Verdana" CssClass="btable1"
                                PagerStyle-Font-Size="11px" HorizontalAlign="Center" PagerSettings-Mode="Numeric" OnPageIndexChanging="gvPaymentHistory_PageIndexChanging"
                                GridLines="None" 
                                CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%">
                                <AlternatingRowStyle CssClass="btable2"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField>
										<HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("AffiliateUsername") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
										<HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("Amount") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
										<HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("DatePaid") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
										<HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("Notes") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
										<HeaderStyle CssClass="table_header2" Wrap="False"></HeaderStyle>
										<ItemStyle CssClass="table_cell3"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("PrivateNotes") %>
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
