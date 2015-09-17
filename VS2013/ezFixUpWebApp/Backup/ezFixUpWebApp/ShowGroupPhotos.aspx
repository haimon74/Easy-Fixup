<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowGroupPhotos.aspx.cs" Inherits="ezFixUp.ShowGroupPhotos" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="GroupMembers" Src="~/Components/Groups/GroupMembers.ascx" %>
<%@ Register TagPrefix="uc1" TagName="ViewGroupPhotos" Src="~/Components/Groups/ViewGroupPhotos.ascx" %>
<%@ Register TagPrefix="uc1" TagName="UploadGroupPhoto" Src="~/Components/Groups/UploadGroupPhoto.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar"><uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="SideMenuItem"><asp:linkbutton id="lnkGroupHome" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupHome_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem"><asp:linkbutton id="lnkGroupGallery" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupGallery_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupMembers" runat="server"><asp:linkbutton id="lnkGroupMembers" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupMembers_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlMessageBoard" runat="server"><asp:linkbutton id="lnkMessageBoard" Runat="server" CssClass="SideMenuLink" OnClick="lnkMessageBoard_Click" ></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlGroupEvents" runat="server"><asp:linkbutton id="lnkGroupEvents" Runat="server" CssClass="SideMenuLink" OnClick="lnkGroupEvents_Click"></asp:linkbutton>
		</div>
		<div class="SideMenuItem" id="pnlAjaxChat" runat="server"><asp:HyperLink id="lnkOpenAjaxChat" Runat="server" CssClass="SideMenuLink"></asp:HyperLink>
		</div>
        <div id="divAddThis" visible="false" runat="server" class="SideMenuItem BookmarkLink">
        </div>    
		<div class="navline" id="pnlNavLine"  runat="server"></div>
		<div class="SideMenuItem" id="pnlUploadPhoto" runat="server"><asp:linkbutton id="lnkUploadPhoto" Runat="server" CssClass="SideMenuLink" OnClick="lnkUploadPhoto_Click"></asp:linkbutton>
		</div>
		<div class="navline"></div>
		<div class="SideMenuItem"><asp:linkbutton id="lnkBrowseGroups" Runat="server" CssClass="SideMenuLink" OnClick="lnkBrowseGroups_Click"></asp:linkbutton>
		</div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
    </div>
    <div id="main">
        <asp:MultiView ID="mvGroup" runat="server">
            <asp:View ID="viewGroupPhotos" runat="server">
                <uc1:ViewGroupPhotos ID="ViewGroupPhotos1" runat="server" />
            </asp:View>
            <asp:View ID="viewUploadPhoto" runat="server">
                <uc1:UploadGroupPhoto ID="UploadGroupPhoto1" runat="server" />
            </asp:View>
            <asp:View ID="viewGroupMembers" runat="server">
                <uc1:GroupMembers ID="GroupMembers1" runat="server" />
            </asp:View>
        </asp:MultiView>
    </div>
</asp:Content>
