<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Friends.aspx.cs" Inherits="AspNetDating.Friends" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import namespace="AspNetDating.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
	    <components:BannerView id="bvFriendsLeft" runat="server" Key="FriendsLeft"></components:BannerView>
	</div>
	<div id="main">
	    <div id="pnlRelationshipRequests" runat="server">
	        <uc1:LargeBoxStart ID="LargeBoxStart3" runat="server" />
		    <asp:DataList HorizontalAlign="Center" ID="dlRelationshipRequests" 
                Runat="server" Width="100%" 
                onitemcommand="dlRelationshipRequests_ItemCommand" onitemcreated="dlRelationshipRequests_ItemCreated" 
                >
			    <ItemTemplate>
		    	    <table cellpadding="0" cellspacing="0" class="searchresultstable">
		    		    <tr>
		    			    <td valign="top" align="center" width="100">
							    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
								    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, true) %></a></td>
		    			    <td valign="top" class="pleft5">
						    <% if (showSlogan) { %><h1 class="slogan"><%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %></h1>
						    <% } %><span class="label"><%= Lang.Trans("Username") %>:</span><span class="value"><%# Eval("Username") %><br /></span>
						    <span id="pnlAge" runat="server" class="label"><%= Lang.Trans("Age") %>:</span><span id="pnlAgeValue" class="value" runat="server"><%# Eval("Age") %><br /></span>
						    <% if (showLastOnline) { %><span class="label"><%= Lang.Trans("Last Online") %>:</span><span class="value"><%# Eval("LastOnlineString") %><br /></span>
						    <% } %><span class="label"><%= Lang.Trans("Relationship") %>:</span><span class="value"><%# Eval("Type") %><br /></span>
						    <% if (showRating) { %><span class="label"><%= Lang.Trans("Rating") %>:</span><span class="value"><%# Eval("Rating") %><br /></span>
						    <% } %><ul class="itemlinks">
						            <li>
							            <asp:LinkButton ID="lnkAccept" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Accept"></asp:LinkButton>
						            </li>
						            <li>
							            <asp:LinkButton ID="lnkReject" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Reject"></asp:LinkButton>
						            </li>
					               </ul>
		    			    </td>
		    		    </tr>
		    	    </table>
			    </ItemTemplate>
			    <SeparatorTemplate>
				    <div class="separator6"></div>
				    <div class="line"></div>
				    <div class="separator6"></div>
			    </SeparatorTemplate>
		    </asp:DataList>
		    <uc1:LargeBoxEnd ID="LargeBoxEnd3" runat="server" />
		</div>
	    <div id="pnlFriendsRequests" runat="server">
	        <uc1:LargeBoxStart ID="LargeBoxStart2" runat="server" />
		    <asp:DataList HorizontalAlign="Center" ID="dlPendingFriendsRequests" 
                Runat="server" Width="100%" 
                onitemcommand="dlPendingFriendsRequests_ItemCommand" 
                onitemcreated="dlPendingFriendsRequests_ItemCreated">
			    <ItemTemplate>
		    	    <table cellpadding="0" cellspacing="0" class="searchresultstable">
		    		    <tr>
		    			    <td valign="top" align="center" width="100">
							    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
								    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, true) %></a></td>
		    			    <td valign="top" class="pleft5">
						    <% if (showSlogan) { %><h1 class="slogan"><%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %></h1>
						    <% } %><span class="label"><%= Lang.Trans("Username") %>:</span><span class="value"><%# Eval("Username") %><br /></span>
						    <span id="pnlAge" runat="server" class="label"><%= Lang.Trans("Age") %>:</span><span id="pnlAgeValue" class="value" runat="server"><%# Eval("Age") %><br /></span>
						    <% if (showLastOnline) { %><span class="label"><%= Lang.Trans("Last Online") %>:</span><span class="value"><%# Eval("LastOnlineString") %><br /></span>
						    <% } %><span class="label"><%= Lang.Trans("Added to Friends on") %>:</span><span class="value"><%# Eval("AddedToFriendsOn") %><br /></span>
						    <% if (showRating) { %><span class="label"><%= Lang.Trans("Rating") %>:</span><span class="value"><%# Eval("Rating") %><br /></span>
						    <% } %><ul class="itemlinks">
						            <li>
							            <asp:LinkButton ID="lnkAccept" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Accept"></asp:LinkButton>
						            </li>
						            <li>
							            <asp:LinkButton ID="lnkReject" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Reject"></asp:LinkButton>
						            </li>
					               </ul>
		    			    </td>
		    		    </tr>
		    	    </table>
			    </ItemTemplate>
			    <SeparatorTemplate>
				    <div class="separator6"></div>
				    <div class="line"></div>
				    <div class="separator6"></div>
			    </SeparatorTemplate>
		    </asp:DataList>
		    <uc1:LargeBoxEnd ID="LargeBoxEnd2" runat="server" />
		</div>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<div class="srvmsg">
			<asp:Label ID="lblMessage" Runat="server" EnableViewState="false"></asp:Label>
		</div>
		<asp:DataList HorizontalAlign="Center" ID="dlFriends" Runat="server" 
            Width="100%" onitemcreated="dlFriends_ItemCreated" 
            onitemdatabound="dlFriends_ItemDataBound">
			<ItemTemplate>
		    	<table cellpadding="0" cellspacing="0" class="searchresultstable">
		    		<tr>
		    			<td valign="top" align="center" width="100">
							<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
								<%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, true) %></a></td>
		    			<td valign="top" class="pleft5">
						<% if (showSlogan) { %><h1 class="slogan"><%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %></h1>
						<% } %><span class="label"><%= Lang.Trans("Username") %>:</span><span class="value"><%# Eval("Username") %><br /></span>
						<span id="pnlAge" runat="server" class="label"><%= Lang.Trans("Age") %>:</span><span id="pnlAgeValue" class="value" runat="server"><%# Eval("Age") %><br /></span>
						<% if (showLastOnline) { %><span class="label"><%= Lang.Trans("Last Online") %>:</span><span class="value"><%# Eval("LastOnlineString") %><br /></span>
						<% } %><span class="label"><%= Lang.Trans("Added to Friends on") %>:</span><span class="value"><%# Eval("AddedToFriendsOn") %><br /></span>
						<% if (showRating) { %><span class="label"><%= Lang.Trans("Rating") %>:</span><span class="value"><%# Eval("Rating") %><br /></span>
						<% } %><ul class="itemlinks">
						<li>
							<asp:HyperLink ID="lnkSendMessage" Runat="server"></asp:HyperLink>
						</li>
						<li>
							<asp:HyperLink ID="lnkRemoveFromFriends" Runat="server"></asp:HyperLink>
						</li>
					</ul>
		    			</td>
		    		</tr>
		    	</table>
			</ItemTemplate>
			<SeparatorTemplate>
				<div class="separator6"></div>
				<div class="line"></div>
				<div class="separator6"></div>
			</SeparatorTemplate>
		</asp:DataList>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>