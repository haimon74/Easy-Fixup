<%@ Import Namespace="ezFixUp.Classes"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditAdCtrl.ascx.cs" Inherits="ezFixUp.Admin.EditAdCtrl" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<uc1:MessageBox id="MessageBox" runat="server"></uc1:MessageBox>
<asp:Panel ID="pnlEditAd" runat="server">
<table border="0" cellpadding="0" cellspacing="0" id="stable">
    <tr>
        <td align="center" class="table_header" colspan="2">
            <%= Lang.TransA("Classified Information") %></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Category") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddCategories" runat="server" AutoPostBack="true" 
                onselectedindexchanged="ddCategories_SelectedIndexChanged"></asp:DropDownList>
            <asp:DropDownList ID="ddSubcategories" runat="server" Visible="false"></asp:DropDownList></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Expiration date") %></td>
        <td class="table_cell">
            <asp:TextBox ID="txtExpiratoinDate" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Location")%></td>
        <td class="table_cell">
            <asp:TextBox ID="txtLocation" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Subject")%></td>
        <td class="table_cell">
            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Message")%></td>
        <td class="table_cell">
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="textbox" Rows="5" Columns="50"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="table_cell">
            <%= Lang.TransA("Approved") %></td>
        <td class="table_cell">
            <asp:DropDownList ID="ddApproved" runat="server"></asp:DropDownList></td>
    </tr>
</table>
</asp:Panel>
<asp:Panel ID="pnlUploadAdPhotos" runat="server" Visible="false">
                <%= Lang.TransA("Classified Photos") %>
            <asp:Repeater ID="rptAdPhotos" runat="server" onitemcommand="rptAdPhotos_ItemCommand" onitemcreated="rptAdPhotos_ItemCreated">
                    <ItemTemplate>
                        <%# ezFixUp.AdPhoto.RenderImageTag((int)Eval("AdPhotoID"), 90, 90, null, true) %>
                        <asp:LinkButton ID="lnkDelete" runat="server" CommandArgument='<%# Eval("AdPhotoID") %>' CommandName="DeletePhoto"></asp:LinkButton>
                        <br />
                    </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rptAddPhoto" runat="server">
                <ItemTemplate>
                    <asp:FileUpload ID="fuAdPhoto" runat="server" />
                    <br />
                    <asp:TextBox ID="txtAdPhotoDescription" runat="server"></asp:TextBox>
                    <br />
                </ItemTemplate>
            </asp:Repeater>
</asp:Panel>
<div class="separator10"></div>
<div class="add-buttons2">
	<div class="add-buttons">
        <asp:Button ID="btnSave" runat="server" onclick="btnSave_Click"></asp:Button>
        <asp:Button ID="btnCancel" runat="server" onclick="btnCancel_Click"></asp:Button>
	</div>
</div>
<div class="separator10"></div>