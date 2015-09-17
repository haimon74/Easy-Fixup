<%@ Page Language="c#" MasterPageFile="Site.Master" CodeBehind="ShowMessage.aspx.cs"
    AutoEventWireup="True" Inherits="AspNetDating.ShowMessage" %>

<%@ Import Namespace="AspNetDating.Classes" %>
<%@ MasterType TypeName="AspNetDating.Site" %>
<%@ Register Src="Components/ReportAbuse.ascx" TagName="ReportAbuse" TagPrefix="uc2" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="Components/LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="Components/LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SideMenuStart" Src="Components/SideMenuStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="SmallBoxEnd" Src="Components/SmallBoxEnd.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphContent" runat="server">
    <div id="sidebar">
        <uc1:SideMenuStart ID="SmallBoxStart1" runat="server"></uc1:SideMenuStart>
        <div id="fmt">
            <ul class="fmt">
                <li >
                    <asp:LinkButton CssClass="SideMenuLink" ID="lnkReply" runat="server" OnClick="LnkReplyClick" />
                </li>
                <li id="divTranslate"  visible="false" runat="server">
                    <a href="#" class="SideMenuLink" id="btnTranslate">
                        <%= "Translate Message".Translate() %></a>
                </li>
                <li id="pnlBlockUser" runat="server" >
                    <asp:LinkButton ID="lnkBlockUser" runat="server" CssClass="SideMenuLink" OnClick="LnkBlockUserClick" />
                </li>
                <li id="pnlUnblockUser" runat="server" >
                    <asp:LinkButton ID="lnkUnblockUser" runat="server" CssClass="SideMenuLink" OnClick="LnkUnblockUserClick" />
                </li>
                <li id="pnlGrantAccessToPrivatePhotos" runat="server" >
                    <asp:LinkButton ID="lnkGrantAccess" runat="server" CssClass="SideMenuLink" OnClick="LnkGrantAccessClick" />
                </li>
                <li id="pnlDenyAccessToPrivatePhotos" runat="server" >
                    <asp:LinkButton ID="lnkDenyAccess" runat="server" CssClass="SideMenuLink" OnClick="LnkDenyAccessClick" />
                </li>
                <li id="pnlReportAbuse" runat="server" >
                    <asp:LinkButton ID="lnkReportAbuse" runat="server" CssClass="SideMenuLink" OnClick="LnkReportAbuseClick" />
                </li>
                <li id="pnlDelete" runat="server" >
                    <asp:LinkButton ID="lnkDelete" runat="server" CssClass="SideMenuLink" OnClick="LnkDeleteClick" />
                </li>
                <li >
                    <asp:LinkButton CssClass="SideMenuLink" ID="lnkBack" runat="server" OnClick="LnkBackClick" />
                </li>
            </ul>
       </div>
        <uc1:SmallBoxEnd ID="SmallBoxEnd1" runat="server"></uc1:SmallBoxEnd>
        <div style="height:300px;position:static;">
            <components:BannerView ID="bvShowMessageLeftBottom" runat="server" Key="ShowMessageLeftBottom">
            </components:BannerView>
        </div>
    </div>
    <div id="main">
        <div style="background: transparent;">
            <uc1:LargeBoxStart ID="LargeBoxStart1" runat="server"></uc1:LargeBoxStart>
            <div id="container" >
                <uc2:ReportAbuse ID="ReportAbuse1" runat="server" />
                <table style="margin: 15px 15px 5px 15px; ">
                <tr>
                    <td style="width:47%;" >
                    <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td valign="top" width="60px">
                            <a id="lnkShowUserTo" runat="server">
                                <asp:Literal ID="ltrPhotoTo" runat="server"></asp:Literal></a>
                        </td>
                        <td valign="top">
                        <b>
                            <span class="label c026683" style="font-size: larger;">
                                <%= Lang.Trans("From") %>:</span>&nbsp;<asp:Label ID="lblFromUsername" runat="server" ForeColor="#5DA1B3"></asp:Label><br />
                            <span class="label  c026683" style="font-size: larger;">
                                <%= Lang.Trans("To") %>:</span>&nbsp;<asp:Label ID="lblToUsername" runat="server"  ForeColor="#5DA1B3"></asp:Label>
                                </b>
                        </td>
                    </tr>
                </table>
                        </td>
                    <td style="width:47%;" >
                <table id="aboutPanel" runat="server" cellpadding="0" cellspacing="0" border="0" visible="false">
                    <tr>
                        <td valign="top" width="60px">
                            <a id="lnkShowUserAbout" runat="server">
                                <asp:Literal ID="ltrPhotoAbout" runat="server"></asp:Literal></a>
                        </td>
                        <td valign="top">
                        <b>
                            <span class="label c026683" style="font-size: larger;">
                                <%= Lang.Trans("About") %>:</span>&nbsp;<asp:Label ID="lblAboutUsername" runat="server" ForeColor="#5DA1B3"></asp:Label><br />
                            <span class="label  c026683" style="font-size: larger;">
                                <%= Lang.Trans("Age") %>:</span>&nbsp;<asp:Label ID="lblAboutAge" runat="server"  ForeColor="#5DA1B3"></asp:Label>
                        </b>
                        </td>
                    </tr>
                </table>
                        </td>
                    </tr>
                    </table>
                <div class="separator">
                </div>
                <div class="info">
                    <asp:Label ID="lblMessageTime" runat="server"></asp:Label>
                    <asp:LinkButton ID="lbReplay2" runat="server" OnClick="LnkReplyClick" CssClass="medium zurbtn red" />
                </div>
                <div class="clear">
                </div>
                <b><asp:Label ID="lblSubject" runat="server"></asp:Label></b>
                <div class="separator">
                </div>
                <asp:Label ID="lblMessage" CssClass="translatable" runat="server"></asp:Label>
                <div class="separator">
                </div>
                <div id="pnlAttachedGifts" runat="server">
                <asp:DataList ID="dlAttachedGifts" runat="server" CellSpacing="5"  RepeatDirection="Horizontal"
                    HorizontalAlign="Justify" RepeatColumns="1" RepeatLayout="Table" ShowHeader="true">
                <HeaderTemplate>
                    <h3><%= "Attached Gifts".Translate() %></h3>
                </HeaderTemplate>
                <ItemTemplate>
                    <table width="100%"><tr>
                        <td>
                            <p><b><%# Eval("GiftName") %></b></p>
                            <p><%# Eval("GiftPhrase") %></p>
                        </td>
                        <td style="width:280px;height:280px;border:solid 1px purple;background:url(<%# Eval("GiftImageUrl") %>) no-repeat;"></td>
                    </tr></table>
                </ItemTemplate>
                </asp:DataList>
                </div>
                <div class="separator">
                </div>
                <div id="pnlPreviousMessages" visible="false" runat="server" class="text">
                    <b>
                        <%= Lang.Trans("Previous Messages") %></b><div class="line">
                        </div>
                    <div class="clear">
                    </div>
                    <div class="separator">
                    </div>
                    <asp:Repeater ID="rptPreviousMessages" runat="server">
                        <ItemTemplate>
                            <b>
                                <%# Eval("Username") %>:</b> <span class="translatable">
                                    <%# Eval("Message") %></span>
                        </ItemTemplate>
                        <SeparatorTemplate>
                            <br />
                            <div class="separator">
                            </div>
                        </SeparatorTemplate>
                    </asp:Repeater>
                </div>
                <br />
                <div style="height:150px;position:static;">
                    <components:BannerView ID="bvShowMessageRightBottom" runat="server" Key="ShowMessageRightBottom">
                    </components:BannerView>
                </div>
            </div>
            <uc1:LargeBoxEnd ID="LargeBoxEnd1" runat="server"></uc1:LargeBoxEnd>
        </div>
    </div>
</asp:Content>
