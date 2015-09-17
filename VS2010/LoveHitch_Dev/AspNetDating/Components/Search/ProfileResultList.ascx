﻿<%@ Control Language="c#" AutoEventWireup="True" CodeBehind="ProfileResultList.ascx.cs"
    Inherits="AspNetDating.Components.Search.ProfileResultList" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<script type="text/javascript">
    asyncScripts.push('scripts/components/searchResult1.js');
</script>

<asp:DataList ID="dlUsers" CssClass="UsersBoxContent" RepeatColumns="2" RepeatLayout="table"
    GridLines="None" RepeatDirection="Horizontal" runat="server" EnableViewState="false"
    CellSpacing="0" Visible="true" OnItemCreated="dlUsers_ItemCreated" BorderWidth="0"
    OnItemDataBound="dlUsers_ItemDataBound">
    <ItemTemplate>
        <table class="ItemOfSearchResults1">
            <tr>
                <td style="height: <%#ListPhotoWidthPx%>px;">
                    <div class="ThumbsWrap open_modal roundedPrimaryImage" style="width: <%# ListPhotoWidthPx%>px; height: <%#ListPhotoWidthPx%>px;
                            background: url(<%# (string)Eval("PhotoUrl") %>) no-repeat;" onclick="location.href = '<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>';" >
                        <table style="width: 100%; height: 100%;">
                            <tr>
                                <td style="vertical-align: bottom; width: 100%; height: 100%;">
                                    <div class="ThumbIconsWrap fr" style="display: none;" viewed="<%#Eval("Username")%>"
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
                            <div class="clear">
                            </div>
                            <span class="value">
                                <%# ((string)Eval("Name")).Replace("...", "<a href='./ProfileBilling.aspx'>...</a>")%></span>
                            <div class="clear">
                            </div>
                            <% if (ShowGender)
                               { %>
                            <span class="value">
                                <%# Eval("Gender") %></span>
                            <% } %>
                            <%= (ShowAge && ShowGender) ? "," : "" %>
                            <% if (ShowAge)
                               { %>
                            <span class="value">
                                <%# Lang.Trans((string)Eval("Age")) %></span>
                            <% } %>
                            <span class="ml5px itemicons" id="pnlIcons" runat="server" style="display: block;position:relative;top:3px;">
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
                            </span>
                        </div>
                    
                        <%if (!Config.Users.DisableGenderInformation)
                        {%>
                        <div class="fl">
                            <span class="value"><%# (string)Eval("InterestedIn") %></span><br />
                        </div>
                        <%
                        }%>                                                    
                        <% if (false && showCity)
                           { %>
                        <div class="fl">
                            <span class="value">
                                <%# Eval("Location") %></span>
                            <% } %>
                        </div>
                        <% if (ShowLastOnline)
                           { %>
                        <div class="fl">
                            <span class="label">
                                <%= Lang.Trans("Last Online") %>:</span><br />
                            <span class="value">
                                <%# Eval("LastOnlineString") %></span><br />
                        </div>
                        <% } %>
                    </div>
                    <div class="fl" style='width: auto; display:none;'>
                        <asp:LinkButton ID="lbBigMatchmaking" runat="server" CssClass="MatchBtn tipsys"
                            PostBackUrl="" username='<%# Eval("Username")%>' image_id='<%# (int)Eval("PhotoId") %>'
                            Visible='False' ToolTip='<%# (string)Eval("MatchmakingToolTip") %>'
                            OnClientClick='<%# (string)Eval("MatchmakingOnClientClick") %>' /> 
                    </div>
                    <div class="fl" style='width: auto; display:block;'>
                        <asp:LinkButton ID="lbWink" runat="server" ToolTip='<%# Lang.Trans("Wink") %>' 
                            CssClass="WinkBtnSmall tipsys"
                            PostBackUrl='' OnClientClick='<%# Eval("SendWinkPostbackUrl") %>'
                            Visible='<%# (bool)Eval("WinkIsVisible") %>' />
                        <asp:LinkButton ID="lbMsg" runat="server" ToolTip='<%# Lang.Trans("Send Message") %>'
                            CssClass="MsgBtnSmall tipsys" PostBackUrl='' OnClientClick='<%# Eval("SendMsgPostbackUrl") %>' />
                        <asp:LinkButton ID="lbGift" runat="server" ToolTip='<%# Lang.Trans("Send Gift") %>'
                            CssClass="GiftBtnSmall tipsys" PostBackUrl='' OnClientClick='<%# Eval("SendGiftPostbackUrl") %>' />
                        <asp:LinkButton ID="lbAdd" runat="server" ToolTip='<%# Lang.Trans("Friendship request")%>'
                            CssClass="AddBtnSmall tipsys" PostBackUrl='' OnClientClick='<%# Eval("AddFriendPostbackUrl") %>'
                            Visible='<%# (bool)Eval("AddFriendIsVisible") %>' />
                        <asp:LinkButton ID="lbMatchmaking" runat="server" CssClass="MatchBtnSmall tipsys"
                            PostBackUrl="" username='<%# Eval("Username")%>' image_id='<%# (int)Eval("PhotoId") %>'
                            Visible='<%# (bool)Eval("MatchmakingIsVisible") %>' ToolTip='<%# (string)Eval("MatchmakingToolTip") %>'
                            OnClientClick='<%# (string)Eval("MatchmakingOnClientClick") %>' />
                        <asp:LinkButton ID="lbFavorite" runat="server" ToolTip='<%# Lang.Trans("Add to favorite list")%>'
                            CssClass="FavoriteBtnSmall tipsys" PostBackUrl='' OnClientClick='<%# Eval("AddFavoritePostbackUrl") %>'
                            Visible="false" />
                    </div>
                </td>                
            </tr>
            <tr class="tr2">
                <td colspan="2">
                    <div class="fl">
                        <div class='fl' style='height: 20px;padding:5px;text-indent:0px;font-size:small;width:99%;'>
                            <span style='color:#C1272D;text-transform:uppercase;'><%# "Mutual Friends".Translate() %> </span>
                            <span style='color:#127BC2;cursor:pointer;margin-left:10px;' 
                                    onclick="jQuery('#iconsTutorialModal').dialog({resizable: false,height:600,width:900,modal: true});" >
                                <u class='tipsys' title='<%# "Need help with mutual friends? click here...".Translate()%>'><%# "what is it?".Translate() %></u>
                            </span>
                        </div>
                                
                        <div id="mfdiv1" class="fl ThumbsWrapSmall ThumbsWrapMF" style="background: url(<%# (string)Eval("MutualFriend1_PhotoUrl") %>) no-repeat;
                            visibility: <%# ((int)Eval("MutualFriend1_PhotoId") == -99) ? "hidden" : "visible" %>"
                            title='<%# (string) Eval("MutualFriend1_Name")%>'>
                            <div id="mfpop1" class='<%= MatchmakerHelper.IsOnlyMatchmakerSessionUser() ? " " : "ThumbIconsWrapMF"  %>  <%# (string)Eval("MutualFriend1_PopBG") %>' style="display: none;" style='display: none;'>
                                <asp:LinkButton ID="lbModalMF_AskAbout1" runat="server" CssClass="ModalAskAboutBtnSmall tipsys" 
                                    ToolTip='<%# String.Format("Ask {0} About {1}".Translate(), (string)Eval("MutualFriend1_Name"), (string)Eval("Name"))%>'
                                    PostBackUrl='<%# (string)Eval("AskAbout1_PostbackUrl")%>' />
                                <asp:Label ID="lbModalMF_MatchReq1" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys" 
                                    onclick='event.preventDefault();matchmakingRequestClick(event, this.id);' 
                                    ToolTip='<%# String.Format("Request {0} For Matchmaking with {1}".Translate(), (string)Eval("MutualFriend1_Name"), (string)Eval("Name"))%>'
                                    mf_username='<%#(string)Eval("MutualFriend1_Username")%>' viewed='<%#(string) Eval("Username")%>' 
                                    mf_name='<%#(string)Eval("MutualFriend1_Name")%>' viewed_name='<%#(string) Eval("Name") %>' />
                            </div>
                            <div  title="<%# (string)Eval("Username") %>" onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("Username")%>'" style="display:block;"></div>
                        </div> 
                        <div id="mfdiv2" class="fl ThumbsWrapSmall ThumbsWrapMF" style="background: url(<%# (string)Eval("MutualFriend2_PhotoUrl") %>) no-repeat;
                            visibility: <%# ((int)Eval("MutualFriend2_PhotoId") == -99) ? "hidden" : "visible" %>"
                            title='<%# (string) Eval("MutualFriend2_Name")%>'>
                            <div id="mfpop2" class='<%= MatchmakerHelper.IsOnlyMatchmakerSessionUser() ? " " : "ThumbIconsWrapMF"  %>  <%# (string)Eval("MutualFriend2_PopBG") %>' style='display: none;'>
                                <asp:LinkButton ID="lbModalMF_AskAbout2" runat="server" CssClass="ModalAskAboutBtnSmall tipsys" 
                                    ToolTip='<%# String.Format("Ask {0} About {1}".Translate(), (string)Eval("MutualFriend2_Name"), (string)Eval("Name"))%>'
                                    PostBackUrl='<%# (string)Eval("AskAbout2_PostbackUrl")%>' />
                                <asp:Label ID="lbModalMF_MatchReq2" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys" 
                                    onclick='event.preventDefault();matchmakingRequestClick(event, this.id);' 
                                    ToolTip='<%# String.Format("Request {0} For Matchmaking with {1}".Translate(), (string)Eval("MutualFriend2_Name"), (string)Eval("Name"))%>'
                                    mf_username='<%#(string)Eval("MutualFriend2_Username")%>' viewed='<%#(string) Eval("Username") %>'
                                    mf_name='<%#(string)Eval("MutualFriend2_Name")%>' viewed_name='<%#(string) Eval("Name") %>' />
                            </div>
                            <div  title="<%# (string)Eval("Username") %>" onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("Username")%>'" style="display:block;"></div>
                        </div>
                        <div id="mfdiv3" class="fl ThumbsWrapSmall ThumbsWrapMF" style="background: url(<%# (string)Eval("MutualFriend3_PhotoUrl") %>) no-repeat;
                                visibility: <%# ((int)Eval("MutualFriend3_PhotoId") == -99) ? "hidden" : "visible" %>"
                                title='<%# (string) Eval("MutualFriend3_Name")%>'>
                            <div id="mfpop3" class='<%= MatchmakerHelper.IsOnlyMatchmakerSessionUser() ? " " : "ThumbIconsWrapMF"  %>  <%# (string)Eval("MutualFriend3_PopBG") %>' style='display: none;'>
                                <asp:LinkButton ID="lbModalMF_AskAbout3" runat="server" CssClass="ModalAskAboutBtnSmall tipsys" 
                                    ToolTip='<%# String.Format("Ask {0} About {1}".Translate(), (string)Eval("MutualFriend3_Name"), (string)Eval("Name"))%>'
                                    PostBackUrl='<%# (string)Eval("AskAbout3_PostbackUrl")%>' />
                                <asp:Label ID="lbModalMF_MatchReq3" runat="server" CssClass="ModalMatchRequestBtnSmall tipsys" 
                                        onclick='event.preventDefault();matchmakingRequestClick(event, this.id);' 
                                        ToolTip='<%# String.Format("Request {0} For Matchmaking with {1}".Translate(), (string)Eval("MutualFriend3_Name"), (string)Eval("Name"))%>'
                                        mf_username='<%#(string)Eval("MutualFriend3_Username")%>' viewed='<%#(string) Eval("Username") %>'
                                        mf_name='<%#(string)Eval("MutualFriend3_Name")%>' viewed_name='<%#(string) Eval("Name") %>' />
                            </div>
                            <div  title="<%# (string)Eval("Username") %>" onclick="location.href = 'ShowUser.aspx?uid=<%# (string)Eval("Username")%>'" style="display:block;"></div>
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
    </ItemTemplate>
</asp:DataList>
<asp:Panel ID="pnlPaginator" Visible="True" runat="server">
    <div class="line">
    </div>
    <div class="endbox">
        <table width="80%">
            <tr>
                <td style="width:35%;">
                    <asp:LinkButton ID="lnkFirst" runat="server" CssClass="firstPageBtn tipsys"></asp:LinkButton>
                    <asp:LinkButton ID="lnkPrev" runat="server" CssClass="prevPageBtn tipsys"></asp:LinkButton>
                </td>
                <td>
                    <asp:Label ID="lblPager" runat="server"></asp:Label>
                </td>
                <td style="width:35%;">
                    <asp:LinkButton ID="lnkNext" runat="server" CssClass="nextPageBtn tipsys"></asp:LinkButton>
                    <asp:LinkButton ID="lnkLast" runat="server" CssClass="lastPageBtn tipsys"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
