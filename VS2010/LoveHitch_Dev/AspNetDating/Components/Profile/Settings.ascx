<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" 
            AutoEventWireup="True" 
            CodeBehind="Settings.ascx.cs" 
            Inherits="AspNetDating.Components.Profile.SettingsCtrl"
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register Src="../LocationPicker.ascx" TagName="LocationPicker" TagPrefix="uc2" %>
<%@ Register Src="../DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register Src="../BirthdayPicker.ascx" TagName="BirthdayPicker" TagPrefix="uc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Extenders" TagPrefix="cc2" %>
<%@ Register Assembly="jQueryDotNetLib" Namespace="jQueryDotNetLib.Controls" TagPrefix="cc1" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"></uc1:LargeBoxStart>
<input id="hidUsername" type="hidden" name="hidUsername" runat="server" />
<div class="extraPadContent">
    <uc1:HeaderLine ID="hlPersonalSettings" runat="server"></uc1:HeaderLine>
    <div class="wrap-view">
        <table class="settings fl blockTable" width="105%" cellpadding="0" cellspacing="0" style="white-space:nowrap;">
        <tbody>
            <tr>
                <td colspan="2"  style="display:block;">
                    <uc2:LocationPicker ID="LocationPicker1" runat="server" style="display: inline;" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="display:block;">
                    <table class="w100">
                        <tr>
                            <td style="width: 40%;">
                                <%= Lang.Trans("Name") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" CssClass="l_form_txt" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trInterestedIn" runat="server">
                            <td style="width: 40%;">
                                <%= Lang.Trans("Interested in") %>
                            </td>
                            <td>
                                <asp:DropDownList ID="dropInterestedIn" CssClass="l_form_ddl" runat="server">
                                    <asp:ListItem Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="pnlBirthdate" runat="server">
                            <td style="width: 40%;">
                                <%= Lang.Trans("Birth date") %>
                            </td>
                            <td><%-- style="float:none;display:inline;">--%>
                                <uc3:BirthdayPicker ID="BirthdayPicker1" runat="server" CssClass="l_form_txt" />
                                <uc2:DatePicker ID="datePicker1" CssClass="datepicker" runat="server" Visible="false"/>
                            </td>
                        </tr>
                        <tr id="trBirthdate2" runat="server" style="display:none;">
                            <td style="width: 40%;">
                                <asp:Label ID="lblBirthDate2" runat="server"></asp:Label>
                            </td>
                            <td>
                                <%--<uc2:DatePicker ID="datePicker2" CssClass="datepicker" runat="server"></uc2:DatePicker>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%;">
                                <%= Lang.Trans("E-Mail") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" CssClass="l_form_txt" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%;">
                                <%= Lang.Trans("Current password") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrentPassword" CssClass="l_form_txt"
                                    runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%;">
                                <%= Lang.Trans("New password") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNewPassword" CssClass="l_form_txt" runat="server"
                                    TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 40%;">
                                <%= Lang.Trans("Confirm new password") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtConfirmNewPassword" CssClass="l_form_txt"
                                    runat="server" TextMode="Password"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>                
            </tr>
        </tbody>
        </table>
    </div>
    <uc1:HeaderLine ID="hlAdditionalSettings" runat="server"></uc1:HeaderLine>
    <div class="wrap-view">
        <table  class="w90 fl">
            <tr><td colspan="2"><asp:CheckBox ID="cbReceiveEmails" runat="server"></asp:CheckBox></td></tr>
            <tr><td colspan="2"><asp:CheckBox ID="cbProfileVisible" runat="server"></asp:CheckBox></td></tr>
            <tr>
                <td class="w40">
                    <%= Lang.Trans("I'd like to receive messages from") %>:
                </td>
                <td class="w60">
                    <asp:DropDownList ID="ddMessagesFrom" runat="server" CssClass="l_form_ddl fl"
                        AutoPostBack="false">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="separator" />
                </td>
            </tr>
        </table>
        <div id="pnlAge" runat="server">
            <table class="w90 fl">
                <tr>
                    <td  class="w40">
                        <%= Lang.Trans("Members should be from") %>
                    </td>
                    <td  class="w60">
                        <asp:DropDownList ID="ddlAgeFrom" runat="server" OnSelectedIndexChanged="comboAgeFrom_SelectedIndexChanged"
                            CssClass="s_form_ddl" AutoPostBack="false">
                        </asp:DropDownList>
                        <%= Lang.Trans("to") %>
                        <asp:DropDownList ID="ddlAgeTo" runat="server" OnSelectedIndexChanged="comboAgeTo_SelectedIndexChanged"
                            CssClass="s_form_ddl" AutoPostBack="false">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtAgeFrom" runat="server" Visible="false" CssClass="l_form_txt"
                            Size="2" MaxLength="2"></asp:TextBox>
                        <asp:TextBox ID="txtAgeTo" runat="server" Visible="false" CssClass="l_form_txt"
                            Size="2" MaxLength="2"></asp:TextBox>
                        <%= Lang.Trans("years old") %>
                    </td>
                </tr>
            </table>
        </div>
        <div class="separator">
        </div>
        <div id="pnlProfileSkin" runat="server" visible="false">
            <asp:UpdatePanel ID="updatePanelProfileSkin" runat="server" UpdateMode="Conditional"
                RenderMode="Inline">
                <ContentTemplate>
                    <%= "Profile skin:".Translate() %>&nbsp;<asp:DropDownList CssClass="l_form_ddl"
                        ID="dropProfileSkin" AutoPostBack="true" OnSelectedIndexChanged="dropProfileSkin_OnSelectedIndexChanged"
                        runat="server">
                    </asp:DropDownList>
                    &nbsp; <a id="lnkPreviewSkin" target="_blank" visible="false" runat="server"></a>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="separator">
        </div>
        <div id="pnlRelationshipStatus" runat="server" visible="false">
            <asp:UpdatePanel ID="UpdatePanelRelationshipStatus" runat="server">
                <ContentTemplate>
                    <%= "Relationship status".Translate() %>:&nbsp;
                    <asp:DropDownList ID="ddRelationshipStatus" CssClass="l_form_ddl" runat="server"
                        AutoPostBack="true" OnSelectedIndexChanged="ddRelationshipStatus_SelectedIndexChanged">
                        <asp:ListItem Value=""></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddInRelationshipWith" CssClass="l_form_ddl" runat="server"
                        AutoPostBack="false" Visible="false">
                    </asp:DropDownList>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="separator">
        </div>
        <asp:CheckBox ID="cbPhotoRequired" runat="server" />
        <div id="pnlDisableProfileRating" runat="server">
            <asp:CheckBox ID="cbDisableProfileRating" runat="server" />
        </div>
        <div id="pnlDisableVoting" runat="server">
            <asp:CheckBox ID="cbDisableVoting" runat="server" />
        </div>
        <div id="pnlDisableProfileComments" runat="server">
            <asp:CheckBox ID="cbDisableProfileComments" runat="server" />
        </div>
        <div id="pnlDisablePhotoComments" runat="server">
            <asp:CheckBox ID="cbDisablePhotoComments" runat="server" />
        </div>
        <div id="pnlDisablePhotoRating" runat="server">
            <asp:CheckBox ID="cbDisablePhotoRating" runat="server" />
        </div>
        <div id="pnlDisableBlogComments" runat="server">
            <asp:CheckBox ID="cbDisableBlogComments" runat="server" />
        </div>
    </div>
    <div class="buttons">
        <asp:Button runat="server" ID="btnSaveChanges" CssClass="medium zurbtn blue"></asp:Button>
    </div>
    <div class="srvmsg">
        <asp:Label ID="lblError" CssClass="error" runat="server"></asp:Label>
    </div>    
</div>
<uc1:LargeBoxEnd ID="LargeBoxEnd" runat="server"></uc1:LargeBoxEnd>

    <div id="divTwitter" runat="server">
        <uc1:LargeBoxStart ID="LargeBoxStart3" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
        <div class="text center">
            <asp:Label ID="lblTwitterStatus" runat="server" />
        </div>
        <div class="separator6">
        </div>
        <div class="wrap-view" id="divEnterTwitterCredentials" runat="server">
            <table class="settings" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 40%;">
                        <%= Lang.Trans("Twitter username") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTwitterUsername" CssClass="l_form_txt"
                            runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 40%;">
                        <%= Lang.Trans("Twitter password") %>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTwitterPassword" CssClass="l_form_txt" TextMode="Password"
                            runat="server" />
                    </td>
                </tr>
            </table>
            <div class="buttons">
                <asp:Button runat="server" ID="btnSaveTwitterSettings"></asp:Button>
            </div>
        </div>
        <div class="wrap-view" id="divTwitterStatus" runat="server" visible="false">
            <div class="buttons">
                <asp:Button runat="server" ID="btnRemoveTwitterCredentials"></asp:Button>
            </div>
        </div>
        <uc1:LargeBoxEnd runat="server" ID="Largeboxend3"></uc1:LargeBoxEnd>
    </div>
    <div id="divCardSpace" runat="server">
        <uc1:LargeBoxStart ID="LargeBoxStart2" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
        <div class="text center">
            <%= Lang.Trans("Use the button below to present a new information card")%>
        </div>
        <div class="separator6">
        </div>
        <div class="cardspace" align="center">
            <button onclick="window.open('<%= Config.Urls.SecureHome + "/PresentInfoCard.aspx" %>', 'cardspace', 'width=300,height=210,resizable=0,menubar=0,status=0,toolbar=0'); return false;">
                <img align="top" src="images/infocard_23x16.png" />
                <%= Lang.Trans("Use information card") %>
            </button>
        </div>
        <uc1:LargeBoxEnd runat="server" ID="Largeboxend2"></uc1:LargeBoxEnd>
    </div>
<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div style="padding: 5px 20px 5px 20px;">
<div class="text center">
    <%= Lang.Trans("We hope you've found the love of your life and this is the reason you've decided to delete your account.")%>
</div>
<div class="buttons">
    <asp:UpdatePanel ID="UpdatePanelConfirmPassword" runat="server">
        <ContentTemplate>
            <div id="pnlConfirmPasswordForDeletion" runat="server" visible="false">
                <div class="srvmsg">
                    <asp:Label ID="lblErrorConfirmPasswordForDeletion" CssClass="error" runat="server"
                        EnableViewState="false"></asp:Label></div>
                <div class="separator">
                </div>
                <%= "Enter your password".Translate() %>:
                <asp:TextBox ID="txtConfirmPasswordForDeletion" CssClass="smalltextbox" runat="server"
                    TextMode="Password"></asp:TextBox>
            </div>
            <asp:Button ID="btnDelete" CssClass="medium zurbtn red" runat="server"></asp:Button>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
</div>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"></uc1:LargeBoxEnd>

