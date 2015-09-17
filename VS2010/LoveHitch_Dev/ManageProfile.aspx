<%@ Page Language="c#" MasterPageFile="~/Site.Master" CodeBehind="ManageProfile.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.ManageMemberProfile" %>

<%@ Register Src="Components/Profile/Gadgets.ascx" TagName="Gadgets" TagPrefix="uc9" %>
<%@ Register Src="Components/Profile/Billing.ascx" TagName="Billing" TagPrefix="uc6" %>
<%@ Register Src="Components/Profile/RecordVideo.ascx" TagName="RecordVideo" TagPrefix="uc7" %>
<%@ Register Src="Components/Profile/UploadVideo.ascx" TagName="UploadVideo" TagPrefix="uc10" %>
<%@ Register Src="Components/Profile/UploadAudio.ascx" TagName="UploadAudio" TagPrefix="uc12" %>
<%@ Register Src="Components/Profile/Settings.ascx" TagName="Settings" TagPrefix="uc8" %>
<%@ Register Src="Components/Profile/ViewPhotos.ascx" TagName="ViewPhotos" TagPrefix="uc5" %>
<%@ Register Src="Components/Profile/ViewProfile.ascx" TagName="ViewProfile" TagPrefix="uc4" %>
<%@ Register Src="Components/Profile/EditPhotos.ascx" TagName="EditPhotos" TagPrefix="uc3" %>
<%@ Register Src="Components/Profile/EditTopic.ascx" TagName="EditTopic" TagPrefix="uc2" %>
<%@ Register Src="Components/Profile/EditSkin.ascx" TagName="EditSkin" TagPrefix="uc13" %>
<%@ Register Src="~/Components/Profile/PrivacySettings.ascx" TagName="PrivacySettings"
    TagPrefix="uc14" %>
<%@ Register Src="~/Components/Profile/ViewEvents.ascx" TagName="ViewEvents" TagPrefix="uc15" %>
<%@ Register Src="~/Components/Profile/FmtDropDownListItem.ascx" TagName="FmtDdListItem" TagPrefix="uc16" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

      
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<script type="text/javascript">
    //asyncScripts.push('scripts/jquery.popeye.js');
    asyncScripts.push('scripts/pages/ManageProfile.js');
</script>    
    <asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
        <Scripts>
            <asp:ScriptReference  Path="~/scripts/pages/ManageProfile.js" />            
        </Scripts>
    </asp:ScriptManagerProxy>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lnkEditTopics" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkViewProfile" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkUploadPhotos" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkUploadSalutePhoto" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkUploadAudio" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkUploadVideo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkViewPhotos" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkViewEvents" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkEditSkin" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkPrivacySettings" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkSettings" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkGadgets" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="lnkSubscription" EventName="Click" />            
        </Triggers>
        <ContentTemplate>
        
        <div id="sidebar">
                <uc1:SideMenuStart ID="SmallBoxStart1" runat="server"></uc1:SideMenuStart>
                <div id="fmt">
                    <ul class="fmt">
                        <li>
                            <asp:LinkButton ID="lnkEditTopics" runat="server" OnClientClick="return false;"></asp:LinkButton>
                            <div>
                                <ul class="fmtDrpDwn" style="display:none;">
                                    <asp:PlaceHolder ID="plhTopics" runat="server"></asp:PlaceHolder>
                                </ul>
                            </div>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkViewProfile" runat="server" OnClick="lnkViewProfile_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkUploadPhotos" runat="server" OnClick="lnkUploadPhotos_Click"></asp:LinkButton></li>
                        <li>
                            <asp:LinkButton ID="lnkViewPhotos" runat="server" OnClick="lnkViewPhotos_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkViewEvents" runat="server" OnClick="lnkViewEvents_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkPrivacySettings" runat="server" OnClick="lnkPrivacySettings_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkSettings" runat="server" OnClick="lnkSettings_Click"></asp:LinkButton>
                        </li>
                        <li>
                            <asp:LinkButton ID="lnkSubscription" runat="server" OnClick="lnkSubscription_Click"></asp:LinkButton>
                        </li>
                        <li id="liEditSkin" runat="server">
                            <asp:LinkButton ID="lnkEditSkin" runat="server" OnClick="lnkEditSkin_Click"></asp:LinkButton></li>
                        <li id="liSalutePhoto" runat="server">
                            <asp:LinkButton ID="lnkUploadSalutePhoto" runat="server" OnClick="lnkUploadSalutePhoto_Click"></asp:LinkButton></li>
                        <li id="liAudioUpload" runat="server">
                            <asp:LinkButton ID="lnkUploadAudio" runat="server" OnClick="lnkUploadAudio_Click"></asp:LinkButton></li>
                        <li id="liUploadVideo" runat="server">
                            <asp:LinkButton ID="lnkUploadVideo" runat="server" OnClick="lnkUploadVideo_Click"></asp:LinkButton></li>
                        <li id="liGadgets" runat="server">
                            <asp:LinkButton ID="lnkGadgets" runat="server" OnClick="lnkGadgets_Click"></asp:LinkButton>
                        </li>                                                
                    </ul>
                </div>
                <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
                <div class="clear">
                </div>
                <div style="height:550px;position:static;">
                    <components:BannerView ID="bvProfileLeftBottom" runat="server" Key="ProfileLeftBottom">
                    </components:BannerView>
                </div>
            </div>
            <div id="main">
                <uc2:EditTopic ID="EditTopic1" runat="server"  Visible="false"/>
                <uc3:EditPhotos ID="EditPhotos1" runat="server" Visible="false" />
                <uc4:ViewProfile ID="ViewProfile1" runat="server"  />
                <uc5:ViewPhotos ID="ViewPhotos1" runat="server"  Visible="false"/>
                <uc8:Settings ID="Settings1" runat="server"  Visible="false"/>
                <uc12:UploadAudio ID="UploadAudio1" runat="server"  Visible="false"/>
                <uc10:UploadVideo ID="UploadVideo1" runat="server"  Visible="false"/>
                <uc9:Gadgets ID="Gadgets1" runat="server"  Visible="false"/>
                <uc6:Billing ID="Billing1" runat="server"  Visible="false"/>
                <uc13:EditSkin ID="EditSkin1" runat="server"  Visible="false"/>
                <uc15:ViewEvents ID="ViewEvents1" runat="server"  Visible="false"/>
                <uc14:PrivacySettings ID="PrivacySettings1" runat="server"  Visible="false"/>
                <div style="height:100px;position:static;">
                    <components:BannerView ID="bvProfileRightBottom" runat="server" Key="ProfileRightBottom">
                    </components:BannerView>
                </div>
            </div>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
