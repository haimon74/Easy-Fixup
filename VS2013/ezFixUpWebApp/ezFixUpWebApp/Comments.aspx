<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Comments.aspx.cs" Inherits="ezFixUp.Comments" %>
<%@ Import namespace="ezFixUp"%>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxStart" Src="Components/SmallBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="~/Components/HeaderLine.ascx" %>
<%@ Import namespace="ezFixUp.Classes"%>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
	<div id="sidebar">
		<div id="banner1"></div>
	</div>
	<div id="main">
		<uc1:largeboxstart id="LargeBoxStart1" runat="server"></uc1:largeboxstart>
        <asp:UpdatePanel ID="UpdatePanelComments" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div class="srvmsg">
			        <asp:Label ID="lblMessage" Runat="server"></asp:Label>
		        </div>
                <div id="pnlProfileComments" runat="server">
                    <uc1:HeaderLine ID="hlUserComments" runat="server"></uc1:HeaderLine>
                    <asp:Repeater ID="rptProfileComments" runat="server" 
                        onitemcommand="rptComments_ItemCommand" onitemcreated="rptComments_ItemCreated">
                        <HeaderTemplate>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="commentswrap">
                                <nobr>[<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %>]
					    <b>
					    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("FromUsername"))%>' target="_self" class="user-name" onmouseover="showUserPreview('<%# Eval("FromUsername") %>')" onmouseout="hideUserPreview()">
							    <%# Eval("FromUsername") %></a>: </b></nobr>
                                <%# Eval("CommentText") %>
                                <asp:LinkButton ID="lnkDeleteComment" CssClass="blinks" CommandName="DeleteComment"
                                    CommandArgument='<%# Eval("Id") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
                                    runat="server"></asp:LinkButton>
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
                        <asp:LinkButton ID="lnkViewAllComments" runat="server" 
                            onclick="lnkViewAllComments_Click"></asp:LinkButton>
                    </div>
                </div>
				<div class="separator"></div>
                <div id="pnlPicturesComments" runat="server">
                    <uc1:HeaderLine ID="hlPicturesComments" runat="server"></uc1:HeaderLine>
                    <asp:Repeater ID="rptPicturesComments" runat="server" 
                        onitemdatabound="rptPicturesComments_ItemDataBound" 
                        onitemcreated="rptPicturesComments_ItemCreated" 
                        onitemcommand="rptPicturesComments_ItemCommand">
                        <ItemTemplate>
	                        <table cellpadding="0" cellspacing="0" width="100%">
	                        	<tr>
	                        		<td width="100" align="center">
	                        			<%# ImageHandler.RenderImageTag((int)Eval("PhotoID"), 90, 90, "photoframe", false, true, false)%></td>	                        			
	                        		<td valign="top">
			                            <asp:Repeater ID="rptPhotoComments" runat="server" OnItemCommand="rptPhotoComments_ItemCommand" OnItemCreated="rptPhotoComments_ItemCreated">
			                                <ItemTemplate>
			                                    <div class="commentswrap">
			                                        <nobr>[<%# ((DateTime)Eval("DatePosted")).ToShortDateString() %>]
								                    <b>
								                    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("FromUsername"))%>' target="_self" class="user-name" onmouseover="showUserPreview('<%# Eval("FromUsername") %>')" onmouseout="hideUserPreview()">
										            <%# Eval("FromUsername") %></a>: </b></nobr>
			                                        <%# Eval("CommentText") %>
			                                        <asp:LinkButton ID="lnkDeleteComment" CssClass="blinks" CommandName="DeleteComment"
			                                            CommandArgument='<%# Eval("ID") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
			                                            runat="server"></asp:LinkButton>
			                                    </div>
			                                </ItemTemplate>
											<separatortemplate>
												<div class="separator"></div>
											</separatortemplate>
			                            </asp:Repeater>
	                        		</td>
	                        	</tr>
	                        </table>
	                        <div id="pnlViewAllPhotoComments" runat="server" class="morelink">
	                            <asp:LinkButton ID="lnkViewAllPhotoComments" runat="server" CommandName="ViewAllPhotoComments" CommandArgument='<%# Eval("PhotoID") %>'
	                            ></asp:LinkButton>
	                        </div>
                        </ItemTemplate>
						<separatortemplate>
							<div class="separator"></div>
						</separatortemplate>
                    </asp:Repeater>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
		<uc1:largeboxend id="LargeBoxEnd1" runat="server"></uc1:largeboxend>
	</div>
</asp:Content>
