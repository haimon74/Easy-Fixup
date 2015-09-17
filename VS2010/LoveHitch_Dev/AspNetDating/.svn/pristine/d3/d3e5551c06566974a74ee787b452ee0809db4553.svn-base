<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CommissionsHistory.aspx.cs"
    Inherits="AspNetDating.Affiliates.CommissionsHistory" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AffilaiteHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>
        <%= Lang.Trans("Commissions") %>
    </title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AffilaiteHeader ID="AffiliateHeader1" runat="server"></uc1:AffilaiteHeader>
        <div id="layout">
            <div id="crupms">
                <img src="images/i_commissionshistory.jpg" />
                <div id="crumpstxt">
                    <%= Lang.Trans("Commissions")%>
                </div>
            </div>
            <div id="sidebar">
                <uc1:AffiliateMenu ID="AffiliateMenu1" runat="server"></uc1:AffiliateMenu>
            </div>
            <div id="main">
                <div id="content-head">
                    <div class="ch-head">
                        <%= Lang.Trans("Commissions")%>
                    </div>
                    <div class="ch-description">
                        <%= Lang.Trans("Use this section to view your commissions...") %>
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
                                <asp:DropDownList ID="ddAffiliateCommissionsPerPage" CssClass="pages" Font-Name="Verdana"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddAffiliateCommissionsPerPage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </asp:Panel>
                            <div class="separator06">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvCommissions" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="1" PagerStyle-CssClass="table_cell" PagerStyle-Font-Names="Verdana" Width="100%" CssClass="btable1"
                                PagerStyle-Font-Size="11px" HorizontalAlign="Center" PagerSettings-Mode="Numeric"  BorderWidth="0" CellPadding="0"
                                 CellSpacing="0" GridLines="None"
                                OnPageIndexChanging="gvCommissions_PageIndexChanging">
                                <AlternatingRowStyle CssClass="btable2"></AlternatingRowStyle>
                                <Columns>
                                    <asp:TemplateField>
                                    <HeaderStyle CssClass="table_header2"></HeaderStyle>
									<ItemStyle CssClass="table_cell3" Wrap="False"></ItemStyle>
                                        <ItemTemplate>
                                            <%# Eval("Username") %>
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
                                            <%# Eval("TimeStamp") %>
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
