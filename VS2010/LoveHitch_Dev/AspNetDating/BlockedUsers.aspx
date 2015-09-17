<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="BlockedUsers.aspx.cs" Inherits="AspNetDating.BlockedUsers" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
		<uc1:SideMenuStart ID="SideMenuStart1" runat="server"></uc1:SideMenuStart>
        <div id="fmt">
            <ul class="fmt">
                <li  id="pnlBackLink" runat="server">
                    <asp:LinkButton ID="lnkBack" Runat="server"/>
                </li>                        
            </ul>
        </div>        
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
		<div style="height:300px;position:static;">
            <components:BannerView ID="bvBlockedUsersLeftBottom" runat="server" Key="BlockedUsersLeftBottom">
            </components:BannerView>
        </div>
	</div>
	<div id="main">
	    <asp:UpdatePanel ID="UpdatePanelBlockedUsers" runat="server">
	    <ContentTemplate>
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<div class="srvmsg">
			<asp:Label ID="lblMessage" Runat="server"></asp:Label>
		</div>
		<asp:DataList HorizontalAlign="Center" ID="dlBlockedUsers" Runat="server" 
            Width="100%" onitemcommand="dlBlockedUsers_ItemCommand" 
            onitemcreated="dlBlockedUsers_ItemCreated">
			<ItemTemplate>
		    	<table cellpadding="0" cellspacing="0" class="searchresultstable">
		    		<tr>
		    			<td valign="top" align="center" width="100">
							<a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' runat="server" id="A1">
								<%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, true) %></a></td>							
						<td valign="top" class="pleft5">						
							<% if (showSlogan) { %><h1 class="slogan"><%# ((string) Eval("Slogan")).Length > 80 ? ((string) Eval("Slogan")).Substring(0, 80) + "..." : Eval("Slogan") %></h1>
							<% } %><span class="label"><%= Lang.Trans("Username") %>:</span>
								<span class="value"><%# Eval("Username") %><br /></span>
							<span id="pnlAge" runat="server" class="label"><%= Lang.Trans("Age") %>:</span>
								<span class="value"><%# Eval("Age") %><br /></span>
							<% if (showLastOnline) { %><span class="label"><%= Lang.Trans("Last Online") %>:</span> 
									<span class="value"><%# Eval("LastOnlineString") %><br /></span>
							<% } %><span  class="label"><%= Lang.Trans("Blocked on") %>:</span>
									<span class="value"><%# Eval("BlockedOn") %><br /></span>
							<% if (showRating) { %><span class="label"><%= Lang.Trans("Rating") %>:</span>
									<span class="value"><%# Eval("Rating") %><br /></span>								
							<% } %><ul class="itemlinks">
								<li>
									<asp:LinkButton ID="lnkUnblock" Runat="server" CommandArgument='<%# Eval("Username") %>' CommandName="Unblock">
									</asp:LinkButton>
								</li>
							</ul>
						</td>
		    		</tr>
		    	</table>
			</ItemTemplate>
			<SeparatorTemplate>
				<div class="line"></div>
			</SeparatorTemplate>
			</asp:DataList>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
		</ContentTemplate>		
		</asp:UpdatePanel>
		<div style="height:100px;position:static;">
            <components:BannerView ID="bvBlockedUsersRightBottom" runat="server" Key="BlockedUsersRightBottom">
            </components:BannerView>
        </div>
	</div>
</asp:Content>
