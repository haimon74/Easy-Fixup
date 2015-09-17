<%@ Control Language="c#" AutoEventWireup="True" Codebehind="RecordVideo.ascx.cs" Inherits="AspNetDating.Components.Profile.RecordVideo" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register TagPrefix="uc1" TagName="VideoRecorder" Src="../../VR/VideoRecorder.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Import Namespace="AspNetDating.Classes" %>
<input type="hidden" id="hidUsername" runat="server" NAME="hidUsername" />
<uc1:HeaderLine id="HeaderLine1" runat="server" />
<div class="note">
	<%= Lang.Trans("Use this section to record a short video that will be a part of your profile.") %>
</div>
<div class="center">
	<uc1:VideoRecorder id="VideoRecorder1" runat="server"></uc1:VideoRecorder>
    <asp:UpdatePanel ID="UpdatePanelGridMessages" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lnkSetPublic" runat="server" OnClick="lnkSetPublic_Click"></asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="lnkSetPrivate" runat="server" OnClick="lnkSetPrivate_Click"></asp:LinkButton><br />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblError" runat="server" CssClass="error" EnableViewState="false"></asp:Label>	
</div>