<%@ Import Namespace="ezFixUp" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Control Language="c#" 
            AutoEventWireup="True" 
            CodeBehind="ViewPhotos.ascx.cs"
            Inherits="ezFixUp.Components.Profile.ViewPhotos" 
            TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register Src="../ReportAbuse.ascx" TagName="ReportAbuse" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%--<%@OutputCache Duration="60" Shared="true" VaryByParam="*" VaryByCustom="browser"%>--%>
<uc1:LargeBoxStart ID="LargeBoxStart" runat="server"></uc1:LargeBoxStart>
<input type="hidden" id="hidUsername" runat="server" name="hidUsername" />
<%@ Register TagPrefix="components" Namespace="ezFixUp.Components" Assembly="ezFixUpWebApp" %>
    <div class="SectionContent">
        <div>
            <div id="divSlideshowLink" class="modes skincolor" runat="server">
                [&nbsp;<asp:LinkButton ID="lnkRunSlideshow" CssClass="skinlinkcolor" runat="server"
                    OnClick="lnkRunSlideshow_Click"></asp:LinkButton>&nbsp;]
            </div>
            <div class="clear">
            </div>
            <components:BannerView id="bvShowUserPhotosAbovePhoto" runat="server" Key="ShowUserPhotosAbovePhoto"></components:BannerView>
            <div class="separator">
            </div>
            <div id="divSlideshow" class="center" runat="server" visible="false">
                <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                    width="<%= Config.Photos.PhotoMaxWidth %>" height="<%= Config.Photos.PhotoMaxHeight %>"
                    id="flvplayer">
                    <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/aspnet_client/ImageRotator/imagerotator.swf"/>
                    <param name="quality" value="high"/>
                    <param name="bgcolor" value="#FFFFFF"/>
                    <param name="wmode" value="transparent"/>
                    <param name="allowfullscreen" value="true"/>
                    <param name="menu" value="false"/>
                    <param name="flashvars" value="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Profile/<%= "UserPhotoData.ashx?uid=" + User.Username %>&shuffle=false" />
                    <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd(Convert.ToChar("/")) %>/aspnet_client/ImageRotator/imagerotator.swf"
                        quality="high" wmode="transparent" bgcolor="#FFFFFF" width="<%= Config.Photos.PhotoMaxWidth %>"
                        height="<%= Config.Photos.PhotoMaxHeight %>" name="flvplayer" align="" type="application/x-shockwave-flash"
                        allowfullscreen="true" menu="false" pluginspage="https://www.macromedia.com/go/getflashplayer"
                        flashvars="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Profile/<%= "UserPhotoData.ashx?uid=" + User.Username %>&shuffle=false"></embed></object>
            </div>
            <div id="pnlPhotos" class="viewphotos" runat="server">
                <asp:UpdatePanel ID="UpdatePanelPhotos" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="srvmsg">
                            <asp:Label ID="lblError" CssClass="error" runat="server" /></div>
                        <div id="pnlPhotoAlbums" runat="server" Visible="false">
                            <%= "Album name".Translate() %>&nbsp;
                            <asp:DropDownList ID="ddPhotoAlbums" CssClass="dropdownlist" runat="server" AutoPostBack="true" onselectedindexchanged="ddPhotoAlbums_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="center">
                            <br />
                            <asp:LinkButton ID="lnkPhoto" runat="server" OnClick="lnkPhoto_Click">
                                <asp:Literal ID="ltrPhoto" runat="server"></asp:Literal>
                            </asp:LinkButton>
                            <div id="pnlUnlockPhotos" runat="server">
                                <components:ContentView ID="cvUnlockPhotos" Key="UnlockPhotos" runat="server">
        		                </components:ContentView>
                                <asp:Button ID="btnUnlockPhotos" runat="server" 
                                    onclick="btnUnlockPhotos_Click" />
                            </div>
                            <div id="divUsersInPhoto" visible="false" class="skincolor" runat="server" enableviewstate="false">
                                <%= "In this photo:".Translate() %>
                                <asp:Literal ID="ltrUsersInPhoto" runat="server" />
                            </div>
                            <br />
                            <components:BannerView id="bvShowUserPhotosUnderPhoto" runat="server" Key="ShowUserPhotosUnderPhoto"></components:BannerView>
                            <div id="pnlPhotoRating" runat="server">
                                <asp:Label ID="lblRating" CssClass="label skincolor skincolorlabel" runat="server"></asp:Label>
                                <asp:Label ID="lblRatingAverage" CssClass="value skincolor" runat="server"></asp:Label>
                                <asp:Panel ID="pnlRatePhoto" CssClass="down10" runat="server" Visible="False" HorizontalAlign="Center">
                                    <asp:Label ID="lblYourRating" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddRating" runat="server">
                                    </asp:DropDownList>
                                    <asp:Button ID="btnRatePhoto" runat="server"></asp:Button>
                                    <input id="hidcurrentPhotoID" type="hidden" runat="server" name="hidcurrentPhotoID" />
                                </asp:Panel>
                            </div>
                            <b>
                                <asp:Label ID="lblName" CssClass="down10" runat="server" EnableViewState="false" /></b>
                            <div class="clear">
                            </div>
                            <asp:Label ID="lblDescription" runat="server" EnableViewState="false" />
                            <div id="pnlReportAbuseLink" class="down10 skincolor" runat="server">
                                [
                                <asp:LinkButton ID="lnkReportAbuse" OnClick="lnkReportAbuse_Click" CssClass="skinlinkcolor" runat="server"></asp:LinkButton>
                                ]
                            </div>
                        </div>
                        <div id="pnlUserComments" runat="server">
                            <uc1:HeaderLine ID="hlUserComments" runat="server"></uc1:HeaderLine>
                            <asp:Repeater ID="rptComments" runat="server" OnItemCommand="rptComments_ItemCommand">
                                <HeaderTemplate>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="commentswrap skincolor">
                                        <nobr>[<%# ((DateTime)Eval("Date")).ToShortDateString() %>]
							    <a href='<%# UrlRewrite.CreateShowUserUrl((string)Eval("Username"))%>' target="_self" class="user-name skinlinkcolor" onmouseover="showUserPreview('<%# Eval("Username") %>')" onmouseout="hideUserPreview()">
									    <%# Eval("Username") %>
								    </a>:</nobr>
                                        <%# Eval("Comment") %>
                                        <asp:LinkButton ID="lnkDeleteComment" CssClass="blinks" CommandName="DeleteComment"
                                            CommandArgument='<%# Eval("ID") %>' Text='<%# "[ " + Lang.Trans("Remove") + " ]" %>'
                                            Visible='<%# Eval("CanDelete") %>' runat="server">
                                        </asp:LinkButton>
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
                                        <%= Lang.Trans("Add Comment") %></a> ]
                                </div>
                                <div id="divAddCommentBox">
                                    <asp:TextBox ID="txtNewComment" CssClass="multilinetextbox" MaxLength="200" runat="server"
                                        TextMode="MultiLine"></asp:TextBox><br>
                                    <div class="buttons">
                                        <asp:Button ID="btnSubmitNewComment" runat="server" OnClick="btnSubmitNewComment_Click">
                                        </asp:Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkRunSlideshow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="dlPhotos" EventName="ItemCommand" />
                    </Triggers>
                </asp:UpdatePanel>
                <br />
                <div class="line">
                </div>
                <asp:UpdatePanel ID="UpdatePanelThumbnails" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                    <ContentTemplate>
                        <asp:DataList ID="dlPhotos" runat="server" SkinID="UserPhotos" RepeatDirection="Horizontal"
                            CellPadding="0" CssClass="ViewPhotosThumbs" CellSpacing="0" Width="100%" ShowFooter="False" ShowHeader="False">
                            <ItemTemplate>
                            <itemstyle VerticalAlign="middle" align="center"></itemstyle>
                            	<asp:LinkButton ID="lnkShowPhoto" CommandName="ShowPhoto" CommandArgument='<%# Eval("PhotoId") %>' runat="server">
								<%# ImageHandler.RenderImageTag((int)Eval("PhotoId"), 90, 90, "photoframe", false, true, false) %></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </ContentTemplate>
                      <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="lnkRunSlideshow" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="ddPhotoAlbums" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <uc2:ReportAbuse ID="ReportAbuse1" Visible="false" runat="server" />
        </div>
    </div>
<uc1:LargeBoxEnd ID="LargeBoxEnd" runat="server"></uc1:LargeBoxEnd>

