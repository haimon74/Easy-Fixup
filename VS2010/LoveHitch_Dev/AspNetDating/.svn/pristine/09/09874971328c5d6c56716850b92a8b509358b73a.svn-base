<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddRemoveFriend.aspx.cs" Inherits="AspNetDating.AddRemoveFriend" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
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
            <components:BannerView ID="bvAddRemoveFriendLeftBottom" runat="server" Key="AddRemoveFriendLeftBottom">
            </components:BannerView>
        </div>
	</div>
	<div id="main">
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
		<div class="center">
			<br />
			<br />
			<div id="srvmsg" class="srvmsg">
				<asp:Label ID="lblMessage" Runat="server"></asp:Label>
			</div>
			<br />
			<br />
		</div>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
		<div style="height:100px;position:static;">
            <components:BannerView ID="bvAddRemoveFriendRightBottom" runat="server" Key="AddRemoveFriendRightBottom">
            </components:BannerView>
        </div>
	</div>
</asp:Content>
