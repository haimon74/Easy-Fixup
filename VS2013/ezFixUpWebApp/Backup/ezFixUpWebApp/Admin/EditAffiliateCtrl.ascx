<%@ Import namespace="System.Globalization"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAffiliateCtrl.ascx.cs" Inherits="ezFixUp.Admin.EditAffiliateCtrl" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>

<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
<table border="0" cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <td align="center" class="table_header" colspan="2">
            <%= Lang.TransA("Affilaite Information") %></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Username") %></td>
        <td class="table_cell">
            <%= Affiliate.Username %></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Password") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Name") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Email")%></td>
        <td class="table_cell">
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Site URL") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtSiteURL" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Payment details") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtPaymentDetails" runat="server" TextMode="MultiLine" CssClass="textbox" Rows="5" Columns="50"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Deleted") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddDeleted" runat="server"></asp:DropDownList></td>
    </tr>
    <tr >
        <td class="table_cell">
            <%= Lang.TransA("Active") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddActive" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Percentage") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtPercentage" runat="server"></asp:TextBox>%
        </td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Fixed amount") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtFixedAmount" runat="server"></asp:TextBox><%= CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %>
        </td>
    </tr>
    <tr >
        <td class="table_cell">
            <%= Lang.TransA("Recurrent") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddRecurrent" runat="server"></asp:DropDownList>
        </td>
    </tr>
    <tr >
        <td class="table_cell">
            <%= Lang.TransA("Balance") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtBalance" runat="server"></asp:TextBox><%= CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol %>
        </td>
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