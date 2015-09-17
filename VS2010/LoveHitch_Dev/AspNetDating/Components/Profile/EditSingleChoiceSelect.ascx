<%@ Control Language="c#" AutoEventWireup="True" Codebehind="EditSingleChoiceSelect.ascx.cs" Inherits="AspNetDating.Components.Profile.EditSingleChoiceSelect" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<div id="pnlID" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" class="searchtable">
	<tr>
		<td width="30%" style="white-space: nowrap;width:30%;"><asp:Label id="lblName" runat="server"  Font-Bold="false"/>:&nbsp;</td>
		<td width="58%"><asp:DropDownList id="dropValue" CssClass="l_form_ddl" runat="server"></asp:DropDownList></td>
	</tr>
    </table>
    <input type="hidden" id="hidQuestionId" runat="server" name="hidQuestionId" />
</div>
