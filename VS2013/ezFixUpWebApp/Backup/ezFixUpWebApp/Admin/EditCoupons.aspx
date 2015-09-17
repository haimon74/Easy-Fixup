<%@ Page Language="C#" AutoEventWireup="true" Codebehind="EditCoupons.aspx.cs" Inherits="ezFixUp.Admin.EditCoupons" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">

<html>
	<head runat="server">
	    <title><%= Lang.TransA("Edit Coupons") %></title>
	    <link rel="stylesheet" type="text/css" href="images/style.css" media="all"/>
	</head>
	<body>
	    <form id="form1" runat="server">
	        <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
	        <div id="layout">
	            <div id="crupms">
	                <img src="images/i_coupons.jpg" alt=''/>
	                <div id="crumpstxt">
	                    <%= Lang.TransA("Edit Coupons")%>
	                </div>
	            </div>
	            <div id="sidebar">
	                <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
	            </div>
	            <div id="main">
	                <div id="content-head">
	                    <div class="ch-head">
	                        <%= Lang.TransA("Edit Coupons")%>
	                    </div>
	                    <div class="ch-description">
	                        <%= Lang.TransA("Use this section to edit your website coupons...") %>
	                    </div>
	                </div>
	                <asp:UpdatePanel ID="UpdatePanelSearchCoupons" runat="server">
	                    <ContentTemplate>
                            <uc1:messagebox id="MessageBox" runat="server"></uc1:messagebox>	                    
	                        <div id="pnlCoupon" runat="server" visible="false">
	                            <table cellpadding="0" cellspacing="0" class="stable2">
	                                <tr>
	                                    <td align="center" colspan="2" class="table_header">
	                                        <%= Lang.TransA("Coupon details") %></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Coupon Code") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:TextBox ID="txtCouponName" Width="100%" CssClass="textbox2" runat="server" Enabled="false"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Coupon Type") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:DropDownList ID="ddlCouponType" Width="100%" CssClass="textbox2" runat="server"></asp:DropDownList></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Start Date") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:TextBox ID="txtStartDate" Width="100%" CssClass="textbox2" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Due Date") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:TextBox ID="txtDueDate" Width="100%" CssClass="textbox2" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Owner Username") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:TextBox ID="txtUsername" Width="100%" CssClass="textbox2" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("Maximum Uses") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:TextBox ID="txtMaxUse" Width="100%" CssClass="textbox2" runat="server"></asp:TextBox></td>
	                                </tr>
	                                <tr>
	                                    <td class="table_cell" width="40%">
	                                        <%= Lang.TransA("When can use") %></td>
	                                    <td class="table_cell" width="60%">
	                                        <asp:CheckBox ID="cbRegistrationOnly" Width="100%" CssClass="textbox2" runat="server" Text="Only at Registration"></asp:CheckBox></td>
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
	                        <div id="pnlCoupons" runat="server" align="center">
	                            <asp:DataGrid ID="dgCoupons" runat="server" AutoGenerateColumns="false" OnItemCommand="dgCoupons_ItemCommand"
	                                OnItemCreated="dgCoupons_ItemCreated" OnItemDataBound="dgCoupons_ItemDataBound"
	                                Width="100%" CssClass="btable1" CellPadding="0" CellSpacing="0" BorderWidth="0"
	                                GridLines="None">
	                                <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
	                                <HeaderStyle></HeaderStyle>
	                                <Columns>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Code</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# Eval("CouponCode")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Type</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# Eval("CouponType")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Owner</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# Eval("CouponOwner")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Start At</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# (DateTime)Eval("StartDate")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Due Date</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# (DateTime)Eval("DueDate")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Max Use</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# (int)Eval("MaxUse")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>#Used</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <%# (int)Eval("Used")%>
	                                        </ItemTemplate>
	                                    </asp:TemplateColumn>
	                                    <asp:TemplateColumn>
	                                        <HeaderStyle CssClass="table_header2"></HeaderStyle>
	                                        <HeaderTemplate>Commands</HeaderTemplate>
	                                        <ItemStyle CssClass="table_cell3"></ItemStyle>
	                                        <ItemTemplate>
	                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# (string)Eval("CouponCode") %>'
	                                                CommandName="EditCoupon" CssClass="gridlink">
											      <img src="images/edit_icon.jpg" border="0">
											        <%# Lang.TransA("Edit")%>
	                                            </asp:LinkButton>
	                                            &nbsp;&nbsp;
	                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# (string)Eval("CouponCode") %>'
	                                                CommandName="DeleteCoupon" >
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
