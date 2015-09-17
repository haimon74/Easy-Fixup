<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RatePhotos.aspx.cs" Inherits="ezFixUp.RatePhotos" %>

<%@ Import Namespace="ezFixUp"%>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Import namespace="ezFixUp.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<asp:UpdatePanel ID="UpdatePanelRatePhotos" runat="server">
	        <ContentTemplate>
            <div id="sidebar">
	            <uc1:SmallBoxStart id="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
	            <div class="center lineh">
	                <components:ContentView ID="cvRatePhotos" Key="RatePhotos" runat="server"></components:ContentView>
	                <div id="pnlRatedPhoto" runat="server">
	                <div class="separator"></div>
	                    <%= ImageHandler.RenderImageTag(PrevPhotoId, 90, 90, "photoframe", false, true) %></div>
	                <asp:Panel ID="pnlRating" runat="server" Visible="false">
	                    <%= String.Format("Rating: {0} based on {1} votes".Translate(), Rating.ToString("0.00"), Votes)%><br />
	                    <%= "You rated".Translate() + ": " + CurrentVote %></asp:Panel>
        		</div>
	            <uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
	        </div>
	        <div id="main">
                <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
                <div class="UserFilter">
                <span ID="pnlGender" runat="server">
                    <asp:DropDownList ID="ddGender" CssClass="dropdownlist" runat="server" AutoPostBack="true" onselectedindexchanged="ddGender_SelectedIndexChanged"></asp:DropDownList>
                </span>
                <span id="pnlAge" runat="server">
                    <%= Lang.Trans("Age") %>
                    <asp:TextBox ID="txtFrom" runat="server" CssClass="smalltextbox" Size="2" AutoPostBack="true"
                            MaxLength="2" ontextchanged="txtFrom_TextChanged"></asp:TextBox>
                    <%= Lang.Trans("to") %>
                    <asp:TextBox ID="txtTo" runat="server" CssClass="smalltextbox"  Size="2" AutoPostBack="true"
                            MaxLength="2" ontextchanged="txtTo_TextChanged"></asp:TextBox>
                </span>
				</div>
                <div class="separator"></div>
                <div class="center">
                <asp:Panel ID="pnlVotes" runat="server">
                 <%= Lang.Trans("Rate with:") %><asp:Repeater ID="rptVotes" runat="server" onitemcommand="rptRating_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkVote" runat="server" CommandArgument='<%# Eval("Vote") %>'><%# Eval("Vote") %></asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
                </asp:Panel>
                <div class="separator"></div>
                <asp:Panel ID="pnlCurrentPhoto" runat="server">
                    <asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
                    <div class="clear"></div>
                    <asp:Label ID="lblName" runat="server"></asp:Label>
                    <div class="clear"></div>
                    <asp:Label ID="lblDescription" runat="server"></asp:Label>
                    <div class="clear"></div>
                    <a id="lnkUser" runat="server" target="_blank"></a>
                    <div class="clear"></div>
                    <asp:Label ID="lblAge" runat="server"></asp:Label>
                    <div class="clear"></div>
                    <asp:Label ID="lblLocation" runat="server"></asp:Label>
                </asp:Panel>
                </div>
                <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
                <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	        </div>
	    </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
