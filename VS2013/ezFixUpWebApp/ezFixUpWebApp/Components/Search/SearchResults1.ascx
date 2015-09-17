﻿<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="SearchResults1.ascx.cs"
    Inherits="ezFixUp.Components.Search.SearchResults1" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>

<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<script type="text/javascript">
    function triggerEvent(elmName, eventName) {
        //$('#' + elmName).click();
        //return;
        var element = document.getElementById(elmName);
        if (document.createEvent) {
            // dispatch for firefox + others
            var evt = document.createEvent("MouseEvents");
            evt.initMouseEvent(eventName, true, true, window,
                                0, 0, 0, 0, 0, false, false, false, false, 0, null);
            return !element.dispatchEvent(evt);
        } else if (document.createEventObject) {
            alert(elmName + eventName);
            try {
                // dispatch for IE
                var evt = document.createEventObject();
                $('#' + elmName).change();
                return element.fireEvent('on' + eventName, evt);
            } catch (e) {
                $('#' + elmName).click();
            }
        }
    }
    asyncScripts.push('scripts/components/searchResult1.js');
</script>
<asp:ScriptManagerProxy ID="smProxy" runat="server">
    <Scripts>
        <asp:ScriptReference Path="~/scripts/components/searchResult1.js" />
    </Scripts>
</asp:ScriptManagerProxy>
<asp:UpdatePanel ID="UpdatePanelSearchResults" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="dlUsers" />
        <asp:PostBackTrigger ControlID="dlUsersGrid" />
        <asp:AsyncPostBackTrigger ControlID="lnkShowGrid" />
        <asp:AsyncPostBackTrigger ControlID="lnkShowDetails" />
    </Triggers>
    <ContentTemplate>
        <div id="divSwitchModes" runat="server" visible="false" class="modes">
            <div style="width: 28px; height: 28px; background: #D2E5EB; padding: 3px; display: inline-block;
                cursor: pointer;" title='<%= "Show as Photos Gallery".Translate() %>' class="tipsy"
                onclick="triggerEvent('<%=lnkShowGrid.ClientID %>', 'click')" onmouseout="$(this).css({background:'whitesmoke'})"
                onmouseover="$(this).css({background:'#D2E5EB'})">
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
                <div style="width: 5px; height: 5px; margin: 1px; background: darkblue; float: left;">
                </div>
            </div>
            <div style="width: 28px; height: 28px; background: whitesmoke; padding: 3px; display: inline-block;
                cursor: pointer;" title='<%= "Show as Profiles List".Translate() %>' class="tipsy"
                onclick="triggerEvent('<%=lnkShowDetails.ClientID%>','click');" onmouseout="$(this).css({background:'whitesmoke'})"
                onmouseover="$(this).css({background:'#D2E5EB'})">
                <div style="width: 26px; height: 5px; margin: 1px; background: grey; float: left;">
                </div>
                <div style="width: 26px; height: 5px; margin: 1px; background: grey; float: left;">
                </div>
                <div style="width: 26px; height: 5px; margin: 1px; background: grey; float: left;">
                </div>
                <div style="width: 26px; height: 5px; margin: 1px; background: grey; float: left;">
                </div>
            </div>
            <asp:LinkButton ID="lnkShowGrid" CssClass="lnkShowGrid showPushedBtn" runat="server"
                BorderStyle="Inset" OnClick="lnkShowGrid_Click" Style="display: none;">
                <%= Lang.Trans("Show as Photo Grid") %></asp:LinkButton>
            <asp:LinkButton ID="lnkShowDetails" CssClass="lnkShowList showReleasedBtn" runat="server"
                Enabled="False" BorderStyle="Outset" OnClick="lnkShowDetails_Click" Style="display: none;">
                <%= Lang.Trans("Show as Profile List") %></asp:LinkButton>
        </div>
        <asp:DataList ID="dlUsersGrid" runat="server" CellPadding="1" CellSpacing="0" Style="margin: 0 auto;background-color:white;"
            RepeatDirection="Horizontal" Visible="false" OnItemCreated="dlUsersGrid_ItemCreated"
            RepeatLayout="Table">
            <ItemTemplate>
                <div class="ThumbsWrapGrid open_modal roundedItemOfSearchResults" title="<%#Eval("Name")%>" style="background-image: url(<%# (string)Eval("PhotoUrl") %>);
                    background-repeat: no-repeat; background-position: center center; width: <%#(int)(dlUsersGrid.Width.Value) %>px;
                    height: <%# (int)(dlUsersGrid.Width.Value) %>px;" onclick="location.href = '<%# UrlRewrite.CreateShowUserUrl((string) Eval("UserName")) %>';">
                    <table style="width: 100%; height: 100%;">
                        <tr>
                            <td style="vertical-align: bottom; width: 100%; height: 100%;">
                                <div class="ThumbIconsWrap fr" style="display: none;" viewed="<%#Eval("UserName")%>"
                                    image_id="<%# (int)Eval("PhotoId") %>">
                                    <span class="ThumbIconsWrapEnd2"><span class="ThumbIconsWrapBgr2">
                                        <img src="Images/more.png" data-hover="Images/more.png" border="0"
                                            class="rollover tipsys" alt="view-options" title="<%= "all action & mutual friends".Translate() %>" />
                                    </span></span>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <asp:DataList ID="dlUsers" CssClass="UsersBoxContent " RepeatColumns="2" RepeatLayout="table"
            GridLines="Both" RepeatDirection="Horizontal" runat="server" EnableViewState="false"
             Style="margin: 0 auto;border:none transparent;" 
            Visible="true" OnItemCommand="dlUsers_ItemCommand"
            OnItemCreated="dlUsers_ItemCreated" BorderWidth="0" OnItemDataBound="dlUsers_ItemDataBound">
            <ItemTemplate>
                <div style='border:solid skyblue 2px;margin:3px; ' class="roundedItemOfSearchResults">
                <table class="ItemOfSearchResults1">
                    <tr>
                        <td style="height: <%#ListPhotoWidthPx%>px;" >
                            <div class="ThumbsWrap open_modal roundedPrimaryImage" style="width: <%# ListPhotoWidthPx%>px; height: <%#ListPhotoWidthPx%>px;
                                background: url(<%# (string)Eval("PhotoUrl") %>) no-repeat;" onclick="location.href = '<%# UrlRewrite.CreateShowUserUrl((string) Eval("UserName")) %>';">
                                <table style="width: 100%; height: 100%;">
                                    <tr>
                                        <td style="vertical-align: bottom; width: 100%; height: 100%;">
                                            <div class="ThumbIconsWrap fr" style="display: none;" viewed="<%#Eval("UserName")%>"
                                                image_id="<%# (int)Eval("PhotoId") %>">
                                                <span style='color:redbrick;'><%= "more...".Translate() %>
                                                    <img src="Images/more.png" data-hover="Images/more.png" border="0" 
                                                        class="rollover tipsys" alt="view-options" title="<%= "all action & mutual friends".Translate() %>" >
                                                </span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td valign="top" style="width: <%# ResultLayoutWidth/2 - ListPhotoWidthPx%>px;">
                            <div style='height:135px;'>
                                <div class="fl">
                                    <span class="value">
                                        <%# ((string)Eval("Name")).Replace("...", "<a href='./ProfileBilling.aspx'>...</a>")%></span><br />
                                    <% if (ShowGender)
                                       { %>
                                    <span class="value">
                                        <%# ((string)Eval("Gender")).Translate() %></span>
                                    <% } %>
                                    <%= (ShowAge && ShowGender) ? "," : "" %>
                                    <% if (ShowAge)
                                       { %>
                                    <span class="value">
                                        <%# (string)(Eval("Age") ?? String.Empty) %></span>
                                    <% } %>
                                    <span class="ml5px itemicons" id="pnlIcons" runat="server" style='position:relative;top:3px;'>
                                    <%# Config.UserScores.ShowLevelIcons && (!(bool) Eval("HideUserLevelIcon")) && Eval("UserLevel") is UserLevel ?
					                String.Format("<img src=\"{0}\" border=\"0\" title=\"{1} - {2}\" />",
                                                    ((UserLevel)Eval("UserLevel")).GetIconUrl(), String.Format(Lang.Trans("Level {0}"), ((UserLevel)Eval("UserLevel")).LevelNumber), ((UserLevel)Eval("UserLevel")).Name) : ""%>
                                    <% if (Config.Users.EnableZodiacSign && ShowZodiacSign)
                                        { %>
                                    <%# (DataBinder.Eval(Container.DataItem, "ZodiacSign2") == DBNull.Value)
                                        ?
                                        String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                                        User.GetZodiacImageUrl((User.eZodiacSign)Eval("ZodiacSign1")), 
                                                        User.GetZodiacTooltip((User.eZodiacSign)Eval("ZodiacSign1")))
                                        :
                                        String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                                        User.GetZodiacImageUrl((User.eZodiacSign)Eval("ZodiacSign1")), 
                                                        User.GetZodiacTooltip((User.eZodiacSign)Eval("ZodiacSign1")))
                                        + String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                                        User.GetZodiacImageUrl(((User.eZodiacSign)Eval("ZodiacSign2"))), 
                                                        User.GetZodiacTooltip(((User.eZodiacSign)Eval("ZodiacSign2"))))
                                    %>
                                    <% } %>
                                    </span><br />
                                </div>
                            
                                <div class="clear"></div>
                                <div style='width: 100%; '>
                                    <%if (!Config.Users.DisableGenderInformation)
                                        {%>
                                    <span class="value">
                                        <%# Eval("InterestedIn") %></span><br />
                                        <%
                                        }%>
                                    <% if (false && showCity)
                                        { %>
                                    <span class="value">
                                        <%# Eval("Location") %></span><br />
                                    <% } %>
                                    <% if (false && ShowLastOnline)
                                        { %>
                                    <span class="label">
                                        <%= Lang.Trans("Last Online") %>:</span><br />
                                    <span class="value">
                                        <%# Eval("LastOnlineString") %></span><br />
                                    <% } %>                                
                                </div>
                            </div>
                            <div class="fl" style='width: auto; display:none;'>
                                <asp:LinkButton ID="lbBigMatchmaking" runat="server" CssClass="MatchBtn tipsys"
                                    PostBackUrl="" username='<%# Eval("Username")%>' image_id='<%# (int)Eval("PhotoId") %>'
                                    Visible='False' ToolTip='<%# (string)Eval("MatchmakingToolTip") %>'
                                    OnClientClick='<%# (string)Eval("MatchmakingOnClientClick") %>' /> 
                            </div>
                            <div class="fl" style='width: auto; display:block;'>
                                <asp:LinkButton ID="lbWink" runat="server" ToolTip='<%# Lang.Trans("Wink") %>' CssClass="WinkBtnSmall tipsys"
                                    PostBackUrl="" OnClientClick='<%# Eval("SendWinkPostbackUrl") %>' 
                                    Visible='<%# (bool)Eval("WinkIsVisible") %>'   />
                                <asp:LinkButton ID="lbMsg" runat="server" ToolTip='<%# Lang.Trans("Send Message") %>' CssClass="MsgBtnSmall tipsys" 
                                    PostBackUrl="" OnClientClick='<%# Eval("SendMsgPostbackUrl") %>'    />
                                <asp:LinkButton ID="lbGift" runat="server" ToolTip='<%# Lang.Trans("Send Gift") %>' CssClass="GiftBtnSmall tipsys" 
                                    PostBackUrl="" OnClientClick='<%# Eval("SendGiftPostbackUrl") %>'    />
                                <asp:LinkButton ID="lbAdd" runat="server" ToolTip='<%# Lang.Trans("Friendship request")%>' CssClass="AddBtnSmall tipsys" 
                                    OnClientClick='<%# Eval("AddFriendPostbackUrl") %>'
                                    PostBackUrl="" Visible='<%# (bool)Eval("AddFriendIsVisible") %>' />
                                <asp:LinkButton ID="lbMatchmaking" runat="server" CssClass="MatchBtnSmall tipsys"
                                    PostBackUrl="" username='<%# Eval("Username")%>' image_id='<%# (int)Eval("PhotoId") %>'
                                    Visible='<%# (bool)Eval("MatchmakingIsVisible") %>' ToolTip='<%# (string)Eval("MatchmakingToolTip") %>'
                                    OnClientClick='<%# (string)Eval("MatchmakingOnClientClick") %>' />
                                <%--<asp:LinkButton ID="lbBlock" runat="server" ToolTip='<%# Lang.Trans("Block")%>' CssClass="BlockBtnSmall" />--%>
                                <asp:LinkButton ID="lbFavorite" runat="server" ToolTip='<%# Lang.Trans("Add to favorite list")%>'
                                    OnClientClick='<%# Eval("AddFavoritePostbackUrl") %>' 
                                    CssClass="FavoriteBtnSmall tipsys" PostBackUrl="" Visible="false" />
                            </div>
                        </td>
                    </tr>
                    <tr class="tr2" >
                        <td colspan="2" >
                            <div class="fl" style="clear:left;">
                                <div class='fl' style='height: 20px;padding:5px;text-indent:0px;font-size:small;width:99%;'>
                                    <span style='color:#C1272D;text-transform:uppercase;'><%# "Mutual Friends".Translate() %> </span>
                                    <span style='color:#127BC2;cursor:pointer;margin-left:10px;' 
                                            onclick="jQuery('#iconsTutorialModal').dialog({resizable: false,height:600,width:900,modal: true});" >
                                        <u class='tipsys' title='<%# "Need help with mutual friends? click here...".Translate()%>'><%# "what is it?".Translate() %></u>
                                    </span>
                                </div>
                                
                                <div id="mfdiv1" class="fl ThumbsWrapSmall ThumbsWrapMF roundedMutualFriends" style="background: url(<%# (string)Eval("MutualFriend1_PhotoUrl") %>) no-repeat;
                                    visibility: <%# ((int)Eval("MutualFriend1_PhotoId") == -99) ? "hidden" : "visible" %>"
                                    title='<%# (string) Eval("MutualFriend1_Name")%>' onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("MutualFriend1_Username")%>'">
                                    <div id="mfpop1" class='<%= MatchmakerHelper.IsOnlyMatchmakerSessionUser() ? " " : "ThumbIconsWrapMF"  %>  <%# (string)Eval("MutualFriend1_PopBG") %>'
                                        style="display: none;" style='display: none;'>
                                        <asp:LinkButton ID="lbModalMF_AskAbout1" runat="server" CssClass="ModalAskAboutBtnSmall tipsys"
                                            ToolTip='<%# String.Format("Ask {0} About {1}".Translate(), (string)Eval("MutualFriend1_Name"), (string)Eval("Name"))%>'
                                            PostBackUrl='<%# (string)Eval("AskAbout1_PostbackUrl")%>' />
                                        <asp:Label ID="lbModalMF_MatchReq1" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys"
                                            onclick='cancelEvent(event);matchmakingRequestClick(event, this.id);' ToolTip='<%# String.Format("Request {0} For Matchmaking with {1}".Translate(), (string)Eval("MutualFriend1_Name"), (string)Eval("Name"))%>'
                                            mf_username='<%#(string)Eval("MutualFriend1_Username")%>' viewed='<%#(string) Eval("Username") %>'
                                            mf_name='<%#(string)Eval("MutualFriend1_Name")%>' viewed_name='<%#(string) Eval("Name") %>' />
                                    </div>
                                </div>
                                <div id="mfdiv2" class="fl ThumbsWrapSmall ThumbsWrapMF roundedMutualFriends" style="background: url(<%# (string)Eval("MutualFriend2_PhotoUrl") %>) no-repeat;
                                    visibility: <%# ((int)Eval("MutualFriend2_PhotoId") == -99) ? "hidden" : "visible" %>"
                                    title='<%# (string) Eval("MutualFriend2_Name")%>' onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("MutualFriend2_Username")%>'">
                                    <div id="mfpop2" class='<%= MatchmakerHelper.IsOnlyMatchmakerSessionUser() ? " " : "ThumbIconsWrapMF"  %>  <%# (string)Eval("MutualFriend2_PopBG") %>'
                                        style='display: none;'>
                                        <asp:LinkButton ID="lbModalMF_AskAbout2" runat="server" CssClass="ModalAskAboutBtnSmall tipsys"
                                            ToolTip='<%# String.Format("Ask {0} About {1}".Translate(), (string)Eval("MutualFriend2_Name"), (string)Eval("Name"))%>'
                                            PostBackUrl='<%# (string)Eval("AskAbout2_PostbackUrl")%>' />
                                        <asp:Label ID="lbModalMF_MatchReq2" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys"
                                            onclick='cancelEvent(event);matchmakingRequestClick(event, this.id);' ToolTip='<%# String.Format("Request {0} For Matchmaking with {1}".Translate(), (string)Eval("MutualFriend2_Name"), (string)Eval("Name"))%>'
                                            mf_username='<%#(string)Eval("MutualFriend2_Username")%>' viewed='<%#(string) Eval("Username") %>'
                                            mf_name='<%#(string)Eval("MutualFriend2_Name")%>' viewed_name='<%#(string) Eval("Name") %>' />
                                    </div>
                                </div>
                                <div id="mfdiv3" class="fl ThumbsWrapSmall ThumbsWrapMF roundedMutualFriends" style="background: url(<%# (string)Eval("MutualFriend3_PhotoUrl") %>) no-repeat;
                                    visibility: <%# ((int)Eval("MutualFriend3_PhotoId") == -99) ? "hidden" : "visible" %>"
                                    title='<%# (string) Eval("MutualFriend3_Name")%>' onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("MutualFriend3_Username")%>'">
                                    <div id="mfpop3" class='<%= MatchmakerHelper.IsOnlyMatchmakerSessionUser() ? " " : "ThumbIconsWrapMF"  %>  <%# (string)Eval("MutualFriend3_PopBG") %>'
                                        style='display: none;'>
                                        <asp:LinkButton ID="lbModalMF_AskAbout3" runat="server" CssClass="ModalAskAboutBtnSmall tipsys"
                                            ToolTip='<%# String.Format("Ask {0} About {1}".Translate(), (string)Eval("MutualFriend3_Name"), (string)Eval("Name"))%>'
                                            PostBackUrl='<%# (string)Eval("AskAbout3_PostbackUrl")%>' />
                                        <asp:Label ID="lbModalMF_MatchReq3" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys"
                                            onclick='cancelEvent(event);matchmakingRequestClick(event, this.id);' ToolTip='<%# String.Format("Request {0} For Matchmaking with {1}".Translate(), (string)Eval("MutualFriend3_Name"), (string)Eval("Name"))%>'
                                            mf_username='<%#(string)Eval("MutualFriend3_Username")%>' viewed='<%#(string) Eval("Username") %>'
                                            mf_name='<%#(string)Eval("MutualFriend3_Name")%>' viewed_name='<%#(string) Eval("Name") %>' />
                                    </div>
                                </div>
                                <div class="MoreIconWrap fl" viewed='<%# Eval("Username")%>' image_id='<%# (int)Eval("PhotoId") %>'
                                    title='<%# Lang.Trans("Click to open all actions and mutual friends...") %>'
                                    style="color:#BF1E2D;font-size:small;text-align:center;margin-top:35px;color:#127BC2;cursor:pointer;margin-left:10px;">
                                    <span><u><%="more...".Translate() %></u></span>                                    
                                </div>
                            </div>
                            
                        </td>
                    </tr>
                </table>
                </div>
            </ItemTemplate>
        </asp:DataList>
        <div>
            <div class="clear">
            </div>
            <div class="separator6">
            </div>
        </div>
        <asp:Panel ID="pnlPaginator" Visible="True" runat="server">
            <div class="line">
            </div>
            <div class="endbox">
                <table width="80%">
                    <tr>
                        <td style="width: 35%;">
                            <asp:LinkButton ID="lnkFirst" runat="server" CssClass="firstPageBtn tipsys"></asp:LinkButton>
                            <asp:LinkButton ID="lnkPrev" runat="server" CssClass="prevPageBtn tipsys"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="lblPager" runat="server"></asp:Label>
                        </td>
                        <td style="width: 35%;">
                            <asp:LinkButton ID="lnkNext" runat="server" CssClass="nextPageBtn tipsys"></asp:LinkButton>
                            <asp:LinkButton ID="lnkLast" runat="server" CssClass="lastPageBtn tipsys"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        <script type="text/javascript">
            function updatePreloadImagesPanel(content) {
                //alert(content);
                jQuery(function ($) {
                    jQuery("#preloadContent").html(content);
                });
                //alert($("#preloadContent").html());
            }
        </script>
        <div id="preloadContent">
        </div>
        <%
            string imgTagsStr = "";
            UserSession userSession = PageBase.GetCurrentUserSession();
            if (userSession != null && Page is Friends3)
            {
                if (userSession.UserFriendsPrelodImages != null)
                {
                    if (CurrentUserSession != null && CurrentUserSession.UserFriendsPrimaryPhotoIds != null)
                    {
                        CurrentUserSession.UserFriendsPrelodImages =
                            CurrentUserSession.UserFriendsPrimaryPhotoIds
                                .Select(id => ImageHandler.CreateImageUrl(id, 117, 117, true, true, true))
                                .Skip(20 * (CurrentPage))
                                .Take(20).ToArray();
                    }
                    imgTagsStr = userSession.UserFriendsPrelodImages
                        .Aggregate("", (current, imgTag) =>
                            current + String.Format("<img src='{0}' style='display:{1};' alt='preload_img' />"
                                                    , imgTag, "none"));
                    //var scriptTmplate = "<script type='text/javascript'>jQuery(function($) {$(document.body).append('{0}')});</script>";

                    System.Web.UI.ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "updatePreloadImagesCaller" + DateTime.Now.Ticks.ToString(),
                                                            "updatePreloadImagesPanel(\"" + imgTagsStr + "\");", true);
                    //litPreloadImages.Text = String.Format(scriptTmplate, imgTagsStr);
                }
            } %>
        <%--<script type="text/javascript">
            jQuery(function($) {
                $("#pnlPrelodImages").load("./friendslist.aspx #preloadContent");
            });
        </script>--%>
        <%--<script type="text/javascript" >
        alert('hello');
        updatePreloadImagesPanel("<%= imgTagsStr %>");
    </script>--%>
        <asp:Literal ID="litPreloadImages" runat="server"></asp:Literal>
    </ContentTemplate>
</asp:UpdatePanel>
<%--<div id="pnlPrelodImages">        
   </div>--%>