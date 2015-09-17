<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditSingleChoiceRadio.ascx.cs" Inherits="ezFixUp.Components.Profile.EditSingleChoiceRadio" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
	<%@ Register src="../HeaderLine.ascx" tagname="HeaderLine" tagprefix="uc1" %>
<div id="pnlID" runat="server">
	<div ID="pnlUser" Runat="server">
		<uc1:HeaderLine ID="hlName" runat="server" />
	</div>
	<div ID="pnlAdmin" Runat="server">
		<h3 class="SectionHeading"><%= hlName.Title %></h3>
	</div>
	<div class="note">
		<asp:Label ID="lblDescription" Runat="server" />
	</div>
	<div class="hint">
		<asp:Label ID="lblHint" Runat="server" />
	</div>
		<asp:radiobuttonlist RepeatColumns="3" CssClass="font" id="rbValues" Width="100%" runat="server"></asp:radiobuttonlist>
    <INPUT id="hidQuestionId" type="hidden" runat="server">
</div>
