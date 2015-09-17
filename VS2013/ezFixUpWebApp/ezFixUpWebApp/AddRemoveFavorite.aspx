<%@ Page language="C#" MasterPageFile="~/Site.Master" Codebehind="AddRemoveFavorite.aspx.cs" AutoEventWireup="True" Inherits="ezFixUp.AddRemoveFavorite" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>

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
            <components:BannerView ID="bvAddRemoveFavoriteLeftBottom" runat="server" Key="AddRemoveFavoriteLeftBottom">
            </components:BannerView>
        </div>
	</div>
	<div id="main">
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<div class="center">
			<br />
			<br />
			<div class="srvmsg">
				<asp:Label ID="lblMessage" Runat="server"></asp:Label>
			</div>
			<br />
			<br />
		</div>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
		<div style="height:100px;position:static;">
            <components:BannerView ID="bvAddRemoveFavoriteRightBottom" runat="server" Key="AddRemoveFavoriteRightBottom">
            </components:BannerView>
        </div>
	</div>
</asp:Content>