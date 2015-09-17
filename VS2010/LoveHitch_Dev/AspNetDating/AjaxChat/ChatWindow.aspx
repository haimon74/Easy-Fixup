<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ChatWindow.aspx.cs" Inherits="Estream.AjaxChat.ChatWindowPage" %>
<%@ Register TagPrefix="cc1" Namespace="Estream.AjaxChat" Assembly="AjaxChat" %>
<%@ Register Assembly="AjaxChat" Namespace="Estream.AjaxChat" TagPrefix="cc1" %>
<%@ Import Namespace="Estream.AjaxChat.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="https://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ajax Chat</title>
    <link rel="stylesheet" type="text/css" href="Images/style.css" media="all" />  
</head>
<body onload="InitializeChat(<%= chatRoomId %>); SetHeight();" onunload="CloseChat()"
    onresize="SetHeight();" onkeypress="__trapESC(event)" onclick="hideAllContextMenus(event)" scroll="no">
    <form id="form1" runat="server" style="height: 100%">
        <bgsound id="sound" />
        <span id="spanSound"></span>
        <table border="0" cellspacing="0" cellpadding="0" id="loading" style="position:absolute; z-index: 888888; visibility: hidden;">    
            <tr>
                <td class="loginbox-txt" style="padding: 30px;"><%= Lang.Trans("Loading...")%></td>
            </tr>
        </table>
        <table border="0" cellspacing="0" cellpadding="0" id="loginBox" style="position: absolute; z-index: 888888; visibility: hidden">
		    <tr>
			    <td colspan="2" class="loginbox-head"><%= Lang.Trans("Enter your nickname")%></td>
	        </tr>
	        <tr>
			    <td class="loginbox-label"><%= Lang.Trans("Nickname") %></td>
			    <td class="loginbox-txt">
			    		<input id="txtNickname" type="text" onkeypress="HandleEnterKeyForNickName(event)" />
			    </td>
	   	    </tr>
            <tr>
			    <td colspan="2">
			        <div id="avatars">
			            <asp:DataList ID="dlAvatars" CssClass="AvatarsTable" SkinId="Avatars" runat="server">
			                <ItemTemplate>
			                    <img class="avatar_unselected" src='<%# DataBinder.GetDataItem(Container) %>' onclick="Avatar_Click(this)" border=0 />
			                </ItemTemplate>
			            </asp:DataList>
			            <input id="hidSelectedAvatar" type="hidden" />
			        </div>
	   	        </td>
	   	    </tr>	   	    
	        <tr>
			    <td colspan="2" class="add-buttons">
			        <p id="loginerror"></p>
				    <input type="button" value='<%= Lang.Trans("LogIn") %>' onclick="LogIn_Click()" />
		        </td>
	        </tr>
	    </table>
	    <table border="0" cellspacing="0" cellpadding="0" id="passwordBox" style="position: absolute; z-index: 888888; visibility:hidden;">
		    <tr>
			    <td colspan="2" class="loginbox-head"><span id="passwordbox-headertext"></span></td>
	        </tr>
	        <tr>
			    <td class="loginbox-label"><%= Lang.Trans("Password") %></td>
			    <td class="loginbox-txt">
			        <input id="txtPassword" type="text" onkeypress="HandleEnterKeyForPassword(event)" />
			    </td>
	   	    </tr>
	        <tr>
			    <td colspan="2" class="add-buttons">
			        <p id="passworderror"></p>
				    <input type="button" value='<%= Lang.Trans("Submit") %>' onclick="SubmitPassword_Click()" />
		        </td>
	        </tr>	   	    	        	    
	    </table>	    
        <div id="contextMenus" style="position: absolute; z-index: 888888;"></div>
        <div id="emoticonsPanelDiv" style="position:absolute;z-index:888888;visibility:hidden;"></div>
        <div id="receiveVideosDiv" style="position: absolute; z-index: 888888;">
        </div>
        <div id="divBroadcastVideo" style="position: absolute; width: 215px; z-index: 999999; display: none;">
            <div id="divBroadcastVideoDragHandle" class="SendVideoTop" style="width: 100%" onmousedown="dragStart(event, 'divBroadcastVideo')">
                <div class="dragHandle">
                    <div class="dragtext"><%= Lang.Trans("Video Broadcast")%></div><div class="videoclose" onclick="broadcastVideo(false);">x</div></div>
            </div>
            <div id="divBroadcastVideoObject" class="SendVideoBtm"></div>
        </div>
        <asp:ScriptManager ID="scriptManager" runat="server">
            <Services>
                <asp:ServiceReference Path="ChatEngine.asmx" InlineScript="true" />
            </Services>
        </asp:ScriptManager>
        <div id="wrap">      
            <div id="header">
                <img alt="" src="Images/logo.png" onload="fixPng(this);" />
                <div id="header_right">
                    <div id="righthead" class="righthead">
                        <%= Lang.Trans("Ajax Chat")%>
                    </div>
                </div>
            </div>
            <div id="content-wrap" style="display: none">
                <div id="content">
                    <div id="sidebar">
                        <div id="tabs" class="tabs">
                            <div id="divChatRoomTab" class="tab_on" onclick="OnTabClicked(0);">
                            </div>
                        </div>
                        <div class="clear">
                        </div>
                        <div id="messages">
                        </div>
                    </div>
                    <div id="main">
                        <div id="users" oncontextmenu='return false;'>
							<div class="usersonline_left"></div>
                            <div id="usersonline"></div>
							<div class="usersonline_right"></div>
                            <div id="usersright">
                            </div>
                        </div>
                        <div id="select_chatroom">
                        	<div align="center" >
                            <asp:DropDownList ID="ddChatRooms" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddChatRooms_SelectedIndexChanged">
                            </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="footer">
                    <div id="footer_fonts">
                        <!--Format Text Options-->
                        <img title='<%= Lang.Trans("Bold") %>' src='Images/text_bold.png' class="button"
                            onmouseover="document.getElementById('hidBoldButtonState').value = this.className; this.className == 'buttondown'?this.className = 'buttondown':this.className = 'buttonover'" 
                            onmouseout="this.className= document.getElementById('hidBoldButtonState').value" 
                            onclick="SetFontBold((document.getElementById('hidBoldButtonState').value=(document.getElementById('hidBoldButtonState').value=='button'?'buttondown':'button'))=='buttondown');document.getElementById('hidBoldButtonState').value == 'button'?this.className='buttonover':this.className='buttondown'" />
                        <img title='<%= Lang.Trans("Italic") %>' src='Images/text_italic.png' class="button"
                            onmouseover="document.getElementById('hidItalicButtonState').value = this.className; this.className == 'buttondown'?this.className = 'buttondown':this.className = 'buttonover'" 
                            onmouseout="this.className= document.getElementById('hidItalicButtonState').value" 
                            onclick="SetFontItalic((document.getElementById('hidItalicButtonState').value=(document.getElementById('hidItalicButtonState').value=='button'?'buttondown':'button'))=='buttondown');document.getElementById('hidItalicButtonState').value == 'button'?this.className='buttonover':this.className='buttondown'" />
                        <img title='<%= Lang.Trans("Underline") %>' src='Images/text_underline.png' class="button"
                            onmouseover="document.getElementById('hidUnderlineButtonState').value = this.className; this.className == 'buttondown'?this.className = 'buttondown':this.className = 'buttonover'" 
                            onmouseout="this.className= document.getElementById('hidUnderlineButtonState').value"                            
                            onclick="SetFontUnderline((document.getElementById('hidUnderlineButtonState').value=(document.getElementById('hidUnderlineButtonState').value=='button'?'buttondown':'button'))=='buttondown');document.getElementById('hidUnderlineButtonState').value == 'button'?this.className='buttonover':this.className='buttondown'" />
                            
                        <input id="hidBoldButtonState" name="hidBoldButtonState" type="hidden" />
                        <input id="hidItalicButtonState" name="hidItalicButtonState" type="hidden" />
                        <input id="hidUnderlineButtonState" name="hidUnderlineButtonState" type="hidden" />
                        
                        <a id='pick' name='pick' onclick="cp.show('pick');return false;">
                            <img id="colorIcon" title="Font Color" src='Images/colorpick.gif' style="background-color: black"
                                class="button" onmouseover="this.className='buttonover'" onmouseout="this.className='button'" /></a>
                        <select id="font" style="vertical-align: middle;">
                            <option value=""><%= Lang.Trans("Font") %></option>
                            <option value="Arial">Arial</option>
                            <option value="Verdana">Verdana</option>
                            <option value="Wingdings">Wingdings</option>
                            <option value="Courier">Courier</option>
                            <option value="Impact">Impact</option>
                            <option value="Georgia">Georgia</option>
                            <option value="Comic Sans MS">Comic Sans MS</option>
                        </select>
                        <select id="fontSize" style="vertical-align: middle;">
                            <option value=""><%= Lang.Trans("Size") %></option>
                            <option value="7">7</option>
                            <option value="8">8</option>
                            <option value="9">9</option>
                            <option value="10">10</option>
                            <option value="11">11</option>
                            <option value="12">12</option>
                            <option value="14">14</option>
                            <option value="15">15</option>
                            <option value="16">16</option>
                            <option value="18">18</option>
                            <option value="20">20</option>
                            <option value="22">22</option>
                            <option value="28">28</option>
                            <option value="32">32</option>
                        </select>
                        <a id="emo" name="emo" onclick="if(emoticonsPanel)emoticonsPanel.show('emo');return false;">
                            <img title='<%= Lang.Trans("Insert Emotions") %>' src='Images/emoticon_smile.png' class="button" onmouseover="this.className='buttonover'"
                                onmouseout="this.className='button'" /></a>
                        &nbsp;&nbsp;&nbsp;
                        <img title='<%= Lang.Trans("Alert on new messages") %>' src='Images/bell.png' class="button"
                            onmouseover="document.getElementById('hidAlertButtonState').value = this.className; this.className == 'buttondown'?this.className = 'buttondown':this.className = 'buttonover'" 
                            onmouseout="this.className= document.getElementById('hidAlertButtonState').value"                            
                            onclick="SetAlert((document.getElementById('hidAlertButtonState').value=(document.getElementById('hidAlertButtonState').value=='button'?'buttondown':'button'))=='buttondown');document.getElementById('hidAlertButtonState').value == 'button'?this.className='buttonover':this.className='buttondown'" />
                        <input id="hidAlertButtonState" name="hidAlertButtonState" type="hidden" />
                        &nbsp;&nbsp;&nbsp;
                        <img id="iconWebcam" title='<%= Lang.Trans("Send Audio/Video") %>' src="Images/webcam.png" class="button"
                            onmouseover="this.className='buttonover'"
                            onmouseout="this.className='button'" 
                            onclick="broadcastVideo(true)"
                            style="display: none;" />
                        <img id="iconSendFile" title='<%= Lang.Trans("Send File") %>' src="Images/folder_go.png" class="button"
                            onmouseover="this.className='buttonover'"
                            onmouseout="this.className='button'" 
                            onclick="window.open('SendFile.aspx?to=' + currentlyActiveTab,'sendFile','location=0,status=1,scrollbars=0,width=400,height=200');"
                            style="display: none;" />
                    </div>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td id="sendtxt_td">
                                <div id="sendmsg_txt">
                                    <input id="txtSendMessage" maxlength="200" type="text" value="" onkeydown="return KeyHandler(event)" />
                                </div>
                            </td>
                            <td width="90">
                                <div id="sendmsg_btn">
                                    <div align="center">
                                        <input id="btnSendMessage" type="button" value="" onclick="SendMessage()" />
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <div id="webcamdetector"></div>  
    </form>      
</body>
</html>
