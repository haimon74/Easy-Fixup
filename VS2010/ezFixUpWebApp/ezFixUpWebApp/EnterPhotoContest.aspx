<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="EnterPhotoContest.aspx.cs"
    Inherits="ezFixUp.EnterPhotoContestPage" %>

<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="grb" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
		<div class="SideMenuItem"><asp:linkbutton id="lnkBackToContest" CssClass="SideMenuLink" Runat="server" OnClick="lnkBackToContest_Click"></asp:linkbutton></div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
    </div>
    <div id="main">
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
		<div class="srvmsg error"><asp:Label ID="lblError" runat="server" EnableViewState="False"></asp:Label></div>
        <div class="text">
        <%= Lang.Trans("Entry in contest") %>:
        <b><asp:Label ID="lblContestName" runat="server" /></b>
        <br />
        <%= Lang.Trans("Contest starts on") %>:
		<asp:Label ID="lblStartDate" CssClass="date" runat="server" />
        <br />
        <%= Lang.Trans("Contest ends on") %>:
        <asp:Label ID="lblEndDate" CssClass="date" runat="server" />
        </div>
		<div class="clear"></div>
		<div class="separator6"></div>
        <%= Lang.Trans("Contest terms") %>
        :
        <asp:Label ID="lblContestTerms" runat="server" />
        <br />
        <div class="text">
            <%= Lang.Trans("Which photo do you want to submit to this contest?") %></div>
        <asp:DataList ID="dlPhotos" runat="server" RepeatDirection="Horizontal"
            RepeatColumns="4" CellPadding="0" CellSpacing="0" Width="100%" ShowFooter="False"
            ShowHeader="False">
            <ItemStyle VerticalAlign="bottom" />
            <ItemTemplate>
                <div class="center">
                    <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, false) %><br />
                    <asp:HiddenField ID="hidPhotoId" Value='<%# Eval("PhotoId") %>' runat="server" />
                    <grb:GroupRadioButton ID="rbPhoto" GroupName="photos" runat="server" />
                </div>
            </ItemTemplate>
        </asp:DataList>
        <div class="clear">
        </div>
        <div class="buttons">
        <asp:Button ID="btnEnterContest" runat="server" OnClick="btnEnterContest_Click" />
        </div>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
    </div>
</asp:Content>
