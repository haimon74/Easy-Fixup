<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="NewUsersWebPart.ascx.cs"
            Inherits="AspNetDating.Components.WebParts.NewUsersWebPart" %>
<%@ Register TagName="ProfileList" Src="~/Components/Search/ProfileResultList.ascx" TagPrefix="dl" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<div class="UsersBoxContent">
             <table class="w100">
    <tr>
        <td style="padding:0 !important;">
   <div class="scrollbarwrap">
        <dl:ProfileList ID="plNewMembers" runat="server" ResultType="NewMembers"  
            LimitRows="1" RepeatColumns="2" GridMode="false" PaginatorEnabled="false" >
        </dl:ProfileList>
        <%--<asp:DataList ID="dlNewMembers" runat="server" SkinID="NewUsersHome" Width="100%"
            RepeatDirection="Horizontal" RepeatLayout="Table" CellPadding="0" CellSpacing="0"
            RepeatColumns="3" OnItemCreated="dlNewMembers_ItemCreated" OnItemDataBound="dlNewMembers_ItemDataBound">
            <ItemTemplate>
                <table>
                    <tr>
                        <td>
                            <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                                <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 90, 90, "photoframe", false, true, true) %></a><div
                                    class="clear">
                                </div>
                        </td>
                        <td>
                            <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                                <b>
                                    <%# Eval("Username") %></b></a><br />
                            <span id="pnlGenderAge" runat="server">
                                <%# Lang.Trans("Gender") %><span id="pnlDelimiter" runat="server">/</span>(<%# Eval("Age") %><%# Eval("Age2") != String.Empty ? "," + Eval("Age2") : string.Empty %>)</span>
                            <span id="pnlLocation" runat="server"></span><span id="pnlAbout" runat="server">
                            </span><a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                                <%# Lang.Trans("Mutual Friend") %>
                                <img id="pnlFriendImage" runat="server" />
                            </a>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <btn:SendWinkIcon ID="btnSendWinkIcon" runat="server" Gravity="e" Size="Mini" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <btn:SendMsgIcon ID="btnSendMsgIcon" runat="server" Gravity="e" Size="Mini" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <btn:SendGiftIcon ID="btnSendGiftIcon" runat="server" Gravity="e" Size="Mini" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <btn:AddFriendIcon ID="btnAddFriendIcon" runat="server" Gravity="e" Size="Mini" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <btn:AddFavoriteIcon ID="btnAddFavoriteIcon" runat="server" Gravity="e" Size="Mini" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:DataList>--%>
    </div>
    </td>
        <td class="w3" style="padding:3px 0 !important; vertical-align:bottom;">
            <asp:Button ID="btnContinueNewUsers" runat="server"  CssClass="btnContinueSymbol"
                        Text="»"
                ToolTip="continue" OnClick="btnContinueNewUsersClicked" /> 
        </td>
    </tr>
    </table>
</div>
<div class="StandardWebpartBtm">
</div>
