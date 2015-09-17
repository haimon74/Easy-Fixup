<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MessengerWindow.aspx.cs"
    Inherits="Estream.AjaxChat.MessengerWindow" %>

<%@ Register Assembly="AjaxChat" Namespace="Estream.AjaxChat" TagPrefix="cc1" %>
<%@ Import Namespace="Estream.AjaxChat.Classes" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ajax Messenger</title>
    <link rel="stylesheet" type="text/css" href="Images/style.css" media="all" />  
</head>
<body onload="InitializeMessenger(<%= ChatRoom.MESSENGER_ROOM_ID %>, '<%= TargetUserId %>', '<%= IsInitiator %>'); SetMessengerHeight();"
    onunload="CloseChat()" onresize="SetMessengerHeight();" onkeypress="__trapESC(event)"
    scroll="no">
    <form id="form1" runat="server">
        <bgsound id="sound" />
        <span id="spanSound"></span>
        <div id="emoticonsPanelDiv" style="position: absolute; z-index: 888888; visibility: hidden;">
        </div>
        <input id="hidSelectedAvatar" type="hidden" />
        <asp:ScriptManager ID="scriptManager" runat="server">
            <Services>
                <asp:ServiceReference Path="ChatEngine.asmx" InlineScript="true" />
            </Services>
        </asp:ScriptManager>
        <div id="am_wrap">
            <div id="am_header">
                <div class="am_logo">
                    <img src="images/am_left_logo.png" onload="fixPng(this);"/></div>
                <div class="am_header_right">
                </div>
            </div>
            <div id="am_content-wrap_left">
                <div class="trays">
                </div>
                <div id="messages" class="am_content_text">
                </div>
            </div>
            <div id="am_content-wrap_right">
                <div id="divTargetUserVideo" class="video" style="background-image: url(<%= TargetUserPhoto %>); background-repeat: no-repeat; background-position: center center">
                </div>
                <div>               
                    <%-- User profile information might go here --%>
                </div>
            </div>
            <div id="am_footer_left">
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
                        <option value="">
                            <%= Lang.Trans("Font") %>
                        </option>
                        <option value="Arial">Arial</option>
                        <option value="Verdana">Verdana</option>
                        <option value="Wingdings">Wingdings</option>
                        <option value="Courier">Courier</option>
                        <option value="Impact">Impact</option>
                        <option value="Georgia">Georgia</option>
                        <option value="Comic Sans MS">Comic Sans MS</option>
                    </select>
                    <select id="fontSize" style="vertical-align: middle;">
                        <option value="">
                            <%= Lang.Trans("Size") %>
                        </option>
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
                        <img title='<%= Lang.Trans("Insert Emotions") %>' src='Images/emoticon_smile.png'
                            class="button" onmouseover="this.className='buttonover'" onmouseout="this.className='button'" /></a>
                    &nbsp;&nbsp;&nbsp;
                    <img title='<%= Lang.Trans("Alert on new messages") %>' src='Images/bell.png' class="button"
                        onmouseover="document.getElementById('hidAlertButtonState').value = this.className; this.className == 'buttondown'?this.className = 'buttondown':this.className = 'buttonover'"
                        onmouseout="this.className= document.getElementById('hidAlertButtonState').value"
                        onclick="SetAlert((document.getElementById('hidAlertButtonState').value=(document.getElementById('hidAlertButtonState').value=='button'?'buttondown':'button'))=='buttondown');document.getElementById('hidAlertButtonState').value == 'button'?this.className='buttonover':this.className='buttondown'" />
                    <input id="hidAlertButtonState" name="hidAlertButtonState" type="hidden" />
                    &nbsp;&nbsp;&nbsp;
                    <img id="iconWebcam" title='<%= Lang.Trans("Send Audio/Video") %>' src="Images/webcam.png"
                        class="button" 
                        onmouseover="document.getElementById('hidWebcamState').value = this.className; this.className == 'buttondown'?this.className = 'buttondown':this.className = 'buttonover'"
                        onmouseout="this.className= document.getElementById('hidWebcamState').value"
                        onclick="broadcastVideo((document.getElementById('hidWebcamState').value=(document.getElementById('hidWebcamState').value=='button'?'buttondown':'button'))=='buttondown');document.getElementById('hidWebcamState').value == 'button'?this.className='buttonover':this.className='buttondown'"
                        style="display: none;" />
                    <input id="hidWebcamState" name="hidWebcamState" type="hidden" />
                    <img id="iconSendFile" title='<%= Lang.Trans("Send File") %>' src="Images/folder_go.png"
                        class="button" onmouseover="this.className='buttonover'" onmouseout="this.className='button'"
                        onclick="window.open('SendFile.aspx?to=<%= TargetUserId %>&chatRoomId=<%= ChatRoom.MESSENGER_ROOM_ID %>','sendFile','location=0,status=1,scrollbars=0,width=400,height=200');"
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
            <div id="am_footer_right">
                <div id="divCurrentUserVideo" class="video" style="background-image: url(<%= CurrentUserPhoto %>); background-repeat: no-repeat; background-position: center center">
                </div>
            </div>
        </div>
        <div id="webcamdetector">
        </div>
    </form>
</body>
</html>
