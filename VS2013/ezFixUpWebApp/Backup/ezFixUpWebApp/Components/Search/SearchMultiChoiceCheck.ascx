<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchMultiChoiceCheck.ascx.cs" Inherits="ezFixUp.Components.Search.SearchMultiChoiceCheck" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <div class="LargeBoxWrap BoxWrapStyle">
	<b><asp:Label ID="lblName" Runat="server" /></b>
		<asp:CheckBoxList id="cbValues" runat="server" Width="100%" RepeatColumns="3"></asp:CheckBoxList>
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>