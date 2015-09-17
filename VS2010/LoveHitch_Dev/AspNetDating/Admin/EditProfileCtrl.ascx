<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditProfileCtrl.ascx.cs"
    Inherits="AspNetDating.Admin.EditProfileCtrl" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="../Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<ajaxtoolkit:cascadingdropdown id="CascadingDropDownCountry" runat="server" targetcontrolid="dropCountry"
    category="Country" servicepath="~/Services/Service.asmx" prompttext=" " servicemethod="GetCountries" />
<ajaxtoolkit:cascadingdropdown id="CascadingDropDownState" runat="server" targetcontrolid="dropRegion"
    category="Region" servicepath="~/Services/Service.asmx" prompttext=" " servicemethod="GetRegionsByCountry"
    parentcontrolid="dropCountry" />
<ajaxToolkit:CascadingDropDown ID="CascadingDropDownCity" runat="server" TargetControlID="dropCity"
    Category="City" ServicePath="~/Services/Service.asmx" PromptText=" "
    ServiceMethod="GetCitiesByCountryAndRegion" ParentControlID="dropRegion" />
<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
<table border="0" cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <td class="table_header" colspan="2">
            <%= Lang.TransA("Personal Settings") %>
        </td>
    </tr>
    <tr id="trCountry" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("Country") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="dropCountry" runat="server">
            </asp:DropDownList></td>
    </tr>
    <tr  id="trState" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("State") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="dropRegion" runat="server">
            </asp:DropDownList></td>
    </tr>
    <tr id="trCity" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("City") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="dropCity" runat="server">
            </asp:DropDownList></td>
    </tr>
    <tr  id="trZipCode" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("Zip/Postal Code") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtZipCode" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Name") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
    </tr>
    <tr id="pnlGender" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("Gender") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="dropGender" runat="server">
                <asp:ListItem Value=""></asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr id="trInterestedIn" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("Interested in") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="dropInterestedIn" runat="server">
                <asp:ListItem Value=""></asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr id="pnlBirthdate" runat="server">
        <td class="table_cell">
            <%= Lang.TransA("Birthdate") %></td>
        <td class="table_cell">
            <uc2:DatePicker id="datePicker1" runat="server">
            </uc2:DatePicker></td>
    </tr>
    <tr id="trBirthdate2" runat="server">
        <td class="table_cell">
            <asp:Label ID="lblBirthdate2" runat="server"></asp:Label></td>
        <td class="table_cell">
            <uc2:DatePicker id="datePicker2" runat="server">
            </uc2:DatePicker></td>
    </tr>
    <tr >
        <td class="table_cell">
            <%= Lang.TransA("E-Mail") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("New password") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr >
        <td class="table_cell">
            <%= Lang.TransA("Confirm new password") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtPassword2" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2" class="table_cell">
            <asp:CheckBox ID="cbReceiveEmails" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr>
        <td colspan="2" class="table_cell">
            <asp:CheckBox ID="cbProfileVisible" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr id="trUserVerified" runat="server">
        <td colspan="2" class="table_cell">
            <asp:CheckBox ID="cbUserVerified" runat="server"></asp:CheckBox>
        </td>
    </tr>
    <tr id="trFeaturedMember" runat="server">
        <td colspan="2" class="table_cell">
            <asp:CheckBox ID="cbFeaturedMember" runat="server"></asp:CheckBox>
        </td>
    </tr>
</table>
<div class="separator10"></div>
<input type="hidden" id="hidUsername" runat="server" name="hidUsername" />
<asp:PlaceHolder ID="plhProfile" runat="server"></asp:PlaceHolder>
<div class="add-buttons2">
	<div class="add-buttons">	
		<asp:Button ID="btnSave" runat="server" onclick="btnSave_Click"></asp:Button>
   		<asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click"></asp:Button>
	</div>
</div>
<div class="separator10"></div>
