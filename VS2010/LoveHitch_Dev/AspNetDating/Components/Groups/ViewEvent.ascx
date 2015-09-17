<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewEvent.ascx.cs" Inherits="AspNetDating.Components.Groups.ViewEvent" %>
<%@ Import namespace="AspNetDating"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DatePicker" Src="~/Components/DatePicker.ascx" %>
<%@ Register Src="~/Components/DatePicker.ascx" TagName="DatePicker" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" CssClass="StandardBoxX" runat="server"></uc1:LargeBoxStart>
<div class="srvmsg"><asp:Label ID="Label1" CssClass="error" runat="server" EnableViewState="false"></asp:Label></div>
<h3 class="SectionHeading"><asp:Label ID="lblTitle" runat="server"></asp:Label></h3>
<table cellpadding="0" cellspacing="0">
	<tr>
		<td valign="top">
			<img class="photoframe" src='GroupEventImage.ashx?id=<%= EventID %>&width=120&height=120&diskCache=1' alt=""/>
		</td>
		<td valign="top" class="pleft5">
			<b><%= Lang.Trans("Event date:") %></b>&nbsp;
			<asp:Label ID="lblDate" runat="server"></asp:Label>
			<div class="clear"></div>
			<b><%= Lang.Trans("Created by:") %></b>&nbsp;
			<a href='ShowUser.aspx?uid=<%= CreatedBy %>'><%= CreatedBy %></a>
			<div class="clear"></div>
			<b><%= Lang.Trans("Location:") %></b>&nbsp;
			<asp:Label ID="lblLocation" runat="server"></asp:Label>
			<asp:Image ID="imgGoogleMap" runat="server" Visible="false" />
		</td>
	</tr>
</table>
<div class="clear"></div>
<div class="separator6"></div>
<asp:Label ID="lblDescription" runat="server"></asp:Label>
<div class="separator"></div>
<div id="divUserFriends" runat="server">
    <uc1:HeaderLine ID="hlUserFriends" runat="server"></uc1:HeaderLine>
    <asp:DataList ID="dlUserFriends" CssClass="UsersBoxContent" runat="server" RepeatDirection="Horizontal"
        RepeatLayout="Table" CellSpacing="3" GridLines="None" SkinID="Friends" Width="100%">
        <ItemTemplate>
            <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
                <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'><%# Eval("Username") %></a>
        </ItemTemplate>
    </asp:DataList>
</div>
<asp:UpdatePanel ID="UpdatePanelAttenders" runat="server">
    <ContentTemplate>
        <div id="pnlAttenders" runat="server">
            <uc1:HeaderLine ID="hlAttenders" runat="server"></uc1:HeaderLine>
            <asp:DataList ID="dlAttenders" CssClass="UsersBoxContent" runat="server" RepeatDirection="Horizontal"
                RepeatLayout="Table" CellSpacing="3" GridLines="None" SkinID="Friends" Width="100%">
                <ItemTemplate>
                    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                        <%# ImageHandler.RenderImageTag((int)Eval("ImageId"), 50, 50, "photoframe", false, true, true) %></a><div class="clear"></div>
                    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>'>
                        <%# Eval("Username") %></a>
                </ItemTemplate>
            </asp:DataList>
        </div>
        <div class="center">
        	<asp:Label ID="lblAttending" runat="server"></asp:Label>
	        <div class="buttons">
		        <asp:Button ID="btnJoinEvent" runat="server" onclick="btnJoinEvent_Click" />
		        <asp:Button ID="btnLeaveEvent" runat="server" onclick="btnLeaveEvent_Click" />
	        </div>
        	<div class="separator"></div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="upnlGroupEventsComments" runat="server">
    <ContentTemplate>
    <div id="pnlBlogComments" runat="server">
        <uc1:HeaderLine ID="hlGroupEventsComments" runat="server"></uc1:HeaderLine>
        <asp:Repeater ID="rptComments" runat="server" 
            onitemcommand="rptComments_ItemCommand" 
            onitemcreated="rptComments_ItemCreated">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
            <div class="commentswrap">
                <nobr>[<%# ((DateTime)Eval("Date")).ToShortDateString() %>]
		        <b><a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' target=_blank class="user-name">
				        <%# Eval("Username") %></a>: </b></nobr>
                    <%# Eval("Comment") %>
                            <asp:LinkButton ID="lnkDeleteComment" CssClass="blinks" CommandName="DeleteComment"
                                CommandArgument='<%# Eval("Id") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
                                Visible='<%# Eval("CanDelete") %>' runat="server"></asp:LinkButton>
            </div>
            </ItemTemplate>
            <SeparatorTemplate>
    	        <div class="clear"></div>
    	        <div class="separator6"></div>
            </SeparatorTemplate>
            <FooterTemplate>
            </FooterTemplate>
	    </asp:Repeater>
        <div id="spanAddNewComment" class="wrap-sized" runat="server">
            <div class="separator"></div>
            <div id="divAddCommentLink" class="center">
            [ <a onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';"
                href="javascript: void(0)">
                <%= Lang.Trans("Add Comment") %></a>]
            </div>
            <div id="divAddCommentBox">
                <asp:TextBox ID="txtNewComment" runat="server" CssClass="multilinetextbox" TextMode="MultiLine"></asp:TextBox><br>
                <div class="buttons">
                    <asp:Button ID="btnSubmitNewComment" runat="server" 
                        onclick="btnSubmitNewComment_Click"></asp:Button>
                </div>
           </div>
        </div>
    </div>
    </ContentTemplate>
</asp:UpdatePanel>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>