<%@ Page Title="" Language="C#" MasterPageFile="ShowUser.Master" AutoEventWireup="true" CodeBehind="ShowUserError.aspx.cs" Inherits="ezFixUp.ShowUserError" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftContent" runat="server">
    <div id="pnlPhotoAlbums" runat="server" visible="false">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
            <asp:DataList ID="dlPhotoAlbums" runat="server" 
            onitemcommand="dlPhotoAlbums_ItemCommand">
                <ItemTemplate>
                    <div class="SideMenuItem">
                        <asp:LinkButton ID="lnkPhotoAlbum" runat="server" Text='<%# Eval("PhotoAlbumName") %>' CommandArgument='<%# Eval("PhotoAlbumID") %>' CommandName="ViewPhotoAlbum" CssClass="SideMenuLink skinlinkcolor skinactionlinkcolor"></asp:LinkButton>
                        (<%# Eval("NumberOfPhotos") %>)
                    </div>
                </ItemTemplate>
            </asp:DataList>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="cphInnerContent" runat="server">
		<uc1:LargeBoxStart id="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
			<div class="center">
				<div class="separator"></div>
				<div class="srvmsg"><asp:Label ID="lblError" CssClass="message" runat="server" EnableViewState="False"></asp:Label></div>			</div>
			<br/>
		<uc1:LargeBoxEnd id="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
</asp:Content>
