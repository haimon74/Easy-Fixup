<%@ Import namespace="AspNetDating.Classes"%>
<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchRangeChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Search.SearchRangeChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <div class="LargeBoxWrap BoxWrapStyle">
		<b><asp:Label ID="lblName" Runat="server" /></b>
		<%= Lang.Trans("from") %><asp:DropDownList ID="ddFrom" Runat="server" />
		<%= Lang.Trans("to") %><asp:DropDownList ID="ddTo" Runat="server" />
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>
