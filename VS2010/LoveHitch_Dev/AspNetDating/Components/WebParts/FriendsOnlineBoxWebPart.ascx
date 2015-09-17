<%@ Import Namespace="AspNetDating"%>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="FriendsOnlineBoxWebPart.ascx.cs" 
            Inherits="AspNetDating.Components.WebParts.FriendsOnlineBoxWebPart" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>

<div id="friendsonlinebox">
	<div class="SideMenuBoxContent">
    	<div class="SideMenuBoxInfo">    
            <asp:MultiView ID="mvFriends" runat="server" ActiveViewIndex="0">
            <asp:View ID="vFriends" runat="server">
                <asp:Repeater ID="rptFriends" Runat="server">
                    <ItemTemplate>
                    <div class="text">
                        <a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>' 
                           onmouseover="showUserPreview('<%# Eval("Username") %>')" 
                           onmouseout="hideUserPreview()"><%# Eval("Name") %>
                        </a>
                        <span id="pnlStatusText" runat="server" 
                              visible='<%# Config.Users.EnableUserStatusText && User.IsPaidMember(((PageBase) Page).CurrentUserSession.Username) && Eval("StatusText") != DBNull.Value %>'> - <%# Eval("StatusText") %>
                        </span>
                        <br>
                    </div>			
                    </ItemTemplate>
                </asp:Repeater>
            </asp:View>
            <asp:View ID="vNoFriends" runat="server">
                <div class="center">
                    <%= Lang.Trans("There are no friends online!") %></div>
            </asp:View>
            </asp:MultiView>
		</div>
	</div>    
	<div class="clear"></div>
</div>
<div class="SideMenuWebpartBtm"></div>