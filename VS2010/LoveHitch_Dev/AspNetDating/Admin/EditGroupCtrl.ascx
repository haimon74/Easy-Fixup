<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditGroupCtrl.ascx.cs" Inherits="AspNetDating.Admin.EditGroupCtrl" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
<table border="0" cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <td align="center" class="table_header" colspan="2">
            <%= Lang.TransA("Group Information") %></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Group Icon") %></td>
        <td class="table_cell">
            <img src='<%= Config.Urls.Home%>/GroupIcon.ashx?groupID=<%= GroupID %>&width=150&height=150&diskCache=1' /></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Name") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Categories") %></td>
        <td class="table_cell">
            <asp:ListBox ID="lbCategories" runat="server" SelectionMode="Multiple"></asp:ListBox>
            <br />
            *<%= Lang.TransA("Hold Ctrl for multiple selection") %></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Description")%></td>
        <td class="table_cell">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="textbox" Rows="5" Columns="50"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Group Image") %></td>
        <td class="table_cell">
            <asp:FileUpload ID="fuGroupImage" runat="server"/></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Owner") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddOwner" runat="server"></asp:DropDownList></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Access Level") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddAccessLevel" runat="server"></asp:DropDownList></td>
    </tr>
    <tr >
        <td class="table_cell">
            <%= Lang.TransA("Approved") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddApproved" runat="server">
            </asp:DropDownList></td>
    </tr>
    <tr >
        <td class="table_cell">
   			        <%= Lang.TransA("Minimum age restriction")%></td>
        <td class="table_cell">   	
   			        <asp:DropDownList ID="ddAgeRestriction" runat="server"></asp:DropDownList>
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