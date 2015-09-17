<%@ Page Language="C#" AutoEventWireup="true" Codebehind="PaymentHistory.aspx.cs"
    Inherits="ezFixUp.Affiliates.PaymentHistory" %>

<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="AffilaiteHeader" Src="AffiliateHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AffiliateMenu" Src="AffiliateMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditAffiliateCtrl" Src="~/Affiliates/EditAffiliateCtrl.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
<html>
<head>
    <title>
        <%= Lang.Trans("Payment Details") %>
    </title>
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
</head>
<body>
    <form id="form1" runat="server">
        <uc1:AffilaiteHeader ID="AffiliateHeader1" runat="server"></uc1:AffilaiteHeader>
        <div id="layout">
            <div id="crupms">
                <img src="images/i_paymenthistory.jpg" />
                <div id="crumpstxt">
                    <%= Lang.Trans("Payment Details") %>
                </div>
            </div>
            <div id="sidebar">
                <uc1:AffiliateMenu ID="AffiliateMenu1" runat="server"></uc1:AffiliateMenu>
            </div>
            <div id="main">
                <div id="content-head">
                    <div class="ch-head">
                        <%= Lang.Trans("Payment Details") %>
                    </div>
                    <div class="ch-description">
                        <%= Lang.Trans("Use this section to view your payment details...") %>
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
                                <asp:DropDownList ID="ddAffiliatesPaymentHistoryPerPage" CssClass="pages" Font-Name="Verdana"
                                    runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddAffiliatesPaymentHistoryPerPage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </asp:Panel>
                            <div class="separator06">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        <table class="stable2" cellpadding="0" cellspacing="0">
                        	<tr>
                        		<td align="center" class="table_header">&nbsp;</td>
                        	</tr>
                        	<tr>
                        		<td class="table_cell">
                        			<%= Lang.Trans("Your balance is") %>:
                            		<b><%= Affiliate.Balance.ToString("C") %></b>
                            		<br>
									<div class="separator06"></div>
                        			<div class="add-buttons">
                        				<asp:Button ID="btnRequestPayment" runat="server" OnClick="btnRequestPayment_Click" />
                        			</div>

                        		</td>
                        	</tr>
                        </table>                           
							<div class="separator06"></div>
                            <div class="label">
                            <asp:Label ID="lblPaymentHistory" runat="server"></asp:Label>
                            </div>
							<div class="separator06"></div>
                            <asp:GridView ID="gvPaymentHistory" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                PageSize="1" PagerStyle-CssClass="table_cell" PagerStyle-Font-Names="Verdana" CssClass="btable1"
                                PagerStyle-Font-Size="11px" HorizontalAlign="Center" PagerSettings-Mode="Numeric" GridLines="None" 
                                CellPadding="0" CellSpacing="0" BorderWidth="0" Width="100%"
                                OnPageIndexChanging="gvPaymentHistory_PageIndexChanging">
                                <AlternatingRowStyle CssClass="btable2"></AlternatingRowStyle>
                                <Columns>
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
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <div class="separator10"></div>
            </ContentTemplate>
            </asp:UpdatePanel>
            </div>
        </div>
    </form>
</body>
</html>
