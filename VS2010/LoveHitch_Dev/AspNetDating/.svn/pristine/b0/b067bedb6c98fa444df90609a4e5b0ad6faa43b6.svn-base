<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="MyProfileWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.MyProfileWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="~/Components/FlexButton.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<div class="MyProfileContent">
    <div class="clear4" ></div>
    <div id="divFaceBookStatusUpdate" visible="false" runat="server">

        <%--<script type="text/javascript">
            FB.init("<%= AspNetDating.Classes.FacebookHelper.ApiKey %>", "Facebook/xd_receiver.htm", { permsToRequestOnConnect: "publish_stream" });
        </script>--%>

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
    <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td valign="top" >
                    <asp:Image ID="imgPhoto" runat="server" CssClass="photoframe" style="margin:0 5px 0 5px;border-width:1px;"></asp:Image>
                </td>
                <td valign="top" class="w60">
                    <div class="wrap-info">
                    <ul>
                        <li id="liWhoViewedMyProfile" runat="server">
                            <%= Lang.Trans("Your profile has been viewed") %>
                            <b><asp:Label ID="lblProfileViews" runat="server" /></b>&nbsp;<%= Lang.Trans("times") %>&nbsp;
                            <%--[&nbsp;<asp:LinkButton ID="lnkViewProfileViewers" runat="server" OnClick="lnkViewProfileViewers_Click" />&nbsp;]--%>
                            <asp:LinkButton ID="lnkViewProfileViewers" runat="server" OnClick="lnkViewProfileViewers_Click"
                                CssClass="btnViewTiny" Width="40px" Height="20px"   />
                                
                        </li>
                        <li id="pnlRating" runat="server">
                            <%= Lang.Trans("Average rating") %>: <b>
                                <asp:Label ID="lblRating" runat="server" /></b>
                        </li>
                        <li id="pnlVotes" runat="server">
                            <%= Lang.Trans("Your votes") %>: <b>
                            <asp:Label ID="lblVotes" runat="server" /></b> 
                            <%--[&nbsp;<asp:LinkButton ID="lnkViewMutualVotes" runat="server" OnClick="lnkViewMutualVotes_Click" />&nbsp;] --%>
                            <asp:LinkButton ID="lnkViewMutualVotes" runat="server" OnClick="lnkViewMutualVotes_Click"
                                CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlNewUsers" runat="server">
                            <asp:LinkButton ID="lnkNewUsers" runat="server" OnClick="lnkNewUsers_Click" Visible="false"></asp:LinkButton>
                        </li>
                        <li id="pnlUsersOnline" runat="server" Visible="false" style="display:none;">
                            <asp:Label ID="lblUsersOnline" runat="server"></asp:Label>&nbsp;
                            <%--[&nbsp;<asp:LinkButton ID="lnkUsersOnline" runat="server" OnClick="lnkUsersOnline_Click"></asp:LinkButton>&nbsp;]--%>
                            <asp:LinkButton ID="lnkUsersOnline" runat="server" OnClick="lnkUsersOnline_Click"
                                CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlUsersBroadcasting" runat="server">
                            <asp:Label ID="lblUsersBroadcasting" runat="server"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkUsersBroadcasting" runat="server" OnClick="lnkUsersBroadcasting_Click"
                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlNewMessages" runat="server">
                            <asp:Label ID="lblNewMessages" runat="server"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkNewMessages" runat="server" OnClick="lnkNewMessages_Click" 
                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlRelationshipRequests" runat="server">
                            <asp:Label ID="lblRelationshipRequests" runat="server"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkRelationshipRequests" runat="server" OnClick="lnkRelationshipRequests_Click" 
                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlFriendsRequests" runat="server">
                            <asp:Label ID="lblFriendsRequests" runat="server"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkFriendsRequests" runat="server" OnClick="lnkFriendsRequests_Click" 
                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlNewMsgs" runat="server" visible="false">
                            <asp:Label ID="lblNewMsgs" runat="server"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkNewMsgs" runat="server" OnClick="lnkNewEcards_Click" 
                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
                        <li id="pnlStatusText" runat="server" visible="false">
                            <asp:UpdatePanel ID="UpdatePanelStatusText" runat="server">
                                <ContentTemplate>
                                    <div>
                                        <%= Lang.Trans("Your status")%>:
                                        <div id="pnlViewStatusText" runat="server" style="display: inline">
                                            <b>
                                                <asp:Label ID="lblStatusText" runat="server"></asp:Label></b> 
                                                <asp:LinkButton ID="lnkEditStatusText" runat="server" OnClick="lnkEditStatusText_Click" 
                                                            CssClass="btnViewTiny" Width="40px" Height="20px"   />
                                        </div>
                                        <div id="pnlEditStatusText" runat="server" visible="false" style="display: inline">
                                            <asp:TextBox ID="txtStatusText" runat="server"></asp:TextBox>
                                            <asp:LinkButton ID="lnkUpdateStatusText" runat="server" OnClick="lnkUpdateStatusText_Click"
                                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </li>
                        <li id="pnlBlockedUsers" runat="server" Visible="false">
                            <asp:Label ID="lblBlockedUsers" runat="server"></asp:Label>&nbsp;
                            <asp:LinkButton ID="lnkBlockedUsers" runat="server" OnClick="lnkBlockedUsers_Click" 
                                    CssClass="btnViewTiny" Width="40px" Height="20px" />
                        </li>
                        <li id="pnlPendingInvitations" runat="server" visible="false">
                            <asp:Label ID="lblPendingInvitatinos" runat="server" />&nbsp;
                            <asp:LinkButton ID="lnkPendingInvitations" runat="server" OnClick="lnkPendingInvitations_Click" 
                                    CssClass="btnViewTiny" Width="40px" Height="20px"   />
                        </li>
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
                        <li id="pnlInviteFriendsFromFacebook" visible="false" runat="server">
                            <a class="myprofilelink" href="~/InviteFriendsFromFacebook.aspx">
                                    <%= Lang.Trans("Invite friends from Facebook") %>
                            </a> 
                        </li>
                    </ul>
                </div>
            </td>
            <td class="w20">
                <div id="pnlInviteAFriend" runat="server" style="height:50px;" visible="false">
                    <%--<div class="separator"/>--%>
                    <asp:Label ID="lblInviteAFriend" runat="server"></asp:Label>
                    <a  class="LargeButton" href="<%= Config.Mailing.EnableAddressBookImporter ? "./InviteFriends.aspx" : "./InviteFriend.aspx" %>">
                        <%= Lang.Trans("Invite a friend") %></a></div>
            </td>
        </tr>
    </table>
</div>
