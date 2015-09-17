<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="MatchmakingInProcessWebPart.ascx.cs"
            Inherits="AspNetDating.Components.WebParts.MatchmakingInProcessWebPart" %>
<%@ Import Namespace="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div class="UsersBoxContent">
    <div class="scrollbarwrap">
        <asp:DataList ID="dlMatchmakings" runat="server" SkinID="GenderFriendsMatchMaker" Width="100%"
            CellPadding="0" CellSpacing="0" RepeatDirection="Vertical" RepeatLayout="Table"
            RepeatColumns="1" OnItemCreated="dlGenderFriends_ItemCreated">
            <ItemTemplate>
                <table style='text-align:center;'>
                    <tr>
                        <td>
                            <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("MatchToUsername")) %>'>
                                <b><%# UserSession.GetViewedUserDisplayedNameByUsername(PageBase.GetCurrentUserSession().Username, (string)Eval("MatchToUsername"))%></b>
                            </a><br />
                            <span id="pnlMatchToGenderAge" runat="server"><%# (string)Eval("MatchToGender")%>
                                <span id="pnlMatchToDelimiter" runat="server">/</span>
                                (<%# Eval("MatchToAge")%>)
                            </span>
                            <span id="pnlMatchToLocation" runat="server"></span>
                            <span id="pnlMatchToAbout" runat="server"></span><br />
                            <asp:LinkButton ID="lbMatchToMsg" runat="server" 
                                ToolTip='<%# String.Format(Lang.Trans("Send a Personal Message to {0}"), 
                                                            UserSession.GetViewedUserDisplayedNameByUsername(PageBase.GetCurrentUserSession().Username, (string)Eval("MatchToUsername"))) %>'
                                CssClass="MmBlueMsgBtn tipsys" Width="35" Height="25" />
                                <br /><%# Lang.Trans("Personal Message") %>
                        </td>
                        <td>
                            <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("MatchToUsername")) %>'>
                                <%# (int)Eval("MatchToImageId") == 0 
                                ? ImageHandler.RenderFacebookImageTag(long.Parse((string)Eval("MatchToUsername")), ImageHandler.eFacebookImageType.Normal, "photoframe")
                                : ImageHandler.RenderImageTag((int)Eval("MatchToImageId"), 90, 90, "photoframe", false, true, true)%>
                            </a>
                                <div class="clear"></div>
                        </td>
                        <td >
                            <asp:LinkButton ID="lbSendBothMsg" runat="server" ToolTip='<%# Lang.Trans("Send both a message")%>'
                                CssClass="MmMidArrowBg tipsys"  Width="106" Height="65"/><br />
                                <%# Lang.Trans("Send Both")%>
                        </td>
                        <td>
                            <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("MatchWithUsername")) %>'>
                                <%# (int)Eval("MatchWithImageId") == 0
                                ? ImageHandler.RenderFacebookImageTag(long.Parse((string)Eval("MatchWithUsername")), ImageHandler.eFacebookImageType.Normal, "photoframe")
                                : ImageHandler.RenderImageTag((int)Eval("MatchWithImageId"), 90, 90, "photoframe", false, true, true)%>
                            </a>
                            <div class="clear"></div>
                        </td>
                        <td>
                            <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("MatchWithUsername")) %>'>
                                <b><%# UserSession.GetViewedUserDisplayedNameByUsername(PageBase.GetCurrentUserSession().Username, (string)Eval("MatchWithUsername"))%></b>
                            </a><br />
                            <span id="pnlMatchWithGenderAge" runat="server">
                                <%# (string)Eval("MatchWithGender")%>
                                <span id="pnlMatchWithDelimiter" runat="server">/</span>
                                (<%# Eval("MatchWithAge") %>)
                            </span>
                            <span id="pnlMatchWithMatchLocation" runat="server"></span>
                            <span id="pnlMatchWithAbout" runat="server"></span><br />
                            <asp:LinkButton ID="lbMatchWithMsg" runat="server"  
                                ToolTip='<%# String.Format(Lang.Trans("Send a Personal Message to {0}"), 
                                                            UserSession.GetViewedUserDisplayedNameByUsername(PageBase.GetCurrentUserSession().Username, (string)Eval("MatchWithUsername"))) %>'
                                CssClass="MmPurpleMsgBtn tipsys"  Width="35" Height="25"/>
                                <br /><%# Lang.Trans("Personal Message") %>
                        </td>                        
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>
    </div>
</div>
<div class="StandardWebpartBtm">
</div>
