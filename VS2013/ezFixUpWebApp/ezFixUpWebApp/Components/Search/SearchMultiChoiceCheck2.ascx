<%@ Control Language="c#" AutoEventWireup="True" Codebehind="SearchMultiChoiceCheck2.ascx.cs" Inherits="ezFixUp.Components.Search.SearchMultiChoiceCheck2" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" enableviewstate="false" runat="server">
    <div class="expander">
        <a href="#" onclick="return false;" class="plus1">
            <asp:Literal ID="ltrName" Runat="server" />
        </a>
    </div>
    <div id="divExpandee" class="expandee" style="display: none;padding-left:15px" runat="server">
		<asp:CheckBoxList id="cbValues" runat="server" Width="100%"></asp:CheckBoxList>
    </div>
    <input type="hidden" id="hidQuestionId" runat="server" NAME="hidQuestionId">
</div>