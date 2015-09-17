<%@ Control Language="c#" AutoEventWireup="True" Codebehind="VideoRecorder.ascx.cs" Inherits="ezFixUp.Components.Profile.VideoRecorder" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%if (bRender) {%>
<div class="center">
<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" width="220" height="184"
		codebase="https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab">
		<param name="movie" value="<%= RootPath %>VR/vPlayer.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&username=<%= strTargetUsername%>&mode=auto&" /><param name="quality" value="high" /><param name="bgcolor" value="#CCCCCC" />
		<embed src="<%= RootPath %>VR/vPlayer.swf?sessID=<%= strSessID %>&myName=<%= strUserID %>&username=<%= strTargetUsername%>&mode=auto&" quality="high" bgcolor="#CCCCCC" 
		width="220" height="184" name="detectiontest" align="middle"
		play="true"
		loop="false"
		quality="high"
		allowScriptAccess="sameDomain"
		type="application/x-shockwave-flash"
		pluginspage="https://www.macromedia.com/go/getflashplayer">
		</embed>
</object>	
</div>
<%}%>