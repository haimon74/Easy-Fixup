<%@ Control Language="C#" 
            AutoEventWireup="true" 
            CodeBehind="FriendsOnlineBox.ascx.cs" 
            Inherits="AspNetDating.Components.FriendsOnlineBox" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="SmallBoxEnd.ascx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<div id="friendsonlinebox">
	<div class="box-top8">
		<h4 class="box-head8">
			<%= Lang.Trans("friends online") %>
		</h4>
	</div>
	<div class="SideMenuBoxContent">
		<asp:Repeater ID="rptFriends" Runat="server">
			<ItemTemplate>
			<div class="text">
				<a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>' onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()"><%# Eval("Username") %></a><br>
			</div>			
			</ItemTemplate>
		</asp:Repeater>
	</div>
	<div class="clear"></div>
</div>