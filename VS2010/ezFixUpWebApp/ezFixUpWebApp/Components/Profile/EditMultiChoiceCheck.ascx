<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditMultiChoiceCheck.ascx.cs" Inherits="ezFixUp.Components.Profile.EditMultiChoiceCheck" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>

<style type="text/css">
    table.cb30 tr td{width:30%;}
</style>

<div id="pnlID" runat="server">
    <div id="tdUser" runat="server">
	<uc1:HeaderLine ID="hlName" runat="server" />
    </div>
    <div id="tdAdmin" runat="server">
	<h3 class="SectionHeading">
		<%= hlName.Title %>
	</h3>
    </div>
    <div id="pnlDescription" class="note" Runat="server" style="DISPLAY: none">
	<asp:Label ID="lblDescription" Runat="server" />
    </div>
    <div id="pnlHint" class="hint" Runat="server" style="DISPLAY: none">
	<asp:Label ID="lblHint" Runat="server" />
    </div>
    <asp:checkboxlist id="cbValues" SkinID="Interests" runat="server" 
                CellSpacing="1" CssClass="fl font cb30" Width="100%"
                RepeatLayout="Table" RepeatColumns="3" RepeatDirection="Horizontal"></asp:checkboxlist>
    <input id="hidQuestionId" type="hidden" name="hidQuestionId" runat="server"/>
    <div class="separator"></div>
</div>

