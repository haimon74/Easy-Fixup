<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="ManageProfile.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.MemberProfile" %>

<%@ Register Src="Components/Profile/Gadgets.ascx" TagName="Gadgets" TagPrefix="uc9" %>
<%@ Register Src="Components/Profile/Billing.ascx" TagName="Billing" TagPrefix="uc6" %>
<%@ Register Src="Components/Profile/RecordVideo.ascx" TagName="RecordVideo" TagPrefix="uc7" %>
<%@ Register Src="Components/Profile/UploadVideo.ascx" TagName="UploadVideo" TagPrefix="uc10" %>
<%@ Register Src="Components/Profile/UploadAudio.ascx" TagName="UploadAudio" TagPrefix="uc12" %>
<%@ Register Src="Components/Profile/Settings.ascx" TagName="Settings" TagPrefix="uc8" %>
<%@ Register Src="Components/Profile/ViewPhotos.ascx" TagName="ViewPhotos" TagPrefix="uc5" %>
<%@ Register Src="Components/Profile/ViewProfile.ascx" TagName="ViewProfile" TagPrefix="uc4" %>
<%@ Register Src="Components/Profile/EditPhotos.ascx" TagName="EditPhotos" TagPrefix="uc3" %>
<%@ Register Src="Components/Profile/EditProfile.ascx" TagName="EditProfile" TagPrefix="uc2" %>
<%@ Register Src="Components/Profile/EditSkin.ascx" TagName="EditSkin" TagPrefix="uc13" %>
<%@ Register Src="~/Components/Profile/PrivacySettings.ascx" TagName="PrivacySettings"
    TagPrefix="uc14" %>
<%@ Register Src="~/Components/Profile/ViewEvents.ascx" TagName="ViewEvents" TagPrefix="uc15" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="left">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkEditProfile" CssClass="SideMenuLink" runat="server" OnClick="lnkEditProfile_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkViewProfile" CssClass="SideMenuLink" runat="server" OnClick="lnkViewProfile_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkUploadPhotos" CssClass="SideMenuLink" runat="server" OnClick="lnkUploadPhotos_Click"></asp:LinkButton>
        </div>
        <div id="pnlSalutePhoto" runat="server" class="SideMenuItem" visible="false">
            <asp:LinkButton ID="lnkUploadSalutePhoto" CssClass="SideMenuLink" runat="server"
                OnClick="lnkUploadSalutePhoto_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem" id="pnlAudioUpload" runat="server">
            <asp:LinkButton ID="lnkUploadAudio" CssClass="SideMenuLink" runat="server" OnClick="lnkUploadAudio_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem" id="pnlUploadVideo" runat="server">
            <asp:LinkButton ID="lnkUploadVideo" CssClass="SideMenuLink" runat="server" OnClick="lnkUploadVideo_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkViewPhotos" CssClass="SideMenuLink" runat="server" OnClick="lnkViewPhotos_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkViewEvents" CssClass="SideMenuLink" runat="server" OnClick="lnkViewEvents_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem" id="pnlEditSkin" runat="server">
            <asp:LinkButton ID="lnkEditSkin" CssClass="SideMenuLink" runat="server" OnClick="lnkEditSkin_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkPrivacySettings" CssClass="SideMenuLink" runat="server" OnClick="lnkPrivacySettings_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem">
            <asp:LinkButton ID="lnkSettings" CssClass="SideMenuLink" runat="server" OnClick="lnkSettings_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem" id="trSubscription" runat="server">
            <asp:LinkButton ID="lnkSubscription" CssClass="SideMenuLink" runat="server" OnClick="lnkSubscription_Click"></asp:LinkButton>
        </div>
        <div class="SideMenuItem" id="trGadgets" runat="server">
            <asp:LinkButton ID="lnkGadgets" CssClass="SideMenuLink" runat="server" OnClick="lnkGadgets_Click"></asp:LinkButton>
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
    </div>
    <div id="right">
        <uc2:EditProfile ID="EditProfile1" runat="server" />
        <uc3:EditPhotos ID="EditPhotos1" runat="server" />
        <uc4:ViewProfile ID="ViewProfile1" runat="server" />
        <uc5:ViewPhotos ID="ViewPhotos1" runat="server" />
        <uc8:Settings ID="Settings1" runat="server" />
        <uc12:UploadAudio ID="UploadAudio1" runat="server" />
        <uc10:UploadVideo ID="UploadVideo1" runat="server" />
        <uc9:Gadgets ID="Gadgets1" runat="server" />
        <uc6:Billing ID="Billing1" runat="server" />
        <uc13:EditSkin ID="EditSkin1" runat="server" />
        <uc14:PrivacySettings ID="PrivacySettings1" runat="server" />
        <uc15:ViewEvents ID="ViewEvents1" runat="server" />
    </div>
</asp:Content>
