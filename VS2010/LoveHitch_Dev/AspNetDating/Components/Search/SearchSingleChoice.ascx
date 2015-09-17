<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchSingleChoice.ascx.cs" Inherits="AspNetDating.Components.Search.SearchSingleChoice" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <div class="LargeBoxWrap BoxWrapStyle">
        <b><asp:Label ID="lblName" Runat="server" /></b>
        <asp:DropDownList id="dropValues" CssClass="dropdownlist" runat="server"></asp:DropDownList>
    </div>
    <INPUT id="hidQuestionId" type="hidden" runat="server">
</div>