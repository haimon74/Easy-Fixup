<%@ Control Language="c#" AutoEventWireup="True" Codebehind="HeaderLine.ascx.cs" Inherits="ezFixUp.Components.HeaderLine" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div class="labelHdr">
    <asp:Label ID="lblTitle" Runat="server" CssClass="fl"/> 
    <asp:LinkButton ID="lbEdit" runat="server" CssClass="small zurbtn red fr"
        Visible="false" OnClick="LinkButtonClicked"></asp:LinkButton>
</div>
