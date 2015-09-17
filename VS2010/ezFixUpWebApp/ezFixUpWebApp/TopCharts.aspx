<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="TopCharts.aspx.cs" Inherits="ezFixUp.TopCharts"%>

<%@ Register Src="Components/TopUsers.ascx" TagName="TopUsers" TagPrefix="uc1" %>
<%@ Register Src="Components/TopPhotos.ascx" TagName="TopPhotos" TagPrefix="uc1" %>
<%@ Register Src="Components/TopModerators.ascx" TagName="TopModerators" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
<div class="WideBox">
	<div class="WideBoxTop">
        <div id="pnlButtons" runat="server" class="topchartswrap">
            <asp:Button ID="btnShowTopUsers" runat="server" onclick="btnShowTopUsers_Click" />
            <asp:Button ID="btnShowTopPhotos" runat="server" onclick="btnShowTopPhotos_Click" />
            <asp:Button ID="btnShowTopModerators" runat="server" onclick="btnShowModerators_Click" />
        </div>    
    </div>
    <div class="WideBoxContent">
        <asp:MultiView ID="mvTopCharts" runat="server">
            <asp:View ID="viewTopUsers" runat="server">
                <uc1:TopUsers id="TopUsersCtrl" runat="server"></uc1:TopUsers>
            </asp:View>
            <asp:View ID="viewTopPhotos" runat="server">
                <uc1:TopPhotos id="TopPhotosCtrl" runat="server"></uc1:TopPhotos>
            </asp:View>
            <asp:View ID="viewTopModerators" runat="server">
                <uc1:TopModerators id="TopModeratorsCtrl" runat="server"></uc1:TopModerators>
            </asp:View>
        </asp:MultiView>
    </div>
</div>        
</asp:Content>
