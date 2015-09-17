<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Control Language="c#" 
            EnableViewState="false" 
            AutoEventWireup="True" 
            CodeBehind="NewUsers.ascx.cs"
            Inherits="ezFixUp.Components.NewUsers" 
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagName="SendMsgIcon" Src="~/Components/Buttons/SendMsgIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="SendWinkIcon" Src="~/Components/Buttons/SendWinkIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="SendGiftIcon" Src="~/Components/Buttons/SendGiftIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="AddFriendIcon" Src="~/Components/Buttons/AddFriendIcon.ascx" TagPrefix="btn" %>
<%@ Register TagName="AddFavoriteIcon" Src="~/Components/Buttons/AddFavoriteIcon.ascx" TagPrefix="btn" %>


<div id="UsersBox" class="hslice">
    <div class="UsersBoxTop">
        <h2 class="UsersBoxHeader">
            <asp:Label ID="lblNewUsersBoxHeader" runat="server"></asp:Label></h2>
        <span class="entry-title" style="display: none">
            <asp:Label ID="lblNewUsersBoxTitle" runat="server"></asp:Label></span> <span class="ttl"
                style="display: none">60</span>
    </div>
    <div class="entry-content">
        <div class="UsersBoxContent">
            <div class="scrollbarwrap">
                <asp:DataList ID="dlNewMembers" runat="server" 
                    Width="100%" 
                    RepeatDirection="Horizontal"
                    RepeatLayout="Flow" 
                    RepeatColumns="2"           
                    OnItemCreated="dlNewMembers_ItemCreated"
                    OnItemDataBound="dlNewMembers_ItemDataBound">
                    <ItemStyle Width="48%" />
                    <ItemTemplate>
                        <div class="clear4"></div>
                        <div >
                            <div class="fl" style="width:112px;">
                                <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                                    <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 110, 110, "noframe", false, true, true) %>
                                 </a>
                            </div>
                            <div class="fl" style="width:100px;">
                                <a class="user-name" href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                                    <b><%# Eval("Username") %></b></a><br />
                                <span id="pnlGenderAge" runat="server">
                                    <%# Lang.Trans(Eval("Gender").ToString()) %> (<%# Eval("Age") %><%# Eval("Age2") != String.Empty ? "," + Eval("Age2") : string.Empty %>)</span><br />
                                <span id="pnlLocation" runat="server"><%# Lang.Trans(Eval("City").ToString()) %>, <%# Lang.Trans(Eval("Country").ToString()) %>  </span><br />
                                <span id="pnlStatus" runat="server"><%# Eval("StatusText") %></span><br />
                                <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' 
                                    class="fl" style="margin-left:5px;" title='<%# Lang.Trans("Mutual Friend") %>'>
                                    <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 35, 35, "noframe", false, true, true) %>
                                </a>                                
                            </div>
                            <div class="fl" style="width:28px;">
                                <table>
                                    <tr><td>
                                        <btn:SendWinkIcon ID="btnSendWinkIcon" runat="server" Gravity="e" Size="Mini" />
                                    </td></tr>
                                    <tr><td>
                                        <btn:SendMsgIcon ID="btnSendMsgIcon" runat="server" Gravity="e" Size="Mini"  />
                                    </td></tr>
                                    <tr><td>
                                        <btn:SendGiftIcon ID="btnSendGiftIcon" runat="server" Gravity="e" Size="Mini"  />
                                    </td></tr>
                                    <tr><td>
                                        <btn:AddFriendIcon ID="btnAddFriendIcon" runat="server" Gravity="e" Size="Mini"  />
                                    </td></tr>
                                    <tr><td>
                                        <btn:AddFavoriteIcon ID="btnAddFavoriteIcon" runat="server"  Gravity="e" Size="Mini" />
                                    </td></tr>
                                </table>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:DataList>
            </div>
        </div>
    </div>
</div>
