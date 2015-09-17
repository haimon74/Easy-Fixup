<%@ Import Namespace="AspNetDating.Classes" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxEnd" Src="../LargeBoxEnd.ascx" %>
<%@ Register TagPrefix="uc1" TagName="LargeBoxStart" Src="../LargeBoxStart.ascx" %>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditSkin.ascx.cs"
    Inherits="AspNetDating.Components.Profile.EditSkin" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" %>
<%@ Register TagPrefix="uc1" TagName="HeaderLine" Src="../HeaderLine.ascx" %>
<%@ Register TagPrefix="components" Namespace="AspNetDating.Components" Assembly="AspNetDating" %>

<uc1:LargeBoxStart id="LargeBoxStart1" runat="server" />
<input type="hidden" id="hidUsername" runat="server" />
<asp:Label ID="lblError" CssClass="error" runat="server" />
<asp:PlaceHolder ID="plhEditSkinControls" runat="server" />
<div class="center">
    <asp:Button ID="btnSave" OnClick="btnSave_Click" runat="server" />
    <div id="divPreviewSkin" visible="false" runat="server">[
    <a href="<%= UrlRewrite.CreateShowUserUrl(User.Username) %>" target="_new"><%= "Preview skin".Translate() %></a>
    ]</div>
</div>
<uc1:LargeBoxEnd id="LargeBoxEnd1" runat="server" />