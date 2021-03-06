﻿<%@ Import Namespace="ezFixUp" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewGroupPhotos.ascx.cs"
    Inherits="ezFixUp.Components.Groups.ViewGroupPhotos" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="~/Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="~/Components/LargeBoxStart.ascx" %>

<uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
<asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="False"></asp:Label>
    <div id="divSlideshowLink" class="modes" runat="server">
        [&nbsp;<asp:LinkButton ID="lnkRunSlideshow" runat="server" OnClick="lnkRunSlideshow_Click"></asp:LinkButton>&nbsp;]
    </div>
    <div class="clear">
    </div>
	<div class="separator"></div>
    <div id="divSlideshow" class="center" runat="server" visible="false">
        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
            width="<%= Config.Photos.PhotoMaxWidth %>" height="<%= Config.Photos.PhotoMaxHeight %>"
            id="flvplayer">
            <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/ImageRotator/imagerotator.swf">
            <param name="quality" value="high">
            <param name="bgcolor" value="#FFFFFF">
            <param name="wmode" value="transparent">
            <param name="allowfullscreen" value="true">
            <param name="menu" value="false">
            <param name="flashvars" value="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Groups/<%= "GroupPhotoData.ashx?gid=" + GroupID %>&shuffle=false" />
            <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/ImageRotator/imagerotator.swf"
                quality="high" wmode="transparent" bgcolor="#FFFFFF" width="<%= Config.Photos.PhotoMaxWidth %>"
                height="<%= Config.Photos.PhotoMaxHeight %>" name="flvplayer" align="" type="application/x-shockwave-flash"
                allowfullscreen="true" menu="false" pluginspage="https://www.macromedia.com/go/getflashplayer"
                flashvars="width=<%= Config.Photos.PhotoMaxWidth %>&height=<%= Config.Photos.PhotoMaxHeight %>&file=Components/Groups/<%= "GroupPhotoData.ashx?gid=" + GroupID %>&shuffle=false"></embed></object>
    </div>
    <asp:MultiView ID="mvGroupPhotos" ActiveViewIndex="0" runat="server">
        <asp:View ID="viewGroupPhotosView" runat="server">
            <asp:DataList ID="dlGroupPhotos" CssClass="GroupPhotos" runat="server" RepeatColumns="3" RepeatLayout="Table"
                RepeatDirection="Horizontal" GridLines="None" BorderWidth="0" Width="100%" OnItemCommand="dlGroupPhotos_ItemCommand"
                CellPadding="0" OnItemDataBound="dlGroupPhotos_ItemDataBound">
                <ItemStyle Width="30%" VerticalAlign="Top" CssClass="BoxWrapStyle" />
                <ItemTemplate>
                        <a href='GroupImage.ashx?gpid=<%# Eval("GroupPhotoID") %>&diskCache=1' title='<%# Eval("Username") %><br><%# Eval("Name") %><br><%# Eval("Description") %><br><%# Eval("Date") %>'>
                            <img class="photoframe" src='GroupImage.ashx?gpid=<%# Eval("GroupPhotoID") %>&width=200&height=90&diskCache=1' alt="" />
                        </a>
                        <div><b><%# Eval("Name") %></b></div>
                            <%# Eval("Date") %>&nbsp; <a href='<%# UrlRewrite.CreateShowUserUrl((string) Eval("Username"))%>'>
                                <%# Eval("Username") %></a>
                        <ul id="ulEditControls" runat="server">
                            <li>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("GroupPhotoID") %>'></asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("GroupPhotoID") %>'></asp:LinkButton>
                            </li>
                        </ul>
                    </ItemTemplate>
            </asp:DataList>
            <asp:Panel ID="pnlPaginator" runat="server">
                <div class="separator"></div>
                <div class="endbox">
                    <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click"></asp:LinkButton>
                    <asp:Label ID="lblPager" runat="server"></asp:Label>
                    <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click"></asp:LinkButton>
                    <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click"></asp:LinkButton>
                </div>
            </asp:Panel>
            <div class="buttons">
                <asp:Button ID="btnUploadPhoto" runat="server" OnClick="btnUploadPhoto_Click" />
            </div>
        </asp:View>
        <asp:View ID="viewDeleteOptions" runat="server">
            <%= Lang.Trans("What do you want to do?") %><br />
            <asp:RadioButtonList ID="rbList" runat="server" />
            <asp:CheckBox ID="cbKickMember" runat="server" />
            <asp:CheckBox ID="cbBanMember" runat="server" />
            <asp:DropDownList ID="ddBanPeriod" runat="server">
            </asp:DropDownList>
            <br />
            <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />
            <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" />
        </asp:View>
        <asp:View ID="viewEditGroupPhoto" runat="server">
            <div class="wrap-sized">
                <br />
                <div align="center">
                    <img class="photoframe" src='GroupImage.ashx?gpid=<%= GroupPhotoID %>&width=450&height=450&diskCache=1' />
                </div>
                <br />
                <%= Lang.Trans("Name") %><br />
                <asp:TextBox ID="txtName" CssClass="titletextbox" runat="server"></asp:TextBox>
                <div class="separator">
                </div>
                <%= Lang.Trans("Description") %><br />
                <asp:TextBox ID="txtDescription" CssClass="multilinetextbox" runat="server" TextMode="MultiLine"
                    Rows="5" Columns="50"></asp:TextBox>
                <div class="separator">
                </div>
                <div class="buttons">
                    <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" />
                    <asp:Button ID="btnCancelUpdate" runat="server" OnClick="btnCancelUpdate_Click" />
                </div>
            </div>
        </asp:View>
    </asp:MultiView>
<uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
