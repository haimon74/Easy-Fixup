<%@ Page Title="" Language="C#" MasterPageFile="~/Mobile/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="ezFixUp.Mobile.Home" %>
<%@ Import Namespace="ezFixUp.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<h1 id="lblTitle" runat="server" />
    <div class="SeparatorLine"></div>   
    <div class="ContentWrap">     
    <ul class="InfoUlWrap">
        <li id="liWhoViewedMyProfile" runat="server">
        <%= Lang.Trans("Your profile has been viewed") %><b>
            <asp:Label ID="lblProfileViews" runat="server" /></b>&nbsp;<%= Lang.Trans("times") %>&nbsp;[
        <asp:LinkButton ID="lnkViewProfileViewers" runat="server" OnClick="lnkViewProfileViewers_Click" />
        ]
        <br />
        </li>
        <li id="pnlRating" runat="server">
            <%= Lang.Trans("Average rating") %>: <b>
                <asp:Label ID="lblRating" runat="server" /></b><br />
        </li>
        <li id="pnlVotes" runat="server">
            <%= Lang.Trans("Your votes") %>: <b>
                <asp:Label ID="lblVotes" runat="server" /></b> [&nbsp;<asp:LinkButton ID="lnkViewMutualVotes"
                    runat="server" OnClick="lnkViewMutualVotes_Click" />&nbsp;] </li>
        <li id="pnlNewUsers" runat="server">
            <asp:LinkButton ID="lnkNewUsers" runat="server" OnClick="lnkNewUsers_Click"></asp:LinkButton>
            <br />
        </li>
        <li id="pnlUsersOnline" runat="server">
            <asp:Label ID="lblUsersOnline" runat="server"></asp:Label>&nbsp;[
            <asp:LinkButton ID="lnkUsersOnline" runat="server" OnClick="lnkUsersOnline_Click"></asp:LinkButton>&nbsp;]</li>
        <li id="pnlNewMessages" runat="server">
            <asp:Label ID="lblNewMessages" runat="server"></asp:Label>&nbsp;[
            <asp:LinkButton ID="lnkNewMessages" runat="server" OnClick="lnkNewMessages_Click" />&nbsp;]</li>
        <li id="pnlStatusText" runat="server" visible="false">
            <asp:UpdatePanel ID="UpdatePanelStatusText" runat="server">
                <ContentTemplate>
                <div class="headerfix2">
                    <%= Lang.Trans("Your status")%>:
                    <div id="pnlViewStatusText" runat="server" style="display:inline">
                        <b><asp:Label ID="lblStatusText" runat="server"></asp:Label></b>
                        [&nbsp;<asp:LinkButton ID="lnkEditStatusText" runat="server" onclick="lnkEditStatusText_Click" />&nbsp;]
                    </div>
                    <div id="pnlEditStatusText" runat="server" visible="false" style="display:inline">
                        <asp:TextBox ID="txtStatusText" CssClass="SmallTextField" runat="server"></asp:TextBox>
                        [&nbsp;<asp:LinkButton ID="lnkUpdateStatusText" runat="server" 
                            onclick="lnkUpdateStatusText_Click"></asp:LinkButton>&nbsp;]
                    </div>
                </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </li>
    </ul>
    </div>
    <div class="SeparatorLine"></div>   
    <div ID="pnlLogout" class="logout SubheaderColor" align="center" Runat="server">
		<asp:LinkButton id="lnkLogout"  CssClass="hlinks SkinSubheaderLinkColor" runat="server" onclick="lnkLogout_Click"></asp:LinkButton>&nbsp;&nbsp;
	</div>
</asp:Content>
