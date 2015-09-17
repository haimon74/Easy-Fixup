<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" Codebehind="PhotoContests.aspx.cs"
    Inherits="ezFixUp.Classes.PhotoContestsPage" %>

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
		    <div class="SideMenuItem">
			    <asp:LinkButton CssClass="SideMenuLink" ID="lnkViewActiveContests" Runat="server" Enabled="False" OnClick="lnkViewActiveContests_Click" />
		    </div>
		    <div class="SideMenuItem">
			    <asp:LinkButton CssClass="SideMenuLink" ID="lnkViewPastContests" Runat="server" OnClick="lnkViewPastContests_Click" />
		    </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
    </div>
    <div id="main">
        <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
        <asp:Repeater ID="rptContests" runat="server">
            <ItemTemplate>
            <uc2:HeaderLine ID="hlName" Title='<%# Eval("Name")  %>' runat="server" />
	          	<table cellpadding="0" cellspacing="0">
              		<tr>
            			<td align="center">
            				<table style="width:180px; height:90px;" cellspacing="0" cellpadding="0">
								<tr>
									<td valign="middle" class="photoframe2 center">
									<%# ImageHandler.RenderImageTag((int)Eval("TopPhotoId1"), 90, 90, null, false, false)%></td>
									<td valign="middle" class="photoframe2 center">
									<%# ImageHandler.RenderImageTag((int)Eval("TopPhotoId2"), 90, 90, null, false, false)%></td>
									<td valign="middle" class="photoframe2 center">
									<%# ImageHandler.RenderImageTag((int)Eval("TopPhotoId3"), 90, 90, null, false, false)%></td>
								</tr>
							</table>
            			</td>
            			<td class="pleft5" valign="top">
            				<%# Eval("Description") %><br />
                			<%= Lang.Trans("Entries") %>:&nbsp;<b><%# Eval("Entries") %></b><br />
                			<div runat="server" visible='<%# !lnkViewActiveContests.Enabled %>'>
                			    [&nbsp;<a href="PhotoContest.aspx?cid=<%# Eval("ContestId") %>"><%= Lang.Trans("View Contest") %></a>&nbsp;]
                			</div>
                			<div runat="server" visible='<%# (int) Eval("Entries") > Config.Ratings.MinPhotosToStartContest %>'>
                			[&nbsp;<a href="PhotoContest.aspx?cid=<%# Eval("ContestId") %>&top=1"><%= Lang.Trans("View Top Entries") %></a>&nbsp;]
                			</div>
            			</td>
            		</tr>
            	</table>
            	<div class="separator"></div>
            </ItemTemplate>
            <SeparatorTemplate>
				<div class="line"></div>
            </SeparatorTemplate>
        </asp:Repeater>
        <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
    </div>
</asp:Content>
