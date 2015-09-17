<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="MyHome.aspx.cs" AutoEventWireup="True"
    Inherits="AspNetDating.Home" %>

<%@ Register Src="Components/Groups/NewGroups.ascx" TagName="NewGroups" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="SearchBox" Src="Components/Search/SearchBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="NewsBox" Src="Components/NewsBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="BirthdayBox" Src="Components/BirthdayBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FriendsOnlineBox" Src="Components/FriendsOnlineBox.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Import Namespace="ezFixUp.Data" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="AspNetDating" %>
<%@ Register Src="Components/WebParts/NewUsersWebPart.ascx" TagName="NewUsersWebPart"
    TagPrefix="uc3" %>
<%@ Register Src="Components/WebParts/NewVideosWebPart.ascx" TagName="NewVideosWebPart"
    TagPrefix="uc3" %>
<%@ Register Src="Components/WebParts/PopularBlogPostsWebPart.ascx" TagName="PopularBlogPostsWebPart"
    TagPrefix="uc3" %>
<%@ Register Src="Components/WebParts/NewGroupsWebPart.ascx" TagName="NewGroupsWebPart"
    TagPrefix="uc3" %>
<%@ Register TagPrefix="CustomWebPartManager" Namespace="AspNetDating.CustomWebPartManager"
    Assembly="AspNetDating" %>
<%@ Register TagPrefix="CustomWebPartZone" Namespace="AspNetDating.CustomWebPartZone"
    Assembly="AspNetDating" %>
<%@ Register TagPrefix="CustomEditorZone" Namespace="AspNetDating.CustomEditorZone"
    Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <CustomWebPartManager:CustomWebPartManager ID="WebPartManager1" Personalization-InitialScope="User"
        Personalization-Enabled="true" runat="server">
    </CustomWebPartManager:CustomWebPartManager>
    <div id="divFaceBookStatusUpdate" visible="false" runat="server">

        <script type="text/javascript">
            FB.init("<%= Facebook.ApiKey %>", "Facebook/xd_receiver.htm", { permsToRequestOnConnect: "publish_stream" });
        </script>

        <input type="hidden" id="hidOldStatus" />
        <input type="hidden" id="hidPostbackControlID" />

        <script type="text/javascript">
            String.Trim = function(str) {
                return str.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
            }

            function InitializeRequestHandler(sender, args) {
                var elem = args.get_postBackElement();
                $get("hidPostbackControlID").value = elem.id;
            }

            function EndRequestHandler(sender, args) {
                if ($get("hidPostbackControlID").value == '<%= lnkEditStatusText.ClientID %>') {
                    $get("hidOldStatus").value = $get('<%= txtStatusText.ClientID %>').value;
                }
                if ($get("hidPostbackControlID").value == '<%= lnkUpdateStatusText.ClientID %>') {
                    var oldStatus = String.Trim($get('hidOldStatus').value);
                    var status = String.Trim($get('<%= lblStatusText.ClientID %>').innerText);
                    if (status != oldStatus) {
                        if (status != '') {
                            FB.ensureInit(function() {
                                FB.Connect.streamPublish(status, null, null, null, null, null, true);
                            });
                        }
                    }
                }
            }

            Sys.WebForms.PageRequestManager.getInstance().add_initializeRequest(InitializeRequestHandler);
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        </script>

    </div>
    <div id="left">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <CustomWebPartZone:CustomWebPartZone TabIndex="0" BorderStyle="None" BorderWidth="0"
                    CssClass="SideMenuWebpartsWrap" ID="wpzHomePageLeftZone" WebPartVerbRenderMode="TitleBar"
                    HeaderText="" Padding="0" PartChromePadding="0" Width="100%" runat="server">
                    <PartStyle CellPadding="0" CssClass="SideMenuWebpartContent" BorderWidth="0"></PartStyle>
                    <PartTitleStyle CssClass="SideMenuWebpartTop" BorderWidth="0"></PartTitleStyle>
                    <ZoneTemplate>
                    </ZoneTemplate>
                    <CloseVerb Text="Close" Visible="false" />
                    <MinimizeVerb Visible="false" ImageUrl="~/Images/minimize.gif" />
                    <RestoreVerb Visible="false" ImageUrl="~/Images/restore.gif" />
                    <DeleteVerb Visible="false" ImageUrl="~/Images/delete.gif" />
                    <EditVerb Visible="false" ImageUrl="~/Images/edit.gif" />
                </CustomWebPartZone:CustomWebPartZone>
                <div id="pnlEditorZoneLeft" runat="server">
                    <CustomEditorZone:CustomEditorZone CssClass="SideMenuEditorZone" Padding="0" Width="100%"
                        ID="ezEditorZoneLeft" PartChromePadding="0" runat="server">
                        <HeaderStyle CssClass="SideMenuWebpartTop" />
                        <FooterStyle CssClass="SideMenuWebpartBtm"></FooterStyle>
                        <HeaderCloseVerb ImageUrl="~/Images/delete.gif" />
                        <ZoneTemplate>
                        </ZoneTemplate>
                    </CustomEditorZone:CustomEditorZone>
                </div>
                <div class="mode">
                    <asp:LinkButton ID="lnkCatalogForLeftParts" runat="server" OnClick="lnkCatalogForLeftParts_Click"></asp:LinkButton>
                </div>
                <components:BannerView ID="bvHomeLeftBottom" runat="server" Key="HomeLeftBottom">
                </components:BannerView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="right">
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
        <components:BannerView ID="bvHomeBeforeProfile" runat="server" Key="HomeBeforeProfile">
        </components:BannerView>
        <table cellpadding="0" cellspacing="0" width="100%" border="0">
            <tr>
                <td valign="top">
                    <div class="wrap-info">
                        <li id="liWhoViewedMyProfile" runat="server">
                            <%= Lang.Trans("Your profile has been viewed") %><b>
                                <asp:Label ID="lblProfileViews" runat="server" /></b>&nbsp;<%= Lang.Trans("times") %>&nbsp;[
                            <asp:LinkButton ID="lnkViewProfileViewers" runat="server" OnClick="lnkViewProfileViewers_Click" />
                            ]
                            <br />
                        </li>
                        <li id="pnlRating" runat="server">
                            <%= Lang.Trans("Average rating") %>: <b>
                                <asp:Label ID="lblRating" runat="server" /></b><br />
                        </li>
                        <li id="pnlVotes" runat="server">
                            <%= Lang.Trans("Your votes") %>: <b>
                                <asp:Label ID="lblVotes" runat="server" /></b> [&nbsp;<asp:LinkButton ID="lnkViewMutualVotes"
                                    runat="server" OnClick="lnkViewMutualVotes_Click" />&nbsp;] </li>
                        <li id="pnlNewUsers" runat="server">
                            <asp:LinkButton ID="lnkNewUsers" runat="server" OnClick="lnkNewUsers_Click"></asp:LinkButton>
                            <br />
                        </li>
                        <li id="pnlUsersOnline" runat="server">
                            <asp:Label ID="lblUsersOnline" runat="server"></asp:Label>&nbsp;[
                            <asp:LinkButton ID="lnkUsersOnline" runat="server" OnClick="lnkUsersOnline_Click"></asp:LinkButton>
                            &nbsp;]</li>
                        <li id="pnlUsersBroadcasting" runat="server">
                            <asp:Label ID="lblUsersBroadcasting" runat="server"></asp:Label>
                            &nbsp;[
                            <asp:LinkButton ID="lnkUsersBroadcasting" runat="server" OnClick="lnkUsersBroadcasting_Click"></asp:LinkButton>
                            &nbsp;]</li>
                        <li id="pnlNewMessages" runat="server">
                            <asp:Label ID="lblNewMessages" runat="server"></asp:Label>&nbsp;[
                            <asp:LinkButton ID="lnkNewMessages" runat="server" OnClick="lnkNewMessages_Click" />
                            &nbsp;]</li>
                        <li id="pnlRelationshipRequests" runat="server">
                            <asp:Label ID="lblRelationshipRequests" runat="server"></asp:Label>
                            &nbsp;[
                            <asp:LinkButton ID="lnkRelationshipRequests" runat="server" OnClick="lnkRelationshipRequests_Click" />&nbsp;]</li>
                        <li id="pnlFriendsRequests" runat="server">
                            <asp:Label ID="lblFriendsRequests" runat="server"></asp:Label>
                            &nbsp;[
                            <asp:LinkButton ID="lnkFriendsRequests" runat="server" OnClick="lnkFriendsRequests_Click" />&nbsp;]</li>
                        <li id="pnlNewEcards" runat="server">
                            <asp:Label ID="lblNewEcards" runat="server"></asp:Label>&nbsp;[
                            <asp:LinkButton ID="lnkNewEcards" runat="server" OnClick="lnkNewEcards_Click" />
                            &nbsp;]</li>
                        <li id="pnlStatusText" runat="server" visible="false">
                            <asp:UpdatePanel ID="UpdatePanelStatusText" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <%= Lang.Trans("Your status")%>:
                                        <div id="pnlViewStatusText" runat="server" style="display: inline">
                                            <b>
                                                <asp:Label ID="lblStatusText" runat="server"></asp:Label></b> [&nbsp;<asp:LinkButton
                                                    ID="lnkEditStatusText" runat="server" OnClick="lnkEditStatusText_Click" />&nbsp;]
                                        </div>
                                        <div id="pnlEditStatusText" runat="server" visible="false" style="display: inline">
                                            <asp:TextBox ID="txtStatusText" runat="server"></asp:TextBox>
                                            [&nbsp;<asp:LinkButton ID="lnkUpdateStatusText" runat="server" OnClick="lnkUpdateStatusText_Click"></asp:LinkButton>
                                            &nbsp;]
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li id="pnlBlockedUsers" runat="server">
                            <asp:Label ID="lblBlockedUsers" runat="server"></asp:Label>
                            &nbsp;[ <a href="BlockedUsers.aspx">
                                <%= Lang.Trans("View") %></a>&nbsp;]</li>
                        <li id="pnlPendingInvitations" runat="server" visible="false">
                            <asp:Label ID="lblPendingInvitatinos" runat="server" />&nbsp;[
                            <asp:LinkButton ID="lnkPendingInvitations" runat="server" OnClick="lnkPendingInvitations_Click" />
                            &nbsp;]</li>
                        <asp:Repeater ID="rptContestsRanks" Visible="False" runat="server">
                            <ItemTemplate>
                                <li>
                                    <%# String.Format(Lang.Trans("Your entry is ranked #{0} in the \"{1}\" contest"),
                            Eval("Rank"), Eval("ContestName")) %></li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Repeater ID="rptGroupTopicSubscriptions" runat="server" Visible="false">
                            <ItemTemplate>
                                <li>
                                    <%# String.Format(Lang.Trans("There are new posts in topic \"{0}\" in group \"{1}\""),
                            Eval("GroupTopicName"), Eval("GroupName"))%>&nbsp;[ <a href='<%# UrlRewrite.CreateShowGroupTopicsUrl((string) Eval("GroupID"), (string) Eval("GroupTopicID")) %>'>
                                <%= Lang.Trans("View") %></a>&nbsp;]</li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <li id="pnlInviteAFriend" runat="server">
                            <div class="separator">
                            </div>
                            <asp:Label ID="lblInviteAFriend" runat="server"></asp:Label>
                            <a href="<%= Config.Mailing.EnableAddressBookImporter ? "ImportFriends.aspx" : "InviteFriend.aspx" %>">
                                <%= Lang.Trans("Invite a friend") %></a></li>
                        <li id="pnlInviteFriendsFromFacebook" visible="false" runat="server"><a href="InviteFriendsFromFacebook.aspx">
                            <%= Lang.Trans("Invite friends from Facebook") %></a></li>
                        </ul>
                    </div>
                </td>
                <td valign="top" width="210px" align="right">
                    <asp:Image ID="imgPhoto" runat="server"></asp:Image>
                </td>
            </tr>
        </table>
        <div class="clear">
        </div>
        <components:BannerView ID="bvHomeAfterProfile" runat="server" Key="HomeAfterProfile">
        </components:BannerView>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
        <asp:UpdatePanel ID="upWebParts" runat="server">
            <ContentTemplate>
                <CustomWebPartZone:CustomWebPartZone TabIndex="1" BorderStyle="None" CssClass="StandardWebpartsWrap"
                    ID="wpzHomePageRightZone" WebPartVerbRenderMode="TitleBar" HeaderText="" Padding="0"
                    PartChromePadding="0" Width="100%" runat="server">
                    <PartStyle CellPadding="0" CssClass="StandardWebpartContent"></PartStyle>
                    <PartTitleStyle Width="100%" CssClass="StandardWebpartTop"></PartTitleStyle>
                    <ZoneTemplate>
                    </ZoneTemplate>
                    <CloseVerb Visible="false" />
                    <RestoreVerb Visible="false" ImageUrl="~/Images/restore.gif" />
                    <MinimizeVerb Visible="false" ImageUrl="~/Images/minimize.gif" />
                    <EditVerb Visible="false" ImageUrl="~/Images/edit.gif" />
                    <DeleteVerb Visible="false" ImageUrl="~/Images/delete.gif" />
                </CustomWebPartZone:CustomWebPartZone>
                <div id="pnlEditorZoneRight" runat="server">
                    <CustomEditorZone:CustomEditorZone PartChromePadding="0" Padding="0" Width="100%"
                        CssClass="StandardEditorZone" ID="ezEditorZoneRight" runat="server">
                        <HeaderStyle CssClass="StandardWebpartTop" />
                        <FooterStyle CssClass="StandardWebpartBtm"></FooterStyle>
                        <HeaderCloseVerb ImageUrl="~/Images/delete.gif" />
                        <ZoneTemplate>
                        </ZoneTemplate>
                    </CustomEditorZone:CustomEditorZone>
                </div>
                <asp:Panel ID="pnlCatalog" Visible="false" runat="server">
                    <div class="StandardBox WebpartsCatalogWrap">
                        <div class="StandardBoxTop">
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <h2 class="StandardBoxHeader">
                                            <%= Lang.Trans("Add Components") %></h2>
                                    </td>
                                    <td align="right" valign="middle">
                                        <asp:ImageButton ID="imgbCloseCatalog" runat="server" ImageUrl="~/Images/delete.gif"
                                            OnClick="imgbCloseCatalog_Click"></asp:ImageButton>&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="StandardBoxContent">
                            <asp:Repeater ID="rptCatalogWebParts" runat="server" OnItemCommand="rptCatalogWebParts_ItemCommand">
                                <ItemTemplate>
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0" class="WebpartsCatalogZone">
                                        <tr>
                                            <td valign="top" width="100">
                                                <img border="0" title='<%# ((string)Eval("Name")).Translate() %>' src='<%# Eval("ThumbnailURL") %>' />
                                            </td>
                                            <td valign="top">
                                                <div class="SectionHeading">
                                                    <%# ((string)Eval("Name")).Translate() %></div>
                                                <div class="note">
                                                    <%# ((string)Eval("Description")).Translate() %></div>
                                                <div class="buttons">
                                                    <asp:Button ID="btnAdd" CommandName="Add" Text='<%# "Add".Translate() %>' CommandArgument='<%# Eval("ControlPath") %>'
                                                        runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <div class="separator6">
                                    </div>
                                </SeparatorTemplate>
                            </asp:Repeater>
                            <div class="buttons">
                                <asp:Button ID="btnCancelCatalog" runat="server" OnClick="btnCancelCatalog_Click" />
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <div class="mode">
                    <asp:LinkButton ID="lnkCatalogForRightParts" runat="server" OnClick="lnkCatalogForRightParts_Click"></asp:LinkButton>
                </div>
                <components:BannerView ID="bvHomeRightBottom" runat="server" Key="HomeRightBottom">
                </components:BannerView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
