<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="ReviewNewPhotos.aspx.cs" Inherits="AspNetDating.ReviewNewPhotos"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="~/Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="~/Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar"><uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
		<div class="center">
		<%= Lang.Trans("Moderation points") %>:<b>
		<%= CurrentUserSession.ModerationScores %></b>
        </div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
	    <uc1:smallboxstart id="SmallBoxStart2" CssClass="SideMenuBoxX" runat="server"></uc1:smallboxstart>
	        <components:ContentView ID="cvPhotoModerationRules" Key="PhotoModerationRules" runat="server">
            </components:ContentView>	
            <div class="clear"></div>
		<uc1:smallboxend id="SmallBoxEnd2" runat="server"></uc1:smallboxend>		
    </div>
    <div id="main">
        <asp:UpdatePanel ID="UpdatePanelPhotoDetails" runat="server">
        	<ContentTemplate>
	            <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
	            <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
	            <div class="center">	
		            <asp:Image ID="imgPhoto" CssClass="photoframe" runat="server" />
		            <div id="pnlPhotoName" runat="server">
		                <%= Lang.Trans("Name") %>:
		                <asp:Label ID="lblPhotoName" runat="server"></asp:Label>
		            </div>
		            <div id="pnlPhotoDescription" runat="server">
		                <%= Lang.Trans("Description") %>:
		                <asp:Label ID="lblPhotoDescription" runat="server"></asp:Label>
		            </div>
		            <div id="pnlUsername" runat="server">
		                <%= Lang.Trans("Username") %>:
		                <a id="lnkUsername" runat="server"></a>
		            </div>
		            <div class="separator"></div>
		            <div class="buttons">
			            <asp:Button ID="btnApprove" runat="server" onclick="btnApprove_Click" />
			            <asp:Button ID="btnReject" runat="server" onclick="btnReject_Click" />
			            <asp:Button ID="btnPass" runat="server" onclick="btnPass_Click" />
		            </div>
	            </div>
            </ContentTemplate>
        </asp:UpdatePanel>        
    	<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>
