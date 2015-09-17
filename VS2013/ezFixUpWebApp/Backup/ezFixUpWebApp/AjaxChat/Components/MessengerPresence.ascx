<%@ Control Language="C#" AutoEventWireup="true" Codebehind="MessengerPresence.ascx.cs"
    Inherits="Estream.AjaxChat.Components.MessengerPresence" %>
<div id="divNotification">
    <img id="imgAvatar" />
    <span id="spanMessage"></span>
    <div id="divViewProfile">
        [&nbsp;<a id="lnkViewProfile" href="#" target="_blank">view user profile</a>&nbsp;]
    </div>
    <div style="clear: both"></div>
    <input id="btnAccept" type="button" value="Accept" onclick="AcceptChat()" />
    <input id="btnReject" type="button" value="Reject" onclick="RejectChat()" />
</div>
