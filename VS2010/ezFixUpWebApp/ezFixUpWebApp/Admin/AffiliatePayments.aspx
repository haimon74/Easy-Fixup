<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AffiliatePayments.aspx.cs" Inherits="ezFixUp.Admin.AffiliatePayments" %>
<%@ Import namespace="System.Globalization"%>

<%@ Import Namespace="ezFixUp.Classes" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head>
	    <title><%= Lang.TransA("Affiliate Request Payments") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all" />
	</head>
	<body>
	    <form id="form1" runat="server">
			<uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
			<div id="layout">
                <div id="crupms">
                    <img src="images/i_browsemessage.jpg" />
                    <div id="crumpstxt"><%= Lang.TransA("Affiliate Request Payments")%></div>
                </div>
                <div id="sidebar">
                    <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
                </div>
				<div id="main">
        			<div id="content-head">
						<div class="ch-head"><%= Lang.TransA("Affiliate Request Payments")%></div>
                        <div class="ch-description">
                            <%= Lang.TransA("Use this section to view affiliate request payments...") %>
                        </div>
					</div>
					<asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                    <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>
                <table width="100%" cellpadding="0" cellspacing="0">
                	<tr>
                		<td></td>
                	</tr>
                </table>
                    <asp:MultiView ID="mvAffiliateRequestPayments" runat="server">
                        <asp:View ID="viewPaymentRequests" runat="server">
                            <asp:Repeater ID="rptAffiliateRequestPayments" runat="server" 
                                    OnItemCommand="rptAffiliateRequestPayments_ItemCommand" 
                                    OnItemDataBound="rptAffiliateRequestPayments_ItemDataBound">
					            <ItemTemplate>
					                <table cellpadding="0" cellspacing="0" id="stable">
					            		<tr>
					            			<td colspan="2" class="table_header">
					            				<%= Lang.TransA("Affiliate Request Payment") %></td>
					            		</tr>
					            		<tr>
					            			<td class="table_cell" width="30"><%= Lang.TransA("Username") %>:
					            			</td>
					            			<td class="table_cell"><%# Eval("Username") %></td>
					            		</tr>
					            		<tr>
					            			<td class="table_cell"><%= Lang.TransA("Balance") %>:
					            			</td>
					            			<td class="table_cell"><%# Eval("Balance") %></td>
					            		</tr>
					            		<tr>
					            			<td class="table_cell"><%= Lang.TransA("Payment Details") %>:
					            			</td>
					            			<td class="table_cell"><%# Eval("PaymentDetails") %></td>
					            		</tr>
					            		<tr>
					            			<td colspan="2" class="table_cell">
					            				<div class="add-buttons">
						            				<asp:Button ID="btnMarkPaid" runat="server" CommandName="Pay" CommandArgument='<%# Eval("AffiliateID") %>'/>
					            				</div>
					            			</td>
					            		</tr>
					            	</table>
					            	<div class="separator10"></div>
					            </ItemTemplate>
					        </asp:Repeater>
                        </asp:View>
                        <asp:View ID="viewPay" runat="server">
                        	<table id="stable" cellpadding="0" cellspacing="0">
			            		<tr>
			            			<td colspan="2" class="table_header">
		            				<%= Lang.TransA("Affiliate Payment Notes") %></td>
			            		</tr>
                        		<tr>
                        			<td class="table_cell"><%= Lang.TransA("Amount") %>:</td>
                        			<td class="table_cell"><asp:TextBox ID="txtAmount" runat="server"></asp:TextBox><%= CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %></td>
                        		</tr>
                        		<tr>
                        			<td class="table_cell"><%= Lang.TransA("Notes") %>:</td>
                        			<td class="table_cell"><asp:TextBox ID="txtNotes" runat="server" TextMode="multiline" Rows="5" Columns="50"></asp:TextBox></td>
                        		</tr>
                        		<tr>
                        			<td class="table_cell"><%= Lang.TransA("Private Notes") %></td>
                        			<td class="table_cell"><asp:TextBox ID="txtPrivateNotes" runat="server" TextMode="multiline" Rows="5" Columns="50"></asp:TextBox></td>
                        		</tr>
                        		<tr>
                        			<td class="table_cell" colspan="2">
                        				<div class="add-buttons"><asp:Button ID="btnPay" runat="server" OnClick="btnPay_Click"/></div>
                        			</td>
                        		</tr>
                            </table>
			            	<div class="separator10"></div>
                         </asp:View>
                    </asp:MultiView>
					</ContentTemplate>
				    </asp:UpdatePanel>
                </div>
            </div>
	    </form>
	</body>
</html>
