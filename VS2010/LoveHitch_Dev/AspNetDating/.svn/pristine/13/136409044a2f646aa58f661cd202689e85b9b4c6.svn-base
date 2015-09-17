<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Control Language="c#" 
            AutoEventWireup="True" 
            CodeBehind="ViewProfile.ascx.cs"
            Inherits="AspNetDating.Components.Profile.ViewProfile" 
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register Src="ViewVideo.ascx" TagName="ViewVideo" TagPrefix="uc2" %>
<%@ Register Src="ViewVideoBroadcast.ascx" TagName="ViewVideoBroadcast" TagPrefix="uc2" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register src="../ModalPopup.ascx" tagname="ModalPopup" tagprefix="uc3" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<script type="text/javascript">
    var viewed = '<%= (this.User != null) ? this.User.Username : String.Empty %>';
    asyncScripts.push('scripts/components/viewProfile.js');
</script>
<%--<asp:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
<Scripts>
    <asp:ScriptReference  Path="~/scripts/jquery.popeye.js" />
    <asp:ScriptReference  Path="~/scripts/components/viewProfile.js" />
</Scripts>
</asp:ScriptManagerProxy>--%>
<input id="hidUsername" type="hidden" style="display:none;" name="hidUsername" runat="server"/>
<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
<components:BannerView ID="bvShowUserRightTop" runat="server" Key="ShowUserRightTop">
</components:BannerView>
<div class="viewprofile">
    <uc1:HeaderLine ID="hlSlogan" CssClass="translatable" runat="server" Visible="false" />
    <div class="SectionContent_" style="margin-bottom:2px;">
        <table class="ProfileInfoTable" cellpadding="0" cellspacing="1" width="100%">
            <tr>
                <td valign="top" class="skincolor">
                    <asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
                </td>
                <td valign="top" width="40%" class="skincolor">
                    <div class="fl">
                        <div id="divBesicDetails" runat="server">
                            <asp:Label CssClass="value skincolor" ID="lblBasicDetails" runat="server"></asp:Label>
                        </div>
                    </div>
                    
                    <div class="fl" style='display:none;'>
                        <div id="pnlGender" runat="server">
                            <span class="label skincolor ">
                                <%= Lang.Trans("Gender") %></span>:<asp:Label CssClass="value skincolor" ID="lblGender"
                                    runat="server"></asp:Label>
                        </div>
                        <div id="pnlAge" runat="server">
                            <span class="label skincolor ">
                                <%= Lang.Trans("Age") %></span>:<asp:Label CssClass="value skincolor" ID="lblAge"
                                    runat="server"></asp:Label>
                        </div>                        
                    </div>
                    <div class="fl m5px">
                        <% if (Config.Users.EnableZodiacSign && !Config.Users.DisableAgeInformation)
                           { %>
                        <% if (!zodiacSign2.HasValue) %>
                        <% { %>
                        <%= String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                    User.GetZodiacImageUrl(zodiacSign1), User.GetZodiacTooltip(zodiacSign1))%>
                        <% } %>
                        <% else
                            {%>
                        <%= String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                        User.GetZodiacImageUrl(zodiacSign1), User.GetZodiacTooltip(zodiacSign1)) %>
                        <%= String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                        User.GetZodiacImageUrl(zodiacSign2.Value), User.GetZodiacTooltip(zodiacSign2.Value)) %>
                        <% }%>
                        <% } %>
                        <asp:Literal ID="ltrSkype" runat="server" visible="false"></asp:Literal>
                    </div>
                    <div class="clear"></div>                    
                    <div id="pnlInterestedIn" runat="server" visible="false" style='display:none;'>
                        <span class="label skincolor ">
                            <%= ((Page as PageBase).CurrentUserSession.InterestedIn != AspNetDating.Classes.User.eGender.Matchmaker) ? Lang.Trans("Interested In") : "" %></span>:
                            <asp:Label CssClass="value skincolor" ID="lblInterestedIn" runat="server"></asp:Label>
                    </div>
                    <div id="pnlLocation" runat="server">
                        <span class="label skincolor ">
                            <%= Lang.Trans("From (Location)") %></span>:<span class="value skincolor">
                                <%= User.Country.Translate() %></span>
                        <br>
                    </div>
                    <div id="pnlDistance" class="skincolor" runat="server">
                        <span class="label skincolor ">
                            <%= Lang.Trans("Distance") %></span>:<asp:Label CssClass="value skincolor" ID="lblDistance"
                                runat="server"></asp:Label>
                        <br>
                    </div>
                    <div  style='visibility:hidden;'>
                        <span class="label skincolor ">
                            <%= Lang.Trans("Last Online") %></span>:
                        <asp:Label CssClass="value skincolor" ID="lblLastOnline"
                                runat="server"></asp:Label>
                    </div>
                    <div id="pnlStatusText" runat="server" visible="false">
                        <span class="label skincolor ">
                            <%= Lang.Trans("Status") %></span>:<asp:Label CssClass="value skincolor translatable"
                                ID="lblStatusText" runat="server"></asp:Label>
                    </div>
                    <div id="pnlProfileMatch" runat="server">
                        <span class="label skincolor ">
                            <%= Lang.Trans("Match") %></span>:<asp:Label CssClass="value skincolor" ID="lblMatchedPercentage"
                                runat="server"></asp:Label>
                    </div>
                    
                    <table id="profileCompleted" style="margin:22px 0 5px 0;" width="190" runat="server">
                        <tr>
                            <td colspan="2" style="color: Navy;">
                                <%= String.Format("Your Profile is {0} % completed".Translate(),GetProfileCompletePercentage())%>
                            </td>
                        </tr>
                        <tr>
                            <td id="completed" runat="server" style="background: rgb(190,30,45); height: 8px;">
                            </td>
                            <td id="notCompleted" runat="server" style="height: 8px;">
                            </td>
                        </tr>
                    </table>
                    <asp:UpdatePanel ID="updPanelUserPhotos" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="divUserThumbs" runat="server">
                                <asp:DataList ID="dlUserThumbs" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" CellSpacing="0" GridLines="None" SkinID="UserThumbs" Width="100%">
                                    <ItemTemplate>
                                        <a href='#' onclick="return false;" style="cursor:default;">
                                            <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 55, 55, "photoframe", false, true, true) %>
                                            <div class="clear">
                                            </div>
                                        </a>
                                    </ItemTemplate>
                                </asp:DataList>
                                <div class="clear"></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="hide">
                        <asp:UpdatePanel ID="UpdatePanelRating" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div id="pnlRating" runat="server" class="lineh">
                                    <span class="label skincolor ">
                                        <%= Lang.Trans("Average Rating") %></span>:<asp:Label CssClass="value skincolor"
                                            ID="lblRatingAverage" runat="server"></asp:Label>
                                    <br>
                                    <div id="pnlRateUser" runat="server">
                                        <span class="label skincolor ">
                                            <%= Lang.Trans("Your Rating") %></span>&nbsp;<asp:DropDownList ID="ddRating" runat="server">
                                            </asp:DropDownList>
                                        <asp:Button ID="btnRateUser" runat="server" OnClick="btnRateUser_Click"></asp:Button>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdatePanel ID="UpdatePanelVoting" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div id="pnlVoting" class="lineh" runat="server">
                                    <span class="label skincolor ">
                                        <%= Lang.Trans("Votes Score") %></span>:<asp:Label CssClass="value skincolor" ID="lblVotesScore"
                                            runat="server"></asp:Label>
                                    <div id="pnlVoteUser" runat="server">
                                        <span class="label skincolor ">
                                            <%= Lang.Trans("Your Vote") %></span>:
                                        <asp:ImageButton ID="btnVoteDown" ImageUrl="~/images/votedown.gif" runat="server"
                                            OnClick="btnVote_Click" />
                                        &nbsp;
                                        <asp:ImageButton ID="btnVoteUp" ImageUrl="~/images/voteup.gif" runat="server" OnClick="btnVote_Click" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div id="divMemberOf" visible="false" runat="server">
                        <span class="label skincolor ">
                            <%= Lang.Trans("Member of") %></span>:<span class="value"><asp:PlaceHolder ID="plhGroupsLinks"
                                runat="server" />
                            </span>
                    </div>
                    <div id="pnlRelationship" visible="false" runat="server">
                        <span id="spanRelationship" runat="server" class="label skincolor ">
                        </span>:<a id="lnkUsername" runat="server"></a><asp:Label CssClass="value skincolor"
                            ID="lblRelationship" runat="server"></asp:Label>
                    </div>
                    <div style="display:none;">
                    <asp:UpdatePanel ID="UpdatePanelVideo" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="pnlVideosHeaderLine" runat="server">
                                <uc1:HeaderLine ID="hlVideos" runat="server"></uc1:HeaderLine>
                            </div>
                            <div class="center">
                                <asp:Label ID="lblVideoPrivacySettingsError" runat="server" EnableViewState="false"></asp:Label></div>
                            <div id="pnlVideos" runat="server" class="SectionContent">
                                <div id="pnlRecordedVideo" runat="server" visible="false">
                                    <uc2:ViewVideo ID="ViewVideo1" runat="server" />
                                </div>
                                <div id="pnlVideoUpload" runat="server" visible="false">
                                    <div class="center">
                                        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                            width="325" height="262" id="flvplayer">
                                            <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                                            <param name="quality" value="high">
                                            <param name="bgcolor" value="#FFFFFF">
                                            <param name="wmode" value="transparent">
                                            <param name="allowfullscreen" value="true">
                                            <param name="flashvars" value="width=325&height=262&file=<%= VideoUploadUrl %>&image=<%= VideoUploadUrl.Replace(".flv", ".png") %>&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png" />
                                            <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                                quality="high" wmode="transparent" bgcolor="#FFFFFF" width="325" height="262"
                                                name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                                pluginspage="https://www.macromedia.com/go/getflashplayer" flashvars="width=325&height=262&file=<%= VideoUploadUrl %>&image=<%= VideoUploadUrl.Replace(".flv", ".png") %>&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png"></embed></object>
                                    </div>
                                </div>
                                <div id="pnlVideoUnlock" runat="server">
                                    <components:ContentView ID="cvUnlockVideo" Key="UnlockVideo" runat="server"></components:ContentView>
                                    <div class="center">
                                        <asp:Button ID="btnUnlockVideo" runat="server" OnClick="btnUnlockVideo_Click" /></div>
                                </div>
                                <div id="pnlVideoEmbed" runat="server" visible="false">
                                    <div class="center">
                                        <asp:Literal ID="ltrVideoEmbed" runat="server"></asp:Literal>
                                        <br />
                                        <asp:Label ID="lblVideoEmbedName" CssClass="skincolor" runat="server"></asp:Label>
                                    </div>
                                </div>
                                <div id="divVideoThumbnails" class="center" runat="server">
                                    <asp:DataList ID="dlVideos" Width="100%" CellPadding="3" RepeatColumns="3" RepeatDirection="Horizontal"
                                        runat="server" OnItemCommand="dlVideos_ItemCommand">
                                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" CssClass="overflow" Width="33%" />
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImageButton1" ImageUrl='<%# Eval("ThumbnailUrl") %>' CommandName="SelectVideo"
                                                CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>'
                                                runat="server" />
                                            <div>
                                                <asp:LinkButton ID="LinkButton1" CssClass="skinlinkcolor" Text='<%# Eval("Title") %>'
                                                    CommandName="SelectVideo" CommandArgument='<%# Eval("ThumbnailUrl") + "|" + Eval("VideoUrl") + "|" + Eval("Title") %>'
                                                    runat="server" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <div id="pnlViewAllVideos" runat="server" class="morelink">
                                        <asp:LinkButton ID="lnkViewAllVideos" CssClass="skinlinkcolor" runat="server" OnClick="lnkViewAllVideos_Click"></asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <asp:UpdatePanel ID="UpdatePanelAudioUploads" runat="server">
                        <ContentTemplate>
                            <div id="pnlAudioHeaderLine" runat="server">
                                <uc1:HeaderLine ID="hlAudio" runat="server"></uc1:HeaderLine>
                            </div>
                            <div id="pnlAudioUploads" class="SectionContent" runat="server">
                                <div class="center">
                                    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                        width="450" height="134" id="Object1">
                                        <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"/>
                                        <param name="quality" value="high"/>
                                        <param name="bgcolor" value="#FFFFFF"/>
                                        <param name="wmode" value="transparent"/>
                                        <param name="allowfullscreen" value="true"/>
                                        <param name="flashvars" value="width=450&height=134&playlist=right&playlistsize=220&file=Components/Profile/<%= Server.UrlEncode("UserAudioData.ashx?uid=" + User.Username + "&vid=" + (((PageBase)Page).CurrentUserSession != null ? ((PageBase)Page).CurrentUserSession.Username : String.Empty)) %>&searchbar=false&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png" />
                                        <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                            quality="high" wmode="transparent" bgcolor="#FFFFFF" width="450" height="134"
                                            name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                            pluginspage="https://www.macromedia.com/go/getflashplayer" allowscriptaccess="always"
                                            flashvars="width=450&height=134&playlist=right&playlistsize=220&file=Components/Profile/<%= Server.UrlEncode("UserAudioData.ashx?uid=" + User.Username + "&vid=" + (((PageBase)Page).CurrentUserSession != null ? ((PageBase)Page).CurrentUserSession.Username : String.Empty)) %>&searchbar=false&logo=<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/images/watermark2.png"></embed></object>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    </div>
                </td>
                <td>
                <table>
                    <tr>
                        <td style="width:35px;">
                            <div id="actionIcons1" runat="server"  style="width:30px;display:inline;">
                                <asp:LinkButton ID="lbWink" runat="server" CssClass="WinkBtnMini tipsys" />
                                <br />
                                <asp:LinkButton ID="lbMsg" runat="server"  CssClass="MsgBtnMini tipsys" />
                                <br />
                                <asp:LinkButton ID="lbGift" runat="server"  CssClass="GiftBtnMini tipsys" />
                                <br />                                
                            </div>
                        </td>
                        <td style="width:35px;">
                            <div id="actionIcons2" runat="server"  style="width:30px;display:inline;">
                                <asp:LinkButton ID="lbMatchTo" runat="server" CssClass="MatchBtnMini tipsys"
                                                 OnClientClick="setFriendToMatch(this.id);return false;" />
                                <asp:LinkButton ID="lbMatchWith" runat="server" CssClass="MatchBtnMini tipsys"
                                                 OnClientClick="setFriendToMatchWith(this.id);return false;" />
                                <asp:LinkButton ID="lbBlock" runat="server" OnClick="lbBlock_Clicked" CssClass="BlockBtnMini tipsys"/>
                                <asp:LinkButton ID="lbUnblock" runat="server" OnClick="lbUnblock_Clicked"  CssClass="BlockBtnMini tipsys"/>
                                <br />
                                <asp:LinkButton ID="lbAddFavorite" runat="server"  CssClass="FavoriteBtnMini tipsys" />
                                <asp:LinkButton ID="lbRemoveFavorite" runat="server"  CssClass="UnfavoriteBtnMini tipsys" />
                                <br />
                                <asp:LinkButton ID="lbAddFriend" runat="server" OnClick="lbAddFriend_Clicked"  CssClass="AddBtnMini tipsys" />
                                <asp:LinkButton ID="lbRemoveFriend" runat="server" OnClick="lbRemoveFriend_Clicked"  CssClass="RemoveBtnMini tipsys" />
                                <asp:LinkButton ID="lbAcceptFriendship" runat="server" OnClick="lbAcceptFriendship_Clicked" Visible="false"  CssClass="AcceptBtnMini tipsys" />
                                <asp:LinkButton ID="lbRejectFriendship" runat="server" OnClick="lbRejectFriendship_Clicked" Visible="false" CssClass="RejectBtnMini tipsys" />
                                <br />
                                
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="height:20px;"></div>
                            <div class="MoreIconWrap" id="moreBtn" runat="server">
                                <img src="Images/more.png" data-hover="Images/more.png" border="0" 
                                    class="rollover tipsys" alt="view-options" title="<%= "all action & mutual friends".Translate() %>" />
                            </div>
                            <script type="text/javascript">
                                jQuery(function($) {
                                    jQuery("div.MoreIconWrap").attr('viewed', viewed);
                                    jQuery("div.MoreIconWrap").attr('image_id', 0);
                                });
                            </script>
                        </td>
                    </tr>
                </table>
                </td>
                <td  style="display:inline-block;vertical-align:top;width:130px;">
                    <asp:UpdatePanel ID="UpdatePanelFriends" UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <div id="divUserFriends" runat="server"  style="display:inline-block;">
                                <div class="center">
                                    <asp:Label ID="lblFriendsPrivacySettingsError" runat="server" EnableViewState="false"></asp:Label>
                                </div>
                                <div id="pnlFriends" runat="server" >
                                    <div><%= "Mutual Friends".Translate() %></div>
                                    <asp:DataList ID="dlUserFriends" runat="server" RepeatColumns="1"  EnableViewState="false"
                                        RepeatDirection="Vertical" CellSpacing="2"  Width="100%"  >
                                        <ItemTemplate>
                                            <div id="mfdiv" class="ThumbsWrapMF" style="background:url('<%# (string)Eval("ImageUrl")%>') no-repeat" 
                                                        onclick="window.location='<%# (string)Eval("ShowUserUrl")%>'">
                                                <div id="mfpop" class="ThumbIconsWrapMF  <%# (string)Eval("FriendshipRequestBG") %>" style="display: none;">
                                                    <asp:LinkButton ID="lbModalMF_AskAbout" runat="server" CssClass="ModalAskAboutBtnSmall tipsys" 
                                                                    PostBackUrl='<%# (string)Eval("AskAboutUrl")%>' />
                                                    <asp:Label ID="lbModalMF_MatchReq" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys" 
                                                        viewed='<%# (string)Eval("ViewedUsername")%>' 
                                                        viewed_name='<%# (string)Eval("ViewedName")%>' 
                                                        mf_name='<%# (string)Eval("MutualFriendName")%>' 
                                                        mf_username='<%# (string)Eval("MutualFriendUsername")%>' 
                                                        onclick='cancelEvent(event);matchmakingRequestClick(event, this.id);' />
                                                </div>
                                                <div  title="<%# (string)Eval("MutualFriendUsername") %>" onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("MutualFriendUsername")%>'" style="display:block;"></div>
                                            </div>                                            
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <div id="divViewAllFriends" class="morelink" runat="server">
                                        <asp:LinkButton ID="lnkViewAllFriends" CssClass="skinlinkcolor" runat="server"
                                            OnClick="lnkViewAllFriends_Click" Visible="false"></asp:LinkButton>
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>
        </table>
    </div>    
    <asp:PlaceHolder ID="plhProfile" runat="server"></asp:PlaceHolder><br />
    <div id="pnlUserComments" runat="server" style="clear:both;">
        <uc1:HeaderLine ID="hlUserComments" runat="server"></uc1:HeaderLine>
        <div class="SectionContent">
            <asp:UpdatePanel ID="UpdatePanelComments" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Repeater ID="rptComments" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="commentswrap skincolor">
                                <nobr>[<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %>]
						<b>
						<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("FromUsername"))%>' target="_self" class="skinlinkcolor" onmouseover="showUserPreview('<%# Eval("FromUsername") %>')" onmouseout="hideUserPreview()">
								<%# Eval("FromUsername") %></a>: </b></nobr>
                                <%# Eval("CommentText") %>
                                <asp:LinkButton ID="lnkDeleteComment" CssClass="blinks" CommandName="DeleteComment"
                                    CommandArgument='<%# Eval("Id") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
                                    Visible='<%# Eval("CanDelete") %>' runat="server"></asp:LinkButton>
                            </div>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <div class="clear">
                            </div>
                            <div class="separator6">
                            </div>
                        </SeparatorTemplate>
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                    <div id="divViewAllComments" runat="server" class="morelink">
                        <asp:LinkButton ID="lnkViewAllComments" CssClass="skinlinkcolor" runat="server"></asp:LinkButton>
                    </div>
                    <div class="clear">
                    </div>
                    <div id="spanAddNewComment" class="wrap-sized" runat="server">
                        <div class="separator">
                        </div>
                        <div id="divAddCommentLink" class="center">
                            [&nbsp;<a href="javascript: void(0)" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';"
                                class="skinlinkcolor">
                                <%= Lang.Trans("Add Comment") %></a>&nbsp;]
                        </div>
                        <div id="divAddCommentBox">
                            <asp:TextBox ID="txtNewComment" CssClass="multilinetextbox" MaxLength="200" runat="server"
                                TextMode="MultiLine"></asp:TextBox><br>
                            <div class="buttons">
                                <asp:Button ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click">
                                </asp:Button>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div id="pnlVideoBroadcast" class="box videobroadcast" visible="false" runat="server">
        <uc1:HeaderLine ID="hlVideoBroadcast" runat="server"></uc1:HeaderLine>
        <div id="pnlSubscribe" runat="server" visible="false">
            <a href='<%= Config.Urls.Home %>/ManageProfile.aspx?sel=payment'>
                <%= "You need to upgrade your plan in order to view video broadcast".Translate() %></a>
        </div>
        <div id="pnlVideoStream" runat="server" class="SectionContent">
            <uc2:ViewVideoBroadcast ID="ViewVideoBroadcast1" runat="server" />
        </div>
    </div>
    <div id="pnlUnlockVideoStream" runat="server" visible="false">
        <components:ContentView ID="cvUnlockVideoStream" Key="UnlockVideoStream" runat="server">
        </components:ContentView>
        <div class="center">
            <asp:Button ID="btnUnlockVideoStream" runat="server" OnClick="btnUnlockVideoStream_Click" /></div>
    </div>
    
    
</div>

<uc3:ModalPopup ID="ModalPopup1" runat="server" Visible="false" ModalHeight="300" LinkText="click me..." TitleText="Modal Title" />

<components:BannerView ID="bvShowUserRightBottom" runat="server" Key="ShowUserRightBottom">
</components:BannerView>
</ContentTemplate>
</asp:UpdatePanel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
