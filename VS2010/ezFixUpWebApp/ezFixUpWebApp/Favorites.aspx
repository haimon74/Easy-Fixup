<%@ Page language="c#" MasterPageFile="~/Site.Master" Codebehind="Favorites.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.Favourites" %>
<%@ Import namespace="ezFixUp"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="mm" TagName="MatchToFriend" Src="Components/Matchmaker/MatchToFriend.ascx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
	    <mm:MatchToFriend ID="MatchToFriend1" runat="server" Visible="true" />
    
        <components:BannerView id="bvFavoritesLeft" runat="server" Key="FavoritesLeft"></components:BannerView>
	</div>
	<div id="main">
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<div class="srvmsg">
			<asp:Label ID="lblMessage" Runat="server"></asp:Label>
		</div>
		<asp:DataList HorizontalAlign="Center" ID="dlFavourites" Runat="server" Width="100%" OnItemDataBound="dlFavourites_ItemDataBound" OnItemCreated="dlFavourites_ItemCreated">
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
						<% } %><span class="label"><%= Lang.Trans("Added to Favorites on") %>:</span><span class="value"><%# Eval("AddedToFavouritesOn") %><br /></span>
						<% if (showRating) { %><span class="label"><%= Lang.Trans("Rating") %>:</span><span class="value"><%# Eval("Rating") %><br /></span>
						<% } %><ul class="itemlinks">
						<li>
							<asp:HyperLink ID="lnkSendMessage" Runat="server">
							</asp:HyperLink>
						</li>
						<li>
							<asp:HyperLink ID="lnkRemoveFromFavourites" Runat="server">
							</asp:HyperLink>
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