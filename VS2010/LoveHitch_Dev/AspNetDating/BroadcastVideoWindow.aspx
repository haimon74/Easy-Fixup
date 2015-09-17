<%@ Page Language="c#" CodeBehind="BroadcastVideoWindow.aspx.cs" AutoEventWireup="True"
    Inherits="AspNetDating.BroadcastVideoWindow" %>
<%@ Import namespace="AspNetDating"%>
<%@ Import Namespace="AspNetDating.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body id="ctl00_body" class="videopanelwrap" runat="server">
    <form id="form1" runat="server">
        <input id="hidVideoGuid" type="hidden" value='<%= VideoGuid %>' />
        <input id="hidCurrentUser" type="hidden" value='<%= ((PageBase)Page).CurrentUserSession.Username %>' />
        <div id="divUserPreview" style="display: none; position: absolute; z-index: 99999">
            <div id="divUserPreviewImage"></div>
            <div id="divUserPreviewDetails"></div>
        </div>
	    <asp:ScriptManager ID="ScriptManager" runat="server" />
	    <script language="javascript" type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (sender, args) { 
                newEventsTimer = setInterval("FetchNewEventsForVideoBroadcast()", 2000);
                FetchNewEventsForVideoBroadcast();
            });	    
	    </script>
	    <table class="broadcastvideotable" cellpadding="0" cellspacing="0" width="100%">
	    	<tr>
	    		<td valign="top" width="320">
				    <div id="divBroadcastFlash">
				        <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0"
				            width="320" height="240">
				            <param name="movie" value="BroadcastVideoResource.ashx?res=broadcast&br_width=320&br_height=240&br_guid=<%= VideoGuid %>&br_mode=audiovideo&br_add=<%= FlashServer %>">
				            <param name="quality" value="high">
				            <embed src="BroadcastVideoResource.ashx?res=broadcast&br_width=320&br_height=240&br_guid=<%= VideoGuid %>&br_mode=audiovideo&br_add=<%= FlashServer %>"
				                quality="high" pluginspage="https://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash"
				                width="320" height="240"></embed>
				        </object>
				    </div>
	    		</td>
	    		<td valign="top">
	    			<div class="videopanel">
			            <div id="userswatching" class="userswatching"></div>
			            <div id="videomessages" class="videomessages_window"></div>
			            <div id="videosendmsg" class="videosendmsg">
			                <input id="txtSendMessage" maxlength="100" type="text" value="" onkeydown="return KeyHandler(event)" />
			                <input id="btnSendMessage" type="button" value='<%= Lang.Trans("Send") %>' onclick="SendMessage()" />
			            </div>
			        </div>
	    		</td>
	    	</tr>
	    </table>
        <asp:UpdatePanel ID="UpdatePanelUsers" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer ID="TimerMessages" Interval="30000" runat="server" 
                    ontick="TimerMessages_Tick">
                </asp:Timer>
                <div class="BroadcastWatchersHead"><%= Lang.Trans("Broadcast Watchers")%></div>
                <asp:DataList ID="dlUsers" SkinID="BroadcastWatchers" CssClass="BroadcastWatchersContent" runat="server" onitemcommand="dlUsers_ItemCommand">
                    <ItemTemplate>
                        - <a href='<%# UrlRewrite.CreateShowUserUrl((string)DataBinder.GetDataItem(Container))%>' target="_blank" class="user-name" onmouseover="showUserPreview('<%# DataBinder.GetDataItem(Container) %>')" onmouseout="hideUserPreview()">
							<%# DataBinder.GetDataItem(Container)%></a>
							&nbsp;[ <asp:LinkButton ID="lnkBlock" Text='<%# Lang.Trans("Block") %>' runat="server" CommandName="Block" CommandArgument='<%# DataBinder.GetDataItem(Container) %>'></asp:LinkButton> ]
                    </ItemTemplate>
                    <SeparatorTemplate>
                        <div class="clear"></div>
                    </SeparatorTemplate>
                </asp:DataList>
            </ContentTemplate>
        </asp:UpdatePanel>	    
    </form>
</body>
</html>
