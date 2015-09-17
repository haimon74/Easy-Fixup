<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ApproveAudioUploads.aspx.cs" Inherits="ezFixUp.Admin.ApproveAudioUploads" %>

<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="DeleteButton" Src="DeleteVideoButton/DeleteButton.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminHeader" Src="AdminHeader.ascx" %>
<%@ Register TagPrefix="uc1" TagName="AdminMenu" Src="AdminMenu.ascx" %>
<%@ Register TagPrefix="uc1" TagName="MessageBox" Src="MessageBox.ascx" %>
<%@ Import Namespace="ezFixUp.Classes" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html>
<head>
    <title>
        <%= Lang.TransA("Approve Audio Uploads") %>
    </title>
    <meta name="GENERATOR" content="Microsoft Visual Studio .NET 7.1">
    <meta name="CODE_LANGUAGE" content="C#">
    <meta name="vs_defaultClientScript" content="JavaScript">
    <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5">
    <link rel="stylesheet" type="text/css" href="images/style.css" media="all">
</head>
<body>
    <form id="Form1" method="post" runat="server">
    <uc1:AdminHeader ID="AdminHeader1" runat="server"></uc1:AdminHeader>
    <div id="layout">
        <div id="crupms">
            <img src="images/i_avpproveaudio.jpg">
            <div id="crumpstxt">
                <%= Lang.TransA("Approve Audio Uploads") %>
            </div>
        </div>
        <div id="sidebar">
            <uc1:AdminMenu ID="AdminMenu1" runat="server"></uc1:AdminMenu>
        </div>
        <div id="main">
            <div id="content-head">
                <div class="ch-head">
                    <%= Lang.TransA("Approve Audio Uploads") %>
                </div>
                <div class="ch-description">
                    <%= Lang.TransA("Use this section to approve or reject pending audio uploads...") %>
                </div>
            </div>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <uc1:MessageBox ID="MessageBox" runat="server"></uc1:MessageBox>
                    <table width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td align="right">
                                <asp:Panel ID="pnlAudioPerPage" runat="server" CssClass="perpage">
                                    <%= Lang.TransA("Audio uploads per page") %>:
                                    <asp:DropDownList ID="ddAudioPerPage" CssClass="pages" Font-Name="Verdana" Font-Size="9px"
                                        runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="ddAudioPerPage_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </asp:Panel>
                                <div class="separator06">
                                </div>
                            </td>
                        </tr>
                    </table>
                    <asp:DataGrid ID="dgPendingAudio" runat="server" Width="100%" PageSize="2" AllowPaging="True"
                        AutoGenerateColumns="False" CssClass="btable1" CellPadding="0" CellSpacing="0"
                        BorderWidth="0" GridLines="None" 
                        onitemcommand="dgPendingAudio_ItemCommand" 
                        onitemcreated="dgPendingAudio_ItemCreated" 
                        onpageindexchanged="dgPendingAudio_PageIndexChanged">
                        <AlternatingItemStyle CssClass="btable2"></AlternatingItemStyle>
                        <HeaderStyle></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3" Wrap="False" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <a target="_blank" href="<%= Config.Urls.Home%>/ShowUser.aspx?uid=<%# Eval("Username")%>">
                                        <%# Eval("Username")%></a>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle CssClass="table_header2c" Wrap="False"></HeaderStyle>
                                <ItemStyle CssClass="table_cell3c" Width="90%"></ItemStyle>
                                <ItemTemplate>
                                	<table cellpadding="0" cellspacing="0" width="90%">
                                		<tr>
                                			<td>
                                			
                                    <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,40,0"
                                        width="325" height="262" id="flvplayer">
                                        <param name="movie" value="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf">
                                        <param name="quality" value="high">
                                        <param name="bgcolor" value="#FFFFFF">
                                        <param name="wmode" value="transparent">
                                        <param name="allowfullscreen" value="true">
                                        <param name="flashvars" value="width=325&height=262&file=<%# Eval("AudioUrl") %>" />
                                        <embed src="<%= HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/') %>/aspnet_client/FlvMediaPlayer/mediaplayer.swf"
                                            quality="high" wmode="transparent" bgcolor="#FFFFFF" width="325" height="262"
                                            name="flvplayer" align="" type="application/x-shockwave-flash" allowfullscreen="true"
                                            pluginspage="https://www.macromedia.com/go/getflashplayer" flashvars="width=325&height=262&file=<%# Eval("AudioUrl") %>"></embed></object>

                                			</td>
                                			<td>
                                    <asp:Button ID="btnApprove" Text='<%# Lang.TransA("Approve") %>' CommandName="approve"
                                        CommandArgument='<%# Eval("Id") %>' Enabled='<%# HasWriteAccess %>' runat="server">
                                    </asp:Button>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnReject" Text='<%# Lang.TransA("Reject") %>' CommandName="reject"
                                        CommandArgument='<%# Eval("Id") %>' Enabled='<%# HasWriteAccess %>' runat="server">
                                    </asp:Button>

                                			</td>
                                		</tr>
                                	</table>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                        <PagerStyle CssClass="table_cell" Font-Name="Verdana" Font-Size="11px" HorizontalAlign="center"
                            Mode="NumericPages"></PagerStyle>
                    </asp:DataGrid>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
