<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchResults.ascx.cs" Inherits="ezFixUp.Components.Search.SearchResults" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>

<%@ Import namespace="ezFixUp"%>
<%@ Import Namespace="ezFixUp.Classes" %>
<script type="text/javascript">
    function pageLoad() {
        $('.ThumbsWrap').mouseover(function() {
            $('.ThumbIconsWrap').hide();
            $(this).find('.ThumbIconsWrap').show();
            $(function() { $('.tipsy').tipsy(); $('.tipsys').tipsy({ gravity: 's' }); });
        });
    }
    $(document.body).load(function() {
    
    });
</script>
<asp:UpdatePanel ID="UpdatePanelSearchResults" runat="server">
    <ContentTemplate>
        <div id="divSwitchModes" runat="server" visible="false" class="modes">
            [&nbsp;<asp:LinkButton ID="lnkShowGrid" runat="server" OnClick="lnkShowGrid_Click"><%= Lang.Trans("Show as Photo Grid") %></asp:LinkButton>&nbsp;|&nbsp;<asp:LinkButton ID="lnkShowDetails" runat="server" Enabled="False" OnClick="lnkShowDetails_Click"><%= Lang.Trans("Show as Profile List") %></asp:LinkButton>&nbsp;]
        </div>
        <div class="clear"></div>
	    <asp:DataList id="dlUsers" runat="server" Width="100%"  CellPadding="0" CellSpacing="0"
            OnItemCommand="dlUsers_ItemCommand" OnItemCreated="dlUsers_ItemCreated" 
            OnItemDataBound="dlUsers_ItemDataBound">
        <alternatingitemstyle CssClass="AlternativeRow"></alternatingitemstyle>
		    <ItemTemplate>
		    	<table cellpadding="0" cellspacing="0" width="100%" class="SearchResultsTable">
		    		<tr>
		    			<td valign="top" align="center" width="100">
						    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server">
							    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, true) %></a></td>
		    			<td valign="top" class="pleft5">
						    <% if (showSlogan) { %><h1 class="slogan"><%#Server.HtmlEncode((((string) Eval("Slogan")).Length > 80 ?
	                                                                                    ((string) Eval("Slogan")).Substring(0, 80) + "..." :
	                                                                                                                                           Eval("Slogan")) as string) %></h1><% } %><span class="label"><%= Lang.Trans("Username") %>:</span> 
								    <span class="value"><%# Eval("Username") %><br /></span><% if (GroupID != null) { %>&nbsp;(<%# Lang.Trans((string)Eval("GroupMemberType"))%>)
								    <% } %><% if (ShowAge) { %><span class="label"><%= Lang.Trans("Age")%>:</span>
								    <span class="value"><%# Eval("Age") %><br /></span><% } %><% if (ShowGender) { %><span class="label"><%= Lang.Trans("Gender") %>:</span>
								    <span class="value"><%# Lang.Trans((string)Eval("Gender")) %><br /></span><% } %><% if (showCity) { %><span  class="label"><%= Lang.Trans("Location") %>:</span>
								   <span class="value"><%# Eval("Location") %><br /></span><% } %><% if (ShowDistance) { %><span class="label" id="pnlDistance" visible='<%# ((string)Eval("Distance") != "") %>' runat="server"><%= Lang.Trans("Distance") %>:</span>
								    <span class="value"><%# Eval("Distance") %><br /></span><% } %><% if (ShowViewedOn) { %><span class="label"><%= Lang.Trans("Last Viewed") %>:</span>
								    <span class="value"><%# Eval("ViewedOnString") %><br /></span><% } %><% if (ShowModerationScore) { %><span class="label"><%= Lang.Trans("Moderation score") %>:</span>
								    <span class="value"><%# (int)Eval("ModerationScore") %><br /></span><% } %><% if (ShowLastOnline) { %><span class="label"><%= Lang.Trans("Last Online") %>:</span>
								    <span class="value"><%# Eval("LastOnlineString") %><br /></span><% } %><% if (showRating) { %><span class="label"><%= Lang.Trans("Rating") %>:</span>
								    <span class="value"><%# Eval("Rating") %><br /></span><% } %><% if (GroupID != null) { %><span class="label"><%= Lang.Trans("Member Since") %>:</span>
								    <span class="value"><%# Eval("GroupMemberJoinDate")%><br /></span><% } %><div class="itemicons" id="pnlIcons" runat="server">
						    <%# Config.UserScores.ShowLevelIcons && (!(bool) Eval("HideUserLevelIcon")) && Eval("UserLevel") is UserLevel ?
						        String.Format("<img src=\"{0}\" border=\"0\" title=\"{1} - {2}\" />",
	                                                        ((UserLevel)Eval("UserLevel")).GetIconUrl(), String.Format(Lang.Trans("Level {0}"), ((UserLevel)Eval("UserLevel")).LevelNumber), ((UserLevel)Eval("UserLevel")).Name) : ""%>
	                        <% if (Config.Users.EnableZodiacSign && ShowZodiacSign)
                               { %>
                               <%# (DataBinder.Eval(Container.DataItem, "ZodiacSign2") == DBNull.Value)
                                   ?
                                   String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                                                       User.GetZodiacImageUrl((User.eZodiacSign)Eval("ZodiacSign1")), User.GetZodiacTooltip((User.eZodiacSign)Eval("ZodiacSign1")))
                                   :
                                   String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                                                       User.GetZodiacImageUrl((User.eZodiacSign)Eval("ZodiacSign1")), User.GetZodiacTooltip((User.eZodiacSign)Eval("ZodiacSign1")))
                                   + String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />",
                                                                       User.GetZodiacImageUrl(((User.eZodiacSign)Eval("ZodiacSign2"))), User.GetZodiacTooltip(((User.eZodiacSign)Eval("ZodiacSign2"))))
                               %>
                            <% } %>
	                        <%# (bool)Eval("Blocked")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_blocked.gif", Lang.Trans("Blocked")) : "" %><%# (bool)Eval("MessageHistory")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_log.gif", Lang.Trans("Message History")) : "" %><%# (bool)Eval("VerifiedByUsers")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_verified.gif", Lang.Trans("Verified")) : "" %><%# (bool)Eval("VerifiedByAdmin")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_verifiedbyadmin.gif", Lang.Trans("Verified By Admin")) : "" %><%# (bool)Eval("PrivatePhoto")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_privatephoto.gif", Lang.Trans("Private Photo")) : "" %><%# (bool)Eval("VideoProfile")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_video.gif", Lang.Trans("Video Profile")) : "" %><%# (bool)Eval("HasBlog")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_blog.gif", Lang.Trans("Blog")) : "" %><%# (bool)Eval("IsBroadcastingVideo")? 
							    String.Format("<img src=\"{0}\" border=\"0\" title=\"{1}\" />", 
							    "images/icon_webcam.gif", Lang.Trans("Currently Broadcasting Video")) : "" %></div>
			    	<ul id="pnlManageGroupMembers" class="itemlinks" runat="server" visible='<%# GroupID != null && CurrentUserSession != null ? true : false %>'>
					    <li id="liMakeAdmin" runat="server">
						    <asp:LinkButton ID="lnkMakeAdmin" CommandName="MakeAdmin" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
					    <li id="liRemoveAdmin" runat="server">
						    <asp:LinkButton ID="lnkRemoveAdmin" CommandName="RemoveAdmin" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
					    <li id="liMakeModerator" runat="server">
						    <asp:LinkButton ID="lnkMakeModerator" CommandName="MakeModerator" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
					    <li id="liRemoveModerator" runat="server">
						    <asp:LinkButton ID="lnkRemoveModerator" CommandName="RemoveModerator" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
					    <li id="liMakeVip" runat="server">
						    <asp:LinkButton ID="lnkMakeVip" CommandName="MakeVip" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
					    <li id="liRemoveVip" runat="server">
						    <asp:LinkButton ID="lnkRemoveVip" CommandName="RemoveVip" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
					    <li id="liDeleteMember" runat="server">
						    <asp:LinkButton ID="lnkDeleteMember" CommandName="DeleteMember" CommandArgument='<%# Eval("Username") %>' Runat="server">
						    </asp:LinkButton>
					    </li>
		        	</ul>                            
		    			</td>
		    		</tr>
		    	</table>

		    </ItemTemplate>
		    <SeparatorTemplate>
			    <% if (showFriendsPath) { %><div style="width: 94px; text-align: center"><span style="font-size: large">&dArr;</span> <%= "knows".Translate() %></div>
			    <% } %>
		    </SeparatorTemplate>
	    </asp:DataList>
	    <asp:DataList id="dlUsersGrid" CssClass="UsersBoxContent" RepeatColumns="3" 
            RepeatLayout="table" RepeatDirection="Horizontal" runat="server" Width="100%" 
            EnableViewState="false" Visible="false" onitemcreated="dlUsersGrid_ItemCreated">
	        <ItemTemplate>
            	<div class="ThumbsWrap" style="background:url(<%# Config.Photos.EnablePhotoStack ?
ImageHandler.CreateImageStackUrl((string) Eval("Username"), 180, 150) :
ImageHandler.CreateImageUrl((int)Eval("PhotoId"), 180, 150, false, true, true) %>) no-repeat"
                    onclick="location.href = '<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>';">
				<div class="ThumbsHeightFix">		        
                    <div class="ThumbIconsWrap" style="display: none;">
                    	<span class="ThumbIconsWrapEnd">
                        <span class="ThumbIconsWrapBgr">
                        <a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>"><img src="Images/more.png" data-hover="Images/more.png" border="0" class="rollover tipsys" title="<%= "View user profile".Translate() %>" /></a>
                       	<a href="<%# "SendMessage.aspx?to_user=" + (string) Eval("Username") + "&src=search" %>"><img src="Images/SendEmailB.png" data-hover="Images/SendEmail.png" border="0" class="rollover tipsys" title="<%= "Send a message".Translate() %>"/></a>                    
                        <a href='<%# "~/SendEcard.aspx?uid=" + (string) Eval("Username") + "&src=search" %>' runat="server" visible='<%# CurrentUserSession != null && CurrentUserSession.CanSendEcards() != PermissionCheckResult.No %>'><img src="Images/SendEcardB.png" data-hover="Images/SendEcard.png" border="0" class="rollover tipsys" title="<%= "Send an e-card".Translate() %>"/></a>
                        <a href='<%# "~/AddRemoveFavorite.aspx?uid=" + (string) Eval("Username") + "&cmd=add&src=search" %>' runat="server" visible='<%# Config.Users.EnableFavorites %>'><img src="Images/FavoritesB.png" data-hover="Images/Favorites.png" border="0" class="rollover tipsys" title="<%= "Add to favorites".Translate() %>"/></a>
                        </span></span>                                                                                      
                    </div>  
                    </div>                                  
                	<div class="clear"></div>
                	
                    <ajaxToolkit:RoundedCornersExtender ID="rcePanelThumbsInfo" runat="server" 
                        TargetControlID="PanelThumbsInfo" Radius="6" Corners="All" />
                    <asp:Panel id="PanelThumbsInfo" runat="server" CssClass="MyThumbsInfo">
                        <a href='<%# "ShowUser.aspx?uid=" + Eval("Username") %>'><%# Eval("Username") %></a><br>
                        <span id="pnlGenderAge" runat="server">
                            <%# Eval("Age") %><span id="pnlDelimiter" runat="server">, </span><%# Lang.Trans((string) Eval("Gender")) %>
                        </span>
                	</asp:Panel>
                </div>
	        </ItemTemplate>
	    </asp:DataList>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="dlUsers" />
    </Triggers>
</asp:UpdatePanel>
<asp:Panel ID="pnlPaginator" Visible="True" Runat="server">
    <asp:UpdatePanel ID="UpdatePanelPaginator" runat="server">
        <ContentTemplate>
		    <div class="line"></div>
		    <div class="endbox">
			    <asp:LinkButton id="lnkFirst" runat="server"></asp:LinkButton>
			    <asp:LinkButton id="lnkPrev" runat="server"></asp:LinkButton>
			    <asp:Label id="lblPager" runat="server"></asp:Label>
			    <asp:LinkButton id="lnkNext" runat="server"></asp:LinkButton>
			    <asp:LinkButton id="lnkLast" runat="server"></asp:LinkButton>
		    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
