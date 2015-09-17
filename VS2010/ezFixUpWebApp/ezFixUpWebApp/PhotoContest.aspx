<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="True" Codebehind="PhotoContest.aspx.cs"
    Inherits="ezFixUp.Classes.PhotoContestPage" %>

<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Data" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register src="Components/HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
        <uc1:SmallBoxStart ID="SmallBoxStart1" runat="server"></uc1:SmallBoxStart>
        <asp:UpdatePanel ID="UpdatePanelLastRanked" runat="server">
            <ContentTemplate>
                <div id="divNoRankedMessage" class="center lineh" runat="server">
                    <%= Lang.Trans("Begin voting to compare your choices with the community!") %><div class="separator6">
                    </div>
                </div>
                <div id="divLastRankedMessage" class="center overflow" visible="false" runat="server">
                    <div align="center">
                        <table cellpadding="0" cellspacing="0" width="90%">
                            <tr>
                                <td class="center" align="center">
                                    <div class="photoframe2s center mauto" style="width: 50px; height: 50px;">
                                        <table style="width: 50px; height: 50px;" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgLastLeft" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td valign="middle" class="center">
                                    &nbsp;<%= Lang.Trans("VS") %>&nbsp;</td>
                                <td class="center" align="center">
                                    <div class="photoframe2 center mauto" style="width: 50px; height: 50px;">
                                        <table style="width: 50px; height: 50px" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="imgLastRight" runat="server" /></td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="center" height="30px">
                                	<div class="overflow center usernamefix">
	                                    <a id="linkLastLeft" class="user-name" runat="server" />
                                    </div>
                                </td>
                                <td height="30px">
                                </td>
                                <td class="center" height="30px">
                                	<div class="overflow center usernamefix">
                                    	<a id="linkLastRight" class="user-name" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="clear">
                    </div>
                    <div class="separator">
                    </div>
                    <div class="srvmsg" style="width:90%">
                        <asp:Label ID="lblVotersAgree" runat="server" /></div>
                    <div class="separator6">
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clear">
        </div>
        <asp:UpdatePanel ID="UpdatePanelFavouriteEntries" runat="server">
            <ContentTemplate>
                <div id="divPersonalFavs" visible="false" runat="server">
                	<h4 class="BoxSubHeading"><%= Lang.Trans("Your favourite entries") %></h4>
                    <div class="SideMenuBoxInfo">
                        <asp:DataList ID="dlFavouriteEntries" runat="server" RepeatDirection="Vertical" RepeatColumns="1"
                            CellPadding="4" CellSpacing="0" Width="100%" ShowFooter="False" ShowHeader="False">
                            <ItemStyle VerticalAlign="Middle"></ItemStyle>
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td width="30px" class="bold" align="center">
                                            <%# Eval("Rank") %>&nbsp;</td>
                                        <td width="80px" height="60px" class="photoframe2 center">
                                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 80, 60, null, false, true, false) %></td>
                                        <td>
                                            <div class="nameover overflow">
                                                &nbsp;<a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>' target="_blank"><%# Eval("Username") %></a></div>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:DataList>
                    </div>
                    <div class="center">
                        <asp:LinkButton ID="lnkViewAllFavourites" runat="server" OnClick="lnkViewAllFavourites_Click" /></div>
                    <div class="center">
                        <asp:LinkButton ID="lnkViewTopEntries" Visible="False" runat="server" OnClick="lnkViewTopEntries_Click" /></div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lnkViewAllFavourites" />
                <asp:PostBackTrigger ControlID="lnkViewTopEntries" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="clear">
        </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
    </div>
    <div id="main">
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <uc2:HeaderLine ID="hlContestName" runat="server" />
        <div class="note">
            <asp:Label ID="lblContestDescription" runat="server" /></div>
        <div id="divPhotos" runat="server">
            <asp:UpdatePanel ID="UpdatePanelPhotos" runat="server">
                <ContentTemplate>
                    <div align="center">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="200px" class="center photoframe2">
                                    <a id="lnkLeftPhoto" rel="lightbox" target="_blank" runat="server">
                                        <asp:Image ID="imgLeftPhoto" runat="server" /></a></td>
                                <td class="bold">
                                    &nbsp;&nbsp;<%= Lang.Trans("VS") %>&nbsp;&nbsp;</td>
                                <td width="200px" class="center photoframe2">
                                    <a id="lnkRightPhoto" rel="lightbox" target="_blank" runat="server">
                                        <asp:Image ID="imgRightPhoto" runat="server" /></a></td>
                            </tr>
                            <tr>
                                <td class="center">
                                    <div class="separator6">
                                    </div>
                                    <asp:Button ID="btnPickLeft" runat="server" OnClick="btnPickLeft_Click" />&nbsp;
                                    <a id="linkLeftUsername" class="user-name" runat="server" />
                                </td>
                                <td>
                                </td>
                                <td class="center">
                                    <div class="separator6">
                                    </div>
                                    <asp:Button ID="btnPickRight" runat="server" OnClick="btnPickRight_Click" />&nbsp;
                                    <a id="linkRightUsername" class="user-name" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div id="divNotEnoughPhotos" class="srvmsg message" visible="false" runat="server">
            <%= Lang.Trans("There are not enough photos to start the contest!") %></div>
        <div id="divContestHasEnded" class="srvmsg message" visible="false" runat="server">
            <%= Lang.Trans("This contest has ended!") %></div>
        <div id="divAllFavourites" align="center" visible="false" runat="server">
            <%= String.Format(Lang.Trans("Your personal top {0} list!"), Config.Ratings.FavoriteEntriesCount) %><asp:DataList ID="dlAllFavourites" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                CellPadding="4" CellSpacing="0" Width="100%" ShowFooter="False" ShowHeader="False">
                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center">
                                <table style="width: 90px; height: 90px;" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="middle" class="photoframe2 center">
                                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, null, false, true, false) %></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="overflow">
                                <%# Eval("Rank") %>.&nbsp; <a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username")) %>" target="_blank">
                                    <%# Eval("Username") %></a></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <asp:Button ID="btnBackToPhotos" runat="server" OnClick="btnBackToPhotos_Click" />
        </div>
        <div id="divViewTopEntries" align="center" visible="false" runat="server">
            <%= String.Format(Lang.Trans("Top {0} entries!"), Config.Ratings.TopEntriesCount) %><asp:DataList ID="dlTopEntries" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                CellPadding="4" CellSpacing="0" Width="100%" ShowFooter="False" ShowHeader="False">
                <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center"></ItemStyle>
                <ItemTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="center">
                                <table style="width: 90px; height: 90px;" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="middle" class="photoframe2 center">
                                        <a href='<%# String.Format("Image.ashx?id={0}", Eval("PhotoId")) %>' target="_blank">
                                            <%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, null, false, true, false) %></a></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" class="overflow">
                                <%# Eval("Rank") %>.&nbsp; <a href="<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"))%>" target="_blank">
                                    <%# Eval("Username") %></a></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:DataList>
            <br />
            <asp:Button ID="btnBackToPhotos2" runat="server" OnClick="btnBackToPhotos_Click" />
        </div>
        <div class="buttons">
            <div class="line"></div>
            <div class="separator6"></div>
            <asp:Button ID="btnViewTopEntries" runat="server" OnClick="lnkViewTopEntries_Click" />
            <div class="separator"></div>
            <asp:Button ID="btnEnterContest" runat="server" OnClick="btnEnterContest_Click" />
            <div id="divContestRank" visible="false" runat="server">
                <asp:Label ID="lblCurrentRank" runat="server" />
                <div class="separator6"></div>
                <asp:Button ID="btnLeaveContest" runat="server" OnClick="btnLeaveContest_Click" />
            </div>
        </div>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
    </div>
</asp:Content>