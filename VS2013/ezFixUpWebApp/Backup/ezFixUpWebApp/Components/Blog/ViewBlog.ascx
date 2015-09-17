<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AddPost" Src="AddPost.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewBlog.ascx.cs" Inherits="ezFixUp.Components.Blog.ViewBlogCtrl"
    TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"></uc1:LargeBoxStart>
<components:BannerView id="bvBlogRightTop" runat="server" Key="BlogRightTop"></components:BannerView>
<uc1:HeaderLine ID="hlBlog" runat="server"></uc1:HeaderLine>
<div class="SectionContent">
<div class="hide">
            <div id="divViewBlog" class="lineh skincolor" runat="server">
                <asp:Repeater ID="rptBlogPosts" runat="server">
                    <ItemTemplate>
                        <nobr>[<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %>]
							<asp:LinkButton ID="lnkViewPost" CssClass="skinlinkcolor" CommandName="ViewPost" CommandArgument='<%# Eval("Id") %>' Runat="server">
								<%# Eval("Title") %>
							</asp:LinkButton></nobr>
                        <%# Eval("Content") %>
                        <asp:LinkButton ID="lnkViewPost2" CssClass="skinlinkcolor" CommandName="ViewPost" CommandArgument='<%# Eval("Id") %>'
                            runat="server">
						[&nbsp;<%# Lang.Trans("more") %>&nbsp;]
                        </asp:LinkButton>
                        <div class="separator"></div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
    </div>
            <div id="divViewPost" runat="server" class="skincolor" visible="false">
                <asp:LinkButton ID="lnkBackToBlog2" CssClass="skinlinkcolor" runat="server" OnClick="lnkBackToBlog_Click"></asp:LinkButton><br>
                <div id="divManagePost" runat="server" visible="false">
                    [&nbsp;<asp:LinkButton ID="lnkEditPost" runat="server" OnClick="lnkEditPost_Click"></asp:LinkButton>&nbsp;|&nbsp;<asp:LinkButton
                        ID="lnkDeletePost" runat="server" OnClick="lnkDeletePost_Click"></asp:LinkButton>&nbsp;]
                    <br>
                    &nbsp;
                </div>
                <asp:Label ID="lblDate" runat="server"></asp:Label><br>
                <asp:Label ID="lblTitle" CssClass="bold" runat="server"></asp:Label><br>
                (<asp:Label CssClass="italic" ID="lblDirectLink" runat="server"></asp:Label>)<br>
                <br>
               	<asp:Label ID="lblContent" runat="server"></asp:Label>
                <br>
                <asp:LinkButton ID="lnkBackToBlog" CssClass="skinlinkcolor" runat="server" OnClick="lnkBackToBlog_Click"></asp:LinkButton><br>
                <br>
                <div id="pnlBlogComments" runat="server">
                <uc1:HeaderLine ID="hlUserComments" runat="server"></uc1:HeaderLine>
                <asp:Repeater ID="rptComments" runat="server">
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                    <div class="comments skincolor">
                        <nobr>[<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %>]
						<b><a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username")) %>' target=_blank class="user-name" onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()">
								<%# Eval("Username") %>
							</a>: </b></nobr>
                            <%# ((string)Eval("CommentText")).Length > 250 ?
					((string)Eval("CommentText")).Substring(0, 200) + "<span id=\"spanCommentsMore" + Eval("Id") + "\">... [ <a href=\"javascript: void 0;\" onclick=\"document.getElementById('spanCommentsFull" + Eval("Id") + "').style.display = 'inline';document.getElementById('spanCommentsMore" + Eval("Id") + "').style.display = 'none';\">" + Lang.Trans("more") + "</a> ]</span><span id=\"spanCommentsFull" + Eval("Id") + "\" style=\"display: none;\">" + ((string)Eval("CommentText")).Substring(200) + "</span>"  :
					Eval("CommentText") %>
                        <asp:LinkButton CssClass="blinks" ID="lnkDeleteComment" CommandName="DeleteComment"
                            CommandArgument='<%# Eval("Id") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
                            Visible='<%# Eval("CanDelete") %>' runat="server">
                        </asp:LinkButton>
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
                        <%= Lang.Trans("Add Comment") %>
                    </a>]
                    </div>
	        	    <div id="divAddCommentBox">
	                    <asp:TextBox ID="txtNewComment" runat="server" CssClass="multilinetextbox" TextMode="MultiLine"></asp:TextBox><br>
	                    <div class="buttons">
	                        <asp:Button ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click"></asp:Button>
	                    </div>
	 	           </div>
                </div>
                </div>
            </div>
            <uc1:AddPost ID="AddPost1" runat="server" Visible="False"></uc1:AddPost>
</div>    
<components:BannerView id="bvBlogRightBottom" runat="server" Key="BlogRightBottom"></components:BannerView>
<uc1:LargeBoxEnd runat="server" ID="Largeboxend1"></uc1:LargeBoxEnd>
