<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowAd.aspx.cs" Inherits="AspNetDating.ShowAd" %>

<%@ Import namespace="AspNetDating.Classes"%>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>
<%@ Register TagPrefix="uc1" TagName="FlexButton" Src="Components/FlexButton.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
	    <uc1:smallboxstart id="SmallBoxStart1" runat="server"></uc1:smallboxstart>
	    <div class="SideMenuItem"><a href='Ads.aspx' class="SideMenuLink"><%= "All Classifieds".Translate() %></a></div>
	    <div class="SideMenuItem"><a ID="lnkBackToCategory" runat="server" class="SideMenuLink"></a></div>
	    <div class="SideMenuItem"><a href='Ads.aspx?uid=<%= PostedBy %>' class="SideMenuLink"><%= "User other listings".Translate()%></a></div>
	    <div id="pnlEdit" runat="server" class="SideMenuItem"><a ID="lnkEdit" runat="server" class="SideMenuLink"></a></div>
	    <div id="pnlDelete" runat="server" class="SideMenuItem"><asp:LinkButton ID="lnkDelete" runat="server" CssClass="SideMenuLink" onclick="lnkDelete_Click"></asp:LinkButton></div>
		<div id="pnlMyAds" runat="server" class="SideMenuItem"><a href='Ads.aspx?show=ma' class="SideMenuLink"><%= "My Classifieds".Translate()%></a></div>
		<uc1:smallboxend id="SmallBoxEnd1" runat="server"></uc1:smallboxend>
		<uc1:SmallBoxStart ID="SmallBoxStart2" runat="server" />
			<div class="center">
			<div class="separator"></div>
		    <%= "Keyword".Translate() %>:
		    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
		    			<div class="separator"></div>
		    <uc1:FlexButton ID="fbSearch" runat="server" RenderAs="Button" OnClick="fbSearch_Click"/>
		    </div>
		<uc1:SmallBoxEnd ID="SmallBoxEnd2" runat="server" />
    </div>
    <div id="main">
        <asp:UpdatePanel ID="UpdatePanelPostAd" runat="server">
            <ContentTemplate>
                <uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
                <div class="srvmsg"><asp:Label ID="lblError" CssClass="error" runat="server" EnableViewState="False"></asp:Label></div>
                <span class="label"><%= "Subject".Translate() %>:</span>
                <asp:Label ID="lblSubject" runat="server"></asp:Label>
                <br />
                <span class="label"><%= "Posted by".Translate() %>:</span>
                <a id="lnkPostedBy" runat="server" class="user-name">
                    <asp:Label ID="lblPostedBy" runat="server"></asp:Label>
                </a>
                <br />
                <span class="label"><%= "Date".Translate() %>:</span>
                <asp:Label ID="lblDate" runat="server"></asp:Label>
                <br />
                <span class="label"><%= "Location".Translate() %>:</span>
                <asp:Label ID="lblLocation" runat="server"></asp:Label>
                <br />
                <span class="label"><%= "Description".Translate() %>:</span>
                <asp:Label ID="lblDescription" runat="server"></asp:Label>
				<div class="separator"></div>
                <div class="center">
                <asp:LinkButton ID="lnkPhoto" runat="server" onclick="lnkPhoto_Click">
                    <asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
                </asp:LinkButton>                
                <br />
                <asp:Label ID="lblAdPhotoDescription" runat="server"></asp:Label>
                </div>
                <div class="separator"></div>
                <asp:DataList ID="dlPhotos" runat="server" SkinID="UserPhotos" RepeatDirection="Horizontal"
                    CellPadding="0" CellSpacing="0" Width="100%" ShowFooter="False" 
                    ShowHeader="False" onitemcommand="dlPhotos_ItemCommand">
                    <ItemTemplate>
                        <table class="thumbs-wrap" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" valign="middle">
                                    <asp:LinkButton ID="lnkShowPhoto" CommandName="ShowPhoto" CommandArgument='<%# Eval("ID") %>' runat="server">
									<%# AspNetDating.AdPhoto.RenderImageTag((int)Eval("ID"), 90, 90, "photoframe", true) %></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:DataList>
                <div id="pnlUserComments" runat="server">
                            <uc1:HeaderLine ID="hlUserComments" runat="server"></uc1:HeaderLine>
                            <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="commentswrap skincolor">
                                        <nobr>[<%# ((DateTime)Eval("Date")).ToShortDateString() %>]
							    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username"))%>' target="_blank" class="user-name skinlinkcolor" onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()">
									    <%# Eval("Username") %></a>:</nobr>
                                        <%# Eval("Comment") %>
                                        <asp:LinkButton ID="lnkDeleteComment" CssClass="blinks" CommandName="DeleteComment"
                                            CommandArgument='<%# Eval("ID") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
                                            Visible='<%# Eval("CanDelete") %>' runat="server"></asp:LinkButton>
                                    </div>
                                </ItemTemplate>
                                <SeparatorTemplate>
                                    <div class="clear">
                                    </div>
                                    <div class="separator6">
                                    </div>
                                </SeparatorTemplate>
                                <FooterTemplate>
                                </FooterTemplate>
                            </asp:Repeater>
                            <div id="divViewAllComments" runat="server" class="morelink">
                                <asp:LinkButton ID="lnkViewAllComments" runat="server" OnClick="lnkViewAllComments_Click"></asp:LinkButton>
                            </div>
                            <div id="spanAddNewComment" class="wrap-sized" runat="server">
                                <div class="separator">
                                </div>
                                <div id="divAddCommentLink" class="center">
                                    [ <a href="javascript: void(0)" onclick="document.getElementById('divAddCommentLink').style.display = 'none'; document.getElementById('divAddCommentBox').style.display = 'block';" class="skinlinkcolor">
                                        <%= Lang.Trans("Add Comment") %></a>]
                                </div>
                                <div id="divAddCommentBox">
                                    <asp:TextBox ID="txtNewComment" CssClass="multilinetextbox" MaxLength="200" runat="server"
                                        TextMode="MultiLine"></asp:TextBox><br />
                                    <div class="buttons">
                                        <asp:Button ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click">
                                        </asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                <uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>            
</asp:Content>
