<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditSalutePhoto.aspx.cs" Inherits="ezFixUp.EditSalutePhoto" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<div id="sidebar">
    <uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
    <div class="SideMenuItem"><a href="ManageProfile.aspx" class="SideMenuLink"><%= "Edit Profile".Translate() %></a>
	</div>
    <uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
</div>
<div id="main">
    <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
    <div class="note">
        <components:contentview ID="cvNote" Key="UploadSalutePhotoNotes" runat="server">
        </components:contentview>
    </div>
    <div class="srvmsg">
        <asp:Label ID="lblError" CssClass="error" EnableViewState="False" runat="server" />
    </div>
        <table cellpadding="2" cellspacing="0" class="BoxWrapStyle" width="100%">
        <tr>
            <td width="160" align="center" valign="top">
                <img id="tempPhoto" runat="server" src="Image.ashx?id=session&width=150&height=150&seed=<%= new Random().NextDouble().ToString() %>"
                    border="0" />
                <br />
            	<asp:CheckBox ID="chkPrivatePhoto" runat="server"></asp:CheckBox>
            </td>
            <td valign="top">
				<span class="label"><%= Lang.Trans("Name") %>:</span>
                <asp:TextBox ID="txtName" CssClass="textbox" runat="server" MaxLength="80"></asp:TextBox><br />
                <span class="label"><%= Lang.Trans("Description")%>:</span>
                <asp:TextBox ID="txtDescription" CssClass="textbox" runat="server" Width="96%" MaxLength="200" TextMode="MultiLine"></asp:TextBox><br />
                <asp:Label CssClass="label" ID="lblPhoto" runat="server" />
                <asp:FileUpload ID="ufPhoto" runat="server" />
                <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click"></asp:Button>
                <div id="divImageRotateControls" visible="false" runat="server">
                    <asp:Button ID="btnRotateLeft" runat="server" OnClick="btnRotateLeft_Click" />
                    &nbsp;
                    <asp:Button ID="btnRotateRight" runat="server" OnClick="btnRotateRight_Click" />
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
            	<div class="separator6"></div>
                <div class="line"></div>
            	<div class="separator6"></div>  
                <asp:LinkButton ID="lnkSave" runat="server" OnClick="lnkSave_Click"></asp:LinkButton>&nbsp;|&nbsp;
                <asp:LinkButton ID="lnkCancel" runat="server" OnClick="lnkCancel_Click"></asp:LinkButton>
                <div class="separator"></div>
            </td>
        </tr>
    </table>    
    <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
</div>
</asp:Content>
