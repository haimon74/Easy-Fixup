<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroupTopics.aspx.cs" Inherits="AspNetDating.ShowGroupTopics" %>
<%@ Register TagPrefix="uc1" TagName="ViewTopics" Src="~/Components/Groups/ViewTopics.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="EditPost" Src="~/Components/Groups/EditPost.ascx"%>
<%@ Register TagPrefix="uc1" TagName="ViewPosts" Src="~/Components/Groups/ViewPosts.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SearchTopicResults" Src="~/Components/Groups/SearchTopicResults.ascx" %>
<%@ Import namespace="AspNetDating.Classes"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar"><uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
        <div class="SideMenuItem"><asp:linkbutton id="lnkGroupHome" CssClass="SideMenuLink" Runat="server" OnClick="lnkGroupHome_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupGallery" runat="server"><asp:linkbutton id="lnkGroupGallery" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupGallery_Click" ></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupMembers" runat="server"><asp:linkbutton id="lnkGroupMembers" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupMembers_Click" ></asp:linkbutton>
		</div>
        <div class="SideMenuItem" id="pnlMessageBoard" runat="server"><asp:linkbutton id="lnkMessageBoard" CssClass="SideMenuLink" Runat="server" OnClick="lnkMessageBoard_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupEvents" runat="server"><asp:linkbutton id="lnkGroupEvents" CssClass="SideMenuLink" Runat="server" OnClick="lnkGroupEvents_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlAjaxChat" runat="server"><asp:HyperLink id="lnkOpenAjaxChat" Runat="server" CssClass="SideMenuLink"></asp:HyperLink>
		</div>
		<div class="navline" id="pnlNavLine" runat="server"></div>
        <div class="SideMenuItem" id="pnlStartNewTopic" runat="server"><asp:linkbutton id="lnkStartNewTopic" CssClass="SideMenuLink" Runat="server" OnClick="lnkStartNewTopic_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlSubscribeForTopic" runat="server"><asp:linkbutton id="lnkSubscribeForTopic" CssClass="SideMenuLink" Runat="server" OnClick="lnkSubscribeForTopic_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlUnsubscribeFromTopic" runat="server"><asp:linkbutton id="lnkUnsubscribeFromTopic" CssClass="SideMenuLink" Runat="server" OnClick="lnkUnsubscribeFromTopic_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlAddNewPost" runat="server"><asp:linkbutton id="lnkAddNewPost" CssClass="SideMenuLink" Runat="server" OnClick="lnkAddNewPost_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlEditTopic" runat="server"><asp:linkbutton id="lnkEditTopic" CssClass="SideMenuLink" Runat="server" OnClick="lnkEditTopic_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlDeleteTopic" runat="server"><asp:linkbutton id="lnkDeleteTopic" CssClass="SideMenuLink" Runat="server" OnClick="lnkDeleteTopic_Click"></asp:linkbutton>
		</div>
        <div id="divAddThis" visible="false" runat="server" class="SideMenuItem BookmarkLink">
        </div>    
		<div class="navline"></div>
		<div class="SideMenuItem"><asp:linkbutton id="lnkBrowseGroups" Runat="server" CssClass="SideMenuLink" OnClick="lnkBrowseGroups_Click" ></asp:linkbutton>
		</div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <asp:MultiView ID="mvTopics" runat="server">
            <asp:View ID="viewMain" runat="server">
                <uc1:ViewTopics ID="ViewTopics1" runat="server" />
            </asp:View>
            <asp:View ID="viewStartNewTopic" runat="server">
                <uc1:EditPost ID="EditPost1" runat="server" />
            </asp:View>
            <asp:View ID="viewPosts" runat="server">
                <uc1:ViewPosts ID="ViewPosts1" runat="server" />
            </asp:View>
            <asp:View ID="viewSearchResults" runat="server">
			        <uc1:SearchTopicResults id="SearchTopicResults1" runat="server"></uc1:SearchTopicResults>
			</asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
