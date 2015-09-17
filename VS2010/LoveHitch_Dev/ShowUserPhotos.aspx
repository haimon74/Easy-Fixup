<%@ Page Language="C#" MasterPageFile="~/ShowUser.Master" AutoEventWireup="true" CodeBehind="ShowUserPhotos.aspx.cs" Inherits="AspNetDating.ShowUserPhotos" %>
<%@ Import Namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="ViewPhotos" Src="Components/Profile/ViewPhotos.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphLeftContent" runat="server">
    <div id="pnlPhotoAlbums" runat="server">
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
    <uc1:ViewPhotos id="ViewPhotosCtrl" runat="server" />
</asp:Content>