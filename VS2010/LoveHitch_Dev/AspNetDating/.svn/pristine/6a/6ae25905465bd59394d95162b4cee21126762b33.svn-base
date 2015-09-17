<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAffiliateCtrl.ascx.cs" Inherits="AspNetDating.Affiliates.EditAffiliateCtrl" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>

<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
<table border="0" cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <td align="center" class="table_header" colspan="2">
            <%= Lang.Trans("Account Information") %></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("Current password") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("New password") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("Confirm new password") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="password"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("Name") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("Email") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("Site URL") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtSiteURL" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.Trans("Payment Details")%></td>
        <td class="table_cell">
            <asp:TextBox ID="txtPaymentDetails" runat="server" TextMode="MultiLine" CssClass="textbox" Rows="5" Columns="50"></asp:TextBox></td>
    </tr>
</table>
<div class="separator10"></div>
<div class="add-buttons2">
	<div class="add-buttons">
        <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click"></asp:Button>
	</div>
</div>
<div class="separator10"></div>