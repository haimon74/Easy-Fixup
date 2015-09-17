<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="PrivacySettings.ascx.cs"
            Inherits="AspNetDating.Components.Profile.PrivacySettings" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>

<div class="extraPadContent">
    <uc1:HeaderLine ID="hlVisibilitySettings" runat="server" Visible="true"></uc1:HeaderLine>
    <div class="wrap-view">
        <table cellpadding="2" cellspacing="0" width="100%" class="fl">
            <tr>
                <td width="30%" style="white-space: nowrap;width:30%;">
                    <%= "My profile is visible for".Translate() %>:
                </td>
                <td width="58%">
                    <asp:DropDownList CssClass="l_form_ddl" ID="ddViewProfile" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td width="30%" style="white-space: nowrap;width:30%;">
                    <%= "My photos are visible for".Translate()%>:
                </td>
                <td width="58%">
                    <asp:DropDownList CssClass="l_form_ddl" ID="ddViewPhotos" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <%--<tr>
                <td>
                    <%= "My videos are visible for".Translate()%>:
                </td>
                <td>
                    <asp:DropDownList CssClass="l_form_ddl" ID="ddViewVideos" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <tr>
                <td width="30%" style="white-space: nowrap;width:30%;">
                    <%= "My friends are visible for".Translate()%>:
                </td>
                <td width="58%">
                    <asp:DropDownList CssClass="l_form_ddl" ID="ddViewFriends" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>
            <%--<tr>
                <td>
                    <%= "My blog is visible for".Translate()%>:
                </td>
                <td>
                    <asp:DropDownList CssClass="l_form_ddl" ID="ddViewBlog" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>--%>
        </table>
        <div id="pnlDisableLevelIcon" runat="server">
            <asp:CheckBox ID="cbDisableLevelIcon" runat="server" />
        </div>
        <asp:CheckBox ID="cbDisableProfileViews" runat="server" />
        <br />
        <asp:CheckBox ID="cbHideFriends" runat="server" />
        <br />
        <div id="pnlHideGroupMembership" runat="server">
            <asp:CheckBox ID="cbHideGroupMembership" runat="server" />
        </div>
        <div class="separator">
        </div>
        <uc1:HeaderLine ID="hlEventsSettings" runat="server"></uc1:HeaderLine>
        <asp:CheckBoxList ID="cblEventsSettings" CellPadding="0" CellSpacing="0" CssClass="CheckBoxList"
            runat="server">
        </asp:CheckBoxList>
    </div>
    <div class="buttons">
        <asp:Button runat="server" ID="btnSaveChanges" CssClass="medium zurbtn blue" 
            OnClick="btnSaveChanges_Click"  />
    </div>
    <div class="srvmsg">
        <asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="false"></asp:Label>
    </div>    
</div>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"></uc1:LargeBoxEnd>
