<%@ Control Language="c#" AutoEventWireup="True" Codebehind="ViewVideo.ascx.cs" Inherits="ezFixUp.Components.Profile.ViewVideo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<input type="hidden" id="hidUsername" runat="server" NAME="hidUsername">
<div class="note">

</div>
<uc1:VideoRecorder id="VideoRecorder1" runat="server"></uc1:VideoRecorder>
